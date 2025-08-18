using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ProyectoPedidosResto.Utils;  // Asegúrate de tener este using

namespace ProyectoPedidosResto
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Resetear todos los mozos activos al iniciar la aplicación
            AuthHelper.ResetearMozosActivos();
        }
    }
}