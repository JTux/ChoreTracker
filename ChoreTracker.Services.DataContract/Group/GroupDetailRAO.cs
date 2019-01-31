using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services.DataContract.Group
{
    public class GroupDetailRAO
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string GroupInviteKey { get; set; }

        public string GroupOwner { get; set; }
    }
}
