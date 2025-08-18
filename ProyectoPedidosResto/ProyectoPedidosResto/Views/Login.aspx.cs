using ProyectoPedidosResto.Domain;
using ProyectoPedidosResto.Models;
using ProyectoPedidosResto.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;


namespace ProyectoPedidosResto.Views
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AuthHelper.UsuarioAutenticado())
            {
                var (mozoId, mozoNombre) = AuthHelper.ObtenerMozoDesdeTicket();
                if (mozoId.HasValue && !string.IsNullOrEmpty(mozoNombre))
                {
                    AuthHelper.SetearMozoSession(mozoId.Value, mozoNombre);
                }
                Response.Redirect("Tables.aspx");
                return;
            }

            if (!IsPostBack)
            {
                cargarusuarios();
            }
        }

        protected void ddlEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtener el nombre seleccionado
            string nombreSeleccionado = ddlEmpresas.SelectedValue;

            // Buscar el usuario correspondiente
            var reader = new ReadingUser();
            List<User> usuarios = reader.LeerUsuarios();
            User usuarioSeleccionado = usuarios.Find(u => u.Nombre.Trim().Equals(nombreSeleccionado, StringComparison.OrdinalIgnoreCase));

            if (usuarioSeleccionado != null)
            {
                // Guardar el usuario en la sesión
                Session["UsuarioSeleccionado"] = usuarioSeleccionado;
                Response.Redirect(Request.RawUrl);
            }
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

                int tiempoExpiracion = 10;

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

                AuthHelper.SetearMozoSession(resultado.MozoId, resultado.MozoNombre);

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
        protected void cargarusuarios()
        {
            var reader = new ReadingUser();
            List<User> usuarios = reader.LeerUsuarios();
            ddlEmpresas.Items.Clear();
            ddlEmpresas.Items.Add(new ListItem("Seleccione su empresa", ""));

            foreach (var user in usuarios)
            {
                // Usamos Cat_nombre como Text Y como Value
                var nombre = user.Nombre.Trim();
                ddlEmpresas.Items.Add(new ListItem(nombre, nombre));
            }
        }
        }
    }