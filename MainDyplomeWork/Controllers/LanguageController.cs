using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.Entity;
using SmartReservationCinema.FilmContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class LanguageController : Controller
    {
        private readonly FilmDbContext _context;
        public LanguageController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Index(string search)
        {
            var language = from a in _context.Languages.OrderBy(l => l.LanguageName) select a;

            if (!String.IsNullOrEmpty(search))
            {
                language = language.Where(l => l.LanguageName.Contains(search));
            }

            return View(await language.ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Languages
                .FirstOrDefaultAsync(m => m.Id == id);

            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Create(Language language)
        {
            if (ModelState.IsValid)
            {
                _context.Add(language);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(language);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Languages.FindAsync(id);

            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int id, Language language)
        {
            if (id != language.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(language);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(language.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(language);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _context.Languages
                .FirstOrDefaultAsync(m => m.Id == id);

            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var language = await _context.Languages.FindAsync(id);

            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool LanguageExists(int id)
        {
            return _context.Languages.Any(e => e.Id == id);
        }
    }
}
