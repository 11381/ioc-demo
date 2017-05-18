using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MapStructure.Tests
{
    interface IDummyType { }
    class DummyType : IDummyType { }
    interface IDummyTypeWithDependencies { }

    class DummyTypeWithDependencies : IDummyTypeWithDependencies
    {
        public DummyTypeWithDependencies(IDummyType dummyType)
        {
            
        }
    }

    class ThreadIndependentResolver<T>
    {
        [ThreadStatic]
        public static T instance;
    }

    public class IocContainerTests
    {
        [Fact]
        public void should_throw_if_not_registered()
        {
            var container = new IocContainer();
            Assert.Throws<DependencyException>(() => container.Resolve<IDummyType>());
        }

        [Fact]
        public void should_resolve_object()
        {
            var container = new IocContainer();
            container.Register<IDummyType, DummyType>();
            var instance = container.Resolve<IDummyType>();
            Assert.Equal(typeof(DummyType), instance.GetType());
        }

        [Fact]
        public void should_throw_if_ctor_dependency_not_registered()
        {
            var container = new IocContainer();
            container.Register<IDummyTypeWithDependencies, DummyTypeWithDependencies>();
            Assert.Throws<DependencyException>(() => container.Resolve<IDummyTypeWithDependencies>());
        }

        [Fact]
        public void should_resolve_object_with_registered_dependencies()
        {
            var container = new IocContainer();
            container.Register<IDummyTypeWithDependencies, DummyTypeWithDependencies>();
            container.Register<IDummyType, DummyType>();
            var instance = container.Resolve<IDummyTypeWithDependencies>();
            Assert.Equal(typeof(DummyTypeWithDependencies), instance.GetType());
        }

        [Fact]
        public void should_resolve_from_factory()
        {
            var container = new IocContainer();
            var concretion = new DummyType();
            container.Register<IDummyType>(iocContainer => concretion);
            var instance = container.Resolve<IDummyType>();
            Assert.Equal(instance, concretion);
        }

        [Fact]
        public async void should_resolve_from_thread_static()
        {
            var container = new IocContainer();
            container.Register<IDummyType>(iocContainer =>
            {
                return ThreadIndependentResolver<IDummyType>.instance ?? (ThreadIndependentResolver<IDummyType>.instance = new DummyType());
            });
            var instance = container.Resolve<IDummyType>();
            var threadedInstance = await Task.Run(() =>
            {
                return container.Resolve<IDummyType>();
            });
            Assert.NotEqual(instance, threadedInstance);
        }

        [Fact]
        public void should_resolve_as_transient_if_no_scope_specified()
        {
            var container = new IocContainer();
            container.Register<IDummyType, DummyType>();
            var instance = container.Resolve<IDummyType>();
            Assert.NotEqual(instance, container.Resolve<IDummyType>());
        }

        [Fact]
        public void should_resolve_as_singleton_when_specified()
        {
            var container = new IocContainer();
            container.Register<IDummyType, DummyType>(DependencyScope.Singleton);
            var instance = container.Resolve<IDummyType>();
            Assert.Equal(instance, container.Resolve<IDummyType>());
        }
    }
}