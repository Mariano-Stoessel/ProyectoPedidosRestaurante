using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoPedidosResto.Domain;

namespace ProyectoPedidosResto.Models
{
    public class ReadingUser
    {
        public List<User> LeerUsuarios()
        {
            var usuarios = new List<User>();
            var acceso = new DataAccess.AccesoDatos();

            string consultaSql = @"SELECT 
                                        IdUsuario,
                                        Nombre,
                                        Logo,
                                        IP,
                                        Port,
                                        DatabaseName,
                                        Usuario,           
                                        Password,
                                        Activo,
                                        IdTipoUsuario,
                                        CantidadIngreso,
                                        FechaUltimoIngreso
                                    FROM Usuarios";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var u = new User
                    {
                        IdUsuario = acceso.Lector.GetInt32(0),
                        Nombre = acceso.Lector.GetString(1),
                        Logo = acceso.Lector.IsDBNull(2) ? null : acceso.Lector.GetString(2),
                        IP = acceso.Lector.IsDBNull(3) ? null : acceso.Lector.GetString(3),
                        Port = acceso.Lector.IsDBNull(4) ? null : acceso.Lector.GetString(4),
                        DatabaseName = acceso.Lector.IsDBNull(5) ? null : acceso.Lector.GetString(5),
                        UsuarioDB = acceso.Lector.IsDBNull(6) ? null : acceso.Lector.GetString(6),
                        Password = acceso.Lector.IsDBNull(7) ? null : acceso.Lector.GetString(7),
                        // Activo puede venir como TINYINT(1) en MySQL: Convert.ToBoolean lo maneja bien
                        Activo = !acceso.Lector.IsDBNull(8) && Convert.ToBoolean(acceso.Lector.GetValue(8)),
                        IdTipoUsuario = acceso.Lector.IsDBNull(9) ? 0 : acceso.Lector.GetInt32(9),
                        CantidadIngreso = acceso.Lector.IsDBNull(10) ? 0 : acceso.Lector.GetInt32(10),
                        FechaUltimoIngreso = acceso.Lector.IsDBNull(11) ? (DateTime?)null : acceso.Lector.GetDateTime(11)
                    };

                    usuarios.Add(u);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al leer usuarios: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }

            return usuarios;
        }
    }
}