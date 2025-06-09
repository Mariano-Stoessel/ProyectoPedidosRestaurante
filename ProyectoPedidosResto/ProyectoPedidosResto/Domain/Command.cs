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
            Com_Id_Art = -1;
            Com_Hora = string.Empty;
            Com_Cant = string.Empty;
            Com_Detalle = string.Empty;
            Com_Unitario = string.Empty;
            Com_Estado = string.Empty;
        }
        public int Com_Indice   {get; set; }
        public int Com_MesaId   {get; set;}
        public int Com_Id_Art   {get; set;}
        public string Com_Hora     {get; set;}
        public string Com_Cant     {get; set;}
        public string Com_Detalle  {get; set;}
        public string Com_Unitario {get; set;}
        public string Com_Estado   {get; set;}
    }
}