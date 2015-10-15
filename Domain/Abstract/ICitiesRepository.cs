using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Abstract
{
    public interface ICitiesRepository
    {
        IEnumerable<City> Cities { get; }
        IEnumerable<string> GetCitiesName();
        bool Add(string Name);
        bool Update(City city);
        bool Delete(int Id);
        bool Contain(string Name);
    }
}
