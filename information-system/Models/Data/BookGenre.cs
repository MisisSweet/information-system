using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace information_system.Models.Data
{
    public class BookGenre
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int GenreId { get; set; }
        [JsonIgnore]
        public virtual Book Book { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
