using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using Elements = iText.Layout.Element;
using iText.Layout.Properties;
using iText.Pdfa;
using iText.StyledXmlParser.Jsoup.Nodes;

using Document = iText.Layout.Document;
using Image = iText.Layout.Element.Image;
using Style = iText.Layout.Style;
using Table = iText.Layout.Element.Table;


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
                        Session.Add("MesSeleccionado", DateTime.Now.Month.ToString().PadLeft(2, '0'));
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

                    if (Session["fechaSeleccionada"] == null)
                    {
                        Session.Add("fechaSeleccionada", DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        Session["fechaSeleccionada"] = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    
                }

                

                switch (eventarget1)
                {
                    case "MostartDatosCita":
                        sombraMensaje.Visible = true;
                        llenaDatosCita(datosEnviados);
                        break;

                    case "MostartDatosCitaNueva":
                        sombraMensaje.Visible = true;

                        llenaCamposCitaNueva(datosEnviados);

                        Session["fechaSeleccionada"] = datosEnviados.Split('T')[0];

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

                CargarCitas(Session["MesSeleccionado"].ToString());

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

            ScriptManager.RegisterStartupScript(this, this.GetType(), "X", "<script language='javascript'>cargaCalendario(" + eventos + "," + bloqueos + "," + (Session["Doctor"].ToString() == "1" ? "'timeGridDay'" : "'timeGridWeek'") + "," + eventosChico + ",'" + (Session["fechaSeleccionada"] == null ? DateTime.Now.ToString("yyyy-MM-dd") : Session["fechaSeleccionada"])  + "');</script>", false);
        }

        protected void btnCancelarCita_Click(object sender, EventArgs e)
        {
            mostrarmensaje();
            if(btnCancelarCita.Text == "Cancelar bloqueo")
            {
                lblMensaje.Text = "¿Desea continuar con la cancelación del bloqueo de citas para el día <b>" + txtFechaCita.Text.Split('-')[2] + "/" + txtFechaCita.Text.Split('-')[1] + "/" + txtFechaCita.Text.Split('-')[0] + "</b> de las <b> " + txtHoraInicio.Text + " a las " + txtHoraFin.Text + "</b>?";
            }
            else
            {
                lblMensaje.Text = "¿Desea continuar con la cancelación de la cita de <b>" + TxtNombre.Text + " " + TxtApellidoP.Text + " " + TxtApellidoM.Text + "</b> para el día <b>" + txtFechaCita.Text.Split('-')[2] + "/" + txtFechaCita.Text.Split('-')[1] + "/" + txtFechaCita.Text.Split('-')[0] + "</b> a las <b> " + txtHoraInicio.Text + "</b>?";
            }
            ocultarBotonesMensaje();
            btnCancCancelarCita.Visible = true;
            btnConfCancelarCita.Visible = true;
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            CargarCitas("08");

            sombraMensaje.Visible = false;
            divGridArchivos.Visible = false;
            btnVerArchivos.Text = "Mostrar Archivos <i class='fa fa-arrow-circle-right btnVeArchivos' aria-hidden='true'></i>";
            btnVerArchivos.ToolTip = "Mostrar Archivos";

            OcultarCamposHistorial();
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
            if (btnCancelarCita.Text == "Cancelar bloqueo")
            {
                lblMensaje.Text = "Bloqueo cancelado";
            }
            else
            {
                lblMensaje.Text = "Cita cancelada";
            }
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
            divFContenidoHistoriaC.Visible = false;
            divFContenidoNotaEvol.Visible = false;

            btnPestanaDatosCita.Visible = true;
            btnPestanaResulCita.Visible = true;
            btnPestanaHistorial.Visible = true;
            btnPestanaNotaEvol.Visible = true;

            btnPestanaDatosCita.CssClass = "btnPestana btnPestanaSeleccionada";
            btnPestanaResulCita.CssClass = "btnPestana";
            btnPestanaHistorial.CssClass = "btnPestana";
            btnPestanaNotaEvol.CssClass = "btnPestana";

            DataSet datosCita = connMySql.traerDatosCitas(Id_Cita);

            String tipo = "";
            foreach (DataRow dRow in datosCita.Tables[0].Rows)
            {
                TxtIDCita.Text = dRow["ID_Cita"].ToString();

                if (dRow["tipo"].ToString() == "1")
                {
                    TxtIDCliente.Text = dRow["ID_Cliente"].ToString();
                    TxtNombre.Text = dRow["Nombre"].ToString();
                    TxtApellidoP.Text = dRow["apellido_paterno"].ToString();
                    TxtApellidoM.Text = dRow["apellido_materno"].ToString();
                    TxtFechaN.Text = dRow["Fecha_Nacimiento"].ToString().Split(' ')[0];
                    TxtEmail.Text = dRow["EMAIL"].ToString();
                }
                else
                {
                    divDatosCliente.Visible = false;
                    btnPestanaDatosCita.Visible = false;
                    btnPestanaResulCita.Visible = false;
                    btnPestanaHistorial.Visible = false;
                    btnPestanaNotaEvol.Visible = false;

                }

                ddlDoctorCita.SelectedValue = dRow["ID_usuario"].ToString();
                txtFechaCita.Text = dRow["fecha_cita"].ToString().Split(' ')[0];
                txtHoraInicio.Text = dRow["hora_inicio"].ToString().Split(' ')[1];
                txtHoraFin.Text = dRow["hora_fin"].ToString().Split(' ')[1];
                txtNotasCita.Text = dRow["nota"].ToString();
                tipo = dRow["tipo"].ToString();

                Session["fechaSeleccionada"] = dRow["fecha_cita"].ToString().Split(' ')[0];
            }

            if (tipo == "1")
            {
                btnModificarCita.Text = "Modificar cita";
                btnCancelarCita.Text = "Cancelar cita";
            }
            else
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

            if (Session["Doctor"].ToString() == "1")
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
            btnPestanaHistorial.Visible = false;
            btnPestanaNotaEvol.Visible = false;

            btnPestanaDatosCita.CssClass = "btnPestana btnPestanaSeleccionada";
            btnPestanaResulCita.CssClass = "btnPestana";
            btnPestanaHistorial.CssClass = "btnPestana";
            btnPestanaNotaEvol.CssClass = "btnPestana";

            txtFechaCita.Text = datosSeleccionados.Substring(0, 10);
            txtHoraInicio.Text = datosSeleccionados.Substring(11, 5);
            txtHoraFin.Text = (int.Parse(datosSeleccionados.Substring(11, 2)) + 1).ToString().PadLeft(2, '0') + datosSeleccionados.Substring(13, 3);
            ddlDoctorCita.SelectedValue = ddlDoctor.SelectedValue;

            btnCancelarCita.Visible = false;
            btnModificarCita.Visible = false;
            btnAgendarCita.Visible = false;
            btnInhabiliarHorario.Visible = false;

            divSeleccionCliente.Visible = true;
            divDatosCliente.Visible = false;

            divFContenidoDatoCita.Visible = true;
            divFContenidoResulCita.Visible = false;
            divFContenidoHistoriaC.Visible = false;
            divFContenidoNotaEvol.Visible = false;
        }

        protected void bntNuevoCliente_Click(object sender, EventArgs e)
        {
            divFormularioCita.Visible = false;
            llenarMedio();
            llenaEstados();
            divFormularioCliente.Visible = true;

            ddlFormCliFactura.SelectedValue = "0";
            ddlSexo.Items.FindByValue("-1").Text = "--Seleccionar--";
        }

        protected void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            divFormularioCita.Visible = false;
            divFormBusqCliente.Visible = true;

            FormCliContenido.Visible = true;
            divClienteResultados.Visible = false;
            ddlSexo.Items.FindByValue("-1").Text = "TODOS";
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
                chkSeleccionado.Attributes.Add("FechaN", (((DataRowView)e.Row.DataItem).Row.ItemArray[5].ToString() != "" ? ((DateTime)((DataRowView)e.Row.DataItem).Row.ItemArray[5]).ToString("yyyy-MM-dd") : ""));

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

            if (CitaExistente.Length == 0)
            {
                connMySql.AgregarCita(connMySql.traerIDEmpleado(Session["usuario"].ToString()), txtFechaCita.Text, txtHoraInicio.Text, txtHoraFin.Text, TxtIDCliente.Text, txtNotasCita.Text, ddlDoctorCita.SelectedValue, "1");

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
                ddlSexo.Attributes.Remove("style");
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
            if (Sexo == "-1")
            {
                ddlSexo.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlSexo.Attributes.Remove("style");
            }

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
            String CitaExistente = connMySql.validaCitaColisionada(ddlDoctor.SelectedValue, txtFechaCita.Text, txtHoraInicio.Text, txtHoraFin.Text, "");

            if (CitaExistente.Length == 0)
            {
                connMySql.AgregarCita(connMySql.traerIDEmpleado(Session["usuario"].ToString()), txtFechaCita.Text, txtHoraInicio.Text, txtHoraFin.Text, "0", txtNotasCita.Text, ddlDoctorCita.SelectedValue, "0");

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
            TimeSpan horaAperturaTS = new TimeSpan(int.Parse(horaApertura.Split(':')[0]), int.Parse(horaApertura.Split(':')[1]), 0);
            String horaCierre = connMySql.TraerHrCierre();
            TimeSpan horaCierreTS = new TimeSpan(int.Parse(horaCierre.Split(':')[0]), int.Parse(horaCierre.Split(':')[1]), 0);
            String diasInhabilesSemana = connMySql.TraerDiasSemana();


            foreach (DataRow dRow in diasInhabiles.Tables[0].Rows)
            {
                DateTime fecha = (DateTime)dRow["fechas_inhabiles"];
                if (fecha == diaSeleccionado.Date)
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

            if (diaSeleccionado < DateTime.Now)
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
            btnPestanaHistorial.CssClass = "btnPestana";
            btnPestanaNotaEvol.CssClass = "btnPestana";

            divFContenidoDatoCita.Visible = true;
            divFContenidoResulCita.Visible = false;
            divFContenidoHistoriaC.Visible = false;
            divFContenidoNotaEvol.Visible = false;
        }

        protected void btnPestanaResulCita_Click(object sender, EventArgs e)
        {
            btnPestanaDatosCita.CssClass = "btnPestana";
            btnPestanaResulCita.CssClass = "btnPestana btnPestanaSeleccionada";
            btnPestanaHistorial.CssClass = "btnPestana";
            btnPestanaNotaEvol.CssClass = "btnPestana";

            divFContenidoDatoCita.Visible = false;
            divFContenidoResulCita.Visible = true;
            divFContenidoHistoriaC.Visible = false;
            divFContenidoNotaEvol.Visible = false;

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
            }
            else
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
            divGridArchivos.Visible = false;
            divProxCita.Visible = false;
            btnVerArchivos.Text = "Mostrar Archivos <i class='fa fa-arrow-circle-right btnVeArchivos' aria-hidden='true'></i>";
            btnVerArchivos.ToolTip = "Mostrar Archivos";
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
                descargarPDF.Visible = true;
            }
            else
            {
                descargarPDF.Visible = false;
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
            descargarPDF.Visible = true;
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
        
        
        public void crearPdf()
        {
            String doctor = ddlDoctorCita.SelectedItem.Text;
            String doctorid = ddlDoctorCita.SelectedValue;

            String fecha = txtFechaCita.Text;
            String hora = txtHoraInicio.Text;
            String idcita = TxtIDCita.Text;
            DateTime fechapartida = DateTime.Parse(fecha);
            DataSet datosReceta = connMySql.TraerReceta(idcita);
            //String path = "C:/Users/UNYII/source/repos/Sistema_Farmacia/SistemaFarmacia/SistemaFarmacia/";
            String path = Server.MapPath("Citas.aspx.cs").Remove(Server.MapPath("Citas.aspx.cs").IndexOf("Citas.aspx.cs"));
            String imgF = "Imagenes/recetaFondo.jpg";
            String RUTA = "Pdf/Receta_" + TxtNombre.Text + "" + TxtApellidoP.Text + "" + txtFechaCita.Text + ".pdf";
            PdfWriter pdfEscrito = new PdfWriter(path + RUTA);

            PdfDocument pdf = new PdfDocument(pdfEscrito);
            PageSize pageSize = PageSize.LETTER;
            Document documento = new Document(pdf, PageSize.LETTER);

            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD); 
            PdfCanvas canvas = new PdfCanvas(pdf.AddNewPage());
            canvas.AddImage(ImageDataFactory.Create(path + imgF), pageSize, false);
            
            documento.SetMargins(150, 30, 90, 30);

            DataSet datosDoctor = connMySql.TraerInfoDoctor(doctorid);
            if (datosDoctor.Tables.Count > 0)
            {
                foreach (DataRow dRow in datosDoctor.Tables[0].Rows)
                {
                    String prefijo = dRow["prefijo"].ToString();
                    String titulo = dRow["titulo"].ToString();
                    String especialidad = dRow["especialidades"].ToString();
                    String instagram = dRow["instagram"].ToString();
                    String facebook = dRow["facebook"].ToString();
                    String sitioWeb = dRow["Sitio_web"].ToString();
                    String correo = dRow["correo"].ToString();
                    String numTel = dRow["numero_fijo"].ToString();
                    String numCel = dRow["Numero_celular"].ToString();
                    String Cprof = dRow["Cedula_profesional"].ToString();
                    String Cesp = dRow["Cedula_especialidad"].ToString();

                    pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new HeaderEventHandler1(doctor, fecha, hora, prefijo, titulo, especialidad, Cesp, Cprof));
                    pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler1(numCel, numTel));
                    pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandler2(instagram, facebook, sitioWeb, correo));
                }
            }
            
            documento.Add(new Elements.Paragraph(" "));
            documento.Add(new Elements.Paragraph(" "));
            documento.Add(new Elements.Paragraph(" "));
            documento.Add(new Elements.Paragraph(" "));
            documento.Add(new Elements.Paragraph("NOMBRE DE CLIENTE:").SetFontSize(10f).SetFont(bold));
            documento.Add(new Elements.Paragraph(TxtNombre.Text + " " + TxtApellidoP.Text + " " + TxtApellidoM.Text).SetFontSize(10f));
            documento.Add(new Elements.Paragraph(" "));
            documento.Add(new Elements.Paragraph("MEDICAMENTO:").SetFontSize(10f).SetFont(bold));

            if (datosReceta != null)
            {

                foreach (DataRow dRow in datosReceta.Tables[0].Rows)
                {

                    String medicamentoS = dRow["Medicamento"].ToString();
                    String indicacionesS = dRow["indicaciones"].ToString();
                    String dosisiS = dRow["dosis"].ToString();
                    String frecuenciaS = dRow["frecuencia"].ToString();
                    String periodo = dRow["periodo"].ToString();

                    documento.Add(new Elements.Paragraph(medicamentoS + ", " + indicacionesS).SetFontSize(10f));
                    documento.Add(new Elements.Paragraph(dosisiS + " " + frecuenciaS + " " + periodo).SetFontSize(10f));
                    documento.Add(new Elements.Paragraph(""));



                }
            }
            documento.Add(new Elements.Paragraph("INDICACIONES GENERALES:\n").SetFont(bold).SetFontSize(10f));
            documento.Add(new Elements.Paragraph(txtIndicaciones.Text).SetFontSize(10f));
            
            
            documento.Close();


            try
            {
                //string strURL = "Imagenes/Expediente/Receta_" + TxtNombre.Text + "" + TxtApellidoP.Text + "" + txtFechaCita.Text + ".pdf";
                System.Net.WebClient req = new System.Net.WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=\" Receta_" + TxtNombre.Text + "" + TxtApellidoP.Text + "" + txtFechaCita.Text + ".pdf");
                byte[] data = req.DownloadData(Server.MapPath(RUTA));
                response.BinaryWrite(data);
                response.End();
            }
            catch (Exception ex)
            {
            }
        }

        public class HeaderEventHandler1 : IEventHandler
        {
            Image Img;
            String Doctor;
            String Fecha;
            String Hora;
            String Prefijo;
            String Titulo;
            String Esp;
            String DoctorId;
            String cedulaP;
            String cedulaE;

            public HeaderEventHandler1(String doctor, String fecha, String hora, String prefijo, String titulo, String especialidad, String Cprof, String Cesp)
            {
                Doctor = doctor;
                Fecha = fecha;
                Hora = hora;
                Prefijo = prefijo;
                Titulo = titulo;
                Esp = especialidad;
                cedulaP = Cprof;
                cedulaE = Cesp;
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas1 = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);
                Rectangle rootArea = new Rectangle(255, 625, 300, 150);
                new Canvas(canvas1, pdfDoc, rootArea).Add(Traertabla(docEvent));
            }

            public Table Traertabla(PdfDocumentEvent docEvent)
            {
                DateTime fechapartida = DateTime.Parse(Fecha);

                float[] cellWidth = { 100f, 0f };
                Table tableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

                Style styleCell = new Style().SetBorder(Border.NO_BORDER);
                Style styleText = new Style().SetTextAlignment(TextAlignment.RIGHT).SetFontSize(10f);

                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                PdfFont arial = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                
                Elements.Cell cell = new Elements.Cell().AddStyle(styleText).AddStyle(styleCell)
                 .Add(new Elements.Paragraph("Dermatologia Clinica, Quirurgica y Estetica\n").SetFont(bold).SetFontColor(new DeviceRgb(104, 39, 148)))
                 .Add(new Elements.Paragraph(Prefijo + " " + Doctor + "\n ").SetFont(arial).SetFontColor(new DeviceRgb(77, 77, 77)))
                 .Add(new Elements.Paragraph(Titulo + "\n ").SetFont(arial).SetFontColor(new DeviceRgb(104, 39, 148)))
                 .Add(new Elements.Paragraph("C.Prof. " + cedulaP + " / C.Esp. " + cedulaE + "\n ").SetFont(arial).SetFontColor(new DeviceRgb(77, 77, 77)))
                 .Add(new Elements.Paragraph(Esp + "\n ").SetFont(arial).SetFontColor(new DeviceRgb(77, 77, 77)))
                 .Add(new Elements.Paragraph("\n ").SetFont(arial).SetFontColor(new DeviceRgb(77, 77, 77)))
                 .Add(new Elements.Paragraph(fechapartida.Day + "                   " + fechapartida.Month.ToString().PadLeft(2,'0') + "                " + fechapartida.Year + "\n").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)));

                tableEvent.AddCell(cell);

                return tableEvent;
            }
        }

        public class FooterEventHandler1 : IEventHandler
        {
            //Image Img;
            String numC;
            String numT;

            public FooterEventHandler1( String numCell, String numTell)
            {
                //Img = img2;
                numT = numTell;
                numC = numCell;
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas1 = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);
                Rectangle rootArea = new Rectangle(255, 25, 200, 100);
                new Canvas(canvas1, pdfDoc, rootArea).Add(Traertabla(docEvent));
            }

            public Table Traertabla(PdfDocumentEvent docEvent)
            {
                float[] cellWidth = { 30f, 0f };
                Table tableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

                Style styleCell = new Style()
                    .SetBorder(Border.NO_BORDER);
                Style styleText = new Style().SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f);
                Style styleText1 = new Style().SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f);

                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                
                Elements.Cell cell = new Elements.Cell()
                    .Add(new Elements.Paragraph("\n ").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)))
                    .Add(new Elements.Paragraph(numT + "\n ").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)))
                    .Add(new Elements.Paragraph(numC + "\n").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)))
                    .AddStyle(styleText).AddStyle(styleCell).SetBorder(new SolidBorder(ColorConstants.WHITE, 1));
                tableEvent.AddCell(cell);

                return tableEvent;
            }
        }

        public class FooterEventHandler2 : IEventHandler
        {
            Image Img;
            String insta;
            String face;
            String web;
            String cor;

            public FooterEventHandler2(String facebook, String instagram, String sitioWeb, String correo)
            {
                insta = instagram;
                face = facebook;
                web = sitioWeb;
                cor = correo;
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas1 = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);
                Rectangle rootArea = new Rectangle(49, 47, 100, 60);
                new Canvas(canvas1, pdfDoc, rootArea).Add(Traertabla(docEvent));
            }

            public Table Traertabla(PdfDocumentEvent docEvent)
            {
                float[] cellWidth = { 100f, 0f };
                Table tableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

                Style styleCell = new Style()
                    .SetBorder(Border.NO_BORDER);
                Style styleText = new Style().SetTextAlignment(TextAlignment.LEFT).SetFontSize(9f);


                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                Elements.Cell cell = new Elements.Cell()
                    .Add(new Elements.Paragraph(insta + "\n").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)))
                    .Add(new Elements.Paragraph(face + "\n ").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)))
                    .Add(new Elements.Paragraph(cor + "\n").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)))
                    .Add(new Elements.Paragraph(web + "\n").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)))
                 .AddStyle(styleText).AddStyle(styleCell).SetBorder(new SolidBorder(ColorConstants.WHITE, 1));
                tableEvent.AddCell(cell);

                return tableEvent;
            }
        }

        protected void ManipulatePdf()
        {
            String doctor = ddlDoctorCita.SelectedItem.Text;
            String fecha = txtFechaCita.Text;
            String hora = txtHoraInicio.Text;
            String idcita = TxtIDCita.Text;
            DataSet datosReceta = connMySql.TraerReceta(idcita);
            String dest = "C:\\inetpub\\wwwroot\\farmacia040820\\SistemaFarmacia-Desarrollo\\SistemaFarmacia\\Imagenes\\Expediente\\Receta_" + TxtNombre.Text + "" + TxtApellidoP.Text + "" + txtFechaCita.Text + ".pdf";
            String imgF = "C:\\inetpub\\wwwroot\\farmacia040820\\SistemaFarmacia-Desarrollo\\SistemaFarmacia\\Imagenes\\RecetaFondo.jpg";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PageSize pageSize = PageSize.A4.Rotate();

            Document doc = new Document(pdfDoc, PageSize.LETTER);
        }

        protected void descargarPDF_Click(object sender, EventArgs e)
        {
            crearPdf();
        }









        protected void btnVerArchivos_Click(object sender, EventArgs e)
        {
            if (divGridArchivos.Visible == true)
            {
                divGridArchivos.Visible = false;

                btnVerArchivos.Text = "Mostrar Archivos <i class='fa fa-arrow-circle-right btnVeArchivos' aria-hidden='true'></i>";
                btnVerArchivos.ToolTip = "Mostrar Archivos";
            }
            else
            {
                String idCita = TxtIDCita.Text;
                DataSet ds = connMySql.traerExpediente(idCita);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    divGridArchivos.Visible = true;

                    btnVerArchivos.Text = "Ocultar Archivos <i class='fa fa-arrow-circle-left btnVeArchivos' aria-hidden='true'></i>";
                    btnVerArchivos.ToolTip = "Ocultar Archivos";
                }
            }
            cargarReceta();
        }

        protected void btnPestanaHistorial_Click(object sender, EventArgs e)
        {
            btnPestanaDatosCita.CssClass = "btnPestana";
            btnPestanaResulCita.CssClass = "btnPestana";
            btnPestanaHistorial.CssClass = "btnPestana btnPestanaSeleccionada";
            btnPestanaNotaEvol.CssClass = "btnPestana";

            divFContenidoDatoCita.Visible = false;
            divFContenidoResulCita.Visible = false;
            divFContenidoHistoriaC.Visible = true;
            divFContenidoNotaEvol.Visible = false;

            String Id_Cliente = TxtIDCliente.Text;

            rbtnFitzpatrick.SelectedIndex = -1;
            txtCabezaCuello.Text = "";
            txtTronco.Text = "";
            txtExtremidadesInf.Text = "";
            txtExtremidadesSup.Text = "";
            txtLocalizada.Text = "";
            txtDiseminada.Text = "";
            txtGeneralizada.Text = "";
            rbtnPredominio.SelectedIndex = -1;
            txtMorfologia.Text = "";
            txtInterrogatorio.Text = "";
            txtRestoAnexos.Text = "";
            txtSintomatologia.Text = "";
            txtTratamientosA.Text = "";
            txtAntecedentes.Text = "";
            txtDiagnostico.Text = "";
            txtTratamientoH.Text = "";
            if (connMySql.ValidaExistenciaHistoria(Id_Cliente))
            {
                DataSet HistoralClinico = connMySql.TraerHistoriaC(Id_Cliente);
                if (HistoralClinico != null)
                {

                    foreach (DataRow dRow in HistoralClinico.Tables[0].Rows)
                    {
                        String fitzpatrick = dRow["Fitzpatrick"].ToString();
                        if (fitzpatrick != "1" && fitzpatrick != "2" && fitzpatrick != "3" && fitzpatrick != "4" && fitzpatrick != "5")
                        {
                            rbtnFitzpatrick.SelectedIndex = -1;
                        }
                        else
                        {
                            rbtnFitzpatrick.SelectedValue = dRow["Fitzpatrick"].ToString();
                        }
                        txtCabezaCuello.Text = dRow["Loc_cabeza"].ToString();
                        txtTronco.Text = dRow["Loc_tronco"].ToString();
                        txtExtremidadesInf.Text = dRow["Loc_extinf"].ToString();
                        txtExtremidadesSup.Text = dRow["Loc_extsup"].ToString();
                        txtLocalizada.Text = dRow["Ext_localizada"].ToString();
                        txtDiseminada.Text = dRow["Ext_Diseminada"].ToString();
                        txtGeneralizada.Text = dRow["Ext_Generalizada"].ToString();
                        if (dRow["Predominio"].ToString() != "a" && dRow["Predominio"].ToString() != "b" && dRow["Predominio"].ToString() != "c")
                        {
                            rbtnPredominio.SelectedIndex = -1;
                        }
                        else
                        {
                            rbtnPredominio.SelectedValue = dRow["Predominio"].ToString();
                        }
                        txtMorfologia.Text = dRow["Morfologia"].ToString();
                        txtInterrogatorio.Text = dRow["Interrogatorio"].ToString();
                        txtRestoAnexos.Text = dRow["Resto_anexos"].ToString();
                        txtSintomatologia.Text = dRow["Sintomatologia"].ToString();
                        txtTratamientosA.Text = dRow["Tratamientos_anterior"].ToString();
                        txtAntecedentes.Text = dRow["Antecedentes"].ToString();
                        txtDiagnostico.Text = dRow["Diagnostico"].ToString();
                        txtTratamientoH.Text = dRow["Tratamiento"].ToString();
                    }
                }
                else
                {

                }
            }
        }
        protected void btnGuardaHistoria_Click(object sender, EventArgs e)
        {
            String id_cliente = TxtIDCliente.Text;

            String fitzpatrick = rbtnFitzpatrick.SelectedValue;
            String Cabeza_cuello = txtCabezaCuello.Text;
            String Tronco = txtTronco.Text;
            String Ext_Inf = txtExtremidadesInf.Text;
            String Ext_sup = txtExtremidadesSup.Text;
            String Localizada = txtLocalizada.Text;
            String Diseminada = txtDiseminada.Text;
            String Generalizada = txtGeneralizada.Text;
            String Predominio = rbtnPredominio.SelectedValue;
            String Morfologia = txtMorfologia.Text;
            String Interrogatorio = txtInterrogatorio.Text;
            String Resto_Anexos = txtRestoAnexos.Text;
            String Sintomatologia = txtSintomatologia.Text;
            String TratamientosA = txtTratamientosA.Text;
            String Antecedentes = txtAntecedentes.Text;
            String Diagnostico = txtDiagnostico.Text;
            String TratamientoH = txtTratamientoH.Text;

            if (connMySql.ValidaExistenciaHistoria(id_cliente))
            {
                connMySql.ActualizaHistoriaC(connMySql.traerIDEmpleado(Session["usuario"].ToString()), fitzpatrick, Cabeza_cuello, Tronco, Ext_Inf, Ext_sup, Localizada, Diseminada, Generalizada, Predominio, Morfologia, Interrogatorio, Resto_Anexos, Sintomatologia, TratamientosA, Antecedentes, Diagnostico, TratamientoH, id_cliente);
            }
            else
            {
                connMySql.GuardarHistoriaClinica(connMySql.traerIDEmpleado(Session["usuario"].ToString()), fitzpatrick, Cabeza_cuello, Tronco, Ext_Inf, Ext_sup, Localizada, Diseminada, Generalizada, Predominio, Morfologia, Interrogatorio, Resto_Anexos, Sintomatologia, TratamientosA, Antecedentes, Diagnostico, TratamientoH, id_cliente);
            }

            Div3.Visible = true;
            div5.Visible = true;
            divmensaje2.Visible = false;
        }
        protected void lbtnMostrarEscalaF_Click(object sender, EventArgs e)
        {
            if (divEscalaFitzpatrick.Visible == false)
            {
                divEscalaFitzpatrick.Visible = true;
                lbtnFitzpatrick.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnFitzpatrick.ToolTip = "Ocultar";
            }
            else
            {
                divEscalaFitzpatrick.Visible = false;
                lbtnFitzpatrick.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnFitzpatrick.ToolTip = "Mostrar";
            }
        }
        protected void lbtnMostrarLocalizacionH_Click(object sender, EventArgs e)
        {
            if (divLocalizacionH.Visible == false)
            {
                divLocalizacionH.Visible = true;
                lbtnLocalizacionH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnLocalizacionH.ToolTip = "Ocultar";
            }
            else
            {
                divLocalizacionH.Visible = false;
                lbtnLocalizacionH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnLocalizacionH.ToolTip = "Mostrar";
            }
        }
        protected void lbtnMostrarExtensionH_Click(object sender, EventArgs e)
        {
            if (divExtensionH.Visible == false)
            {
                divExtensionH.Visible = true;
                lbtnExtensionH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnExtensionH.ToolTip = "Ocultar";
            }
            else
            {
                divExtensionH.Visible = false;
                lbtnExtensionH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnExtensionH.ToolTip = "Mostrar";
            }
        }
        protected void lbtnMostrarPredominioH_Click(object sender, EventArgs e)
        {
            if (divPredominioH.Visible == false)
            {
                divPredominioH.Visible = true;
                lbtnPredominioH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnPredominioH.ToolTip = "Ocultar";
            }
            else
            {
                divPredominioH.Visible = false;
                lbtnPredominioH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnPredominioH.ToolTip = "Mostrar";
            }
        }
        protected void lbtnMostrarMorfologiaH_Click(object sender, EventArgs e)
        {
            if (divMorfologiaH.Visible == false)
            {
                divMorfologiaH.Visible = true;
                lbtnMorfologiaH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnMorfologiaH.ToolTip = "Ocultar";
            }
            else
            {
                divMorfologiaH.Visible = false;
                lbtnMorfologiaH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnMorfologiaH.ToolTip = "Mostrar";
            }
        }
        protected void lbtnInterrogatorioH_Click(object sender, EventArgs e)
        {
            if (divInterrogatorioH.Visible == false)
            {
                divInterrogatorioH.Visible = true;
                lbtnInterrogatorioH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnInterrogatorioH.ToolTip = "Ocultar";
            }
            else
            {
                divInterrogatorioH.Visible = false;
                lbtnInterrogatorioH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnInterrogatorioH.ToolTip = "Mostrar";
            }
        }
        protected void lbtnAnexosH_Click(object sender, EventArgs e)
        {
            if (divAnexosH.Visible == false)
            {
                divAnexosH.Visible = true;
                lbtnAnexosH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnAnexosH.ToolTip = "Ocultar";
            }
            else
            {
                divAnexosH.Visible = false;
                lbtnAnexosH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnAnexosH.ToolTip = "Mostrar";
            }
        }
        protected void lbtnSintomatologiaH_Click(object sender, EventArgs e)
        {
            if (divSintomatologiaH.Visible == false)
            {
                divSintomatologiaH.Visible = true;
                lbtnSintomatologiaH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnSintomatologiaH.ToolTip = "Ocultar";
            }
            else
            {
                divSintomatologiaH.Visible = false;
                lbtnSintomatologiaH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnSintomatologiaH.ToolTip = "Mostrar";
            }
        }
        protected void lbtnTratamientosH_Click(object sender, EventArgs e)
        {
            if (divTratamientosH.Visible == false)
            {
                divTratamientosH.Visible = true;
                lbtnTratamientosH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnTratamientosH.ToolTip = "Ocultar";
            }
            else
            {
                divTratamientosH.Visible = false;
                lbtnTratamientosH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnTratamientosH.ToolTip = "Mostrar";
            }
        }
        protected void lbtnAntecedentesH_Click(object sender, EventArgs e)
        {
            if (divAntecedentesH.Visible == false)
            {
                divAntecedentesH.Visible = true;
                lbtnAntecedentesH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnAntecedentesH.ToolTip = "Ocultar";
            }
            else
            {
                divAntecedentesH.Visible = false;
                lbtnAntecedentesH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnAntecedentesH.ToolTip = "Mostrar";
            }
        }
        protected void lbtnDiagnosticoH_Click(object sender, EventArgs e)
        {
            if (divDiagnosticoH.Visible == false)
            {
                divDiagnosticoH.Visible = true;
                lbtnDiagnosticoH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnDiagnosticoH.ToolTip = "Ocultar";
            }
            else
            {
                divDiagnosticoH.Visible = false;
                lbtnDiagnosticoH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnDiagnosticoH.ToolTip = "Mostrar";
            }
        }
        protected void lbtnTratamientoH_Click(object sender, EventArgs e)
        {
            if (divTratamientoH.Visible == false)
            {
                divTratamientoH.Visible = true;
                lbtnTratamientoH.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnTratamientoH.ToolTip = "Ocultar";
            }
            else
            {
                divTratamientoH.Visible = false;
                lbtnTratamientoH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnTratamientoH.ToolTip = "Mostrar";
            }
        }
        public void OcultarCamposHistorial()
        {
            divEscalaFitzpatrick.Visible = true;
            lbtnFitzpatrick.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
            lbtnFitzpatrick.ToolTip = "Ocultar";
            divLocalizacionH.Visible = false;
            lbtnLocalizacionH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnLocalizacionH.ToolTip = "Mostrar";
            divExtensionH.Visible = false;
            lbtnExtensionH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnExtensionH.ToolTip = "Mostrar";
            divPredominioH.Visible = false;
            lbtnPredominioH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnPredominioH.ToolTip = "Mostrar";
            divMorfologiaH.Visible = false;
            lbtnMorfologiaH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnMorfologiaH.ToolTip = "Mostrar";
            divInterrogatorioH.Visible = false;
            lbtnInterrogatorioH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnInterrogatorioH.ToolTip = "Mostrar";
            divAnexosH.Visible = false;
            lbtnAnexosH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnAnexosH.ToolTip = "Mostrar";
            divSintomatologiaH.Visible = false;
            lbtnSintomatologiaH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnSintomatologiaH.ToolTip = "Mostrar";
            divTratamientosH.Visible = false;
            lbtnTratamientosH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnTratamientosH.ToolTip = "Mostrar";
            divAntecedentesH.Visible = false;
            lbtnAntecedentesH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnAntecedentesH.ToolTip = "Mostrar";
            divDiagnosticoH.Visible = false;
            lbtnDiagnosticoH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnDiagnosticoH.ToolTip = "Mostrar";
            divTratamientoH.Visible = false;
            lbtnTratamientoH.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnTratamientoH.ToolTip = "Mostrar";
        }
        protected void btnPestanaNotaEvol_Click(object sender, EventArgs e)
        {
            btnPestanaDatosCita.CssClass = "btnPestana";
            btnPestanaResulCita.CssClass = "btnPestana";
            btnPestanaHistorial.CssClass = "btnPestana";
            btnPestanaNotaEvol.CssClass = "btnPestana btnPestanaSeleccionada";

            divFContenidoDatoCita.Visible = false;
            divFContenidoResulCita.Visible = false;
            divFContenidoHistoriaC.Visible = false;
            divFContenidoNotaEvol.Visible = true;

            String Id_Cita = TxtIDCita.Text;
            String id_cliente = TxtIDCliente.Text;

            //txtTAnota.Text = "";
            //txtFCnota.Text = "";
            //txtFRnota.Text = "";
            //txtTempNota.Text = "";
            //txtPesoNota.Text = "";
            //txtTallaNota.Text = "";
            //txtEvolucionNota.Text = "";
            //txtDiagnosticoNota.Text = "";
            //txtTratamientoNota.Text = "";
            //txtFechaCitaProx.Text = "";
            //txtHoraICitaProx.Text = "";
            //txtHoraFCitaProx.Text = "";

            RestableceCamposNotaEvol();

            if (connMySql.ValidaExistenciaEvolucion(Id_Cita))
            {
                DataSet NotaEvolucion = connMySql.TraerNotaEvolucion(Id_Cita);
                if (NotaEvolucion != null)
                {
                    foreach (DataRow dRow in NotaEvolucion.Tables[0].Rows)
                    {
                        txtTAnota.Text = dRow["ta"].ToString();
                        txtFCnota.Text = dRow["fc"].ToString();
                        txtFRnota.Text = dRow["fr"].ToString();
                        txtTempNota.Text = dRow["temperatura"].ToString();
                        txtPesoNota.Text = dRow["peso"].ToString();
                        txtTallaNota.Text = dRow["talla"].ToString();
                        txtEvolucionNota.Text = dRow["evolucion"].ToString();
                        txtDiagnosticoNota.Text = dRow["diagnostico"].ToString();
                        txtTratamientoNota.Text = dRow["tratamiento"].ToString();
                    }
                }
            }
            DataSet ProximaCita = connMySql.TraerProxCita(id_cliente, txtFechaCita.Text);
            if (ProximaCita != null)
            {
                foreach (DataRow dRow in ProximaCita.Tables[0].Rows)
                {
                    txtFechaCitaProx.Text = dRow["fecha"].ToString();
                    txtHoraICitaProx.Text = dRow["hora_inicio"].ToString();
                    txtHoraFCitaProx.Text = dRow["hora_fin"].ToString();
                }
            }
            TraerNotasEvolucion(id_cliente);
        }


        protected void btnAgendarProxCita_Click(object sender, EventArgs e)
        {
            Div3.Visible = true;
            div4.Visible = false;
            divProxCita.Visible = true;
            divmensaje2.Visible = false;
        }
        protected void btnCancelaProxCita_Click(object sender, EventArgs e)
        {
            Div3.Visible = false;
            divProxCita.Visible = false;

            txtFechaProxCitaAg.Text = "";
            txtHoraIProxCitaAg.Text = "";
            txtHoraFProxCitaAg.Text = "";
            MsjErrorCitaProx.Text = "";
        }
        protected void btnAgendaCitaProx_Click(object sender, EventArgs e)
        {
            String CitaExistente = connMySql.validaCitaColisionada(ddlDoctor.SelectedValue, txtFechaProxCitaAg.Text, txtHoraIProxCitaAg.Text, txtHoraFProxCitaAg.Text, "");

            if (CitaExistente.Length == 0)
            {
                connMySql.AgregarCita(connMySql.traerIDEmpleado(Session["usuario"].ToString()), txtFechaProxCitaAg.Text, txtHoraIProxCitaAg.Text, txtHoraFProxCitaAg.Text, TxtIDCliente.Text, txtNotaCitaAg.Text, ddlDoctorCita.SelectedValue, "1");

                divMsjOKcitaProx.Visible = true;

                txtFechaProxCitaAg.Text = "";
                txtHoraIProxCitaAg.Text = "";
                txtHoraFProxCitaAg.Text = "";
                txtNotaCitaAg.Text = "";

                MsjErrorCitaProx.Text = "";
                Div3.Visible = true;
                divProxCita.Visible = false;

                String id_cliente = TxtIDCliente.Text;
                DataSet ProximaCita = connMySql.TraerProxCita(id_cliente, txtFechaCita.Text);
                if (ProximaCita != null)
                {
                    foreach (DataRow dRow in ProximaCita.Tables[0].Rows)
                    {
                        txtFechaCitaProx.Text = dRow["fecha"].ToString();
                        txtHoraICitaProx.Text = dRow["hora_inicio"].ToString();
                        txtHoraFCitaProx.Text = dRow["hora_fin"].ToString();
                    }
                }
            }
            else
            {
                MsjErrorCitaProx.Text = "El horario seleccionado coincide con una cita del " + CitaExistente;
            }
        }
        protected void btnOkCitaProx_Click(object sender, EventArgs e)
        {
            divMsjOKcitaProx.Visible = false;
            Div3.Visible = false;
        }
        protected void btnGuardaNotaEvolucion_Click(object sender, EventArgs e)
        {
            String id_cita = TxtIDCita.Text;
            String TA = txtTAnota.Text;
            String FC = txtFCnota.Text;
            String FR = txtFRnota.Text;
            String Temperatura = txtTempNota.Text;
            String Peso = txtPesoNota.Text;
            String Talla = txtTallaNota.Text;
            String Evolucion = txtEvolucionNota.Text;
            String Diagnostico = txtDiagnosticoNota.Text;
            String Tratamiento = txtTratamientoNota.Text;

            if (connMySql.ValidaExistenciaEvolucion(id_cita))
            {
                connMySql.ActualizaNotaEvolucion(connMySql.traerIDEmpleado(Session["usuario"].ToString()), TA, FC, FR, Temperatura, Peso, Talla, Evolucion, Diagnostico, Tratamiento, id_cita);
            }
            else
            {
                connMySql.GuardarNotaEvolucion(connMySql.traerIDEmpleado(Session["usuario"].ToString()), TA, FC, FR, Temperatura, Peso, Talla, Evolucion, Diagnostico, Tratamiento, id_cita);
            }

            Div3.Visible = true;
            div5.Visible = true;
            divmensaje2.Visible = false;
        }

        protected void linkMostrarNota_Click(object sender, EventArgs e)
        {
            divFormNotaConsulta.Visible = true;
            divFormularioCita.Visible = false;

            GridViewRow row = ((GridViewRow)((LinkButton)sender).NamingContainer);

            txtEvolConsulta.Text = ((Label)row.FindControl("evolucion")).Text;
            txtDiagnosticoConsulta.Text = ((Label)row.FindControl("diagnostico")).Text;
            fechanotaevol.InnerText = "Fecha: " + ((Label)row.FindControl("fecha_creacion")).Text;
            txtTAconsulta.Text = ((Label)row.FindControl("ta")).Text;
            txtFCconsulta.Text = ((Label)row.FindControl("fc")).Text;
            txtFRconsulta.Text = ((Label)row.FindControl("fr")).Text;
            txtTempConsulta.Text = ((Label)row.FindControl("temperatura")).Text;
            txtPesoConsulta.Text = ((Label)row.FindControl("peso")).Text;
            txtTallaConsulta.Text = ((Label)row.FindControl("talla")).Text;
            txtTratamientoConsulta.Text = ((Label)row.FindControl("tratamiento")).Text;

        }
        protected void btnCerrarCNota_Click(object sender, EventArgs e)
        {
            divFormNotaConsulta.Visible = false;
            divFormularioCita.Visible = true;

            txtEvolConsulta.Text = "";
            txtDiagnosticoConsulta.Text = "";
            fechanotaevol.InnerText = "";
            txtTAconsulta.Text = "";
            txtFCconsulta.Text = "";
            txtFRconsulta.Text = "";
            txtTempConsulta.Text = "";
            txtPesoConsulta.Text = "";
            txtTallaConsulta.Text = "";
            txtTratamientoConsulta.Text = "";
        }
        protected void lbtnNotasEvolucion_Click(object sender, EventArgs e)
        {
            if (divNotasEvolucionGral.Visible == false)
            {
                divNotasEvolucionGral.Visible = true;
                lbtnNotasEvolucion.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnNotasEvolucion.ToolTip = "Ocultar";
            }
            else
            {
                divNotasEvolucionGral.Visible = false;
                lbtnNotasEvolucion.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnNotasEvolucion.ToolTip = "Mostrar";
            }
        }
        protected void lbtnNotaActual_Click(object sender, EventArgs e)
        {
            if (divNotaActual.Visible == false)
            {
                divNotaActual.Visible = true;
                lbtnNotaActual.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
                lbtnNotaActual.ToolTip = "Ocultar";
            }
            else
            {
                divNotaActual.Visible = false;
                lbtnNotaActual.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
                lbtnNotaActual.ToolTip = "Mostrar";
            }
        }
        public void TraerNotasEvolucion(String id_cliente)
        {
            DataSet ds = connMySql.TraerNotasEvolucionCliente(id_cliente);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvNotasEvolucionGral.DataSource = ds.Tables[0];
                gvNotasEvolucionGral.DataBind();
            }
            else
            {
                gvNotasEvolucionGral.DataSource = ds.Tables[0];
                gvNotasEvolucionGral.DataBind();

                int totalColumnas = ds.Tables[0].Columns.Count;

                if (gvNotasEvolucionGral.Rows.Count == 0)
                {
                    DataTable dtTemporal = new DataTable();
                    dtTemporal.Columns.Add("ID_Nota");
                    dtTemporal.Columns.Add("Evolucion");
                    dtTemporal.Columns.Add("Diagnostico");
                    dtTemporal.Columns.Add("Fecha");
                    dtTemporal.Columns.Add("TA");
                    dtTemporal.Columns.Add("FC");
                    dtTemporal.Columns.Add("FR");
                    dtTemporal.Columns.Add("Temperatura");
                    dtTemporal.Columns.Add("Peso");
                    dtTemporal.Columns.Add("Talla");
                    dtTemporal.Columns.Add("Tratamiento");
                    dtTemporal.NewRow();
                    DataRow drTemporal = dtTemporal.NewRow();
                    dtTemporal.Rows.InsertAt(drTemporal, 0);

                    gvNotasEvolucionGral.DataSource = dtTemporal;
                    gvNotasEvolucionGral.DataBind();
                }

                gvNotasEvolucionGral.Rows[0].Cells.Clear();
                gvNotasEvolucionGral.Rows[0].Cells.Add(new TableCell());
                gvNotasEvolucionGral.Rows[0].Cells[0].ColumnSpan = 11;
                gvNotasEvolucionGral.Rows[0].Cells[0].CssClass = "lblSinResultado";
                gvNotasEvolucionGral.Rows[0].Cells[0].Text = "Sin resultados";

                gvNotasEvolucionGral.Visible = true;
            }
        }
        public void RestableceCamposNotaEvol()
        {
            divNotaActual.Visible = true;
            lbtnNotaActual.Text = "<i class='fa fa-angle-up' aria-hidden='true'></i>";
            lbtnNotaActual.ToolTip = "Ocultar";
            divNotasEvolucionGral.Visible = false;
            lbtnNotasEvolucion.Text = "<i class='fa fa-angle-down' aria-hidden='true'></i>";
            lbtnNotasEvolucion.ToolTip = "Mostrar";

            txtTAnota.Text = "";
            txtFCnota.Text = "";
            txtFRnota.Text = "";
            txtTempNota.Text = "";
            txtPesoNota.Text = "";
            txtTallaNota.Text = "";
            txtEvolucionNota.Text = "";
            txtDiagnosticoNota.Text = "";
            txtTratamientoNota.Text = "";
            txtFechaCitaProx.Text = "";
            txtHoraICitaProx.Text = "";
            txtHoraFCitaProx.Text = "";
        }







        protected void btnDesgargarNE_Click(object sender, EventArgs e)
        {
            crearPdfEvolucion();
        }

        public void crearPdfEvolucion()
        {

            String anota = txtTAnota.Text;
            String fcnota = txtFCnota.Text;
            String frnota = txtFRnota.Text;
            String tempnota = txtTempNota.Text;
            String pesonota = txtPesoNota.Text;
            String tallanota = txtTallaNota.Text;
            String evolnota = txtEvolucionNota.Text;
            String diagnosticonota = txtDiagnosticoNota.Text;
            String tratamientonota = txtTratamientoNota.Text;
            String fechacitaprox = txtFechaCitaProx.Text;
            String nombrecliente = TxtNombre.Text;
            String apellidoPcliente = TxtApellidoP.Text;
            String apellidoMcliente = TxtApellidoM.Text;


            String doctor = ddlDoctorCita.SelectedItem.Text;
            String doctorid = ddlDoctorCita.SelectedValue;

            String fecha = txtFechaCita.Text;
            String hora = txtHoraInicio.Text;
            String idcita = TxtIDCita.Text;
            DateTime fechapartida = DateTime.Parse(fecha);
            DataSet datosReceta = connMySql.TraerReceta(idcita);
            //String path = "C:/inetpub/wwwroot/farmacia040820/SistemaFarmacia-Desarrollo/SistemaFarmacia/";
            String path = Server.MapPath("Citas.aspx.cs").Remove(Server.MapPath("Citas.aspx.cs").IndexOf("Citas.aspx.cs"));
            String imgF = "Imagenes/NotaEvolucionFondo.jpg";
            String RUTA = "Pdf/NotaEvolucion_" + TxtNombre.Text + "" + TxtApellidoP.Text + "" + txtFechaCita.Text + ".pdf";
            PdfWriter pdfEscrito = new PdfWriter(path + RUTA);

            PdfDocument pdf = new PdfDocument(pdfEscrito);
            PageSize pageSize = PageSize.LETTER;
            Document documento = new Document(pdf, PageSize.LETTER);

            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            PdfCanvas canvas = new PdfCanvas(pdf.AddNewPage());
            canvas.AddImage(ImageDataFactory.Create(path + imgF), pageSize, false);

            documento.SetMargins(210, 40, 90, 40);


            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new HeaderEventHandlerevolt1(nombrecliente, apellidoPcliente, apellidoMcliente, fecha, hora, fechacitaprox));
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new FooterEventHandlerevolt2(tallanota, fcnota, frnota, tempnota, pesonota, anota));

            documento.Add(new Elements.Paragraph("Evolucion: " + evolnota).SetFontSize(11f));
            documento.Add(new Elements.Paragraph("").SetFontSize(11f));
            documento.Add(new Elements.Paragraph("Diagnostico: " + diagnosticonota).SetFontSize(11f));
            documento.Add(new Elements.Paragraph("").SetFontSize(11f));
            documento.Add(new Elements.Paragraph("Tratamiento: " + tratamientonota).SetFontSize(11f));


            documento.Close();


            try
            {
                //string strURL = "Imagenes/Expediente/Receta_" + TxtNombre.Text + "" + TxtApellidoP.Text + "" + txtFechaCita.Text + ".pdf";
                System.Net.WebClient req = new System.Net.WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=\"NotaEvolucion_" + TxtNombre.Text + "" + TxtApellidoP.Text + "" + txtFechaCita.Text + ".pdf");
                byte[] data = req.DownloadData(Server.MapPath(RUTA));
                response.BinaryWrite(data);
                response.End();
            }
            catch (Exception ex)
            {
            }
        }
        public class HeaderEventHandlerevolt1 : IEventHandler
        {
            //Image Img;
            String nombreC;
            String apellidoP;
            String apellidoM;
            String Fecha;
            String Hora;
            String proxcita;

            public HeaderEventHandlerevolt1(String nombrecliente, String apellidoPcliente, String apellidoMcliente, String fecha, String hora, String fechacitaprox)
            {
                //Img = img2;
                nombreC = nombrecliente;
                apellidoP = apellidoPcliente;
                apellidoM = apellidoMcliente;
                Fecha = fecha;
                Hora = hora;
                proxcita = fechacitaprox;



            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas1 = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);
                Rectangle rootArea = new Rectangle(30, 587, 400, 100);
                new Canvas(canvas1, pdfDoc, rootArea).Add(Traertabla(docEvent));
            }

            public Table Traertabla(PdfDocumentEvent docEvent)
            {
                Char delimitador = ':';
                String[] fracTiempo = Hora.Split(delimitador);
                DateTime fechapartida = DateTime.Parse(Fecha);
                DateTime fechaprox = DateTime.Parse(proxcita);

                float[] cellWidth = { 30f, 0f };
                Table tableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

                Style styleCell = new Style()
                    .SetBorder(Border.NO_BORDER);
                Style styleText = new Style().SetTextAlignment(TextAlignment.CENTER).SetFontSize(10f);
                Style styleText1 = new Style().SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f);

                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);

                Elements.Cell cell = new Elements.Cell()
                    .AddStyle(styleText).AddStyle(styleCell)
                    .Add(new Elements.Paragraph(nombreC + "    " + apellidoP + "      " + apellidoM + "\n ").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)))
                     .Add(new Elements.Paragraph("\n").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)))
                    .Add(new Elements.Paragraph(fechapartida.Day + "           " + fechapartida.Month.ToString().PadLeft(2, '0') + "         " + fechapartida.Year + "                        " + fracTiempo[0] + ":" + fracTiempo[1] + "                                       " + fechaprox.Day + "/" + fechaprox.Month + "/" + fechaprox.Year + "\n ").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)))
                    .Add(new Elements.Paragraph("\n").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)));

                tableEvent.AddCell(cell);

                return tableEvent;
            }
        }

        public class FooterEventHandlerevolt2 : IEventHandler
        {
            String Tallanota;
            String Fcnota;
            String Frnota;
            String Tempnota;
            String Pesonota;
            String Anota;

            public FooterEventHandlerevolt2(String tallanota, String fcnota, String frnota, String tempnota, String pesonota, String anota)

            {

                Tallanota = tallanota;
                Fcnota = fcnota;
                Frnota = frnota;
                Tempnota = tempnota;
                Pesonota = pesonota;
                Anota = anota;

            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas1 = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);
                Rectangle rootArea = new Rectangle(12, 539, 600, 60);
                new Canvas(canvas1, pdfDoc, rootArea).Add(Traertabla(docEvent));
            }

            public Table Traertabla(PdfDocumentEvent docEvent)
            {
                float[] cellWidth = { 100f, 0f };
                Table tableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

                Style styleCell = new Style()
                    .SetBorder(Border.NO_BORDER);
                Style styleText = new Style().SetTextAlignment(TextAlignment.CENTER).SetFontSize(9f);


                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);

                Elements.Cell cell = new Elements.Cell()
                    .AddStyle(styleText).AddStyle(styleCell)
                    .Add(new Elements.Paragraph(Anota.PadRight(39,' ') + Fcnota.PadRight(39, ' ') + Frnota.PadRight(39, ' ') + Tempnota.PadRight(39, ' ')+ Pesonota.PadRight(39, ' ') + Tallanota.PadRight(39, ' ') + "\n").SetFont(bold).SetFontColor(new DeviceRgb(77, 77, 77)));


                tableEvent.AddCell(cell);

                return tableEvent;
            }
        }
    }
}