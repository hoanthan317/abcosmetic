using System.Web.Http;

namespace ABCosmeticWAD
{
    class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
                config.Routes.Clear();
                config.Routes.MapHttpRoute(
                   name: "DefaultApi",
                   routeTemplate: "api/{controller}/{id}",
                   defaults: new
                   { id = RouteParameter.Optional }
                );
            }
        }
    }
