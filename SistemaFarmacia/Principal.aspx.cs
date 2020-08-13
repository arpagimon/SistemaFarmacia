using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaFarmacia
{
    public partial class MenuPrincipal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Conexion connMysql = new Conexion();
            if (!IsPostBack)
            {
                MasterFarmacia master = (MasterFarmacia)this.Master;
                master.mostrarbtnRegresar(false);
                master.cambiarLblTitle("<h1>Menú principal</h1>");
                
                master.mostrarLblUser("<p>Usuario: " + this.Session["usuario"].ToString() + " </p>");
                
                iconoClientes.Visible = false;
                iconoClientesMes.Visible = false;
                iconoAdministracion.Visible = false;

                String permisos = "";
                try
                {
                    permisos = Session["Permisos"].ToString();
                }
                catch (Exception ex)
                {
                    Response.Redirect("Login.aspx");

                }

                foreach (String permiso in permisos.Split(','))
                {
                    switch (permiso)
                    {
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                            iconoClientes.Visible = true;
                            break;

                        case "21":
                            iconoClientesMes.Visible = true;
                            break;
                            
                        case "31":
                        case "32":
                        case "33":
                        case "34":
                        case "35":
                        case "36":
                        case "37":
                        case "38":
                        case "39":
                        case "310":
                        case "311":
                            iconoAdministracion.Visible = true;
                            break;
                    }
                }
            }
        }

        protected void Clientes_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConsultarCliente.aspx");
        }

        protected void Administración_Click(object sender, EventArgs e)
        {
            Response.Redirect("Administracion.aspx");
        }

        protected void ClientesMes_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ConsultarClienteMes.aspx");
        }
    }
}