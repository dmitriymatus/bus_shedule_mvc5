using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.News
{
    public class NewsViewModel
    {
        public int Id { get; set; }

        [Required]        
        public string Title { get; set; }

        public DateTime Time { get; set; }

        [Required]
        [MaxLength(500)]
        public string Text { get; set; }
    }
}