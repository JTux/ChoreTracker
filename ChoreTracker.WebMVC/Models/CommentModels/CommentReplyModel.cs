using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoreTracker.WebMVC.Models.CommentModels
{
    public class CommentReplyModel
    {
        public int CommentId { get; set; }
        public string Content { get; internal set; }
        public int GroupId { get; internal set; }
    }
}