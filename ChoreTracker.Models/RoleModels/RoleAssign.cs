using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Models.RoleModels
{
    public enum Role { GroupOwner = 1, GroupMember}
    public class RoleAssign
    {
        public Role GroupChoice { get; set; }
    }
}
