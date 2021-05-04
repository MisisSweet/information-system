using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models.Data
{
    public class Chapter
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int NumberChapter { get; set; }
        public string Name { get; set; }

        public virtual Book Book { get; set; }
    }
}
