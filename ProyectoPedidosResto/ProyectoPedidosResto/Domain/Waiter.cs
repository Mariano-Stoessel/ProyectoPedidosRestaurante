using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Domain
{
    public class Waiter
    {   
        public Waiter()
        {
            Mozo_Id = -1;
            Mozo_Nombre = string.Empty;
            Mozo_Activo = string.Empty;
            Mozo_Contrasena = string.Empty;
        }

        public int Mozo_Id { get; set; }
        public string Mozo_Nombre { get; set; }
        public string Mozo_Activo { get; set; }
        public string Mozo_Contrasena { get; set; }
    }
}