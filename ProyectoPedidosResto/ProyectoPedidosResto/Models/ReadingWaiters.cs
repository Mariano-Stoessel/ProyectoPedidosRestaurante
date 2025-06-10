using ProyectoPedidosResto.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Models
{
    public class ReadingWaiters
    {
        public List<Waiter> LeerMozos()
        {
            var mozos = new List<Waiter>();
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "SELECT Mozo_Id, Mozo_Nombre, Mozo_Activo, Mozo_Contrasena FROM mozos";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var mozo = new Waiter
                    {
                        Mozo_Id = acceso.Lector.GetInt32(0),
                        Mozo_Nombre = acceso.Lector.GetString(1),
                        Mozo_Activo = acceso.Lector.IsDBNull(2) ? null : acceso.Lector.GetString(2),
                        Mozo_Contrasena = acceso.Lector.IsDBNull(3) ? null : acceso.Lector.GetString(3),
                    };
                    mozos.Add(mozo);
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

            return mozos;
        }
    }
}