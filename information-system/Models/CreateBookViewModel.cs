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
        [Display(Name = "Название книги")]
        public string BookName { get; set; }
        [Required]
        [Display(Name = "Автор")]
        public string Author { get; set; }
        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Год публикации")]
        public int Year { get; set; }
        [Required]
        [Display(Name = "Артикул")]
        public string Articl { get; set; }
        [Display(Name ="Изображение")]
        public string BookPicture { get; set; }
        [Required]
        [Display(Name = "Жанры")]
        public string Genre { get; set; }
        [Required]
        [Display(Name = "Дисциплины")]
        public string Discipline { get; set; }
        [Required]
        [Display(Name = "Специальности")]
        public string Specialty { get; set; }
        public List<string> Chapters { get; set; }
        [Required]
        [Display(Name = "Количество")]
        public int Count { get; set; }
    }
}
