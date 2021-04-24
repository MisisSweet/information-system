using information_system.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models
{
    public class UserRole
    {
        public User user { get; set; }
        public string role { get; set; } 
    }
}
