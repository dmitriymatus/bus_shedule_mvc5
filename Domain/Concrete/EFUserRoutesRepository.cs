using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Models;

namespace Domain.Concrete
{
    public class EFUserRoutesRepository : IUserRoutesRepository
    {
        SheduleDbContext context = new SheduleDbContext();
        public IEnumerable<UserRoute> Routes
        {
            get { return context.UserRoutes; }
        }

        public void AddRoute(string userName, string busNumber, string name, string stop, string endStop, int city)
        {
            UserRoute route = new UserRoute
            {
                Name = name,
                UserName = userName,
                Stop = stop,
                EndStop = endStop,
                BusNumber = busNumber,
                CityId = city
            };
            context.UserRoutes.Add(route);
            context.SaveChanges();

        }

        public IEnumerable<UserRoute> GetUserRoutes(string userName, int? city)
        {
            return context.UserRoutes.Where(x => x.UserName == userName && x.CityId == city);
        }


        public void UpdateRoute(int Id, string Name, string BusNumber, string Stop, string EndStop, int city)
        {
            UserRoute route = context.UserRoutes.FirstOrDefault(x => x.Id == Id);
            route.BusNumber = BusNumber;
            route.Name = Name;
            route.Stop = Stop;
            route.EndStop = EndStop;
            route.CityId = city;

            context.SaveChanges();
        }


        public void Delete(int Id)
        {
            UserRoute route = context.UserRoutes.FirstOrDefault(x => x.Id == Id);
            context.UserRoutes.Remove(route);
            context.SaveChanges();
        }
    }
}
