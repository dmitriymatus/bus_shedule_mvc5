using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MvcApplication.Infrastructure;
using Application.Infrastructure;
using Application.Models;
using Ninject;

namespace Application
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(BusStopViewModel), new BusStopBinder());
            DependencyResolver.SetResolver(new NinjectDependencyResolver(new StandardKernel()));
        }
    }
}
