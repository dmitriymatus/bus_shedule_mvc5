﻿using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Bus
    {
        [Key]
        public int Id { get; set; }

        public string Number { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}
