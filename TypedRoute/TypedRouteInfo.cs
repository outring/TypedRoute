using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace TypedRoute
{
	public class TypedRouteInfo<TController>
	{
		private readonly MethodCallExpression methodCall;
		private RouteValueDictionary defaults;
		private string name;
		private string[] namespaces;

		public TypedRouteInfo(string name, string url, Expression<Func<TController, ActionResult>> controllerAction,
		                      object constraints, string[] namespaces)
		{
			methodCall = controllerAction.Body as MethodCallExpression;
			if (methodCall == null)
				throw new ArgumentOutOfRangeException("controllerAction", "Action should be a MethodCallExpression");
			Namespaces = namespaces;
			Name = name;
			Url = string.IsNullOrWhiteSpace(url) ? Guid.NewGuid().ToString() : url;
			Constraints = constraints;
		}

		public string Name
		{
			get { return name ?? (name = GetName()); }
			set { name = value; }
		}

		public string Url { get; set; }

		public RouteValueDictionary Defaults
		{
			get { return defaults ?? (defaults = GetDefaults()); }
			set { defaults = value; }
		}

		public object Constraints { get; set; }

		public string[] Namespaces
		{
			get { return namespaces ?? (namespaces = GetNamespaces()); }
			set { namespaces = value; }
		}

		public Route UseIn(Func<string, string, object, object, string[], Route> mapRouteMethod)
		{
			Route route = mapRouteMethod(Name, Url, new {}, Constraints, Namespaces);
			route.Defaults = Defaults;
			return route;
		}

		private string GetName()
		{
			return Guid.NewGuid().ToString();
		}

		private RouteValueDictionary GetDefaults()
		{
			RouteValueDictionary defaults = GetActionParameters();
			defaults["controller"] = GetControllerName();
			defaults["action"] = GetActionName();
			return defaults;
		}

		private RouteValueDictionary GetActionParameters()
		{
			var defaultParameters = new RouteValueDictionary();
			ParameterInfo[] methodParameters = methodCall.Method.GetParameters();
			for (int i = 0; i < methodParameters.Length; i++)
			{
				Expression argumentExpression = methodCall.Arguments[i];
				if (argumentExpression is ConstantExpression)
					defaultParameters.Add(methodParameters[i].Name, (argumentExpression as ConstantExpression).Value);
			}
			return defaultParameters;
		}

		private string GetActionName()
		{
			object[] attributes = methodCall.Method.GetCustomAttributes(typeof (ActionNameAttribute), true);
			return attributes.Length == 1 ? ((ActionNameAttribute) attributes[0]).Name : methodCall.Method.Name;
		}

		private string GetControllerName()
		{
			string controllerName = typeof (TController).Name;
			return controllerName.Remove(controllerName.LastIndexOf("Controller"));
		}

		private string[] GetNamespaces()
		{
			return new[] {typeof (TController).Namespace};
		}
	}
}