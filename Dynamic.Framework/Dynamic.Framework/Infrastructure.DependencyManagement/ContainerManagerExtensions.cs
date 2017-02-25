using Autofac.Builder;
using System.Web;

namespace Dynamic.Framework.Infrastructure.DependencyManagement
{
    public static class ContainerManagerExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> PerLifeStyle<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder, ComponentLifeStyle lifeStyle)
        {
            switch (lifeStyle)
            {
                case ComponentLifeStyle.Singleton:
                    return builder.SingleInstance();
                case ComponentLifeStyle.Transient:
                    return builder.InstancePerDependency();
                case ComponentLifeStyle.LifetimeScope:
                    return HttpContext.Current != null ? Autofac.Integration.Mvc.RegistrationExtensions.InstancePerHttpRequest<TLimit, TActivatorData, TRegistrationStyle>(builder) : builder.InstancePerLifetimeScope();
                default:
                    return builder.SingleInstance();
            }
        }
    }
}
