using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Web.DataContract.Group
{
    public class GroupAcceptanceDTO
    {
        public int GroupId { get; set; }
        public int GroupMemberId { get; set; }
        public bool Accepted { get; set; }
    }
}
