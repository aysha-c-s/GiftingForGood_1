using GFG1.Models;
using GFG1.Data;
using GFG1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GiftingForGood.Controllers
{
    
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PostController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post model, IFormFile? image)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.UserId = userId;

            if (image != null && image.Length > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (validExtensions.Contains(ext))
                {
                    string fileName = Guid.NewGuid() + ext;
                    string filePath = Path.Combine(_env.WebRootPath, "img", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    model.Photo = fileName;
                }
                else
                {
                    model.Photo = "food.png"; // Default image
                }
            }
            else
            {
                model.Photo = "food.png"; // Default image
            }

            _context.Posts.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}