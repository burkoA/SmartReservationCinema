using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartReservationCinema.FilmContext;
using SmartReservationCinema.Models;

namespace SmartReservationCinema.Controllers
{
    public class CommentController : Controller
    {
        private readonly FilmDbContext _db;

        public CommentController(FilmDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("CreateComment");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentModel commentModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AddComment(commentModel, _db, HttpContext);
                    return RedirectToAction("Details", "Film", new { id = commentModel.IdFilm });
                }
                else return View("CreateComment", commentModel);
            }
            catch
            {
                return View("CreateComment", commentModel);
            }
        }

        public static void AddComment(CommentModel commentModel, FilmDbContext _db, HttpContext httpContext)
        {
            Comment comment = new Comment();

            comment.Text = commentModel.Text;
            comment.FilmId = commentModel.IdFilm;
            comment.UserId = AccountController.GetCurrentUser(_db, httpContext).Id;

            _db.Comments.Add(comment);
            _db.SaveChanges();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _db.Comments.Remove(_db.Comments.Find(id));
                _db.SaveChanges();
                return View();
            }
            catch
            {
                return View("Error", new ErrorViewModel() { RequestId = "Delete error" });
            }
        }
    }
}
