using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;
using SmartReservationCinema.Models;

namespace SmartReservationCinema.Controllers
{
    public class FilmMarksController : Controller
    {
        private readonly FilmDbContext _context;

        public FilmMarksController(FilmDbContext context)
        {
            _context = context;
        }

        // GET: FilmMarks
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Index()
        {
            var filmDbContext = _context.FilmMarks.Include(f => f.Film).Include(f => f.User);
            return View(await filmDbContext.ToListAsync());
        }

        // GET: FilmMarks/Details/5
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

        // GET: FilmMarks/Create
        [Authorize]
        public IActionResult Create([FromQuery]int filmId)
        {
            //ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Description");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");

            return View("MarkPlace",new FilmMark() { FilmId = filmId});
        }

        // POST: FilmMarks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Mark,FilmId")] FilmMark filmMark)
        {
            User user = AccountController.GetCurrentUser(_context, HttpContext);
            if(user == null)
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
                    //ModelState.AddModelError("", "Slow down!");
                    //               return Content("Slow down!");
                    //return View("MarkPlace", filmMark);
                }
                _context.Add(filmMark);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MarkSaved));
            }
            //ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Description", filmMark.FilmId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", filmMark.UserId);
            return View("MarkPlace",filmMark);
        }

        private bool CheckSpamMark(User user, int filmId)
        {
            int cnt = _context.FilmMarks.Where(fm => fm.FilmId == filmId && fm.UserId == user.Id).Count();
            if(cnt > 0)
            {
                return false;
            }
			cnt = _context.FilmMarks.Where(fm => fm.MarkDate>DateTime.Now.AddMinutes(-15) && fm.UserId == user.Id).Count();
            if (cnt > 3)
            {
                return false;
            }
			return true;
        }

        public IActionResult MarkSaved()
        {
            return View();
        }

        //// GET: FilmMarks/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var filmMark = await _context.FilmMarks.FindAsync(id);
        //    if (filmMark == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Description", filmMark.FilmId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", filmMark.UserId);
        //    return View(filmMark);
        //}

        // POST: FilmMarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Mark,MarkDate,UserId,FilmId")] FilmMark filmMark)
        //{
        //    if (id != filmMark.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(filmMark);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!FilmMarkExists(filmMark.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["FilmId"] = new SelectList(_context.Films, "Id", "Description", filmMark.FilmId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", filmMark.UserId);
        //    return View(filmMark);
        //}

        // GET: FilmMarks/Delete/5
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

        // POST: FilmMarks/Delete/5
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
