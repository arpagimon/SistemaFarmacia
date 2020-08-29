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
            DatosCorreo datosCorreo_ = null;
            if (Session["TipoPreview"].ToString() == "mensual")
            {
                datosCorreo_ = connMySql.ConsultaDatosCorreo();
            }
            else
            {
                datosCorreo_ = connMySql.ConsultaDatosCorreoSelec();
            }

            
            divCorreo.InnerHtml = "<div style='solid 1px #eceff1'>"+
                "<h2 style='font - weight: 400;'>" + datosCorreo_.SMTP_SUJETO + "</h2>" +
                "<h3 style='font-weight: bold;font-size: .875rem;'>" + datosCorreo_.SMTP_CORREO+ "</h3>"+
                "<h4 style='color:#5f6368;letter-spacing: .3px;font-size: .75rem;'>Para  " + datosCorreo_.SMTP_CORREO_PRUEBA+"</div>"+
                "<div style='text-align:center; border-top: solid 1px #eceff1;'><h2>" + datosCorreo_.SMTP_MENSAJE + "</h2>" +
               "<img src='" + datosCorreo_.SMTP_IMAGEN + "' style='text-align:center;'/><br/>" +
               "<h3 style='text-align: center;'>" + datosCorreo_.SMTP_FIRMA.Replace("\n", "<br/>") + "</h3></div>";
        }
    }
}