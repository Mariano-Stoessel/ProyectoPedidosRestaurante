using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; // Cambiado para MySQL
using ProyectoPedidosResto.Domain;

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
                var csb = new MySqlConnectionStringBuilder
                {
                    //conexion local
                    Server = "localhost",
                    Database = "mega",
                    UserID = "root",
                    Password = "meko",
                    Port = 3306,
                    SslMode = 0
                     
                    /*
                    Server =  "190.103.205.57" ,
                     Database = "restaurantedb",
                     UserID = "Mariano",
                     Password = "@@Tormenta1420!",
                     Port= 3306,
                     SslMode = 0
                    */                   
                };

                
                conexion = new MySqlConnection(csb.ConnectionString);
                comando = new MySqlCommand { Connection = conexion };
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
