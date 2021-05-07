using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models
{
    public class ManageUserRolesViewModel
    {
        public List<ManageUserRolesViewModelItem> List { get; set; } = new List<ManageUserRolesViewModelItem>();
        public string SelectedValue { get; set; }
    }

    public class ManageUserRolesViewModelItem
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
