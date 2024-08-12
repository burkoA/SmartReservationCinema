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
    public class GenresController : Controller
    {
        private readonly FilmDbContext _context;

        public GenresController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Index([FromQuery] string search = "")
        {
            IEnumerable<Genre> items = _context.Genres;

            if (!string.IsNullOrEmpty(search))
            {
                var wordsToSearch = SplitSearch(search);
                items = items.Where(g =>
                {
                    foreach (string word in wordsToSearch)
                    {
                        if (g.GenreName.ToLower().Contains(word.ToLower()))
                            return true;
                    }
                    return false;
                });
            }

            return View(items.OrderBy(g => g.GenreName).ToList());
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

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
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
        public async Task<IActionResult> Create([Bind("Id,GenreName")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GenreName")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}
