using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoPedidosResto.Domain;
using TableDomain = ProyectoPedidosResto.Domain.Table;

namespace ProyectoPedidosResto.Views
{
    public partial class Tables : System.Web.UI.Page
    {
        public List<TableDomain> Mesas { get; set; } = new List<TableDomain>();
        public List<Waiter> Mozos { get; set; } = new List<Waiter>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // simulacion de mesas y mozos (Inicializa la propiedad Mesas (no una variable local)
                Mesas = new List<TableDomain>()
                {
                    new TableDomain { Mesa_Id = 1, Mesa_Estado = "Libre" },
                    new TableDomain { Mesa_Id = 2, Mesa_Estado = "Reservado" },
                    new TableDomain { Mesa_Id = 3, Mesa_Estado = "Ocupada", Mesa_Mozo = "Juan Pérez" },
                    new TableDomain { Mesa_Id = 4, Mesa_Estado = "Libre" },
                    new TableDomain { Mesa_Id = 5, Mesa_Estado = "Ocupada", Mesa_Mozo = "Ana Gómez" },
                    new TableDomain { Mesa_Id = 6, Mesa_Estado = "Reservado" }
                };

                // Guarda la lista de mesas en Session
                Session["MesasOriginal"] = Mesas;

                Mozos = new List<Waiter>()
                {
                    new Waiter { Mozo_Id = 1, Mozo_Nombre = "Juan Pérez", Mozo_Activo = "Si" },
                    new Waiter { Mozo_Id = 2, Mozo_Nombre = "Ana Gómez", Mozo_Activo = "Si" },
                    new Waiter { Mozo_Id = 3, Mozo_Nombre = "Wanchope Avila",  Mozo_Activo = "Si" },
                    new Waiter { Mozo_Id = 4, Mozo_Nombre = "Dormilon Diaz",  Mozo_Activo = "No" },
                };
                //

                // Cargar Filtros de estado en el DropDownList (pasar a una funcion)
                ddlFiltros.Items.Add(new ListItem("Todos", "1"));
                ddlFiltros.Items.Add(new ListItem("Libre", "2"));
                ddlFiltros.Items.Add(new ListItem("Reservado", "3"));
                ddlFiltros.Items.Add(new ListItem("Ocupada", "4"));

                ddlFiltros.SelectedValue = "1";

                // Cargar Mozos en el DropDownList (pasar a una funcion)
                foreach (var mozo in Mozos)
                {
                    if (mozo.Mozo_Activo == "Si") // Solo agregar mozos activos
                    {
                        ddlMozos.Items.Add(new ListItem(mozo.Mozo_Nombre, mozo.Mozo_Id.ToString()));
                    }
                }
            }
        }

        protected void ddlFiltros_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Recargar datos actualizados de mesas y mozos
            // (Aquí deberías obtener los datos desde tu fuente real, por ejemplo, base de datos)
            Mesas = new List<TableDomain>()
            {
                new TableDomain { Mesa_Id = 1, Mesa_Estado = "Libre" },
                new TableDomain { Mesa_Id = 2, Mesa_Estado = "Reservado" },
                new TableDomain { Mesa_Id = 3, Mesa_Estado = "Ocupada", Mesa_Mozo = "Juan Pérez" },
                new TableDomain { Mesa_Id = 4, Mesa_Estado = "Libre" },
                new TableDomain { Mesa_Id = 5, Mesa_Estado = "Ocupada", Mesa_Mozo = "Ana Gómez" },
                new TableDomain { Mesa_Id = 6, Mesa_Estado = "Reservado" }
            };

            Session["MesasOriginal"] = Mesas;

            Mozos = new List<Waiter>()
            {
                new Waiter { Mozo_Id = 1, Mozo_Nombre = "Juan Pérez", Mozo_Activo = "Si" },
                new Waiter { Mozo_Id = 2, Mozo_Nombre = "Ana Gómez", Mozo_Activo = "Si" },
                new Waiter { Mozo_Id = 3, Mozo_Nombre = "Wanchope Avila",  Mozo_Activo = "Si" },
                new Waiter { Mozo_Id = 4, Mozo_Nombre = "Dormilon Diaz",  Mozo_Activo = "No" },
            };

            // Filtrado según el filtro seleccionado
            var mesasOriginal = Session["MesasOriginal"] as List<TableDomain>;
            if (mesasOriginal == null)
                return;

            string filtro = ddlFiltros.SelectedValue;
            List<TableDomain> mesasFiltradas;

            switch (filtro)
            {
                case "1": // Todos
                    mesasFiltradas = mesasOriginal;
                    break;
                case "2": // Libre
                    mesasFiltradas = mesasOriginal.Where(m => m.Mesa_Estado.Equals("Libre", StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "3": // Reservado
                    mesasFiltradas = mesasOriginal.Where(m => m.Mesa_Estado.Equals("Reservado", StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case "4": // Ocupada
                    mesasFiltradas = mesasOriginal.Where(m => m.Mesa_Estado.Equals("Ocupada", StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                default:
                    mesasFiltradas = mesasOriginal;
                    break;
            }

            Mesas = mesasFiltradas;

            // Si tienes controles de datos, aquí deberías hacer DataBind()
            // Por ejemplo: gridViewMesas.DataSource = Mesas; gridViewMesas.DataBind();
        }

        protected void chkMisMesas_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Selecciona "Todos" en el filtro
            ddlFiltros.SelectedValue = "1";

            // Limpia el textbox de búsqueda
            txtBuscar.Text = string.Empty;

            // Desmarca el checkbox de "Mis mesas"
            chkMisMesas.Checked = false;

            // Cargar datos de nuevo
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

        protected void btnAceptarMesa_Click(object sender, EventArgs e)
        {
            // Obtener el número de mesa desde el CommandArgument
            var btn = (Button)sender;
            string mesaNumero = btn.CommandArgument;

            // Leer el valor del hidden field y obtener valor CantidadPersonas
            string personasKey = "hfPersonas" + mesaNumero;
            string personasValue = Request.Form[personasKey];

            int cantidadPersonas = 1;
            int.TryParse(personasValue, out cantidadPersonas);

            // Revisar que funcione el traspaso de datos!!

            Response.Redirect(Request.RawUrl);
        }

        protected void Comanda_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("commands.aspx");
        }
    }
}