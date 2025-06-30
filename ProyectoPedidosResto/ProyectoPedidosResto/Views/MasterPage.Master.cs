using ProyectoPedidosResto.Models;
using ProyectoPedidosResto.Utils;
using System;

namespace ProyectoPedidosResto.Views
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MozoNombre"] != null)
            {
                lblUsuario.Text = "Sesión de " + Session["MozoNombre"].ToString();
            }

            string currentPage = System.IO.Path.GetFileName(Request.Path).ToLower();
            bool isAuthenticated = AuthHelper.UsuarioAutenticado();

            if (currentPage == "login.aspx" || Session["MozoId"] == null || !isAuthenticated)
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