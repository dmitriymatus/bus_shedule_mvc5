using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models.Cities
{
    public class CitiesIndexViewModel
    {
        public IEnumerable<string> Cities { get; set; }
        public string SelectedCity { get; set; }
    }
}