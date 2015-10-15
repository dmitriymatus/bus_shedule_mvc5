using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;

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
            //int? cityId = (int?)Session["City"];
            var city = citiesRepository.Cities.FirstOrDefault();
            var model = repository.GetNewsInCity(city.Id).Any();
            return PartialView(model);
        }

        public ActionResult GetItems(string City, int Page = 1)
        {
            int? cityId = (int?)Session["City"];
            //var city = citiesRepository.Cities.FirstOrDefault();
            var model = repository.GetNewsInCity(cityId)
                .OrderBy(x=>x.Time)
                .Skip((Page * ItemsOnPage) - ItemsOnPage)
                .Take(ItemsOnPage);
            return PartialView("Items",model);
        }
    }
}