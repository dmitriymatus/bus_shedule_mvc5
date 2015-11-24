using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Models.Admin
{
    public class AddSheduleViewModel
    {
        [Required]
        public string Bus { get; set; }

        [Required]
        public string Stop { get; set; }
        
        [Required]
        public string EndStop { get; set; }

        public IEnumerable<string> Days { get; set; }

        [Required]
        public IEnumerable<string> SelectedDays { get; set; }

        public IEnumerable<SelectListItem> Buses { get; set; }

        [Required]
        public IEnumerable<TimeSpan> Shedule { get; set; }
    }
}
