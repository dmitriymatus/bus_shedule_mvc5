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

namespace Application.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        IStopsRepository repository;
        ISheduleCreator creator;
        public AdminController(IStopsRepository _repository, ISheduleCreator _creator)
        {
            repository = _repository;
            creator = _creator;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fileName = this.HttpContext.Request.MapPath("~/Content/shedule.xls");
                    model.file.SaveAs(fileName);

                    creator.Create(fileName,repository);

                    TempData["Success"] = "Расписание добавлено";
                }
                catch
                {
                    TempData["Erors"] = "Ошибка при обработке файла, проверьте правильность файла";
                }
                return View();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult AddStop()
        {
            var model = CreateViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddStop(BusStopViewModel stop)
        {
            if (!ModelState.IsValid)
            {
                var model = CreateViewModel();
                model.Stop = stop;
                return View(model);
            }

            if (repository.Contain(stop.busNumber, stop.stopName, stop.finalStop, stop.days))
            {
                ModelState.AddModelError("", "Запись уже существует");
                var model = CreateViewModel();
                model.Stop = stop;
                return View(model);
            }
            else
            {
                repository.AddStop(stop.busNumber, stop.stopName, stop.finalStop, stop.days);
                TempData["Success"] = "Запись добавлена";
            }
            return RedirectToAction("AddStop");
        }

        [HttpGet]
        [OutputCache(Duration = 1, NoStore = true)]
        public ActionResult Edit()
        {
            var buses = repository.GetBuses();
            return View(buses);
        }


        [HttpPost]
        public ActionResult Edit(BusStopViewModel stop)
        {
            if (repository.Update(stop.busNumber, stop.stopName, stop.finalStop, stop.days, stop.stops))
            {
                TempData["Success"] = "Запись обновлена";
            }
            else
            {
                TempData["Erors"] = "Запись не обновлена";
            }
            return RedirectToAction("Edit");
        }

        [HttpGet]
        [OutputCache(Duration = 1, NoStore = true)]
        public ActionResult Delete()
        {
            var buses = repository.GetBuses();
            return View(buses);
        }


        [HttpPost]
        public ActionResult Delete(BusStopViewModel stop)
        {
            if (repository.Delete(stop.busNumber, stop.stopName, stop.finalStop, stop.days))
            {
                TempData["Success"] = "Запись удалена";
            }
            return RedirectToAction("Delete");
        }

        [HttpGet]
        public ActionResult DeleteAll()
        {
            repository.DeleteAll();
            TempData["Success"] = "Записи удалены";
            return RedirectToAction("Index");
        }

        [NonAction]
        private AdminAddViewModel CreateViewModel()
        {
            var numbers = repository.Stops.Select(x => x.busNumber).Distinct();
            var stopNames = repository.Stops.Select(x => x.stopName).Distinct().OrderBy(x => x);
            var finalStops = repository.Stops.Select(x => x.finalStop).Distinct().OrderBy(x => x);
            var days = repository.Stops.Select(x => x.days).Distinct().OrderBy(x => x);
            AdminAddViewModel model = new AdminAddViewModel
            {
                Numbers = numbers,
                StopNames = stopNames,
                Days = days,
                FinalStops = finalStops,
                Stop = new BusStopViewModel()
            };
            return model;
        }


    }
}