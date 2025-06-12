using ProyectoPedidosResto.Domain;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace ProyectoPedidosResto.Models
{
    public class ReadingCategory
    {
        public List<Category> LeerCategorias()
        {
            var categorias = new List<Category>();
            var acceso = new DataAccess.AccesoDatos();
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