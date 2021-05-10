using information_system.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models
{
    public class HomeModel
    {
        public List<Book> Books { get; set; }
        public List<Work> Works { get; set; }
    }

}
