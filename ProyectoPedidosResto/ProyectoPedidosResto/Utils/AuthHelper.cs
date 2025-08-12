using System;
using System.Web;
using System.Web.Security;
using ProyectoPedidosResto.Models;

namespace ProyectoPedidosResto.Utils
{
    public static class AuthHelper
    {
        public const int MinutesToExpire = 1; // 480 = 8 horas en minutos

        public static void SetearMozoSession(int mozoId, string mozoNombre, DateTime now)
        {
            HttpContext.Current.Session["MozoId"] = mozoId;
            HttpContext.Current.Session["MozoNombre"] = mozoNombre;
            HttpContext.Current.Session["MozoFecha"] = DateTime.Now;
        }

        public static void LimpiarYCerrarSesion()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();

            // Borra la cookie del mozo
            if (HttpContext.Current.Request.Cookies["MozoInfo"] != null)
            {
                var cookie = new HttpCookie("MozoInfo");
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void CrearMozoCookie(int mozoId, string mozoNombre, DateTime login)
        {
            var cookie = new HttpCookie("MozoInfo");
            cookie.Values["Mozo_Id"] = mozoId.ToString();
            cookie.Values["Mozo_Nombre"] = mozoNombre;
            cookie.Values["Mozo_Fecha"] = login.ToString("o"); // ISO 8601
            cookie.Expires = login.AddMinutes(MinutesToExpire);
            cookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static (int? MozoId, string MozoNombre, DateTime? MozoLogin) LeerMozoCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies["MozoInfo"];
            if (cookie == null)
                return (null, null, null);

            int mozoId;
            DateTime mozoLogin;
            if (!int.TryParse(cookie.Values["Mozo_Fecha"], out mozoId))
                return (null, null, null);

            string mozoNombre = cookie.Values["Mozo_Nombre"];
            if (!DateTime.TryParse(cookie.Values["Mozo_Fecha"], out mozoLogin))
                return (null, null, null);

            return (mozoId, mozoNombre, mozoLogin);
        }

        public static void LimpiarMozosInactivos() //AGREGAR EN BASE DE DATOS MOZO_FECHA
        {
            var readerMozos = new ReadingWaiters();
            var mozos = readerMozos.LeerMozos();
            DateTime ahora = DateTime.Now;

            foreach (var mozo in mozos)
            {
                // Solo limpiar mozos activos y con fecha de login válida
                if (mozo.Mozo_Activo == "SI" && mozo.Mozo_Fecha != default(DateTime))
                {
                    if ((ahora - mozo.Mozo_Fecha).TotalMinutes >= MinutesToExpire)
                    {
                        readerMozos.CambiarEstadoMozo(mozo.Mozo_Id, "NO");
                    }
                }
            }
        }

        public static bool LoginNoExpirado(DateTime loginTime)
        {
            // Valida si el login aún no expiró según MinutesToExpire
            return (DateTime.Now - loginTime).TotalMinutes < MinutesToExpire;
        }

        public static void ResetearMozosActivos()
        {
            var readerMozos = new ReadingWaiters();
            var mozos = readerMozos.LeerMozos();
            foreach (var mozo in mozos)
            {
                if (mozo.Mozo_Activo == "SI")
                {
                    readerMozos.CambiarEstadoMozo(mozo.Mozo_Id, "NO");
                }
            }
        }
    }
}