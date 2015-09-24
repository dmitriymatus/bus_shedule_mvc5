using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models.AdminManage
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public String Email { get; set; }
        public IEnumerable<string> Roles { get; set;}
    }
}