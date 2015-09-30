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
        List<UserRoute> examplelist = new List<UserRoute>();

        public RoutesController(IUserRoutesRepository _repository, IStopsRepository _stopsRepository)
        {
           // examplelist.Add(new UserRoute { Id = "1" , BusNumber = "19",Name = "Домой", UserName = "Admin", Stop = "АП", EndStop = "АП-Пригородный вокзал", Days = "Рабочие" });
            repository = _repository;
            stopsRepository = _stopsRepository; 
        }

        public ActionResult Index()
        {
            var model = repository.Routes.Where(x=> x.UserName == User.Identity.Name).Select(x => x.Name);
            return PartialView("_IndexPartial",new SelectList(model));
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Buses = stopsRepository.GetBuses();
            return View(new RouteAddViewModel());
        }

        [HttpPost]
        public ActionResult Add(RouteAddViewModel userRoute)
        {
            if(!ModelState.IsValid)
            {
                return View(userRoute);
            }
            repository.AddRoute(User.Identity.Name, userRoute.BusNumber, userRoute.Name, userRoute.Stop, userRoute.EndStop, userRoute.Days);
            TempData["result"] = "Запись добавлена";
            return RedirectToAction("Add");
        }


        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }
    


        public ActionResult SelectRoutes(string Name)
        {
            var routes = repository.Routes.Where(x => x.Name == Name.Trim(' '));
            var model = new List<RoutesViewModel>();

            foreach(var route in routes)
            {
                var result = stopsRepository.GetItems(route.Stop, route.BusNumber, route.EndStop, route.Days);
                var nearestTime = Stops.GetNearestTime(result);
                model.Add(new RoutesViewModel() { Name = route.Name, BusNumber = route.BusNumber, Stop = route.Stop, Days = route.Days, NearestBus = nearestTime });
            }

            return PartialView("_SelectRoutes",model);
        }


    }
}