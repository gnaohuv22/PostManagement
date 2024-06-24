using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }
        public string Fullname { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Posts> Posts { get; set; } = new List<Posts>();
    }
}
