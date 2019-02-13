using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.WebMVC.DataContract.Group
{
    public class GroupMemberDetailDTO
    {
        public int MemberId { get; set; }

        public string UserName { get; set; }

        public int RewardPoints { get; set; }
    }
}
