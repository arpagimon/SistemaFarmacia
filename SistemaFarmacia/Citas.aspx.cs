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
                    if (Session["MesSeleccionado"] == null)
                    {
                        Session.Add("MesSeleccionado", DateTime.Now.Month.ToString().PadLeft(2,'0'));
                    }
                    else
                    {
                        Session["MesSeleccionado"] = DateTime.Now.Month.ToString().PadLeft(2, '0');
                    }
                    
                    llenaDropMedicos();
                    llenaEstadosDFa();
                    llenaEstados();
                    llenaPaises();
                    master.cambiarLblTitle("<img src='Imagenes/citas-morado.png' alt='Citas'><h1>Citas</h1>");
                    master.mostrarLblUser("<p>Usuario: " + this.Session["usuario"].ToString() + " </p>");
                }

                CargarCitas(Session["MesSeleccionado"].ToString());


                switch (eventarget1)
                {
                    case "MostartDatosCita":
                        sombraMensaje.Visible = true;
                        llenaDatosCita(datosEnviados);
                        break;

                    case "MostartDatosCitaNueva":
                        sombraMensaje.Visible = true;

                        llenaCamposCitaNueva(datosEnviados);

                        if (DiaValido(datosEnviados.Split('T')[0], datosEnviados.Split('T')[1]))
                        {
                            divFormularioCita.Visible = true;
                        }
                        else
                        {
                            divFormularioCita.Visible = false;
                            divMensaje.Visible = true;
                            ocultarBotonesMensaje();
                            btnCerrarMensaje.Visible = true;
                            btnContCitas.Visible = true;
                            lblMensaje.Text = "La fecha seleccionada es un dia y/o hora no hábil, ¿Desea continuar con la creación de la cita?";
                        }
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
            String bloqueos = "[";
            String eventosChico = "[";

            DataSet dsCitas = connMySql.traerCitasDoctor(Mes, ddlDoctor.SelectedValue);
            
            if (dsCitas.Tables.Count > 0)
            {
                foreach (DataRow dRow in dsCitas.Tables[0].Rows)
                {
                    if (dRow["tipo"].ToString() == "1")
                    {
                        if (eventos.Length > 1)
                        {
                            eventos += ",{ id: '" + dRow["ID_Cita"] + "', title: '" + dRow["nombre"].ToString() + " " + dRow["apellido_paterno"].ToString() + " " + dRow["apellido_materno"].ToString() + "', start: '" + dRow["hora_inicio"].ToString() + "', end:'" + dRow["hora_fin"].ToString() + "'}";
                            eventosChico += ",{ start: '" + dRow["Fechas"].ToString() + "', end:'" + dRow["Fechas"].ToString() + "', overlap: false, display: 'background', color: '#58D68D'}";
                        }
                        else
                        {
                            eventos += "{ id: '" + dRow["ID_Cita"] + "', title: '" + dRow["nombre"].ToString() + " " + dRow["apellido_paterno"].ToString() + " " + dRow["apellido_materno"].ToString() + "', start: '" + dRow["hora_inicio"].ToString() + "', end:'" + dRow["hora_fin"].ToString() + "'}";
                            eventosChico += "{ start: '" + dRow["Fechas"].ToString() + "', end:'" + dRow["Fechas"].ToString() + "', overlap: false, display: 'background', color: '#58D68D'}";
                        }
                    }
                    else
                    {
                        if (bloqueos.Length > 1)
                        {
                            bloqueos += ",{ id: '" + dRow["ID_Cita"] + "', title: 'Horario bloqueado', start: '" + dRow["hora_inicio"].ToString() + "', end:'" + dRow["hora_fin"].ToString() + "'}";
                        }
                        else
                        {
                            bloqueos += "{ id: '" + dRow["ID_Cita"] + "', title: 'Horario bloqueado', start: '" + dRow["hora_inicio"].ToString() + "', end:'" + dRow["hora_fin"].ToString() + "'}";
                        }
                    }
                }
            }

            eventos += "]";
            bloqueos += "]";
            eventosChico += "]";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "X", "<script language='javascript'>cargaCalendario(" + eventos + "," + bloqueos + "," + (Session["Doctor"].ToString() == "1" ? "'timeGridDay'" : "'timeGridWeek'") + "," + eventosChico + ");</script>", false);
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
            String CitaExistente = connMySql.validaCitaColisionada(ddlDoctor.SelectedValue, txtFechaCita.Text, txtHoraInicio.Text, txtHoraFin.Text, TxtIDCita.Text);

            if (CitaExistente.Length == 0)
            {
                mostrarmensaje();
                ocultarBotonesMensaje();
                btnConfModifCita.Visible = true;
                btnCancModifCita.Visible = true;

                lblMensaje.Text = "¿Desea continuar con la modificación de la cita de <b>" + TxtNombre.Text + " " + TxtApellidoP.Text + " " + TxtApellidoM.Text + "</b> a el día <b>" + txtFechaCita.Text.Split('-')[2] + "/" + txtFechaCita.Text.Split('-')[1] + "/" + txtFechaCita.Text.Split('-')[0] + "</b> con un horario de <b> " + txtHoraInicio.Text + "</b> a las <b> " + txtHoraFin.Text + "</b>?";

                lblErrorFormCita.Text = "";
            }
            else
            {
                lblErrorFormCita.Text = "El horario seleccionado coincide con una cita del " + CitaExistente;
            }
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
            btnOKClienteGuardado.Visible = false;
            btnContCitas.Visible = false;
        }

        public void llenaDatosCita(String Id_Cita)
        {
            divSeleccionCliente.Visible = false;
            divDatosCliente.Visible = true;

            divFContenidoDatoCita.Visible = true;
            divFContenidoResulCita.Visible = false;

            btnPestanaDatosCita.Visible = true;
            btnPestanaResulCita.Visible = true;

            btnPestanaDatosCita.CssClass = "btnPestana btnPestanaSeleccionada";
            btnPestanaResulCita.CssClass = "btnPestana";

            DataSet datosCita = connMySql.traerDatosCitas(Id_Cita);

            String tipo = "";
            foreach(DataRow dRow in datosCita.Tables[0].Rows)
            {
                TxtIDCita.Text = dRow["ID_Cita"].ToString();

                if (dRow["tipo"].ToString() == "1")
                {
                    TxtNombre.Text = dRow["Nombre"].ToString();
                    TxtApellidoP.Text = dRow["apellido_paterno"].ToString();
                    TxtApellidoM.Text = dRow["apellido_materno"].ToString();
                    TxtFechaN.Text = dRow["Fecha_Nacimiento"].ToString().Split(' ')[0];
                    TxtEmail.Text = dRow["EMAIL"].ToString();
                }else
                {
                    divDatosCliente.Visible = false;
                }
                
                ddlDoctorCita.SelectedValue = dRow["ID_usuario"].ToString();
                txtFechaCita.Text = dRow["fecha_cita"].ToString().Split(' ')[0];
                txtHoraInicio.Text = dRow["hora_inicio"].ToString().Split(' ')[1];
                txtHoraFin.Text = dRow["hora_fin"].ToString().Split(' ')[1];
                txtNotasCita.Text = dRow["nota"].ToString();
                tipo = dRow["tipo"].ToString();
            }

            if (tipo == "1") {
                btnModificarCita.Text = "Modificar cita";
                btnCancelarCita.Text = "Cancelar cita";
            } else
            {
                btnModificarCita.Text = "Modificar bloqueo";
                btnCancelarCita.Text = "Cancelar bloqueo";
            }

            ocultarBotonesMensaje();
            divFormularioCita.Visible = true;
            btnAgendarCita.Visible = false;
            btnInhabiliarHorario.Visible = false;
            btnModificarCita.Visible = true;
            btnCancelarCita.Visible = true;
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

            if(Session["Doctor"].ToString() == "1")
            {
                ListItem OpcionDoctor = ddlDoctorCita.Items.FindByValue(connMySql.traerIDEmpleado(Session["usuario"].ToString()));

                ddlDoctorCita.Items.Clear();
                ddlDoctorCita.Items.Add(OpcionDoctor);

                ddlDoctor.Items.Clear();
                ddlDoctor.Items.Add(OpcionDoctor);
            }
        }

        public void llenaCamposCitaNueva(String datosSeleccionados)
        {
            TxtIDCita.Text = "";
            lblErrorFormCita.Text = "";

            btnPestanaDatosCita.Visible = true;
            btnPestanaResulCita.Visible = false;

            btnPestanaDatosCita.CssClass = "btnPestana btnPestanaSeleccionada";
            btnPestanaResulCita.CssClass = "btnPestana";
            
            txtFechaCita.Text = datosSeleccionados.Substring(0, 10);
            txtHoraInicio.Text = datosSeleccionados.Substring(11, 5);
            txtHoraFin.Text = (int.Parse(datosSeleccionados.Substring(11, 2)) +1).ToString().PadLeft(2,'0') + datosSeleccionados.Substring(13, 3);
            ddlDoctorCita.SelectedValue = ddlDoctor.SelectedValue;

            btnCancelarCita.Visible = false;
            btnModificarCita.Visible = false;
            btnAgendarCita.Visible = false;
            btnInhabiliarHorario.Visible = false;

            divSeleccionCliente.Visible = true;
            divDatosCliente.Visible = false;
        }

        protected void bntNuevoCliente_Click(object sender, EventArgs e)
        {
            divFormularioCita.Visible = false;
            llenarMedio();
            llenaEstados();
            divFormularioCliente.Visible = true;
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            divFormularioCita.Visible = false;
            divFormBusqCliente.Visible = true;

            FormCliContenido.Visible = true;
            divClienteResultados.Visible = false;
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
                chkSeleccionado.Attributes.Add("FechaN", (((DataRowView)e.Row.DataItem).Row.ItemArray[5].ToString() != "" ? ((DateTime)((DataRowView)e.Row.DataItem).Row.ItemArray[5]).ToString("yyyy-MM-dd"):""));

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
            String CitaExistente = connMySql.validaCitaColisionada(ddlDoctor.SelectedValue, txtFechaCita.Text, txtHoraInicio.Text, txtHoraFin.Text, "");

            if (CitaExistente.Length == 0) { 
                connMySql.AgregarCita(connMySql.traerIDEmpleado(Session["usuario"].ToString()), txtFechaCita.Text, txtHoraInicio.Text, txtHoraFin.Text, TxtIDCliente.Text, txtNotasCita.Text, ddlDoctorCita.SelectedValue,"1");

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

                lblErrorFormCita.Text = "";
            }
            else
            {
                lblErrorFormCita.Text = "El horario seleccionado coincide con una cita del " + CitaExistente;
            }
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
                        dtTemporal.Columns.Add("tel_casa_fijo");
                        dtTemporal.Columns.Add("extension");
                        dtTemporal.Columns.Add("celular");
                        dtTemporal.Columns.Add("EMAIL");
                        dtTemporal.Columns.Add("OBSERVACIONES");
                        dtTemporal.Columns.Add("NOTA");
                        dtTemporal.Columns.Add("MEDIO");
                        dtTemporal.Columns.Add("estatus");
                        dtTemporal.Columns.Add("PAIS");
                        dtTemporal.Columns.Add("Enviar_Correo");
                        dtTemporal.Columns.Add("req_factura");
                        dtTemporal.Columns.Add("rfc");
                        dtTemporal.Columns.Add("entidad");
                        dtTemporal.Columns.Add("calle_dirf");
                        dtTemporal.Columns.Add("noInterior_dirf");
                        dtTemporal.Columns.Add("noExterior_dirf");
                        dtTemporal.Columns.Add("Colonia_dirf");
                        dtTemporal.Columns.Add("CP_dirf");
                        dtTemporal.Columns.Add("Estado_dirf");
                        dtTemporal.Columns.Add("municipio_dirf");
                        dtTemporal.Columns.Add("pais_dirf");
                        dtTemporal.Columns.Add("NOMBRAZON_FACTURA");
                        dtTemporal.Columns.Add("sexo");
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
        

        protected void llenarMedio()
        {
            DataSet ds = connMySql.consultarmedios();
            ddlFormCliMedio.DataSource = ds;
            ddlFormCliMedio.DataTextField = "MEDIO";
            ddlFormCliMedio.DataValueField = "MEDIO";
            ddlFormCliMedio.DataBind();

            ddlFormCliMedio.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void txtFormCliFecNac_TextChanged(object sender, EventArgs e)
        {
            String f = txtFormCliFecNac.Text;
            DateTime now = DateTime.Today;
            DateTime before = DateTime.Parse(f);
            int age = now.Year - before.Year;
            String edad;
            if (now.Month < before.Month)
            {
                age = age - 1;
                edad = age.ToString();

                TxtFormCliEdad.Text = edad;
            }
            else
            {
                if (now.Month == before.Month)
                {
                    if (now.Day < before.Day)
                    {
                        age = age - 1;
                        edad = age.ToString();

                        TxtFormCliEdad.Text = edad;
                    }
                    else
                    {
                        edad = age.ToString();
                        TxtFormCliEdad.Text = edad;
                    }
                }
                else
                {
                    edad = age.ToString();
                    TxtFormCliEdad.Text = edad;
                }
            }
        }

        protected void btnAddDatosF_Click(object sender, EventArgs e)
        {
            divFormularioCliente.Visible = false;
            divMensajeDF.Visible = true;
        }

        public void llenaEstados()
        {
            ddlFormCliEstado.DataTextField = "estado";
            ddlFormCliEstado.DataValueField = "ID";
            ddlFormCliEstado.DataSource = connMySql.traerEstado();
            ddlFormCliEstado.DataBind();

            ddlFormCliEstado.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void ddlFormCliEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFormCliEstado.SelectedValue == "33")
            {
                divMunicipio.Visible = false;
                divPais.Visible = true;
            }
            else
            {
                divMunicipio.Visible = true;
                divPais.Visible = false;
                llenaMunicipio(ddlFormCliEstado.SelectedValue);
            }
        }

        public void llenaMunicipio(String Estado)
        {
            ddlFormCliMunicipio.Enabled = true;
            ddlFormCliMunicipio.DataTextField = "MUNICIPIO";
            ddlFormCliMunicipio.DataValueField = "MUNICIPIO";
            ddlFormCliMunicipio.DataSource = connMySql.traerMunicipio(Estado);
            ddlFormCliMunicipio.DataBind();
            ddlFormCliMunicipio.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void FormCliCancelar_Click(object sender, EventArgs e)
        {
            divFormularioCliente.Visible = false;

            txtFormCliNombre.Text = "";
            txtFormCliApePat.Text = "";
            txtFormCliApeMat.Text = "";
            ddlFormCliEstado.SelectedIndex = -1;
            ddlFormCliMunicipio.SelectedIndex = -1;

            TxtFormCliEdad.Text = "";
            txtFormCliFecIngreso.Text = "";
            ddlFormCliMedio.SelectedIndex = -1;

            txtFormCliTelFijo.Text = "";
            txtFormCliExtension.Text = "";
            txtFormCliCelular.Text = "";
            txtFormCliFecNac.Text = "";
            txtFormCliEmail.Text = "";
            TxtFormCliObservaciones.Text = "";
            TxtFormCliNota.Text = "";
            ddlFormCliEstatus.SelectedIndex = 0;

            ddlFormCliPais.SelectedIndex = -1;
            divPais.Visible = false;
            divMunicipio.Visible = true;

            lblFormCliError.Text = "";

            try
            {
                txtFormCliNombre.Attributes.Remove("style");
                txtFormCliApePat.Attributes.Remove("style");
                txtFormCliApeMat.Attributes.Remove("style");
                ddlFormCliEstado.Attributes.Remove("style");
                ddlFormCliMunicipio.Attributes.Remove("style");
                TxtFormCliEdad.Attributes.Remove("style");
                txtFormCliFecIngreso.Attributes.Remove("style");
                ddlFormCliMedio.Attributes.Remove("style");
                txtFormCliTelFijo.Attributes.Remove("style");
                txtFormCliCelular.Attributes.Remove("style");
                txtFormCliFecNac.Attributes.Remove("style");
                txtFormCliEmail.Attributes.Remove("style");
                ddlFormCliPais.Attributes.Remove("style");
                btnAddDatosF.Attributes.Remove("style");
                txtNombRDFa.Attributes.Remove("style");
                txtRfcDFa.Attributes.Remove("style");
                txtCalleDFa.Attributes.Remove("style");
                txtNoExtDFa.Attributes.Remove("style");
                txtNoIntDFa.Attributes.Remove("style");
                txtColoniaDFa.Attributes.Remove("style");
                ddlEstadoDFa.Attributes.Remove("style");
                ddlMunicipioDFa.Attributes.Remove("style");
                ddlPaisDFa.Attributes.Remove("style");
                ddlEntidadDFa.Attributes.Remove("style");
                txtCPdfA.Attributes.Remove("style");
            }
            catch { }
            
            ddlFormCliFactura.SelectedIndex = 1;
            
            divBtnDatosF.Visible = false;
            divPaisDFa.Visible = false;
            divMunicipioDFa.Visible = true;
            txtNombRDFa.Text = "";
            txtRfcDFa.Text = "";
            txtCalleDFa.Text = "";
            txtNoExtDFa.Text = "";
            txtNoIntDFa.Text = "";
            txtColoniaDFa.Text = "";
            ddlPaisDFa.SelectedIndex = -1;
            ddlMunicipioDFa.SelectedIndex = -1;
            ddlEstadoDFa.SelectedIndex = -1;
            ddlEntidadDFa.SelectedIndex = 1;
            txtCPdfA.Text = "";
            
            divFormularioCita.Visible = true;
            divFormularioCliente.Visible = false;

        }

        protected void FormCliAgregar_Click(object sender, EventArgs e)
        {
            String resultado = "";
            Boolean pasa = true;

            String Nombre = txtFormCliNombre.Text;
            if (Nombre.Trim().Length == 0)
            {
                txtFormCliNombre.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtFormCliNombre.Attributes.Remove("style");
            }

            String ApellidoP = txtFormCliApePat.Text;
            if (ApellidoP.Trim().Length == 0)
            {
                txtFormCliApePat.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtFormCliApePat.Attributes.Remove("style");
            }

            String ApellidoM = txtFormCliApeMat.Text;
            if (ApellidoM.Trim().Length == 0)
            {
                txtFormCliApeMat.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtFormCliApeMat.Attributes.Remove("style");
            }

            String Estado = (ddlFormCliEstado.SelectedIndex < 1 ? "" : ddlFormCliEstado.Items[ddlFormCliEstado.SelectedIndex].Text);
            if (Estado.Trim().Length == 0)
            {
                ddlFormCliEstado.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlFormCliEstado.Attributes.Remove("style");
            }

            String Municipio = (ddlFormCliMunicipio.SelectedIndex < 1 ? "" : ddlFormCliMunicipio.Items[ddlFormCliMunicipio.SelectedIndex].Text);
            if ((Estado != "--OTRO--" ? (Municipio.Trim().Length == 0 || Municipio == "0") : false))
            {
                ddlFormCliMunicipio.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlFormCliMunicipio.Attributes.Remove("style");
            }

            String Edad = TxtFormCliEdad.Text;
            if (Edad.Trim().Length == 0)
            {
                TxtFormCliEdad.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                TxtFormCliEdad.Attributes.Remove("style");
            }

            String FechaI = txtFormCliFecIngreso.Text;
            if (FechaI.Trim().Length == 0)
            {
                txtFormCliFecIngreso.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtFormCliFecIngreso.Attributes.Remove("style");
            }

            String Medio = ddlFormCliMedio.SelectedValue;
            if (Medio.Trim().Length == 0 || Medio.Equals("0"))
            {
                ddlFormCliMedio.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlFormCliMedio.Attributes.Remove("style");
            }

            String TelFijo = txtFormCliTelFijo.Text;

            String Extension = txtFormCliExtension.Text;
            String Celular = txtFormCliCelular.Text;
            if (TelFijo.Trim().Length > 0 || Celular.Trim().Length > 0)
            {
                txtFormCliTelFijo.Attributes.Remove("style");
                txtFormCliCelular.Attributes.Remove("style");
            }
            else
            {
                txtFormCliTelFijo.Attributes.Add("style", "border: 1px red solid;");
                txtFormCliCelular.Attributes.Add("style", "border: 1px red solid;");
            }

            String FechaN = txtFormCliFecNac.Text;
            if (FechaN.Trim().Length == 0)
            {
                txtFormCliFecNac.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtFormCliFecNac.Attributes.Remove("style");
            }

            String Enviar_Correo = ddlFormCliEnviarCorreo.SelectedValue;
            String Email = txtFormCliEmail.Text;

            if (Enviar_Correo == "1" && Email.Trim().Length == 0)
            {
                txtFormCliEmail.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtFormCliEmail.Attributes.Remove("style");
            }

            String Observaciones = TxtFormCliObservaciones.Text;
            String Nota = TxtFormCliNota.Text;
            String Estatus = ddlFormCliEstatus.SelectedValue;
            String Pais = (ddlFormCliPais.SelectedIndex < 1 ? "" : ddlFormCliPais.Items[ddlFormCliPais.SelectedIndex].Text);
            if ((Estado == "--OTRO--" ? (Pais.Trim().Length == 0 || Pais == "0") : false))
            {
                ddlFormCliPais.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlFormCliPais.Attributes.Remove("style");
            }

            String Sexo = ddlSexo.SelectedValue;

            String Req_Factura = ddlFormCliFactura.SelectedValue;

            String RFC = "";
            String Entidad = "";
            String NomRazon = "";

            String CalleF = "";
            String NoIntF = "";
            String NoExtF = "";
            String ColoniaF = "";
            String CpF = "";
            String EstadoF = "";
            String MunicipioF = "";
            String PaisF = "";

            if (Req_Factura == "1")
            {
                Boolean pasaFactura = true;
                RFC = txtRfcDFa.Text;
                if (RFC.Trim().Length == 0)
                {
                    txtRfcDFa.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    txtRfcDFa.Attributes.Remove("style");
                }
                Entidad = ddlEntidadDFa.SelectedItem.Text;
                NomRazon = txtNombRDFa.Text;
                if (NomRazon.Trim().Length == 0)
                {
                    txtNombRDFa.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    txtNombRDFa.Attributes.Remove("style");
                }

                CalleF = txtCalleDFa.Text;
                if (CalleF.Trim().Length == 0)
                {
                    txtCalleDFa.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    txtCalleDFa.Attributes.Remove("style");
                }

                NoExtF = txtNoExtDFa.Text;
                if (NoExtF.Trim().Length == 0)
                {
                    txtNoExtDFa.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    txtNoExtDFa.Attributes.Remove("style");
                }
                NoIntF = txtNoIntDFa.Text;
                

                ColoniaF = txtColoniaDFa.Text;
                if (ColoniaF.Trim().Length == 0)
                {
                    txtColoniaDFa.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    txtColoniaDFa.Attributes.Remove("style");
                }

                CpF = txtCPdfA.Text;
                if (CpF.Trim().Length == 0)
                {
                    txtCPdfA.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    txtCPdfA.Attributes.Remove("style");
                }

                EstadoF = (ddlEstadoDFa.SelectedIndex < 1 ? "" : ddlEstadoDFa.Items[ddlEstadoDFa.SelectedIndex].Text);
                if (EstadoF.Trim().Length == 0)
                {
                    ddlEstadoDFa.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    ddlEstadoDFa.Attributes.Remove("style");
                }

                MunicipioF = (ddlMunicipioDFa.SelectedIndex < 1 ? "" : ddlMunicipioDFa.Items[ddlMunicipioDFa.SelectedIndex].Text);
                if ((EstadoF != "--OTRO--" ? (MunicipioF.Trim().Length == 0 || MunicipioF == "0") : false))
                {
                    ddlMunicipioDFa.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    ddlMunicipioDFa.Attributes.Remove("style");
                }

                PaisF = (ddlPaisDFa.SelectedIndex < 1 ? "" : ddlPaisDFa.Items[ddlPaisDFa.SelectedIndex].Text);
                if ((EstadoF == "--OTRO--" ? (PaisF.Trim().Length == 0 || PaisF == "0") : false))
                {
                    ddlPaisDFa.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    ddlPaisDFa.Attributes.Remove("style");
                }

                if (pasaFactura)
                {
                    btnAddDatosF.Attributes.Remove("Style");
                }
                else
                {
                    pasa = false;
                    btnAddDatosF.Attributes.Add("style", "border: 2px red solid;");
                }
            }
            
            if (pasa)
            {
                lblFormCliError.Text = "";
                resultado = connMySql.GuardaCliente(Nombre.ToUpper(), ApellidoP.ToUpper(), ApellidoM.ToUpper(), Edad, FechaN, FechaI, Municipio, TelFijo, Extension, Celular, Email, Observaciones, Nota, (Medio == "0" ? "" : Medio), Estatus, Estado, Pais, Enviar_Correo, connMySql.traerIDEmpleado(Session["usuario"].ToString()), Req_Factura, RFC, Entidad, CalleF, NoIntF, NoExtF, ColoniaF, CpF, EstadoF, MunicipioF, PaisF, NomRazon, Sexo);
                
                divFormularioCliente.Visible = false;

                divSeleccionCliente.Visible = false;
                divDatosCliente.Visible = true;

                btnAgendarCita.Visible = true;
                
                TxtIDCliente.Text = connMySql.traeUltimoID();
                TxtNombre.Text = txtFormCliNombre.Text;
                TxtApellidoP.Text = txtFormCliApePat.Text;
                TxtApellidoM.Text = txtFormCliApeMat.Text;
                TxtEmail.Text = txtFormCliEmail.Text;
                TxtFechaN.Text = txtFormCliFecNac.Text;
                
                //Limpia las opciones
                txtFormCliNombre.Text = "";
                txtFormCliApePat.Text = "";
                txtFormCliApeMat.Text = "";
                ddlFormCliEstado.SelectedIndex = -1;
                ddlFormCliMunicipio.SelectedIndex = -1;
                TxtFormCliEdad.Text = "";
                txtFormCliFecIngreso.Text = "";
                ddlFormCliMedio.SelectedIndex = -1;
                txtFormCliTelFijo.Text = "";
                txtFormCliExtension.Text = "";
                txtFormCliCelular.Text = "";
                txtFormCliFecNac.Text = "";
                txtFormCliEmail.Text = "";
                TxtFormCliObservaciones.Text = "";
                TxtFormCliNota.Text = "";
                ddlFormCliEstatus.SelectedIndex = 0;
                ddlFormCliPais.SelectedIndex = -1;
                divPais.Visible = false;
                divMunicipio.Visible = true;
                ddlSexo.SelectedIndex = -1;

                txtNombRDFa.Text = "";
                txtNoExtDFa.Text = "";
                txtNoIntDFa.Text = "";
                txtCalleDFa.Text = "";
                txtRfcDFa.Text = "";
                txtColoniaDFa.Text = "";
                txtCPdfA.Text = "";
                ddlEntidadDFa.SelectedValue = "1";
                ddlEstadoDFa.SelectedIndex = -1;
                ddlMunicipioDFa.SelectedIndex = -1;
                ddlPaisDFa.SelectedIndex = -1;
                divPaisDFa.Visible = false;
                divMunicipioDFa.Visible = true;
                divMensajeDF.Visible = false;
                
                divMensaje.Visible = true;
                ocultarBotonesMensaje();
                
                btnOKClienteGuardado.Visible = true;
                lblMensaje.Text = "El cliente se registró exitosamente.";
                divFormularioCita.Visible = false;
            }
            else
            {
                lblFormCliError.Text = "Favor de llenar los campos faltantes";
            }
        }

        protected void ddlFormCliFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFormCliFactura.SelectedValue == "1")
            {
                divBtnDatosF.Visible = true;
            }
            else
            {
                divBtnDatosF.Visible = false;
            }
        }

        protected void ddlEstadoDFa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEstadoDFa.SelectedValue == "33")
            {
                divMunicipioDFa.Visible = false;
                divPaisDFa.Visible = true;

                llenaPaisesDFa();
            }
            else
            {
                divMunicipioDFa.Visible = true;
                divPaisDFa.Visible = false;
                llenaMunicipioDFa(ddlEstadoDFa.SelectedValue);
            }
        }


        public void llenaMunicipioDFa(String Estado)
        {
            ddlMunicipioDFa.Enabled = true;
            ddlMunicipioDFa.DataTextField = "MUNICIPIO";
            ddlMunicipioDFa.DataValueField = "MUNICIPIO";
            ddlMunicipioDFa.DataSource = connMySql.traerMunicipio(Estado);
            ddlMunicipioDFa.DataBind();
            ddlMunicipioDFa.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        public void llenaEstadosDFa()
        {
            ddlEstadoDFa.DataTextField = "estado";
            ddlEstadoDFa.DataValueField = "ID";
            ddlEstadoDFa.DataSource = connMySql.traerEstado();
            ddlEstadoDFa.DataBind();

            ddlEstadoDFa.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        public void llenaPaisesDFa()
        {
            ddlPaisDFa.DataTextField = "pais";
            ddlPaisDFa.DataValueField = "pais";
            ddlPaisDFa.DataSource = connMySql.traerPaises();
            ddlPaisDFa.DataBind();

            ddlPaisDFa.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void btnCancelaDF_Click(object sender, EventArgs e)
        {
            divFormularioCliente.Visible = true;
            divMensajeDF.Visible = false;

            btnLimpiaDF.Visible = true;
            btnGuardaDF.Visible = true;
        }

        protected void btnLimpiaDF_Click(object sender, EventArgs e)
        {
            txtNombRDFa.Text = "";
            txtNoExtDFa.Text = "";
            txtNoIntDFa.Text = "";
            txtCalleDFa.Text = "";
            txtRfcDFa.Text = "";
            txtColoniaDFa.Text = "";
            txtCPdfA.Text = "";

            ddlEntidadDFa.SelectedValue = "1";
            ddlEstadoDFa.SelectedIndex = -1;
            ddlMunicipioDFa.SelectedIndex = -1;
            ddlPaisDFa.SelectedIndex = -1;
            divPaisDFa.Visible = false;
            divMunicipioDFa.Visible = true;
        }

        protected void btnGuardaDF_Click(object sender, EventArgs e)
        {
            Boolean pasa = true;
            String NombreF = txtNombRDFa.Text;
            String NoExtF = txtNoExtDFa.Text;
            String NoIntF = txtNoIntDFa.Text;
            String CalleF = txtCalleDFa.Text;
            String RFCF = txtRfcDFa.Text;
            String ColoniaF = txtColoniaDFa.Text;
            String CPF = txtCPdfA.Text;
            String EntidadF = ddlEntidadDFa.SelectedItem.Text; ;
            String EstadoF = (ddlEstadoDFa.SelectedIndex < 1 ? "" : ddlEstadoDFa.Items[ddlEstadoDFa.SelectedIndex].Text);
            String MunicipioF = (ddlMunicipioDFa.SelectedIndex < 1 ? "" : ddlMunicipioDFa.Items[ddlMunicipioDFa.SelectedIndex].Text);
            String PaisF = (ddlPaisDFa.SelectedIndex < 1 ? "" : ddlPaisDFa.Items[ddlPaisDFa.SelectedIndex].Text);

            if (NombreF.Trim().Length == 0)
            {
                txtNombRDFa.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtNombRDFa.Attributes.Remove("style");
            }

            if (NoExtF.Trim().Length == 0)
            {
                txtNoExtDFa.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtNoExtDFa.Attributes.Remove("style");
            }

            if (CalleF.Trim().Length == 0)
            {
                txtCalleDFa.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtCalleDFa.Attributes.Remove("style");
            }

            if (RFCF.Trim().Length == 0)
            {
                txtRfcDFa.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtRfcDFa.Attributes.Remove("style");
            }

            if (ColoniaF.Trim().Length == 0)
            {
                txtColoniaDFa.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtColoniaDFa.Attributes.Remove("style");
            }

            if (CPF.Trim().Length == 0)
            {
                txtCPdfA.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                txtCPdfA.Attributes.Remove("style");
            }

            if (EntidadF.Trim().Length == 0)
            {
                ddlEntidadDFa.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlEntidadDFa.Attributes.Remove("style");
            }

            if (EstadoF.Trim().Length == 0)
            {
                ddlEstadoDFa.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlEstadoDFa.Attributes.Remove("style");
            }

            if ((EstadoF != "--OTRO--" ? (MunicipioF.Trim().Length == 0 || MunicipioF == "0") : false))
            {
                ddlMunicipioDFa.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlMunicipioDFa.Attributes.Remove("style");
            }

            if ((EstadoF == "--OTRO--" ? (PaisF.Trim().Length == 0 || PaisF == "0") : false))
            {
                ddlPaisDFa.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlPaisDFa.Attributes.Remove("style");
            }

            if (pasa)
            {
                lblErrorDF.Text = "";
                divFormularioCliente.Visible = true;
                divMensajeDF.Visible = false;

                btnAddDatosF.Attributes.Remove("Style");
            }
            else
            {
                lblErrorDF.Text = "Favor de llenar los campos faltantes.";
            }
        }
        
        public void llenaPaises()
        {
            ddlFormCliPais.DataTextField = "pais";
            ddlFormCliPais.DataValueField = "pais";
            ddlFormCliPais.DataSource = connMySql.traerPaises();
            ddlFormCliPais.DataBind();

            ddlFormCliPais.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void btnOKClienteGuardado_Click(object sender, EventArgs e)
        {
            divMensaje.Visible = false;
            divFormularioCita.Visible = true;
        }

        protected void btnOpcionInhabilitar_Click(object sender, EventArgs e)
        {
            btnInhabiliarHorario.Visible = true;
            divSeleccionCliente.Visible = false;
        }

        protected void btnInhabiliarHorario_Click(object sender, EventArgs e)
        {
            String CitaExistente = connMySql.validaCitaColisionada(ddlDoctor.SelectedValue, txtFechaCita.Text, txtHoraInicio.Text, txtHoraFin.Text,"");

            if (CitaExistente.Length == 0)
            {
                connMySql.AgregarCita(connMySql.traerIDEmpleado(Session["usuario"].ToString()), txtFechaCita.Text, txtHoraInicio.Text, txtHoraFin.Text, "0", txtNotasCita.Text, ddlDoctorCita.SelectedValue,"0");

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
            else
            {
                lblErrorFormCita.Text = "El horario seleccionado coincide con una cita del " + CitaExistente; 
            }
        }

        public Boolean DiaValido(String Dia, String Hora_Inicio)
        {
            Boolean valida = true;
            String[] diasPartes = Dia.Split('-');
            String[] horaPartes = Hora_Inicio.Split(':');

            DateTime diaSeleccionado = new DateTime(int.Parse(Dia.Split('-')[0]), int.Parse(Dia.Split('-')[1]), int.Parse(Dia.Split('-')[2]), int.Parse(Hora_Inicio.Split(':')[0]), int.Parse(Hora_Inicio.Split(':')[1]), 0);
            DayOfWeek NombreDiaSemana = diaSeleccionado.DayOfWeek;
            int diaSemana = 0;

            switch (diaSeleccionado.DayOfWeek)
            {
                case DayOfWeek.Sunday: // Domingo
                    diaSemana = 0;
                    break;

                case DayOfWeek.Monday: //lunes
                    diaSemana = 1;
                    break;

                case DayOfWeek.Tuesday: //Martes
                    diaSemana = 2;
                    break;

                case DayOfWeek.Wednesday: // Miercoles
                    diaSemana = 3;
                    break;

                case DayOfWeek.Thursday: // Jueves
                    diaSemana = 4;
                    break;

                case DayOfWeek.Friday: //Viernes
                    diaSemana = 5;
                    break;

                case DayOfWeek.Saturday: //Sabado
                    diaSemana = 6;
                    break;

                default:
                    break;
            }

            TimeSpan horaDelDia = diaSeleccionado.TimeOfDay;

            DataSet diasInhabiles = connMySql.TraerFechaInhabiles();
            String horaApertura = connMySql.TraerHrApertura();
            TimeSpan horaAperturaTS = new TimeSpan(int.Parse(horaApertura.Split(':')[0]), int.Parse(horaApertura.Split(':')[1]),0);
            String horaCierre = connMySql.TraerHrCierre();
            TimeSpan horaCierreTS = new TimeSpan(int.Parse(horaCierre.Split(':')[0]), int.Parse(horaCierre.Split(':')[1]), 0);
            String diasInhabilesSemana = connMySql.TraerDiasSemana();


            foreach(DataRow dRow in diasInhabiles.Tables[0].Rows)
            {
                DateTime fecha = (DateTime)dRow["fechas_inhabiles"];
                if(fecha == diaSeleccionado.Date)
                { 
                    valida = false;
                }
            }

            if (diasInhabilesSemana.Split(',')[diaSemana] == "True")
            {
                valida = false;
            }

            if (horaDelDia < horaAperturaTS)
            {
                valida = false;
            }

            if (horaDelDia > horaCierreTS)
            {
                valida = false;
            }

            if(diaSeleccionado < DateTime.Now)
            {
                valida = false;
            }

            return valida;
        }

        protected void btnContCitas_Click(object sender, EventArgs e)
        {
            divMensaje.Visible = false;
            ocultarBotonesMensaje();

            divFormularioCita.Visible = true;
        }

        protected void btnPestanaDatosCita_Click(object sender, EventArgs e)
        {
            btnPestanaDatosCita.CssClass = "btnPestana btnPestanaSeleccionada";
            btnPestanaResulCita.CssClass = "btnPestana";

            divFContenidoDatoCita.Visible = true;
            divFContenidoResulCita.Visible = false;
        }

        protected void btnPestanaResulCita_Click(object sender, EventArgs e)
        {
            btnPestanaDatosCita.CssClass = "btnPestana";
            btnPestanaResulCita.CssClass = "btnPestana btnPestanaSeleccionada";

            divFContenidoDatoCita.Visible = false;
            divFContenidoResulCita.Visible = true;

            String Id_Cita = TxtIDCita.Text;
            //if (Id_Cita == null)
            //{
            txtObjetivo.Text = "";
            txtSintomas.Text = "";
            txtIndicaciones.Text = "";
            TextBoxMedicamento.Text = "";
            TextBoxIndicaciones.Text = "";
            TextBoxDosis.Text = "";
            TextFrecuencia.Text = "";
            TextPeriodo.Text = "";

            //}

            DataSet datosCita = connMySql.TraerResultadoCita(Id_Cita);
            if (datosCita != null)
            {

                foreach (DataRow dRow in datosCita.Tables[0].Rows)
                {

                    txtObjetivo.Text = dRow["objetivo"].ToString();
                    txtSintomas.Text = dRow["sintomas"].ToString();
                    txtIndicaciones.Text = dRow["indicaciones_generales"].ToString();
                }
            }
            else
            {
                txtObjetivo.Text = "";
                txtSintomas.Text = "";
                txtIndicaciones.Text = "";
            }
            cargarReceta();
            cargarExpediente();
        }

        protected void btnResulCitaGuardar_Click(object sender, EventArgs e)
        {

            String archivo = System.IO.Path.GetFileName(FUploadResultCita.FileName);
            String idCita = TxtIDCita.Text;
            DataSet resultadocompara = connMySql.TraerResultadoCitaCompara(idCita);
            if (resultadocompara.Tables[0].Rows.Count == 0)
            {
                connMySql.GuardarResultadoCita(idCita, txtObjetivo.Text, txtSintomas.Text, txtIndicaciones.Text);
            }else
            {
                connMySql.ActualizaResultadoCita(idCita, txtObjetivo.Text, txtSintomas.Text, txtIndicaciones.Text);
            }

            String medicina = TextBoxMedicamento.Text;
            String indicacion = TextBoxIndicaciones.Text;
            String dosis = TextBoxDosis.Text;
            if (medicina != "" || indicacion != "" || dosis != "")
            {

                String frecuencia = TextFrecuencia.Text;
                String periodo = TextPeriodo.Text;

                String idCliente = TxtIDCliente.Text;
                String resultado1 = connMySql.GuardarReceta(idCita, medicina, indicacion, dosis, frecuencia, periodo);
            }

            if (FUploadResultCita.FileName.Trim().Length != 0)
            {
                if (FUploadResultCita.HasFile)
                {
                    string nombreArchivo = FUploadResultCita.FileName;
                    string ruta = "Imagenes/Expediente/" + nombreArchivo;
                    FUploadResultCita.SaveAs(Server.MapPath(ruta));

                    //FUploadResultCita.SMTP_IMAGEN = ruta;
                    String nombreArchivoT = nombreArchivo;
                    String rutaT = ruta;
                    String resultado2 = connMySql.GuardarExpediente(idCita, nombreArchivoT, rutaT);

                }

            }
            Div3.Visible = true;
            div5.Visible = true;
            divmensaje2.Visible = false;
        }

        protected void plusClick_Click(object sender, EventArgs e)
        {
            String medicina = TextBoxMedicamento.Text;
            String indicacion = TextBoxIndicaciones.Text;
            String dosis = TextBoxDosis.Text;
            String idCliente = TxtIDCliente.Text;
            String idCita = TxtIDCita.Text;
            String frecuencia = TextFrecuencia.Text;
            String periodo = TextPeriodo.Text;
            if (medicina == "" || dosis == "" || frecuencia == "")
            {
                Div3.Visible = true;
                div6.Visible = true;
            }
            else
            {
                String resultado = connMySql.GuardarReceta(idCita, medicina, indicacion, dosis, frecuencia, periodo);
                cargarReceta();

                TextBoxMedicamento.Text = "";
                TextBoxIndicaciones.Text = "";
                TextBoxDosis.Text = "";
                TextFrecuencia.Text = "";
                TextPeriodo.Text = "";
            }
        }

        public void cargarReceta()
        {
            String idCita = TxtIDCita.Text;
            DataSet ds = connMySql.TraerReceta(idCita);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridViewReceta.DataSource = ds.Tables[0];
                GridViewReceta.DataBind();
            }
            else
            {
                GridViewReceta.DataSource = ds.Tables[0];
                GridViewReceta.DataBind();
                int totalColumnas = ds.Tables[0].Columns.Count;

                if (GridViewReceta.Rows.Count == 0)
                {
                    DataTable dtTemporal = new DataTable();
                    dtTemporal.Columns.Add("Medicamento");
                    dtTemporal.Columns.Add("indicaciones");
                    dtTemporal.Columns.Add("dosis");
                    dtTemporal.Columns.Add("frecuencia");
                    dtTemporal.Columns.Add("periodo");
                    dtTemporal.NewRow();
                    DataRow drTemporal = dtTemporal.NewRow();
                    dtTemporal.Rows.InsertAt(drTemporal, 0);

                    GridViewReceta.DataSource = dtTemporal;
                    GridViewReceta.DataBind();
                    GridViewReceta.Visible = true;
                }
                GridViewReceta.Rows[0].Cells.Clear();
                GridViewReceta.Rows[0].Cells.Add(new TableCell());
                GridViewReceta.Rows[0].Cells[0].ColumnSpan = 6;
            }
        }

        protected void linkeditar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            Label medicamento = (Label)row.FindControl("medicamentoId");
            Label indicaciones = (Label)row.FindControl("indicacionesId");
            Label dosis = (Label)row.FindControl("dosisId");
            Label frecuencia = (Label)row.FindControl("frecuenciaId");
            Label periodo = (Label)row.FindControl("periodoId");
            String medicamentoEdit = medicamento.Text;
            String indicacionesEdit = indicaciones.Text;
            String dosisEdit = dosis.Text;
            String frecuenciaEdit = frecuencia.Text;
            String periodoEdit = periodo.Text;
            medicamentoref.Text = medicamentoEdit;
            indicacionRef.Text = indicacionesEdit;
            dosisref.Text = dosisEdit;
            frecuenciaref.Text = frecuenciaEdit;
            periodoref.Text = periodoEdit;

            MedicamentoACt.Text = medicamentoEdit;
            IndicacionesAct.Text = indicacionesEdit;
            DosisAct.Text = dosisEdit;
            FrecuenciaAct.Text = frecuenciaEdit;
            PeriodoACt.Text = periodoEdit;
            Div3.Visible = true;
            div4.Visible = true;
            divmensaje2.Visible = false;
        }


        protected void GridViewReceta_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void eliminarCancelar_Click(object sender, EventArgs e)
        {
            Div3.Visible = false;
        }

        protected void eliminarAceptar_Click(object sender, EventArgs e)
        {
            String medicamentoFinal = recupera.Text;
            String idCitaFinal = recuperaId.Text;

            String resultado = connMySql.BorrarMedicamento(medicamentoFinal, idCitaFinal);
            Div3.Visible = false;
            cargarReceta();


        }

        protected void btnCancelaraAct_Click(object sender, EventArgs e)
        {
            Div3.Visible = false;
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            String idcita = TxtIDCita.Text;
            String medicamentoNuevo = MedicamentoACt.Text;
            String indicacionNuevo = IndicacionesAct.Text;
            String dosisNuevo = DosisAct.Text;
            String frecuenciaNuevo = FrecuenciaAct.Text;
            String periodoNuevo = PeriodoACt.Text;
            String medicamentoViejo = medicamentoref.Text;
            String indicacionViejo = indicacionRef.Text;
            String dosisViejo = dosisref.Text;
            String frecuenciaViejo = frecuenciaref.Text;
            String periodoViejo = periodoref.Text;
            String resultado = connMySql.ActualizaReceta(idcita, medicamentoNuevo, indicacionNuevo, dosisNuevo, frecuenciaNuevo, periodoNuevo, medicamentoViejo, indicacionViejo, dosisViejo, frecuenciaViejo, periodoViejo);
            cargarReceta();
            Div3.Visible = false;
            div4.Visible = false;


        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Div3.Visible = false;
            div5.Visible = false;
            div6.Visible = false;
            //sombraMensaje.Visible = false;
        }


        protected void linkeliminar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            Label medicamento = (Label)row.FindControl("medicamentoId");
            String medicamentoAct = medicamento.Text;
            recupera.Text = medicamento.Text;
            recuperaId.Text = TxtIDCita.Text;
            Div3.Visible = true;
            divmensaje2.Visible = true;
            div4.Visible = false;
        }


        public void cargarExpediente()
        {

            String idCita = TxtIDCita.Text;
            DataSet ds = connMySql.traerExpediente(idCita);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridVieArchivo.DataSource = ds.Tables[0];
                GridVieArchivo.DataBind();
            }
            else
            {
                GridVieArchivo.DataSource = ds.Tables[0];
                GridVieArchivo.DataBind();
                int totalColumnas = ds.Tables[0].Columns.Count;

                if (GridVieArchivo.Rows.Count == 0)
                {


                    DataTable dtTemporal = new DataTable();
                    dtTemporal.Columns.Add("Id_Cita");
                    dtTemporal.Columns.Add("archivo");
                    dtTemporal.Columns.Add("ruta_archivo");
                    dtTemporal.NewRow();
                    DataRow drTemporal = dtTemporal.NewRow();
                    dtTemporal.Rows.InsertAt(drTemporal, 0);

                    GridVieArchivo.DataSource = dtTemporal;
                    GridVieArchivo.DataBind();
                    GridVieArchivo.Visible = true;
                }
                GridVieArchivo.Rows[0].Cells.Clear();
                GridVieArchivo.Rows[0].Cells.Add(new TableCell());
                GridVieArchivo.Rows[0].Cells[0].ColumnSpan = 6;
            }
        }

        protected void linkdescargar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            Label ruta = (Label)row.FindControl("rutaId");
            Label nombre = (Label)row.FindControl("archivoId");
            String nombrearch = nombre.Text;
            String rutaD = ruta.Text;
            string path = rutaD;
            try
            {
                string strURL = path;
                System.Net.WebClient req = new System.Net.WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + nombrearch + "\"");
                //response.AddHeader("Content-Disposition", "attachment;filename=\"" + Server.MapPath(strURL) + "\"");
                byte[] data = req.DownloadData(Server.MapPath(strURL));
                response.BinaryWrite(data);
                response.End();
            }
            catch (Exception ex)
            {
            }
        }

        protected void plusClickArcivo_Click(object sender, EventArgs e)
        {
            String idCita = TxtIDCita.Text;
            if (FUploadResultCita.FileName.Trim().Length != 0)
            {
                if (FUploadResultCita.HasFile)
                {
                    string nombreArchivo = FUploadResultCita.FileName;
                    string ruta = "Imagenes/Expediente/" + nombreArchivo;
                    FUploadResultCita.SaveAs(Server.MapPath(ruta));

                    //FUploadResultCita.SMTP_IMAGEN = ruta;
                    String nombreArchivoT = nombreArchivo;
                    String rutaT = ruta;
                    String resultado2 = connMySql.GuardarExpediente(idCita, nombreArchivoT, rutaT);
                    cargarExpediente();
                }
            }
        }

    }
}