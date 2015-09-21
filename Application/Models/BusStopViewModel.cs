using System.ComponentModel.DataAnnotations;
using Application.Infrastructure;

namespace Application.Models
{
    public class BusStopViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Номер")]
        public string busNumber { get; set; }
        [Required]
        [Display(Name = "Остановка")]
        public string stopName { get; set; }

        [Required]
        [StopsFormat(ErrorMessage = "Неправильно заполнено расписание")]
        [Display(Name = "Расписание")]
        public string stops { get; set; }

        [Required]
        [Display(Name = "Конечная")]
        public string finalStop { get; set; }
        [Required]
        [Display(Name = "Дни")]
        public string days { get; set; }
    }
}