using ChoreTracker.Data;
using ChoreTracker.Models.CommentModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
                ParentId = model.ParentId,
                GroupId = model.GroupId
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Comments.Add(comment);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CommentListItem> GetGroupComments(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx
                    .Comments
                    .Where(c => c.GroupId == groupId)
                    .Select(c =>
                    new CommentListItem
                    {
                        Poster = ctx.Users.FirstOrDefault(u => u.Id == c.OwnerId.ToString()).UserName,
                        OwnerId = c.OwnerId,
                        CommentId = c.CommentId,
                        Content = c.Content,
                        ParentId = c.ParentId
                    });

                return query.ToList();
            }
        }

        public CommentDetail GetCommentById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var comment = ctx.Comments.FirstOrDefault(c => c.CommentId == id);

                return new CommentDetail
                {
                    CommentId = comment.CommentId,
                    Content = comment.Content,
                    Poster = ctx.Users.FirstOrDefault(u => u.Id == comment.OwnerId.ToString()).UserName
                };
            }
        }
    }
}
