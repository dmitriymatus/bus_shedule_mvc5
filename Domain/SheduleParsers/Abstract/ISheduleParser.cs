using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Abstract;

namespace Domain.SheduleParsers.Abstract
{
    public interface ISheduleParser
    {
        IEnumerable<TimeTable> Parse(string fileName, City city);
    }
}
