using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaFarmacia
{
    public partial class MasterFarmacia : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void MostrarMensaje(string Titulo, string Mensaje)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "X", "<script language='javascript'>alert('"+Titulo+"','"+Mensaje+"');</script>", false);
        }

        public void mostrarMensaje(Boolean opcion)
        {
            sombra.Visible = opcion;
        }
        
        public void mostrarbtnSalir(Boolean opcion)
        {
            btnSalir.Visible = opcion;
        }

        protected void btnSalir_Click(object sender, ImageClickEventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");

        }

        protected void btnRegresar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Principal.aspx");
        }

        public void mostrarbtnRegresar(Boolean opcion)
        {
            btnRegresar.Visible = opcion;
        }

        

    }
}