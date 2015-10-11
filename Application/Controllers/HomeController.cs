using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Application.Infrastructure;

namespace Application.Controllers
{
    [OutputCache(Duration = 3600, SqlDependency = "shedule:BusStops")]
    public class HomeController : Controller
    {
        IUserRoutesRepository routesRepository;
        IStopsRepository repository;
        public HomeController(IStopsRepository _repository, IUserRoutesRepository _routesRepository)
        {
            repository = _repository;
            routesRepository = _routesRepository;
        }

        public ActionResult Index()
        {
            ViewBag.HasUserRoutes = routesRepository.Routes.Where(x => x.UserName == User.Identity.Name).Any();
            return View(repository.GetBuses());
        }

        //----------------------------------------------------------------------------------------

        public JsonResult GetBuses()
        {
            var buses = repository.GetBuses();

            return Json(buses, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStopsNames(string busNumber)
        {
            var result = repository.GetStops(busNumber);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFinalStops(string stopName, string busNumber)
        {
            var result = repository.GetFinalStops(stopName, busNumber);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDays(string stopName, string busNumber, string endStop)
        {
            var result = repository.GetDays(stopName, busNumber, endStop);
            var now = Days.GetDays(result);

            var model = new { result = result, now = now };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStops(string busNumber, string stopName, string endStopName, string days)
        {
            var result = repository.GetItems(stopName, busNumber, endStopName, days);
            var nearestTime = Stops.GetNearestTime(result);

            var model = new {stops = result, nearestStop = nearestTime };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOtherBuses(string stopName, string busNumber)
        {
            var result = repository.GetOtherBuses(stopName, busNumber);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //-------------------------------------------------------------------------------------------------

    }
}