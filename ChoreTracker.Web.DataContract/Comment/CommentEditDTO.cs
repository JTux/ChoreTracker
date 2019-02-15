using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Web.DataContract.Comment
{
    public class CommentEditDTO
    {
        [Required]
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int GroupId { get; set; }
    }
}
