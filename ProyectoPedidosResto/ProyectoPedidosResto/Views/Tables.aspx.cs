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
            if (!User.Identity.IsAuthenticated)
            {
                var authCookie = Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    try
                    {
                        var ticket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                        if (ticket != null)
                        {
                            int mozoId;
                            if (int.TryParse(ticket.UserData, out mozoId))
                            {
                                var readerMozos = new ReadingWaiters();
                                readerMozos.CambiarEstadoMozo(mozoId, "NO");
                            }
                        }
                    }
                    catch { /* Ignorar errores de cookie corrupta */ }
                }

                Response.Redirect("Login.aspx");
                return;
            }

            validarUsuarioActivo();

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
            // Obtener el id del mozo logueado desde la sesión
            int idMozo = 0;
            if (Session["MozoId"] != null)
            {
                int.TryParse(Session["MozoId"].ToString(), out idMozo);
            }

            var readerMesas = new ReadingTables();

            //Hacerlo por BBDD directo despues

            if (chkMisMesas.Checked && idMozo > 0)
            {
                // Filtrar mesas por idMozo
                Mesas = readerMesas.LeerMesas().Where(m => m.Mesa_IdMozo == idMozo).ToList();
            }
            else
            {
                // Mostrar todas las mesas
                Mesas = readerMesas.LeerMesas();
            }
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
            var (mesaNumero, idMozo, cantidadPersonas, observaciones) = ObtenerDatosMesa();
            ActualizarMesaOcupada(mesaNumero, idMozo, cantidadPersonas, observaciones);

            Response.Redirect(Request.RawUrl);
        }

        protected void btnTomarComanda_Click(object sender, EventArgs e)
        {
            var (mesaNumero, idMozo, cantidadPersonas, observaciones) = ObtenerDatosMesa();
            ActualizarMesaOcupada(mesaNumero, idMozo, cantidadPersonas, observaciones);

            string url = $"Commands.aspx?idMesa={mesaNumero}";
            Response.Redirect(url);
        }

        private (string mesaNumero, int idMozo, int cantidadPersonas, string observaciones) ObtenerDatosMesa()
        {
            string mesaNumero = hfMesaSeleccionadaId.Value;

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
        private void validarUsuarioActivo()
        {
            if (Session["MozoId"] == null)
            {
                var authCookie = Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var ticket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null)
                    {
                        int mozoId = int.Parse(ticket.UserData);
                        string mozoNombre = ticket.Name;

                        // Validar el mozo en la base de datos antes de restaurar la sesión
                        var readerMozos = new ReadingWaiters();
                        var mozo = readerMozos.LeerMozos().FirstOrDefault(m => m.Mozo_Id == mozoId);

                        if (mozo == null || mozo.Mozo_Activo != "SI")
                        {
                            // Si el mozo no existe o no está activo, cerrar sesión y redirigir
                            System.Web.Security.FormsAuthentication.SignOut();
                            Session.Clear();
                            Response.Redirect("Login.aspx");
                            return;
                        }

                        Session["MozoId"] = mozoId;
                        Session["MozoNombre"] = mozoNombre;
                    }
                }
            }
        }
    }
}