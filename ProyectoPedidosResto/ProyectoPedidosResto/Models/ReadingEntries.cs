using ProyectoPedidosResto.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Models
{
    public class ReadingEntries
    {
        public List<Entry> LeerIngresos()
        {
            // Recuperar el usuario seleccionado de la sesión
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            var acceso = new DataAccess.AccesoDatos(user);
            var ingresos = new List<Entry>();
            string consultaSql = "SELECT Ingreso_Indice, Ingreso_IdMozo, Ingreso_Entrada, Ingreso_Salida FROM mozos_ingresos";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var ingreso = new Entry
                    {
                        Ingreso_Id = acceso.Lector.GetInt32(0),
                        Ingreso_MozoId = acceso.Lector.GetInt32(1),
                        Ingreso_Entrada = acceso.Lector.IsDBNull(2) ? (DateTime?)null : acceso.Lector.GetDateTime(2),
                        Ingreso_Salida = acceso.Lector.IsDBNull(3) ? (DateTime?)null : acceso.Lector.GetDateTime(3)
                    };
                    ingresos.Add(ingreso);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al leer ingresos: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }

            return ingresos;
        }
    }
}