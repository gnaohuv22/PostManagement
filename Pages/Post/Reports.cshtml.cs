using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication1.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Pages.Post
{
    [Authorize]
    public class ReportModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReportModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<PostDto> Posts { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public async Task<IActionResult> OnGetAsync(DateTime? startDate, DateTime? endDate)
        {
            StartDate = startDate ?? DateTime.Now.AddMonths(-1);
            EndDate = endDate?.Date.AddDays(1).AddSeconds(-1) ?? DateTime.Now;
            var DefaultStartDate = DateTime.Now.AddMonths(-1);
            var DefaultEndDate = DateTime.Now.AddDays(1).AddSeconds(-1);
            if (StartDate > EndDate)
            {
                Posts = await _context.Posts
                .Include(p => p.AppUser)
                .Include(p => p.PostCategories)
                .Where(p => p.CreatedDate >= DefaultStartDate && p.CreatedDate <= DefaultEndDate)
                .Select(p => new PostDto
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    CategoryName = p.PostCategories.CategoryName,
                    CreatedDate = p.CreatedDate,
                    AuthorEmail = p.AppUser.Email,
                    PublishStatus = p.PublishStatus
                }).OrderByDescending(p => p.CreatedDate).ToListAsync();
            }
            else
            {
                Posts = await _context.Posts
                .Include(p => p.AppUser)
                .Include(p => p.PostCategories)
                .Where(p => p.CreatedDate >= StartDate && p.CreatedDate <= EndDate)
                .Select(p => new PostDto
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    CategoryName = p.PostCategories.CategoryName,
                    CreatedDate = p.CreatedDate,
                    AuthorEmail = p.AppUser.Email,
                    PublishStatus = p.PublishStatus
                }).OrderByDescending(p => p.CreatedDate).ToListAsync();
            }
            
            return Page();
        }
    }
}
