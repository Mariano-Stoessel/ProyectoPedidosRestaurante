using ProyectoPedidosResto.Models;
using ProyectoPedidosResto.Utils;
using System;

namespace ProyectoPedidosResto.Views
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Leer datos de la cookie usando AuthHelper
            var (mozoId, mozoNombre, mozoLogin) = AuthHelper.LeerMozoCookie();
            bool cookieValida = mozoId.HasValue && !string.IsNullOrEmpty(mozoNombre) && mozoLogin.HasValue &&
                        AuthHelper.LoginNoExpirado(mozoLogin.Value);

            string currentPage = System.IO.Path.GetFileName(Request.Path).ToLower();
            Console.WriteLine(currentPage);

            // 1. Si está en login y la cookie es válida, redirigir a Tables.aspx
            if (currentPage == "login" && cookieValida)
            {
                Response.Redirect("~/Views/Tables.aspx");
                return;
            }

            // 2. Si la cookie es válida, mostrar datos y continuar
            if (cookieValida)
            {
                lblUsuario.Text = "Sesión de " + mozoNombre;
            }
            else if (Session["MozoId"] != null && Session["MozoNombre"] != null && Session["MozoFecha"] != null)
            {
                // 3. Fallback: validar la sesión
                int sessionMozoId = (int)Session["MozoId"];
                string sessionMozoNombre = Session["MozoNombre"].ToString();
                DateTime sessionMozoFecha = (DateTime)Session["MozoFecha"];

                if (AuthHelper.LoginNoExpirado(sessionMozoFecha))
                {
                    lblUsuario.Text = "Sesión de " + sessionMozoNombre;

                    // Si está en login y la sesión es válida, redirigir a Tables.aspx
                    if (currentPage == "login")
                    {
                        Response.Redirect("~/Views/Tables.aspx");
                        return;
                    }
                }
                else
                {
                    // Sesión expirada
                    AuthHelper.LimpiarMozosInactivos();
                    AuthHelper.LimpiarYCerrarSesion();
                    Response.Redirect("~/Views/Login.aspx?expirado=1");
                    return;
                }
            }
            else if (currentPage == "login")
            {
                return;
            }
            else
            {
                // Ningún mecanismo válido, limpiar y redirigir
                AuthHelper.LimpiarMozosInactivos();
                AuthHelper.LimpiarYCerrarSesion();
                Response.Redirect("~/Views/Login.aspx?expirado=1");
                return;
            }

            // Mostrar/ocultar controles según estado
            if (currentPage == "login")
            {
                pnlHamburguesa.Visible = false;
                pnlCollapse.Visible = false;
                lblUsuario.Visible = false;
                btnCerrarSesion.Visible = false;
            }
            else
            {
                pnlHamburguesa.Visible = true;
                pnlCollapse.Visible = true;
                lblUsuario.Visible = true;
                btnCerrarSesion.Visible = true;
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            if (Session["MozoId"] != null)
            {
                int mozoId = (int)Session["MozoId"];
                var readerMozos = new ReadingWaiters();
                readerMozos.CambiarEstadoMozo(mozoId, "NO");
            }

            AuthHelper.LimpiarYCerrarSesion();

            Response.Redirect("Login.aspx", false);
        }
    }
}