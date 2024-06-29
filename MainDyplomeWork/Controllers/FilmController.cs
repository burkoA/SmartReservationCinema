using SmartReservationCinema.FilmContext;
using SmartReservationCinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SmartReservationCinema.Services;

namespace SmartReservationCinema.Controllers
{
    public class FilmController : Controller
    {
        private readonly FilmDbContext _db;
        private readonly int itemsOnPage = 5;
        private readonly IWebHostEnvironment _env;
        private string[] wordsToSearch = null;
        // GET: FilmController
        public FilmController(FilmDbContext context, IWebHostEnvironment env)
        {
            _db = context;
            _env = env;
        }
        public ActionResult Index(int? id, int curPage=0,[FromQuery] String search="")
        {
            ViewBag.GenresList = _db.Genres.OrderBy(g => g.GenreName).ToList();
            ViewBag.CurrentUser = AccountController.GetCurrentUser(_db, HttpContext);
            IEnumerable<Film> items;

            if (id.HasValue && id.Value > 0)
            {
                IEnumerable<Genre_Film> tmp = _db.GenresFilmes.Where(gf => gf.Id_Genre == id.Value);

                items = _db.Films.Include(f=> f.FavouriteFilms).Include(item => item.Marks).Include(F => F.Genres).ThenInclude((Genre_Film gf) => gf.genre)
                   .Where(f => f.Genres.Contains(tmp.FirstOrDefault(gf => f.Id == gf.Id_Film)));
                //.ThenInclude((Genre_Film gf) => gf.genre)
            }
            else items = _db.Films.Include(item => item.Marks).Include(f=>f.FavouriteFilms).Include(F => F.Genres).ThenInclude((Genre_Film gf) => gf.genre);
            if (!string.IsNullOrEmpty(search))
            {
                var wordsToSearch = SplitSearch(search);
                items = items.Where(c => wordsToSearch.Any(word => c.FilmName.ToLower().Contains(word.ToLower())));
            }


            int itemCnt = items.Count();

            items = items.Skip(itemsOnPage * curPage).Take(itemsOnPage);

            int pageCnt = (int)Math.Ceiling(itemCnt / (double)itemsOnPage);
            ViewBag.pageCnt = pageCnt;
            ViewBag.curGenge = id??0;
            ViewBag.search = search;
            return View(items.OrderBy(f => f.FilmName));
        }

