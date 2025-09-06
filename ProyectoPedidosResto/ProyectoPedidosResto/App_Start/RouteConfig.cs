using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;


namespace ProyectoPedidosResto
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("login", "Login", "~/Views/Login.aspx");
            routes.MapPageRoute("tables", "Tables", "~/Views/Tables.aspx");
            routes.MapPageRoute("commands", "Commands", "~/Views/Commands.aspx");
            routes.MapPageRoute("default", "", "~/Views/Login.aspx");        
        }
    }
}
