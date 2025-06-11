using ProyectoPedidosResto.Domain;
using ProyectoPedidosResto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
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
                cargarDatos(); // Cargar datos de mesas y mozos al iniciar la página

                cargarDdl(); // Cargar Filtros de estado y datos en los DropDownList

            }
        }

        private void cargarDdl()
        {
            // Cargar Filtros de estado
            ddlFiltros.Items.Add(new ListItem("Todos", "1"));
            ddlFiltros.Items.Add(new ListItem("Libre", "2"));
            ddlFiltros.Items.Add(new ListItem("Reservado", "3"));
            ddlFiltros.Items.Add(new ListItem("Ocupada", "4"));

            ddlFiltros.SelectedValue = "1";

            // Cargar Mozos
            ddlMozos.Items.Clear();
            ddlMozos.Items.Add(new ListItem("Seleccione mozo", "", true));
            foreach (var mozo in Mozos)
            {
                if (mozo.Mozo_Activo == "SI") // Solo agregar mozos activos
                {
                    ddlMozos.Items.Add(new ListItem(mozo.Mozo_Nombre, mozo.Mozo_Id.ToString()));
                }
            }
        }

        private void cargarDatos()
        {
            var readerMesas = new ReadingTables();
            Mesas = readerMesas.LeerMesas();

            var readerMozos = new ReadingWaiters();
            Mozos = readerMozos.LeerMozos();
        }

        protected void ddlFiltros_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Determinar el estado según el valor seleccionado
            string filtro = ddlFiltros.SelectedValue;
            string estado = null;

            // Obtener el valor del DropDown para ver estado
            switch (filtro)
            {
                case "2": estado = "Libre"; break;
                case "3": estado = "Reservado"; break;
                case "4": estado = "Ocupada"; break;
            }

            // Obtener el texto de búsqueda desde el textbox
            string texto = txtBuscar.Text?.Trim();

            // Llamar al método que filtra desde la base de datos
            var readerMesas = new ReadingTables();
            Mesas = readerMesas.LeerMesasFiltrado(estado, texto);
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

            // Recarga los datos por defecto (todas las mesas)
            var readerMesas = new ReadingTables();
            Mesas = readerMesas.LeerMesas();
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Obtener el texto de búsqueda desde el textbox
            string texto = txtBuscar.Text?.Trim();

            // Obtener el valor del DropDown para ver estado
            string filtro = ddlFiltros.SelectedValue;
            string estado = null;

            switch (filtro)
            {
                case "2": estado = "Libre"; break;
                case "3": estado = "Reservado"; break;
                case "4": estado = "Ocupada"; break;
            }

            // Llamar al método que filtra desde la base de datos
            var readerMesas = new ReadingTables();
            if (string.IsNullOrEmpty(texto))
                Mesas = readerMesas.LeerMesas();
            else
                Mesas = readerMesas.LeerMesasFiltrado(estado, texto);
        }

        protected void btnAceptarMesa_Click(object sender, EventArgs e)
        {
            // Obtener el número de mesa desde el HiddenField
            string mesaNumero = hfMesaId.Value;

            // Obtener el id del mozo seleccionado
            int idMozo = 0;
            int.TryParse(ddlMozos.SelectedValue, out idMozo);

            // Obtener cantidad de personas
            int cantidadPersonas = 1;
            int.TryParse(Request.Form["hfPersonas"], out cantidadPersonas);

            // Obtener observaciones desde un TextBox (ejemplo: txtObservaciones)
            string observaciones = txtObservaciones.Text;

            // Aquí puedes usar las variables para guardar en BBDD

            // Recargar la página (sin redirigir a otra)
            Response.Redirect(Request.RawUrl);
        }

        protected void Comanda_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("commands.aspx");
        }
    }
}