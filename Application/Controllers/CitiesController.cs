﻿using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Models.Cities;

namespace Application.Controllers
{
    public class CitiesController : Controller
    {
        ICitiesRepository citiesRepository;
        public CitiesController(ICitiesRepository _citiesRepository)
        {
            citiesRepository = _citiesRepository;
        }

        public ActionResult Index()
        {
            var city = citiesRepository.Cities.FirstOrDefault();
            CitiesIndexViewModel model = new CitiesIndexViewModel { Cities = new List<string>(), SelectedCity = null };
            if (Session["City"] == null && city != null)
            {
                Session["City"] = city.Id;
            }
            if (citiesRepository.Cities.Any())
            {

                model = new CitiesIndexViewModel
                {
                    Cities = citiesRepository.Cities.Select(x => x.Name),
                };
                var temp = citiesRepository.Cities.Where(x => x.Id == (int)Session["City"]);
                if (temp.Any())
                {
                    model.SelectedCity = temp.First().Name;
                }
                else
                {
                    model.SelectedCity = citiesRepository.Cities.FirstOrDefault().Name;
                }
            }
            return PartialView("_CitiesIndex", model);
        }

        [OutputCache(Duration = 1, NoStore = false)]
        public void SetCity(string city)
        {
            var result = citiesRepository.Cities.Where(x => x.Name == city);
            if (result.Any())
            {
                Session["City"] = result.First().Id;
            }
        }

        //=============================CRUD==================================

        [Authorize(Roles = "admin")]
        public ActionResult List()
        {
            var model = citiesRepository.Cities.Select(x => x.Name);
            return View(model);
        }


        [Authorize(Roles = "admin")]
        public ActionResult Add()
        {
            var model = new CityViewModel();
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Add(CityViewModel model)
        {
            var contain = citiesRepository.Contain(model.Name);
            if (ModelState.IsValid && !contain)
            {
                var success = citiesRepository.Add(model.Name);
                if (success)
                {
                    TempData["Success"] = "Запись добавлена";
                    return RedirectToAction("List", "Cities");
                }
            }
            if (contain)
            {
                TempData["Errors"] = "Запись с таким городом уже существует";
            }
            else
            {
                TempData["Errors"] = "Что-то пошло не так";
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete()
        {
            var model = citiesRepository.Cities.Select(x => x.Name);
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id)
        {
            if (citiesRepository.Cities.Count() == 1)
            {
                TempData["Errors"] = "Нельзя удалить единственную запись";
                return RedirectToAction("List", "Cities");
            }
            var success = citiesRepository.Delete(Id);
            if (success)
            {
                TempData["Success"] = "Запись удалена";

            }
            else
            {
                TempData["Errors"] = "Что-то пошло не так";
            }
            return RedirectToAction("List", "Cities");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(string City)
        {
            CityViewModel model = new CityViewModel();
            var cities = citiesRepository.Cities.Where(x => x.Name == City);
            if (cities.Any())
            {
                var city = cities.First();
                model.Id = city.Id;
                model.Name = city.Name;
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(CityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = citiesRepository.Update(new Domain.Models.City { Id = model.Id, Name = model.Name });
            if (success)
            {
                TempData["Success"] = "Запись обновлена";
                return RedirectToAction("List", "Cities");
            }
            else
            {
                TempData["Errors"] = "Что-то пошло не так";
            }
            return View(model);
        }
    }
}