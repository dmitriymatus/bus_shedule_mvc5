using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminManageController : Controller
    {
        private const int usersOnPage = 10;
        private ApplicationUserManager _userManager;
        public AdminManageController()
        {
        }

        public AdminManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public ActionResult Users(string searchValue="All",int page = 1)
        {
            var aaa = HttpContext.Request.RawUrl;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Items", GetItems(page,searchValue));
            }
            return View(GetItems(page, searchValue));
        }

        private IQueryable<Application.Models.ApplicationUser> GetItems(int page, string searchValue)
        {
            if(searchValue == "All" || searchValue == null)
            { 
                return UserManager.Users.OrderBy(x => x.UserName).Skip((page * usersOnPage) - usersOnPage).Take(usersOnPage);
            }
            else
            {
                return UserManager.Users.Where(x => x.UserName.Contains(searchValue)).OrderBy(x => x.UserName).Skip((page * usersOnPage) - usersOnPage).Take(usersOnPage);
            }
        }



    }
}