using information_system.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models
{
    public class FilterModel
    {
        public string Name { get; set; }
        public string Discipline { get; set; }
        public string Specialty { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int CountPage{ get; set; }
        public List<Book> Books { get; set; } 
    }
}
