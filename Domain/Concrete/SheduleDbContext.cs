using System.Data.Entity;
using Domain.Models;

namespace Domain.Concrete
{
    public class SheduleDbContext : DbContext
    {
        public DbSet<busStop> Stops { get; set; }
        public DbSet<UserRoute> UserRoutes { get; set; }
    }
}
