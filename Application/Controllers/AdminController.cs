using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Application.Models.Admin;
using System.Text;
using System.Text.RegularExpressions;
using Application.Models;
using System.Reflection;
using Domain.Models;
using Domain.SheduleParsers.Abstract;
using Application.Infrastructure.SheduleParserFactory.Abstract;

namespace Application.Controllers
{
    [Authorize(Roles = "admin")]
    [OutputCache(Duration = 3600, SqlDependency = "shedule:Shedules")]
    public class AdminController : Controller
    {
        ISheduleRepository sheduleRepository;
        ISheduleParserFactory factory;
        public AdminController(ISheduleRepository _sheduleRepository, ISheduleParserFactory _factory)
        {
            sheduleRepository = _sheduleRepository;
            factory = _factory;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddFileViewModel model = new AddFileViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(AddFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int? cityId = (int?)Session["City"];
                    City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
                    var fileName = this.HttpContext.Request.MapPath("~/Content/shedule" + cityId + ".xls");
                    model.file.SaveAs(fileName);

                    ISheduleParser parser = factory.Create(city.Name.ToLower());
                    var shedule = parser.Parse(fileName, city);

                    sheduleRepository.AddSheduleRange(shedule);

                    TempData["Success"] = "Расписание добавлено";
                }
                catch
                {
                    TempData["Erors"] = "Ошибка при обработке файла, проверьте правильность файла";
                }
                return RedirectToAction("Add");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult AddStop()
        {
            int? cityId = (int?)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            var model = new AdminAddViewModel
            {
                Numbers = city.Buses.Select(x => x.Number),
                Stop = new BusStopViewModel(),
                StopNames = city.BusStops.Select(x => x.Name),
                FinalStops = sheduleRepository.Directions.Where(x => x.Bus.City == city).Select(x => x.Name).Distinct(),
                Days = sheduleRepository.Days.Select(x => x.Name).Distinct()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddStop(BusStopViewModel stop)
        {
            int? cityId = (int?)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);

            if (!ModelState.IsValid)
            {
                 var model = CreateViewModel(city);
                 model.Stop = stop;
                 return View(model);
            }

            Shedule item = new Shedule();
            if(sheduleRepository.Buses.Where(x => x.Number == stop.busNumber).Any())
            {
                item.Bus = sheduleRepository.Buses.First(x => x.Number == stop.busNumber);
            }
            else
            {
                item.Bus = new Bus { Number = stop.busNumber, City = city };
            }

            if (sheduleRepository.BusStops.Where(x => x.Name == stop.stopName).Any())
            {
                item.BusStop = sheduleRepository.BusStops.First(x => x.Name == stop.stopName);
                if(!item.BusStop.Buses.Contains(item.Bus))
                {
                    item.BusStop.Buses.Add(item.Bus);
                }
            }
            else
            {
                item.BusStop = new BusStop {  Name = stop.stopName, City = city, Buses = new List<Bus>() };
                item.BusStop.Buses.Add(item.Bus);
            }
            
            if (sheduleRepository.Directions.Where(x => x.Name == stop.finalStop && x.Bus == item.Bus).Any())
            {
                item.Direction = sheduleRepository.Directions.First(x => x.Name == stop.finalStop && x.Bus == item.Bus);
            }
            else
            {
                item.Direction = new Direction { Bus = item.Bus, Name = stop.finalStop };
            }

            if (sheduleRepository.Days.Where(x => x.Name == stop.days).Any())
            {
                item.Days = sheduleRepository.Days.First(x => x.Name == stop.days);
                if (!item.Days.Buses.Contains(item.Bus))
                {
                    item.Days.Buses.Add(item.Bus);
                }
            }
            else
            {
                item.Days = new Days { Name = stop.days, Buses = new List<Bus>() };
                item.Days.Buses.Add(item.Bus);
            }

            item.City = city;
            item.Items = stop.stops;

            if (sheduleRepository.Shedule.Where(x => x.Bus == item.Bus && x.BusStop == item.BusStop && x.City == item.City && x.Days == item.Days && x.Direction == item.Direction).Any())
            {
                ModelState.AddModelError("", "Запись уже существует");
                var model = CreateViewModel(city);
                model.Stop = stop;
                return View(model);
            }

            sheduleRepository.AddShedule(item);
            TempData["Success"] = "Запись добавлена";
            return RedirectToAction("AddStop");
        }

        [HttpGet]
        public ActionResult Edit()
        {
            int? cityId = (int?)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            
            var buses = city.Buses.Select(x => x.Number);
            return View(buses);
        }


        [HttpPost]
        [OutputCache(Duration = 60, NoStore = false)]
        public ActionResult Edit(BusStopViewModel stop)
        {
            int? cityId = (int?)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            Shedule item = sheduleRepository.Shedule.FirstOrDefault(x => x.Bus.Number == stop.busNumber && x.BusStop.Name == stop.stopName && x.Direction.Name == stop.finalStop && x.Days.Name == stop.days && x.City == city);
            if (sheduleRepository.UpdateShedule(item, stop.stops))
                TempData["Success"] = "Запись обновлена";
            else
                TempData["Erors"] = "Запись не обновлена";
            return RedirectToAction("Edit");
        }

        [HttpGet]
        public ActionResult Delete()
        {
            int? cityId = (int?)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            var buses = city.Buses.Select(x => x.Number);
            return View(buses);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 60, NoStore = false)]
        public ActionResult Delete(BusStopViewModel stop)
        {
            int? cityId = (int?)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            Shedule item = sheduleRepository.Shedule.FirstOrDefault(x => x.Bus.Number == stop.busNumber && x.BusStop.Name == stop.stopName && x.Direction.Name == stop.finalStop && x.Days.Name == stop.days && x.City == city);
            if (sheduleRepository.DeleteShedule(item))
            { 
                TempData["Success"] = "Запись удалена";
            }
            return RedirectToAction("Delete");
        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 60, NoStore = false)]
        public ActionResult DeleteAll()
        {
            int? cityId = (int?)Session["City"];
            City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
            if (sheduleRepository.DeleteAllShedule(city))
            {
                TempData["Success"] = "Записи удалены";
            }
            else
            {
                TempData["Erors"] = "Записи не удалены";
            }
            return RedirectToAction("Index");
        }

        private AdminAddViewModel CreateViewModel(City city)
        {
            var numbers = city.
                Buses.
                Select(x=>x.Number).
                Distinct();

            var stopNames = city.
                BusStops.
                Select(x => x.Name).
                Distinct();

            var finalStops = sheduleRepository.
                Directions.
                Where(x => x.Bus.City == city).
                Select(x => x.Name).
                Distinct();

            var days = sheduleRepository.
                Days.
                Select(x => x.Name).
                Distinct();

            return new AdminAddViewModel
            {
                Numbers = numbers,
                StopNames = stopNames,
                Days = days,
                FinalStops = finalStops,
                Stop = new BusStopViewModel()
            };
        }


    }
}