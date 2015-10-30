using System.Data.Entity;
using Domain.Models;

namespace Domain.Concrete
{
    public class SheduleDbContext : DbContext
    {
        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<Days> Days { get; set; }
        public DbSet<Shedule> Shedule { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<UserRoute> UserRoutes { get; set; }
        public DbSet<News> News { get; set; }
    }
}
