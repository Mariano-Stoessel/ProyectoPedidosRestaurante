using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Domain
{
    public class User
    {
       
            public int IdUsuario { get; set; }
            public string Nombre { get; set; }
            public string Logo { get; set; }          // Directorio/URL del logo
            public string IP { get; set; }
            public string Port { get; set; }
            public string DatabaseName { get; set; }
            public string UsuarioDB { get; set; }     // Para diferenciar del nombre propio
            public string Password { get; set; }
            public bool Activo { get; set; }
            public int IdTipoUsuario { get; set; }
            public int CantidadIngreso { get; set; }
            public DateTime? FechaUltimoIngreso { get; set; }
        


    }
}