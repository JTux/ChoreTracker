using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.CommentModels
{
    public class CommentDetail
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public string Poster { get; set; }
    }
}
