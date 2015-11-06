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
        void AddRoute(UserRoute route);
        void UpdateRoute(UserRoute route);
        void DeleteRoute(UserRoute route);
    }
}
