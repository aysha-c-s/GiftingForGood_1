using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GFG1.Data;
using GFG1.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace GFG1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Name = user.Username;
            ViewBag.Email = user.Email;
            ViewBag.Phone = user.Phone;
            ViewBag.OptionalPhone = user.OptionalPhone;
            ViewBag.Address = user.Address;

            var posts = _context.Posts.Include(p => p.User).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                posts = posts.Where(p => p.ShortDescription.Contains(search) ||
                                         p.Area.Contains(search) ||
                                         p.FullAddress.Contains(search) ||
                                         p.Type.Contains(search) ||
                                         p.Amount.ToString().Contains(search) ||
                                         p.ExpireDate.ToString().Contains(search));
            }

            return View(await posts.ToListAsync());
        }

        public async Task<IActionResult> MyPosts(string search)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = _context.Posts.Where(d => d.UserId == Int32.Parse(userId));

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.ShortDescription.Contains(search) ||
                                         d.Area.Contains(search) ||
                                         d.FullAddress.Contains(search) ||
                                         d.Type.Contains(search) ||
                                         d.Amount.Contains(search) ||
                                         d.ExpireDate.ToString().Contains(search));
            }

            var userPosts = await query.Include(d => d.User).ToListAsync();
            return View(userPosts);
        }
    }
}
