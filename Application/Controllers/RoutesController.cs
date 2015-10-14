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
        IStopsRepository stopsRepository;

        public RoutesController(IUserRoutesRepository _repository, IStopsRepository _stopsRepository)
        {
            repository = _repository;
            stopsRepository = _stopsRepository; 
        }

        public JsonResult Index()
        {
            var model = repository.Routes.Where(x => x.UserName == User.Identity.Name)
                                         .Select(x => x.Name)
                                         .Distinct();
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Add()
        {
            int city = (int)Session["City"];
            ViewBag.Buses = stopsRepository.GetBuses(city);
            return View(new RouteAddViewModel());
        }

        [HttpPost]
        public ActionResult Add(RouteAddViewModel userRoute)
        {
            if (!ModelState.IsValid)
            {
                return View(userRoute);
            }
            repository.AddRoute(User.Identity.Name, userRoute.BusNumber, userRoute.Name, userRoute.Stop, userRoute.EndStop);
            TempData["result"] = "Запись добавлена";

            return RedirectToAction("Add");
        }


        [HttpGet]
        public ActionResult Edit()
        {
            var model = repository.Routes.Where(x => x.UserName == User.Identity.Name);
            return View(model);
        }
    


        public ActionResult SelectRoutes(string Name)
        {
            var routes = repository.Routes.Where(x => x.Name == Name.Trim(' ') && x.UserName == User.Identity.Name);
            var model = new List<RoutesViewModel>();

            foreach(var route in routes)
            {               
                var allDays = stopsRepository.GetDays(route.Stop, route.BusNumber, route.EndStop);
                var result = stopsRepository.GetItems(route.Stop, route.BusNumber, route.EndStop, Days.GetDays(allDays));
                var nearestTime = Stops.GetNearestTime(result);
                model.Add(new RoutesViewModel()
                {
                    Name = route.Name,
                    BusNumber = route.BusNumber,
                    Stop = route.Stop,
                    NearestBus = nearestTime
                });
            }
            return PartialView("_SelectRoutes", model);
        }


        public ActionResult Route(int Id)
        {
            ViewBag.Id = Id;
            var model = repository.Routes.Where(x => x.Id == Id).FirstOrDefault();


            int city = (int)Session["City"];
            var Buses = stopsRepository.GetBuses(city);
            var Stops = stopsRepository.GetStops(model.BusNumber);
            var FinalStops = stopsRepository.GetFinalStops(model.Stop, model.BusNumber);
            var Days = stopsRepository.GetDays(model.Stop, model.BusNumber, model.EndStop);

            var result = new RoutesEditViewModel()
            {
                BusNumber = model.BusNumber,
                Stop = model.Stop,
                Name = model.Name,
                EndStop = model.EndStop,
                Buses = Buses,
                Stops = Stops,
                EndStops = FinalStops,
            };
            return View(result);
        }


        [HttpPost]
        public ActionResult SaveChanges(int Id, RoutesEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                int city = (int)Session["City"];
                model.Buses = stopsRepository.GetBuses(city);
                model.Stops = stopsRepository.GetStops(model.BusNumber);
                model.EndStops = stopsRepository.GetFinalStops(model.Stop, model.BusNumber);
                ViewBag.Id = Id;                
                return View("Route", model);
            }

            repository.UpdateRoute(Id, model.Name, model.BusNumber, model.Stop, model.EndStop);
            TempData["result"] = "Запись обновлена";
            return RedirectToAction("Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? Id)
        {
            if (Id != null)
            {
                var route = repository.Routes.Where(x => x.Id == Id).FirstOrDefault();
                if(route.UserName == User.Identity.Name)
                { 
                repository.Delete((int)Id);
                TempData["result"] = "Запись удалена";
                }
            }            
            return RedirectToAction("Edit","Routes");
        }

    }
}