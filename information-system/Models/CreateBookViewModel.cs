using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models
{
    public class CreateBookViewModel
    {
        [Required]
        [Display(Name = "BookName")]
        public string BookName { get; set; }
        [Required]
        [Display(Name = "Author")]
        public string Author { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Year")]
        public int Year { get; set; }
        [Required]
        [Display(Name = "Articl")]
        public string Articl { get; set; }
    }
}
