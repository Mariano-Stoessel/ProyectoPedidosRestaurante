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
            Articulo_Nombre = string.Empty;
            Articulo_Stock = string.Empty;
            Articulo_Precio = string.Empty;
            Articulo_Categoria = string.Empty;
            Estado = true;

        }
        public int Articulo_Indice { get; set; }
        public string Articulo_Nombre { get; set; }
        public string Articulo_Stock { get; set; }
        public string Articulo_Precio { get; set; }
        public string Articulo_Categoria { get; set; }
        public bool Estado { get; set; }

    }
}