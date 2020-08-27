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

                //calendario.HeightSpec = DayPilot.Web.Ui.Enums.HeightSpecEnum.Full;
                //calendario.Days = 7;
                CargarCitas("1","08");
            }
        }

        protected void calendario_EventClick(object sender, DayPilot.Web.Ui.Events.EventClickEventArgs e)
        {

        }

        protected void calendario_BeforeEventRender(object sender, DayPilot.Web.Ui.Events.Calendar.BeforeEventRenderEventArgs e)
        {

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
                        eventos += ",{id: '" + dRow["ID_Cita"] + "', title: '" + dRow["nombre"].ToString() + "', start: '" + dRow["hora_inicio"].ToString() + "', end:'" + dRow["hora_fin"].ToString() + "'}";
                    }
                    else
                    {
                        eventos += "{ id: '" + dRow["ID_Cita"] + "', title: '" + dRow["nombre"].ToString() + "', start: '" + dRow["hora_inicio"].ToString() + "', end:'" + dRow["hora_fin"].ToString() + "'}";
                    }
                }
            }

            eventos += "]";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "X", "<script language='javascript'>cargaCalendario(" + eventos + ");</script>", false);
        }

        protected void calendario_EventAdd(object sender, DayPilot.Web.Ui.Events.Calendar.EventAddEventArgs e)
        {

        }
        protected void ButtonOK_Click(object sender, EventArgs e)
        {
            // do your job here
            //Modal.Close(this);
        }

        protected void calendario_EventDoubleClick(object sender, DayPilot.Web.Ui.Events.EventClickEventArgs e)
        {

        }

        protected void calendario_Command(object sender, DayPilot.Web.Ui.Events.CommandEventArgs e)
        {

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