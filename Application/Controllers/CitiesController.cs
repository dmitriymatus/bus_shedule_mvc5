using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Models;
using Application.Models.Cities;

namespace Application.Controllers
{
    public class CitiesController : Controller
    {
        IRepository<City> citiesRepository;

        public CitiesController(IRepository<City> _citiesRepository)
        {
            citiesRepository = _citiesRepository;
        }

        public ActionResult Index()
        {
            var city = citiesRepository.Get(null).FirstOrDefault();
            CitiesIndexViewModel model = new CitiesIndexViewModel { Cities = new List<string>(), SelectedCity = null };
            if (Session["City"] == null && city != null)
            {
                Session["City"] = city.Id;
            }
            if (citiesRepository.Get(null).Any())
            {

                model = new CitiesIndexViewModel
                {
                    Cities = citiesRepository.Get(null).Select(x => x.Name),
                };
                var cityId = (int)Session["City"];
                var temp = citiesRepository.Get(x => x.Id == cityId);
                if (temp.Any())
                {
                    model.SelectedCity = temp.First().Name;
                }
                else
                {
                    model.SelectedCity = citiesRepository.Get(null).FirstOrDefault().Name;
                }
            }
            return PartialView("_CitiesIndex", model);
        }

        [OutputCache(Duration = 1, NoStore = false)]
        public void SetCity(string city)
        {
            var result = citiesRepository.Get(x => x.Name == city);
            if (result.Any())
            {
                Session["City"] = result.First().Id;
            }
        }

        //=============================CRUD==================================

        [Authorize(Roles = "admin")]
        public ActionResult List()
        {
            var model = citiesRepository.Get(null).Select(x => x.Name);
            return View(model);
        }


        [Authorize(Roles = "admin")]
        public ActionResult Add()
        {
            return View(new CityViewModel());
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Add(CityViewModel model)
        {
            if (!ModelState.IsValid)
            {                  
                    return View(model);
            }
            try
            {
                citiesRepository.Insert(new City { Name = model.Name });
                TempData["Success"] = "Запись добавлена";
            }
            catch(Exception ex)
            {
                //log error
                ModelState.AddModelError("", "Ошибка при добавлении записи. Повторите попытку позже");
                return View(model);
            }
            return View(new CityViewModel());
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(string City)
        {
            var city = citiesRepository.Get(x => x.Name == City).First();
            return View(new CityViewModel { Id = city.Id, Name = city.Name });
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(CityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var city = citiesRepository.GetByID(model.Id);
                city.Name = model.Name;
                citiesRepository.Update(city);
                TempData["Success"] = "Запись обновлена";
            }
            catch (Exception ex)
            {
                //log error
                ModelState.AddModelError("", "Ошибка при обновлении записи. Повторите попытку позже");
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            if (citiesRepository.Get(null).Count() == 1)
            {
                return RedirectToAction("List", "Cities");
            }

            try
            {
                citiesRepository.Delete(Id);
                TempData["Success"] = "Запись удалена";
            }
            catch(Exception ex)
            {
                //log
                TempData["Errors"] = "Ошибка при удалении записи. Повторите попытку позже";
            }
            return RedirectToAction("List", "Cities");
        }

    }
}