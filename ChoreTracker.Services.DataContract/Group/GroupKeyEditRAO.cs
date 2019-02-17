using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services.DataContract.Group
{
    public class GroupKeyEditRAO
    {
        public int GroupId { get; set; }
        public string NewInviteKey { get; set; }
    }
}
