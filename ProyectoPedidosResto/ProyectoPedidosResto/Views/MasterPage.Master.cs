using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoPedidosResto.Models;

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

            // Si el usuario está logueado y no estan en Login, muestra el menú

            string currentPage = System.IO.Path.GetFileName(Request.Path).ToLower();
            if (currentPage == "login.aspx" || Session["MozoId"] == null)
            {
                pnlHamburguesa.Visible = false;
                pnlCollapse.Visible = false;
            }
            else
            {
                pnlHamburguesa.Visible = true;
                pnlCollapse.Visible = true;
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

            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx", false);
        }
    }
}