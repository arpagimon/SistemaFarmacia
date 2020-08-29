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
            String eventarget1 = Request["__EVENTTARGET"];
            String datosEnviados = Request["__EVENTARGUMENT"];
            MasterFarmacia master = (MasterFarmacia)this.Master;

            if (SesionViva())
            {
                if (!IsPostBack)
                {
                    llenaDropMedicos();
                    master.cambiarLblTitle("<img src='Imagenes/citas.png' alt='Citas'><h1>Citas</h1>");
                    master.mostrarLblUser("<p>Usuario: " + this.Session["usuario"].ToString() + " </p>");
                }

                CargarCitas("08");

                switch (eventarget1)
                {
                    case "MostartDatosCita":
                        sombraMensaje.Visible = true;
                        llenaDatosCita(datosEnviados);
                        break;

                    case "MostartDatosCitaNueva":
                        sombraMensaje.Visible = true;
                        divFormularioCita.Visible = true;
                        llenaCamposCitaNueva(datosEnviados);

                        break;

                    default:
                        break;
                }


            }
            else
            {
                sombraMensaje.Visible = true;

                mostrarmensaje();
                ocultarBotonesMensaje();
                lblMensaje.Text = "Su sesión ha caducado, vuelva a hacer login.";
                btnOkSalir.Visible = true;
                return;
            }
        }
        

        public void CargarCitas(String Mes)
        {
            String eventos = "[";

            DataSet dsCitas = connMySql.traerCitasDoctor(Mes, ddlDoctor.SelectedValue);

            
            if (dsCitas.Tables.Count > 0) { 
                foreach (DataRow dRow in dsCitas.Tables[0].Rows)
                {
                    if(eventos.Length > 1)
                    {
                        eventos += ",{ id: '" + dRow["ID_Cita"] + "', title: '" + dRow["nombre"].ToString() + " " + dRow["apellido_paterno"].ToString() + " " + dRow["apellido_materno"].ToString() + "', start: '" + dRow["hora_inicio"].ToString() + "', end:'" + dRow["hora_fin"].ToString() + "'}";
                    }
                    else
                    {
                        eventos += "{ id: '" + dRow["ID_Cita"] + "', title: '" + dRow["nombre"].ToString() +" "+ dRow["apellido_paterno"].ToString() +" "+ dRow["apellido_materno"].ToString() + "', start: '" + dRow["hora_inicio"].ToString() + "', end:'" + dRow["hora_fin"].ToString() + "'}";
                    }
                }
            }

            eventos += "]";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "X", "<script language='javascript'>cargaCalendario(" + eventos + ");</script>", false);
        }
        

        
        protected void btnCancelarCita_Click(object sender, EventArgs e)
        {
            mostrarmensaje();
            lblMensaje.Text = "¿Desea continuar con la cancelación de la cita de <b>" + TxtNombre.Text + " " + TxtApellidoP.Text + " " + TxtApellidoM.Text + "</b> para el día <b>" + txtFechaCita.Text.Split('-')[2] + "/" + txtFechaCita.Text.Split('-')[1] + "/" + txtFechaCita.Text.Split('-')[0] + "</b> a las <b> " + txtHoraInicio.Text + "</b>?";
            ocultarBotonesMensaje();
            btnCancCancelarCita.Visible = true;
            btnConfCancelarCita.Visible = true;
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            CargarCitas("08");

            sombraMensaje.Visible = false;
        }

        
        protected void btnModificarCita_Click(object sender, EventArgs e)
        {
            mostrarmensaje();
            ocultarBotonesMensaje();
            btnConfModifCita.Visible = true;
            btnCancModifCita.Visible = true;

            lblMensaje.Text = "¿Desea continuar con la modificación de la cita de <b>" + TxtNombre.Text + " " + TxtApellidoP.Text + " " + TxtApellidoM.Text + "</b> a el día <b>" + txtFechaCita.Text.Split('-')[2] + "/" + txtFechaCita.Text.Split('-')[1] + "/" + txtFechaCita.Text.Split('-')[0] + "</b> con un horario de <b> " + txtHoraInicio.Text + "</b> a las <b> " + txtHoraFin.Text + "</b>?";

        }

        #region MsjCancelacionCita
        protected void btnCancCancelarCita_Click(object sender, EventArgs e)
        {
            divMensaje.Visible = false;
            divFormularioCita.Visible = false;
            sombraMensaje.Visible = false;
        }

        protected void btnConfCancelarCita_Click(object sender, EventArgs e)
        {
            connMySql.cancelarCita(TxtIDCita.Text, connMySql.traerIDEmpleado(Session["usuario"].ToString()));

            lblMensaje.Text = "Cita cancelada";
            ocultarBotonesMensaje();
            btnCerrarMensaje.Visible = true;
        }
        #endregion
        #region MsjModifciaCita
        protected void btnCancModifCita_Click(object sender, EventArgs e)
        {
            divMensaje.Visible = false;
            divFormularioCita.Visible = false;
            sombraMensaje.Visible = false;
        }

        protected void btnConfModifCita_Click(object sender, EventArgs e)
        {
            connMySql.ActualizaCita(TxtIDCita.Text, connMySql.traerIDEmpleado(Session["usuario"].ToString()), txtFechaCita.Text, txtHoraInicio.Text, txtHoraFin.Text);

            lblMensaje.Text = "Cita modificada";
            ocultarBotonesMensaje();
            btnCerrarMensaje.Visible = true;
        }
        #endregion

        protected void btnCerrarMensaje_Click(object sender, EventArgs e)
        {
            divMensaje.Visible = false;
            lblMensaje.Text = "";
            sombraMensaje.Visible = false;
            ocultarBotonesMensaje();
        }
        public void ocultarBotonesMensaje()
        {
            btnCerrarMensaje.Visible = false;
            btnCancCancelarCita.Visible = false;
            btnConfCancelarCita.Visible = false;
            btnCancModifCita.Visible = false;
            btnConfModifCita.Visible = false;
            btnOkSalir.Visible = false;
        }

        public void llenaDatosCita(String Id_Cita)
        {

            divSeleccionCliente.Visible = false;
            divDatosCliente.Visible = true;

            DataSet datosCita = connMySql.traerDatosCitas(Id_Cita);

            foreach(DataRow dRow in datosCita.Tables[0].Rows)
            {
                TxtIDCita.Text = dRow["ID_Cita"].ToString();
                TxtNombre.Text = dRow["Nombre"].ToString();
                TxtApellidoP.Text = dRow["apellido_paterno"].ToString();
                TxtApellidoM.Text = dRow["apellido_materno"].ToString();
                TxtFechaN.Text = dRow["Fecha_Nacimiento"].ToString().Split(' ')[0];
                TxtEmail.Text = dRow["EMAIL"].ToString();

                ddlDoctorCita.SelectedValue = dRow["ID_usuario"].ToString();
                txtFechaCita.Text = dRow["fecha_cita"].ToString().Split(' ')[0];
                txtHoraInicio.Text = dRow["hora_inicio"].ToString().Split(' ')[1];
                txtHoraFin.Text = dRow["hora_fin"].ToString().Split(' ')[1];
                txtNotasCita.Text = dRow["nota"].ToString();
            }

            ocultarBotonesMensaje();
            divFormularioCita.Visible = true;
            btnAgendarCita.Visible = false;
        }

        protected void llenaDropMedicos()
        {
            ddlDoctorCita.DataTextField = "Nombre";
            ddlDoctorCita.DataValueField = "id_usuario";
            ddlDoctorCita.DataSource = connMySql.consultaMedico();
            ddlDoctorCita.DataBind();

            ddlDoctorCita.Items.Insert(0, new ListItem("--Seleccionar--", "0"));


            ddlDoctor.DataTextField = "Nombre";
            ddlDoctor.DataValueField = "id_usuario";
            ddlDoctor.DataSource = connMySql.consultaMedico();
            ddlDoctor.DataBind();

            ddlDoctor.SelectedIndex = 0;
        }

        public void llenaCamposCitaNueva(String datosSeleccionados)
        {
            TxtIDCita.Text = "";

            txtFechaCita.Text = datosSeleccionados.Substring(0, 10);
            txtHoraInicio.Text = datosSeleccionados.Substring(11, 5);
            txtHoraFin.Text = (int.Parse(datosSeleccionados.Substring(11, 2)) +1).ToString().PadLeft(2,'0') + datosSeleccionados.Substring(13, 3);
            ddlDoctorCita.SelectedValue = ddlDoctor.SelectedValue;

            btnCancelarCita.Visible = false;
            btnModificarCita.Visible = false;
            btnAgendarCita.Visible = false;

            divSeleccionCliente.Visible = true;
            divDatosCliente.Visible = false;
        }

        protected void bntNuevoCliente_Click(object sender, EventArgs e)
        {

        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            divFormularioCita.Visible = false;
            divFormBusqCliente.Visible = true;
        }

    #region buscarcliente
        protected void btnFormCliBuscar_Click(object sender, EventArgs e)
        {
            String condicion = "";
            Boolean conCondicion = false;

            if (TxtBusqCliNombre.Text.Trim().Length > 0)
            {
                condicion += " nombre like '%" + TxtBusqCliNombre.Text.Trim() + "%' ";
                conCondicion = true;
            }

            if (TxtBusqCliApellidoP.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " apellido_paterno like '%" + TxtBusqCliApellidoP.Text.Trim() + "%' ";
                conCondicion = true;
            }

            if (TxtBusqCliApellidoM.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " apellido_materno like '%" + TxtBusqCliApellidoM.Text.Trim() + "%' ";
                conCondicion = true;
            }

            if (TxtBusqCliFechaN.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " fecha_nacimiento like '%" + TxtBusqCliFechaN.Text.Trim() + "%' ";
                conCondicion = true;
            }

            if (TxtBusqCliEmail.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " email like '%" + TxtBusqCliEmail.Text.Trim() + "%' ";
                conCondicion = true;
            }

            if (conCondicion)
            {
                lblBusqCliError.Text = "";
                cargaClientes(condicion);
                Session["condicion"] = condicion;

                FormCliContenido.Visible = false;
                divClienteResultados.Visible = true;

            }
            else
            {
                lblBusqCliError.Text = "Favor de llenar al menos un campo";
            }

        }

        protected void btnFormCliLimpiar_Click(object sender, EventArgs e)
        {
            TxtBusqCliNombre.Text = "";
            TxtBusqCliApellidoP.Text = "";
            TxtBusqCliApellidoM.Text = "";
            TxtBusqCliFechaN.Text = "";
            TxtBusqCliEmail.Text = "";
        }

        protected void btnFormCliCerrar_Click(object sender, EventArgs e)
        {
            TxtBusqCliNombre.Text = "";
            TxtBusqCliApellidoP.Text = "";
            TxtBusqCliApellidoM.Text = "";
            TxtBusqCliFechaN.Text = "";
            TxtBusqCliEmail.Text = "";

            divFormularioCita.Visible = true;
            divFormBusqCliente.Visible = false;
        }

        protected void gvClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button chkSeleccionado = (Button)e.Row.FindControl("btnSeleccionar");
                chkSeleccionado.Attributes.Add("ID_Cliente", ((DataRowView)e.Row.DataItem).Row.ItemArray[0].ToString());
                chkSeleccionado.Attributes.Add("Nombre", ((DataRowView)e.Row.DataItem).Row.ItemArray[1].ToString());
                chkSeleccionado.Attributes.Add("ApellidoP", ((DataRowView)e.Row.DataItem).Row.ItemArray[2].ToString());
                chkSeleccionado.Attributes.Add("ApellidoM", ((DataRowView)e.Row.DataItem).Row.ItemArray[3].ToString());
                chkSeleccionado.Attributes.Add("Email", ((DataRowView)e.Row.DataItem).Row.ItemArray[12].ToString());
                chkSeleccionado.Attributes.Add("FechaN", ((DateTime)((DataRowView)e.Row.DataItem).Row.ItemArray[5]).ToString("yyyy-MM-dd"));

                Label etiquetaFechaN = (Label)e.Row.FindControl("lblFechaN");
                try
                {
                    etiquetaFechaN.Text = ((DateTime)((DataRowView)e.Row.DataItem).Row.ItemArray[5]).ToString("dd/MM/yyyy");
                }
                catch { }

            }
        }

        protected void gvClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClientes.PageIndex = e.NewPageIndex;
            cargaClientes(Session["condicion"].ToString());
        }


        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            TxtIDCliente.Text = ((Button)sender).Attributes["ID_Cliente"].ToString();
            TxtNombre.Text = ((Button)sender).Attributes["Nombre"].ToString();
            TxtApellidoP.Text = ((Button)sender).Attributes["ApellidoP"].ToString();
            TxtApellidoM.Text = ((Button)sender).Attributes["ApellidoM"].ToString();
            TxtEmail.Text = ((Button)sender).Attributes["Email"].ToString();
            TxtFechaN.Text = ((Button)sender).Attributes["FechaN"].ToString();

            divFormularioCita.Visible = true;
            divFormBusqCliente.Visible = false;


            TxtBusqCliNombre.Text = "";
            TxtBusqCliApellidoP.Text = "";
            TxtBusqCliApellidoM.Text = "";
            TxtBusqCliFechaN.Text = "";
            TxtBusqCliEmail.Text = "";

            divSeleccionCliente.Visible = false;
            divDatosCliente.Visible = true;

            btnAgendarCita.Visible = true;
        }

        protected void btnAgendarCita_Click(object sender, EventArgs e)
        {
            connMySql.AgregarCita(connMySql.traerIDEmpleado(Session["usuario"].ToString()), txtFechaCita.Text, txtHoraInicio.Text, txtHoraFin.Text, TxtIDCliente.Text, txtNotasCita.Text, ddlDoctorCita.SelectedValue);

            lblMensaje.Text = "Cita agendada";
            ocultarBotonesMensaje();
            divMensaje.Visible = true;
            divFormularioCita.Visible = false;
            btnCerrarMensaje.Visible = true;


            TxtNombre.Text = "";
            TxtApellidoP.Text = "";
            TxtApellidoM.Text = "";
            TxtFechaN.Text = "";
            TxtEmail.Text = "";
            txtFechaCita.Text = "";
            txtHoraInicio.Text = "";
            txtHoraFin.Text = "";
            TxtIDCliente.Text = "";
            txtNotasCita.Text = "";
            
            
        }



        public void cargaClientes(String condicion)
        {
            if (SesionViva())
            {
                DataSet ds = connMySql.TraerClientes(condicion, "1,0");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvClientes.DataSource = ds.Tables[0];
                    gvClientes.DataBind();
                }
                else
                {
                    gvClientes.DataSource = ds.Tables[0];
                    gvClientes.DataBind();

                    int totalColumnas = ds.Tables[0].Columns.Count;

                    if (gvClientes.Rows.Count == 0)
                    {
                        DataTable dtTemporal = new DataTable();
                        dtTemporal.Columns.Add("ID_CLIENTE");
                        dtTemporal.Columns.Add("Nombre");
                        dtTemporal.Columns.Add("Apellido_paterno");
                        dtTemporal.Columns.Add("Apellido_materno");
                        dtTemporal.Columns.Add("EDAD");
                        dtTemporal.Columns.Add("FECHA_NACIMIENTO");
                        dtTemporal.Columns.Add("FECHA_INGRESO");
                        dtTemporal.Columns.Add("ESTADO");
                        dtTemporal.Columns.Add("MUNICIPIO");
                        dtTemporal.Columns.Add("PAIS");
                        dtTemporal.Columns.Add("tel_casa_fijo");
                        dtTemporal.Columns.Add("extension");
                        dtTemporal.Columns.Add("celular");
                        dtTemporal.Columns.Add("EMAIL");
                        dtTemporal.Columns.Add("OBSERVACIONES");
                        dtTemporal.Columns.Add("NOTA");
                        dtTemporal.Columns.Add("MEDIO");
                        dtTemporal.Columns.Add("estatus");
                        dtTemporal.Columns.Add("Enviar_Correo");
                        dtTemporal.Columns.Add("REQ_FACTURA");
                        dtTemporal.Columns.Add("NOMBRAZON_FACTURA");
                        dtTemporal.Columns.Add("RFC");
                        dtTemporal.Columns.Add("ENTIDAD");
                        dtTemporal.Columns.Add("DIR_FACTURA");
                        dtTemporal.NewRow();
                        DataRow drTemporal = dtTemporal.NewRow();
                        dtTemporal.Rows.InsertAt(drTemporal, 0);

                        gvClientes.DataSource = dtTemporal;
                        gvClientes.DataBind();
                    }

                    gvClientes.Rows[0].Cells.Clear();
                    gvClientes.Rows[0].Cells.Add(new TableCell());
                    gvClientes.Rows[0].Cells[0].ColumnSpan = 5;
                    gvClientes.Rows[0].Cells[0].CssClass = "lblSinResultado";
                    gvClientes.Rows[0].Cells[0].Text = "Sin resultados";

                    gvClientes.Visible = true;
                }
            }
        }

        #endregion



        public void mostrarmensaje()
        {
            divMensaje.Visible = true;
            divFormularioCita.Visible = false;
        }


        public Boolean SesionViva()
        {
            Boolean vive = true;
            try
            {
                var temporal = Session["Permisos"].ToString();
            }
            catch (Exception ex)
            {
                vive = false;
            }

            return vive;
        }

        protected void btnOkSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void ddlDoctor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarCitas("08");
        }
    }
}