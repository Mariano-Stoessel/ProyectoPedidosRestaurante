using ProyectoPedidosResto.Domain;
using ProyectoPedidosResto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ProyectoPedidosResto.Views
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                var authCookie = Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    var ticket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null)
                    {
                        string[] userData = ticket.UserData.Split('|');
                        int mozoId = int.Parse(userData[0]);
                        string mozoNombre = userData[1];

                        Session["MozoId"] = mozoId;
                        Session["MozoNombre"] = mozoNombre;
                    }
                }


                Response.Redirect("Tables.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ddlEmpresas.Items.Add(new ListItem("Empresa 1", "1"));
                ddlEmpresas.Items.Add(new ListItem("Empresa 2", "2"));
                ddlEmpresas.Items.Add(new ListItem("Empresa 3", "3"));
            }
        }

        protected void ddlEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Validar usuario y contraseña
            string usuario = txtUsuario.Text.Trim().ToUpper();
            string contrasena = txtPassword.Text.Trim();
            string mensaje = string.Empty;

            var resultado = ValidarUsuario(usuario, contrasena);

            if (resultado.EsValido)
            {
                // Cambiar estado a activo aquí
                var readerMozos = new ReadingWaiters();
                readerMozos.CambiarEstadoMozo(resultado.MozoId, "SI");

                string userData = $"{resultado.MozoId}|{resultado.MozoNombre}";

                int tiempoExpiracion = 1;

                // Crea el ticket de autenticación
                var ticket = new System.Web.Security.FormsAuthenticationTicket(
                    1,
                    resultado.MozoNombre,
                    DateTime.Now,
                    // DateTime.Now.AddHours(tiempoExpiracion),
                    DateTime.Now.AddMinutes(tiempoExpiracion),
                    true,
                    userData
                );

                // Encripta el ticket
                string encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(ticket);

                // Crea la cookie
                var cookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, encryptedTicket)
                {
                    Expires = ticket.Expiration,
                    HttpOnly = true
                };
                Response.Cookies.Add(cookie);

                Session["MozoId"] = resultado.MozoId;
                Session["MozoNombre"] = resultado.MozoNombre;

                // Iniciar el "cronómetro" en el servidor en paralelo
                int mozoId = resultado.MozoId;
                DateTime expiration = ticket.Expiration;
                System.Threading.Tasks.Task.Run(() =>
                {
                    TimeSpan waitTime = expiration - DateTime.Now;
                    if (waitTime.TotalMilliseconds > 0)
                        System.Threading.Thread.Sleep(waitTime);

                    // Cambiar el estado del mozo en la base de datos después de que expire la cookie
                    var readerMozosBg = new ReadingWaiters();
                    readerMozosBg.CambiarEstadoMozo(mozoId, "NO");
                });

                Response.Redirect("Tables.aspx");
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
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
                return (false, 0, null,"Por favor, complete todos los campos.");

            var readerMozos = new ReadingWaiters();
            var mozos = readerMozos.LeerMozos();

            foreach (var mozo in mozos)
            {
                string usuarioEsperado = mozo.Mozo_Nombre + mozo.Mozo_Id; // Concatenar nombre y ID del mozo
                if (usuario.Equals(usuarioEsperado, StringComparison.OrdinalIgnoreCase) && contrasena == mozo.Mozo_Contrasena)
                {
                    if (mozo.Mozo_Activo == "SI")
                        return (false, 0, null, "El mozo ya está activo en otra sesión.");

                    return (true, mozo.Mozo_Id, mozo.Mozo_Nombre, null);
                }
            }
            return (false, 0, null, "Usuario o contraseña incorrectos.");
        }
    }
}