using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Admin
{
    public class AddBusViewModel
    {
        [Required]
        [MaxLength(4)]
        [RegularExpression(@"(\d+)[А-Я]",ErrorMessage ="Неправильный номер")]      
        public string Number { get; set; }
    }
}
