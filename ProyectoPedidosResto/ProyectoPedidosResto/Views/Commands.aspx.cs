using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Crypto;
using ProyectoPedidosResto.Domain;
using ProyectoPedidosResto.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComandsDomain = ProyectoPedidosResto.Domain.Command;

namespace ProyectoPedidosResto.Views
{

    public partial class Commands : System.Web.UI.Page
    {
        List<Article> articulos = new List<Article>();
       // List<Command> commands = new List<Command>();
        public List<ComandsDomain> commands { get; set; } = new List<ComandsDomain>();
        public decimal TotalPedido { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            validarUsuarioActivo();

            if (!IsPostBack)
            {
                string idMesa = Request.QueryString["idMesa"];
                if (!string.IsNullOrEmpty(idMesa)) lblIdMesa.Text = idMesa;

                CargarCategorias();

                CargarProductos();

                CargarProductosLista(idMesa);

                //CargarMozo(idMesa);

                // Calcula el total usando la lista de la sesión
                CargarTotal();

            }
        }

        protected void btnEliminarProducto_Click(object sender, EventArgs e)
        {
            EliminarComanda();
        }

        protected void btnAceptarCantidad_Click(object sender, EventArgs e)
        {
            int nuevaCantidad;
            if (int.TryParse(hfNuevaCantidad.Value, out nuevaCantidad))
            {
                if (nuevaCantidad == 0) { EliminarComanda(); } else
                {
                ActualizarCantidad(nuevaCantidad);
                }


                // Aquí tienes el valor de la cantidad elegida en el modal
                // Puedes usarlo para actualizar el producto seleccionado
            }
        }
        private void ActualizarCantidad(int nuevaCantidad) {
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
            borrarcomanda.ElimiarCommands(Idcomanda);
            CargarProductosLista(lblIdMesa.Text);
            CargarTotal();
        }

        private void CargarTotal()
        {
            var productosListaSesion = commands;
            TotalPedido = productosListaSesion.Sum(p => int.Parse(p.Com_Cant) * p.Com_Unitario);
            lblTotal.Text = "$" + TotalPedido.ToString("N2");
        }
        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            int idProducto = int.Parse(hfProductoSeleccionado.Value);
            string NombreProducto = hfNombreProductoSeleccionado.Value.ToString();
            string cantidad = hfCantidad.Value;
            string precioUnitario = hfPrecioProductoSeleccionado.Value;
            InsertarComandas(idProducto, NombreProducto, cantidad, precioUnitario);

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
        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Tables.aspx");
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
        /* private void CargarMozo(String idmozo)
         {

         }*/
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
