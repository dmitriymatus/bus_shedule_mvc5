using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Shedule
    {
        [Key]
        public int Id { get; set; }

        public Days Days { get; set; }

        public TimeSpan Time { get; set; }

        public int TimeTableId { get; set; }
        public TimeTable TimeTable { get; set; }
    }
}
