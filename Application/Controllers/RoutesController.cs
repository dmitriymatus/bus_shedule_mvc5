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
        IRepository<UserRoute> routesRepository;
        IRepository<City> citiesRepository;
        IRepository<TimeTable> timeTablesRepository;

        public RoutesController(IRepository<UserRoute> _routesRepository, IRepository<City> _citiesRepository, IRepository<TimeTable> _timeTablesRepository)
        {
            routesRepository = _routesRepository;
            citiesRepository = _citiesRepository;
            timeTablesRepository = _timeTablesRepository;
        }

        public ActionResult Index()
        {
            var model = routesRepository.Get(x => x.UserName == User.Identity.Name).Any();
            return PartialView("_Index", model);
        }

        [HttpGet]
        public ActionResult List()
        {
            int? city = (int?)Session["City"];
            var model = routesRepository.Get(x => x.UserName == User.Identity.Name && x.TimeTable.Bus.CityId == city);
            return View(model);
        }

        [OutputCache(Duration = 1, NoStore = false)]
        public JsonResult GetRoutes()
        {
            int? city = (int?)Session["City"];
            var model = routesRepository.Get(x => x.UserName == User.Identity.Name && x.TimeTable.Bus.CityId == city)
                                         .Select(x => x.Name)
                                         .Distinct();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectRoutes(string Name)
        {
            int city = (int)Session["City"];
            var routes = routesRepository.Get(x => x.Name == Name && x.UserName == User.Identity.Name && x.TimeTable.Bus.CityId == city);
            var model = new List<RoutesViewModel>();

            foreach (var route in routes)
            {
                var allDays = route.TimeTable.Shedules.Select(x => x.Days.ToDescription());
                var day = Application.Infrastructure.Days.GetDays(allDays);
                var result = route.TimeTable.Shedules.Where(x => x.Days.ToDescription() == day).Select(x => x.Time);
                var nearestTime = Stops.GetNearestTime(result);
                model.Add(new RoutesViewModel()
                {
                    Name = route.Name,
                    BusNumber = route.TimeTable.Bus.Number,
                    Stop = route.TimeTable.Stop.Name,
                    NearestBus = nearestTime
                });
            }
            return PartialView("_SelectRoutes", model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            int? cityId = (int?)Session["City"];
            City city = citiesRepository.GetByID(cityId);
            var buses = city.Buses.Select(x => x.Number);
            return View(new RouteAddViewModel { Buses = buses, Stops = new List<string>(), FinalStops = new List<string>() });
        }

        [HttpPost]
        public ActionResult Add(RouteAddViewModel userRoute)
        {
            int cityId = (int)Session["City"];
            City city = citiesRepository.GetByID(cityId);
            if (!ModelState.IsValid)
            {
                var buses = city.Buses.Select(x => x.Number);
                return View(userRoute);
            }
            var bus = city.Buses.FirstOrDefault(x => x.Number == userRoute.BusNumber);
            var stop = city.Stops.FirstOrDefault(x => x.Name == userRoute.Stop);
            var finalStop = city.Stops.FirstOrDefault(x => x.Name == userRoute.EndStop);
            var timeTable = timeTablesRepository.Get(x => x.BusId == bus.Id && x.Stop.Id == stop.Id && x.FinalStop.Id == finalStop.Id).FirstOrDefault();
            userRoute.Buses = city.Buses.Select(x => x.Number);
            userRoute.Stops = timeTablesRepository.Get(x => x.BusId == bus.Id).Select(x => x.Stop.Name).Distinct();
            userRoute.FinalStops = timeTablesRepository.Get(x => x.BusId == bus.Id && x.Stop.Id == stop.Id).Select(x => x.FinalStop.Name);
            UserRoute route = new UserRoute
            {
                UserName = User.Identity.Name,
                Name = userRoute.Name,
                TimeTable = timeTable
            };
            if (routesRepository.Get(x => x.UserName == User.Identity.Name && x.TimeTable.BusId == bus.Id && x.TimeTable.Stop.Id == stop.Id && x.TimeTable.FinalStop.Id == finalStop.Id).Any())
            {
                ModelState.AddModelError("", "Такой маршрут уже существует");
                return View(userRoute);
            }
            try
            {
                routesRepository.Insert(route);
                TempData["Message"] = "Запись добавлена";
            }
            catch (Exception ex)
            {
                //Log the error
                ModelState.AddModelError("", "Невозможно добавить запись. Попробуйте повторить попытку позже");
                return View(userRoute);
            }

            return RedirectToAction("Add");
        }


        public ActionResult Edit(int Id)
        {
            var model = routesRepository.GetByID(Id);
            if (model == null)
            {
                return RedirectToAction("Edit", "Routes");
            }
            int cityId = (int)Session["City"];
            City city = citiesRepository.GetByID(cityId);
            var result = new RoutesEditViewModel()
            {
                Id = Id,
                BusNumber = model.TimeTable.Bus.Number,
                Stop = model.TimeTable.Stop.Name,
                Name = model.Name,
                EndStop = model.TimeTable.FinalStop.Name,
                Buses = city.Buses.Select(x => x.Number),
                Stops = timeTablesRepository.Get(x => x.BusId == model.TimeTable.BusId).Select(x => x.Stop.Name).Distinct(),
                EndStops = timeTablesRepository.Get(x => x.BusId == model.TimeTable.BusId && x.Stop.Id == model.TimeTable.Stop.Id).Select(x => x.FinalStop.Name)
            };
            return View(result);
        }


        [HttpPost]
        public ActionResult Edit(int Id, RoutesEditViewModel model)
        {
            int cityId = (int)Session["City"];
            City city = citiesRepository.GetByID(cityId);
            if (!ModelState.IsValid)
            {
                return View("Route", model);
            }
            var bus = city.Buses.FirstOrDefault(x => x.Number == model.BusNumber);
            var stop = city.Stops.FirstOrDefault(x => x.Name == model.Stop);
            var finalStop = city.Stops.FirstOrDefault(x => x.Name == model.EndStop);
            var newTimeTable = timeTablesRepository.Get(x => x.BusId == bus.Id && x.Stop.Id == stop.Id && x.FinalStop.Id == finalStop.Id).FirstOrDefault();

            var route = routesRepository.GetByID(Id);
            route.Name = model.Name;
            route.TimeTable = newTimeTable;
            route.TimeTableId = newTimeTable.Id;

            try
            {
                routesRepository.Update(route);
            }
            catch(Exception ex)
            {
                //log
                model.Buses = city.Buses.Select(x => x.Number);
                model.Stops = timeTablesRepository.Get(x => x.BusId == route.TimeTable.BusId).Select(x => x.Stop.Name).Distinct();
                model.EndStops = timeTablesRepository.Get(x => x.BusId == route.TimeTable.BusId && x.Stop.Id == route.TimeTable.Stop.Id).Select(x => x.FinalStop.Name);
                ModelState.AddModelError("", "Невозможно обновить запись. Попробуйте повторить попытку позже");
                return View("Route", model);
            }

            TempData["result"] = "Запись обновлена";
            return RedirectToAction("Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? Id)
        {
            if (Id != null)
            {
                var route = routesRepository.GetByID(Id);
                if (route != null && route.UserName == User.Identity.Name)
                {
                    routesRepository.Delete(route);
                    TempData["result"] = "Запись удалена";
                }
            }
            return RedirectToAction("List", "Routes");
        }

    }
}