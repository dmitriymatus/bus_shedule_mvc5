using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Models;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class EFUserRoutesRepository : IUserRoutesRepository
    {
        SheduleDbContext context = new SheduleDbContext();

        public IEnumerable<UserRoute> Routes
        {
            get { return context.UserRoutes.Include(x => x.Bus).Include(x => x.City).Include(x => x.Direction).Include(x => x.Stop); }
        }

        public void AddRoute(UserRoute route)
        {
            route.Bus = context.Buses.FirstOrDefault(x => x.Id == route.Bus.Id);
            route.City = context.Cities.FirstOrDefault(x => x.Id == route.City.Id);
            route.Stop = context.BusStops.FirstOrDefault(x => x.Id == route.Stop.Id);
            route.Direction = context.Directions.FirstOrDefault(x => x.Id == route.Direction.Id);
            context.UserRoutes.Add(route);
            context.SaveChanges();

        }

        public void UpdateRoute(UserRoute route)
        {
            UserRoute item = context.UserRoutes.FirstOrDefault(x => x.Id == route.Id);
            item.Bus = context.Buses.FirstOrDefault(x => x.Id == route.Bus.Id);
            item.Stop = context.BusStops.FirstOrDefault(x => x.Id == route.Stop.Id);
            item.Direction = context.Directions.FirstOrDefault(x => x.Id == route.Direction.Id);
            item.City = context.Cities.FirstOrDefault(x => x.Id == route.City.Id);
            context.SaveChanges();
        }

        public void DeleteRoute(UserRoute route)
        {
            context.UserRoutes.Remove(route);
            context.SaveChanges();
        }
    }
}
