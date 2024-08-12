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
    public class ActorController : Controller
    {
        private readonly FilmDbContext _context;

        public ActorController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Index(string search)
        {
            var actors = from a in _context.Actors.OrderBy(a => a.Name) select a;

            if (!String.IsNullOrEmpty(search))
            {
                actors = actors.Where(a => a.Name.Contains(search));
            }

            return View(await actors.ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Create(Actor actor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(actor);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int id, Actor actor)
        {
            if (id != actor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id))
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

            return View(actor);
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _context.Actors.FirstOrDefaultAsync(m => m.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actor = await _context.Actors.FindAsync(id);

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
