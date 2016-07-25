using Learning.Web.Filters;
using System.Web.Http;

namespace Learning.Web {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services
            config.Filters.Add(new ForceHttpsAttribute());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Courses",
                routeTemplate: "api/courses/{id}",
                defaults: new { controller = "courses", id = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "Students",
                routeTemplate: "api/students/{userName}",
                defaults: new { controller = "students", userName = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "Enrollments",
                routeTemplate: "api/courses/{courseId}/students/{userName}",
                defaults: new { controller = "enrollments", userName = RouteParameter.Optional }
                );
        }
    }
}
