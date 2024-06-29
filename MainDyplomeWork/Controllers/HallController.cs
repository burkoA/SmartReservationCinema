using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class HallController : Controller
    {
        private readonly FilmDbContext _context;

        public HallController(FilmDbContext context)
        {
            _context = context;
        }
        // GET: HallController
        public async Task<IActionResult> Index(string search)
        {
            var halls = from h in _context.Halls.OrderBy(h => h.HallName).Include(h => h.Cinema)
                        select h;

            if (!String.IsNullOrEmpty(search))
            {
                halls = halls.Where(h => h.HallName.Contains(search));
            }

            return View(await halls.ToListAsync());
        }

        // GET: HallController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.Include(c => c.Cinema).Include(h=>h.HallSectors)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }
            GenerateList();
            return View(hall);
        }

        // GET: HallController/Create
        public ActionResult Create()
        {
            GenerateList();
            return View();
        }

        private void GenerateList()
        {
            ViewBag.Cinema = new SelectList(_context.Cinemas.OrderBy(c => c.CinemaName), "Id", "CinemaName");
        }

        // POST: HallController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hall hall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            GenerateList();
            return View(hall);
        }

        // GET: HallController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.FindAsync(id);
            GenerateList();
            Hall halls = _context.Halls.Include(h => h.Cinema).Where(c => c.Id == id).FirstOrDefault();
            if (halls == null)
            {
                return NotFound();
            }
            return View(halls);
        }

        // POST: HallController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hall hall)
        {
            if (id != hall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallExists(hall.Id))
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
            GenerateList();
            return View(hall);
        }

        // GET: HallController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.Include(c => c.Cinema)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // POST: HallController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([FromForm] int id, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }

                Hall hall = _context.Halls.Where(c => c.Id == id).FirstOrDefault();

                if (hall == null)
                {
                    return View("Error", "Id not found");
                }

                _context.Halls.Remove(hall);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HallController/Create
        public ActionResult CreateSector(int id)
        {            
            return View(new HallSector() { HallId=id});
        }

        // POST: HallController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSector([FromForm]HallSector hallSector)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hallSector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = hallSector.HallId });
            }
          
            return View(hallSector);
        }

        public async Task<IActionResult> EditSector(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hallSector = await _context.HallSectors.FindAsync(id);            
            
            if (hallSector == null)
            {
                return NotFound();
            }
            return View(hallSector);
        }

        // POST: HallController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSector(int id, HallSector hallSector)
        {
            if (id != hallSector.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hallSector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallSectorExists(hallSector.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new {id=hallSector.HallId});
            }
           
            return View(hallSector);
        }

        // GET: HallController/Delete/5
        public async Task<IActionResult> DeleteSector(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hallSector = await _context.HallSectors.Include(c => c.Hall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hallSector == null)
            {
                return NotFound();
            }

            return View(hallSector);
        }

        // POST: HallController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSector([FromForm] int id, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }

                HallSector hallSect = _context.HallSectors.Where(c => c.Id == id).FirstOrDefault();

                if (hallSect == null)
                {
                    return View("Error", "Id not found");
                }

                _context.HallSectors.Remove(hallSect);
                _context.SaveChanges();

                return RedirectToAction(nameof(Details), new { id = hallSect.HallId });
            }
            catch
            {
                return View();
            }
        }

        private bool HallExists(int id)
        {
            return _context.Halls.Any(e => e.Id == id);
        }

        private bool HallSectorExists(int id)
        {
            return _context.HallSectors.Any(e => e.Id == id);
        }
    }
}
