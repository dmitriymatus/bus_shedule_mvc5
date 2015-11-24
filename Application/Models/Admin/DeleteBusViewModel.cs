using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Admin
{
    public class DeleteBusViewModel
    {
        [Required]
        public string Bus { get; set; }
        public List<SelectListItem> Buses { get; set; }
    }
}
