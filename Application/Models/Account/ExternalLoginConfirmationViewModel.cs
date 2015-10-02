using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }
    }
}