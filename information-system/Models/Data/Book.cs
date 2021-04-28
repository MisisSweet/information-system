using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Articl { get; set; }
        public int StatusId { get; set; }

        public virtual Status Status { get; set; }
    }
}   
