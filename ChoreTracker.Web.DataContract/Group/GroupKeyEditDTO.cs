using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Web.DataContract.Group
{
    public class GroupKeyEditDTO
    {
        public int GroupId { get; set; }
        public string NewInviteKey { get; set; }
    }
}
