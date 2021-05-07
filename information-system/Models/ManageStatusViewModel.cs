using information_system.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models
{
    public class ManageStatusViewModel
    {
        public List<Status> List { get; set; } = new List<Status>();
        public string SelectedValue { get; set; }
    }

}
