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

namespace Application.Controllers
{
    [Authorize(Roles = "admin")]
    // [OutputCache(Duration = 3600, SqlDependency = "shedule:BusStops")]
    public class AdminController : Controller
    {
        ISheduleRepository sheduleRepository;
        ICitiesRepository citiesRepository;
        public AdminController(ISheduleRepository _sheduleRepository, ICitiesRepository _citiesRepository)
        {
            sheduleRepository = _sheduleRepository;
            citiesRepository = _citiesRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            var assembly = Assembly.Load("Domain");
            var parser = assembly.GetType("Domain.SheduleParsers.Abstract.ISheduleParser");
            var parsers = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(parser)).Select(x => x.Name);

            AddFileViewModel model = new AddFileViewModel { Parsers = parsers };

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
                    // City city = citiesRepository.Cities.FirstOrDefault(x => x.Id == cityId);
                    City city = sheduleRepository.Cities.FirstOrDefault(x => x.Id == cityId);
                    var fileName = this.HttpContext.Request.MapPath("~/Content/shedule" + cityId + ".xls");
                    model.file.SaveAs(fileName);

                    var assembly = Assembly.Load("Domain");
                    var parserInterface = assembly.GetType("Domain.SheduleParsers.Abstract.ISheduleParser");
                    var parserType = assembly.GetTypes().FirstOrDefault(x => x.GetInterfaces().Contains(parserInterface) && x.Name == model.Parser);

                    ISheduleParser parser = Activator.CreateInstance(parserType) as ISheduleParser;

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

        //[HttpGet]
        //public ActionResult AddStop()
        //{
        //    int? cityId = (int?)Session["City"];
        //    var city = citiesRepository.Cities.FirstOrDefault(x => x.Id == cityId);
        //    var model = new AdminAddViewModel
        //    {
        //        Numbers = city.Buses.Select(x => x.Number),
        //        Stop = new BusStopViewModel(),
        //        StopNames = city.BusStops.Select(x => x.Name),
        //        FinalStops = sheduleRepository.Shedule.Where(x => x.City == city).Select(x => x.EndStop.Name).Distinct(),
        //        Days = sheduleRepository.Shedule.Where(x => x.City == city).Select(x => x.Days.Name).Distinct()
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult AddStop(BusStopViewModel stop)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // var model = CreateViewModel();
        //        // model.Stop = stop;
        //        // return View(model);
        //    }
        //    int? city = (int?)Session["City"];
        //    //if (repository.Contain(stop.busNumber, stop.stopName, stop.finalStop, stop.days, city))
        //    //{
        //    //    ModelState.AddModelError("", "Запись уже существует");
        //    //    var model = CreateViewModel();
        //    //    model.Stop = stop;
        //    //    return View(model);
        //    //}
        //    Shedule item = new Shedule
        //    {


        //    };
        //    //repository.AddStop(stop.busNumber, stop.stopName, stop.finalStop, stop.days, city);
        //    TempData["Success"] = "Запись добавлена";
        //    return RedirectToAction("AddStop");
        //}

        //[HttpGet]
        //public ActionResult Edit()
        //{
        //    int? city = (int?)Session["City"];
        //    var buses = repository.GetBuses(city);
        //    return View(buses);
        //}


        //[HttpPost]
        //[OutputCache(Duration = 60, NoStore = false)]
        //public ActionResult Edit(BusStopViewModel stop)
        //{
        //    int? city = (int?)Session["City"];
        //    if (repository.Update(stop.busNumber, stop.stopName, stop.finalStop, stop.days, stop.stops, city))
        //        TempData["Success"] = "Запись обновлена";
        //    else
        //        TempData["Erors"] = "Запись не обновлена";
        //    return RedirectToAction("Edit");
        //}

        //[HttpGet]
        //public ActionResult Delete()
        //{
        //    int? city = (int?)Session["City"];
        //    var buses = repository.GetBuses(city);
        //    return View(buses);
        //}

        //[HttpDelete]
        //[ValidateAntiForgeryToken]
        //[OutputCache(Duration = 60, NoStore = false)]
        //public ActionResult Delete(BusStopViewModel stop)
        //{
        //    int? city = (int?)Session["City"];
        //    if (repository.Delete(stop.busNumber, stop.stopName, stop.finalStop, stop.days, city))
        //        TempData["Success"] = "Запись удалена";
        //    return RedirectToAction("Delete");
        //}


        //[HttpDelete]
        //[ValidateAntiForgeryToken]
        //[OutputCache(Duration = 60, NoStore = false)]
        //public ActionResult DeleteAll()
        //{
        //    int? city = (int?)Session["City"];
        //    repository.DeleteAll(city);
        //    TempData["Success"] = "Записи удалены";
        //    return RedirectToAction("Index");
        //}

        //private AdminAddViewModel CreateViewModel()
        //{
        //    var numbers = repository.Stops.Select(x => x.BusNumber).Distinct();
        //    var stopNames = repository.Stops.Select(x => x.StopName).Distinct().OrderBy(x => x);
        //    var finalStops = repository.Stops.Select(x => x.FinalStop).Distinct().OrderBy(x => x);
        //    var days = repository.Stops.Select(x => x.Days).Distinct().OrderBy(x => x);
        //    return new AdminAddViewModel
        //    {
        //        Numbers = numbers,
        //        StopNames = stopNames,
        //        Days = days,
        //        FinalStops = finalStops,
        //        Stop = new BusStopViewModel()
        //    };
        //}


    }
}