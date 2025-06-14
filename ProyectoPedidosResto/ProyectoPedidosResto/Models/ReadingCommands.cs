using ProyectoPedidosResto.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.EnterpriseServices;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace ProyectoPedidosResto.Models
{
    public class ReadingCommands
    {
        public List<Command> LeerCommands(int mesa)
        {
            var Comandas = new List<Command>();
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "SELECT Com_Indice, Com_MesaId, Com_Detalle, Com_Cant, Com_Unitario, Com_Estado FROM mesa_comandas where Com_MesaId=@idmesa ORDER BY Com_Detalle ASC ";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@idmesa", mesa);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var comandas = new Command
                    {
                        Com_Indice = acceso.Lector.GetInt32(0),
                        Com_MesaId = acceso.Lector.GetInt32(1),
                        ArticuloNombre=acceso.Lector.IsDBNull(2) ? null : acceso.Lector.GetString(2),
                        Com_Cant =  acceso.Lector.GetInt32(3),
                        Com_Unitario= acceso.Lector.GetDecimal(4),
                        Com_Estado = acceso.Lector.IsDBNull(5) ? null : acceso.Lector.GetString(5),


                    };
                    Comandas.Add(comandas);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al leer Comandas: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }

            return Comandas;
        }


    }
}
