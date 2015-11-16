using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Models;
using System.Data.Entity;
using Application.Infrastructure;

namespace Application.Controllers
{
    // [OutputCache(Duration = 3600, SqlDependency = "shedule:Shedules")]
    public class HomeController : Controller
    {
        IRepository<UserRoute> routesRepository;
        IRepository<City> citiesRepository;
        IRepository<TimeTable> timeTablesRepository;
        IRepository<Shedule> shedulesRepository;

        public HomeController(IRepository<UserRoute> _routesRepository,
                                IRepository<City> _citiesRepository,
                                IRepository<TimeTable> _timeTablesRepository,
                                IRepository<Shedule> _shedulesRepository)
        {
            citiesRepository = _citiesRepository;
            routesRepository = _routesRepository;
            timeTablesRepository = _timeTablesRepository;
            shedulesRepository = _shedulesRepository;
        }

        // [OutputCache(Duration = 2,NoStore = false)]
        public ActionResult Index()
        {
            ViewBag.HasUserRoutes = routesRepository.Get(x => x.UserName == User.Identity.Name).Any();
            return View();
        }

        //----------------------------------------------------------------------------------------

        //  [OutputCache(Duration = 60, VaryByParam = "city")]
        public JsonResult GetBuses(string city)
        {
            int? cityId = (int?)Session["City"];
            City City = citiesRepository.GetByID(cityId);
            var buses = City.Buses.Select(x => x.Number);
            return Json(buses, JsonRequestBehavior.AllowGet);
        }

        //  [OutputCache(Duration = 60, VaryByParam = "busNumber")]
        public JsonResult GetStopsNames(string busNumber)
        {
            int cityId = (int)Session["City"];
            City city = citiesRepository.GetByID(cityId);
            var bus = city.Buses.FirstOrDefault(x => x.Number == busNumber);

            var result = timeTablesRepository
                        .Get(x => x.Bus.Id == bus.Id)
                        .Select(x => x.Stop.Name)
                        .Distinct();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //  [OutputCache(Duration = 60, VaryByParam = "stopName ; busNumber")]
        public JsonResult GetFinalStops(string stopName, string busNumber)
        {
            int cityId = (int)Session["City"];
            City city = citiesRepository.GetByID(cityId);
            var bus = city.Buses.FirstOrDefault(x => x.Number == busNumber);
            var stop = city.Stops.FirstOrDefault(x => x.Name == stopName);

            var result = timeTablesRepository
                        .Get(x => x.Bus.Id == bus.Id && x.Stop.Id == stop.Id)
                        .Select(x => x.FinalStop.Name)
                        .Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //   [OutputCache(Duration = 60, VaryByParam = "stopName ; busNumber ; endStop")]
        public JsonResult GetDays(string stopName, string busNumber, string endStop)
        {
            int cityId = (int)Session["City"];
            City city = citiesRepository.GetByID(cityId);
            var bus = city.Buses.FirstOrDefault(x => x.Number == busNumber);
            var stop = city.Stops.FirstOrDefault(x => x.Name == stopName);
            var finalStop = city.Stops.FirstOrDefault(x => x.Name == endStop);
            var result = timeTablesRepository
                        .Get(x => x.Bus.Id == bus.Id && x.Stop.Id == stop.Id && x.FinalStop.Id == finalStop.Id)
                        .FirstOrDefault()
                        .Shedules
                        .Select(x => x.Days.ToDescription())
                        .Distinct();

            var now = Application.Infrastructure.Days.GetDays(result);
            var model = new { result = result, now = now };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //    [OutputCache(Duration = 60, VaryByParam = "busNumber ; stopName ; endStopName; days")]
        public JsonResult GetStops(string busNumber, string stopName, string endStopName, string days)
        {
            int cityId = (int)Session["City"];
            City city = citiesRepository.GetByID(cityId);
            var bus = city.Buses.FirstOrDefault(x => x.Number == busNumber);
            var stop = city.Stops.FirstOrDefault(x => x.Name == stopName);
            var finalStop = city.Stops.FirstOrDefault(x => x.Name == endStopName);
            var result = timeTablesRepository
                        .Get(x => x.Bus.Id == bus.Id && x.Stop.Id == stop.Id && x.FinalStop.Id == finalStop.Id)
                        .FirstOrDefault()
                        .Shedules
                        .Where(x => x.Days.ToDescription() == days)
                        .Select(x => x.Time);

            var nearestTime = Stops.GetNearestTime(result);
            var model = new { stops = result.Select(x => x.ToString("hh\\:mm")), nearestStop = nearestTime };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //    [OutputCache(Duration = 60, VaryByParam = "stopName ; busNumber")]
        public JsonResult GetOtherBuses(string stopName, string busNumber)
        {
            int cityId = (int)Session["City"];
            City city = citiesRepository.GetByID(cityId);
            var result = city
                        .Stops
                        .First(x => x.Name == stopName)
                        .TimeTables
                        .Where(x => x.Bus.Number != busNumber)
                        .Select(x => x.Bus.Number)
                        .Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //-------------------------------------------------------------------------------------------------

    }
}