using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartReservationCinema.FilmContext;
using SmartReservationCinema.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class CalendarController : Controller
    {

        private readonly FilmDbContext _db;

        public CalendarController(FilmDbContext context)
        {
            _db = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EventWarning(EventModel eventModel)
        {
            HttpContext.Session.SetString("LastUser", HttpContext.User.Identity.Name ?? "");
            if (ModelState.IsValid)
            {
                return View(eventModel);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        [HttpPost]
        [GoogleScopedAuthorize(CalendarService.ScopeConstants.CalendarEvents, CalendarService.ScopeConstants.Calendar)]
        public async Task<IActionResult> EventCreate(EventModel eventModel, [FromServices] IGoogleAuthProvider auth)
        {
            if (ModelState.IsValid)
            {

                GoogleCredential cred = await auth.GetCredentialAsync();
                var service = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = cred
                });

                CalendarList calendars = await service.CalendarList.List().ExecuteAsync();
                CalendarListEntry calEntry = calendars.Items.Where((CalendarListEntry calItem) => calItem.Primary == true).FirstOrDefault();
                string calendarId = calEntry.Id;

                Event ev = new Event()
                {
                    Start = new EventDateTime() { DateTime = eventModel.StartDateTime },
                    End = new EventDateTime() { DateTime = eventModel.EndDateTime },
                    Description = eventModel.CinemaName,
                    Summary = eventModel.FilmName
                };

                Event ev2 = await service.Events.Insert(ev, calendarId).ExecuteAsync();

                string userName = HttpContext.Session.GetString("LastUser");
                if (!string.IsNullOrEmpty(userName))
                {
                    User user = _db.Users.FirstOrDefault(u => u.Email == userName);
                    AccountController.InternalLogin(user, HttpContext);
                }

                return View("EventAddOk");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
