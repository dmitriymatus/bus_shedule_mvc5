using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Application.Models.AdminManage;

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
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Items", GetItems(page,searchValue));
            }
            return View(GetItems(page, searchValue));
        }

        [HttpGet]
        [ActionName("User")]
        public async Task<ActionResult> UserInfo(string id)
        {
            var roles = await UserManager.GetRolesAsync(id);           
            var user = await UserManager.FindByIdAsync(id);

            UserViewModel model = new UserViewModel() { Name = user.UserName, Email = user.Email ?? "-", Roles = roles, Id = id };
            return View(model);
        }
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<ActionResult> SetOnRole(string Id, string role = "admin")
        {
            if(Id != null && role != null)
            { 
            var result = await UserManager.AddToRoleAsync(Id, role);
            }

            return RedirectToAction("User",new {id = Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveFromRole(string Id, string role = "admin")
        {
            if (Id != null && role != null)
            {
                var result = await UserManager.RemoveFromRoleAsync(Id, role);
            }

            return RedirectToAction("User", new { id = Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string Id)
        {
            if (Id != null)
            {
                var user = await UserManager.FindByIdAsync(Id);
                var result = await UserManager.DeleteAsync(user);
            }

            return RedirectToAction("Users");
        }


        private IQueryable<Application.Models.ApplicationUser> GetItems(int page, string searchValue)
        {
            if(searchValue == "All" || searchValue == null)
            { 
                return UserManager.Users.Where(x=>x.UserName != User.Identity.Name).OrderBy(x => x.UserName).Skip((page * usersOnPage) - usersOnPage).Take(usersOnPage);
            }
            else
            {
                return UserManager.Users.Where(x => x.UserName.Contains(searchValue) &&  x.UserName != User.Identity.Name).OrderBy(x => x.UserName).Skip((page * usersOnPage) - usersOnPage).Take(usersOnPage);
            }
        }



    }
}