using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserRoute
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string BusNumber { get; set; }
        public string Name { get; set; }
        public string Stop { get; set; }
        public string EndStop { get; set; }
        public string Days { get; set; }
    }
}
