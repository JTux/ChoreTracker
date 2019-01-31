using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services.DataContract.Comment
{
    public class CommentListItemRAO
    {
        public int CommentId { get; set; }
        public int ParentId { get; set; }
        public string Content { get; set; }
        public string Poster { get; set; }
        public Guid OwnerId { get; set; }
    }
}
