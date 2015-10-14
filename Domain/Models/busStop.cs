using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{

    public class BusStop
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Номер")]
        public string BusNumber { get; set; }

        [Required]
        [Display(Name = "Остановка")]
        public string StopName { get; set; }

        [Required]
        [Display(Name = "Расписание")]
        public string Stops { get; set; }

        [Required]
        [Display(Name = "Конечная")]
        public string FinalStop { get; set; }

        [Required]
        [Display(Name = "Дни")]
        public string Days { get; set; }

        [Required]
        public int? CityId { get; set; }

    }

}
