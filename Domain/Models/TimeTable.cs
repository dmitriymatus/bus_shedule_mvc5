using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Domain.Models
{
    public class TimeTable
    {
        [Key]
        public int Id { get; set; }

        public int BusId { get; set; }
        public virtual Bus Bus { get; set; }

        public virtual Stop Stop { get; set; }

        public virtual Stop NextStop { get; set; }

        public virtual Stop FinalStop { get; set; }

        public virtual ICollection<Shedule> Shedules { get; set; }

    }
}
