using System;
using System.Collections.Generic;
using ProyectoPedidosResto.Domain;

namespace ProyectoPedidosResto.Models
{
    public class ReadingTables
    {
        public List<Table> LeerMesas()
        {
            var mesas = new List<Table>();
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "SELECT Mesa_Id, Mesa_Estado, Mesa_Mozo, Mesa_IdMozo, Mesa_CantPer, Mesa_Obs, Mesa_UltModif FROM mesas";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var mesa = new Table
                    {
                        Mesa_Id = acceso.Lector.GetInt32(0),
                        Mesa_Estado = acceso.Lector.GetString(1),
                        Mesa_Mozo = acceso.Lector.IsDBNull(2) ? null : acceso.Lector.GetString(2),
                        Mesa_IdMozo = acceso.Lector.IsDBNull(3) ? (int?)null : acceso.Lector.GetInt32(3),
                        Mesa_CantPer = acceso.Lector.IsDBNull(4) ? null : acceso.Lector.GetString(4),
                        Mesa_Obs = acceso.Lector.IsDBNull(5) ? null : acceso.Lector.GetString(5),
                        Mesa_UltModif = acceso.Lector.IsDBNull(6) ? (DateTime?)null : acceso.Lector.GetDateTime(6),
                    };
                    mesas.Add(mesa);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al leer mesas: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }

            return mesas;
        }
    }
}