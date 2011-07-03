﻿using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace TypedRoute
{
    public static class AreaRegistrationContextExtensions
    {
        public static Route MapRoute<TController>(this AreaRegistrationContext context, string name, string url,
                                                  Expression<Func<TController, ActionResult>> action)
            where TController : IController
        {
            return MapRoute(context, name, url, action, null /* constraints */, null /* namespaces */);
        }

        public static Route MapRoute<TController>(this AreaRegistrationContext context, string name, string url,
                                                  Expression<Func<TController, ActionResult>> action, object constraints)
            where TController : IController
        {
            return MapRoute(context, name, url, action, constraints, null /* namespaces */);
        }

        public static Route MapRoute<TController>(this AreaRegistrationContext context, string name, string url,
                                                  Expression<Func<TController, ActionResult>> action,
                                                  string[] namespaces)
            where TController : IController
        {
            return MapRoute(context, name, url, action, null /* constraints */, namespaces);
        }

        public static Route MapRoute<TController>(this AreaRegistrationContext context, string name, string url,
                                                  Expression<Func<TController, ActionResult>> action, object constraints,
                                                  string[] namespaces)
            where TController : IController
        {
            var typedRouteInfo = new TypedRouteInfo<TController>(action, namespaces);
            Route route = context.MapRoute(name, url, new {}, constraints, typedRouteInfo.Namespaces);
            route.Defaults = typedRouteInfo.Defaults;
            return route;
        }
    }
}