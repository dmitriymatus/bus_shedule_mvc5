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
            get
            {
                return context.UserRoutes;
            }
        }

        public void AddRoute(string userName,string busNumber, string name, string stop, string endStop, string days)
        {
            UserRoute route = new UserRoute { Name = name, UserName = userName, Stop = stop, EndStop = endStop, Days = days, BusNumber = busNumber };
            context.UserRoutes.Add(route);
            context.SaveChanges();

        }

        public IEnumerable<UserRoute> GetUserRoutes(string userName)
        {
            return context.UserRoutes.Where(x => x.UserName == userName);
        }


        public void UpdateRoute(int Id, string Name, string BusNumber, string Stop, string EndStop, string Days)
        {
            UserRoute route = context.UserRoutes.Where(x => x.Id == Id).FirstOrDefault();
            route.BusNumber = BusNumber;
            route.Name = Name;
            route.Stop = Stop;
            route.EndStop = EndStop;
            route.Days = Days;

            context.SaveChanges();
        }


        public void Delete(int Id)
        {
            UserRoute route = context.UserRoutes.Where(x => x.Id == Id).FirstOrDefault();
            context.UserRoutes.Remove(route);
            context.SaveChanges();
        }
    }
}
