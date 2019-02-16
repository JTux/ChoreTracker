using ChoreTracker.Data;
using ChoreTracker.Data.Entities;
using ChoreTracker.Services.DataContract.Task;
using ChoreTracker.Web.DataContract.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services
{
    public class TaskService
    {
        private Guid _userId;

        public TaskService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateTask(TaskCreateRAO rao)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = new TaskEntity
                {
                    TaskName = rao.TaskName,
                    TaskDescription = rao.TaskDescription,
                    RewardPoints = rao.RewardPoints,
                    GroupId = rao.GroupId,
                    DateCreated = DateTimeOffset.Now,
                    Completed = false
                };

                ctx.Tasks.Add(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<TaskListItemDTO> GetGroupTasks(int groupId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Tasks.Where(t => t.GroupId == groupId)
                    .Select(t => new TaskListItemDTO
                    {
                        TaskId = t.TaskId,
                        TaskName = t.TaskName,
                        RewardPoints = t.RewardPoints,
                        Completed = t.Completed,
                        DateCreated = t.DateCreated
                    }).ToArray();

                return query;
            }
        }
    }
}
