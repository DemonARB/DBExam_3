using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http; // Necesario para GlobalConfiguration y HttpConfiguration
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json; // Necesario para configurar JsonSerializerSettings

// Asegúrate de que el namespace coincida con el de tu proyecto
namespace DBExamen3
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Registro estándar de componentes de ASP.NET MVC y Web API
            AreaRegistration.RegisterAllAreas(); // Registra áreas si las tienes (como HelpPage)
            GlobalConfiguration.Configure(WebApiConfig.Register); // Registra la configuración de Web API (incluyendo nuestro TokenValidationHandler)
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); // Registra filtros globales (si los hubiera)
            RouteConfig.RegisterRoutes(RouteTable.Routes); // Registra las rutas de MVC (para vistas como Home/Index si las usas)
            BundleConfig.RegisterBundles(BundleTable.Bundles); // Registra los bundles de CSS y JS

            // --- CONFIGURACIÓN DEL SERIALIZADOR JSON ---
            // Obtiene la configuración global de la API
            HttpConfiguration config = GlobalConfiguration.Configuration;

            // Le dice al formateador JSON que ignore las referencias circulares
            // Esto evita el error "Self referencing loop detected"
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // --- FIN CONFIGURACIÓN DEL SERIALIZADOR JSON ---
        }
    }
}