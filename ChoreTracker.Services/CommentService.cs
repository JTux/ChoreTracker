using ChoreTracker.Data;
using ChoreTracker.Models.CommentModels;
using ChoreTracker.WebMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services
{
    public class CommentService
    {
        private Guid _userId;
        public CommentService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateComment(CommentCreate model)
        {
            var comment = new Comment
            {
                OwnerId = _userId,
                Content = model.Content,
                ParentId = model.ParentId
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Comments.Add(comment);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CommentListItem> GetComments()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx
                    .Comments
                    .Where(c => c.OwnerId == _userId)
                    .Select(c =>
                    new CommentListItem
                    {
                        CommentId = c.CommentId,
                        Content = c.Content,
                        ParentId = c.ParentId
                    });

                return query.ToArray();
            }
        }
    }
}
