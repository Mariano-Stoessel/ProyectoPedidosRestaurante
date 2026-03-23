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
            
            var acceso = new DataAccess.AccesoDatos();
            var mozos = new List<Waiter>();
            string consultaSql = "SELECT Mozo_Id, Mozo_Nombre, Mozo_Activo, Mozo_Contrasena FROM mozos ORDER BY Mozo_Nombre ASC ";

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
                System.Diagnostics.Debug.WriteLine("Error al leer mozos: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }

            return mozos;
        }
        
        public void CambiarEstadoMozo(int mozoId, string estado)
        {
            
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "UPDATE mozos SET Mozo_Activo = @estado WHERE Mozo_Id = @id";
            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@estado", estado);
                acceso.SetearParametro("@id", mozoId);
                acceso.EjecutarLectura();
            }
            finally
            {
                acceso.CerrarConexion();
            }
        }
        public void ActualizarEstadoMozosPorTiempo(DateTime fechaActual)
        {
            var acceso = new DataAccess.AccesoDatos();

            string consultaSql = @"
        UPDATE mozos
        SET Mozo_Activo = 'NO'
        WHERE Mozo_FecIng IS NOT NULL
        AND Mozo_Activo = 'SI'
        AND TIMESTAMPDIFF(MINUTE, Mozo_FecIng, @fechaActual) > 360";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@fechaActual", fechaActual);
                acceso.EjecutarAccion();
            }
            finally
            {
                acceso.CerrarConexion();
            }
        }

        public void GuardarFechaLogin(int mozoId, DateTime loginTime)
        {
            
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "UPDATE mozos SET Mozo_FecIng = @fecha WHERE Mozo_Id = @id";
            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@fecha", loginTime);
                acceso.SetearParametro("@id", mozoId);
                acceso.EjecutarLectura();
            }
            finally
            {
                acceso.CerrarConexion();
            }
        }
    }
}