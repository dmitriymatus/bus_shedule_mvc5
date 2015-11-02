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
    [OutputCache(Duration = 3600, SqlDependency = "shedule:Shedules")]
    public class HomeController : Controller
    {
        IUserRoutesRepository routesRepository;
        ISheduleRepository sheduleRepository;
        
        public HomeController(ISheduleRepository _sheduleRepository, IUserRoutesRepository _routesRepository)
        {
            sheduleRepository = _sheduleRepository;
            routesRepository = _routesRepository;
        }


        [OutputCache(Duration = 2,NoStore = false)]
        public ActionResult Index()
        {
            ViewBag.HasUserRoutes = routesRepository.Routes.Where(x => x.UserName == User.Identity.Name).Any();
            return View();
        }

        //----------------------------------------------------------------------------------------
        
        [OutputCache(Duration = 60, VaryByParam = "city")]
        public JsonResult GetBuses(string city)
        {
            int? cityId = (int?)Session["City"];
            City City = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            var buses = City.Buses.Select(x => x.Number);
            return Json(buses, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 60, VaryByParam = "busNumber")]
        public JsonResult GetStopsNames(string busNumber)
        {
            int cityId = (int)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            var result = city.Buses.FirstOrDefault(x => x.Number == busNumber).BusStops.Select(x => x.Name);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 60, VaryByParam = "stopName ; busNumber")]
        public JsonResult GetFinalStops(string stopName, string busNumber)
        {
            int cityId = (int)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            Bus bus = city.Buses.FirstOrDefault(x => x.Number == busNumber);
            BusStop stop = bus.BusStops.FirstOrDefault(x => x.Name == stopName);

            var result = sheduleRepository.
                Shedule.
                Where(x => x.City == city && x.Bus == bus && x.BusStop == stop).
                Select(x => x.Direction.Name).
                Distinct();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 60, VaryByParam = "stopName ; busNumber ; endStop")]
        public JsonResult GetDays(string stopName, string busNumber, string endStop)
        {
            int cityId = (int)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            var result = sheduleRepository.
                 Shedule.
                 Where(x => x.City == city && x.Bus.Number == busNumber && x.BusStop.Name == stopName && x.Direction.Name == endStop).
                 Select(x => x.Days.Name).
                 Distinct();
            var now = Application.Infrastructure.Days.GetDays(result);

            var model = new { result = result, now = now };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 60, VaryByParam = "busNumber ; stopName ; endStopName; days")]
        public JsonResult GetStops(string busNumber, string stopName, string endStopName, string days)
        {
            int cityId = (int)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            var result = sheduleRepository.
                 Shedule.
                 First(x => x.City == city && x.Bus.Number == busNumber && x.BusStop.Name == stopName && x.Direction.Name == endStopName && x.Days.Name == days).
                 Items;
            var nearestTime = Stops.GetNearestTime(result);

            var model = new { stops = result.Split(' '), nearestStop = nearestTime };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 60, VaryByParam = "stopName ; busNumber")]
        public JsonResult GetOtherBuses(string stopName, string busNumber)
        {
            int cityId = (int)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            var result = city.
                BusStops.
                First(x => x.Name == stopName).
                Buses.
                Where(x => x.Number != busNumber).
                Select(x => x.Number);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //-------------------------------------------------------------------------------------------------

    }
}