using ProyectoPedidosResto.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ProyectoPedidosResto.Models
{
    public class ReadingWaitersWEB
    {
        public List<Waiter> LeerMozos()
        {
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            var acceso = new DataAccess.AccesoDatos();
            var mozos = new List<Waiter>();
            string consultaSql = "SELECT IdMozo, NombreMozo, Activo FROM restaurantedb.mozos Where IdUsuario = @idusuario ;";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@idusuario", user.IdUsuario);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var mozo = new Waiter
                    {
                        Mozo_Id = acceso.Lector.GetInt32(0),
                        Mozo_Nombre = acceso.Lector.GetString(1),
                        Mozo_Activo = acceso.Lector.IsDBNull(2) ? null : acceso.Lector.GetString(2),
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

        public Waiter LeerMozos(int idmozo, string mozonombre)
        {
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            int userid = user.IdUsuario;
            if(MozoNuevo(idmozo,userid) == 0)
            {
                GuardarMozoNuevo(idmozo, mozonombre, userid);
            }
            var acceso = new DataAccess.AccesoDatos();
            var mozo = new Waiter();
            string consultaSql = "SELECT IdMozo, NombreMozo, Activo FROM restaurantedb.mozos Where IdUsuario = @idusuario AND IdMozo = @idmozo;";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@idusuario", userid);
                acceso.SetearParametro("@idmozo", idmozo);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {

                    mozo.Mozo_Id = acceso.Lector.GetInt32(0);
                    mozo.Mozo_Nombre = acceso.Lector.GetString(1);
                    mozo.Mozo_Activo = acceso.Lector.IsDBNull(2) ? null : acceso.Lector.GetString(2);                  
                    
                   
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

            return mozo;
        }


        public void CambiarEstadoMozo(int mozoId, string estado)
        {
            // Recuperar el usuario seleccionado de la sesión
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "UPDATE restaurantedb.mozos SET Activo = @estado WHERE IdMozo = @id AND IdUsuario = @idusuario ";
            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@estado", estado);
                acceso.SetearParametro("@id", mozoId);
                acceso.SetearParametro("@idusuario", user.IdUsuario);
                acceso.EjecutarAccion();
            }
            finally
            {
                acceso.CerrarConexion();
            }
        }

        
        public void GuardarMozoNuevo(int mozoId, string mozonombre, int idusuario) {
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "INSERT INTO restaurantedb.mozos (IdMozo, NombreMozo,IdUsuario, Activo)" +
                                 "Values(@idmozo, @nombremozo, @idusuario, 'NO')";
            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@idmozo", mozoId );
                acceso.SetearParametro("@nombremozo", mozonombre);
                acceso.SetearParametro("@idusuario", idusuario);
                acceso.EjecutarLectura();
            }
            finally
            {
                acceso.CerrarConexion();
            }

        }
        public int MozoNuevo(int mozoId, int idusuario)
        {
            int contador = 0;

            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "SELECT COUNT(*) FROM restaurantedb.mozos WHERE IdMozo = @idmozo AND IdUsuario = @idusuario";
            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@idmozo", mozoId);
                acceso.SetearParametro("@idusuario", idusuario);
                acceso.EjecutarLectura();
                if (acceso.Lector.Read())
                {
                    contador = acceso.Lector.GetInt32(0);
                }
            }
            finally
            {
                acceso.CerrarConexion();
            }
            return contador;
        }
    }
}