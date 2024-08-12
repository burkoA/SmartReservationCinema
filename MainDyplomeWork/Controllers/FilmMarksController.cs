using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.Entity;
using SmartReservationCinema.FilmContext;
using SmartReservationCinema.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class FilmMarksController : Controller
    {
        private readonly FilmDbContext _context;

        public FilmMarksController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Index()
        {
            var filmDbContext = _context.FilmMarks
                                        .Include(f => f.Film)
                                        .Include(f => f.User);

            return View(await filmDbContext.ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmMark = await _context.FilmMarks
                .Include(f => f.Film)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (filmMark == null)
            {
                return NotFound();
            }

            return View(filmMark);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create([FromQuery] int filmId)
        {
            return View("MarkPlace", new FilmMark() { FilmId = filmId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Mark,FilmId")] FilmMark filmMark)
        {
            User user = AccountController.GetCurrentUser(_context, HttpContext);

            if (user == null)
            {
                return View("Error", new ErrorViewModel() { RequestId = "User not authorize" });
            }

            filmMark.UserId = user.Id;
            ModelState.Remove("Id");
            ModelState.Remove("UserId");
            ModelState.Remove("MarkDate");

            if (ModelState.IsValid)
            {
                if (!CheckSpamMark(user, filmMark.FilmId))
                {
                    string customHtml = "<div style='color: red; font-weight: bold; font-size: 30px;'>" +
                                "Slow down!" +
                                "</div>";
                    return Content(customHtml, "text/html");
                }

                _context.Add(filmMark);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MarkSaved));
            }
            return View("MarkPlace", filmMark);
        }

        private bool CheckSpamMark(User user, int filmId)
        {
            int count = _context.FilmMarks.Where(fm => fm.FilmId == filmId && fm.UserId == user.Id).Count();

            if (count > 0)
            {
                return false;
            }

            count = _context.FilmMarks.Where(fm => fm.MarkDate > DateTime.Now.AddMinutes(-15) && fm.UserId == user.Id).Count();

            if (count > 3)
            {
                return false;
            }

            return true;
        }

        public IActionResult MarkSaved()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmMark = await _context.FilmMarks
                .Include(f => f.Film)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (filmMark == null)
            {
                return NotFound();
            }

            return View(filmMark);
        }

        [Authorize(Roles = "admin,manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filmMark = await _context.FilmMarks.FindAsync(id);

            _context.FilmMarks.Remove(filmMark);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool FilmMarkExists(int id)
        {
            return _context.FilmMarks.Any(e => e.Id == id);
        }
    }
}
