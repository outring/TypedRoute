Strongly typed ASP.NET MVC MapRoute
===================================

This implementation of strongly typed MapRoute is based on [the code by Diego Perez](http://blogs.southworks.net/dperez/2009/05/09/mvc-strongly-typed-action-route-helper/),
but it can work with areas (even without specifying namespaces).
Also you can forget about specifying routes names and even url (for inner actions) if you want.

Usage:

    routes.MapRoute<MyController>(c => c.InnerAction());
    routes.MapRoute<MyController>("some/action/{parameter}", c => c.Action("Default parameter value"));
    routes.MapRoute<MyController>("some/another/action/{parameter}", c => c.Action(ItIs.Any<string>())); //No default value

