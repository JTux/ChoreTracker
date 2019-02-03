﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Data
{
    public class Comment
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
