﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Routes
{
    public class RoutesViewModel
    {
        [Required]
        public string BusNumber { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Stop { get; set; }
        [Required]
        public string NearestBus { get; set; }
    }
}