using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Crypto;
using ProyectoPedidosResto.Domain;
using ProyectoPedidosResto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoPedidosResto.Views
{
    // Simulación de Productos
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
    public class ProductoLista
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public partial class Commands : System.Web.UI.Page
    {
        List<Article> articulos = new List<Article>();
        List<Command> commands = new List<Command>();

        public decimal TotalPedido { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
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
            string idSeleccionado = hfProductoListaSeleccionado.Value;
            // Aquí puedes usar idSeleccionado para eliminar el producto de la lista
            // Ejemplo:
            // EliminarProductoDeLista(idSeleccionado);
        }

        protected void btnAceptarCantidad_Click(object sender, EventArgs e)
        {
            int nuevaCantidad;
            if (int.TryParse(hfNuevaCantidad.Value, out nuevaCantidad))
            {
                // Aquí tienes el valor de la cantidad elegida en el modal
                // Puedes usarlo para actualizar el producto seleccionado
            }
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
            String precioUnitario = hfPrecioProductoSeleccionado.Value;
            var insertcomandas = new ReadingCommands();
            Command comanda = new Command();
            comanda.Com_MesaId= int.Parse(Request.QueryString["idMesa"]);
            comanda.ArticuloIndice = idProducto;
            comanda.ArticuloNombre = NombreProducto;
            comanda.Com_Cant = cantidad;
            comanda.Com_Unitario = decimal.Parse(cantidad.ToString()) * decimal.Parse(precioUnitario);
            insertcomandas.InsertarComanda(comanda);
            CargarProductosLista(comanda.Com_MesaId.ToString());
            CargarTotal();





            // Tu lógica para agregar el producto al pedido
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



    }
}
