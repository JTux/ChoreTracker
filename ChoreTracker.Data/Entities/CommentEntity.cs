using System;
using System.ComponentModel.DataAnnotations;

namespace ChoreTracker.Data.Entities
{
    public class CommentEntity
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public int ParentId { get; set; }

        [Required]
        public string Content { get; set; }
        public int GroupId { get; set; }
    }
}
