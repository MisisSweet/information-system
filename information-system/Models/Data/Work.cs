using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models.Data
{
    public class Work
    {
        public int Id { get; set; }
        public int DayOfWeek { get; set; }
        public bool isWork { get; set; }
        public DateTime WorkStart { get; set; }
        public DateTime WorkEnd { get; set; }
        public DateTime PauseStart { get; set; }
        public DateTime PauseEnd { get; set; }
    }
}
