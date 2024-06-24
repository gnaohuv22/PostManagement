using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Posts
    {
        public int PostId { get; set; }

        public int AuthorId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public bool PublishStatus { get; set; }

        public int CategoryId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual PostCategories PostCategories { get; set; }
    }
}
