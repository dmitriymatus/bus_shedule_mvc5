//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Domain.Abstract;
//using Domain.Models;
//using Application.Models.News;

//namespace Application.Controllers
//{
//    public class NewsController : Controller
//    {
//        private const int ItemsOnPage = 5;
//        INewsRepository repository;
//        ISheduleRepository sheduleRepository;
//        public NewsController(INewsRepository _repository, ISheduleRepository _sheduleRepository)
//        {
//            repository = _repository;
//            sheduleRepository = _sheduleRepository;
//        }

//        public ActionResult Index()
//        {
//            var city = sheduleRepository.Cities.FirstOrDefault();
//            var model = city.News.Any();
//            return PartialView(model);
//        }

//        [OutputCache(Duration = 3600, VaryByParam = "City ; Page", SqlDependency = "shedule:News")]
//        public ActionResult GetItems(string City, int Page = 1)
//        {
//            int? cityId = (int?)Session["City"];
//            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
//            var model = city.News
//                .OrderByDescending(x => x.Time)
//                .Skip((Page * ItemsOnPage) - ItemsOnPage)
//                .Take(ItemsOnPage);
//            return PartialView("Items", model);
//        }

//        [Authorize(Roles = "admin")]
//        public ActionResult List()
//        {
//            int? cityId = (int?)Session["City"];
//            if (cityId == null)
//            {
//                if (sheduleRepository.Cities.Any())
//                {
//                    cityId = sheduleRepository.Cities.First().Id;
//                }
//            }
//            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
//            var model = city.News.Any();
//            return View(model);
//        }

//        [Authorize(Roles = "admin")]
//        public ActionResult Add()
//        {
//            return View(new NewsViewModel());
//        }

//        [Authorize(Roles = "admin")]
//        [HttpPost]
//        public ActionResult Add(NewsViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }
//            int? cityId = (int?)Session["City"];
//            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
//            News news = new News { Title = model.Title, Text = model.Text, Time = DateTime.Now, City = city };
//            repository.Add(news);

//            return View(new NewsViewModel());
//        }


//        [Authorize(Roles = "admin")]
//        public ActionResult Edit(int? Id)
//        {
//            if (Id == null)
//            {
//                return RedirectToAction("List", "News");
//            }
//            var model = new NewsViewModel();
//            var item = repository.News.Where(x => x.Id == Id);
//            if (item.Any())
//            {
//                var news = item.First();
//                model.Id = news.Id;
//                model.Title = news.Title;
//                model.Text = news.Text;
//            }
//            return View(model);
//        }


//        [Authorize(Roles = "admin")]
//        [HttpPost]
//        public ActionResult Edit(NewsViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            model.Time = DateTime.Now;
//            repository.Update(model.Title, model.Text, DateTime.Now, model.Id);
//            return View(model);
//        }


//        [Authorize(Roles = "admin")]
//        [ValidateAntiForgeryToken]
//        [HttpDelete]
//        public ActionResult Delete(int? Id)
//        {
//            if (Id != null)
//            {
//                repository.Delete((int)Id);
//                TempData["Success"] = "Запись удалена";
//            }
//            return RedirectToAction("List", "News");
//        }



//    }
//}