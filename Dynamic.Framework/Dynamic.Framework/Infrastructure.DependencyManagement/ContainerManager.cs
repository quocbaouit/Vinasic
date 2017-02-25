using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Builder;
using RegistrationExtensions = Autofac.RegistrationExtensions;

namespace Dynamic.Framework.Infrastructure.DependencyManagement
{
    public class ContainerManager
    {
        private readonly IContainer _container;

        public IContainer Container
        {
            get
            {
                return this._container;
            }
        }

        public ContainerManager(IContainer container)
        {
            this._container = container;
        }

        public void AddComponent(Type service, Type implementation, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            this.UpdateContainer((Action<ContainerBuilder>)(x =>
            {
                List<Type> list = new List<Type>()
        {
          service
        };
                if (service.IsGenericType)
                {
                    IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> registrationBuilder = ContainerManagerExtensions.PerLifeStyle<object, ReflectionActivatorData, DynamicRegistrationStyle>(RegistrationExtensions.RegisterGeneric(x, implementation).As(list.ToArray()), lifeStyle);
                    if (string.IsNullOrEmpty(key))
                        return;
                    registrationBuilder.Keyed((object)key, service);
                }
                else
                {
                    IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder = ContainerManagerExtensions.PerLifeStyle<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(RegistrationExtensions.RegisterType(x, implementation).As(list.ToArray()), lifeStyle);
                    if (!string.IsNullOrEmpty(key))
                        registrationBuilder.Keyed((object)key, service);
                }
            }));
        }

        public void AddComponent<TService, TImplementation>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            this.AddComponent(typeof(TService), typeof(TImplementation), key, lifeStyle);
        }

        public void AddComponent(Type service, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            this.AddComponent(service, service, key, lifeStyle);
        }

        public void AddComponent<TService>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            this.AddComponent<TService, TService>(key, lifeStyle);
        }

        public void AddComponentInstance<TService>(object instance, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            this.AddComponentInstance(typeof(TService), instance, key, lifeStyle);
        }

        public void AddComponentInstance(Type service, object instance, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            this.UpdateContainer((Action<ContainerBuilder>)(x => ContainerManagerExtensions.PerLifeStyle<object, SimpleActivatorData, SingleRegistrationStyle>(RegistrationExtensions.RegisterInstance<object>(x, instance).Keyed((object)key, service).As(new Type[1]
      {
        service
      }), lifeStyle)));
        }

        public void AddComponentInstance(object instance, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            this.AddComponentInstance(instance.GetType(), instance, key, lifeStyle);
        }

        public void AddComponentWithParameters<TService, TImplementation>(IDictionary<string, string> properties, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            this.AddComponentWithParameters(typeof(TService), typeof(TImplementation), properties, "", ComponentLifeStyle.Singleton);
        }

        public void AddComponentWithParameters(Type service, Type implementation, IDictionary<string, string> properties, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            this.UpdateContainer((Action<ContainerBuilder>)(x =>
            {
                List<Type> list = new List<Type>()
        {
          service
        };
               // IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registrationBuilder = (IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>)Autofac.RegistrationExtensions.WithParameters<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>((IRegistrationBuilder<M0, M1, M2>)Autofac.RegistrationExtensions.RegisterType(x, implementation).As(list.ToArray()), (IEnumerable<Parameter>)Enumerable.Select<KeyValuePair<string, string>, NamedParameter>((IEnumerable<KeyValuePair<string, string>>)properties, (Func<KeyValuePair<string, string>, NamedParameter>)(y => new NamedParameter(y.Key, (object)y.Value))));
                if (string.IsNullOrEmpty(key))
                    return;
               // registrationBuilder.Keyed((object)key, service);
            }));
        }

        public T Resolve<T>(string key = "") where T : class
        {
            if (string.IsNullOrEmpty(key))
                return ResolutionExtensions.Resolve<T>((IComponentContext)this.Scope());
            return ResolutionExtensions.ResolveKeyed<T>((IComponentContext)this.Scope(), (object)key);
        }

        public object Resolve(Type serviceType)
        {
            return ResolutionExtensions.Resolve((IComponentContext)this.Scope(), serviceType);
        }

        public T[] ResolveAll<T>(string key = "")
        {
            if (string.IsNullOrEmpty(key))
                return Enumerable.ToArray<T>(ResolutionExtensions.Resolve<IEnumerable<T>>((IComponentContext)this.Scope()));
            return Enumerable.ToArray<T>(ResolutionExtensions.ResolveKeyed<IEnumerable<T>>((IComponentContext)this.Scope(), (object)key));
        }

        public ILifetimeScope Scope()
        {
            return (ILifetimeScope)this.Container;
        }

        private void UpdateContainer(Action<ContainerBuilder> action)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            action(containerBuilder);
            containerBuilder.Update(this._container);
        }
    }
}
