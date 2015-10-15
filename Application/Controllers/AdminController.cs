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
    [Authorize(Roles = "admin")]
    [OutputCache(Duration = 3600, SqlDependency = "shedule:BusStops")]
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
                    int? city = (int?)Session["City"];
                    creator.Create(fileName,repository, city);

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
            int? city = (int?)Session["City"];
            if (repository.Contain(stop.busNumber, stop.stopName, stop.finalStop, stop.days, city))
            {
                ModelState.AddModelError("", "Запись уже существует");
                var model = CreateViewModel();
                model.Stop = stop;
                return View(model);
            }
            repository.AddStop(stop.busNumber, stop.stopName, stop.finalStop, stop.days, city);
            TempData["Success"] = "Запись добавлена";
            return RedirectToAction("AddStop");
        }

        [HttpGet]
        public ActionResult Edit()
        {
            int? city = (int?)Session["City"];
            var buses = repository.GetBuses(city);
            return View(buses);
        }


        [HttpPost]
        [OutputCache(Duration = 60, NoStore = false)]
        public ActionResult Edit(BusStopViewModel stop)
        {
            int? city = (int?)Session["City"];
            if (repository.Update(stop.busNumber, stop.stopName, stop.finalStop, stop.days, stop.stops, city))
                TempData["Success"] = "Запись обновлена";
            else
                TempData["Erors"] = "Запись не обновлена";
            return RedirectToAction("Edit");
        }

        [HttpGet]
        public ActionResult Delete()
        {
            int? city = (int?)Session["City"];
            var buses = repository.GetBuses(city);
            return View(buses);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 60, NoStore = false)]
        public ActionResult Delete(BusStopViewModel stop)
        {
            int? city = (int?)Session["City"];
            if (repository.Delete(stop.busNumber, stop.stopName, stop.finalStop, stop.days, city))
                TempData["Success"] = "Запись удалена";
            return RedirectToAction("Delete");
        }


        [HttpDelete]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 60, NoStore = false)]
        public ActionResult DeleteAll()
        {
            int? city = (int?)Session["City"];
            repository.DeleteAll(city);
            TempData["Success"] = "Записи удалены";
            return RedirectToAction("Index");
        }

        private AdminAddViewModel CreateViewModel()
        {
            var numbers = repository.Stops.Select(x => x.BusNumber).Distinct();
            var stopNames = repository.Stops.Select(x => x.StopName).Distinct().OrderBy(x => x);
            var finalStops = repository.Stops.Select(x => x.FinalStop).Distinct().OrderBy(x => x);
            var days = repository.Stops.Select(x => x.Days).Distinct().OrderBy(x => x);
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