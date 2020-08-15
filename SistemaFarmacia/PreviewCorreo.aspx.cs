using SistemaFarmacia.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SistemaFarmacia
{
    public partial class PreviewCorreo : System.Web.UI.Page
    {
        Conexion connMySql = new Conexion();

        protected void Page_Load(object sender, EventArgs e)
        {
            DatosCorreo datosCorreo_ = connMySql.ConsultaDatosCorreo();

            divCorreo.InnerHtml = "<div style='text-align:center; border: solid 1px gray;'><h2>" + datosCorreo_.SMTP_MENSAJE + "</h2>" +
               "<img src='" + datosCorreo_.SMTP_IMAGEN + "' style='text-align:center;'/><br/>" +
               "<h3 style='text-align: center;'>" + datosCorreo_.SMTP_FIRMA.Replace("\n", "<br/>") + "</h3></div>";
        }
    }
}