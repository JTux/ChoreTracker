using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.WebMVC.DataContract.Comment
{
    public class CommentEditDTO
    {
        [Required]
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
