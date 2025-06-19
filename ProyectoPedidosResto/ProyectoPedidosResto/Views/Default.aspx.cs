using ProyectoPedidosResto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoPedidosResto.Views
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Validar autenticación
            if (!User.Identity.IsAuthenticated)
            {
                var authCookie = Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    try
                    {
                        var ticket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                        if (ticket != null)
                        {
                            int mozoId;
                            if (int.TryParse(ticket.UserData, out mozoId))
                            {
                                var readerMozos = new ReadingWaiters();
                                readerMozos.CambiarEstadoMozo(mozoId, "NO");
                            }
                        }
                    }
                    catch { /* Ignorar errores de cookie corrupta */ }
                }
                Response.Redirect("Login.aspx");
                return;
            }

            validarUsuarioActivo();
        }

        private void validarUsuarioActivo()
        {
            if (Session["MozoId"] == null)
            {
                var authCookie = Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var ticket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null)
                    {
                        int mozoId = int.Parse(ticket.UserData);
                        string mozoNombre = ticket.Name;

                        // Validar el mozo en la base de datos antes de restaurar la sesión
                        var readerMozos = new ReadingWaiters();
                        var mozo = readerMozos.LeerMozos().FirstOrDefault(m => m.Mozo_Id == mozoId);

                        if (mozo == null || mozo.Mozo_Activo != "SI")
                        {
                            // Si el mozo no existe o no está activo, cerrar sesión y redirigir
                            System.Web.Security.FormsAuthentication.SignOut();
                            Session.Clear();
                            Response.Redirect("Login.aspx");
                            return;
                        }

                        Session["MozoId"] = mozoId;
                        Session["MozoNombre"] = mozoNombre;
                    }
                }
            }
        }
    }
}