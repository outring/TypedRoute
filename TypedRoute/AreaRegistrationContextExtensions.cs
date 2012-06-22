using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace TypedRoute
{
    public static class AreaRegistrationContextExtensions
    {
		public static Route MapRoute<TController>(this AreaRegistrationContext context,
												  Expression<Func<TController, ActionResult>> controllerAction)
			where TController : IController
		{
			return MapRoute(context, null /* name */, null /* url */, controllerAction, null /* constraints */, null /* namespaces */);
		}

        public static Route MapRoute<TController>(this AreaRegistrationContext context, string url,
                                                  Expression<Func<TController, ActionResult>> controllerAction)
            where TController : IController
        {
            return MapRoute(context, null /* name */, url, controllerAction, null /* constraints */, null /* namespaces */);
        }

        public static Route MapRoute<TController>(this AreaRegistrationContext context, string name, string url,
                                                  Expression<Func<TController, ActionResult>> controllerAction)
            where TController : IController
        {
            return MapRoute(context, name, url, controllerAction, null /* constraints */, null /* namespaces */);
        }

        public static Route MapRoute<TController>(this AreaRegistrationContext context, string name, string url,
                                                  Expression<Func<TController, ActionResult>> controllerAction,
                                                  object constraints)
            where TController : IController
        {
            return MapRoute(context, name, url, controllerAction, constraints, null /* namespaces */);
        }

        public static Route MapRoute<TController>(this AreaRegistrationContext context, string name, string url,
                                                  Expression<Func<TController, ActionResult>> controllerAction,
                                                  string[] namespaces)
            where TController : IController
        {
            return MapRoute(context, name, url, controllerAction, null /* constraints */, namespaces);
        }

        public static Route MapRoute<TController>(this AreaRegistrationContext context, string name, string url,
                                                  Expression<Func<TController, ActionResult>> controllerAction,
                                                  object constraints, string[] namespaces)
            where TController : IController
        {
            var typedRouteInfo = new TypedRouteInfo<TController>(name, url, controllerAction, constraints, namespaces);
            return typedRouteInfo.UseIn(context.MapRoute);
        }
    }
}