using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Pages.Post
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;

        }
        public IList<Posts> Posts { get; set; } = default!;
        public void OnGet()
        {

        }

        public ContentResult OnGetGetPostsAsync(int? postId, string title, string content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) userId = "0";

            var query = _context.Posts
                .Include(p => p.AppUser)
                .Include(p => p.PostCategories)
                .Where(p => p.PublishStatus == true || p.AppUser.UserId == int.Parse(userId))
                .Select(p => new PostDto
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    CategoryName = p.PostCategories.CategoryName,
                    CreatedDate = p.CreatedDate,
                    AuthorEmail = p.AppUser.Email,
                    PublishStatus = p.PublishStatus
                })
                .AsQueryable();

            if (postId.HasValue)
            {
                query = query.Where(p => p.PostId == postId.Value);
            }

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(p => p.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(content))
            {
                query = query.Where(p => p.Content.Contains(content));
            }

            var posts = query.ToList();
            string jsonStr = JsonSerializer.Serialize(posts);
            Console.WriteLine(jsonStr);
            return Content(jsonStr);
        }
    }
}
