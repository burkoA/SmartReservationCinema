using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;
using SmartReservationCinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartReservationCinema.Controllers
{
    public class CinemaController : Controller
    {
        private readonly FilmDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly int _itemsPerPage = 5;

        public CinemaController(FilmDbContext context, IWebHostEnvironment env)
        {
            _db = context;
            _env = env;
        }

        [HttpGet]
        public ActionResult Index(int? id, int curPage = 0, [FromQuery] String search = "")
        {
            ViewBag.TownsList = _db.Towns.OrderBy(t => t.TownName).ToList();
            var cinemas = GetCinemas(id, search);

            int totalItems = cinemas.Count();
            cinemas = PaginateCinemas(cinemas, curPage);

            ViewBag.PageCount = (int)Math.Ceiling(totalItems / (double)_itemsPerPage);
            ViewBag.CurrentTownId = id ?? 0;

            return View(cinemas);
        }

        private IEnumerable<Cinema> GetCinemas(int? townId, string search)
        {
            var cinemas = _db.Cinemas.Include(c => c.Town).AsQueryable();

            if (townId.HasValue && townId.Value > 0)
            {
                cinemas = cinemas.Where(c => c.TownId == townId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                var searchWords = SplitSearchWords(search);
                cinemas = cinemas.Where(c => searchWords.Any(word => c.CinemaName.Contains(word, StringComparison.OrdinalIgnoreCase)));
            }

            return cinemas;
        }

        private IEnumerable<Cinema> PaginateCinemas(IEnumerable<Cinema> cinemas, int currentPage)
        {
            return cinemas.Skip(_itemsPerPage * currentPage).Take(_itemsPerPage).ToList();
        }

        private IEnumerable<string> SplitSearchWords(string search)
        {
            return search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }


        [HttpGet]
        public ActionResult Details(int id, DateTime? filterDate = null)
        {
            if (filterDate == null)
            {
                filterDate = DateTime.Today;
            }

            ViewBag.filterDate = filterDate;
            Cinema cinema = _db.Cinemas.Include(t => t.Town).FirstOrDefault(c => c.Id == id);
            ViewBag.Sessions = GetCinemaSessions(id, filterDate.Value);

            return View(cinema);
        }

        private IEnumerable<Session> GetCinemaSessions(int cinemaId, DateTime filterDate)
        {
            return _db.Sessions
                .Where(s => s.CinemaId == cinemaId && s.StartDate <= filterDate && s.EndDate >= filterDate)
                .Include(s => s.Film)
                .Include(s => s.Language)
                .Include(s => s.Hall)
                .Include(s => s.TicketPrices)
                .ToArray();
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create()
        {
            GenerateList();
            return View(new CinemaModel());
        }

        private void GenerateList()
        {
            ViewBag.Towns = new SelectList(_db.Towns.OrderBy(t => t.TownName), "Id", "TownName");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create(CinemaModel cinema)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GenerateList();
                    return View(cinema);
                }

                if (cinema.NewImage != "")
                {
                    cinema.Image = cinema.NewImage;
                }
                else
                {
                    cinema.Image = "DefaultImage.jpg";
                }

                _db.Cinemas.Add(cinema);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View(cinema);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id)
        {
            GenerateList();
            Cinema cinema = _db.Cinemas.Include(c => c.Town).Where(cinema => cinema.Id == id).FirstOrDefault();
            return View(new CinemaModel(cinema));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id, CinemaModel cinema)
        {
            if (!ModelState.IsValid)
            {
                GenerateList();
                return View(cinema);
            }

            UpdateCinemaImage(cinema);
            _db.Cinemas.Update(cinema);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void UpdateCinemaImage(CinemaModel cinema)
        {
            if (!string.IsNullOrEmpty(cinema.NewImage))
            {
                var imgFolder = System.IO.Path.Combine(_env.WebRootPath, "img", "cinemaImage");

                if (!string.IsNullOrEmpty(cinema.Image))
                {
                    System.IO.File.Delete(System.IO.Path.Combine(imgFolder, cinema.Image));
                }

                cinema.Image = cinema.NewImage;
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete(int id)
        {
            Cinema cinema = _db.Cinemas.Where(c => c.Id == id).FirstOrDefault();
            return View(cinema);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }

                Cinema cinema = _db.Cinemas.Where(c => c.Id == id).FirstOrDefault();

                if (cinema == null)
                {
                    return View("Error", "Id not found");
                }

                _db.Cinemas.Remove(cinema);
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
