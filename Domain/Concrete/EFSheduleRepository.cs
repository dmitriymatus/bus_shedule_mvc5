using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Domain.Abstract;
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class EFSheduleRepository : ISheduleRepository
    {
        SheduleDbContext context = new SheduleDbContext();

        public IEnumerable<City> Cities
        {
            get { return context.Cities; }
        }

        public IEnumerable<Direction> Directions
        {
            get { return context.Directions.Include(x => x.Bus) ; }
        }

        public IEnumerable<BusStop> BusStops
        {
            get { return context.BusStops; }
        }

        public void AddStopsRange(IEnumerable<BusStop> entities)
        {
            context.BusStops.AddRange(entities);
            context.SaveChanges();
        }

        public IEnumerable<Bus> Buses
        {
            get { return context.Buses.Include(x => x.City); }
        }

        public void AddBus(Bus entity)
        {
            context.Buses.Add(entity);
            context.SaveChanges();
        }

        public void AddBusesRange(IEnumerable<Bus> entities)
        {
            var aaa = context.Buses.AddRange(entities);
            context.SaveChanges();
        }


        public IEnumerable<Days> Days
        {
            get { return context.Days; }
        }
        public void AddDaysRange(IEnumerable<Days> entities)
        {
            context.Days.AddRange(entities);
            context.SaveChanges();
        }

        public IEnumerable<Shedule> Shedule
        {
            get { return context.Shedule.Include(x => x.Direction).Include(x => x.Days).Include(x => x.Bus).Include(x => x.BusStop).Include(x => x.City); }
        }

        public void AddSheduleRange(IEnumerable<Shedule> entities)
        {
            var buses = entities.Select(x => x.Bus).Distinct(new Compare());
            context.Shedule.AddRange(entities);
            context.SaveChanges();
        }

        public void AddShedule(Shedule entity)
        {
            context.Shedule.Add(entity);
            context.SaveChanges();
        }

        public bool UpdateShedule(Shedule entity, string newShedule)
        {
            try
            {
                entity.Items = newShedule;
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteShedule(Shedule entity)
        {
            try
            {
                Bus bus = entity.Bus;
                bus.BusStops.Remove(entity.BusStop);
                context.Shedule.Remove(entity);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteAllShedule(City city)
        {
            try
            {
                var allShedule = Shedule.Where(x => x.City == city);

                context.UserRoutes.RemoveRange(context.UserRoutes.Where(x => x.Bus.CityId == city.Id));
                context.Buses.RemoveRange(Buses.Where(x => x.City.Id == city.Id));
                context.BusStops.RemoveRange(BusStops.Where(x => x.City.Id == city.Id));
                context.Directions.RemoveRange(Shedule.Where(x => x.City.Id == city.Id).Select(x => x.Direction));
                context.Shedule.RemoveRange(allShedule);
                context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }


        class Compare : IEqualityComparer<Bus>
        {
            public bool Equals(Bus bus1, Bus bus2)
            {
                return (bus1.Number == bus2.Number && bus1.City == bus2.City);
            }
            public int GetHashCode(Bus bus)
            {
                return bus.Number.GetHashCode();
            }
        }

       
    }
}
