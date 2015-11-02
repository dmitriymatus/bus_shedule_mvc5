﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public City City { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }
    }
}
