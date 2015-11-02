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

        public bool Update(City city)
        {
            var items = context.Cities.Where(x => x.Id == city.Id);
            if(items.Any())
            {
                items.First().Name = city.Name;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int Id)
        {
            var item = Cities.Where(x => x.Id == Id).FirstOrDefault();
            if (item != null)
            {
                context.Cities.Remove(item);
                context.SaveChanges();
                return true;
            }
            return false;
        }


        public bool Contain(string Name)
        {
            var items = context.Cities.Where(x => x.Name == Name);
            if (items.Any())
            {
                return true;
            }
            return false;
        }

        public bool Add(string Name)
        {
            var item = context.Cities.Add(new City { Name = Name });
            if (item != null)
            {
                context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
