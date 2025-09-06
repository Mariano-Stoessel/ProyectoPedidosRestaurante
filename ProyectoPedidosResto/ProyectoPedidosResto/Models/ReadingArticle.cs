using ProyectoPedidosResto.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Razor.Tokenizer;

namespace ProyectoPedidosResto.Models
{
    public class ReadingArticle
    {
      
        private static decimal ToDecimalSafe(object value, decimal def = 0m)
        {
            if (value == null || value == DBNull.Value) return def;

            switch (value)
            {
                case decimal d: return d;
                case double db: return Convert.ToDecimal(db, CultureInfo.InvariantCulture);
                case float f: return Convert.ToDecimal(f, CultureInfo.InvariantCulture);
                case long l: return l;
                case int i: return i;
                case short s: return s;
                case string str: return ParseFlexibleDecimal(str, def);
                default: return def;
            }
        }

        // Acepta "12,50", "1.234,50", "1,234.50", "$ 1.234,50", etc.
        private static decimal ParseFlexibleDecimal(string s, decimal def)
        {
            if (string.IsNullOrWhiteSpace(s)) return def;
            s = s.Trim();

            // deja sólo dígitos, coma, punto y signo
            s = s.Replace("\u00A0", " ");                 // non-breaking space
            s = Regex.Replace(s, @"[^\d\.,\-]", "");

            int lastComma = s.LastIndexOf(',');
            int lastDot = s.LastIndexOf('.');

            if (lastComma >= 0 && lastDot >= 0)
            {
                bool commaIsDecimal = lastComma > lastDot;
                char dec = commaIsDecimal ? ',' : '.';
                char thou = commaIsDecimal ? '.' : ',';

                s = s.Replace(thou.ToString(), "");       // quita miles
                if (dec == ',') s = s.Replace(',', '.');  // decimal = punto
            }
            else if (s.Contains(","))                    // sólo coma
            {
                var parts = s.Split(',');
                if (parts.Length == 2 && parts[1].Length <= 2)
                    s = parts[0].Replace(".", "") + "." + parts[1];
                else
                    s = s.Replace(",", "");              // comas como miles
            }
            else if (s.Contains("."))                    // sólo punto
            {
                var parts = s.Split('.');
                if (!(parts.Length == 2 && parts[1].Length <= 2))
                    s = s.Replace(".", "");              // puntos como miles
            }

            return decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var res)
                ? res : def;
        }

        private static string DecimalToDbString(decimal d) =>
            d.ToString(CultureInfo.InvariantCulture);

        // ----------------------------------------------------------
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
                        Articulo_Precio = ToDecimalSafe(acceso.Lector.GetValue(4))
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

                if (acceso.Lector.Read())
                {
                    // 🔴 leer seguro
                    return ToDecimalSafe(acceso.Lector.GetValue(0));
                }

                return 0m; // no encontrado
            }
            finally
            {
                acceso.CerrarConexion();
            }
        }
    }
}