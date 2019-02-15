using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Web.DataContract.Comment
{
    public class CommentCreateDTO
    {
        [Required]
        public int GroupId { get; set; }

        public int ParentId { get; set; }

        [Required]
        [MinLength(5)]
        public string Content { get; set; }
    }
}
