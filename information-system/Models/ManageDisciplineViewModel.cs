using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models
{
    public class ManageDisciplineViewModel
    {
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; }
        public bool Selected { get; set; }
    }
}
