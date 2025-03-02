using GFG1.Data;
using GFG1.Models;
using Microsoft.AspNetCore.Mvc;

namespace GFG1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string name, string password)
        {
            var user = _context.Users.FirstOrDefault(u => (u.Email == name || u.Username == name) && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Incorrect email or password!";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ViewBag.Error = "User already exists!";
                return View();
            }
            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match!";
                return View();
            }
            _context.Users.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login");
        }
    }
}
