using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;
using SmartReservationCinema.Models;
using SmartReservationCinema.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class AccountController : Controller
    {
        private readonly FilmDbContext _context;
        private readonly MailSender _mailSender;

        public AccountController(FilmDbContext context, MailSender mailSender)
        {
            _context = context;
            _mailSender = mailSender;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (!CheckLoginAttempts(model.Email))
                {
                    ModelState.AddModelError("", "Too many try. Try again after 15 min");
                    AddLoginErrorToLog(model.Email);
                    return View(model);
                }

                User user = _context.Users.FirstOrDefault((User user) => user.Email == model.Email);

                if (user != null && user.Password == FilmContext.User.GetPasswordHash(model.Password))
                {
                    InternalLogin(user, HttpContext);
                    return Redirect("~/Film/Index");
                }

                ModelState.AddModelError("", "Wrong password or email");
                AddLoginErrorToLog(model.Email);
            }
            return View(model);
        }

        private void AddLoginErrorToLog(string email)
        {
            var failedLogin = new FailedLogin
            {
                Email = email,
                IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                Time = DateTime.Now
            };

            _context.Add(failedLogin);
            _context.SaveChanges();
        }

        private bool CheckLoginAttempts(string email)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var fifteenMinutesAgo = DateTime.Now.AddMinutes(-15);

            int recentAttempts = _context.FailedLogins.Count(f =>
                (f.Email == email || f.IPAddress == ipAddress) && f.Time > fifteenMinutesAgo);

            return recentAttempts <= 5;
        }

        async public static void InternalLogin(User user, HttpContext httpContext)
        {
            List<Claim> claim = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier,user.Email),
                            new Claim(ClaimTypes.Name,user.Email),
                            new Claim(ClaimTypes.Role,user.Role),
                            new Claim(ClaimTypes.Sid,user.Id.ToString())
                        };
            ClaimsIdentity identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        [HttpGet]
        public IActionResult Registration() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = new User(model);
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    InternalLogin(user, HttpContext);
                    return Redirect("~/Film/Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Unable to register the user. May be this mail already registred");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("~/Film/Index");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            User user = GetCurrentUser(_context, HttpContext);

            if (user == null)
            {
                return View("Error", new ErrorViewModel() { RequestId = "Something went wrong!" });
            }

            ProfileModel profile = new ProfileModel(user);
            return View(profile);
        }

        [HttpPost]
        public IActionResult Profile(ProfileModel profile)
        {
            if (ModelState.IsValid)
            {
                User user = GetCurrentUser(_context, HttpContext);
                user.Update(profile);

                _context.Update(user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Film");
            }

            return View(profile);
        }

        public static User? GetCurrentUser(FilmDbContext db, HttpContext httpContext)
        {
            return db.Users.Where(u => u.Email == httpContext.User.Identity.Name).FirstOrDefault();
        }

        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordModel changePassword)
        {
            if (ModelState.IsValid)
            {
                User user = GetCurrentUser(_context, HttpContext);
                string oldPasswordHashed = FilmContext.User.GetPasswordHash(changePassword.OldPassword);

                if (oldPasswordHashed != user.Password)
                {
                    ModelState.AddModelError("OldPassword", "Old password is wrong!");
                    return View(changePassword);
                }

                user.Password = FilmContext.User.GetPasswordHash(changePassword.NewPassword);
                _context.Users.Update(user);
                _context.SaveChanges();

                return RedirectToAction("PasswordAlreadyChanged");
            }

            return View(changePassword);
        }

        [HttpGet]
        public IActionResult Denied() => View();

        [HttpGet]
        public IActionResult PasswordRestore() => View("RestorePassword");

        [HttpPost]
        public async Task<IActionResult> PasswordRestore(PasswordRestore passwordRestore)
        {
            if (ModelState.IsValid)
            {
                User user = _context.Users.Where(u => u.Email == passwordRestore.Email).FirstOrDefault();

                if (user == null)
                {
                    ModelState.AddModelError("Email", "This email doesn't exist");
                    return View(passwordRestore);
                }

                var code = new Random().Next(100000, 999999);
                HttpContext.Session.SetInt32("RestoreCode", code);
                HttpContext.Session.SetString("CodeTime", DateTime.Now.AddMinutes(15).Ticks.ToString());
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetInt32("CodeIsRight", 0);

                _mailSender.SendMessage(user.Email, "Restore Code Smart Reservation",
                    "Your generate code " + code);
                return RedirectToAction("CheckRestoreCode");
            }

            return View("RestorePassword");
        }

        [HttpGet]
        public IActionResult CheckRestoreCode() => View();

        [HttpPost]
        public async Task<IActionResult> CheckRestoreCode(PasswordRestore passwordRestore)
        {
            if (String.IsNullOrEmpty(passwordRestore.RestoreCode))
            {
                ModelState.AddModelError("RestoreCode", "Restore code cannot be empty!");
                return View(passwordRestore);
            }

            if (!int.TryParse(passwordRestore.RestoreCode, out var code))
            {
                ModelState.AddModelError("RestoreCode", "Invalid restore code.");
                return View(passwordRestore);
            }

            var correctCode = HttpContext.Session.GetInt32("RestoreCode") ?? 0;
            var codeExpirationTime = long.Parse(HttpContext.Session.GetString("CodeTime") ?? "0");

            if (correctCode == 0 || DateTime.Now.Ticks > codeExpirationTime)
            {
                ModelState.AddModelError("RestoreCode", "Restore code has expired.");
                return View(passwordRestore);
            }

            if (code != correctCode)
            {
                ModelState.AddModelError("RestoreCode", "Incorrect restore code.");
                return View(passwordRestore);
            }

            HttpContext.Session.SetInt32("CodeIsRight", 1);
            return RedirectToAction("EnterNewPassword");

        }

        [HttpGet]
        public IActionResult EnterNewPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnterNewPassword(EnterNewPassword newPassword)
        {
            if (!ModelState.IsValid) return View(newPassword);

            String userEmail = HttpContext.Session.GetString("Email");
            int codeIsRight = HttpContext.Session.GetInt32("CodeIsRight") ?? 0;

            if (userEmail == null || codeIsRight != 1)
            {
                ModelState.AddModelError("", "Error work!");
                return View(newPassword);
            }

            User user = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View(newPassword);
            }

            user.Password = FilmContext.User.GetPasswordHash(newPassword.Password);
            _context.Users.Update(user);
            _context.SaveChanges();

            return RedirectToAction("PasswordAlreadyChanged");
        }

        [HttpGet]
        public IActionResult PasswordAlreadyChanged() => View();

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AccountList([FromQuery] String search = "")
        {
            var accounts = from a in _context.Users
                           select a;

            if (!String.IsNullOrEmpty(search))
            {
                accounts = accounts.Where(a => a.FirstName.Contains(search) || a.Email.Contains(search));
            }

            return View(await accounts.ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.SetRoleSelections();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.Role = Request.Form["Role"];
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(AccountList));
            }

            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AccountList));
        }

        [Authorize]
        public async Task<IActionResult> FavouriteFilmList()
        {
            ViewBag.CurrentUser = AccountController.GetCurrentUser(_context, HttpContext);
            int userId = ViewBag.CurrentUser.Id;

            IEnumerable<FavouriteFilm> items = _context.FavouriteFilms.Include(ff => ff.Film).Where(ff => ff.UserId == userId);

            return View(items);
        }

        public async Task<IActionResult> DeleteFavoriteFilm(int id)
        {
            User CurrentUser = AccountController.GetCurrentUser(_context, HttpContext);

            if (CurrentUser == null) { return Denied(); }

            try
            {
                FavouriteFilm favFilm = await _context.FavouriteFilms.FirstOrDefaultAsync(ff => ff.UserId == CurrentUser.Id && ff.Id == id);

                if (favFilm == null) { return NotFound(); }

                _context.FavouriteFilms.Remove(favFilm);
                _context.SaveChanges();
            }
            catch
            {
                return NotFound();
            }

            return RedirectToAction("FavouriteFilmList");
        }
    }
}
