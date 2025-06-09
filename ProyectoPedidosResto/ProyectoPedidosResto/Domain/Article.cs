using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Domain
{
    public class Article
    {

        public  Article()
        {
            Articulo_Indice = -1;
            Articulo_nombre = string.Empty;
            Articulo_Stock = string.Empty;
            Articulo_Precio = string.Empty;
            Articulo_Categoria = new Category();
            Estado = true;

        }
        public int Articulo_Indice { get; set; }
        public string Articulo_nombre { get; set; }
        public string Articulo_Stock { get; set; }
        public string Articulo_Precio { get; set; }
        public Category Articulo_Categoria { get; set; }
        public bool Estado { get; set; }

    }
}