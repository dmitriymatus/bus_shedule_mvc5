using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Admin
{
    public class DeleteSheduleViewModel
    {
        public IEnumerable<string> Buses { get; set; }

        [Required]
        public string Bus { get; set; }
        [Required]
        public string Stop { get; set; }
        [Required]
        public string EndStop { get; set; }
        [Required]
        public string Days { get; set; }
    }
}
