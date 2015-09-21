using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Models;

namespace Application.Models.Admin
{
    public class AdminAddViewModel
    {
        public IEnumerable<string> Numbers { get; set; }
        public IEnumerable<string> StopNames { get; set; }
        public IEnumerable<string> FinalStops { get; set; }
        public IEnumerable<string> Days { get; set; }
        public BusStopViewModel Stop { get; set; }
    }
}