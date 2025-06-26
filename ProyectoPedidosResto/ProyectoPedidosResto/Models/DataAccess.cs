using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; // Cambiado para MySQL

namespace ProyectoPedidosResto.Models
{
    public class DataAccess
    {
        public class AccesoDatos
        {
            private MySqlConnection conexion;
            private MySqlCommand comando;
            private MySqlDataReader lector;

            public MySqlDataReader Lector
            {
                get { return lector; }
            }

            public AccesoDatos()
            {
                conexion = new MySqlConnection("Server=localhost;Port=3306;Database=mega;Uid=root;Pwd=meko;");
                comando = new MySqlCommand();
            }
            public void SetearConsulta(string consulta)
            {
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = consulta;
            }
            public void SetearParametro(string nombre, object valor)
            {
                comando.Parameters.AddWithValue(nombre, valor);
            }

            public void EjecutarLectura()
            {
                comando.Connection = conexion;
                try
                {
                    conexion.Open();
                    lector = comando.ExecuteReader();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void EjecutarAccion()
            {
                comando.Connection = conexion;
                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void CerrarConexion()
            {
                if (lector != null)
                    lector.Close();
                conexion.Close();
            }

            public bool ProbarConexion(out string mensaje)
            {
                try
                {
                    conexion.Open();
                    mensaje = "Conexión exitosa.";
                    return true;
                }
                catch (Exception ex)
                {
                    mensaje = $"Error de conexión: {ex.Message}";
                    return false;
                }
                finally
                {
                    if (conexion.State == System.Data.ConnectionState.Open)
                        conexion.Close();
                }
            }
        }
    }
}
