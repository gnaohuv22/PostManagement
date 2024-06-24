using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class PostCategories
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Posts> Posts { get; set; } = new List<Posts>();
    }
}
