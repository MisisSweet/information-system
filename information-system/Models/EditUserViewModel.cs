using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace information_system.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Логин")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Номер читательского билета")]
        public string NumderReadTicket { get; set; }
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Изображение профиля")]
        public string ProfilePicture { get; set; }
        [Display(Name = "Номер группы")]
        public string GroupNumber { get; set; }
    }
}
