using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SistemaFarmacia
{
    public partial class Citas : System.Web.UI.Page
    {
        Conexion connMySql = new Conexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCitas("1","08");
            }
        }
        

        public void CargarCitas(String Doctor, String Mes)
        {
            String eventos = "[";

            DataSet dsCitas = connMySql.traerCitasDoctor(Mes, Doctor);

            
            if (dsCitas.Tables.Count > 0) { 
                foreach (DataRow dRow in dsCitas.Tables[0].Rows)
                {
                    if(eventos.Length > 1)
                    {
                        eventos += ",{ id: '" + dRow["ID_Cita"] + "', title: '" + dRow["nombre"].ToString() + " " + dRow["apellido_paterno"].ToString() + " " + dRow["apellido_materno"].ToString() + "', start: '" + dRow["hora_inicio"].ToString() + "', end:'" + dRow["hora_fin"].ToString() + "', Nombre='" + dRow["nombre"].ToString() + "', ApellidoPaterno:'" + dRow["apellido_paterno "].ToString() + "', ApellidoMaterno:'" + dRow["apellido_materno "].ToString() + "'}";
                    }
                    else
                    {
                        eventos += "{ id: '" + dRow["ID_Cita"] + "', title: '" + dRow["nombre"].ToString() +" "+ dRow["apellido_paterno"].ToString() +" "+ dRow["apellido_materno"].ToString() + "', start: '" + dRow["hora_inicio"].ToString() + "', end:'" + dRow["hora_fin"].ToString() + "', Nombre='"+ dRow["nombre"].ToString() +"', ApellidoPaterno:'"+ dRow["apellido_paterno "].ToString()+ "', ApellidoMaterno:'" + dRow["apellido_materno "].ToString() + "'}";
                    }
                }
            }

            eventos += "]";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "X", "<script language='javascript'>cargaCalendario(" + eventos + ");</script>", false);
        }

        protected void ButtonOK_Click(object sender, EventArgs e)
        {
            // do your job here
            //Modal.Close(this);
        }

        
        protected void btnCancelarCita_Click(object sender, EventArgs e)
        {

        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void btnAgendarCita_Click(object sender, EventArgs e)
        {

        }
    }
}