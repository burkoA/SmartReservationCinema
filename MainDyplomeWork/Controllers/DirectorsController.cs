using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly FilmDbContext _context;

        public DirectorsController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Index([FromQuery] String search = "")
        {
            IEnumerable<Director> items = _context.Director;

            if (!string.IsNullOrEmpty(search))
            {
                var wordsToSearch = SplitSearch(search);
                items = items.Where(d =>
                {
                    foreach (string word in wordsToSearch)
                    {
                        if (d.Name.ToLower().Contains(word.ToLower()))
                            return true;
                    }
                    return false;
                });
            }

            return View(items.OrderBy(d => d.Name));
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

            var director = await _context.Director.FirstOrDefaultAsync(m => m.Id == id);

            if (director == null)
            {
                return NotFound();
            }

            return View(director);
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
        public async Task<IActionResult> Create([Bind("Id,Name,BirthPlace,WorkExperience,MovieNumber")] Director director)
        {
            if (ModelState.IsValid)
            {
                _context.Add(director);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(director);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Director.FindAsync(id);

            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BirthPlace,WorkExperience,MovieNumber")] Director director)
        {
            if (id != director.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(director);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorExists(director.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(director);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Director.FirstOrDefaultAsync(m => m.Id == id);

            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var director = await _context.Director.FindAsync(id);

            _context.Director.Remove(director);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool DirectorExists(int id)
        {
            return _context.Director.Any(e => e.Id == id);
        }
    }
}
