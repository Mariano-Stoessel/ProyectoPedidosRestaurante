using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoPedidosResto.Views
{
    public partial class Tables : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlFiltros.Items.Add(new ListItem("Todos", "1"));
                ddlFiltros.Items.Add(new ListItem("Disponible", "2"));
                ddlFiltros.Items.Add(new ListItem("Reservado", "3"));
                ddlFiltros.Items.Add(new ListItem("Ocupado", "4"));
            }
        }

        protected void ddlFiltros_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void chkMisMesas_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {

        }
      
        protected void BtnCargarMesa_Click(object sender, EventArgs e)
        {

           
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Aquí va la lógica para filtrar o actualizar los datos según el texto ingresado
            // Por ejemplo:
            string texto = txtBuscar.Text;
            // Filtra tus datos aquí
        }
    }
}