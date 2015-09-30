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
    }
}
