using ProyectoPedidosResto.Domain;
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
            if (!IsPostBack)
            {
            if (Session["UsuarioSeleccionado"] != null)
            {
                    CargarDatosEmpresa();
            }
            else { lblEmpresa.Text = "Sistemas MH"; imgLogo.ImageUrl = "/logos/Default.png"; }

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

        private void CargarDatosEmpresa()
        {
            var usuarioSeleccionado = Session["UsuarioSeleccionado"] as User;
            if (usuarioSeleccionado != null)
            {
                // Cambia la ruta según la propiedad de la empresa/usuario
                imgLogo.ImageUrl = string.IsNullOrEmpty(usuarioSeleccionado.Logo)
                    ? "~/logos/Default.png"
                    : usuarioSeleccionado.Logo;

                lblEmpresa.Text = usuarioSeleccionado.Nombre;
            }
            else
            {
                imgLogo.ImageUrl = "~/logos/Default.png";
                lblEmpresa.Text = "Sistemas MH";
            }
        }

        protected void imgLogo_PreRender(object sender, EventArgs e)
        {
            imgLogo.Attributes["onerror"] = "this.onerror=null;this.src='" + ResolveUrl("~/logos/Default.png") + "';";
        }
    }
}