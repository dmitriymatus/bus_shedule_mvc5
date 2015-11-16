using System.Data.Entity;
using Domain.Models;

namespace Domain.Concrete
{
    public class SheduleDbContext : DbContext
    {
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Shedule> Shedules { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }

        public DbSet<UserRoute> UserRoutes { get; set; }
        public DbSet<News> News { get; set; }  
    }
}
