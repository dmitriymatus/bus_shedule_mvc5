﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Abstract;

namespace Domain.SheduleParser.Abstract
{
    public interface ISheduleParser
    {
        IEnumerable<BusStop> Parse(string fileName, int? city);
    }
}
