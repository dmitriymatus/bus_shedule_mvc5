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
    //[OutputCache(Duration = 3600, SqlDependency = "shedule:Shedules")]
    public class AdminController : Controller
    {
        private ISheduleParserFactory factory;
        private IRepository<Bus> busRepository;
        private IRepository<Stop> stopsRepository;
        private IRepository<TimeTable> timeTablesRepository;
        private IRepository<Shedule> shedulesRepository;
        private IRepository<City> citiesRepository;

        public AdminController(IRepository<Bus> _busRepository,
                               IRepository<Stop> _stopsRepository,
                               IRepository<TimeTable> _timeTablesRepository,
                               IRepository<Shedule> _shedulesRepository,
                               IRepository<City> _citiesRepository,
                               ISheduleParserFactory _factory)
        {
            busRepository = _busRepository;
            stopsRepository = _stopsRepository;
            timeTablesRepository = _timeTablesRepository;
            shedulesRepository = _shedulesRepository;
            citiesRepository = _citiesRepository;
            factory = _factory;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddFromFile()
        {
            AddFileViewModel model = new AddFileViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddFromFile(AddFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int cityId = (int)Session["City"];
                    var city = citiesRepository.GetByID(cityId);
                    var fileName = this.HttpContext.Request.MapPath("~/Content/shedule" + cityId + ".xls");
                    model.file.SaveAs(fileName);

                    ISheduleParser parser = factory.Create(city.Name.ToLower());
                    var shedule = parser.Parse(fileName, city);


                    timeTablesRepository.InsertRange(shedule);
                    TempData["Success"] = "Расписание добавлено";
                }
                catch(Exception ex)
                {
                    TempData["Erors"] = "Ошибка при обработке файла, проверьте правильность файла";
                }
                return RedirectToAction("AddFromFile");
            }
            return View(model);
        }




        [HttpGet]
        public ActionResult AddBus()
        {
            AddBusViewModel model = new AddBusViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddBus(AddBusViewModel model)
        {
            int cityId = (int)Session["City"];
            if(ModelState.IsValid)
            {
                try
                {
                    busRepository.Insert(new Bus { Number = model.Number, CityId = cityId });
                    TempData["Success"] = "Запись добавлена";
                    model = new AddBusViewModel();
                }
                catch(Exception ex)
                {
                    //log
                    ModelState.AddModelError("", "Ошибка при добавлении записи. Повторите попытку позже");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteBus()
        {
            int cityId = (int)Session["City"];

            DeleteBusViewModel model = new DeleteBusViewModel
            {
                Buses = new List<SelectListItem>(busRepository.Get(x => x.CityId == cityId).Select(x => new SelectListItem {Value = x.Id.ToString(), Text = x.Number }))
            };
            return View(model);
        }

        [HttpDelete]
        public ActionResult DeleteBus(DeleteBusViewModel model)
        {
            int cityId = (int)Session["City"];
            if (ModelState.IsValid)
            { 
                try
                {
                    busRepository.Delete(int.Parse(model.Bus));
                    model.Bus = "";
                    TempData["Success"] = "Запись удалена";
                }
                catch(Exception ex)
                {
                    //log
                    ModelState.AddModelError("", "Ошибка при удалении записи. Повторите попытку позже");
                }
            }
            model.Buses = new List<SelectListItem>(busRepository.Get(x => x.CityId == cityId).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Number }));
            return View(model);
        }

        [HttpGet]
        public ActionResult AddStop()
        {
            AddStopViewModel model = new AddStopViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddStop(AddStopViewModel model)
        {
            int cityId = (int)Session["City"];
            if(stopsRepository.Get(x => x.CityId == cityId && x.Name.ToLower() == model.Name.ToLower()).Any())
            {
                ModelState.AddModelError("","Остановка с таким названием уже существует");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    stopsRepository.Insert(new Stop { CityId = cityId, Name = model.Name });
                    TempData["Success"] = "Запись добавлена";
                    model = new AddStopViewModel();
                }
                catch (Exception ex)
                {
                    //log
                    ModelState.AddModelError("", "Ошибка при добавлении записи. Повторите попытку позже");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteStop()
        {
            int cityId = (int)Session["City"];


            DeleteStopViewModel model = new DeleteStopViewModel
            {
                Stops = new List<SelectListItem>(stopsRepository.Get(x => x.CityId == cityId).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }))
            };
            return View(model);
        }

        [HttpDelete]
        public ActionResult DeleteStop(DeleteStopViewModel model)
        {
            int cityId = (int)Session["City"];
            if (ModelState.IsValid)
            {
                try
                {
                    stopsRepository.Delete(int.Parse(model.Stop));
                    model.Stop = "";
                    TempData["Success"] = "Запись удалена";
                }
                catch (Exception ex)
                {
                    //log
                    ModelState.AddModelError("", "Ошибка при удалении записи. Повторите попытку позже");
                }
            }
            model.Stops = new List<SelectListItem>(stopsRepository.Get(x => x.CityId == cityId).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));
            return View(model);
        }

        [HttpGet]
        public ActionResult AddBusRoute()
        {
            int cityId = (int)Session["City"];
            var buses = busRepository.Get(x => x.CityId == cityId).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Number });
            var stops = stopsRepository.Get(x => x.CityId == cityId).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            AddBusRouteViewModel model = new AddBusRouteViewModel { Buses = buses, Stops = stops  };
            return View(model);
        }


        [HttpPost]
        public ActionResult AddBusRoute(AddBusRouteViewModel model)
        {
            int cityId = (int)Session["City"];
            IEnumerable<TimeTable> first = CreateTimeTableRange(model.First, model.Bus);
            IEnumerable<TimeTable> second = CreateTimeTableRange(model.Second, model.Bus);
            if (ModelState.IsValid)
            {
                try
                {
                    timeTablesRepository.InsertRange(first);
                    timeTablesRepository.InsertRange(second);
                    model.Bus = null;
                    model.First = null;
                    model.Second = null;
                    TempData["Success"] = "Запись добавлена";
                }
                catch(Exception ex)
                {
                    //log
                    ModelState.AddModelError("", "Ошибка при добавлении записи. Повторите попытку позже");
                }
            }
            var buses = busRepository.Get(x => x.CityId == cityId).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Number });
            var stops = stopsRepository.Get(x => x.CityId == cityId).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            model.Buses = buses;
            model.Stops = stops;
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteBusRoute()
        {
            int cityId = (int)Session["City"];
            var buses = timeTablesRepository.Get(x => x.Bus.CityId == cityId)
                                            .Select(x => x.Bus)
                                            .Distinct()
                                            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Number });
           
            DeleteBusRouteViewModel model = new DeleteBusRouteViewModel { Buses = buses };
            return View(model);
        }

        [HttpDelete]
        public ActionResult DeleteBusRoute(DeleteBusRouteViewModel model)
        {
            int cityId = (int)Session["City"];
            if (ModelState.IsValid)
            {
                try
                {
                    var bus = busRepository.GetByID(int.Parse(model.Bus));
                    var timeTables = timeTablesRepository.Get(x => x.BusId == bus.Id);
                    timeTablesRepository.DeleteRange(timeTables);
                    TempData["Success"] = "Запись удалена";
                    model.Bus = null;
                }
                catch (Exception ex)
                {
                    //log
                    ModelState.AddModelError("", "Ошибка при удалении записи. Повторите попытку позже");
                }
            }
            model.Buses = timeTablesRepository.Get(x => x.Bus.CityId == cityId)
                                            .Select(x => x.Bus)
                                            .Distinct()
                                            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Number });
            return View(model);
        }

        [HttpGet]
        public ActionResult AddShedule()
        {
            int cityId = (int)Session["City"];
            var buses = timeTablesRepository.Get(x => x.Bus.CityId == cityId && x.Shedules.Count == 0)
                                            .Select(x => x.Bus)
                                            .Distinct()
                                            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Number });
            var days = ((Days[])Enum.GetValues(typeof(Days))).Select(x => x.ToDescription());

            AddSheduleViewModel model = new AddSheduleViewModel { Buses = buses, Days = days };
            return View(model);
        }


        [HttpPost]
        public ActionResult AddShedule(AddSheduleViewModel model)
        {
            int cityId = (int)Session["City"];
            if (ModelState.IsValid)
            {
                try
                {
                    var busId = int.Parse(model.Bus);
                    var stop = stopsRepository.Get(x => x.CityId == cityId && x.Name == model.Stop).First();
                    var endStop = stopsRepository.Get(x => x.CityId == cityId && x.Name == model.EndStop).First();
                    var timeTable = timeTablesRepository.Get(x => x.BusId == busId && x.Stop.Id == stop.Id && x.FinalStop.Id == endStop.Id).First();
                    Days days = new Days();
                    foreach (var day in model.SelectedDays)
                    {
                        days |= ((Days[])Enum.GetValues(typeof(Days))).Where(x => x.ToDescription() == day).First();
                    }
                    List<Shedule> sheduleRange = new List<Shedule>();
                    foreach (var time in model.Shedule)
                    {
                        sheduleRange.Add(new Shedule { Days = days, TimeTableId = timeTable.Id, Time = time });
                    }
                    shedulesRepository.InsertRange(sheduleRange);
                    model.Shedule = null;
                    model.Bus = null;
                    model.SelectedDays = null;
                    TempData["Success"] = "Запись добавлена";
                }
                catch (Exception ex)
                {
                    //log
                    ModelState.AddModelError("", "Ошибка при добавлении записи. Повторите попытку позже");
                }
            }
            model.Buses = timeTablesRepository.Get(x => x.Bus.CityId == cityId && x.Shedules.Count == 0)
                                            .Select(x => x.Bus)
                                            .Distinct()
                                            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Number });
            model.Days = ((Days[])Enum.GetValues(typeof(Days))).Select(x => x.ToDescription());
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteShedule()
        {
            int cityId = (int)Session["City"];
            var buses = timeTablesRepository.Get(x => x.Bus.CityId == cityId && x.Shedules.Count != 0)
                                            .Select(x => x.Bus.Number)
                                            .Distinct();
            DeleteSheduleViewModel model = new DeleteSheduleViewModel { Buses = buses};
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteShedule(DeleteSheduleViewModel model)
        {
            int cityId = (int)Session["City"];
            if(ModelState.IsValid)
            {
                try
                {
                    var bus = busRepository.Get(x => x.Number == model.Bus && x.CityId == cityId).First();
                    var stop = stopsRepository.Get(x => x.Name == model.Stop && x.CityId == cityId).First();
                    var endStop = stopsRepository.Get(x => x.Name == model.EndStop && x.CityId == cityId).First();
                    var timeTable = timeTablesRepository.Get(x => x.Bus.Id == bus.Id && x.Stop.Id == stop.Id && x.FinalStop.Id == endStop.Id).First();
                    var shedules = timeTable.Shedules.Where(x => x.Days.ToDescription() == model.Days);
                    shedulesRepository.DeleteRange(shedules);
                    TempData["Success"] = "Запись удалена";
                }
                catch
                {
                    //log
                    ModelState.AddModelError("", "Ошибка при удалении записи. Повторите попытку позже");
                }
            }
        
            var buses = timeTablesRepository.Get(x => x.Bus.CityId == cityId && x.Shedules.Count != 0)
                                            .Select(x => x.Bus.Number)
                                            .Distinct();
            model.Bus = null;
            model.Buses = buses;
            model.Stop = null;
            model.EndStop = null;
            model.Days = null;
            return View(model);
        }


        [HttpDelete]
        public ActionResult DeleteAll()
        {
            int cityId = (int)Session["City"];
            try
            {
                var buses = busRepository.Get(x => x.CityId == cityId);
                var stops = stopsRepository.Get(x => x.CityId == cityId);
                var timeTables = timeTablesRepository.Get(x => x.Bus.CityId == cityId);
                timeTablesRepository.DeleteRange(timeTables);
                stopsRepository.DeleteRange(stops);
                busRepository.DeleteRange(buses);
                TempData["Success"] = "Записи удалены";
            }
            catch(Exception ex)
            {
                //log
                TempData["Erors"] = "Ошибка при удалении записей. Повторите попытку позже";
            }
            return RedirectToAction("Index", "Admin");
        }


        private IEnumerable<TimeTable> CreateTimeTableRange(IEnumerable<string> stops, string bus)
        {
            List<TimeTable> result = new List<TimeTable>();
            int count = stops.Count();
            var finalStop = stopsRepository.GetByID(int.Parse(stops.Last()));
            for (int i = 0; i < count; i++)
            {
                Stop stop = stopsRepository.GetByID(int.Parse(stops.ElementAt(i)));
                Stop nextStop, previousStop;
                nextStop = i < count - 1 ? stopsRepository.GetByID(int.Parse(stops.ElementAt(i + 1))) : null;
                previousStop = i > 0 ? stopsRepository.GetByID(int.Parse(stops.ElementAt(i - 1))) : null;
                result.Add(new TimeTable
                {
                    BusId = int.Parse(bus),
                    Stop = stop,
                    NextStop = nextStop,
                    PreviousStop = previousStop,
                    FinalStop = finalStop
                });
            }
            return result;
        }
    }
}