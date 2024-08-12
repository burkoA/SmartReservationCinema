using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.Entity;
using SmartReservationCinema.FilmContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class TownController : Controller
    {

        private readonly FilmDbContext _context;

        public TownController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Index([FromQuery] String search = "")
        {
            IEnumerable<Town> items = _context.Towns;

            if (!string.IsNullOrEmpty(search))
            {
                var wordsToSearch = SplitSearch(search);
                items = items.Where(t =>
                {
                    foreach (string word in wordsToSearch)
                    {
                        if (t.TownName.ToLower().Contains(word.ToLower()))
                            return true;
                    }
                    return false;
                });
            }

            return View(items.OrderBy(t => t.TownName).ToList());
        }

        private IEnumerable<string> SplitSearch(string search)
        {
            return search.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var town = await _context.Towns
                .FirstOrDefaultAsync(m => m.Id == id);

            if (town == null)
            {
                return NotFound();
            }

            return View(town);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Create(Town town)
        {
            if (ModelState.IsValid)
            {
                _context.Add(town);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(town);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var town = await _context.Towns.FindAsync(id);

            if (town == null)
            {
                return NotFound();
            }

            return View(town);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int id, Town town)
        {
            if (id != town.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(town);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TownExists(town.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(town);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var town = await _context.Towns
                .FirstOrDefaultAsync(m => m.Id == id);

            if (town == null)
            {
                return NotFound();
            }

            return View(town);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var town = await _context.Towns.FindAsync(id);

            _context.Towns.Remove(town);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TownExists(int id)
        {
            return _context.Towns.Any(e => e.Id == id);
        }
    }
}
