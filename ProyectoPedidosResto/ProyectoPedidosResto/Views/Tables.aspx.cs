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
        // simulacion de mesas
        public class Mesa
        {
            public int Numero { get; set; }
            public string Estado { get; set; }  // "libre", "reservado", "ocupado"
            public string NombrePersona { get; set; }  // solo si está ocupado
        }
        public List<Mesa> Mesas { get; set; } = new List<Mesa>();
        //

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // simulacion de mesas (Inicializa la propiedad Mesas (no una variable local)
                Mesas = new List<Mesa>()
                {
                    new Mesa { Numero = 1, Estado = "libre" },
                    new Mesa { Numero = 2, Estado = "reservado" },
                    new Mesa { Numero = 3, Estado = "ocupado", NombrePersona = "Juan Pérez" },
                    new Mesa { Numero = 4, Estado = "libre" },
                    new Mesa { Numero = 5, Estado = "ocupado", NombrePersona = "Ana Gómez" },
                    new Mesa { Numero = 6, Estado = "reservado" }
                };
                //

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

        protected void Comanda_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("commands.aspx");
        }
    }
}