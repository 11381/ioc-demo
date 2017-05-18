# MapStructure DI / IoC Container

Disclaimer: This is not affiliated with nor anywhere near the functionality or elegance of StructureMap it is simply a playful name because naming is hard.

This is a simple IoC container offering most basic functionality of R&R, all instantiations are transient by default but you can specify singleton as an alternative.

----

### Creating a container

    var container = new IocContainer();
    
### Registering a type

    container.Register<IMessageBus, MyMessageBus>();
    
### Resolving a type

    var instance = container.Resolve<IMessageBus>();

### Register a type specifying a scope other than transient

    container.Register<IMessageBus, MyMessageBus>(DependencyScope.Singleton);

### Register a type with a custom creation factory

    container.Register<IMessageBus>((iocContainer) => {
        var nestedDependency = iocContainer.Resolve<IDataProvider>();
        return new MessageBus(nestedDependency);
    });    

----

This project includes an MVC application using this library as the IOC Container and registers the controllers and also any dependencies thereof.
The MVC application itself does nothing more than display a value from an injected component.
