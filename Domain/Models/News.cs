using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime Time { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}
