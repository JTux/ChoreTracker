using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services.DataContract.Comment
{
    public class CommentCreateRAO
    {
        public int GroupId { get; set; }

        public int ParentId { get; set; }

        public string Content { get; set; }
    }
}
