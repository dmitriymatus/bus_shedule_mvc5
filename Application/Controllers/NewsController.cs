using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Application.Models.News;

namespace Application.Controllers
{
    public class NewsController : Controller
    {
        private const int ItemsOnPage = 5;
        INewsRepository repository;
        ICitiesRepository citiesRepository;
        public NewsController(INewsRepository _repository, ICitiesRepository _citiesRepository)
        {
            repository = _repository;
            citiesRepository = _citiesRepository;
        }

        public ActionResult Index()
        {
            var city = citiesRepository.Cities.FirstOrDefault();
            var model = repository.GetNewsInCity(city.Id).Any();
            return PartialView(model);
        }

        [OutputCache(Duration = 3600, VaryByParam = "City ; Page", SqlDependency = "shedule:News")]
        public ActionResult GetItems(string City, int Page = 1)
        {
            int? cityId = (int?)Session["City"];
            var model = repository.GetNewsInCity(cityId)
                .OrderByDescending(x => x.Time)
                .Skip((Page * ItemsOnPage) - ItemsOnPage)
                .Take(ItemsOnPage);
                return PartialView("Items", model);
        }

        [OutputCache(Duration = 3600, VaryByParam = "City ; Page", SqlDependency = "shedule:News")]
        [Authorize(Roles = "admin")]
        public ActionResult GetAdminItems(string City, int Page = 1)
        {
            int? cityId = (int?)Session["City"];
            var model = repository.GetNewsInCity(cityId)
                .OrderByDescending(x => x.Time)
                .Skip((Page * ItemsOnPage) - ItemsOnPage)
                .Take(ItemsOnPage);
                return PartialView("AdminItems", model);
        }



        [Authorize(Roles = "admin")]
        public ActionResult List()
        {
            int? cityId = (int?)Session["City"];
            if(cityId == null)
            {
                if(citiesRepository.Cities.Any())
                {
                    cityId = citiesRepository.Cities.First().Id;
                }
            }
            var model = repository.GetNewsInCity(cityId).Any();
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Add()
        {
            return View(new NewsViewModel());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Add(NewsViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            int? cityId = (int?)Session["City"];
            if (repository.Add(model.Title, model.Text, DateTime.Now, cityId))
            {
                TempData["Success"] = "Новость добавлена";
            }
            else
            {
                TempData["Errors"] = "Что-то пошло не так";
            }

            return View(new NewsViewModel());
        }


        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? Id)
        {
            if(Id == null)
            {
                return RedirectToAction("List", "News");
            }
            var model = new NewsViewModel();
            var item = repository.News.Where(x => x.Id == Id);
            if (item.Any())
            {
                var news = item.First();
                model.Id = news.Id;
                model.Title = news.Title;
                model.Text = news.Text;
            }
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(NewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Time = DateTime.Now;
            if (repository.Update(model.Title, model.Text, DateTime.Now, model.Id))
            {
                TempData["Success"] = "Новость обновлена";
            }
            else
            {
                TempData["Errors"] = "Что-то пошло не так";
            }
            return View(model);
        }


        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public ActionResult Delete(int Id)
        {
            if (repository.Delete(Id))
            {
                TempData["Success"] = "Запись удалена";
            }
            else
            {
                TempData["Errors"] = "Что-то пошло не так";
            }
            return RedirectToAction("List", "News");
        }



    }
}