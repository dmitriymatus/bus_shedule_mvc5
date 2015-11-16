using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Bus> Buses { get; set; }
        public virtual ICollection<Stop> Stops { get; set; }
    }
}
