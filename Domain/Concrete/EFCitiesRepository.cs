using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Models;

namespace Domain.Concrete
{
    public class EFCitiesRepository : ICitiesRepository
    {
        SheduleDbContext context = new SheduleDbContext();
        public IEnumerable<City> Cities
        {
            get
            {
                return context.Cities;
            }
        }

        public IEnumerable<string> GetCitiesName()
        {
            return context.Cities.Select(x => x.Name);
        }
    }
}
