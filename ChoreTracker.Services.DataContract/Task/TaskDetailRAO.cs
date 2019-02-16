using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services.DataContract.Task
{
    public class TaskDetailRAO
    {
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public int RewardPoints { get; set; }

        public bool Completed { get; set; }

        public DateTimeOffset DateCreated { get; set; }
    }
}
