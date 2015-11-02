using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Domain.Models
{
    public class Shedule
    {
        [Key]
        public int Id { get; set; }

        public Bus Bus { get; set; }

        public BusStop BusStop { get; set; }

        public virtual Direction Direction { get; set; }

        public virtual Days Days { get; set; }

        public City City { get; set; }

        public String Items { get; set; }

    }
}
