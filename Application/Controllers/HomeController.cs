using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Application.Infrastructure;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        IStopsRepository repository;
        public HomeController(IStopsRepository _repository)
        {
            repository = _repository;
        }

        public ActionResult Index()
        {
            var buses = repository.GetBuses();

            return View(buses);
        }

        [OutputCache(Duration = 1, NoStore = true)]
        public JsonResult GetStopsNames(string busNumber)
        {
            var result = repository.GetStops(busNumber);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 1, NoStore = true)]
        public JsonResult GetFinalStops(string stopName, string busNumber)
        {
            var result = repository.GetFinalStops(stopName, busNumber);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 1, NoStore = true)]
        public JsonResult GetDays(string stopName, string busNumber, string endStop)
        {
            var result = repository.GetDays(stopName, busNumber, endStop);
            var now = Days.GetDays(result);

            var model = new { result = result, now = now };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 1, NoStore = true)]
        public JsonResult GetStops(string busNumber, string stopName, string endStopName, string days)
        {
            var result = repository.GetItems(stopName, busNumber, endStopName, days);
            var nearestTime = Stops.GetNearestTime(result);

            var model = new { stops = result, nearestStop = nearestTime };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOtherBuses(string stopName, string busNumber)
        {

            var result = repository.GetOtherBuses(stopName, busNumber);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}