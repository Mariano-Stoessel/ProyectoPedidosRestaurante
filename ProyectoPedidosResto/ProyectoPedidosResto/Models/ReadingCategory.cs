using System;
using System.Collections.Generic;
using ProyectoPedidosResto.Domain;

namespace ProyectoPedidosResto.Models
{
    public class ReadingCategory
    {
        public List<Category> LeerCategorias(string consultaSql)
        {
            var categorias = new List<Category>();
            var acceso = new DataAccess.AccesoDatos();

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
            finally
            {
                acceso.CerrarConexion();
            }

            return categorias;
        }
    }
}