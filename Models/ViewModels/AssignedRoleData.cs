using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVClog.Models.ViewModels
{
    public class AssignedRoleData
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}