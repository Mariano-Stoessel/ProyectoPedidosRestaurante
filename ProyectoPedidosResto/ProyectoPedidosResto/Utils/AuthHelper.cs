using ProyectoPedidosResto.Domain;
using ProyectoPedidosResto.Models;
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace ProyectoPedidosResto.Utils
{
    public static class AuthHelper
    {
        public const int MinutesToExpire = 720; // 720 = 12 horas en minutos

        public static void SetearMozoSession(int mozoId, string mozoNombre, DateTime ingreso)
        {
            HttpContext.Current.Session["MozoId"] = mozoId;
            HttpContext.Current.Session["MozoNombre"] = mozoNombre;
            HttpContext.Current.Session["MozoIngreso"] = ingreso;
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

        public static void CrearUsuariosSeleccionadoCookie(User usuarioSeleccionado, DateTime ingreso)
        {
            // Calcular minutos restantes desde el ingreso original
            double minutosRestantes = MinutesToExpire - (DateTime.Now - ingreso).TotalMinutes;
            if (minutosRestantes <= 0)
                minutosRestantes = 1.0 / 60.0; // 1 segundo en minutos

            var cookie = new HttpCookie("UserInfo");
            cookie.Values["User_id"] = usuarioSeleccionado.IdUsuario.ToString();
            cookie.Values["User_db"] = usuarioSeleccionado.UsuarioDB;
            cookie.Values["User_ip"] = usuarioSeleccionado.IP;
            cookie.Values["User_Database"] = usuarioSeleccionado.DatabaseName;
            cookie.Values["User_Password"] = usuarioSeleccionado.Password;
            cookie.Expires = DateTime.Now.AddMinutes(minutosRestantes);
            cookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void CrearMozoCookie(int mozoId, string mozoNombre, DateTime ingreso)
        {
            // Calcular minutos restantes desde el ingreso original
            double minutosRestantes = MinutesToExpire - (DateTime.Now - ingreso).TotalMinutes;
            if (minutosRestantes <= 0)
                minutosRestantes = 1.0 / 60.0; // 1 segundo en minutos

            var cookie = new HttpCookie("MozoInfo");
            cookie.Values["Mozo_Id"] = mozoId.ToString();
            cookie.Values["Mozo_Nombre"] = mozoNombre;
            cookie.Values["Mozo_Ingreso"] = ingreso.ToString("o"); // ISO 8601
            cookie.Expires = DateTime.Now.AddMinutes(minutosRestantes);
            cookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        // Restaurar el usuario seleccionado desde la cookie si no está en la sesión.
        public static void LeerUsuariosSeleccionadoCookie()
        {
            User usuarioSeleccionado = new User();
            var cookieUsuario = HttpContext.Current.Request.Cookies["UserInfo"];
            if (cookieUsuario == null)
                return;
            int userId;
            if (!int.TryParse(cookieUsuario.Values["User_Id"], out userId))
                return;
            usuarioSeleccionado.IdUsuario = userId;
            usuarioSeleccionado.IP = cookieUsuario.Values["User_ip"];
            usuarioSeleccionado.DatabaseName = cookieUsuario.Values["User_Database"];
            usuarioSeleccionado.Password = cookieUsuario.Values["User_Password"];
            usuarioSeleccionado.UsuarioDB = cookieUsuario.Values["User_db"];
            if (usuarioSeleccionado == null)
                return;
            // Restaurar el usuario en la sesión si no existe
            if (HttpContext.Current.Session["UsuarioSeleccionado"] == null)
            {
                HttpContext.Current.Session["UsuarioSeleccionado"] = usuarioSeleccionado;
            }
        }
        // Leer la cookie del mozo y devolver los datos
        public static (int? MozoId, string MozoNombre, DateTime? ingreso) LeerMozoCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies["MozoInfo"];

            if (cookie == null)
                return (null, null, null);

            int mozoId;
            DateTime mozoLogin;
            if (!int.TryParse(cookie.Values["Mozo_Id"], out mozoId))
                return (null, null, null);

            string mozoNombre = cookie.Values["Mozo_Nombre"];
            if (!DateTime.TryParse(cookie.Values["Mozo_Ingreso"], out mozoLogin))
                return (null, null, null);

            return (mozoId, mozoNombre, mozoLogin);
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