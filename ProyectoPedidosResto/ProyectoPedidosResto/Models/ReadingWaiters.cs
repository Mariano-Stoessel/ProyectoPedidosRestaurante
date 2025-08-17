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
            // Recuperar el usuario seleccionado de la sesión
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            var acceso = new DataAccess.AccesoDatos(user);
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
            // Recuperar el usuario seleccionado de la sesión
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            var acceso = new DataAccess.AccesoDatos(user);
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
    }
}