using ProyectoPedidosResto.Domain.ConnectionBBDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoPedidosResto.Domain.ConnectionBBDD;


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
            // Validar usuario

            bool loginValido = true; // Simulación

            if (loginValido)
            {
                Session["UsuarioLogueado"] = true;
                Response.Redirect("Tables.aspx"); // Redirige a la página Tables
            }
            else
            {
                lblMensaje.Text = "Usuario o contraseña incorrectos.";
            }
        }
    }
}