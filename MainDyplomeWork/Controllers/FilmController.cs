using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.Entity;
using SmartReservationCinema.FilmContext;
using SmartReservationCinema.Models;
using SmartReservationCinema.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartReservationCinema.Controllers
{
    public class FilmController : Controller
    {
        private readonly FilmDbContext _db;
        private readonly IWebHostEnvironment _env;
        private const int _itemsOnPage = 5;

        public FilmController(FilmDbContext context, IWebHostEnvironment env)
        {
            _db = context;
            _env = env;
        }

        [HttpGet]
        public ActionResult Index(int? id, int curPage = 0, [FromQuery] String search = "")
        {
            ViewBag.GenresList = _db.Genres.OrderBy(g => g.GenreName).ToList();
            ViewBag.CurrentUser = AccountController.GetCurrentUser(_db, HttpContext);

            var filmsQuery = _db.Films
                .Include(f => f.FavouriteFilms)
                .Include(f => f.Marks)
                .Include(f => f.Genres).ThenInclude(gf => gf.Genre)
                .AsQueryable();

            if (id.HasValue && id > 0)
            {
                var genreFilms = _db.GenresFilmes.Where(gf => gf.GenreId == id.Value).Select(gf => gf.FilmId);
                filmsQuery = filmsQuery.Where(f => genreFilms.Contains(f.Id));
            }

            if (!string.IsNullOrEmpty(search))
            {
                var searchWords = SplitSearch(search);
                filmsQuery = filmsQuery.Where(f => searchWords.Any(word => f.FilmName.ToLower().Contains(word.ToLower())));
            }

            var totalItems = filmsQuery.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)_itemsOnPage);
            var films = filmsQuery
                .OrderBy(f => f.FilmName)
                .Skip(_itemsOnPage * curPage)
                .Take(_itemsOnPage)
                .ToList();

            ViewBag.PageCount = totalPages;
            ViewBag.CurrentGenre = id ?? 0;
            ViewBag.Search = search;

            return View(films);
        }

        private IEnumerable<string> SplitSearch(string search)
        {
            return search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        public ActionResult Details(int id, int townIdFilter = 0, DateTime? dateFilter = null, [FromForm] CommentModel? comment = null)
        {
            if (HttpContext.Request.Method.ToUpper() == "POST")
            {
                if (ModelState.IsValid)
                {
                    CommentController.AddComment(comment, _db, HttpContext);
                }
            }
            else
            {
                ModelState.Remove("Text");
            }

            dateFilter ??= DateTime.Today;

            var film = _db.Films
                .Include(f => f.Genres).ThenInclude(gf => gf.Genre)
                .Include(f => f.Director)
                .Include(f => f.Actors).ThenInclude(fa => fa.Actor)
                .Include(f => f.Subtitles).ThenInclude(sf => sf.Language)
                .Include(f => f.Comments).ThenInclude(c => c.User)
                .FirstOrDefault(f => f.Id == id);

            if (film == null)
            {
                return NotFound();
            }

            ViewBag.DateFilter = dateFilter;
            ViewBag.SelectedTownId = townIdFilter;
            ViewBag.Towns = _db.Towns.OrderBy(t => t.TownName).ToList();
            ViewBag.UserName = AccountController.GetCurrentUser(_db, HttpContext)?.FirstName;
            ViewBag.CommentModel = new CommentModel { IdFilm = id };
            ViewBag.Rate = _db.FilmMarks.Where(fm => fm.FilmId == id).Average(fm => (double?)fm.Mark) ?? 0;

            if (townIdFilter > 0)
            {
                var cinemas = _db.Cinemas
                    .Include(c => c.Sessions)
                    .Where(c => c.Sessions.Any(s => s.FilmId == id && s.StartDate <= dateFilter && s.EndDate >= dateFilter && s.Cinema.TownId == townIdFilter))
                    .ToList();

                ViewBag.Cinemas = SortByDistance(cinemas);
            }
            else
            {
                ViewBag.Cinemas = new List<CinemaDistanceModel>();
            }

            return View(film);
        }

        public IList<CinemaDistanceModel> SortByDistance(IList<Cinema> cinemas)
        {
            try
            {
                IList<string> cinemaAddress = new List<string>();

                foreach (var cinema in cinemas)
                {
                    cinemaAddress.Add(cinema.Localisation + " " + cinema.Town.TownName);
                }

                User? user = AccountController.GetCurrentUser(_db, HttpContext);
                if (user != null)
                {
                    string userAddress = user.Address + " " + user.City;
                    DistanceFinder distanceFinder = new DistanceFinder("");
                    int[] distances = distanceFinder.GetDistance(userAddress, cinemaAddress.ToArray());
                    IList<CinemaDistanceModel> result = new List<CinemaDistanceModel>();

                    for (int i = 0; i < cinemas.Count; i++)
                    {
                        result.Add(new CinemaDistanceModel(cinemas[i], distances[i]));
                    }

                    return result.OrderBy(r => r.Distance).ToList();
                }

                return cinemas.Select(s => new CinemaDistanceModel(s, 0)).ToList();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while sorting cinemas by distance. Wrong town, address or no sessions available";
                return cinemas.Select(s => new CinemaDistanceModel(s, 0)).ToList();
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create()
        {
            GenerateList();
            return View(new FilmModel());
        }

        private void GenerateList()
        {
            ViewBag.Genres = _db.Genres.OrderBy(g => g.GenreName).ToList();
            ViewBag.Actors = _db.Actors.OrderBy(a => a.Name).ToList();
            ViewBag.Directors = new SelectList(_db.Director.OrderBy(d => d.Name), "Id", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create(FilmModel film, int[] genres, int[] actors)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GenerateList();
                    return View(film);
                }

                if (film.NewImage != "")
                {
                    film.Image = film.NewImage;
                }
                else
                {
                    film.Image = "DefaultImage.jpg";
                }

                _db.Films.Add(film);
                _db.SaveChanges();

                foreach (int genreId in genres)
                {
                    _db.GenresFilmes.Add(new Genre_Film()
                    {
                        GenreId = genreId,
                        FilmId = film.Id
                    });
                }

                foreach (int actorsId in actors)
                {
                    _db.FilmActor.Add(new Film_Actor()
                    {
                        ActorId = actorsId,
                        FilmId = film.Id
                    });
                }

                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View(film);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id)
        {
            GenerateList();
            Film film = _db.Films.Include(f => f.Genres).Where(film => film.Id == id)
                .Include(f => f.Actors).FirstOrDefault();

            return View(new FilmModel(film));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id, FilmModel film, int[] genres, int[] actors)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }

                if (film.NewImage != "")
                {
                    string imgFolder = _env.WebRootPath + "/img/filmsImage/";
                    if (!String.IsNullOrEmpty(film.Image))
                    {
                        System.IO.File.Delete(imgFolder + film.Image);
                    }
                    film.Image = film.NewImage;
                }

                _db.Films.Update(film);

                IEnumerable<Film_Actor> oldActor = _db.FilmActor.Where(fa => fa.FilmId == film.Id).ToList();
                IEnumerable<Genre_Film> oldGenres = _db.GenresFilmes.Where(gf => gf.FilmId == film.Id).ToList();

                foreach (int genreId in genres)
                {
                    if (!oldGenres.Any(fg => fg.GenreId == genreId))
                    {
                        _db.GenresFilmes.Add(new Genre_Film()
                        {
                            GenreId = genreId,
                            FilmId = film.Id
                        });
                    }
                }

                foreach (Genre_Film gengeFilm in oldGenres)
                {
                    if (!genres.Contains(gengeFilm.GenreId))
                    {
                        _db.GenresFilmes.Remove(gengeFilm);
                    }
                }

                foreach (int actorId in actors)
                {
                    if (!oldActor.Any(fa => fa.ActorId == actorId))
                    {
                        _db.FilmActor.Add(new Film_Actor()
                        {
                            ActorId = actorId,
                            FilmId = film.Id
                        });
                    }
                }

                foreach (Film_Actor actorFilm in oldActor)
                {
                    if (!actors.Contains(actorFilm.ActorId))
                    {
                        _db.FilmActor.Remove(actorFilm);
                    }
                }

                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View(film);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete(int id)
        {
            Film film = _db.Films.Where(film => film.Id == id).FirstOrDefault();
            return View(film);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult DeleteConfirm([FromForm] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }

                Film film = _db.Films.Where(film => film.Id == id).FirstOrDefault();

                if (film == null)
                {
                    return View("Error", "Id not found");
                }

                _db.Films.Remove(film);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
