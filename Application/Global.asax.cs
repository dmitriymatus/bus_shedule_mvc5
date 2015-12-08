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
using Application.Models.Admin;
using System.Data.Entity;
using System.Globalization;
using Application.Infrastructure.SheduleParserFactory;

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
            SheduleParsersConfig.RegisterParsers(SheduleParsers.Parsers);
            //ValueProviderFactories.Factories.Insert(0, new CityValueProviderFactory());

            ModelBinders.Binders.Add(typeof(AddSheduleViewModel), new SheduleBinder());
            DependencyResolver.SetResolver(new NinjectDependencyResolver(new StandardKernel()));

             String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SheduleDbContext"].ConnectionString;
             System.Web.Caching.SqlCacheDependencyAdmin.EnableNotifications(connectionString);
             System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(connectionString, "Shedules");
             System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(connectionString, "News");

        }
    }
}
