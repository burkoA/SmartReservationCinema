using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Collections.Specialized.BitVector32;

namespace SmartReservationCinema.Controllers
{
    public class SessionController : Controller
    {

        private readonly FilmDbContext _context;

        public SessionController(FilmDbContext context)
        {
            _context = context;
        }

        // GET: SessionController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: SessionController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: SessionController/Create
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create([FromRoute(Name ="id")]int cinemaId)
        {
            GenerateList(cinemaId);
            Cinema curCinema=_context.Cinemas.FirstOrDefault(c=>c.Id == cinemaId);            
            return View(new Session() { CinemaId=cinemaId, cinema=curCinema});
        }

        private void GenerateList(int cinemaId)
        {
            //ViewBag.CinemaList = new SelectList(_context.Cinemas.OrderBy(c => c.CinemaName), "Id", "CinemaName");
            ViewBag.FilmList = new SelectList(_context.Films.OrderBy(f => f.FilmName), "Id", "FilmName");
            ViewBag.LanguageList = new SelectList(_context.Languages.OrderBy(l => l.LanguageName), "Id", "LanguageName");
            ViewBag.HallList = new SelectList(_context.Halls.Where(h => h.CinemaId == cinemaId).OrderBy(h => h.HallName), "Id", "HallName");
        }

        // POST: SessionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create([FromForm]Session session)
        {
            try
            {
                if (!ModelState.IsValid) { 
                    ViewBag.HallList = new SelectList(_context.Halls.Where(h => h.CinemaId == session.CinemaId).OrderBy(h => h.HallName), "Id", "HallName");
                    return Exception(""); 
                }

                _context.Sessions.Add(session);
                
                _context.SaveChanges();
                return RedirectToAction("Details","Cinema",new {id= session.CinemaId });
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

        // GET: SessionController/Edit/5
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id)
        {            
            Session session = _context.Sessions.Where(s => s.Id == id).Include(s=>s.cinema).FirstOrDefault();
            GenerateList(session.CinemaId);
            ViewBag.Prices = PriceTable(session.Id, session.HallId.Value);
            ViewBag.SessionId = id;
            return View(session);
        }

        // POST: SessionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit([FromRoute]int id,[FromForm] Session session)
        {
            if (id == 0 || id != session.Id) return BadRequest();
            try
            {
                if (!ModelState.IsValid) { throw new Exception(); }
                _context.Sessions.Update(session);
                _context.SaveChanges();
                return RedirectToAction("Details", "Cinema", new { id = session.CinemaId });
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                session.cinema=_context.Cinemas.FirstOrDefault(c=>c.Id==session.CinemaId);
                GenerateList(session.CinemaId);
                ViewBag.Prices = PriceTable(session.Id, session.HallId.Value);
                return View(session);
            }
        }




        // GET: SessionController/Delete/5
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete(int id)
        {
            Session session = _context.Sessions.Include(s => s.cinema).Include(s => s.hall).Include(s => s.film)
                .Include(s => s.Language).FirstOrDefault(s => s.Id == id);
            return View(session);
        }

        // POST: SessionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete([FromRoute]int id, Session session)
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

        private List<TicketPrice> PriceTable(int sessionId, int hallId)
        {
            
                SqlParameter session = new SqlParameter("@sessionId", sessionId);
                SqlParameter hall = new SqlParameter("@hallId", hallId);
                //                List<TicketPrice> prices=_context.TicketPrices.Include(p => p.HallSector).Where(p => p.SessionId == sessionId && p.HallSector.HallId==hallId).ToList();
                // List<TicketPrice> prices = _context.TicketPrices.FromSqlRaw("select TicketPrices.Id, TicketPrices.Price,TicketPrices.SessionId, HallSectors.Id as SectorId from HallSectors left join TicketPrices on HallSectors.Id=TicketPrices.SectorId and TicketPrices.SessionId= @sessionId where  HallSectors.HallId= @hallId",
                //new SqlParameter[]{ session, hall })/*.Include(p => p.HallSector)*/.ToList();*/
                Dictionary<int, TicketPrice> prices = _context.TicketPrices.Include(pr => pr.HallSector).Where(pr => pr.SessionId == sessionId && pr.HallSector.HallId == hallId).ToDictionary(pr => pr.SectorId);

                List<HallSector> sectors=_context.HallSectors.Where(hs=>hs.HallId==hallId).ToList();

                List<TicketPrice> priceTable = new List<TicketPrice>();
                foreach(HallSector sector in sectors)
                {
                    if (prices.ContainsKey(sector.Id))
                        priceTable.Add(prices[sector.Id]);
                    else priceTable.Add(new TicketPrice() { Id=0, Price=0, SectorId=sector.Id, SessionId=sessionId, HallSector=sector });
                }

                return priceTable;
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult EditPrices([FromForm] TicketPrice[] prices, int sessionId)
        {
            
            //List<TicketPrice> existPrices = _context.TicketPrices.Where(p => p.SessionId == sessionId && p.HallSector.HallId == hallId).ToList();
            if (prices.Length > 0)
            {
                foreach (TicketPrice price in prices)
                {
                    if (price.Price != null && price.Price > 0)
                    {
                        //if (existPrices.Where(p => p.SessionId == price.SessionId && p.SectorId == price.SectorId).FirstOrDefault() != null)
                        if(price.Id > 0)
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
            return RedirectToAction("Edit",new {id= sessionId });
/*
            IEnumerable<HallSector> hallSectors = _context.HallSectors.Where(hs => hs.HallId == session.HallId);
            foreach (HallSector sector in hallSectors)
            {
                TicketPrice ticketPrice = new TicketPrice();
                ticketPrice.HallSector = sector;
                ticketPrice.Session = session;

                _context.TicketPrices.Add(ticketPrice);
            }
*/
        }
        

    }
}
