using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Models.Manage;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Application.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {

        private ApplicationUserManager _userManager;
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager)
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


        // GET: Manage
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword()
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["result"] = "Пароль изменён";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult SetPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
            if (result.Succeeded)
            {
                TempData["result"] = "Пароль создан";
                return RedirectToAction("Index");
            }
            return View(model);
        }



        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

    }
}