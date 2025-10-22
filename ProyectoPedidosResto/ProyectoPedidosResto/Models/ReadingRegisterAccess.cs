using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProyectoPedidosResto.Domain;

namespace ProyectoPedidosResto.Models
{
    public class ReadingRegisterAccess
    {
        public bool RegistrarIngresoSiNoExiste(RegisterAccess registro)
        {
            // Recuperar el usuario seleccionado de la sesión
            bool existe = false;
            var acceso = new DataAccess.AccesoDatos();

            string sqlExiste = @"
                SELECT 1
                FROM Ingresos
                WHERE IdUsuario = @idusuario
                  AND Idmozo = @idmozo
                  AND YEAR(Fecha) = YEAR(CURDATE())
                  AND MONTH(Fecha) = MONTH(CURDATE())
                LIMIT 1;";

            try
            {
                acceso.SetearConsulta(sqlExiste);
                acceso.SetearParametro("@idusuario", registro.IdUsuario);
                acceso.SetearParametro("@idmozo", registro.IdMozo);
                acceso.EjecutarLectura();

                existe = acceso.Lector.Read(); // si hay fila, ya existe en el mes actual
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al verificar ingreso: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }

            if (existe) return false; // no inserta

            // Inserta porque no existe registro en el mes actual
            var accesoIns = new DataAccess.AccesoDatos();
            string sqlInsert = @"
                INSERT INTO Ingresos (IdMozo, IdUsuario, Fecha)
                VALUES (@idMozo, @idUsuario, NOW());";

            try
            {
                accesoIns.SetearConsulta(sqlInsert);
                accesoIns.SetearParametro("@idUsuario", registro.IdUsuario);
                accesoIns.SetearParametro("@idMozo", registro.IdMozo);
                accesoIns.EjecutarAccion();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al insertar ingreso: " + ex.Message);
                throw;
            }
            finally
            {
                accesoIns.CerrarConexion();
            }
        }
    }
}