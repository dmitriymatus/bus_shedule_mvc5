using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class UserRoute
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public int TimeTableId { get; set; }
        public virtual TimeTable TimeTable { get; set; }
        //public virtual Bus Bus { get; set; }

        //public virtual Stop Stop { get; set; }

        //public virtual Stop FinalStop { get; set; }

        //public virtual City City { get; set; }
    }
}
