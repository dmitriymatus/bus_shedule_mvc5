using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Models;
using Application.Models.News;

namespace Application.Controllers
{
    public class NewsController : Controller
    {
        private const int ItemsOnPage = 5;
        IRepository<News> newsRepository;
        IRepository<City> cityRepository;

        public NewsController(IRepository<News> _newsRepository, IRepository<City> _cityRepository)
        {
            newsRepository = _newsRepository;
            cityRepository = _cityRepository;
        }

        public ActionResult Index()
        {
            int? cityId = (int?)Session["City"];
            if(cityId == null)
            {
                cityId = cityRepository.Get(null).FirstOrDefault().Id;
            }
            var model = newsRepository.Get(x => x.CityId == cityId).Any();
            return PartialView("_Index",model);
        }

        //[OutputCache(Duration = 3600, VaryByParam = "City ; Page", SqlDependency = "shedule:News")]
        public ActionResult GetItems(string City, int Page = 1)
        {
            int? cityId = (int?)Session["City"];
            var model = newsRepository.Get(x => x.CityId == cityId)
                .OrderByDescending(x => x.Time)
                .Skip((Page * ItemsOnPage) - ItemsOnPage)
                .Take(ItemsOnPage);
            return PartialView("Items", model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult List()
        {
            int? cityId = (int?)Session["City"];
            if (cityId == null)
            {
                cityId = cityRepository.Get(null).FirstOrDefault().Id;
            }
            var model = newsRepository.Get(x => x.CityId == cityId).Any();
            return View(model);
        }


        //=========================CRUD==========================//
        [Authorize(Roles = "admin")]
        public ActionResult Add()
        {
            return View(new NewsViewModel());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Add(NewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int cityId = (int)Session["City"];
            News news = new News { Title = model.Title, Text = model.Text, Time = DateTime.Now, CityId = cityId };
            try
            {
                newsRepository.Insert(news);
                TempData["Success"] = "Новость добавлена";

            }
            catch(Exception ex)
            {
                //log error
                ModelState.AddModelError("", "Ошибка при добавлении записи. Повторите попытку позже");
                return View(model);
            }

            return View(new NewsViewModel());
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? Id)
        {
            
            if (Id == null)
            {
                return RedirectToAction("List", "News");
            }
            var model = new NewsViewModel();
            var item = newsRepository.GetByID(Id);
            if(item != null)
            {
                return View(new NewsViewModel { Id = item.Id, Title = item.Title, Text = item.Text });
            }
            else
            {
                return RedirectToAction("List", "News");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(NewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var news = newsRepository.GetByID(model.Id);
            news.Time = DateTime.Now;
            news.Title = model.Title;
            news.Text = model.Text;

            try
            { 
                newsRepository.Update(news);
                TempData["Success"] = "Запись обновлена";
            }
            catch(Exception ex)
            {
                //log error
                ModelState.AddModelError("", "Ошибка при обновлении записи. Повторите попытку позже");
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public ActionResult Delete(int? Id)
        {
            if (Id != null)
            {
                newsRepository.Delete(Id);
                TempData["Success"] = "Запись удалена";
            }
            return RedirectToAction("List", "News");
        }

    }
}