using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using WebApplication1.Models;

namespace WebApplication1.Pages.Post
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<CRUDHub> _hubContext;

        public DeleteModel(ApplicationDbContext context, IHubContext<CRUDHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        [BindProperty]
        public Posts Post { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post = await _context.Posts.FirstOrDefaultAsync(m => m.PostId == id);

            if (Post == null)
            {
                return NotFound();
            }
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != Post.AuthorId.ToString())
            {
                return RedirectToPage("../Account/AccessDenied");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                Post = post;
                _context.Posts.Remove(Post);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("LoadPost");
            }

            return RedirectToPage("./Index");
        }
    }
}
