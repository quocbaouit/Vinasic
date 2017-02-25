using Autofac;
using Autofac.Builder;
using Dynamic.Framework.Infrastructure.DependencyManagement;
using System;

namespace Dynamic.Framework.Infrastructure
{
    public class DynamicEngine : IEngine
    {
        private ContainerManager _containerManager;

        public ContainerManager ContainerManager
        {
            get
            {
                return this._containerManager;
            }
        }

        public DynamicEngine()
            : this(new ContainerBuilder())
        {
        }

        public DynamicEngine(ContainerBuilder builder)
        {
            this.InitializeContainer(builder);
        }

        private void InitializeContainer(ContainerBuilder builder)
        {
            this._containerManager = new ContainerManager(builder.Build(ContainerBuildOptions.None));
        }

        public void Initialize()
        {
        }

        public T Resolve<T>() where T : class
        {
            return this.ContainerManager.Resolve<T>("");
        }

        public object Resolve(Type serviceType)
        {
            return this.ContainerManager.Resolve(serviceType);
        }

        public T[] ResolveAll<T>()
        {
            return this.ContainerManager.ResolveAll<T>("");
        }
    }
}
