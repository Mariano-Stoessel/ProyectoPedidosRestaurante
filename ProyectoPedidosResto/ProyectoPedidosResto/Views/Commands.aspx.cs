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
                // Lista inicial de productos
                var productos = new List<Producto>
                {
                    new Producto { Id = 1, Nombre = "Coca cola 1.5 L", Cantidad = 2, PrecioUnitario = 5000 },
                    new Producto { Id = 2, Nombre = "Milanesa c/ guarnición", Cantidad = 1, PrecioUnitario = 15000 },
                    new Producto { Id = 3, Nombre = "Sorrentinos Promo", Cantidad = 1, PrecioUnitario = 20000 },
                    new Producto { Id = 4, Nombre = "Fanta 1 L", Cantidad = 2, PrecioUnitario = 900 },
                    new Producto { Id = 5, Nombre = "Mini combo burger", Cantidad = 2, PrecioUnitario = 8000 }
                };
                Session["Productos"] = productos;
                CargarProductos();

                // Calcula el total usando la lista de la sesión
                var productosSesion = Session["Productos"] as List<Producto>;
                TotalPedido = productosSesion.Sum(p => p.Cantidad * p.PrecioUnitario);
                lblTotal.Text = "$" + TotalPedido.ToString("N2");
            }
        }

        protected void BtnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Tables.aspx");
        }

        private void CargarProductos()
        {
            var productos = Session["Productos"] as List<Producto>;
            rptProductos.DataSource = productos;
            rptProductos.DataBind();
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
    }
}