        private IEnumerable<string> SplitSearch(string search)
        {
            return search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        // GET: FilmController/Details/5
        public ActionResult Details(int id, int TownIdFilter = 0, DateTime? dateFilter = null, [FromForm] CommentModel? comment=null)
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
            if (dateFilter == null)
            {
                dateFilter = DateTime.Today;
            }
            ViewBag.dateFilter = dateFilter;
            ViewBag.SelectedTownId = TownIdFilter;
            ViewBag.Towns = _db.Towns.OrderBy(t => t.TownName).ToList();
            ViewBag.UserName = AccountController.GetCurrentUser(_db, HttpContext)?.FirstName;
            ViewBag.CommentModel = new CommentModel() { IdFilm=id};
            
            Film film = _db.Films
                .Include(f => f.Genres).ThenInclude(fg => fg.genre)
                .Include(f => f.Director)
                .Include(f => f.Actors).ThenInclude(fa=>fa.actor)
                .Include(f => f.Subtitles).ThenInclude(sf => sf.Language)
                .Include(f => f.Comments).ThenInclude(c => c.User)
                .FirstOrDefault(f => f.Id == id);

            
            var markByFilm = _db.FilmMarks.Where(filmMark => filmMark.FilmId == id);
            double rate;
            if (markByFilm.Count() > 0)
            {
                rate = markByFilm.Average(filmMark => filmMark.Mark);
            }
            else rate = 0;
            

            ViewBag.Rate = rate;

            if (TownIdFilter > 0 && dateFilter != null)
            {
                ViewBag.Cinemas = SortByDistance(_db.Cinemas.Include(c => c.Sessions)
                  .Where(c => c.Sessions.Any(s => s.FilmId == id &&
                                  s.StartDate <= dateFilter && s.EndDate >= dateFilter &&
                                  s.cinema.TownId == TownIdFilter
                                  )).ToList());
            }
            else ViewBag.Cinemas = new List<CinemaDistanceModel>();
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
                    DistanceFinder distanceFinder = new DistanceFinder("AIzaSyBr1VMywYUjUG0zXgpBz0cIHCnBQKRphbQ");
                    int[] distances = distanceFinder.GetDistance(userAddress, cinemaAddress.ToArray());
                    IList<CinemaDistanceModel> result = new List<CinemaDistanceModel>();
                    for (int i = 0; i < cinemas.Count; i++)
                    {
                        result.Add(new CinemaDistanceModel(cinemas[i], distances[i]));
                    }
                    return result.OrderBy(r => r.Distance).ToList();
                }
                return cinemas.Select(s => new CinemaDistanceModel(s, 0)).ToList();
            } catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while sorting cinemas by distance. Wrong town, address or no sessions available";
                return cinemas.Select(s => new CinemaDistanceModel(s, 0)).ToList();
            }
        }

        //public User? GetCurrent()
        //{
        //    return _db.Users.Where(u => u.Email == HttpContext.User.Identity.Name).FirstOrDefault();
        //}

        // GET: FilmController/Create
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create()
        {
            GenerateList();
            return View(new FilmModel());
        }

        private void GenerateList()
        {
            ViewBag.Genres = _db.Genres.OrderBy(g=> g.GenreName).ToList();
            ViewBag.Actors = _db.Actors.OrderBy(a=> a.Actor_Name).ToList();
            ViewBag.Directors = new SelectList(_db.Director.OrderBy(d => d.Name_Director), "Id_Director", "Name_Director");
        }

        // POST: FilmController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create(FilmModel film, int[] genres, int[] actors)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
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
                        Id_Genre = genreId,
                        Id_Film = film.Id
                    });
                }
                foreach(int actorsId in actors)
                {
                    _db.FilmActor.Add(new Film_Actor()
                    {
                        Id_Actor = actorsId,
                        Id_Film = film.Id
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

        // GET: FilmController/Edit/5
        [Authorize(Roles ="admin,manager")]
        public ActionResult Edit(int id)
        {
            GenerateList();
            Film film = _db.Films.Include(f => f.Genres).Where(film => film.Id == id).Include(f => f.Actors).FirstOrDefault();
            return View(new FilmModel(film));
        }

        // POST: FilmController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id, FilmModel film, int[] genres, int[] actors)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }

                if (film.NewImage != "")
                {
                    string imgFolder = _env.WebRootPath + "/img/filmsImage/";
                    if(! String.IsNullOrEmpty(film.Image))
                    { 
                        System.IO.File.Delete(imgFolder+film.Image);
                    }
                    film.Image = film.NewImage;
                }

                _db.Films.Update(film);
                IEnumerable<Film_Actor> oldActor = _db.FilmActor.Where(fa => fa.Id_Film == film.Id).ToList();
                IEnumerable<Genre_Film> oldGenres = _db.GenresFilmes.Where(gf=>gf.Id_Film==film.Id).ToList();
                foreach (int genreId in genres)
                {
                    if (!oldGenres.Any(fg => fg.Id_Genre == genreId)){ 
                        _db.GenresFilmes.Add(new Genre_Film()
                        {
                            Id_Genre = genreId,
                            Id_Film = film.Id
                        });
                    }
                }
                foreach(Genre_Film gengeFilm in oldGenres)
                {
                    if (!genres.Contains(gengeFilm.Id_Genre))
                    {
                        _db.GenresFilmes.Remove(gengeFilm);
                    }
                }
                foreach(int actorId in actors)
                {
                    if(!oldActor.Any(fa => fa.Id_Actor == actorId))
                    {
                        _db.FilmActor.Add(new Film_Actor()
                        {
                            Id_Actor = actorId,
                            Id_Film = film.Id
                        });
                    }
                }
                foreach(Film_Actor actorFilm in oldActor)
                {
                    if (!actors.Contains(actorFilm.Id_Actor))
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

        // GET: FilmController/Delete/5
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete(int id)
        {
            Film film = _db.Films.Where(film => film.Id == id).FirstOrDefault();
            return View(film);
        }

        // POST: FilmController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete([FromForm] int id, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid) { 
                    throw new Exception(""); 
                }

                Film film = _db.Films.Where(film => film.Id == id).FirstOrDefault();

                if(film==null)
                {
                    return View("Error","Id not found");
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
