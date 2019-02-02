﻿using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Services.DataContract.Comment;
using ChoreTracker.WebMVC.DataContract.Comment;
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

        public bool CreateComment(CommentCreateRAO model)
        {
            if (model.Content == null)
            {
                return false;
            }

            var comment = new CommentEntity
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

        public IEnumerable<CommentListItemDTO> GetGroupComments(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var groupComments = ctx
                    .Comments
                    .Where(c => c.GroupId == groupId)
                    .Select(c =>
                    new CommentListItemDTO
                    {
                        Poster = ctx.Users.FirstOrDefault(u => u.Id == c.OwnerId.ToString()).UserName,
                        OwnerId = c.OwnerId,
                        CommentId = c.CommentId,
                        Content = c.Content,
                        GroupId = c.GroupId,
                        ParentId = c.ParentId
                    }).ToList();

                var sortedList = new List<CommentListItemDTO>();
                var orderedList = groupComments.OrderBy(c => c.ParentId).ToList();

                foreach (var comment in orderedList)
                {
                    if (!sortedList.Contains(comment))
                    {
                        if (comment.ParentId == 0)
                            sortedList.Add(comment);
                        else
                        {
                            var lastSibling = sortedList.LastOrDefault(e => e.ParentId == comment.ParentId);

                            if (lastSibling == null)
                            {
                                var parentIndex = sortedList.IndexOf(sortedList.Find(c => c.CommentId == comment.ParentId));
                                sortedList.Insert((parentIndex + 1), comment);
                            }
                            else
                                sortedList.Insert((sortedList.IndexOf(lastSibling) + 1), comment);
                        }
                    }
                }

                return sortedList;
            }
        }

        public CommentDetailDTO GetCommentById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var comment = ctx.Comments.FirstOrDefault(c => c.CommentId == id);

                return new CommentDetailDTO
                {
                    CommentId = comment.CommentId,
                    Content = comment.Content,
                    Poster = ctx.Users.FirstOrDefault(u => u.Id == comment.OwnerId.ToString()).UserName
                };
            }
        }

        public bool EditComment(CommentEditRAO rao)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Comments
                        .Single(c => c.CommentId == rao.CommentId && c.OwnerId == _userId);

                entity.Content = rao.Content;

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
