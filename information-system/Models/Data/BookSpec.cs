using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace information_system.Models.Data
{
    public class BookSpec
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int SpecialtyId { get; set; }
        [JsonIgnore]
        public virtual Book Book { get; set; }
        public virtual Specialty Specialty { get; set; }
    }
}
