using ProyectoPedidosResto.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using static ProyectoPedidosResto.Models.DataAccess;


namespace ProyectoPedidosResto.Models
{
    public class ReadingCommands
    {
        public List<Command> LeerCommands(int mesa)
        {
            var Comandas = new List<Command>();
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "SELECT Com_Indice, Com_MesaId, Com_Detalle, Com_Cant, Com_Unitario, Com_Estado FROM mesa_comandas where Com_MesaId=@idmesa ORDER BY Com_Detalle ASC ";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@idmesa", mesa);
                acceso.EjecutarLectura();

                while (acceso.Lector.Read())
                {
                    var comandas = new Command
                    {
                        Com_Indice = acceso.Lector.GetInt32(0),
                        Com_MesaId = acceso.Lector.GetInt32(1),
                        ArticuloNombre=acceso.Lector.IsDBNull(2) ? null : acceso.Lector.GetString(2),
                        Com_Cant = acceso.Lector.IsDBNull(3) ? null :  acceso.Lector.GetString(3),
                        Com_Unitario= acceso.Lector.GetDecimal(4),
                        Com_Estado = acceso.Lector.IsDBNull(5) ? null : acceso.Lector.GetString(5),


                    };
                    Comandas.Add(comandas);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al leer Comandas: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }

            return Comandas;
        }

        public void InsertarComanda(Command comanda)
        {

            var Comandas = comanda;
            var acceso = new DataAccess.AccesoDatos();
                string query = @"INSERT INTO mesa_comandas (Com_MesaId, Com_Id_Art, Com_Cant, Com_Hora, Com_Detalle, Com_Estado, Com_Unitario)
                                 VALUES (@mesaId, @articuloId, @cantidad, @hora, @Detalle, @Estado, @Unitario)";          
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(query);

                datos.SetearParametro("@mesaId", comanda.Com_MesaId);
                datos.SetearParametro("@articuloId", comanda.ArticuloIndice);
                datos.SetearParametro("@cantidad", comanda.Com_Cant);
                datos.SetearParametro("@hora", DateTime.Now.ToString("HH:mm:ss"));
                datos.SetearParametro("@Detalle", comanda.ArticuloNombre);
                datos.SetearParametro("@Estado", comanda.Com_Estado = "PEDIDO");
                datos.SetearParametro("@Unitario", comanda.Com_Unitario.ToString());
                

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void EliminarCommands(int idcomanda)
        {          
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "DELETE FROM mesa_comandas WHERE Com_Indice = @comindice ";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@comindice", idcomanda);
                acceso.EjecutarLectura();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al borrar Comandas: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }           
        }
        public void ActualizarCantidadYEstado(string nuevacantidad, int idcomanda, decimal total, string estado)
        {
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "UPDATE mega.mesa_comandas SET Com_Cant = @nuevacantidad, Com_Unitario = @ComUnitario, Com_Estado = @estado WHERE Com_Indice = @comindice ";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@nuevacantidad", nuevacantidad);
                acceso.SetearParametro("@comindice", idcomanda);
                acceso.SetearParametro("@ComUnitario", total.ToString());
                acceso.SetearParametro("@estado", estado);

                acceso.EjecutarLectura();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al Actualizar Comandas: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }
        }

        internal void CambiarMesaComandas(int mesaActualId, int mesaNuevaId)
        {
            var acceso = new DataAccess.AccesoDatos();
            string consultaSql = "UPDATE mesa_comandas SET Com_MesaId = @nuevaMesaId WHERE Com_MesaId = @mesaActualId";

            try
            {
                acceso.SetearConsulta(consultaSql);
                acceso.SetearParametro("@nuevaMesaId", mesaNuevaId);
                acceso.SetearParametro("@mesaActualId", mesaActualId);
                acceso.EjecutarAccion();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al Actualizar Comandas: " + ex.Message);
                throw;
            }
            finally
            {
                acceso.CerrarConexion();
            }
        }
    }
}
