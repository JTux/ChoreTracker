﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.Services.DataContract.Group
{
    public class GroupMemberDetailRAO
    {
        public string UserName { get; set; }

        public bool IsMod { get; set; }

        public int RewardPoints { get; set; }
    }
}
