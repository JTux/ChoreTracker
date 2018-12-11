using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.CommentModels
{
    public class CommentCreate
    {
        [Required]
        public int GroupId { get; set; }

        public int ParentId { get; set; }

        [Required]
        [MinLength(5)]
        public string Content { get; set; }
    }
}
