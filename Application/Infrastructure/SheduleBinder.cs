using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Models;
using System.Text;
using System.Text.RegularExpressions;
using Application.Models.Admin;

namespace Application.Infrastructure
{
    public class SheduleBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(AddSheduleViewModel))
            {
                HttpRequestBase request = controllerContext.HttpContext.Request;
                var model = base.BindModel(controllerContext, bindingContext) as AddSheduleViewModel;
                bindingContext.ModelState.Clear();
                var shedule = request.Form["Shedule"];
                    Regex reg = new Regex(@"\d{1,2}:\d{1,2}");
                    MatchCollection matches = reg.Matches(shedule);
                if (matches.Count != 0)
                {
                    try
                {
                   
                        model.Shedule = matches.Cast<Match>().Select(x => TimeSpan.Parse(x.Value)).ToList();
                    
                }
                catch(Exception ex)
                {
                        NLog.LogManager.GetCurrentClassLogger().Error(ex);
                        bindingContext.ModelState.AddModelError("", "Неверно заполнено расписание");
                }
                }
                return model;

            }
            else
            {
                return base.BindModel(controllerContext, bindingContext);
            }
        }
    }
}