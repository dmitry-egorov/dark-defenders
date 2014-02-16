using System;
using System.Collections.Generic;
using DarkDefenders.Domain.Game.Interfaces;
using DarkDefenders.Domain.Game.Internals;
using DarkDefenders.Domain.Model;
using Infrastructure.DDDES;
using Microsoft.Practices.Unity;

namespace DarkDefenders.Domain.Game
{
    public class GameBootstrapper: IUnityContainer
    {
        private readonly IUnityContainer _container = new UnityContainer();
        private readonly List<Type> _registeredListeners = new List<Type>();

        public GameBootstrapper RegisterResource<TResource>(IResources<TResource> resource)
        {
            _container.RegisterInstance(resource);

            return this;
        }

        public GameBootstrapper RegisterListener<TEvents>(Func<TEvents> recieverFactory)
        {
            _container.RegisterType<TEvents>(new InjectionFactory(_ => recieverFactory()));

            _registeredListeners.Add(typeof(TEvents));

            return this;
        }

        public IGame Bootstrap()
        {
            return _container
            .RegisterDomain(_registeredListeners)
            .ResolveGame();
        }

        public IUnityContainer RegisterType(Type @from, Type to, string name, LifetimeManager lifetimeManager,
            params InjectionMember[] injectionMembers)
        {
            return _container.RegisterType(@from, to, name, lifetimeManager, injectionMembers);
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            return _container.RegisterInstance(t, name, instance, lifetime);
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return _container.Resolve(t, name, resolverOverrides);
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return _container.ResolveAll(t, resolverOverrides);
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return _container.BuildUp(t, existing, name, resolverOverrides);
        }

        public void Teardown(object o)
        {
            _container.Teardown(o);
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            return _container.AddExtension(extension);
        }

        public object Configure(Type configurationInterface)
        {
            return _container.Configure(configurationInterface);
        }

        public IUnityContainer RemoveAllExtensions()
        {
            return _container.RemoveAllExtensions();
        }

        public IUnityContainer CreateChildContainer()
        {
            return _container.CreateChildContainer();
        }

        public IUnityContainer Parent
        {
            get { return _container.Parent; }
        }

        public IEnumerable<ContainerRegistration> Registrations
        {
            get { return _container.Registrations; }
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}