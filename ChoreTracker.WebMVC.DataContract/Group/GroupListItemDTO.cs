﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoreTracker.WebMVC.DataContract.Group
{
    public class GroupListItemDTO
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool InGroup { get; set; }
    }
}
