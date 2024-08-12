using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.Entity;
using SmartReservationCinema.FilmContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartReservationCinema.Controllers
{
    public class SessionController : Controller
    {

        private readonly FilmDbContext _context;

        public SessionController(FilmDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create([FromRoute(Name = "id")] int cinemaId)
        {
            GenerateList(cinemaId);
            Cinema curCinema = _context.Cinemas.FirstOrDefault(c => c.Id == cinemaId);

            return View(new Session() { CinemaId = cinemaId, Cinema = curCinema });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create([FromForm] Session session)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.HallList = new SelectList(_context.Halls.Where(h => h.CinemaId == session.CinemaId).OrderBy(h => h.HallName), "Id", "HallName");
                    return Exception("");
                }

                _context.Sessions.Add(session);

                _context.SaveChanges();
                return RedirectToAction("Details", "Cinema", new { id = session.CinemaId });
            }
            catch
            {
                GenerateList(session.CinemaId);
                return View();
            }
        }

        private ActionResult Exception(string v)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id)
        {
            Session session = _context.Sessions.Where(s => s.Id == id).Include(s => s.Cinema).FirstOrDefault();
            GenerateList(session.CinemaId);

            ViewBag.Prices = PriceTable(session.Id, session.HallId.Value);
            ViewBag.SessionId = id;

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit([FromRoute] int id, [FromForm] Session session)
        {
            if (id == 0 || id != session.Id) return BadRequest();

            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                _context.Sessions.Update(session);
                _context.SaveChanges();

                return RedirectToAction("Details", "Cinema", new { id = session.CinemaId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                session.Cinema = _context.Cinemas.FirstOrDefault(c => c.Id == session.CinemaId);
                GenerateList(session.CinemaId);
                ViewBag.Prices = PriceTable(session.Id, session.HallId.Value);

                return View(session);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete(int id)
        {
            Session session = _context.Sessions
                .Include(s => s.Cinema)
                .Include(s => s.Hall)
                .Include(s => s.Film)
                .Include(s => s.Language)
                .FirstOrDefault(s => s.Id == id);

            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete([FromRoute] int id, Session session)
        {
            try
            {
                _context.Sessions.Remove(session);
                _context.SaveChanges();
                return RedirectToAction("Details", "Cinema", new { id = session.CinemaId });
            }
            catch
            {
                return View(session);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult EditPrices([FromForm] TicketPrice[] prices, int sessionId)
        {

            if (prices.Length > 0)
            {
                foreach (TicketPrice price in prices)
                {
                    if (price.Price != null && price.Price > 0)
                    {
                        if (price.Id > 0)
                        {
                            _context.TicketPrices.Update(price);
                        }
                        else
                        {
                            _context.TicketPrices.Add(price);
                        }
                    }

                }
                _context.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = sessionId });
        }

        private void GenerateList(int cinemaId)
        {
            ViewBag.FilmList = new SelectList(_context.Films.OrderBy(f => f.FilmName), "Id", "FilmName");
            ViewBag.LanguageList = new SelectList(_context.Languages.OrderBy(l => l.LanguageName), "Id", "LanguageName");
            ViewBag.HallList = new SelectList(_context.Halls.Where(h => h.CinemaId == cinemaId).OrderBy(h => h.HallName), "Id", "HallName");
        }

        private List<TicketPrice> PriceTable(int sessionId, int hallId)
        {

            SqlParameter session = new SqlParameter("@sessionId", sessionId);
            SqlParameter hall = new SqlParameter("@hallId", hallId);

            Dictionary<int, TicketPrice> prices = _context.TicketPrices.Include(pr => pr.HallSector).Where(pr => pr.SessionId == sessionId && pr.HallSector.HallId == hallId).ToDictionary(pr => pr.SectorId);

            List<HallSector> sectors = _context.HallSectors.Where(hs => hs.HallId == hallId).ToList();

            List<TicketPrice> priceTable = new List<TicketPrice>();

            foreach (HallSector sector in sectors)
            {
                if (prices.ContainsKey(sector.Id))
                {
                    priceTable.Add(prices[sector.Id]);
                }
                else priceTable.Add(new TicketPrice()
                {
                    Id = 0,
                    Price = 0,
                    SectorId = sector.Id,
                    SessionId = sessionId,
                    HallSector = sector
                });
            }

            return priceTable;

        }
    }
}
