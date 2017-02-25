using Autofac;
using System.Runtime.CompilerServices;

namespace Dynamic.Framework.Infrastructure
{
    public class EngineContext
    {
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                    EngineContext.Initialize(false);
                return Singleton<IEngine>.Instance;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate = false)
        {
            return EngineContext.Initialize(forceRecreate, (ContainerBuilder)null);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate, ContainerBuilder builder)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = EngineContext.CreateEngineInstance(builder);
                Singleton<IEngine>.Instance.Initialize();
            }
            return Singleton<IEngine>.Instance;
        }

        public static IEngine CreateEngineInstance(ContainerBuilder builder)
        {
            if (builder == null)
                return (IEngine)new DynamicEngine();
            return (IEngine)new DynamicEngine(builder);
        }
    }
}
