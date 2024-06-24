

namespace WebApplication1.Models
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool PublishStatus { get; set; }
    }
}
