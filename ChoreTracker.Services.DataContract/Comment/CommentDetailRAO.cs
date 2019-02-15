using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services.DataContract.Comment
{
    public class CommentDetailRAO
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public string Poster { get; set; }
        public int GroupId { get; set; }
    }
}
