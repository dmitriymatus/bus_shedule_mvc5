using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Account
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}