using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models
{
    public class ManageStatusViewModel
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public bool Selected { get; set; }
    }
}
