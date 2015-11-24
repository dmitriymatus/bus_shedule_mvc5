using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Application.Models.Admin
{
    public class AddBusRouteViewModel
    {
        [Required]
        public string Bus { get; set; }

        public IEnumerable<string> First { get; set; }

        public IEnumerable<string> Second { get; set; }
            
        public IEnumerable<SelectListItem> Stops { get; set; }

        public IEnumerable<SelectListItem> Buses { get; set; }
    }
}
