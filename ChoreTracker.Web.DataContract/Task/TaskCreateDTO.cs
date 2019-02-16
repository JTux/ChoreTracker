using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Web.DataContract.Task
{
    public class TaskCreateDTO
    {
        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public int RewardPoints { get; set; }

        public int GroupId { get; set; }
    }
}
