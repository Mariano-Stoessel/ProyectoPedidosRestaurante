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

        protected void btnComanda_Click(object sender, EventArgs e)
        {
            // ARREGLAR!! (EL COMMAND ARGUMENT LLEGA VACIO O
            // CON UN STRING LITERAL DE LO QUE TIENE QUE BUSCAR EN EL FRONT; REVISAR CON Un HIDDEN)

            var btn = (Button)sender;
            if (!int.TryParse(btn.CommandArgument, out int idMesa))
            {
                // Manejar error: id inválido
                return;
            }

            var readerMesas = new ReadingTables();
            var mesa = readerMesas.LeerMesas().FirstOrDefault(m => m.Mesa_Id == idMesa);

            if (mesa != null)
            {
                int idMozo = mesa.Mesa_IdMozo ?? 0;
                string fecha = mesa.Mesa_UltModif?.ToString("yyyy-MM-dd HH:mm:ss");
                string url = $"Commands.aspx?idMesa={idMesa}&idMozo={idMozo}&fecha={HttpUtility.UrlEncode(fecha)}";
                Response.Redirect(url);
            }
        }

        protected void btnAceptarMesa_Click(object sender, EventArgs e)
        {
            var (mesaNumero, idMozo, cantidadPersonas, observaciones) = ObtenerDatosMesa();
            ActualizarMesaOcupada(mesaNumero, idMozo, cantidadPersonas, observaciones);

            Response.Redirect(Request.RawUrl);
        }

        protected void btnTomarComanda_Click(object sender, EventArgs e)
        {
            var (mesaNumero, idMozo, cantidadPersonas, observaciones) = ObtenerDatosMesa();
            ActualizarMesaOcupada(mesaNumero, idMozo, cantidadPersonas, observaciones);

            string url = $"Commands.aspx?idMesa={mesaNumero}&idMozo={idMozo}&cantidadPersonas={cantidadPersonas}&observaciones={HttpUtility.UrlEncode(observaciones)}";
            Response.Redirect(url);
        }

        private (string mesaNumero, int idMozo, int cantidadPersonas, string observaciones) ObtenerDatosMesa()
        {
            string mesaNumero = hfMesaId.Value;

            int idMozo = 0;
            int.TryParse(ddlMozos.SelectedValue, out idMozo);

            int cantidadPersonas = 1;
            int.TryParse(Request.Form["hfPersonas"], out cantidadPersonas);

            string observaciones = txtObservaciones.Text;

            return (mesaNumero, idMozo, cantidadPersonas, observaciones);
        }

        private void ActualizarMesaOcupada(string mesaNumero, int idMozo, int cantidadPersonas, string observaciones)
        {
            var readerMesas = new ReadingTables();
            var mesas = readerMesas.LeerMesas();
            var mesa = mesas.FirstOrDefault(m => m.Mesa_Id.ToString() == mesaNumero);
            if (mesa != null)
            {
                mesa.Mesa_Estado = "OCUPADA";
                mesa.Mesa_IdMozo = idMozo;
                mesa.Mesa_Mozo = ddlMozos.SelectedItem.Text;
                mesa.Mesa_CantPer = cantidadPersonas.ToString();
                mesa.Mesa_Obs = observaciones;

                readerMesas.ActualizarMesa(mesa);
            }
        }
    }
}