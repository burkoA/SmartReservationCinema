using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartReservationCinema.FilmContext;
using System;
using System.Linq;

namespace SmartReservationCinema.Controllers
{
    public class FavouriteFilmController : Controller
    {

        private readonly FilmDbContext _db;
        //// GET: FavouriteFilmController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: FavouriteFilmController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}
        public FavouriteFilmController(FilmDbContext db)
        {
            _db = db;
        }

        [Authorize]
        public ActionResult AddToFavourite(int id)
        {
            try
            {
                User currentUser = AccountController.GetCurrentUser(_db, HttpContext);
                if (_db.FavouriteFilms.Where(ff => ff.UserId == currentUser.Id && ff.FilmId == id).Count() == 0)
                {
                    
                    Film film = _db.Films.Where(f => f.Id == id).FirstOrDefault();
                    if (film == null) throw new Exception("This film doesn't exist");
                    _db.FavouriteFilms.Add(new FavouriteFilm() { Film = film, User = currentUser});
                    _db.SaveChanges();
                   
                }
                else
                {
                    return NotFound();
                }
                return Ok();               
                
            }
            catch
            {
                return BadRequest(); 
            }
        }

        [Authorize]
        public ActionResult RemoveFavourite(int id)
        {
            try
            {
                User currentUser = AccountController.GetCurrentUser(_db, HttpContext);
                FavouriteFilm? favoriteFilm = _db.FavouriteFilms.Where(ff => ff.UserId == currentUser.Id && ff.FilmId == id).FirstOrDefault();
                if (favoriteFilm != null)
                {                    
                    _db.FavouriteFilms.Remove(favoriteFilm);
                    _db.SaveChanges();

                }
                else return NotFound();

                return Ok();

            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: FavouriteFilmController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FavouriteFilmController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
