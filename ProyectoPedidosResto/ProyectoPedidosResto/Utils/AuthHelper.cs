using ProyectoPedidosResto.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ProyectoPedidosResto.Utils
{
    public static class AuthHelper
    {
        public const int MinutesToExpire = 480; // 480 = 8 horas en minutos

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

        public static void LimpiarMozosInactivos()
        {
            var readerMozos = new ReadingWaiters();
            var readerIngresos = new ReadingEntries();

            var mozos = readerMozos.LeerMozos();
            var ingresos = readerIngresos.LeerIngresos();

            DateTime ahora = DateTime.Now;

            foreach (var mozo in mozos)
            {
                if (mozo.Mozo_Activo == "SI")
                {
                    // Buscar el último ingreso del mozo
                    var ultimoIngreso = ingresos
                        .Where(e => e.Ingreso_MozoId == mozo.Mozo_Id)
                        .OrderByDescending(e => e.Ingreso_Entrada)
                        .FirstOrDefault();

                    if (ultimoIngreso != null)
                    {
                        // Si tiene entrada y no tiene salida, sigue trabajando solo si no excedió el tiempo máximo
                        if (ultimoIngreso.Ingreso_Entrada != null &&
                            (ultimoIngreso.Ingreso_Salida == null || ultimoIngreso.Ingreso_Salida <= ultimoIngreso.Ingreso_Entrada))
                        {
                            // Si el tiempo desde la entrada es menor al máximo permitido, sigue trabajando
                            if ((ahora - ultimoIngreso.Ingreso_Entrada.Value).TotalMinutes < MinutesToExpire)
                                continue; // El mozo sigue trabajando

                            // Si ya pasó el tiempo máximo, marcar como inactivo
                            readerMozos.CambiarEstadoMozo(mozo.Mozo_Id, "NO");
                            continue;
                        }
                        // Si la hora actual está fuera del rango de trabajo (ya salió)
                        if (ultimoIngreso.Ingreso_Salida != null && ahora > ultimoIngreso.Ingreso_Salida.Value)
                        {
                            readerMozos.CambiarEstadoMozo(mozo.Mozo_Id, "NO");
                        }
                    }
                    else
                    {
                        // Si no hay ingresos, marcar como inactivo
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