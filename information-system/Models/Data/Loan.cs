using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models.Data
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public int StatusId { get; set; }
        public DateTime Date { get; set; }

        public virtual Status Status { get; set; }
        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}
