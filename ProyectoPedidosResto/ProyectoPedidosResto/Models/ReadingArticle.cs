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
            // Recuperar el usuario seleccionado de la sesión
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");

           List<Article> articulos = new List<Article>();
            var acceso = new DataAccess.AccesoDatos(user);
            string consultaSql = " SELECT Articulo_Indice, Articulo_Nombre, Articulo_Stock, Articulo_Categoria, Articulo_Precio FROM Articulos WHERE Articulo_Stock > 0 ORDER BY Articulo_Nombre ASC ";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var articulo = new Article
                    {
                        Articulo_Indice = acceso.Lector.GetInt32(0),
                        Articulo_Nombre = acceso.Lector.GetString(1),
                        Articulo_Stock = acceso.Lector.GetString(2),
                        
                        Articulo_Categoria = acceso.Lector.GetString(3),
                        Articulo_Precio = acceso.Lector.GetDecimal(4)
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
            // Recuperar el usuario seleccionado de la sesión
            var user = HttpContext.Current.Session["UsuarioSeleccionado"] as User;
            if (user == null)
                throw new InvalidOperationException("No se encontró el usuario seleccionado en la sesión.");

            var articulos = new Article();
            var acceso = new DataAccess.AccesoDatos(user);
            string consultaSql = " SELECT Articulo_Precio FROM mega.articulos WHERE Articulo_Nombre = @NombreArticulo ";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@NombreArticulo", NombreArticulo);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var articulo = new Article
                    {
                        Articulo_Precio = acceso.Lector.GetDecimal(0),
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