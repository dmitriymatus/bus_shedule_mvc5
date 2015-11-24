using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Application.Models.Admin
{
    public class DeleteStopViewModel
    {
        [Required]
        public string Stop { get; set; }
        public List<SelectListItem> Stops { get; set; }
    }
}
