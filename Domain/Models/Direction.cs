using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Direction
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public Bus Bus { get; set; }
    }
}
