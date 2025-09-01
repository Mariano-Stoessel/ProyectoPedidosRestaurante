using ProyectoPedidosResto.Domain;
using ProyectoPedidosResto.Models;
using ProyectoPedidosResto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComandsDomain = ProyectoPedidosResto.Domain.Command;

namespace ProyectoPedidosResto.Views
{

    public partial class Commands : System.Web.UI.Page
    {
        List<Article> articulos = new List<Article>();
        Waiter mozo = new Waiter();
        // List<Command> commands = new List<Command>();
        public List<ComandsDomain> commands { get; set; } = new List<ComandsDomain>();
        public decimal TotalPedido { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Validar sesión antes de continuar
            if (Session["MozoId"] == null)
            {
                Response.Redirect("~/Views/Login.aspx?exp=1");
                return;
            }

            if (!EsMesaValidaYNoLibre())
            {
                Response.Redirect("Tables.aspx");
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                string idMesa = Request.QueryString["idMesa"];
                if (!string.IsNullOrEmpty(idMesa)) lblIdMesa.Text = idMesa;

                CargarCategorias();
                CargarProductos();
                CargarProductosLista(idMesa);
                CargarMesasLibres();
                Mozo_A_Cargo();
                CargarTotal();

            }

            if (Request.QueryString["cambioMesa"] == "ok")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertCambioMesa",
                    "alert('¡Cambio de mesa exitoso!');", true);
            }
        }

        protected void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            EliminarComanda();
        }

        protected void btnModificarProducto_Click(object sender, EventArgs e)
        {
            int nuevaCantidad;
            if (int.TryParse(hfNuevaCantidad.Value, out nuevaCantidad))
            {
                if (nuevaCantidad == 0) { EliminarComanda(); }
                else
                {
                    ActualizarCantidad(nuevaCantidad);
                }

            }
        }

        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfProductoSeleccionado.Value, out int idProducto))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertIdProducto",
                    $"alert('Error: El producto seleccionado no es válido. Valor: {hfProductoSeleccionado.Value}');", true);
                return;
            }
            string NombreProducto = hfNombreProductoSeleccionado.Value.ToString();
            string cantidad = hfCantidad.Value;
            string precioUnitario = hfPrecioProductoSeleccionado.Value;
            InsertarComandas(idProducto, NombreProducto, cantidad, precioUnitario);

        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {

            Response.Redirect("Tables.aspx");
        }

        protected void btnAceptarCambiarMesa_Click(object sender, EventArgs e)
        {
            int mesaActualId = int.Parse(lblIdMesa.Text);
            int mesaNuevaId = int.Parse(ddlMesasLibres.SelectedValue);

            var readerCommands = new ReadingCommands();
            readerCommands.CambiarMesaComandas(mesaActualId, mesaNuevaId);

            var readerTables = new ReadingTables();
            var mesas = readerTables.LeerMesas();

            var mesaNueva = mesas.FirstOrDefault(m => m.Mesa_Id == mesaNuevaId);
            var mesaAnterior = mesas.FirstOrDefault(m => m.Mesa_Id == mesaActualId);

            if (mesaNueva != null && mesaAnterior != null)
            {
                mesaNueva.Mesa_Estado = "OCUPADA";
                mesaNueva.Mesa_IdMozo = mesaAnterior.Mesa_IdMozo;
                mesaNueva.Mesa_Mozo = mesaAnterior.Mesa_Mozo;
                mesaNueva.Mesa_CantPer = mesaAnterior.Mesa_CantPer;
                mesaNueva.Mesa_Obs = mesaAnterior.Mesa_Obs;
                mesaNueva.Mesa_UltModif = DateTime.Now; // Actualizar la fecha de última modificación
                readerTables.ActualizarMesa(mesaNueva);
            }

            if (mesaAnterior != null)
            {
                mesaAnterior.Mesa_Estado = "LIBRE";
                mesaAnterior.Mesa_IdMozo = null;
                mesaAnterior.Mesa_Mozo = null;
                mesaAnterior.Mesa_CantPer = null;
                mesaAnterior.Mesa_Obs = null;
                mesaAnterior.Mesa_UltModif = null; // Actualizar la fecha de última modificación??
                readerTables.ActualizarMesa(mesaAnterior);
            }

            Response.Redirect($"Commands.aspx?idMesa={mesaNuevaId}&cambioMesa=ok");
        }

        private void CargarMesasLibres()
        {
            var reader = new ReadingTables();
            var mesas = reader.LeerMesas(); // Devuelve todas las mesas

            ddlMesasLibres.Items.Clear();
            foreach (var mesa in mesas)
            {
                var item = new ListItem($"Mesa {mesa.Mesa_Id} ({mesa.Mesa_Estado})", mesa.Mesa_Id.ToString());
                if (!string.Equals(mesa.Mesa_Estado, "LIBRE", StringComparison.OrdinalIgnoreCase))
                    item.Attributes.Add("disabled", "disabled");
                ddlMesasLibres.Items.Add(item);
            }
        }

        private bool EsMesaValidaYNoLibre()
        {
            string idMesaStr = Request.QueryString["idMesa"];
            int mesaId;
            if (string.IsNullOrEmpty(idMesaStr) || !int.TryParse(idMesaStr, out mesaId) || mesaId <= 0)
                return false;

            var readerMesas = new ReadingTables();
            var mesa = readerMesas.LeerMesas().FirstOrDefault(m => m.Mesa_Id == mesaId);
            if (mesa == null) return false; // No existe
            if (mesa.Mesa_Estado == "LIBRE") return false; // Está libre

            return true;
        }

        private void Mozo_A_Cargo()
        {
            ReadingTables readingTables = new ReadingTables();
            lblMozo.Text = "Mozo: " + readingTables.BuscarIdMozo(int.Parse(lblIdMesa.Text), lblMozo.Text);
        }

        private void ActualizarCantidad(int nuevaCantidad)
        {
            var buscarprecioArticulo = new ReadingArticle();
            decimal Com_Unitario = buscarprecioArticulo.LeerPrecioArticulos_X_Nombre((hfArticuloNombreoListaSeleccionado.Value).ToString());
            string nuevacantidad = nuevaCantidad.ToString();
            int idcomanda = int.Parse(hfProductoListaSeleccionado.Value);
            string estado = ddlEstado.SelectedValue;

            var actualizarCantidad = new ReadingCommands();
            actualizarCantidad.ActualizarCantidadYEstado(nuevacantidad, idcomanda, Com_Unitario, estado);
            CargarProductosLista(lblIdMesa.Text);
            CargarTotal();
        }

        private void EliminarComanda()
        {
            int Idcomanda = int.Parse(hfProductoListaSeleccionado.Value);
            var borrarcomanda = new ReadingCommands();
            borrarcomanda.EliminarCommands(Idcomanda);
            CargarProductosLista(lblIdMesa.Text);
            CargarTotal();
        }

        private void CargarTotal()
        {
            var productosListaSesion = commands;
            TotalPedido = productosListaSesion.Sum(p => int.Parse(p.Com_Cant) * p.Com_Unitario);
            lblTotal.Text = "$" + TotalPedido.ToString("N2");
        }

        private void InsertarComandas(int idProducto, string NombreProducto, string cantidad, string precioUnitario)
        {
            var insertcomandas = new ReadingCommands();
            Command comanda = new Command();
            comanda.Com_MesaId = int.Parse(Request.QueryString["idMesa"]);
            comanda.ArticuloIndice = idProducto;
            comanda.ArticuloNombre = NombreProducto;
            comanda.Com_Cant = cantidad;
            comanda.Com_Unitario = decimal.Parse(cantidad.ToString()) * decimal.Parse(precioUnitario);
            insertcomandas.InsertarComanda(comanda);
            CargarProductosLista(comanda.Com_MesaId.ToString());
            CargarTotal();
        }

        private void CargarProductos()
        {
            var reader = new ReadingArticle();
            articulos = reader.LeerArticulos();
            rptProductos.DataSource = articulos;
            rptProductos.DataBind();
        }
        private void CargarProductosLista(string idMesa)
        {
            var reader = new ReadingCommands();
            int IdMesa = Convert.ToInt32(idMesa);

            commands = reader.LeerCommands(IdMesa);
            rptProductosLista.DataSource = commands;
            rptProductosLista.DataBind();
        }

        private void CargarCategorias()
        {
            var reader = new ReadingCategory();
            List<Category> categorias = reader.LeerCategorias();

            ddlCategorias.Items.Clear();
            ddlCategorias.Items.Add(new ListItem("TODOS", "")); // Opción por defecto

            foreach (var cat in categorias)
            {
                // Usamos Cat_nombre como Text Y como Value
                var nombre = cat.Cat_nombre.Trim();
                ddlCategorias.Items.Add(new ListItem(nombre, nombre));
            }
        }
    }
}
