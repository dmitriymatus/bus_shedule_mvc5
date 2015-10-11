using System.Data.Entity;
using Domain.Models;

namespace Domain.Concrete
{
    public class SheduleDbContext : DbContext
    {
        public DbSet<BusStop> Stops { get; set; }
        public DbSet<UserRoute> UserRoutes { get; set; }
    }
}
