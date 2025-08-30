using ProyectoPedidosResto.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;

namespace ProyectoPedidosResto.Models
{
    public class ReadingTables
    {
        public List<Table> LeerMesas()
        {
            // Recuperar el usuario seleccionado de la sesión
           
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            var acceso = new DataAccess.AccesoDatos(user);
            var mesas = new List<Table>();
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

        public string BuscarIdMozo(int id, string nombremozo)
        {
            // Recuperar el usuario seleccionado de la sesión
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            var acceso = new DataAccess.AccesoDatos(user);
            Table mesas = new Table();
            string consultaSql = "SELECT Mesa_Mozo FROM mega.mesas WHERE Mesa_Id = @idmesa";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@idmesa", id);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    nombremozo= acceso.Lector.GetString(0);
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

            return nombremozo;
        }

        public List<Table> LeerMesasFiltrado(string estado = null, string texto = null)
        {
            // Recuperar el usuario seleccionado de la sesión
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            var acceso = new DataAccess.AccesoDatos(user);
            var mesas = new List<Table>();
            var condiciones = new List<string>();
            int mesaNumero = 0;
            bool esNumero = !string.IsNullOrEmpty(texto) && int.TryParse(texto, out mesaNumero);

            // Construir condiciones dinámicamente
            if (!string.IsNullOrEmpty(estado))
                condiciones.Add("Mesa_Estado = @estado");

            if (!string.IsNullOrEmpty(texto))
            {
                if (esNumero)
                    condiciones.Add("Mesa_Id = @mesaId");
                else
                    condiciones.Add("Mesa_Mozo LIKE @mozoNombre");
            }

            // Armar consulta SQL
            string consultaSql = "SELECT Mesa_Id, Mesa_Estado, Mesa_Mozo, Mesa_IdMozo, Mesa_CantPer, Mesa_Obs, Mesa_UltModif FROM mesas";
            if (condiciones.Count > 0)
                consultaSql += " WHERE " + string.Join(" AND ", condiciones);

            // Si busca por mozo, ordenar por nombre
            if (!string.IsNullOrEmpty(texto) && !esNumero)
                consultaSql += " ORDER BY Mesa_Mozo";

            try
            {
                acceso.SetearConsulta(consultaSql);

                if (!string.IsNullOrEmpty(estado))
                    acceso.SetearParametro("@estado", estado);

                if (!string.IsNullOrEmpty(texto))
                {
                    if (esNumero)
                        acceso.SetearParametro("@mesaId", mesaNumero);
                    else
                        acceso.SetearParametro("@mozoNombre", texto + "%");
                }

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

        public void ActualizarMesa(Table mesa)
        {
            // Recuperar el usuario seleccionado de la sesión
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            var acceso = new DataAccess.AccesoDatos(user);
            string consultaSql = "UPDATE Mesas SET Mesa_Estado = @estado, Mesa_IdMozo = @idMozo, Mesa_Mozo = @mozo, Mesa_CantPer = @cantPer, Mesa_Obs = @obs, Mesa_UltModif = NOW() WHERE Mesa_Id = @id";

            try
            {
                acceso.SetearConsulta(consultaSql);

                acceso.SetearParametro("@id", mesa.Mesa_Id);
                acceso.SetearParametro("@estado", mesa.Mesa_Estado);
                acceso.SetearParametro("@idMozo", mesa.Mesa_IdMozo);
                acceso.SetearParametro("@mozo", mesa.Mesa_Mozo);
                acceso.SetearParametro("@cantPer", mesa.Mesa_CantPer);
                acceso.SetearParametro("@obs", mesa.Mesa_Obs);

                acceso.EjecutarAccion();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al actualizar mesas: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();

            }
        }
    }
}