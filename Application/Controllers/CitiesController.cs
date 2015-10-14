using Domain.Abstract;
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
            
            if (Session["City"] == null)
            {
            Session["City"] = citiesRepository.Cities.FirstOrDefault().Id;
            }
            else
            {
            }
            var model = new CitiesIndexViewModel { Cities = citiesRepository.GetCitiesName(), SelectedCity = citiesRepository.Cities.FirstOrDefault(x => x.Id == (int)Session["City"]).Name };
            return PartialView("_CitiesIndex",model);
        }
        [OutputCache(Duration = 1, NoStore =false)]
        public void SetCity(string city)
        {
            var result = citiesRepository.Cities.Where(x => x.Name == city);
            if(result.Any())
            {
                Session["City"] = result.First().Id;
            }
        }

    }
}