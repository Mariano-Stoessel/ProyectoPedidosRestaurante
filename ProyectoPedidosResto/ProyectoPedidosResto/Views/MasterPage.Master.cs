using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoPedidosResto.Views
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Si el usuario está logueado, muestra el menú
            pnlHamburguesa.Visible = Session["UsuarioLogueado"] != null;

        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            // cambio de estado de usuario en la BBDD


            Session.Remove("UsuarioLogueado"); // borra estado de usuario
            Response.Redirect("Login.aspx", false);
        }
    }
}