using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var model = citiesRepository.GetCitiesName();
            if (Session["City"] == null)
            {
            Session["City"] = citiesRepository.Cities.FirstOrDefault().Id;
            }
            else
            {

            }
            return PartialView("_CitiesIndex",model);
        }

        public ActionResult SetCity(string city, string returnUrl)
        {
            var result = citiesRepository.Cities.Where(x => x.Name == city);
            if(result.Any())
            {
                Session["City"] = result.First().Id;
            }

            if(string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home");
            }
            return Redirect(returnUrl);
        }

    }
}