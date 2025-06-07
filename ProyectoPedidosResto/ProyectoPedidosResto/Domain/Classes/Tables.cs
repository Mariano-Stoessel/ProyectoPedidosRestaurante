using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Domain.Classes
{
    public class Tables
    {
        public Tables()
        {
            Mesa_Id = -1;
            Mesa_Estado = string.Empty;
            Mesa_Mozo = string.Empty;
            Mesa_IdMozo = -1;
            Mesa_UltModif = DateTime.Now;
            Mesa_CantPer = string.Empty;
            Mesa_Obs = string.Empty;
        }
        public int Mesa_Id { get; set; }
        public  string Mesa_Estado { get; set;}
        public string Mesa_Mozo { get; set; }
        public int Mesa_IdMozo { get; set; }
        public DateTime Mesa_UltModif { get; set; }
        public string Mesa_CantPer { get; set; }
        public string Mesa_Obs { get; set; }






    }
}