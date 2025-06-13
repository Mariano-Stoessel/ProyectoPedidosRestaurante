using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoPedidosResto.Domain;


namespace ProyectoPedidosResto.Views
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) { }
            if (!IsPostBack)
            {
                ddlEmpresas.Items.Add(new ListItem("Empresa 1", "1"));
                ddlEmpresas.Items.Add(new ListItem("Empresa 2", "2"));
                ddlEmpresas.Items.Add(new ListItem("Empresa 3", "3"));
            }
        }

        protected void ddlEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Validar usuario y contraseña

            // Supón que obtienes el nombre del mozo y su id:
            string mozoNombre = "FACU"; // Aquí deberías obtenerlo de la base de datos
            int idUsuario = 8; // También deberías obtenerlo de la base de datos

            bool loginValido = true; // Simulación

            if (loginValido)
            {
                Session["MozoNombre"] = mozoNombre;
                Session["MozoId"] = idUsuario;

                Session["UsuarioLogueado"] = txtUsuario.Text;
                Response.Redirect("Tables.aspx"); // Redirige a la página Tables
            }
            else
            {
                lblMensaje.Text = "Usuario o contraseña incorrectos.";
            }
        }
    }
}