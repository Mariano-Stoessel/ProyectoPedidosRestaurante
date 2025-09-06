using System;
using System.Web.Routing;
using ProyectoPedidosResto.Utils;  // Asegúrate de tener este using

namespace ProyectoPedidosResto
{
    public class Global : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}