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
        private string[] namespaces;

        public TypedRouteInfo(Expression<Func<TController, ActionResult>> action, string[] namespaces)
        {
            methodCall = action.Body as MethodCallExpression;
            if (methodCall == null)
                throw new ArgumentOutOfRangeException("action", "Action should be a MethodCallExpression");
            Namespaces = namespaces;
        }

        public virtual RouteValueDictionary Defaults
        {
            get { return defaults ?? (defaults = GetDefaults()); }
        }

        public virtual string[] Namespaces
        {
            get { return namespaces ?? (namespaces = GetNamespaces()); }
            private set { namespaces = value; }
        }

        protected virtual string ControllerName
        {
            get
            {
                string controllerName = typeof (TController).Name;
                return controllerName.Remove(controllerName.LastIndexOf("Controller"));
            }
        }

        protected virtual RouteValueDictionary GetDefaults()
        {
            RouteValueDictionary defaults = GetActionParameters();
            defaults["controller"] = ControllerName;
            defaults["action"] = GetActionName();
            return defaults;
        }

        protected virtual RouteValueDictionary GetActionParameters()
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

        protected virtual string GetActionName()
        {
            object[] attributes = methodCall.Method.GetCustomAttributes(typeof (ActionNameAttribute), true);
            return attributes.Length == 1 ? ((ActionNameAttribute) attributes[0]).Name : methodCall.Method.Name;
        }


        protected virtual string[] GetNamespaces()
        {
            return new[] {typeof (TController).Namespace};
        }
    }
}