using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Domain.Classes
{
    public class Category
    {

        public Category()
        {
            Cat_id = -1;
            Cat_nombre = string.Empty;
        }
        public int Cat_id { get; set; }
        public string Cat_nombre { get; set; }
    }
}