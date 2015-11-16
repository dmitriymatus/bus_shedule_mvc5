using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Stop
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; }

        public virtual ICollection<TimeTable> TimeTables { get; set; }
    }
}
