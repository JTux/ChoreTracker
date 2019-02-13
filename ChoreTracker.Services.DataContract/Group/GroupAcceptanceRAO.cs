using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services.DataContract.Group
{
    public class GroupAcceptanceRAO
    {
        public int GroupMemberId { get; set; }
        public int GroupId { get; set; }
        public bool Accepted { get; set; }
    }
}
