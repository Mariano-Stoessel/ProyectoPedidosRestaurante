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
            string currentPage = System.IO.Path.GetFileName(Request.Path).ToLower();

            // Mostrar/ocultar controles según estado de página
            bool esLogin = currentPage == "login";
            pnlHamburguesa.Visible = !esLogin;
            pnlCollapse.Visible = !esLogin;
            lblUsuario.Visible = !esLogin;
            btnCerrarSesion.Visible = !esLogin;

            if (!IsPostBack)
            {
                if (Session["UsuarioSeleccionado"] != null)
                {
                    CargarDatosEmpresa();
                }
                else { lblEmpresa.Text = "Sistemas MH"; imgLogo.ImageUrl = "/logos/Default.png"; }
            }

            // Leer datos de la cookie usando AuthHelper
            var (mozoId, mozoNombre, mozoIngreso) = AuthHelper.LeerMozoCookie(); 
            AuthHelper.LeerUsuariosSeleccionadoCookie(); // Asegurarse de que el usuario esté en sesión
            bool cookieValida = mozoId.HasValue && !string.IsNullOrEmpty(mozoNombre) && mozoIngreso.HasValue;

            // 1. Si está en login y la cookie es válida, redirigir a Tables.aspx
            if (currentPage == "login" && cookieValida)
            {
                Response.Redirect("~/Views/Tables.aspx");
                return;
            }

            // 2. Si la cookie es válida, mostrar datos y continuar
            if (cookieValida)
            {
                // Si la sesión no existe, la recreamos usando los datos de la cookie
                if (Session["MozoId"] == null || Session["MozoNombre"] == null || Session["MozoIngreso"] == null)
                {
                    AuthHelper.SetearMozoSession(mozoId.Value, mozoNombre, mozoIngreso.Value);
                }

                lblUsuario.Text = "Sesión de " + mozoNombre;
            }
            else if (Session["MozoId"] != null && Session["MozoNombre"] != null && Session["MozoIngreso"] != null)
            {
                // 3. Validar la sesión
                int sessionMozoId = (int)Session["MozoId"];
                string sessionMozoNombre = Session["MozoNombre"].ToString();
                DateTime sessionMozoIngreso = (DateTime)Session["MozoIngreso"];

                AuthHelper.CrearMozoCookie(sessionMozoId, sessionMozoNombre, sessionMozoIngreso);
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
                if (currentPage == "login")
                {
                    return;
                }
                // Ningún mecanismo válido, limpiar y redirigir
                AuthHelper.LimpiarYCerrarSesion();
                Response.Redirect("~/Views/Login.aspx?exp=1");
                return;
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