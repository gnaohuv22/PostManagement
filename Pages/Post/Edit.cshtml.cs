using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Pages.Post
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<CRUDHub> _hubContext;

        public SelectList CategoryNames { get; set; }

        public EditModel(ApplicationDbContext context, IHubContext<CRUDHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public int PostId { get; set; }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Content { get; set; }

        [BindProperty]
        public bool PublishStatus { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FirstOrDefaultAsync(m => m.PostId == id);

            if (post == null)
            {
                return NotFound();
            }
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != post.AuthorId.ToString())
            {
                return RedirectToPage("../Account/AccessDenied");
            }
            CategoryNames = new SelectList(await _context.PostCategories.ToListAsync(), nameof(PostCategories.CategoryId), nameof(PostCategories.CategoryName));
            PostId = post.PostId;
            Title = post.Title;
            Content = post.Content;
            PublishStatus = post.PublishStatus;
            CategoryId = post.CategoryId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                CategoryNames = new SelectList(await _context.PostCategories.ToListAsync(), nameof(PostCategories.CategoryId), nameof(PostCategories.CategoryName));
                return Page();
            }

            Posts post = _context.Posts.FirstOrDefault(m => m.PostId == PostId);
            if (post == null)
            {
                return NotFound();
            }

            post.Title = Title;
            post.Content = Content;
            post.PublishStatus = PublishStatus;
            post.CategoryId = CategoryId;
            post.UpdatedDate = DateTime.Now;

            _context.Attach(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("LoadPost");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(PostId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
