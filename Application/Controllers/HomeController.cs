using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Application.Infrastructure;

namespace Application.Controllers
{
  //  [OutputCache(Duration = 3600, SqlDependency = "shedule:BusStops")]
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
            var buses = sheduleRepository.Buses.Where(x => x.City.Id == cityId);

            return Json(buses, JsonRequestBehavior.AllowGet);
        }

        //[OutputCache(Duration = 60, VaryByParam = "busNumber")]
        //public JsonResult GetStopsNames(string busNumber)
        //{
        //    int cityId = (int)Session["City"];
        //    var result = repository.GetStops(busNumber, cityId);

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //[OutputCache(Duration = 60, VaryByParam = "stopName ; busNumber")]
        //public JsonResult GetFinalStops(string stopName, string busNumber)
        //{
        //    int cityId = (int)Session["City"];
        //    var result = repository.GetFinalStops(stopName, busNumber, cityId);

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //[OutputCache(Duration = 60, VaryByParam = "stopName ; busNumber ; endStop")]
        //public JsonResult GetDays(string stopName, string busNumber, string endStop)
        //{
        //    int cityId = (int)Session["City"];
        //    var result = repository.GetDays(stopName, busNumber, endStop, cityId);
        //    var now = Days.GetDays(result);

        //    var model = new { result = result, now = now };
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        //[OutputCache(Duration = 60, VaryByParam = "busNumber ; stopName ; endStopName; days")]
        //public JsonResult GetStops(string busNumber, string stopName, string endStopName, string days)
        //{
        //    int cityId = (int)Session["City"];
        //    var result = repository.GetItems(stopName, busNumber, endStopName, days, cityId);
        //    var nearestTime = Stops.GetNearestTime(result);

        //    var model = new {stops = result, nearestStop = nearestTime };
        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}

        //[OutputCache(Duration = 60, VaryByParam = "stopName ; busNumber")]
        //public JsonResult GetOtherBuses(string stopName, string busNumber)
        //{
        //    int cityId = (int)Session["City"];
        //    var result = repository.GetOtherBuses(stopName, busNumber, cityId);

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //-------------------------------------------------------------------------------------------------

    }
}