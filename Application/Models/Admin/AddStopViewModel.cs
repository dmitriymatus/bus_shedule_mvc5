using System.ComponentModel.DataAnnotations;

namespace Application.Models.Admin
{
    public class AddStopViewModel
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
    }
}
