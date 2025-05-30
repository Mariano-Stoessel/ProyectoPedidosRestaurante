using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoPedidosResto.Views
{
    public partial class Tables : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

      

protected void BtnCargarMesa_Click(object sender, EventArgs e)
        {

            modalScript.Text = "<script>var myModal = new bootstrap.Modal(document.getElementById('Modal')); myModal.show();</script>";
        }
    }
}