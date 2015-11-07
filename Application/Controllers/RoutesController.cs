using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Models;
using Application.Models.Routes;
using Application.Infrastructure;

namespace Application.Controllers
{
    [Authorize]
    public class RoutesController : Controller
    {
        IUserRoutesRepository repository;
        ISheduleRepository sheduleRepository;

        public RoutesController(IUserRoutesRepository _repository, ISheduleRepository _sheduleRepository)
        {
            repository = _repository;
            sheduleRepository = _sheduleRepository; 
        }

        [OutputCache(Duration = 1, NoStore = false)]
        public JsonResult Index()
        {
            int? city = (int?)Session["City"];
            var model = repository.Routes.Where(x => x.UserName == User.Identity.Name && x.City.Id == city)
                                         .Select(x => x.Name)
                                         .Distinct();
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Add()
        {
            int? cityId = (int?)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            ViewBag.Buses = city.Buses.Select(x => x.Number);
            return View(new RouteAddViewModel());
        }

        [HttpPost]
        public ActionResult Add(RouteAddViewModel userRoute)
        {
            if (!ModelState.IsValid)
            {
                return View(userRoute);
            }
            int cityId = (int)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            UserRoute route = new UserRoute
            {
                UserName = User.Identity.Name,
                Bus = city.Buses.FirstOrDefault(x => x.Number == userRoute.BusNumber),
                City = city,
                Stop = city.BusStops.FirstOrDefault(x => x.Name == userRoute.Stop),
                Name = userRoute.Name,
                Direction = sheduleRepository.Directions.FirstOrDefault(x => x.Name == userRoute.EndStop && x.Bus.City == city)
            }; 
            repository.AddRoute(route);
            TempData["result"] = "Запись добавлена";

            return RedirectToAction("Add");
        }


        [HttpGet]
        public ActionResult Edit()
        {
            int? city = (int?)Session["City"];
            var model = repository.Routes.Where(x => x.UserName == User.Identity.Name && x.City.Id == city);
            return View(model);
        }



        public ActionResult SelectRoutes(string Name)
        {
            int city = (int)Session["City"];
            var routes = repository.Routes.Where(x => x.Name == Name && x.UserName == User.Identity.Name && x.City.Id == city);
            var model = new List<RoutesViewModel>();

            foreach (var route in routes)
            {
                var allDays = sheduleRepository.Shedule.Where(x => x.Bus.Id == route.Bus.Id && x.BusStop.Id == route.Stop.Id && x.Direction.Id == route.Direction.Id).Select(x => x.Days.Name);
                var day = Application.Infrastructure.Days.GetDays(allDays);
                var result = sheduleRepository.Shedule.FirstOrDefault(x => x.Bus.Id == route.Bus.Id && x.BusStop.Id == route.Stop.Id && x.Direction.Id == route.Direction.Id && x.Days.Name == day).Items;
                var nearestTime = Stops.GetNearestTime(result);
                model.Add(new RoutesViewModel()
                {
                    Name = route.Name,
                    BusNumber = route.Bus.Number,
                    Stop = route.Stop.Name,
                    NearestBus = nearestTime
                });
            }
            return PartialView("_SelectRoutes", model);
        }


        public ActionResult Route(int Id)
        {
            ViewBag.Id = Id;
            var model = repository.Routes.FirstOrDefault(x => x.Id == Id);
            if(model == null)
            {
                return RedirectToAction("Edit","Routes");
            }


            int city = (int)Session["City"];
            var Buses = sheduleRepository.Buses.Where(x => x.City.Id == city).Select(x => x.Number);
            var Stops = sheduleRepository.Buses.FirstOrDefault(x => x.Id == model.Bus.Id).BusStops.Select(x => x.Name);
            var FinalStops = sheduleRepository.Buses.FirstOrDefault(x => x.Id == model.Bus.Id).Directions.Select(x => x.Name);
            var Days = sheduleRepository.Buses.FirstOrDefault(x => x.Id == model.Bus.Id).Days.Select(x => x.Name);

            var result = new RoutesEditViewModel()
            {
                BusNumber = model.Bus.Number,
                Stop = model.Stop.Name,
                Name = model.Name,
                EndStop = model.Direction.Name,
                Buses = Buses,
                Stops = Stops,
                EndStops = FinalStops,
            };
            return View(result);
        }


        [HttpPost]
        public ActionResult SaveChanges(int Id, RoutesEditViewModel model)
        {
            int city = (int)Session["City"];
            if (!ModelState.IsValid)
            {
                model.Buses = sheduleRepository.Buses.Where(x => x.City.Id == city).Select(x => x.Number);
                model.Stops = sheduleRepository.Buses.FirstOrDefault(x => x.City.Id == city && x.Number == model.BusNumber).BusStops.Select(x => x.Name);
                model.EndStops = sheduleRepository.Buses.FirstOrDefault(x => x.City.Id == city && x.Number == model.BusNumber).Directions.Select(x => x.Name);
                ViewBag.Id = Id;
                return View("Route", model);
            }

            var route = repository.Routes.First(x => x.Id == Id);
            route.Name = model.Name;
            route.Bus = sheduleRepository.Buses.FirstOrDefault(x => x.City.Id == city && x.Number == model.BusNumber);
            route.Stop = sheduleRepository.Buses.FirstOrDefault(x => x.City.Id == city && x.Number == model.BusNumber).BusStops.FirstOrDefault(x => x.Name == model.Stop);
            route.Direction = sheduleRepository.Buses.FirstOrDefault(x => x.City.Id == city && x.Number == model.BusNumber).Directions.FirstOrDefault(x => x.Name == model.EndStop);
            route.City = sheduleRepository.Cities.FirstOrDefault(x => x.Id == city);
            repository.UpdateRoute(route);
            TempData["result"] = "Запись обновлена";
            return RedirectToAction("Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? Id)
        {
            if (Id != null)
            {
                var route = repository.Routes.FirstOrDefault(x => x.Id == Id);
                if (route.UserName == User.Identity.Name)
                {
                    repository.DeleteRoute(route);
                    TempData["result"] = "Запись удалена";
                }
            }
            return RedirectToAction("Edit", "Routes");
        }

    }
}