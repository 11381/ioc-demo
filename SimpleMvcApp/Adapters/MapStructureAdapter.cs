using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MapStructure;

namespace SimpleMvcApp.Adapters
{
    public class MapStructureAdapter
    {
        public class MapStructureControllerFactory : DefaultControllerFactory
        {
            private readonly IContainer _container;

            public MapStructureControllerFactory(IContainer container)
            {
                this._container = container;
            }

            protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                if (null == controllerType)
                {
                    return null;
                }

                return (IController) _container.Resolve(controllerType);
            }
        }
    }
}