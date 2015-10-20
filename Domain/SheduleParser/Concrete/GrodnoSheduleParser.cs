using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.SheduleParser.Abstract;
using System.IO;

namespace Domain.SheduleParser.Concrete
{
    public class GrodnoSheduleParser : ISheduleParser
    {
        public IEnumerable<BusStop> Parse(string fileName, int? city)
        {



            throw new NotImplementedException();
        }
    }
}
