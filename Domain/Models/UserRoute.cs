
namespace Domain.Models
{
    public class UserRoute
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Bus Bus { get; set; }
        public string Name { get; set; }
        public BusStop Stop { get; set; }
        public Direction Direction { get; set; }
        public City City { get; set; }
    }
}
