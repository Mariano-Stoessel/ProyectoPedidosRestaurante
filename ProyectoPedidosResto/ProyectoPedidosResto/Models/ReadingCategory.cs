using ProyectoPedidosResto.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace ProyectoPedidosResto.Models
{
    public class ReadingCategory
    {
        public List<Category> LeerCategorias()
        {
            // Recuperar el usuario seleccionado de la sesión
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");
            var acceso = new DataAccess.AccesoDatos(user);
            var categorias = new List<Category>();
            string consultaSql = "SELECT Cat_Id, Cat_Nombre FROM categorias ORDER BY Cat_Nombre ASC ";

            try
            { 
                acceso.SetearConsulta(consultaSql);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var categoria = new Category
                    {
                        Cat_id = acceso.Lector.GetInt32(0),
                        Cat_nombre = acceso.Lector.GetString(1)
                    };
                    categorias.Add(categoria);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al leer Categoris: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }

            return categorias;
        }
        
        
    }
}