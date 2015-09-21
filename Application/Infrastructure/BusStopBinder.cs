using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Infrastructure
{
    public class BusStopBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(BusStopViewModel))
            {
                HttpRequestBase request = controllerContext.HttpContext.Request;
                var model = base.BindModel(controllerContext, bindingContext) as BusStopViewModel;

                Regex reg = new Regex(@"\d{1,2}:\d{1,2}");
                MatchCollection matches = reg.Matches(model.stops);
                if (matches.Count != 0)
                {                
                StringBuilder stops = new StringBuilder();
                foreach (Match match in matches)
                {
                    string time = match.Value;
                    stops.Append(time + " ");
                }
                model.stops = stops.ToString();
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