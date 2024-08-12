using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.Entity;
using SmartReservationCinema.FilmContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class SubtitleController : Controller
    {
        private readonly FilmDbContext _db;

        public SubtitleController(FilmDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Index(string search)
        {
            IQueryable<Subtitle> subtitles = _db.Subtitles.Include(s => s.Film).Include(s => s.Language);

            if (!String.IsNullOrEmpty(search))
            {
                subtitles = subtitles.Where(s => s.Film.FilmName.Contains(search));
            }

            subtitles = subtitles.OrderBy(s => s.Film.FilmName);

            var orderedSubtitles = await subtitles.ToListAsync();

            return View(orderedSubtitles);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtitle = await _db.Subtitles
                .Include(f => f.Film)
                .Include(l => l.Language)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (subtitle == null)
            {
                return NotFound();
            }

            GenerateList();
            return View(subtitle);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create()
        {
            GenerateList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create(Subtitle subtitle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }

                _db.Subtitles.Add(subtitle);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View(subtitle);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id)
        {
            GenerateList();

            Subtitle subtitle = _db.Subtitles
                .Include(s => s.Language).Where(subtitle => subtitle.Id == id)
                .Include(s => s.Film).FirstOrDefault();

            return View(subtitle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id, Subtitle subtitle)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }

                _db.Subtitles.Update(subtitle);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtitle = await _db.Subtitles
                .Include(f => f.Film)
                .Include(l => l.Language)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (subtitle == null)
            {
                return NotFound();
            }

            return View(subtitle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }

                Subtitle subtitle = _db.Subtitles.Where(s => s.Id == id).FirstOrDefault();

                if (subtitle == null)
                {
                    return View("Error", "Id not found");
                }

                _db.Subtitles.Remove(subtitle);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View();
            }
        }

        private void GenerateList()
        {
            ViewBag.Languages = new SelectList(_db.Languages.OrderBy(l => l.LanguageName), "Id", "LanguageName");
            ViewBag.Films = new SelectList(_db.Films.OrderBy(f => f.FilmName), "Id", "FilmName");
        }
    }
}
