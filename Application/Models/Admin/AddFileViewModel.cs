using System.Web;
using System.ComponentModel.DataAnnotations;
using Application.Infrastructure;

namespace Application.Models.Admin
{
    public class AddFileViewModel
    {
        [Required(ErrorMessage = "Выберите файл")]
        [DataType(DataType.Upload)]
        [FileSize(20000000, ErrorMessage = "Максимальный размер файла не должен превышать 20MB")]
        public HttpPostedFileBase file { get; set; }
    }
}