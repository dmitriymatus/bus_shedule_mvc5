using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Cities
{
    public class CityViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}