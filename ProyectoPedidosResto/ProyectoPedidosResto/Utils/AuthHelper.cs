using System.Web;
using System.Web.Security;

namespace ProyectoPedidosResto.Utils
{
    public static class AuthHelper
    {
        public static bool UsuarioAutenticado()
        {
            return HttpContext.Current.User?.Identity?.IsAuthenticated ?? false;
        }

        public static void SetearMozoSession(int mozoId, string mozoNombre)
        {
            HttpContext.Current.Session["MozoId"] = mozoId;
            HttpContext.Current.Session["MozoNombre"] = mozoNombre;
        }

        public static void LimpiarYCerrarSesion()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        public static (int? MozoId, string MozoNombre) ObtenerMozoDesdeTicket()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return (null, null);

            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
            if (ticket == null) return (null, null);

            var userData = ticket.UserData.Split('|');
            if (userData.Length < 2) return (null, null);

            return (int.Parse(userData[0]), userData[1]);
        }
    }
}