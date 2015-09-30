using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Domain.Abstract;
using Domain.Concrete;

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
            kernel.Bind<IStopsRepository>().To<EFStopsRepository>();
            kernel.Bind<IUserRoutesRepository>().To<EFUserRoutesRepository>();
            kernel.Bind<ISheduleCreator>().To<SheduleCreator>();
        }

    }
}