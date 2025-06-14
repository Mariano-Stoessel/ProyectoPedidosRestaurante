using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Domain
{
    public class Command
    {
        public Command() {
            Com_Indice = -1;
            Com_MesaId = -1;
            ArticuloIndice = -1;
            ArticuloNombre = string.Empty;
            Com_Hora = DateTime.Now;
            Com_Cant = -1;
            Com_Unitario = 0m;
            Com_Estado = string.Empty;
        }
        public int Com_Indice   {get; set; }
        public int Com_MesaId   {get; set;}
        public DateTime Com_Hora     {get; set;}
        public int Com_Cant     {get; set;}
        public int ArticuloIndice { get; set; }
        public string ArticuloNombre{get; set;}
        
        public decimal Com_Unitario {get; set;}
        public string Com_Estado   {get; set;}
    }
}