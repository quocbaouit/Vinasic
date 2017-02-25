using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using VINASIC.Data;
using VINASIC.Business;
using VINASIC.Business.Interface;
using VINASIC.Data.Repositories;
using System.Web.Mvc;
using System;
using Dynamic.Framework.Infrastructure.Data;
using VINASIC.App_Global;

namespace VINASIC.App_Start
{
    public class AutofacConfig
    {
        private static IContainer _container;

        public static IContainer Container
        {
            get
            {
                return _container;
            }
        }

        public static void Run()
        {
            SetAutofacContainer();
        }
        private static void SetAutofacContainer()
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterControllers(Assembly.GetExecutingAssembly());

                builder.RegisterType<UnitOfWork<VINASICEntities>>().As<IUnitOfWork<VINASICEntities>>()
                   .WithParameter(new NamedParameter("connectionString", AppGlobal.VinasicConnectionstring))
                   .InstancePerHttpRequest();

                builder.RegisterAssemblyTypes(typeof(T_ProductTypeRepository).Assembly)
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces().InstancePerHttpRequest();

                var services = Assembly.Load("VINASIC.Business");
                builder.RegisterAssemblyTypes(services).AsImplementedInterfaces().InstancePerHttpRequest();

                builder.RegisterFilterProvider();

                _container = builder.Build();
                DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));
            }
            catch (Exception)
            {
            }


        }
    }
}