using System.Web.Http;
using System.Web.Http.Routing;

namespace StudentDataService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            GlobalConfiguration.Configuration.Routes.Add("default", new HttpRoute("{controller}"));

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "student",
                routeTemplate: "{controller}/{id}",
                defaults: new { Id = RouteParameter.Optional });
        }
    }
}
