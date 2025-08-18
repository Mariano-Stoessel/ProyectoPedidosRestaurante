using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Domain
{
    public class RegisterAccess
    {
        public int IdIngreso { get; set; }
        public string NombreMozo { get; set; }
        public int IdUsuario { get; set; }     // FK a Usuarios.IdUsuario (representa el restaurant/usuario)
        public DateTime Fecha { get; set; }
    }
}