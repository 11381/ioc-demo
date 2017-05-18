using System;
using System.Collections.Generic;
using System.Linq;

namespace MapStructure
{
    public class IocContainer : IContainer
    {
        protected List<DependencyInfo> _dependencies = new List<DependencyInfo>();
        public void Register<TContract, TImplementation>()
        {
            this.Register<TContract, TImplementation>(DependencyScope.Transient);
        }

        public void Register<TContract, TImplementation>(DependencyScope scope)
        {
            this._dependencies.Add(new DependencyInfo()
            {
                Contract = typeof(TContract),
                Implementation = typeof(TImplementation),
                Scope = scope
            });
        }

        public void Register<TContract>(Func<IContainer, object> factory)
        {
            this._dependencies.Add(new DependencyInfo()
            {
                Contract = typeof(TContract),
                Factory = factory
            });
        }

        public TContract Resolve<TContract>()
        {
            return (TContract)this.ResolveImplementation(typeof(TContract));
        }

        public object Resolve(Type contractType)
        {
            return this.ResolveImplementation(contractType);
        }

        protected object ResolveImplementation(Type contractType)
        {
            var registration = this.GetDependency(contractType);
            if (null == registration)
            {
                throw new DependencyException($"Unable to resolve type, no registration exists for the type {contractType.Name}");
            }

            if (null != registration.Factory)
            {
                return registration.Factory(this);
            }
            else
            {
                return ResolveScopedDependency(registration);
            }
        }

        protected object ResolveScopedDependency(DependencyInfo dependency)
        {
            if (DependencyScope.Transient == dependency.Scope || null == dependency.Instance)
            {
                try
                {
                    var constructorDependencies = ResolveConstructorParams(dependency);
                    var concretion = Activator.CreateInstance(dependency.Implementation, constructorDependencies.ToArray());
                    dependency.Instance = concretion;
                }
                catch (DependencyException ex)
                {
                    throw new DependencyException($"Error resolving {dependency.Contract.Name}, some constructor parameters failed to resolve. See inner exception for further details.", ex);
                }
            }
            return dependency.Instance;
        }

        protected DependencyInfo GetDependency(Type contractType)
        {
            return this._dependencies.FirstOrDefault(dependency => dependency.Contract == contractType);
        }

        protected IEnumerable<object> ResolveConstructorParams(DependencyInfo dependency)
        {
            var ctor = dependency.Implementation.GetConstructors().First();
            return ctor.GetParameters().Select(parameter => ResolveImplementation(parameter.ParameterType));
        }
    }
}