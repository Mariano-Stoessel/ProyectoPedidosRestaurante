using ProyectoPedidosResto.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ProyectoPedidosResto.Models
{
    public class ReadingArticle
    {
        public List<Article> LeerArticulos()
        {
            var articulos = new List<Article>();
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = " SELECT Articulo_Indice, Articulo_Nombre, Articulo_Stock, Articulo_Categoria, Articulo_Precio FROM Articulos WHERE Articulo_Stock > 0 ORDER BY Articulo_Nombre ASC ";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var articulo = new Article
                    {
                        Articulo_Indice = acceso.Lector.IsDBNull(0) ? 0 : acceso.Lector.GetInt32(0),
                        Articulo_Nombre = acceso.Lector.IsDBNull(1) ? string.Empty : acceso.Lector.GetString(1),
                        Articulo_Stock = acceso.Lector.IsDBNull(2) ? string.Empty : acceso.Lector[2].ToString(),
                        Articulo_Categoria = acceso.Lector.IsDBNull(3) ? string.Empty : acceso.Lector.GetString(3),
                        Articulo_Precio = acceso.Lector.IsDBNull(0) ? 0m : Convert.ToDecimal(acceso.Lector[0])

                    };
                    articulos.Add(articulo);
                }
            }
            finally
            {
                acceso.CerrarConexion();
            }

            return articulos;
        }
        public decimal LeerPrecioArticulos_X_Nombre(string NombreArticulo)
        {
            
            var articulos = new Article();
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = " SELECT Articulo_Precio FROM articulos WHERE Articulo_Nombre = @NombreArticulo ";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@NombreArticulo", NombreArticulo);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var articulo = new Article
                    {
                        Articulo_Precio = acceso.Lector.IsDBNull(0) ? 0m : Convert.ToDecimal(acceso.Lector[0]),
                    };
                    articulos=articulo;
                }
            }
            finally
            {
                acceso.CerrarConexion();
            }
            
            return articulos.Articulo_Precio;
        }
    }
}