using Microsoft.Ajax.Utilities;
using ProyectoPedidosResto.Domain;
using ProyectoPedidosResto.Domain.ConnectionBBDD;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public decimal TotalPedido { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCategorias();

                // Lista inicial de productos
                var productos = new List<Producto>
                {
                    new Producto { Id = 1, Nombre = "Coca cola 1.5 L", Stock = 23, PrecioUnitario = 5000 },
                    new Producto { Id = 2, Nombre = "Milanesa c/ guarnición", Stock = 12, PrecioUnitario = 15000 },
                    new Producto { Id = 3, Nombre = "Sorrentinos Promo", Stock = 16, PrecioUnitario = 20000 },
                    new Producto { Id = 4, Nombre = "Fanta 1 L", Stock = 26, PrecioUnitario = 900 },
                    new Producto { Id = 5, Nombre = "Mini combo burger", Stock = 20, PrecioUnitario = 8000 }
                };
                CargarProductos(productos);

                //Lista de productos pedidos (ejemplo)
                var productosLista = new List<ProductoLista>
                {
                    new ProductoLista { Id = 1, Nombre = "Coca cola 1.5 L", Cantidad = 2, PrecioUnitario = 5000 },
                    new ProductoLista { Id = 2, Nombre = "Milanesa c/ guarnición", Cantidad = 1, PrecioUnitario = 15000 },
                    new ProductoLista { Id = 3, Nombre = "Sorrentinos Promo", Cantidad = 1, PrecioUnitario = 20000 },
                    new ProductoLista { Id = 4, Nombre = "Fanta 1 L", Cantidad = 2, PrecioUnitario = 900 },
                    new ProductoLista { Id = 5, Nombre = "Mini combo burger", Cantidad = 2, PrecioUnitario = 8000 }
                };
                Session["productosLista"] = productosLista;
                CargarProductosLista();

                // Calcula el total usando la lista de la sesión
                var productosListaSesion = Session["productosLista"] as List<ProductoLista>;
                TotalPedido = productosListaSesion.Sum(p => p.Cantidad * p.PrecioUnitario);
                lblTotal.Text = "$" + TotalPedido.ToString("N2");
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

        protected void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            string idProducto = hfProductoSeleccionado.Value;
            string cantidad = hfCantidad.Value;
            // Tu lógica para agregar el producto al pedido
        }
        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Tables.aspx");
        }

        private void CargarProductos(List<Producto> productos)
        {
            rptProductos.DataSource = productos;
            rptProductos.DataBind();
        }

        private void CargarProductosLista()
        {
            var productosLista = Session["productosLista"] as List<ProductoLista>;
            rptProductosLista.DataSource = productosLista;
            rptProductosLista.DataBind();
        }
        private void CargarCategorias()
        {
            var reader = new ReadingCategory();
            string consulta = "SELECT Cat_Id, Cat_Nombre FROM categorias";
            List<Category> categorias = reader.LeerCategorias(consulta);

            ddlCategorias.Items.Clear();
            ddlCategorias.Items.Add(new ListItem("Todos", "")); // Opción por defecto

            foreach (var cat in categorias)
            {
                ddlCategorias.Items.Add(new ListItem(cat.Cat_nombre, cat.Cat_id.ToString()));
            }
        }
    }
}
