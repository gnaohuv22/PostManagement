using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Pages.Post
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<CRUDHub> _hubContext;

        public SelectList CategoryNames { get; set; }

        public CreateModel(ApplicationDbContext context, IHubContext<CRUDHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            CategoryNames = new SelectList(await _context.PostCategories.ToListAsync(), nameof(PostCategories.CategoryId), nameof(PostCategories.CategoryName));
            return Page();
        }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Content { get; set; }

        [BindProperty]
        public bool PublishStatus { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                CategoryNames = new SelectList(await _context.PostCategories.ToListAsync(), nameof(PostCategories.CategoryId), nameof(PostCategories.CategoryName));
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("./Account/AccessDenied");
            }

            Posts post = new Posts
            {
                AuthorId = int.Parse(userId),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Title = Title,
                Content = Content,
                PublishStatus = PublishStatus,
                CategoryId = CategoryId
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("LoadPost");

            return RedirectToPage("./Index");
        }
    }
}
