using System;

namespace ChoreTracker.Web.DataContract.Comment
{
    public class CommentListItemDTO
    {
        public int CommentId { get; set; }
        public int GroupId { get; set; }
        public int ParentId { get; set; }
        public string Content { get; set; }
        public string Poster { get; set; }
        public Guid OwnerId { get; set; }
    }
}
