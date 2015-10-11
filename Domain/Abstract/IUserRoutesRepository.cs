using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Abstract
{
    public interface IUserRoutesRepository
    {
        IEnumerable<UserRoute> Routes { get; }
        IEnumerable<UserRoute> GetUserRoutes(string userName);

        void AddRoute(string userName, string busNumber, string name, string stop, string endStop);
        void UpdateRoute(int Id, string Name, string BusNumber,string Stop, string EndStop);
        void Delete(int Id);
    }
}
