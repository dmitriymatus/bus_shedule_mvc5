using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Domain.Abstract;
using Domain.Concrete;
//using Application.Infrastructure.SheduleParserFactory.Abstract;
//using Application.Infrastructure.SheduleParserFactory.Concrete;

namespace MvcApplication.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            var context = new SheduleDbContext();
            kernel.Bind(typeof(IRepository<>))
                .To(typeof(GenericRepository<>))
                .WithConstructorArgument("_context", context);
           // kernel.Bind<ISheduleParserFactory>().To<DefaultSheduleParserFactory>();
        }

    }
}