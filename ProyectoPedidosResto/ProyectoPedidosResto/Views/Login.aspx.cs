using ProyectoPedidosResto.Domain;
using ProyectoPedidosResto.Models;
using ProyectoPedidosResto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace ProyectoPedidosResto.Views
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["exp"] == "1")
            {
                lblMensaje.Text = "Tu sesión ha expirado. Por favor, vuelve a iniciar sesión.";
            }

            if (!IsPostBack)
            {
                

                

                       
            }
        }

       

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            

            // Validar usuario y contraseña
            string usuario = txtUsuario.Text.Trim().ToUpper();
            string contrasena;
            if (txtPassword.Text == "")
            {
                contrasena = "";
            }
            else
            {
                contrasena = txtPassword.Text.Trim();
            }
            string mensaje = string.Empty;

            var resultado = ValidarUsuario(usuario, contrasena);

            if (resultado.EsValido)
            {
                // Cambiar estado a activo aquí


                //Guardar el ingreso del mozo en la bbdd de Empresas
                
                var readerMozos = new ReadingWaiters();
                
                
                


                // Guarda el inicio de sesión
                
                DateTime ingreso = DateTime.Now;
                readerMozos.GuardarFechaLogin(resultado.MozoId, ingreso);
                readerMozos.CambiarEstadoMozo(resultado.MozoId, "SI");
                AuthHelper.SetearMozoSession(resultado.MozoId, resultado.MozoNombre, ingreso);
                AuthHelper.CrearMozoCookie(resultado.MozoId, resultado.MozoNombre, ingreso);

                Response.Redirect(ResolveUrl("~/Views/Tables.aspx"));
            }
            else
            {
                txtUsuario.Text = string.Empty;
                txtPassword.Text = string.Empty;
                lblMensaje.Text = resultado.Mensaje;
            }

        }

        private (bool EsValido, int MozoId, string MozoNombre, string Mensaje) ValidarUsuario(string usuario, string contrasena)
        {
            // Validación de campos vacíos
            if (string.IsNullOrEmpty(usuario))
                return (false, 0, null, "Por favor, complete todos los campos.");

            var readerMozos = new ReadingWaiters();
            var mozos = readerMozos.LeerMozos();
            readerMozos.ActualizarEstadoMozosPorTiempo(DateTime.Now);


            foreach (var mozo in mozos)
            {
                string usuarioEsperado = mozo.Mozo_Nombre;
                if (usuario.Equals(usuarioEsperado, StringComparison.OrdinalIgnoreCase))
                {
                   if(mozo.Mozo_Activo == "SI")
                    {
                        return (false, 0, null, "Usuario activo en otra sesion.");
                    }


                    if (contrasena == mozo.Mozo_Contrasena || mozo.Mozo_Contrasena == null)
                    {

                        return (true, mozo.Mozo_Id, mozo.Mozo_Nombre, null);
                    }

                }
            }
            return (false, 0, null, "Usuario o contraseña incorrectos.");
        }
        
    }
}