using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MapStructure;
using SimpleMvcApp.Adapters;
using SimpleMvcApp.Controllers;
using SimpleMvcApp.Services;

namespace SimpleMvcApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            var container = new IocContainer();
            
            container.Register<HomeController, HomeController>();
            container.Register<ISimpleService, SimpleService>();

            var controllerFactory = new MapStructureAdapter.MapStructureControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}
