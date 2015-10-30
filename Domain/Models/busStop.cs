using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Domain.Models
{

    public class BusStop
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public City City { get; set; }

        public virtual ICollection<Bus> Buses { get; set; }

    }

}
