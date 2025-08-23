using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Domain
{
    public class Entry // REVISAR SI USAR ESTA CLASE
    {
        public int Ingreso_Id { get; set; }
        public int Ingreso_MozoId { get; set; }
        public string Ingreso_mozo { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Ingreso_Entrada { get; set; }
        public DateTime Ingreso_Salida { get; set; }
    }
}