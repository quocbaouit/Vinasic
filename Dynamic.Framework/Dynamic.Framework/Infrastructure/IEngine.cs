using Dynamic.Framework.Infrastructure.DependencyManagement;
using System;
namespace Dynamic.Framework.Infrastructure
{
    public interface IEngine
    {
        ContainerManager ContainerManager { get; }

        void Initialize();

        T Resolve<T>() where T : class;

        object Resolve(Type type);

        T[] ResolveAll<T>();
    }
}
