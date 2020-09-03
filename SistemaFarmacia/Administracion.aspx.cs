using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SistemaFarmacia.Clases;

namespace SistemaFarmacia
{
    public partial class Administracion : System.Web.UI.Page
    {
        Conexion connMysql = new Conexion();
        String permisos = "";

        
        ListItem lITodos = new ListItem("Todos", "-1");

        protected void Page_Load(object sender, EventArgs e)
        {
            try {

                MasterFarmacia master = (MasterFarmacia)this.Master;
                
                try
                {
                    permisos = Session["Permisos"].ToString();
                }
                catch (Exception ex)
                {
                    sombraMensaje.Visible = true;
                    mostrarMensaje("Su sesión ha caducado, vuelva a hacer login.");
                    divGerentes.Visible = false;
                    divPerfiles.Visible = false;
                    divCorreo.Visible = false;
                    divEnvioCorreo.Visible = false;
                    divClientes.Visible = false;

                    btnOkSalir.Visible = true;
                    MOk.Visible = false;
                }

                if (SesionViva())
                {
                    if (!IsPostBack)
                    {

                        //master.cambiarLblTitle("<img src='Imagenes/Administracion.png' alt='clientes'><h1>Administración</h1>");

                        if (!(permisos.Contains("31") || permisos.Contains("32") || permisos.Contains("33") || permisos.Contains("34") || permisos.Contains("35") || permisos.Contains("36") || permisos.Contains("37") || permisos.Contains("38") || permisos.Contains("39") || permisos.Contains("310") || permisos.Contains("311") || permisos.Contains("314") || permisos.Contains("315") || permisos.Contains("316")) )
                        {
                            Response.Redirect("Principal.aspx");
                        }

                        master.mostrarLblUser("<p>Usuario: " + this.Session["usuario"].ToString() + " </p>");
                        master.mostrarMensaje(false);
                        sombraMensaje.Visible = false;

                        divGerentes.Visible = true;
                        divPerfiles.Visible = false;
                        divCorreo.Visible = false;
                        divEnvioCorreo.Visible = false;
                        divClientes.Visible = false;
                        divConfigCitas.Visible = false;

                        btnOpcionUsuario.CssClass = "seleccionado";
                        btnOpcionPerfil.CssClass = "";
                        btnOpcionCorreo.CssClass = "";
                        btnOpcionClientes.CssClass = "";
                        btnConfigCitas.CssClass = "";
                        cargaGerentes();

                        btnOpcionUsuario.Visible = false;
                        btnOpcionPerfil.Visible = false;
                        btnOpcionCorreo.Visible = false;
                        btnEnvioCorreo.Visible = false;

                        String PrimeraOpcion = "";

                        if (Session["CondicionClientes"] == null)
                        {
                            Session.Add("CondicionClientes", " Enviar_Correo = '1' ");
                        }
                        else
                        {
                            Session["CondicionClientes"] = " Enviar_Correo = '1' ";
                        }


                        if (Session["OrdenClientes"] == null)
                        {
                            Session.Add("OrdenClientes", "");
                        }
                        else
                        {
                            Session["OrdenClientes"] = "";
                        }


                        if (Session["EnvioClientes"] == null)
                        {
                            Session.Add("EnvioClientes", new List<ClienteCorreo>());
                        }
                        else
                        {
                            Session["EnvioClientes"] = new List<ClienteCorreo>();
                        }

                        if (Session["OrdenConfirCorreos"] == null)
                        {
                            Session.Add("OrdenConfirCorreos", "");
                        }
                        else
                        {
                            Session["OrdenConfirCorreos"] = "";
                        }
                        



                        if (permisos.Contains("31") || permisos.Contains("32") || permisos.Contains("33") || permisos.Contains("34"))
                        {
                            PrimeraOpcion = "Usuario";
                            btnOpcionUsuario.Visible = true;
                            if (!permisos.Contains("32"))
                            {
                                btnAgrUsuarioG.Visible = false;
                            }
                        }

                        if (permisos.Contains("35") || permisos.Contains("36") || permisos.Contains("37") || permisos.Contains("38"))
                        {
                            if (PrimeraOpcion.Length == 0)
                            {
                                PrimeraOpcion = "Perfil";
                            }
                            btnOpcionPerfil.Visible = true;

                            if (!permisos.Contains("36"))
                            {
                                btnAgregarP.Visible = false;
                            }
                        }

                        if (permisos.Contains("39") || permisos.Contains("310") || permisos.Contains("311") || permisos.Contains("314") || permisos.Contains("315") || permisos.Contains("316"))
                        {
                            if (PrimeraOpcion.Length == 0)
                            {
                                PrimeraOpcion = "Correo";

                                if (permisos.Contains("39"))
                                {
                                    btnActCorreoTecn.Visible = true;
                                }
                                if (permisos.Contains("310"))
                                {
                                    btnActCorreoConf.Visible = true;
                                }

                                if (permisos.Contains("311"))
                                {
                                    btnActCorreoCont.Visible = true;
                                }

                                if (permisos.Contains("314"))
                                {
                                    btnActCorreoContSelec.Visible = true;
                                }

                                if (permisos.Contains("315"))
                                {
                                    btnActCorreoRecordatorio.Visible = true;
                                }
                                

                                if (permisos.Contains("39"))
                                {
                                    btnConfiguracionTecnica.CssClass = "subOpcion subOpcionSeleccionado";
                                    divCorreoConfTec.Visible = true;
                                }
                                else
                                {
                                    if (permisos.Contains("310"))
                                    {
                                        btnConfiguracionCorreo.CssClass = "subOpcion subOpcionSeleccionado";
                                        divCorreoCont.Visible = true;
                                    }
                                    else
                                    {
                                        if (permisos.Contains("311"))
                                        {
                                            btnContenidCorreo.CssClass = "subOpcion subOpcionSeleccionado";
                                            divCorreoConfCorreo.Visible = true;
                                            btnPreview.Visible = true;
                                        }
                                        else
                                        {
                                            if (permisos.Contains("314"))
                                            {
                                                btnContenidCorreoSelec.CssClass = "subOpcion subOpcionSeleccionado";
                                                btnContenidCorreoSelec.Visible = true;
                                                divCorreoContSelec.Visible = true;
                                            }
                                            else
                                            {
                                                if (permisos.Contains("315"))
                                                {
                                                    btnCorreoRecordatorio.CssClass = "subOpcion subOpcionSeleccionado";
                                                    btnCorreoRecordatorio.Visible = true;
                                                    divCorreoRecordatorio.Visible = true;
                                                }
                                                else if (permisos.Contains("316"))
                                                {
                                                    btnTiposCorreo.CssClass = "subOpcion subOpcionSeleccionado";
                                                    btnTiposCorreo.Visible = true;
                                                    divTiposCorreo.Visible = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            btnOpcionCorreo.Visible = true;
                        }

                        if (permisos.Contains("312"))
                        {
                            if (PrimeraOpcion.Length == 0)
                            {
                                PrimeraOpcion = "EnvioCorreo";
                                divEnvioCorreo.Visible = true;

                            }
                            llenaEstados();
                            llenaPaises();
                            llenarMedio();
                            btnEnvioCorreo.Visible = true;
                        }

                        if (permisos.Contains("313"))
                        {
                            if (PrimeraOpcion.Length == 0)
                            {
                                PrimeraOpcion = "Clientes";
                            }
                            btnOpcionClientes.Visible = true;
                        }

                        switch (PrimeraOpcion)
                        {
                            case "Usuario":
                                btnOpcionUsuario.CssClass = "seleccionado";
                                CambiaTitulo("Usuarios");
                                break;

                            case "Perfil":
                                btnOpcionPerfil.CssClass = "seleccionado";
                                CambiaTitulo("Perfiles");
                                break;

                            case "Correo":
                                btnOpcionCorreo.CssClass = "seleccionado";
                                CambiaTitulo("Configuración técnica");
                                break;

                            case "EnvioCorreo":
                                btnEnvioCorreo.CssClass = "seleccionado";
                                CambiaTitulo("Envío de correo");
                                break;

                            case "Clientes":
                                btnOpcionClientes.CssClass = "seleccionado";
                                CambiaTitulo("Reactivar clientes");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sombraMensaje.Visible = true;
                mostrarMensaje("Su sesión ha caducado, vuelva a hacer login.");
                btnOkSalir.Visible = true;
                MOk.Visible = false;
            }
        }

        public void SeleccionaBtn(String boton)
        {
            divGerentes.Visible = false;
            divPerfiles.Visible = false;
            divCorreo.Visible = false;
            divEnvioCorreo.Visible = false;

            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "";
            btnEnvioCorreo.CssClass = "";

            switch (boton)
            {
                case "btnOpcionUsuario":
                    divGerentes.Visible = true;
                    btnOpcionUsuario.CssClass = "seleccionado";

                    cargaGerentes();
                    ocultarOpcionesCorreo();
                    break;

                case "btnOpcionPerfil":
                    divPerfiles.Visible = true;
                    btnOpcionPerfil.CssClass = "seleccionado";

                    cargaPerfiles();
                    ocultarOpcionesCorreo();
                    break;

                case "btnOpcionCorreo":
                    divCorreo.Visible = true;
                    btnOpcionCorreo.CssClass = "seleccionado";

                    break;

                case "btnEnvioCorreo":
                    break;

                default:
                    break;
            }
        }

        protected void btnOpcionUsuario_Click(object sender, EventArgs e)
        {
            divGerentes.Visible = true;
            divPerfiles.Visible = false;
            divCorreo.Visible = false;
            divEnvioCorreo.Visible = false;
            divClientes.Visible = false;
            divConfigCitas.Visible = false;

            btnConfigCitas.CssClass = "";
            btnOpcionUsuario.CssClass = "seleccionado";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "";
            btnEnvioCorreo.CssClass = "";
            btnOpcionClientes.CssClass = "";
            //btnOpcionClientes.Visible = false;

            CambiaTitulo("Usuarios");

            cargaGerentes();
            ocultarOpcionesCorreo();
        }

        protected void btnOpcionPerfil_Click(object sender, EventArgs e)
        {
            divGerentes.Visible = false;
            divPerfiles.Visible = true;
            divCorreo.Visible = false;
            divEnvioCorreo.Visible = false;
            divClientes.Visible = false;
            divConfigCitas.Visible = false;

            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "seleccionado";
            btnOpcionCorreo.CssClass = "";
            btnEnvioCorreo.CssClass = "";
            btnOpcionClientes.CssClass = "";
            btnConfigCitas.CssClass = "";
            //btnOpcionClientes.Visible = false;

            CambiaTitulo("Perfiles");

            cargaPerfiles();
            ocultarOpcionesCorreo();
        }
        
        protected void btnOpcionCorreo_Click(object sender, EventArgs e)
        {
            CambiaTitulo("Configuración Técnica");

            divGerentes.Visible = false;
            divPerfiles.Visible = false;
            divCorreo.Visible = true;
            divEnvioCorreo.Visible = false;
            divClientes.Visible = false;
            divConfigCitas.Visible = false;

            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "seleccionado";
            btnEnvioCorreo.CssClass = "";
            btnOpcionClientes.CssClass = "";
            btnConfigCitas.CssClass = "";

            //btnOpcionClientes.Visible = false;
            btnConfiguracionCorreo.Visible = false;
            btnConfiguracionTecnica.Visible = false;
            btnContenidCorreo.Visible = false;

            btnContenidCorreoSelec.Visible = false;
            cargaCorreoSelec();

            btnCorreoRecordatorio.Visible = true;
            cargaCorreoRec();

            cargaCorreo();
            
            if (permisos.Contains("39"))
            {
                btnConfiguracionTecnica.Visible = true;
            }

            if (permisos.Contains("310"))
            {
                btnConfiguracionCorreo.Visible = true;
            }

            if (permisos.Contains("311"))
            {
                btnContenidCorreo.Visible = true;
                //btnPreview.Visible = true;
            }

            if (permisos.Contains("314"))
            {
                btnContenidCorreoSelec.Visible = true;
                //btnPreview.Visible = true;
            }

            if (permisos.Contains("315"))
            {
                btnCorreoRecordatorio.Visible = true;
            }

            if (permisos.Contains("316"))
            {
                btnTiposCorreo.Visible = true;
                CargarTipoCorreo("");
            }


            if (permisos.Contains("39"))
            {
                btnConfiguracionTecnica.CssClass = "subOpcion subOpcionSeleccionado";
                btnConfiguracionCorreo.CssClass = "subOpcion";
                btnContenidCorreo.CssClass = "subOpcion";
                btnContenidCorreoSelec.CssClass = "subOpcion";
                btnCorreoRecordatorio.CssClass = "subOpcion";
                btnTiposCorreo.CssClass = "subOpcion";

                divCorreoConfTec.Visible = true;
                divCorreoConfCorreo.Visible = false;
                divCorreoCont.Visible = false;
                divCorreoContSelec.Visible = false;
                divCorreoRecordatorio.Visible = false;
                divTiposCorreo.Visible = false;

                btnActCorreoTecn.Visible = true;
                btnActCorreoConf.Visible = false;
                btnActCorreoCont.Visible = false;
                btnActCorreoContSelec.Visible = false;
                btnActCorreoRecordatorio.Visible = false;
                btnActTipo.Visible = false;
            }
            else
            {
                if (permisos.Contains("310"))
                {
                    btnConfiguracionTecnica.CssClass = "subOpcion";
                    btnConfiguracionCorreo.CssClass = "subOpcion subOpcionSeleccionado";
                    btnContenidCorreo.CssClass = "subOpcion";
                    btnContenidCorreoSelec.CssClass = "subOpcion";
                    btnCorreoRecordatorio.CssClass = "subOpcion";
                    btnTiposCorreo.CssClass = "subOpcion";

                    divCorreoConfTec.Visible = false;
                    divCorreoConfCorreo.Visible = true;
                    divCorreoCont.Visible = false;
                    divCorreoContSelec.Visible = false;
                    divCorreoRecordatorio.Visible = false;
                    divTiposCorreo.Visible = false;

                    btnActCorreoTecn.Visible = false;
                    btnActCorreoConf.Visible = true;
                    btnActCorreoCont.Visible = false;
                    btnActCorreoContSelec.Visible = false;
                    btnActCorreoRecordatorio.Visible = false;
                    btnActTipo.Visible = false;
                }
                else
                {
                    if (permisos.Contains("311"))
                    {
                        btnConfiguracionTecnica.CssClass = "subOpcion";
                        btnConfiguracionCorreo.CssClass = "subOpcion";
                        btnContenidCorreo.CssClass = "subOpcion subOpcionSeleccionado";
                        btnContenidCorreoSelec.CssClass = "subOpcion";
                        btnCorreoRecordatorio.CssClass = "subOpcion";
                        btnTiposCorreo.CssClass = "subOpcion";

                        divCorreoConfTec.Visible = false;
                        divCorreoConfCorreo.Visible = false;
                        divCorreoCont.Visible = true;
                        divCorreoContSelec.Visible = false;
                        divCorreoRecordatorio.Visible = false;
                        divTiposCorreo.Visible = false;

                        btnActCorreoTecn.Visible = false;
                        btnActCorreoConf.Visible = false;
                        btnActCorreoCont.Visible = true;
                        btnPreview.Visible = true;
                        btnActCorreoContSelec.Visible = false;
                        btnActCorreoRecordatorio.Visible = false;
                        btnActTipo.Visible = false;
                    }
                    else
                    {
                        if (permisos.Contains("314"))
                        {
                            btnConfiguracionTecnica.CssClass = "subOpcion";
                            btnConfiguracionCorreo.CssClass = "subOpcion";
                            btnContenidCorreo.CssClass = "subOpcion";
                            btnContenidCorreoSelec.CssClass = "subOpcion subOpcionSeleccionado";
                            btnCorreoRecordatorio.CssClass = "subOpcion";
                            btnTiposCorreo.CssClass = "subOpcion";

                            divCorreoConfTec.Visible = false;
                            divCorreoConfCorreo.Visible = false;
                            divCorreoCont.Visible = false;
                            divCorreoContSelec.Visible = true;
                            divCorreoRecordatorio.Visible = false;
                            divTiposCorreo.Visible = false;

                            btnActCorreoTecn.Visible = false;
                            btnActCorreoConf.Visible = false;
                            btnActCorreoCont.Visible = true;
                            btnPreview.Visible = true;
                            btnActCorreoContSelec.Visible = true;
                            btnActCorreoRecordatorio.Visible = false;
                            btnActTipo.Visible = false;
                        }
                        else if (permisos.Contains("315"))
                        {
                            btnConfiguracionTecnica.CssClass = "subOpcion";
                            btnConfiguracionCorreo.CssClass = "subOpcion";
                            btnContenidCorreo.CssClass = "subOpcion";
                            btnContenidCorreoSelec.CssClass = "subOpcion";
                            btnCorreoRecordatorio.CssClass = "subOpcion subOpcionSeleccionado";
                            btnTiposCorreo.CssClass = "subOpcion";

                            divCorreoConfTec.Visible = false;
                            divCorreoConfCorreo.Visible = false;
                            divCorreoCont.Visible = false;
                            divCorreoContSelec.Visible = false;
                            divCorreoRecordatorio.Visible = true;
                            divTiposCorreo.Visible = false;

                            btnActCorreoTecn.Visible = false;
                            btnActCorreoConf.Visible = false;
                            btnActCorreoCont.Visible = true;
                            btnPreview.Visible = true;
                            btnActCorreoContSelec.Visible = false;
                            btnActCorreoRecordatorio.Visible = true;
                            btnActTipo.Visible = false;

                        }
                        else if (permisos.Contains("316"))
                        {
                            btnConfiguracionTecnica.CssClass = "subOpcion";
                            btnConfiguracionCorreo.CssClass = "subOpcion";
                            btnContenidCorreo.CssClass = "subOpcion";
                            btnContenidCorreoSelec.CssClass = "subOpcion";
                            btnCorreoRecordatorio.CssClass = "subOpcion";
                            btnTiposCorreo.CssClass = "subOpcion subOpcionSeleccionado";

                            divCorreoConfTec.Visible = false;
                            divCorreoConfCorreo.Visible = false;
                            divCorreoCont.Visible = false;
                            divCorreoContSelec.Visible = false;
                            divCorreoRecordatorio.Visible = false;
                            divTiposCorreo.Visible = true;

                            btnActCorreoTecn.Visible = false;
                            btnActCorreoConf.Visible = false;
                            btnActCorreoCont.Visible = true;
                            btnPreview.Visible = true;
                            btnActCorreoContSelec.Visible = false;
                            btnActCorreoRecordatorio.Visible = true;
                            btnActTipo.Visible = true;
                        }
                    }
                }
            }
        }


        protected void btnEnvioCorreo_Click(object sender, EventArgs e)
        {
            CambiaTitulo("Envio selectivo de correo");

            divGerentes.Visible = false;
            divPerfiles.Visible = false;
            divCorreo.Visible = false;
            divEnvioCorreo.Visible = true;
            divClientes.Visible = false;
            divConfigCitas.Visible = false;

            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "";
            btnEnvioCorreo.CssClass = "seleccionado";
            btnOpcionClientes.CssClass = "";
            btnConfigCitas.CssClass = "";

            //btnOpcionClientes.Visible = false;
            cargaClientes();
            ocultarOpcionesCorreo();
        }


        protected void gvGerentes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool editar = false;
            bool eliminar = false;

            if (Session["Permisos"].ToString().Contains("33"))
            {
                editar = true;
            }

            if (Session["Permisos"].ToString().Contains("34"))
            {
                eliminar = true;
            }
            
            if (!editar && !eliminar)
            {
                if (e.Row.RowType != DataControlRowType.Pager)
                {
                    e.Row.Cells[6].Visible = false;
                }
            }
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!editar)
                {
                    e.Row.Cells[6].Controls[0].Visible = false;
                }

                try
                {
                    if (!eliminar)
                    {
                        e.Row.Cells[6].Controls[2].Visible = false;
                    }
                }
                catch { }
            }
        }

        protected void gvGerentes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewRow row = (GridViewRow)gvGerentes.Rows[e.NewEditIndex];

            Label Usuario = (Label)row.FindControl("lblUsuario");
            Label Nombre = (Label)row.FindControl("lblNombre");
            Label ApellidoP = (Label)row.FindControl("lblApellidoP");
            Label ApellidoM = (Label)row.FindControl("lblApellidoM");
            Label Perfil = (Label)row.FindControl("lblPerfil");
            Label idUsuario = (Label)row.FindControl("lblIdUsuario");
            Label Doctor = (Label)row.FindControl("lblDoctor");

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;
            divFormularioG.Visible = true;
            divFormularioP.Visible = false;
            divFormCliente.Visible = false;
            divFormBusqCl.Visible = false;
            divFormTipo.Visible = false;
            divConfirmaDeleteTipo.Visible = false;

            ddlPerfil.DataSource = connMysql.TraerPerfilesddl("");
            ddlPerfil.DataBind();

            TxtUsuario.Text = Usuario.Text;
            TxtNombre.Text = Nombre.Text;
            TxtApellidoP.Text = ApellidoP.Text;
            TxtApellidoM.Text = ApellidoM.Text;
            lblId_usuario.Text = idUsuario.Text;
            ddlPerfil.SelectedValue = ddlPerfil.Items.FindByText(Perfil.Text).Value;

            String Pregunta = connMysql.TraerPreguntaS(idUsuario.Text, "", "");
            if (Pregunta != "")
            {
                ddlPreguntaS.SelectedValue = ddlPreguntaS.Items.FindByText(Pregunta).Value;
            }

            ddlDoctor.SelectedValue = Doctor.Text;

            FGAgregar.Visible = false;
            FGActualizar.Visible = true;
        }
        
        protected void gvGerentes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvGerentes.EditIndex = -1;
            cargaGerentes();
        }

        public void cargaGerentes()
        {
            String condicion = txtBusquedaG.Text.Trim().ToUpper();

            if (condicion.Length > 0)
            {
                condicion = " (nombre like '%" + condicion + "%' or apellido_paterno like '%" + condicion + "%' or apellido_materno like '%" + condicion + "%' or usuario like '%" + condicion + "%' or descripcion like '%" + condicion + "%')";
            }

            DataSet ds = connMysql.TraerUsuarios(condicion);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvGerentes.DataSource = ds.Tables[0];
                gvGerentes.DataBind();
            }
            else
            {
                gvGerentes.DataSource = ds.Tables[0];
                gvGerentes.DataBind();

                int totalColumnas = ds.Tables[0].Columns.Count;

                if (gvGerentes.Rows.Count == 0)
                {
                    DataTable dtTemporal = new DataTable();
                    dtTemporal.Columns.Add("ID_USUARIO");
                    dtTemporal.Columns.Add("Usuario");
                    dtTemporal.Columns.Add("Nombre");
                    dtTemporal.Columns.Add("Apellido_paterno");
                    dtTemporal.Columns.Add("Apellido_materno");
                    dtTemporal.Columns.Add("Perfil");
                    dtTemporal.NewRow();

                    DataRow drTemporal = dtTemporal.NewRow();
                    dtTemporal.Rows.InsertAt(drTemporal, 0);

                    gvGerentes.DataSource = dtTemporal;
                    gvGerentes.DataBind();
                }

                gvGerentes.Rows[0].Cells.Clear();
                gvGerentes.Rows[0].Cells.Add(new TableCell());
                gvGerentes.Rows[0].Cells[0].ColumnSpan = 6;
                gvGerentes.Rows[0].Cells[0].CssClass = "lblSinResultado";
                gvGerentes.Rows[0].Cells[0].Text = "Sin resultados";

                gvPerfiles.Visible = false;
                gvGerentes.Visible = true;
            }
        }
        
        protected void FGCancelar_Click(object sender, EventArgs e)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;

            divFormularioP.Visible = false;
            divFormularioG.Visible = false;
            divFormCliente.Visible = false;
            divFormBusqCl.Visible = false;
            divFormTipo.Visible = false;
            divConfirmaDeleteTipo.Visible = false;

            TxtUsuario.Text = "";
            TxtNombre.Text = "";
            TxtApellidoP.Text = "";
            TxtApellidoM.Text = "";
            ddlPerfil.ClearSelection();
            txtContrasenia.Text = "";
            ddlPreguntaS.SelectedIndex = 0;
            txtContrasenia.Text = "";
            txtRespuestaS.Text = "";
            txtCorreoE.Text = "";
            ddlDoctor.SelectedValue = "0";

            gvGerentes.EditIndex = -1;
            cargaGerentes();

        }

        protected void FGAgregar_Click(object sender, EventArgs e)
        {
            String resultado = "";
            String usuario = TxtUsuario.Text;
            String Nombre = TxtNombre.Text;
            String ApellidoP = TxtApellidoP.Text;
            String ApellidoM = TxtApellidoM.Text;
            String Perfil = ddlPerfil.SelectedValue;
            String contrasenia = txtContrasenia.Text;
            String correo = txtCorreoE.Text;
            String preguntaS = ddlPreguntaS.SelectedItem.Text;
            String respuestaS = txtRespuestaS.Text;
            String doctor = ddlDoctor.SelectedValue;

            resultado = connMysql.GuardaUsuario(Nombre.ToUpper(), ApellidoP.ToUpper(), ApellidoM.ToUpper(), usuario.ToUpper(), Perfil, contrasenia, correo, preguntaS, respuestaS, doctor);

            TxtUsuario.Text = "";
            TxtNombre.Text = "";
            TxtApellidoP.Text = "";
            TxtApellidoM.Text = "";
            ddlPerfil.SelectedIndex = 0;
            txtContrasenia.Text = "";
            ddlPreguntaS.SelectedIndex = 0;
            txtContrasenia.Text = "";
            txtRespuestaS.Text = "";
            txtCorreoE.Text = "";
            ddlDoctor.SelectedValue = "0";

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Usuario guardado exitosamente" : resultado));
            
            gvGerentes.EditIndex = -1;
            cargaGerentes();
        }
        
        public void mostrarMensaje(String mensaje)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);

            sombraMensaje.Visible = true;
            divFormularioG.Visible = false;
            divFormularioP.Visible = false;
            divFormCliente.Visible = false;
            divFormBusqCl.Visible = false;
            divFormTipo.Visible = false;
            divConfirmaDeleteTipo.Visible = false;

            divMensaje.Visible = true;
            lblMensaje.Text = mensaje;
        }

        protected void MOk_Click(object sender, EventArgs e)
        {
            divMensaje.Visible = false;
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;
        }

        protected void btnAgrUsuarioG_Click(object sender, EventArgs e)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;

            divFormularioG.Visible = true;
            divFormularioP.Visible = false;
            divFormCliente.Visible = false;
            divFormBusqCl.Visible = false;
            divFormTipo.Visible = false;
            divConfirmaDeleteTipo.Visible = false;

            FGActualizar.Visible = false;
            FGAgregar.Visible = true;

            ddlPerfil.DataSource = connMysql.TraerPerfiles("");
            ddlPerfil.DataBind();
        }

        protected void txtBusquedaG_TextChanged(object sender, EventArgs e)
        {
            String condicion = txtBusquedaG.Text.Trim().ToUpper();

            if (condicion.Length > 0)
            {
                condicion = " (nombre like '%" + condicion + "%' or apellido_paterno like '%" + condicion + "%' or apellido_materno like '%" + condicion + "%' or usuario like '%" + condicion + "%' or descripcion like '%" + condicion + "%')";
            }

            DataSet ds = connMysql.TraerUsuarios(condicion);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvGerentes.DataSource = ds.Tables[0];
                gvGerentes.DataBind();
            }
            else
            {
                gvGerentes.DataSource = ds.Tables[0];
                gvGerentes.DataBind();

                int totalColumnas = ds.Tables[0].Columns.Count;

                if (gvGerentes.Rows.Count == 0)
                {
                    DataTable dtTemporal = new DataTable();
                    dtTemporal.Columns.Add("ID_USUARIO");
                    dtTemporal.Columns.Add("Usuario");
                    dtTemporal.Columns.Add("Nombre");
                    dtTemporal.Columns.Add("Apellido_paterno");
                    dtTemporal.Columns.Add("Apellido_materno");
                    dtTemporal.Columns.Add("Perfil");
                    dtTemporal.NewRow();

                    DataRow drTemporal = dtTemporal.NewRow();
                    dtTemporal.Rows.InsertAt(drTemporal, 0);

                    gvGerentes.DataSource = dtTemporal;
                    gvGerentes.DataBind();
                }

                gvGerentes.Rows[0].Cells.Clear();
                gvGerentes.Rows[0].Cells.Add(new TableCell());
                gvGerentes.Rows[0].Cells[0].ColumnSpan = 6;
                gvGerentes.Rows[0].Cells[0].CssClass = "lblSinResultado";
                gvGerentes.Rows[0].Cells[0].Text = "Sin resultados";
                
                gvGerentes.Visible = true;
            }
        }

        protected void gvGerentes_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
        }

        protected void gvGerentes_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
        }

        protected void gvGerentes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)gvGerentes.Rows[e.RowIndex];
            Label Id_Usuario = (Label)row.FindControl("lblIdUsuario");
            String resultado = connMysql.EliminaUsuario(Id_Usuario.Text);

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Usuario eliminado exitosamente" : resultado));

            cargaGerentes();
        }

        protected void FGActualizar_Click(object sender, EventArgs e)
        {
            bool CambioContrasenia = !txtContrasenia.Text.Trim().Equals("");
            String usuario = TxtUsuario.Text;
            String Nombre = TxtNombre.Text;
            String ApellidoP = TxtApellidoP.Text;
            String ApellidoM = TxtApellidoM.Text;
            String Perfil = ddlPerfil.SelectedValue;
            String contrasenia = txtContrasenia.Text;
            String idUsuario = lblId_usuario.Text;
            String correo = txtCorreoE.Text;
            String preguntaS = ddlPreguntaS.SelectedItem.Text;
            String respuestaS = txtRespuestaS.Text;
            String Doctor = ddlDoctor.SelectedValue;

            bool CambioRespuesta = !txtRespuestaS.Text.Trim().Equals("");

            String resultado = connMysql.ActualizaUsuario(idUsuario, Nombre.ToUpper(), ApellidoP.ToUpper(), ApellidoM.ToUpper(), usuario.ToUpper(), Perfil, contrasenia, CambioContrasenia, CambioRespuesta, correo, preguntaS, respuestaS, Doctor);

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Usuario actualizado exitosamente" : resultado));

            TxtUsuario.Text = "";
            TxtNombre.Text = "";
            TxtApellidoP.Text = "";
            TxtApellidoM.Text = "";
            ddlPerfil.ClearSelection();
            txtContrasenia.Text = "";
            lblId_usuario.Text = "";
            
            ddlPreguntaS.SelectedIndex = 0;
            txtContrasenia.Text = "";
            txtRespuestaS.Text = "";
            txtCorreoE.Text = "";
            ddlDoctor.SelectedValue = "0";

            gvGerentes.EditIndex = -1;
            cargaGerentes();
        }
        
    #region perfiles
        public void cargaPerfiles()
        {
            String condicion = txtBusquedaP.Text.Trim().ToUpper();

            if (condicion.Length > 0)
            {
                condicion = " (Descripcion like '%" + condicion + "%' )";
            }

            DataSet ds = connMysql.TraerPerfiles(condicion);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPerfiles.DataSource = ds.Tables[0];
                gvPerfiles.DataBind();
            }
            else
            {
                gvPerfiles.DataSource = ds.Tables[0];
                gvPerfiles.DataBind();

                int totalColumnas = ds.Tables[0].Columns.Count;

                if (gvPerfiles.Rows.Count == 0)
                {
                    DataTable dtTemporal = new DataTable();
                    dtTemporal.Columns.Add("ID_USUARIO");
                    dtTemporal.Columns.Add("Usuario");
                    dtTemporal.Columns.Add("Nombre");
                    dtTemporal.Columns.Add("Apellido_paterno");
                    dtTemporal.Columns.Add("Apellido_materno");
                    dtTemporal.Columns.Add("Perfil");
                    dtTemporal.NewRow();

                    DataRow drTemporal = dtTemporal.NewRow();
                    dtTemporal.Rows.InsertAt(drTemporal, 0);

                    gvPerfiles.DataSource = dtTemporal;
                    gvPerfiles.DataBind();
                }

                gvPerfiles.Rows[0].Cells.Clear();
                gvPerfiles.Rows[0].Cells.Add(new TableCell());
                gvPerfiles.Rows[0].Cells[0].ColumnSpan = 6;
                gvPerfiles.Rows[0].Cells[0].CssClass = "lblSinResultado";
                gvPerfiles.Rows[0].Cells[0].Text = "Sin resultados";
                
                gvGerentes.Visible = false;
                gvPerfiles.Visible = true;
            }
        }
        
        protected void txtBusquedaP_TextChanged(object sender, EventArgs e)
        {
            String condicion = txtBusquedaP.Text.Trim().ToUpper();

            if (condicion.Length > 0)
            {
                condicion = " (descripcion like '%" + condicion + "%')";
            }

            DataSet ds = connMysql.TraerPerfiles(condicion);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPerfiles.DataSource = ds.Tables[0];
                gvPerfiles.DataBind();
            }
            else
            {
                gvPerfiles.DataSource = ds.Tables[0];
                gvPerfiles.DataBind();

                int totalColumnas = ds.Tables[0].Columns.Count;

                if (gvPerfiles.Rows.Count == 0)
                {
                    DataTable dtTemporal = new DataTable();
                    dtTemporal.Columns.Add("ID_PERFIL");
                    dtTemporal.Columns.Add("Descripcion");
                    dtTemporal.Columns.Add("Permisos");
                    dtTemporal.NewRow();

                    DataRow drTemporal = dtTemporal.NewRow();
                    dtTemporal.Rows.InsertAt(drTemporal, 0);

                    gvPerfiles.DataSource = dtTemporal;
                    gvPerfiles.DataBind();
                }

                gvPerfiles.Rows[0].Cells.Clear();
                gvPerfiles.Rows[0].Cells.Add(new TableCell());
                gvPerfiles.Rows[0].Cells[0].ColumnSpan = 3;
                gvPerfiles.Rows[0].Cells[0].CssClass = "lblSinResultado";
                gvPerfiles.Rows[0].Cells[0].Text = "Sin resultados";
                
                gvPerfiles.Visible = true;
            }
        }

        protected void btnAgregarP_Click(object sender, EventArgs e)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;

            divFormularioG.Visible = false;
            divFormularioP.Visible = true;
            divFormCliente.Visible = false;
            divFormBusqCl.Visible = false;
            divFormTipo.Visible = false;
            divConfirmaDeleteTipo.Visible = false;

            FPActualizar.Visible = false;
            FPAgregar.Visible = true;
        }

        protected void gvPerfiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool editar = false;
            bool eliminar = false;

            if (Session["Permisos"].ToString().Contains("37"))
            {
                editar = true;
            }

            if (Session["Permisos"].ToString().Contains("38"))
            {
                eliminar = true;
            }
            
            if (!editar && !eliminar)
            {
                if (e.Row.RowType != DataControlRowType.Pager)
                {
                    e.Row.Cells[3].Visible = false;
                }
            }
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!editar)
                {
                    e.Row.Cells[3].Controls[0].Visible = false;
                }

                try
                {
                    if (!eliminar)
                    {
                        e.Row.Cells[3].Controls[2].Visible = false;
                    }
                }
                catch { }
            }
        }

        protected void gvPerfiles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewRow row = (GridViewRow)gvPerfiles.Rows[e.NewEditIndex];

            Label IdPerfil = (Label)row.FindControl("lblIdPerfil");
            Label Descripcion = (Label)row.FindControl("lblDescripcion");
            Label Permisos = (Label)row.FindControl("lblPermisos");

            String[] PermisoA = Permisos.Text.Split(',');
            foreach (String Permiso in PermisoA)
            {
                switch (Permiso.Trim())
                {
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                        foreach (ListItem lsItem in CHKLCliente.Items)
                        {
                            if (lsItem.Value.Equals(Permiso.Trim()))
                            {
                                lsItem.Selected = true;
                            }
                        }
                        break;

                    case "21":
                        chkClienteMes.Checked = true;
                        break;

                    case "31":
                    case "32":
                    case "33":
                    case "34":
                        foreach (ListItem lsItem in chkPerforUsuario.Items)
                        {
                            if (lsItem.Value.Equals(Permiso.Trim()))
                            {
                                lsItem.Selected = true;
                            }
                        }
                        break;

                    case "35":
                    case "36":
                    case "37":
                    case "38":
                        foreach (ListItem lsItem in chkPerforPerfiles.Items)
                        {
                            if (lsItem.Value.Equals(Permiso.Trim()))
                            {
                                lsItem.Selected = true;
                            }
                        }
                        break;

                    case "39":
                    case "310":
                    case "311":
                    case "314":
                        foreach (ListItem lsItem in chkPerforCorreo.Items)
                        {
                            if (lsItem.Value.Equals(Permiso.Trim()))
                            {
                                lsItem.Selected = true;
                            }
                        }
                        break;
                    case "312":
                    case "313":
                        foreach (ListItem lsItem in chkAdmin.Items)
                        {
                            if (lsItem.Value.Equals(Permiso.Trim()))
                            {
                                lsItem.Selected = true;
                            }
                        }
                        break; 
                }
            }
            
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;
            divFormularioG.Visible = false;
            divFormularioP.Visible = true;
            divFormCliente.Visible = false;
            divFormBusqCl.Visible = false;
            divFormTipo.Visible = false;
            divConfirmaDeleteTipo.Visible = false;

            TxtPerfil.Text = Descripcion.Text;
            lblId_perfil.Text = IdPerfil.Text;

            FPAgregar.Visible = false;
            FPActualizar.Visible = true;
        }

        protected void gvPerfiles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPerfiles.EditIndex = -1;
            cargaPerfiles();
        }

        protected void gvPerfiles_RowUpdated(object sender, GridViewUpdatedEventArgs e) { }

        protected void gvPerfiles_RowDeleted(object sender, GridViewDeletedEventArgs e) { }

        protected void gvPerfiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)gvPerfiles.Rows[e.RowIndex];
            Label Id_Perfil = (Label)row.FindControl("lblIdPerfil");
            String resultado = connMysql.EliminaPerfil(Id_Perfil.Text);

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Perfil eliminado exitosamente" : resultado));

            cargaPerfiles();
        }
    #endregion

        protected void FPCancelar_Click(object sender, EventArgs e)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;

            divFormularioP.Visible = false;
            divFormularioG.Visible = false;
            divFormCliente.Visible = false;
            divFormBusqCl.Visible = false;
            divFormTipo.Visible = false;
            divConfirmaDeleteTipo.Visible = false;

            TxtPerfil.Text = "";
            lblId_perfil.Text = "";

            chkClienteMes.Checked = false;
            foreach (ListItem item in CHKLCliente.Items)
            {
                item.Selected = false;
            }

            gvPerfiles.EditIndex = -1;
            cargaPerfiles();
        }

        protected void FPAgregar_Click(object sender, EventArgs e)
        {
            String resultado = "";

            String Descripcion = TxtPerfil.Text.ToUpper();
            String Permisos = "";
            
            if (chkClienteMes.Checked)
            {
                if (Permisos.Length == 0)
                {
                    Permisos = "21";
                }
                else
                {
                    Permisos += ",21";
                }
            }

            foreach (ListItem item in CHKLCliente.Items)
            {
                if (item.Selected)
                {
                    if (Permisos.Length == 0)
                    {
                        Permisos = item.Value;
                    }
                    else
                    {
                        Permisos += "," + item.Value;
                    }
                }
            }

            foreach (ListItem item in chkPerforUsuario.Items)
            {
                if (item.Selected)
                {
                    if (Permisos.Length == 0)
                    {
                        Permisos = item.Value;
                    }
                    else
                    {
                        Permisos += "," + item.Value;
                    }
                }
            }

            foreach (ListItem item in chkPerforPerfiles.Items)
            {
                if (item.Selected)
                {
                    if (Permisos.Length == 0)
                    {
                        Permisos = item.Value;
                    }
                    else
                    {
                        Permisos += "," + item.Value;
                    }
                }
            }


            foreach (ListItem item in chkPerforCorreo.Items)
            {
                if (item.Selected)
                {
                    if (Permisos.Length == 0)
                    {
                        Permisos = item.Value;
                    }
                    else
                    {
                        Permisos += "," + item.Value;
                    }
                }
            }

            foreach (ListItem item in chkAdmin.Items)
            {
                if (item.Selected)
                {
                    if (Permisos.Length == 0)
                    {
                        Permisos = item.Value;
                    }
                    else
                    {
                        Permisos += "," + item.Value;
                    }
                }
            }


            resultado = connMysql.Guardaperfiles(Descripcion.ToUpper(), Permisos);

            TxtPerfil.Text = "";

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Perfil guardado exitosamente" : resultado));
            
            chkClienteMes.Checked = false;
            TxtPerfil.Text = "";

            foreach (ListItem item in CHKLCliente.Items)
            {
                item.Selected = false;
            }

            gvPerfiles.EditIndex = -1;
            cargaPerfiles();
        }

        protected void FPActualizar_Click(object sender, EventArgs e)
        {
            bool CambioContrasenia = !txtContrasenia.Text.Trim().Equals("");
            String Descripcion = TxtPerfil.Text;
            String idPerfil = lblId_perfil.Text;
            String Permisos = "";
            
            if (chkClienteMes.Checked)
            {
                if (Permisos.Length == 0)
                {
                    Permisos = "21";
                }
                else
                {
                    Permisos += ",21";
                }
            }

            foreach (ListItem item in CHKLCliente.Items)
            {
                if (item.Selected)
                {
                    if (Permisos.Length == 0)
                    {
                        Permisos = item.Value;
                    }
                    else
                    {
                        Permisos += "," + item.Value;
                    }
                }
            }

            foreach (ListItem item in chkPerforUsuario.Items)
            {
                if (item.Selected)
                {
                    if (Permisos.Length == 0)
                    {
                        Permisos = item.Value;
                    }
                    else
                    {
                        Permisos += "," + item.Value;
                    }
                }
            }

            foreach (ListItem item in chkPerforPerfiles.Items)
            {
                if (item.Selected)
                {
                    if (Permisos.Length == 0)
                    {
                        Permisos = item.Value;
                    }
                    else
                    {
                        Permisos += "," + item.Value;
                    }
                }
            }


            foreach (ListItem item in chkPerforCorreo.Items)
            {
                if (item.Selected)
                {
                    if (Permisos.Length == 0)
                    {
                        Permisos = item.Value;
                    }
                    else
                    {
                        Permisos += "," + item.Value;
                    }
                }
            }

            foreach (ListItem item in chkAdmin.Items)
            {
                if (item.Selected)
                {
                    if (Permisos.Length == 0)
                    {
                        Permisos = item.Value;
                    }
                    else
                    {
                        Permisos += "," + item.Value;
                    }
                }
            }

            String resultado = connMysql.ActualizaPerfil(idPerfil, Descripcion.ToUpper(), Permisos);

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Perfil actualizado exitosamente" : resultado));

            TxtPerfil.Text = "";

            chkClienteMes.Checked = false;
            foreach (ListItem item in CHKLCliente.Items)
            {
                item.Selected = false;
            }
            
            gvPerfiles.EditIndex = -1;
            cargaPerfiles();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Principal.aspx");
        }

        protected void gvPerfiles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
        }

        protected void cargaCorreo()
        {
            DatosCorreo datosCorreo = connMysql.ConsultaDatosCorreo();

            txtCorreo.Text = datosCorreo.SMTP_CORREO;
            txtCorreoContraseña.Text = datosCorreo.SMTP_PASS;
            txtMensaje.Text = datosCorreo.SMTP_MENSAJE;
            txtSujeto.Text = datosCorreo.SMTP_SUJETO;
            txtSSL.Text = datosCorreo.SMTP_SSL;
            txtHost.Text = datosCorreo.SMTP_HOST;
            txtPuerto.Text = datosCorreo.SMTP_PUERTO;
            txtUltimoC.Text = datosCorreo.UltimoEnvio;
            txtDiasAntes.Text = datosCorreo.DiasAntes;
            if (datosCorreo.PRUEBAS.Equals("1"))
            {
                chkPruebas.Checked = true;
            }
            
            txtCorreoPrueba.Text = datosCorreo.SMTP_CORREO_PRUEBA;
            ddlEnvCorreo.SelectedValue = datosCorreo.ENV_ESTADO;
            txtFirma.Text = datosCorreo.SMTP_FIRMA;
        }

        protected void btnActualizarCorreo_Click(object sender, EventArgs e)
        {
            if ((uploadImgCorreo.PostedFile.ContentLength / 1024) < 1024) {

                lblErrorImage.Text = "";
                DatosCorreo datoscorreo = new DatosCorreo();

                datoscorreo.SMTP_CORREO = txtCorreo.Text;
                datoscorreo.SMTP_PASS = txtCorreoContraseña.Text;
                datoscorreo.SMTP_MENSAJE = txtMensaje.Text;
                datoscorreo.SMTP_SUJETO = txtSujeto.Text;
                datoscorreo.SMTP_SSL = txtSSL.Text;
                datoscorreo.SMTP_HOST = txtHost.Text;
                datoscorreo.SMTP_PUERTO = txtPuerto.Text;
                datoscorreo.UltimoEnvio = txtUltimoC.Text;
                datoscorreo.DiasAntes = txtDiasAntes.Text;
                datoscorreo.SMTP_CORREO_PRUEBA = txtCorreoPrueba.Text;

                datoscorreo.PRUEBAS = (chkPruebas.Checked ? "1" : "0");
                datoscorreo.ENV_ESTADO = ddlEnvCorreo.SelectedValue;    // cambie en Clase DatosCorreo.cs
                datoscorreo.SMTP_FIRMA = txtFirma.Text;

                datoscorreo.SMTP_IMAGEN = "";
                if (uploadImgCorreo.FileName.Trim().Length != 0)
                {
                    if (uploadImgCorreo.HasFile)
                    {
                        string nombreArchivo = uploadImgCorreo.FileName;
                        string ruta = "Imagenes/Correo/" + nombreArchivo;
                        uploadImgCorreo.SaveAs(Server.MapPath(ruta));

                        datoscorreo.SMTP_IMAGEN = ruta;
                    }
                }

                String resultado = connMysql.ActualizaDatosCorreo(datoscorreo);
                
                sombraMensaje.Visible = true;
                mostrarMensaje((resultado.Trim().Equals("OK") ? "Datos de correo actualizado exitosamente" : resultado));
            }else
            {
                lblErrorImage.Text = "* El tamaño de la imagen debe ser menor a 1 MB";
            }
        }

        protected void btnOkSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnConfiguracionTecnica_Click(object sender, EventArgs e)
        {
            btnActCorreoConf.Visible = false;
            btnActCorreoCont.Visible = false;
            btnActCorreoTecn.Visible = true;
            btnActCorreoContSelec.Visible = false;
            btnActTipo.Visible = false;

            btnConfiguracionTecnica.CssClass = "subOpcion subOpcionSeleccionado";
            btnConfiguracionCorreo.CssClass = "subOpcion";
            btnContenidCorreo.CssClass = "subOpcion";
            btnContenidCorreoSelec.CssClass = "subOpcion";
            btnCorreoRecordatorio.CssClass = "subOpcion";
            btnTiposCorreo.CssClass = "subOpcion";

            divConfigCitas.Visible = false;
            btnConfigCitas.CssClass = "";

            divCorreoConfTec.Visible = true;
            divCorreoCont.Visible = false;
            divCorreoConfCorreo.Visible = false;
            divCorreoContSelec.Visible = false;
            divCorreoRecordatorio.Visible = false;

            btnPreview.Visible = false;
            btnActCorreoRecordatorio.Visible = false;

            divTiposCorreo.Visible = false;
            txtBusqTipo.Visible = false;
            imgBusqTipo.Visible = false;

            CambiaTitulo("Configuración Técnica");
        }

        protected void btnConfiguracionCorreo_Click(object sender, EventArgs e)
        {
            btnActCorreoConf.Visible = true;
            btnActCorreoCont.Visible = false;
            btnActCorreoTecn.Visible = false;
            btnActCorreoContSelec.Visible = false;
            btnActTipo.Visible = false;

            btnConfiguracionTecnica.CssClass = "subOpcion";
            btnConfiguracionCorreo.CssClass = "subOpcion subOpcionSeleccionado";
            btnContenidCorreo.CssClass = "subOpcion";
            btnContenidCorreoSelec.CssClass = "subOpcion";
            btnCorreoRecordatorio.CssClass = "subOpcion";
            btnTiposCorreo.CssClass = "subOpcion";

            divConfigCitas.Visible = false;
            divCorreoRecordatorio.Visible = false;
            btnConfigCitas.CssClass = "";

            divCorreoConfTec.Visible = false;
            divCorreoCont.Visible = false;
            divCorreoConfCorreo.Visible = true;
            divCorreoContSelec.Visible = false;
            divTiposCorreo.Visible = false;

            btnActCorreoRecordatorio.Visible = false;
            btnPreview.Visible = false;

            txtBusqTipo.Visible = false;
            imgBusqTipo.Visible = false;

            CambiaTitulo("Configuración de correo");
        }

        protected void btnContenidCorreo_Click(object sender, EventArgs e)
        {
            btnActCorreoConf.Visible = false;
            btnActCorreoCont.Visible = true;
            btnActCorreoTecn.Visible = false;
            btnActCorreoContSelec.Visible = false;
            btnActTipo.Visible = false;

            btnConfiguracionTecnica.CssClass = "subOpcion";
            btnConfiguracionCorreo.CssClass = "subOpcion";
            btnContenidCorreoSelec.CssClass = "subOpcion";
            btnContenidCorreo.CssClass = "subOpcion subOpcionSeleccionado";
            btnCorreoRecordatorio.CssClass = "subOpcion";
            btnTiposCorreo.CssClass = "subOpcion";

            divConfigCitas.Visible = false;
            btnConfigCitas.CssClass = "";

            divCorreoConfTec.Visible = false;
            divCorreoCont.Visible = true;
            divCorreoContSelec.Visible = false;
            divCorreoConfCorreo.Visible = false;
            btnPreview.Visible = true;


            divTiposCorreo.Visible = false;
            txtBusqTipo.Visible = false;
            imgBusqTipo.Visible = false;

            divCorreoRecordatorio.Visible = false;
            btnActCorreoRecordatorio.Visible = false;
            CambiaTitulo("Contenido de correo mensual");
            Session.Add("TipoPreview", "mensual");

        }

        public void ocultarOpcionesCorreo()
        {
            btnConfiguracionTecnica.Visible = false;
            btnConfiguracionCorreo.Visible = false;
            btnContenidCorreo.Visible = false;
            btnContenidCorreoSelec.Visible = false;

            btnConfiguracionTecnica.CssClass = "subOpcion";
            btnConfiguracionCorreo.CssClass = "subOpcion";
            btnContenidCorreo.CssClass = "subOpcion";
            btnContenidCorreoSelec.CssClass = "subOpcion";

            divCorreoConfTec.Visible = false;
            divCorreoCont.Visible = false;
            divCorreoConfCorreo.Visible = true;
            divCorreoContSelec.Visible = false;
        }

        protected void btnActCorreoTecn_Click(object sender, EventArgs e)
        {
            DatosCorreo datoscorreo = new DatosCorreo();
            datoscorreo.SMTP_HOST = txtHost.Text;
            datoscorreo.SMTP_PUERTO = txtPuerto.Text;
            datoscorreo.SMTP_SSL = txtSSL.Text;
            datoscorreo.SMTP_CORREO = txtCorreo.Text;
            datoscorreo.SMTP_PASS = txtCorreoContraseña.Text;

            String resultado = connMysql.ActualizaTecCorreo(datoscorreo);

            if (resultado == "OK")
            {
                sombraMensaje.Visible = true;
                mostrarMensaje((resultado.Trim().Equals("OK") ? "Datos de correo actualizado exitosamente" : resultado));
            }
        }

        protected void btnActCorreoConf_Click(object sender, EventArgs e)
        {
            DatosCorreo datoscorreo = new DatosCorreo();
            
            datoscorreo.DiasAntes = txtDiasAntes.Text;
            datoscorreo.SMTP_CORREO_PRUEBA = txtCorreoPrueba.Text;
            datoscorreo.PRUEBAS = (chkPruebas.Checked ? "1" : "0");
            datoscorreo.ENV_ESTADO = ddlEnvCorreo.SelectedValue;    
            datoscorreo.UltimoEnvio = txtUltimoC.Text;

            String resultado = connMysql.ActualizaConfCorreo(datoscorreo);

            if (resultado == "OK")
            {
                sombraMensaje.Visible = true;
                mostrarMensaje((resultado.Trim().Equals("OK") ? "Datos de correo actualizado exitosamente" : resultado));
            }
        }

        protected void btnActCorreoCont_Click(object sender, EventArgs e)
        {
            DatosCorreo datoscorreo = new DatosCorreo();

            if ((uploadImgCorreo.PostedFile.ContentLength / 1024) < 1024)
            {
                lblErrorImage.Text = "";

                datoscorreo.SMTP_SUJETO = txtSujeto.Text;
                datoscorreo.SMTP_MENSAJE = txtMensaje.Text;
                datoscorreo.SMTP_FIRMA = txtFirma.Text;
                datoscorreo.SMTP_IMAGEN = "";

                if (uploadImgCorreo.FileName.Trim().Length != 0)
                {
                    if (uploadImgCorreo.HasFile)
                    {
                        string nombreArchivo = uploadImgCorreo.FileName;
                        string ruta = "Imagenes/Correo/" + nombreArchivo;
                        uploadImgCorreo.SaveAs(Server.MapPath(ruta));

                        datoscorreo.SMTP_IMAGEN = ruta;
                    }
                }
                String resultado = connMysql.ActualizaContCorreo(datoscorreo);

                if (resultado == "OK")
                {
                    sombraMensaje.Visible = true;
                    mostrarMensaje((resultado.Trim().Equals("OK") ? "Datos de correo actualizado exitosamente" : resultado));
                }
            }
            else
            {
                lblErrorImage.Text = "* El tamaño de la imagen debe ser menor a 1 MB";
            }
        }

        protected void btnActCorreoContSelec_Click(object sender, EventArgs e)
        {
            DatosCorreo datoscorreo = new DatosCorreo();

            if ((uploadImgSelec.PostedFile.ContentLength / 1024) < 1024)
            {
                lblErrorImage.Text = "";

                datoscorreo.SMTP_SUJETO = txtAsuntoSelec.Text;
                datoscorreo.SMTP_MENSAJE = txtMsjSelec.Text;
                datoscorreo.SMTP_FIRMA = txtFirmaSelec.Text;
                datoscorreo.SMTP_IMAGEN = "";

                if (uploadImgSelec.FileName.Trim().Length != 0)
                {
                    if (uploadImgSelec.HasFile)
                    {
                        string nombreArchivo = uploadImgSelec.FileName;
                        string ruta = "Imagenes/Correo/" + nombreArchivo;
                        uploadImgSelec.SaveAs(Server.MapPath(ruta));

                        datoscorreo.SMTP_IMAGEN = ruta;
                    }
                }
                String resultado = connMysql.ActualizaContCorreoSelec(datoscorreo);

                if (resultado == "OK")
                {
                    sombraMensaje.Visible = true;
                    mostrarMensaje((resultado.Trim().Equals("OK") ? "Datos de correo selectivo actualizado exitosamente" : resultado));
                }
            }
            else
            {
                lblErrorImage.Text = "* El tamaño de la imagen debe ser menor a 1 MB";
            }
        }

        public bool RevisarSeleccion()
        {
            CheckBox chk;
            bool seleccion = false;
            foreach (GridViewRow gvRow in gvClientesCorreo.Rows)
            {
                chk = (CheckBox)gvRow.FindControl("chkSeleccionado");
                if (chk.Checked)
                {
                    seleccion = true;
                }
            }
            return seleccion;
        }
        public void LimpiaSeleccion()
        {
            List<ClienteCorreo> lsEnvCorreo = (List<ClienteCorreo>)Session["EnvioClientes"];


            CheckBox chk;
            foreach (GridViewRow gvRow in gvClientesCorreo.Rows)
            {
                chk = (CheckBox)gvRow.FindControl("chkSeleccionado");

                String id_cliente = chk.Attributes["ID_CLIENTE"].ToString();
                DataSet DatosCliente = connMysql.TraerClienteCorreo(id_cliente);

                ClienteCorreo datosClienteCorreo = new ClienteCorreo() { ID_CLIENTE = int.Parse(DatosCliente.Tables[0].Rows[0].ItemArray[0].ToString()), Nombre = DatosCliente.Tables[0].Rows[0].ItemArray[1].ToString(), Apellido_paterno = DatosCliente.Tables[0].Rows[0].ItemArray[2].ToString(), Apellido_materno = DatosCliente.Tables[0].Rows[0].ItemArray[3].ToString(), EMAIL = DatosCliente.Tables[0].Rows[0].ItemArray[12].ToString() };
                if (chk.Checked)
                {
                    chk.Checked = false;
                    if (lsEnvCorreo.Exists(x => x.ID_CLIENTE == datosClienteCorreo.ID_CLIENTE))
                    {
                        lsEnvCorreo.RemoveAll(x => x.ID_CLIENTE == datosClienteCorreo.ID_CLIENTE);
                    }
                }
            }
            btnLimipiaS.Visible = false;
        }

        protected void btnLimpiaS_Click(object sender, EventArgs e)
        {
            LimpiaSeleccion();
        }
        #region EnvioCorreo
        protected void chkECSelectTodo_CheckedChanged(object sender, EventArgs e)
        {
            List<ClienteCorreo> lsEnvCorreo = (List<ClienteCorreo>)Session["EnvioClientes"];

            foreach (DataRow dr in (connMysql.TraerClientes(Session["CondicionClientes"].ToString(), "1,0")).Tables[0].Rows)
            {
                ClienteCorreo temporal = new ClienteCorreo() { ID_CLIENTE = int.Parse(dr["ID_CLIENTE"].ToString()), Nombre = dr["Nombre"].ToString(), Apellido_paterno = dr["Apellido_paterno"].ToString(), Apellido_materno = dr["Apellido_materno"].ToString(), EMAIL = dr["EMAIL"].ToString() };

                if (chkECSelectTodo.Checked)
                {
                    if(temporal.EMAIL != "") { 
                        if (!lsEnvCorreo.Exists(x => x.ID_CLIENTE == temporal.ID_CLIENTE))
                        {
                            lsEnvCorreo.Add(temporal);
                        }
                    }
                }
                else
                {
                    if (lsEnvCorreo.Exists(x => x.ID_CLIENTE == temporal.ID_CLIENTE))
                    {
                        lsEnvCorreo.RemoveAll(x => x.ID_CLIENTE == temporal.ID_CLIENTE);
                    }
                }
            }
            
            Session["EnvioClientes"] = lsEnvCorreo;

            cargaClientes();
        }

        
        protected void gvClientesCorreo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            List<ClienteCorreo> lsEnvCorreo = (List<ClienteCorreo>)Session["EnvioClientes"];

            if (SesionViva())
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        try
                        {
                            LinkButton lbSort = (LinkButton)cell.Controls[0];
                            DataTable dttempora = (DataTable)gvClientesCorreo.DataSource;
                            if (dttempora.DefaultView.Sort == "")
                            {
                            }
                            else
                            {
                                if (lbSort.CommandArgument == dttempora.DefaultView.Sort.Substring(0, dttempora.DefaultView.Sort.IndexOf(" ")))
                                {
                                    lbSort.CssClass = "Seleccionada";
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkSeleccionado = (CheckBox)e.Row.FindControl("chkSeleccionado");

                    ClienteCorreo temporal = new ClienteCorreo() {
                        ID_CLIENTE = int.Parse(((DataRowView)e.Row.DataItem).Row.ItemArray[0].ToString()),
                        Nombre = ((DataRowView)e.Row.DataItem).Row.ItemArray[1].ToString(),
                        Apellido_paterno = ((DataRowView)e.Row.DataItem).Row.ItemArray[2].ToString(),
                        Apellido_materno = ((DataRowView)e.Row.DataItem).Row.ItemArray[3].ToString(),
                        EMAIL = ((DataRowView)e.Row.DataItem).Row.ItemArray[12].ToString() };
                    
                    if (chkSeleccionado.Attributes.Count == 0) {
                        chkSeleccionado.Attributes.Add("ID_CLIENTE", ((DataRowView)e.Row.DataItem).Row.ItemArray[0].ToString());
                    }

                    if (lsEnvCorreo.Exists(x => x.ID_CLIENTE == temporal.ID_CLIENTE)) {
                        chkSeleccionado.Checked = true;
                    }

                    if(temporal.EMAIL == "")
                    {
                        chkSeleccionado.Visible = false;
                        chkSeleccionado.Checked = false;
                    }

                    Label lblNumeroFila = (Label)e.Row.FindControl("lblindice");

                    try
                    {
                        lblNumeroFila.Text = ((e.Row.RowIndex + 1) + (gvClientesCorreo.PageIndex * gvClientesCorreo.PageSize)).ToString();
                    }
                    catch { }

                    Label etiquetaFechaI = (Label)e.Row.FindControl("lblFechaI");
                    Label etiquetaFechaN = (Label)e.Row.FindControl("lblFechaN");

                    try
                    {
                        etiquetaFechaN.Text = ((DateTime)((DataRowView)e.Row.DataItem).Row.ItemArray[5]).ToString("dd/MM/yyyy");
                    }
                    catch { }

                    try
                    {
                        etiquetaFechaI.Text = ((DateTime)((DataRowView)e.Row.DataItem).Row.ItemArray[6]).ToString("dd/MM/yyyy");
                    }
                    catch { }
                }
            }   
        }


        protected void gvClientesCorreo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClientesCorreo.PageIndex = e.NewPageIndex;
            cargaClientes();
        }

        protected void gvClientesCorreo_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SesionViva())
            {
                if (Session["OrdenClientes"].ToString() == e.SortExpression + " " + "ASC")
                {
                    Session["OrdenClientes"] = e.SortExpression + " " + "DESC";
                }
                else
                {
                    Session["OrdenClientes"] = e.SortExpression + " " + "ASC";
                }

                gvClientesCorreo.PageIndex = 0;
                cargaClientes();
            }
        }

        protected void chkSeleccionado_CheckedChanged(object sender, EventArgs e)
        {
            List<ClienteCorreo> lsEnvCorreo = (List<ClienteCorreo>)Session["EnvioClientes"];

            btnLimipiaS.Visible = true;


            String id_cliente =((CheckBox)sender).Attributes["ID_CLIENTE"].ToString();
            DataSet DatosCliente = connMysql.TraerClienteCorreo(id_cliente);

            ClienteCorreo datosClienteCorreo = new ClienteCorreo() { ID_CLIENTE = int.Parse(DatosCliente.Tables[0].Rows[0].ItemArray[0].ToString()), Nombre = DatosCliente.Tables[0].Rows[0].ItemArray[1].ToString(), Apellido_paterno = DatosCliente.Tables[0].Rows[0].ItemArray[2].ToString(), Apellido_materno = DatosCliente.Tables[0].Rows[0].ItemArray[3].ToString(), EMAIL = DatosCliente.Tables[0].Rows[0].ItemArray[12].ToString()};
            
            if (((CheckBox)sender).Checked)
            {
                if (!lsEnvCorreo.Exists(x => x.ID_CLIENTE == datosClienteCorreo.ID_CLIENTE))
                {
                    lsEnvCorreo.Add(datosClienteCorreo);
                }
            }
            else
            {
                if (lsEnvCorreo.Exists(x => x.ID_CLIENTE == datosClienteCorreo.ID_CLIENTE))
                {
                    lsEnvCorreo.RemoveAll(x => x.ID_CLIENTE == datosClienteCorreo.ID_CLIENTE);
                }

                if (!RevisarSeleccion())
                {
                    btnLimipiaS.Visible = false;
                }
            }

            Session["EnvioClientes"] = lsEnvCorreo;
        }

        public void cargaClientes()
        {
            if (SesionViva())
            {
                DataSet ds = connMysql.TraerClientes(Session["CondicionClientes"].ToString(), "1,0");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].DefaultView.Sort = Session["OrdenClientes"].ToString();

                    gvClientesCorreo.DataSource = ds.Tables[0];
                    gvClientesCorreo.DataBind();
                }
                else
                {
                    gvClientesCorreo.DataSource = ds.Tables[0];
                    gvClientesCorreo.DataBind();

                    int totalColumnas = ds.Tables[0].Columns.Count;

                    if (gvClientesCorreo.Rows.Count == 0)
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

                        gvClientesCorreo.DataSource = dtTemporal;
                        gvClientesCorreo.DataBind();
                    }

                    gvClientesCorreo.Rows[0].Cells.Clear();
                    gvClientesCorreo.Rows[0].Cells.Add(new TableCell());
                    gvClientesCorreo.Rows[0].Cells[0].ColumnSpan = 15;
                    gvClientesCorreo.Rows[0].Cells[0].CssClass = "lblSinResultado";
                    gvClientesCorreo.Rows[0].Cells[0].Text = "Sin resultados";

                    gvClientesCorreo.Visible = true;
                }
            }
        }

        protected void chkFormCliRango_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFormCliRango.Checked)
            {
                TxtFormCliEdad.Attributes.Remove("style");
                TxtFormCliEdad.Attributes.Add("style", "width:70px; margin-right: 0px;");
                lblFormCliA.Visible = true;
                txtFormCliEdad2.Visible = true;
            }
            else
            {
                ocultaRango();
                txtFormCliEdad2.Attributes.Remove("style");
                TxtFormCliEdad.Attributes.Add("style", "width:calc(100% - 25px); margin-right:0px;");
                chkFormCliRango.Visible = true;
            }
            panelMsj.DefaultButton = btnFormCliBuscar.ID;
        }
        protected void llenarMedio()
        {
            DataSet ds = connMysql.consultarmedios();
            ddlFormCliMedio.DataSource = ds;
            ddlFormCliMedio.DataTextField = "MEDIO";
            ddlFormCliMedio.DataValueField = "MEDIO";
            ddlFormCliMedio.DataBind();

            ddlFormCliMedio.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        public void llenaEstados()
        {
            ddlFormCliEstado.DataTextField = "estado";
            ddlFormCliEstado.DataValueField = "ID";
            ddlFormCliEstado.DataSource = connMysql.traerEstado();
            ddlFormCliEstado.DataBind();

            ddlFormCliEstado.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        public void llenaPaises()
        {
            ddlFormCliPais.DataTextField = "pais";
            ddlFormCliPais.DataValueField = "pais";
            ddlFormCliPais.DataSource = connMysql.traerPaises();
            ddlFormCliPais.DataBind();

            ddlFormCliPais.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void ddlFormCliEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFormCliEstado.SelectedValue == "33")
            {
                divFormCliMunicipio.Visible = false;
                divFormCliPais.Visible = true;
            }
            else
            {
                divFormCliMunicipio.Visible = true;
                divFormCliPais.Visible = false;
                llenaMunicipio(ddlFormCliEstado.SelectedValue);
            }
        }

        public void llenaMunicipio(String Estado)
        {
            ddlFormCliMunicipio.Enabled = true;
            ddlFormCliMunicipio.DataTextField = "MUNICIPIO";
            ddlFormCliMunicipio.DataValueField = "MUNICIPIO";
            ddlFormCliMunicipio.DataSource = connMysql.traerMunicipio(Estado);
            ddlFormCliMunicipio.DataBind();
            ddlFormCliMunicipio.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (SesionViva())
            {
                MasterFarmacia master = (MasterFarmacia)this.Master;
                master.mostrarMensaje(true);
                sombraMensaje.Visible = true;

                divMensaje.Visible = false;
                divFormularioG.Visible = false;
                divFormularioP.Visible = false;
                divFormCliente.Visible = true;
                divFormTipo.Visible = false;
                divConfirmaDeleteTipo.Visible = false;

                panelMsj.DefaultButton = btnFormCliBuscar.ID;

                if (chkFormCliRango.Checked)
                {
                    TxtFormCliEdad.Attributes.Remove("style");
                    TxtFormCliEdad.Attributes.Add("style", "width:70px; margin-right: 0px;");
                    lblFormCliA.Visible = true;
                    txtFormCliEdad2.Visible = true;
                }
                else
                {
                    ocultaRango();
                    TxtFormCliEdad.Attributes.Add("style", "width:calc(100% - 25px); margin-right:0px;");
                    chkFormCliRango.Visible = true;
                }

                divFormCliObservacionesNota.Visible = false;
                int temporal = ddlFormCliEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1"));
                if (ddlFormCliEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1")) == -1)
                {
                    ddlFormCliEnviarCorreo.Items.Add(new ListItem("Todos", "-1"));
                    ddlFormCliEnviarCorreo.SelectedValue = "-1";
                }

                if (ddlFormCliEstatus.Items.IndexOf(lITodos) == -1)
                {
                    ddlFormCliEstatus.Items.Insert(0, lITodos);
                    ddlFormCliEstatus.SelectedIndex = 0;
                }

                if (ddlFormCliFactura.Items.IndexOf(new ListItem("Todos", "-1")) == -1)
                {
                    ddlFormCliFactura.Items.Add(new ListItem("Todos", "-1"));
                    ddlFormCliFactura.SelectedValue = "-1";
                }
                
                chkFormCliRango.Visible = true;
            }
        }

        public void ocultaRango()
        {
            TxtFormCliEdad.Attributes.Remove("style");
            txtFormCliEdad2.Text = "";
            lblFormCliA.Visible = false;
            txtFormCliEdad2.Visible = false;
            chkFormCliRango.Visible = false;
            chkFormCliRango.Checked = false;
        }

        protected void btnFormCliBuscar_Click(object sender, EventArgs e)
        {
            if (SesionViva())
            {
                Boolean pasa = true;
                String condicion = "";

                if (TxtFormCliNombre.Text.Trim().Length > 0)
                {
                    condicion += " nombre like '%" + TxtFormCliNombre.Text.Trim() + "%' ";
                }

                if (TxtFormCliApellidoP.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " apellido_paterno like '%" + TxtFormCliApellidoP.Text.Trim() + "%' ";
                }

                if (TxtFormCliApellidoM.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " apellido_materno like '%" + TxtFormCliApellidoM.Text.Trim() + "%' ";
                }

                if (ddlFormCliEstado.SelectedIndex > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " Estado like '%" + ddlFormCliEstado.Items[ddlFormCliEstado.SelectedIndex].Text.Trim() + "%' ";
                    if (ddlFormCliMunicipio.SelectedIndex > 0)
                    {
                        condicion += (condicion.Length > 0 ? " and " : "") + " Municipio like '%" + ddlFormCliMunicipio.Items[ddlFormCliMunicipio.SelectedIndex].Text.Trim() + "%' ";
                    }
                }

                if (ddlFormCliPais.SelectedIndex > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " Pais like '%" + ddlFormCliPais.Items[ddlFormCliPais.SelectedIndex].Text.Trim() + "%' ";
                }

                if (TxtFormCliFechaN.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " fecha_nacimiento like '%" + TxtFormCliFechaN.Text.Trim() + "%' ";
                }

                if (TxtFormCliFechaN.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " fecha_nacimiento like '%" + TxtFormCliFechaN.Text.Trim() + "%' ";
                }

                if (chkFormCliRango.Checked)
                {
                    if (TxtFormCliEdad.Text.Trim().Length > 0 && txtFormCliEdad2.Text.Trim().Length > 0)
                    {
                        condicion += (condicion.Length > 0 ? " and " : "") + " (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) >= " + TxtFormCliEdad.Text.Trim() + " and  (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) <= " + txtFormCliEdad2.Text.Trim() + " ";
                        TxtFormCliEdad.Attributes.Remove("style");
                        txtFormCliEdad2.Attributes.Remove("style");
                        TxtFormCliEdad.Attributes.Add("style", "width:70px; margin-right: 0px;");
                        txtFormCliEdad2.Attributes.Add("style", "width:70px; margin-right: 0px;");
                    }
                    else
                    {
                        TxtFormCliEdad.Attributes.Remove("style");
                        txtFormCliEdad2.Attributes.Remove("style");
                        TxtFormCliEdad.Attributes.Add("style", "width:70px; margin-right: 0px;");
                        txtFormCliEdad2.Attributes.Add("style", "width:70px; margin-right: 0px;");

                        if (TxtFormCliEdad.Text.Trim().Length == 0 && txtFormCliEdad2.Text.Trim().Length > 0)
                        {
                            TxtFormCliEdad.Attributes.Add("style", "width:70px; margin-right: 0px; border: 1px red solid;");
                            pasa = false;
                        }

                        if (TxtFormCliEdad.Text.Trim().Length > 0 && txtFormCliEdad2.Text.Trim().Length == 0)
                        {
                            txtFormCliEdad2.Attributes.Add("style", "width:70px; margin-right: 0px; border: 1px red solid;");
                            pasa = false;
                        }
                    }
                }
                else
                {
                    if (TxtFormCliEdad.Text.Trim().Length > 0)
                    {
                        condicion += (condicion.Length > 0 ? " and " : "") + " (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1))=" + TxtFormCliEdad.Text.Trim() + " ";
                    }
                }


                if (TxtFormCliFechaI.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " fecha_ingreso like '%" + TxtFormCliFechaI.Text.Trim() + "%' ";
                }

                if (ddlFormCliMedio.SelectedValue != "0")
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " medio like '%" + ddlFormCliMedio.SelectedValue.Trim() + "%' ";
                }

                if (TxtFormCliTelFijo.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " tel_casa_fijo like '%" + TxtFormCliTelFijo.Text.Trim() + "%' ";
                }

                if (TxtFormCliExtension.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " Extension like '%" + TxtFormCliExtension.Text.Trim() + "%' ";
                }

                if (TxtFormCliCelular.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " celular like '%" + TxtFormCliCelular.Text.Trim() + "%' ";
                }

                if (TxtFormCliEmail.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " email like '%" + TxtFormCliEmail.Text.Trim() + "%' ";
                }

                if (TxtFormCliObservaciones.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " observaciones like '%" + TxtFormCliObservaciones.Text.Trim() + "%' ";
                }

                if (TxtFormCliNota.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " nota like '%" + TxtFormCliNota.Text.Trim() + "%' ";
                }

                if (ddlFormCliEstatus.SelectedValue == "0" || ddlFormCliEstatus.SelectedValue == "1")
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " estatus LIKE '%" + ddlFormCliEstatus.SelectedValue + "%' ";
                }

                if (ddlFormCliEnviarCorreo.SelectedValue != "-1")
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " Enviar_Correo = '" + ddlFormCliEnviarCorreo.SelectedValue + "' ";
                }
                

                if (pasa)
                {
                    lblFormCliError.Text = "";
                    Session["CondicionClientes"] = condicion;
                    cargaClientes();
                    
                    divFormularioG.Visible = false;

                    MasterFarmacia master = (MasterFarmacia)this.Master;
                    master.mostrarMensaje(false);
                    sombraMensaje.Visible = false;
                }
                else
                {
                    lblFormCliError.Text = "Favor de llenar los campos faltantes";
                }
            }
        }

        protected void btnFormCliLimpiar_Click(object sender, EventArgs e)
        {
            if (SesionViva())
            {
                TxtFormCliIdCliente.Text = "";
                TxtFormCliNombre.Text = "";
                TxtFormCliApellidoP.Text = "";
                TxtFormCliApellidoM.Text = "";
                ddlFormCliEstado.SelectedIndex = -1;
                ddlFormCliMunicipio.SelectedIndex = -1;
                TxtFormCliEdad.Text = "";
                txtFormCliEdad2.Text = "";
                TxtFormCliFechaI.Text = "";
                chkFormCliRango.Checked = false;
                ddlFormCliMedio.SelectedIndex = -1;
                TxtFormCliTelFijo.Text = "";
                TxtFormCliExtension.Text = "";
                TxtFormCliCelular.Text = "";
                TxtFormCliFechaN.Text = "";
                TxtFormCliEmail.Text = "";
                TxtFormCliObservaciones.Text = "";
                TxtFormCliNota.Text = "";
                ddlFormCliEstatus.SelectedIndex = 0;
                ddlFormCliPais.SelectedIndex = -1;

                lblFormCliA.Visible = false;
                TxtFormCliEdad.Attributes.Remove("style");
                txtFormCliEdad2.Visible = false;
                TxtFormCliEdad.Attributes.Add("style", "width:calc(100% - 25px); margin-right:0px;");
                divFormCliPais.Visible = false;
                divFormCliMunicipio.Visible = true;
            }
        }

        protected void btnFormCliCerrar_Click(object sender, EventArgs e)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;

            divFormCliente.Visible = false;
        }



        protected void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            List<ClienteCorreo> lsEnvCorreo = (List<ClienteCorreo>)Session["EnvioClientes"];
            divFormCliente.Visible = false;
            if (lsEnvCorreo.Count > 0)
            {
                sombraMensaje.Visible = true;
                divConfirmacionCorreo.Visible = true;
                cargaDestinoCorreo();
            }else
            {
                mostrarMensaje("Favor de seleccionar al menos a un cliente");
            }
        }


    #region confirmacionCorreo

        protected void gvDestinoCorreo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblNumeroFila = (Label)e.Row.FindControl("lblindice");

                try
                {
                    lblNumeroFila.Text = ((e.Row.RowIndex + 1) + (gvClientesCorreo.PageIndex * gvClientesCorreo.PageSize)).ToString();
                }
                catch { }
            }
        }

        protected void gvDestinoCorreo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDestinoCorreo.PageIndex = e.NewPageIndex;
            cargaDestinoCorreo();
        }

        protected void gvDestinoCorreo_Sorting(object sender, GridViewSortEventArgs e)
        {

            if (SesionViva())
            {
                if (Session["OrdenConfirCorreos"].ToString() == e.SortExpression + " " + "ASC")
                {
                    Session["OrdenConfirCorreos"] = e.SortExpression + " " + "DESC";
                }
                else
                {
                    Session["OrdenConfirCorreos"] = e.SortExpression + " " + "ASC";
                }

                gvDestinoCorreo.PageIndex = 0;
                cargaDestinoCorreo();
            }
        }

        public void cargaDestinoCorreo()
        {
            List<ClienteCorreo> lsEnvCorreo = (List<ClienteCorreo>)Session["EnvioClientes"];

            if (SesionViva())
            {
                
                if (lsEnvCorreo.Count > 0)
                {
                    
                    gvDestinoCorreo.DataSource = lsEnvCorreo;
                    if(Session["OrdenConfirCorreos"].ToString().Trim().Length > 0) { 
                        gvDestinoCorreo.Sort(Session["OrdenConfirCorreos"].ToString().Split(' ')[0], (Session["OrdenConfirCorreos"].ToString().Split(' ')[1] == "ASC" ? SortDirection.Ascending : SortDirection.Descending));
                    }else
                    {
                        
                    }
                    gvDestinoCorreo.DataBind();
                }
                else
                {
                    gvDestinoCorreo.DataSource = lsEnvCorreo;
                    gvDestinoCorreo.DataBind();

                    gvDestinoCorreo.Rows[0].Cells.Clear();
                    gvDestinoCorreo.Rows[0].Cells.Add(new TableCell());
                    gvDestinoCorreo.Rows[0].Cells[0].ColumnSpan = 7;
                    gvDestinoCorreo.Rows[0].Cells[0].CssClass = "lblSinResultado";
                    gvDestinoCorreo.Rows[0].Cells[0].Text = "Sin resultados";

                    gvDestinoCorreo.Visible = true;
                }
            }
        }

        protected void btnConfcorreoCancelar_Click(object sender, EventArgs e)
        {

            sombraMensaje.Visible = false;
            divConfirmacionCorreo.Visible = false;
            cargaDestinoCorreo();
        }

        protected void btnConfCorreoOK_Click(object sender, EventArgs e)
        {
            divConfirmacionCorreo.Visible = false;

            List<ClienteCorreo> lsEnvCorreo = (List<ClienteCorreo>)Session["EnvioClientes"];
            List<String> listaCorreos = new List<string>();
            List<String> listaIDs = new List<string>();

            foreach (ClienteCorreo cliente in lsEnvCorreo)
            {
                listaCorreos.Add(cliente.EMAIL);
                listaIDs.Add(cliente.ID_CLIENTE.ToString());
            }

            mandarCorreos(listaCorreos, listaIDs);

            Session["EnvioClientes"] = new List<ClienteCorreo>();

            cargaClientes();

            MContenidoJS.Attributes.Remove("Style");
            CargaJS.Visible = false;
            
            lblMensajeJS.Text = "Los correos han sido enviados exitosamente";
            btnCorreoEnviadoOK.Visible = true;


        }


        protected void btnCorreoEnviadoOK_Click(object sender, EventArgs e)
        {
            sombraMensaje.Visible = false;

            MContenidoJS.Attributes.Add("Style", "display:none;");
            CargaJS.Visible = true;
            lblMensajeJS.Text = "Enviando correos a los clientes seleccionados";
            btnCorreoEnviadoOK.Visible = false;
        }

        public void mandarCorreos(List<String> listaCorreos, List<String> listaIDs)
        {
            DatosCorreo dCorreo = connMysql.ConsultaDatosCorreoSelec();

            connMysql.ActualizaUltimoCorreo();

            EnviarCorreo enviaCorreo = new EnviarCorreo();

            String cadenaIDs = "";
            foreach (String id in listaIDs)
            {
                if (cadenaIDs.Length == 0)
                {
                    cadenaIDs += id;
                }
                else
                {
                    cadenaIDs += "," + id;
                }
            }

            if (listaCorreos.Count > 0)
            {
                bool respuesta = enviaCorreo.EnviarSelectivo(listaCorreos, Server.MapPath(dCorreo.SMTP_IMAGEN));

                if (respuesta)
                {
                    lblMensajeJS.Text = "Los correos se han enviado exitosamente";
                    connMysql.ActualizaCorreoEnvCliente(cadenaIDs);
                    connMysql.GuardaBitacora(Session["usuario"].ToString(), listaIDs, dCorreo);
                }
                else
                {
                    lblMensajeJS.Text = "Ocurrio un problema al enviar los correo, favor de intentarlo nuevamente.";
                }
            }

        }



        #endregion
        #endregion




































        #region reactivarCliente
        protected void btnOpcionClientes_Click(object sender, EventArgs e)
        {
            divClientes.Visible = true;
            divGerentes.Visible = false;
            divPerfiles.Visible = false;
            divEnvioCorreo.Visible = false;
            divCorreo.Visible = false;
            divConfigCitas.Visible = false;

            btnConfigCitas.CssClass = "";
            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "";
            btnEnvioCorreo.CssClass = "";
            btnOpcionClientes.CssClass = "seleccionado";
            CambiaTitulo("Reactivar clientes");
            cargaClientesE("estatus = 3");
            ocultarOpcionesCorreo();
        }

        public void cargaClientesE(String condicion)
        {

            DataSet ds = connMysql.TraerClEliminados(condicion);

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
                gvClientes.Rows[0].Cells[0].ColumnSpan = 17;
                gvClientes.Rows[0].Cells[0].CssClass = "lblSinResultado";
                gvClientes.Rows[0].Cells[0].Text = "Sin resultados";

                //gvGerentes.Visible = false;
                //gvPerfiles.Visible = false;
                gvClientes.Visible = true;
            }
        }
        protected void ddlFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFactura.SelectedValue == "1")
            {
                divDatosFactura.Visible = true;
            }
            else
            {
                divDatosFactura.Visible = false;
            }
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEstado.SelectedValue == "33")
            {
                divMunicipio.Visible = false;
                divPais.Visible = true;
            }
            else
            {
                divMunicipio.Visible = true;
                divPais.Visible = false;
                llenaMunicipio2(ddlEstado.SelectedValue);
            }
        }

        public void llenaMunicipio2(String Estado)
        {
            ddlMunicipio.Enabled = true;
            ddlMunicipio.DataTextField = "MUNICIPIO";
            ddlMunicipio.DataValueField = "MUNICIPIO";
            ddlMunicipio.DataSource = connMysql.traerMunicipio(Estado);
            ddlMunicipio.DataBind();
            ddlMunicipio.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void btnBuscarCl_Click(object sender, EventArgs e)
        {
            Boolean pasa = true;
            String condicion = "";

            if (TxtNombre.Text.Trim().Length > 0)
            {
                condicion += " nombre like '%" + TxtNombre.Text.Trim() + "%' ";
            }

            if (TxtApellidoP.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " apellido_paterno like '%" + TxtApellidoP.Text.Trim() + "%' ";
            }

            if (TxtApellidoM.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " apellido_materno like '%" + TxtApellidoM.Text.Trim() + "%' ";
            }

            if (ddlEstado.SelectedIndex > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " Estado like '%" + ddlEstado.Items[ddlEstado.SelectedIndex].Text.Trim() + "%' ";
                if (ddlMunicipio.SelectedIndex > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " Municipio like '%" + ddlMunicipio.Items[ddlMunicipio.SelectedIndex].Text.Trim() + "%' ";
                }
            }

            if (ddlPais.SelectedIndex > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " Pais like '%" + ddlPais.Items[ddlPais.SelectedIndex].Text.Trim() + "%' ";
            }

            if (TxtFechaN.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " fecha_nacimiento like '%" + TxtFechaN.Text.Trim() + "%' ";
            }

            if (TxtFechaN.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " fecha_nacimiento like '%" + TxtFechaN.Text.Trim() + "%' ";
            }


            if (chkRango.Checked)
            {
                if (TxtEdad.Text.Trim().Length > 0 && txtEdad2.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) >= " + TxtEdad.Text.Trim() + " and  (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1)) <= " + txtEdad2.Text.Trim() + " ";
                    TxtEdad.Attributes.Remove("style");
                    txtEdad2.Attributes.Remove("style");
                    TxtEdad.Attributes.Add("style", "width:70px; margin-right: 0px;");
                    txtEdad2.Attributes.Add("style", "width:70px; margin-right: 0px;");
                }
                else
                {
                    TxtEdad.Attributes.Remove("style");
                    txtEdad2.Attributes.Remove("style");
                    TxtEdad.Attributes.Add("style", "width:70px; margin-right: 0px;");
                    txtEdad2.Attributes.Add("style", "width:70px; margin-right: 0px;");

                    if (TxtEdad.Text.Trim().Length == 0 && txtEdad2.Text.Trim().Length > 0)
                    {
                        TxtEdad.Attributes.Add("style", "width:70px; margin-right: 0px; border: 1px red solid;");
                        pasa = false;
                    }

                    if (TxtEdad.Text.Trim().Length > 0 && txtEdad2.Text.Trim().Length == 0)
                    {
                        txtEdad2.Attributes.Add("style", "width:70px; margin-right: 0px; border: 1px red solid;");
                        pasa = false;
                    }
                }
            }
            else
            {
                if (TxtEdad.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " (YEAR(CURDATE())-YEAR(FECHA_NACIMIENTO) + IF(DATE_FORMAT(CURDATE(),'%m-%d') >= DATE_FORMAT(FECHA_NACIMIENTO,'%m-%d'), 0, -1))=" + TxtEdad.Text.Trim() + " ";
                }
            }

            if (TxtFechaI.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " fecha_ingreso like '%" + TxtFechaI.Text.Trim() + "%' ";
            }

            if (ddlMedio.SelectedValue != "0")
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " medio like '%" + ddlMedio.SelectedValue.Trim() + "%' ";
            }

            if (TxtTelFijo.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " tel_casa_fijo like '%" + TxtTelFijo.Text.Trim() + "%' ";
            }

            if (TxtExtension.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " Extension like '%" + TxtExtension.Text.Trim() + "%' ";
            }

            if (TxtCelular.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " celular like '%" + TxtCelular.Text.Trim() + "%' ";
            }

            if (TxtEmail.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " email like '%" + TxtEmail.Text.Trim() + "%' ";
            }

            if (TxtObservaciones.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " observaciones like '%" + TxtObservaciones.Text.Trim() + "%' ";
            }

            if (TxtNota.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " nota like '%" + TxtNota.Text.Trim() + "%' ";
            }

            if (ddlEnviarCorreo.SelectedValue != "-1")
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " Enviar_Correo = '" + ddlEnviarCorreo.SelectedValue + "' ";
            }
            if (ddlFactura.SelectedValue != "-1")
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " req_factura like '%" + ddlFactura.SelectedValue.Trim() + "%' ";
            }
            if (txtDirFiscal.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " dir_factura like '%" + txtDirFiscal.Text.Trim() + "%' ";
            }
            if (ddlEntidad.SelectedValue != "-1")
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " entidad like '%" + ddlEntidad.SelectedItem.Text.Trim() + "%' ";
            }
            if (txtRfc.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " rfc like '%" + txtRfc.Text.Trim() + "%' ";
            }
            if (txtNmR.Text.Trim().Length > 0)
            {
                condicion += (condicion.Length > 0 ? " and " : "") + " NombRazon_factura like '%" + txtNmR.Text.Trim() + "%' ";
            }
            if (pasa)
            {
                lblError.Text = "";
                //Session["Condicion"] = condicion;
                cargaClientesE(condicion);

                btnLimpiarCl.Visible = true;
                btnBuscarCl.Visible = true;
                divFormBusqCl.Visible = false;
                divFormularioG.Visible = false;

                MasterFarmacia master = (MasterFarmacia)this.Master;
                master.mostrarMensaje(false);
                sombraMensaje.Visible = false;
            }
            else
            {
                lblError.Text = "Favor de llenar los campos faltantes";
            }
        }

        protected void btnLimpiarCl_Click(object sender, EventArgs e)
        {
            TxtIdCliente.Text = "";
            TxtNombre.Text = "";
            TxtApellidoP.Text = "";
            TxtApellidoM.Text = "";
            ddlEstado.SelectedIndex = -1;
            ddlMunicipio.SelectedIndex = -1;
            TxtEdad.Text = "";
            txtEdad2.Text = "";
            TxtFechaI.Text = "";
            chkRango.Checked = false;
            ddlMedio.SelectedIndex = -1;
            TxtTelFijo.Text = "";
            TxtExtension.Text = "";
            TxtCelular.Text = "";
            TxtFechaN.Text = "";
            TxtEmail.Text = "";
            TxtObservaciones.Text = "";
            TxtNota.Text = "";
            ddlPais.SelectedIndex = -1;
            txtRfc.Text = "";
            txtDirFiscal.Text = "";
            txtNmR.Text = "";
            divDatosFactura.Visible = false;
            ddlFactura.SelectedValue = "0";
            ddlEntidad.SelectedValue = "0";

            lblA.Visible = false;
            TxtEdad.Attributes.Remove("style");
            txtEdad2.Visible = false;
            TxtEdad.Attributes.Add("style", "width:calc(100% - 25px); margin-right:0px;");
            divPais.Visible = false;
            divMunicipio.Visible = true;
        }

        protected void FGCancelarCl_Click(object sender, EventArgs e)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;

            divFormularioG.Visible = false;
            divFormBusqCl.Visible = false;


            TxtIdCliente.Text = "";
            TxtNombre.Text = "";
            TxtApellidoP.Text = "";
            TxtApellidoM.Text = "";
            ddlEstado.SelectedIndex = -1;
            ddlMunicipio.SelectedIndex = -1;

            TxtEdad.Text = "";
            TxtFechaI.Text = "";
            ddlMedio.SelectedIndex = -1;

            TxtTelFijo.Text = "";
            TxtExtension.Text = "";
            TxtCelular.Text = "";
            TxtFechaN.Text = "";
            TxtEmail.Text = "";
            TxtObservaciones.Text = "";
            TxtNota.Text = "";

            ddlPais.SelectedIndex = -1;
            divPais.Visible = false;
            divMunicipio.Visible = true;

            ddlEntidad.SelectedIndex = 0;
            txtDirFiscal.Text = "";
            ddlFactura.SelectedIndex = 1;
            txtNmR.Text = "";
            txtRfc.Text = "";
            divDatosFactura.Visible = false;

            lblError.Text = "";

            try
            {
                TxtNombre.Attributes.Remove("style");
                TxtApellidoP.Attributes.Remove("style");
                TxtApellidoM.Attributes.Remove("style");
                ddlEstado.Attributes.Remove("style");
                ddlMunicipio.Attributes.Remove("style");
                TxtEdad.Attributes.Remove("style");
                TxtFechaI.Attributes.Remove("style");
                ddlMedio.Attributes.Remove("style");
                TxtTelFijo.Attributes.Remove("style");
                TxtCelular.Attributes.Remove("style");
                TxtFechaN.Attributes.Remove("style");
                TxtEmail.Attributes.Remove("style");
                ddlPais.Attributes.Remove("style");
            }
            catch { }

            gvGerentes.EditIndex = -1;
            cargaClientesE("");
        }

        protected void btnBuscarCli_Click(object sender, EventArgs e)
        {
            TxtEdad.Enabled = true;
            divFormBusqCl.Visible = true;
            divFormCliente.Visible = false;

            if (chkRango.Checked)
            {
                TxtEdad.Attributes.Remove("style");
                TxtEdad.Attributes.Add("style", "width:70px; margin-right: 0px;");
                lblA.Visible = true;
                txtEdad2.Visible = true;
            }
            else
            {
                ocultaRango2();
                TxtEdad.Attributes.Add("style", "width:calc(100% - 25px); margin-right:0px;");
                chkRango.Visible = true;
            }

            int temporal = ddlEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1"));
            if (ddlEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1")) == -1)
            {
                ddlEnviarCorreo.Items.Add(new ListItem("Todos", "-1"));
                ddlEnviarCorreo.SelectedValue = "-1";
            }

            if (ddlFactura.Items.IndexOf(new ListItem("Todos", "-1")) == -1)
            {
                ddlFactura.Items.Add(new ListItem("Todos", "-1"));
                ddlFactura.SelectedValue = "-1";
            }

            if (ddlEntidad.Items.IndexOf(new ListItem("Todos", "-1")) == -1)
            {
                ddlEntidad.Items.Add(new ListItem("Todos", "-1"));
                ddlEntidad.SelectedValue = "-1";
            }

            divFormBusqCl.Visible = true;
            divFormularioG.Visible = false;
            divFormularioP.Visible = false;

            chkRango.Visible = true;

            llenaEstados2();
            llenarMedio2();

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;
        }

        public void ocultaRango2()
        {
            TxtEdad.Attributes.Remove("style");
            txtEdad2.Text = "";
            lblA.Visible = false;
            txtEdad2.Visible = false;
            chkRango.Visible = false;
            chkRango.Checked = false;
        }

        protected void chkRango_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRango.Checked)
            {
                TxtEdad.Attributes.Remove("style");
                TxtEdad.Attributes.Add("style", "width:70px; margin-right: 0px;");
                lblA.Visible = true;
                txtEdad2.Visible = true;
            }
            else
            {
                ocultaRango2();
                txtEdad2.Attributes.Remove("style");
                TxtEdad.Attributes.Add("style", "width:calc(100% - 25px); margin-right:0px;");
                chkRango.Visible = true;
            }
            //panelMsj.DefaultButton = btnBuscarCl.ID;
        }

        public void llenaEstados2()
        {
            ddlEstado.DataTextField = "estado";
            ddlEstado.DataValueField = "ID";
            ddlEstado.DataSource = connMysql.traerEstado();
            ddlEstado.DataBind();

            ddlEstado.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void llenarMedio2()
        {
            DataSet ds = connMysql.consultarmedios();
            ddlMedio.DataSource = ds;
            ddlMedio.DataTextField = "MEDIO";
            ddlMedio.DataValueField = "MEDIO";
            ddlMedio.DataBind();

            ddlMedio.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void gvClientes_RowDeleting(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            Label Id_Cliente = (Label)row.FindControl("lblIdCliente");
            String IdClienterefinado = Id_Cliente.Text;
            recupera.Text = IdClienterefinado.ToString();

            sombraMensaje.Visible = true;
            mostrarMensaje2(((Label)row.FindControl("lblNombreC")).Text + " " + ((Label)row.FindControl("lblApellidoP")).Text + " " + ((Label)row.FindControl("lblApellidoM")).Text + "?");
        }

        public void mostrarMensaje2(String Nombre)
        {
            lblPregunta2.Text = Nombre.ToUpper() + "";

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;
            divFormularioG.Visible = false;
            divFormularioP.Visible = false;
            divFormBusqCl.Visible = false;
            divMensaje.Visible = false;
            divmensaje2.Visible = true;
            BAceptar.Visible = true;
            BCancelar.Visible = true;
        }

        protected void BCancelar_Click(object sender, EventArgs e)
        {
            divMensaje.Visible = false;
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;
            divmensaje2.Visible = false;
            BAceptar.Visible = false;
            BCancelar.Visible = false;
        }

        protected void BAceptar_Click(object sender, EventArgs e)
        {
            String idcliente = recupera.Text;
            String resultado = connMysql.ReactivaCliente(idcliente, connMysql.traerIDEmpleado(Session["usuario"].ToString()));

            sombraMensaje.Visible = true;
            divmensaje2.Visible = false;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Cliente reactivado exitosamente" : resultado));

            cargaClientesE("");
        }

        #endregion

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
        

        protected void gvClientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label etiquetaFechaI = (Label)e.Row.FindControl("lblFechaI");
                Label etiquetaFechaN = (Label)e.Row.FindControl("lblFechaN");

                try
                {
                    etiquetaFechaN.Text = ((DateTime)((DataRowView)e.Row.DataItem).Row.ItemArray[5]).ToString("dd/MM/yyyy");
                }
                catch { }

                try
                {
                    etiquetaFechaI.Text = ((DateTime)((DataRowView)e.Row.DataItem).Row.ItemArray[6]).ToString("dd/MM/yyyy");
                }
                catch { }
            }

        }


        public void CambiaTitulo(String subtitulo)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            String LblTitleGral = "<img src='Imagenes/Administracion.png' alt='clientes'><h1>Administración";
            master.cambiarLblTitle(LblTitleGral + " - " + subtitulo + "</h1>");
        }

        protected void cargaCorreoSelec()
        {
            DatosCorreo datosCorreo = connMysql.ConsultaDatosCorreoSelec();

            txtMsjSelec.Text = datosCorreo.SMTP_MENSAJE;
            txtAsuntoSelec.Text = datosCorreo.SMTP_SUJETO;
            txtFirmaSelec.Text = datosCorreo.SMTP_FIRMA;
        }

        protected void btnContenidCorreoSelec_Click(object sender, EventArgs e)
        {
            btnActCorreoConf.Visible = false;
            btnActCorreoCont.Visible = false;
            btnActCorreoTecn.Visible = false;
            btnActCorreoContSelec.Visible = true;
            btnActTipo.Visible = false;

            btnConfiguracionTecnica.CssClass = "subOpcion";
            btnConfiguracionCorreo.CssClass = "subOpcion";
            btnContenidCorreoSelec.CssClass = "subOpcion subOpcionSeleccionado";
            btnContenidCorreo.CssClass = "subOpcion";
            btnCorreoRecordatorio.CssClass = "subOpcion";
            btnTiposCorreo.CssClass = "subOpcion";

            divCorreoConfTec.Visible = false;
            divCorreoCont.Visible = false;
            divCorreoContSelec.Visible = true;
            divCorreoConfCorreo.Visible = false;
            btnPreview.Visible = true;

            divCorreoRecordatorio.Visible = false;
            btnActCorreoRecordatorio.Visible = false;

            divTiposCorreo.Visible = false;
            txtBusqTipo.Visible = false;
            imgBusqTipo.Visible = false;

            CambiaTitulo("Contenido de correo selectivo");
            Session.Add("TipoPreview", "selectivo");

        }




        protected void btnConfigCitas_Click(object sender, EventArgs e)
        {
            divGerentes.Visible = false;
            divPerfiles.Visible = false;
            divCorreo.Visible = false;
            divEnvioCorreo.Visible = false;
            divClientes.Visible = false;
            divConfigCitas.Visible = true;

            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "";
            btnEnvioCorreo.CssClass = "";
            btnOpcionClientes.CssClass = "";
            btnConfigCitas.CssClass = "seleccionado";

            ocultarOpcionesCorreo();
            TraerFechasInhabiles();
        }

        protected void FEditHoraLaboral_Click(object sender, EventArgs e)
        {
            string Apertura = horaApertura.Value;
            string Cierre = horaCierre.Value;
            connMysql.GuardaHorarioLaboralA(Apertura);
            connMysql.GuardaHorarioLaboralC(Cierre);

            Horario();

        }
        protected void FEditDiasInhabiles_Click(object sender, EventArgs e)
        {
            bool Checkbox1 = DiaCheckbox1.Checked;
            bool Checkbox2 = DiaCheckbox2.Checked;
            bool Checkbox3 = DiaCheckbox3.Checked;
            bool Checkbox4 = DiaCheckbox4.Checked;
            bool Checkbox5 = DiaCheckbox5.Checked;
            bool Checkbox6 = DiaCheckbox6.Checked;
            bool Checkbox7 = DiaCheckbox7.Checked;

            string diasInhabilesSemana = "" + Checkbox1 + "," + Checkbox2 + "," + Checkbox3 + "," + Checkbox4 + "," + Checkbox5 + "," + Checkbox6 + "," + Checkbox7 + "";
            string resultado = connMysql.GuardaDiasInhabilesSemana(diasInhabilesSemana);

            DiasSemana();


            //resultado = connMysql.GuardaDiasInhabiles(Checkbox1, Checkbox2, Checkbox3, Checkbox4, Checkbox5, Checkbox6, Checkbox7);
        }
        protected void btnActualizarCitasHrsSemana(object sender, EventArgs e)
        {
            bool Checkbox1 = DiaCheckbox1.Checked;
            bool Checkbox2 = DiaCheckbox2.Checked;
            bool Checkbox3 = DiaCheckbox3.Checked;
            bool Checkbox4 = DiaCheckbox4.Checked;
            bool Checkbox5 = DiaCheckbox5.Checked;
            bool Checkbox6 = DiaCheckbox6.Checked;
            bool Checkbox7 = DiaCheckbox7.Checked;
            string Apertura = horaApertura.Value;
            string Cierre = horaCierre.Value;

            var consultHorarioA = connMysql.TraerHrApertura();
            var consultHorarioC = connMysql.TraerHrCierre();
            var editHorarioA = connMysql.GuardaHorarioLaboralA(Apertura);
            var esitHorarioC = connMysql.GuardaHorarioLaboralC(Cierre);
            var consultDiasIbha = connMysql.TraerDiasSemana();
            string diasInhabilesSemana = Checkbox7 + "," + Checkbox1 + "," + Checkbox2 + "," + Checkbox3 + "," + Checkbox4 + "," + Checkbox5 + "," + Checkbox6  ;

            if (Apertura != consultHorarioA)
            {
                if (editHorarioA == "OK" && esitHorarioC == "OK")
                {
                    mostrarMensaje((editHorarioA.Trim().Equals("OK") ? "Horario de atencion editado exitosamente" : esitHorarioC));
                }
                Horario();
            }
            if (consultHorarioC != Cierre)
            {
                if (editHorarioA == "OK" && esitHorarioC == "OK")
                {
                    mostrarMensaje((editHorarioA.Trim().Equals("OK") ? "Horario de atencion editado exitosamente" : esitHorarioC));
                }
                Horario();
            }
            if (consultDiasIbha != diasInhabilesSemana)
            {
                string resultado = connMysql.GuardaDiasInhabilesSemana(diasInhabilesSemana);
                mostrarMensaje((resultado.Trim().Equals("OK") ? "Dias inhabiles editados exitosamente" : resultado));
                DiasSemana();
                Horario();
            }
        }

        protected void DiasSemana()
        {
            var diasSemanaIbhabiles = connMysql.TraerDiasSemana();
            var fooArray = diasSemanaIbhabiles.Split(',');

            DiaCheckbox1.Checked = Convert.ToBoolean(fooArray[1]);
            DiaCheckbox2.Checked = Convert.ToBoolean(fooArray[2]);
            DiaCheckbox3.Checked = Convert.ToBoolean(fooArray[3]);
            DiaCheckbox4.Checked = Convert.ToBoolean(fooArray[4]);
            DiaCheckbox5.Checked = Convert.ToBoolean(fooArray[5]);
            DiaCheckbox6.Checked = Convert.ToBoolean(fooArray[6]);
            DiaCheckbox7.Checked = Convert.ToBoolean(fooArray[0]);
        }

        protected void Horario()
        {
            string actualHrApertura = connMysql.TraerHrApertura();
            string actualHrCierre = connMysql.TraerHrCierre();
            actualHorario.Value = "De " + actualHrApertura + " A " + actualHrCierre + "";
        }
        protected void FAgregarFechasInhabiles(object sender, EventArgs e)
        {
            string FechaInhabil = nuevaFecha.Value;
            if (FechaInhabil != "")
            {
                string resultado = connMysql.GuardaFechasInhabiles(FechaInhabil);
                TraerFechasInhabiles();
                mostrarMensaje((resultado.Trim().Equals("OK") ? "Fecha agregada exitosamente" : resultado));
            }
            else
            {
                mostrarMensaje("Ingresa una fecha");
            }
        }

        public void TraerFechasInhabiles()
        {
            DataSet ds = connMysql.TraerFechaInhabiles();

            if (ds.Tables[0].Rows.Count > 0)
            {
                TbFechas.DataSource = ds.Tables[0];
                TbFechas.DataBind();
            }
            else
            {
                TbFechas.DataSource = ds.Tables[0];
                TbFechas.DataBind();

                if (TbFechas.Rows.Count == 0)
                {
                    DataTable dtTemporal = new DataTable();
                    dtTemporal.Columns.Add("fechas_inhabiles");
                    dtTemporal.NewRow();

                    DataRow drTemporal = dtTemporal.NewRow();
                    dtTemporal.Rows.InsertAt(drTemporal, 0);

                    TbFechas.DataSource = dtTemporal;
                    TbFechas.DataBind();
                }

                TbFechas.Rows[0].Cells.Clear();
                TbFechas.Rows[0].Cells.Add(new TableCell());
                TbFechas.Rows[0].Cells[0].ColumnSpan = 6;
                TbFechas.Rows[0].Cells[0].CssClass = "lblSinResultado";
                TbFechas.Rows[0].Cells[0].Text = "Sin resultados";
                TbFechas.Visible = true;
            }

            Horario();
            DiasSemana();

        }
        protected void TbFechas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)TbFechas.Rows[e.RowIndex];
            Label Fecha = (Label)row.FindControl("lblIdFecha");
            String resultado = connMysql.EliminaFecha(Fecha.Text);

            //sombraMensaje.Visible = true;
            //mostrarMensaje((resultado.Trim().Equals("OK") ? "Fecha eliminado exitosamente" : resultado));
            TraerFechasInhabiles();
        }

        protected void TbFechas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TbFechas.PageIndex = e.NewPageIndex;
            TraerFechasInhabiles();
        }

        protected void btnActCorreoRecordatorio_Click(object sender, EventArgs e)
        {
            DatosCorreo datoscorreo = new DatosCorreo();

            if ((fuploadRec.PostedFile.ContentLength / 1024) < 1024)
            {
                lblErrorImage.Text = "";

                datoscorreo.SMTP_SUJETO = txtAsuntoRec.Text;
                datoscorreo.SMTP_MENSAJE = txtMesajeRec.Text;
                datoscorreo.SMTP_FIRMA = txtFirmaRec.Text;
                datoscorreo.SMTP_IMAGEN = "";
                datoscorreo.DiasAntes = txtDiasAntesCita.Text;

                if (fuploadRec.FileName.Trim().Length != 0)
                {
                    if (fuploadRec.HasFile)
                    {
                        string nombreArchivo = fuploadRec.FileName;
                        string ruta = "Imagenes/Correo/" + nombreArchivo;
                        fuploadRec.SaveAs(Server.MapPath(ruta));

                        datoscorreo.SMTP_IMAGEN = ruta;
                    }
                }
                String resultado = connMysql.ActualizaContCorreoRec(datoscorreo, datoscorreo.DiasAntes);

                if (resultado == "OK")
                {
                    sombraMensaje.Visible = true;
                    mostrarMensaje((resultado.Trim().Equals("OK") ? "Datos de correo recordatorio actualizado exitosamente" : resultado));
                }
            }
            else
            {
                lblErrorImage.Text = "* El tamaño de la imagen debe ser menor a 1 MB";
            }
        }

        protected void cargaCorreoRec()
        {
            DatosCorreo datosCorreo = connMysql.ConsultaDatosCorreoRec();

            txtDiasAntesCita.Text = datosCorreo.DiasAntes;
            txtMesajeRec.Text = datosCorreo.SMTP_MENSAJE;
            txtAsuntoRec.Text = datosCorreo.SMTP_SUJETO;
            txtFirmaRec.Text = datosCorreo.SMTP_FIRMA;
        }

        protected void btnCorreoRecordatorio_Click(object sender, EventArgs e)
        {
            btnActCorreoConf.Visible = false;
            btnActCorreoCont.Visible = false;
            btnActCorreoTecn.Visible = false;
            btnActCorreoContSelec.Visible = false;
            btnActCorreoRecordatorio.Visible = true;
            btnActTipo.Visible = false;

            btnConfiguracionTecnica.CssClass = "subOpcion";
            btnConfiguracionCorreo.CssClass = "subOpcion";
            btnContenidCorreoSelec.CssClass = "subOpcion";
            btnContenidCorreo.CssClass = "subOpcion";
            btnCorreoRecordatorio.CssClass = "subOpcion subOpcionSeleccionado";
            btnTiposCorreo.CssClass = "subOpcion";

            divCorreoConfTec.Visible = false;
            divCorreoCont.Visible = false;
            divCorreoContSelec.Visible = false;
            divCorreoConfCorreo.Visible = false;
            divCorreoRecordatorio.Visible = true;
            btnPreview.Visible = true;

            divTiposCorreo.Visible = false;
            txtBusqTipo.Visible = false;
            imgBusqTipo.Visible = false;

            CambiaTitulo("Configuración de correo recordatorio");
            Session.Add("TipoPreview", "recordatorio");
        }

        protected void txtBusqTipoC_TextChanged(object sender, EventArgs e)
        {
            String condicion = txtBusqTipo.Text.Trim().ToUpper();

            if (condicion.Length > 0)
            {
                condicion = " (Nombre_tipo like '%" + condicion + "%')";
            }

            DataSet ds = connMysql.TraerTipoCorreo(condicion);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTiposCorreo.DataSource = ds.Tables[0];
                gvTiposCorreo.DataBind();
            }
            else
            {
                gvTiposCorreo.DataSource = ds.Tables[0];
                gvTiposCorreo.DataBind();

                int totalColumnas = ds.Tables[0].Columns.Count;

                if (gvTiposCorreo.Rows.Count == 0)
                {
                    DataTable dtTemporal = new DataTable();
                    dtTemporal.Columns.Add("ID_tipo");
                    dtTemporal.Columns.Add("NOMBRE_TIPO");
                    dtTemporal.Columns.Add("SUJETO");
                    dtTemporal.Columns.Add("Mensaje");
                    dtTemporal.Columns.Add("IMGCORREO");
                    dtTemporal.Columns.Add("FIRMA");
                    dtTemporal.NewRow();

                    DataRow drTemporal = dtTemporal.NewRow();
                    dtTemporal.Rows.InsertAt(drTemporal, 0);

                    gvTiposCorreo.DataSource = dtTemporal;
                    gvTiposCorreo.DataBind();
                }

                gvTiposCorreo.Rows[0].Cells.Clear();
                gvTiposCorreo.Rows[0].Cells.Add(new TableCell());
                gvTiposCorreo.Rows[0].Cells[0].ColumnSpan = 3;
                gvTiposCorreo.Rows[0].Cells[0].CssClass = "lblSinResultado";
                gvTiposCorreo.Rows[0].Cells[0].Text = "Sin resultados";

                gvTiposCorreo.Visible = true;
            }
        }
        public void CargarTipoCorreo(String condicion)
        {
            DataSet ds = connMysql.TraerTipoCorreo("");
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTiposCorreo.DataSource = ds.Tables[0];
                gvTiposCorreo.DataBind();
            }
            else
            {
                gvTiposCorreo.DataSource = ds.Tables[0];
                gvTiposCorreo.DataBind();

                int totalColumnas = ds.Tables[0].Columns.Count;

                if (gvTiposCorreo.Rows.Count == 0)
                {
                    DataTable dtTemporal = new DataTable();
                    dtTemporal.Columns.Add("ID_tipo");
                    dtTemporal.Columns.Add("NOMBRE_TIPO");
                    dtTemporal.Columns.Add("SUJETO");
                    dtTemporal.Columns.Add("Mensaje");
                    dtTemporal.Columns.Add("IMGCORREO");
                    dtTemporal.Columns.Add("FIRMA");
                    dtTemporal.NewRow();

                    DataRow drTemporal = dtTemporal.NewRow();
                    dtTemporal.Rows.InsertAt(drTemporal, 0);

                    gvTiposCorreo.DataSource = dtTemporal;
                    gvTiposCorreo.DataBind();
                }

                gvGerentes.Rows[0].Cells.Clear();
                gvGerentes.Rows[0].Cells.Add(new TableCell());
                gvGerentes.Rows[0].Cells[0].ColumnSpan = 6;
                gvGerentes.Rows[0].Cells[0].CssClass = "lblSinResultado";
                gvGerentes.Rows[0].Cells[0].Text = "Sin resultados";

                gvPerfiles.Visible = false;
                gvGerentes.Visible = true;

            }
        }
        protected void btnTiposCorreo_Click(object sender, EventArgs e)
        {
            btnActCorreoConf.Visible = false;
            btnActCorreoCont.Visible = false;
            btnActCorreoTecn.Visible = false;
            btnActCorreoContSelec.Visible = false;
            btnActCorreoRecordatorio.Visible = false;
            btnActTipo.Visible = true;
            txtBusqTipo.Visible = true;
            imgBusqTipo.Visible = true;

            btnConfiguracionTecnica.CssClass = "subOpcion";
            btnConfiguracionCorreo.CssClass = "subOpcion";
            btnContenidCorreoSelec.CssClass = "subOpcion";
            btnContenidCorreo.CssClass = "subOpcion";
            btnCorreoRecordatorio.CssClass = "subOpcion";
            btnTiposCorreo.CssClass = "subOpcion subOpcionSeleccionado";

            divCorreoConfTec.Visible = false;
            divCorreoCont.Visible = false;
            divCorreoContSelec.Visible = false;
            divCorreoConfCorreo.Visible = false;
            divCorreoRecordatorio.Visible = false;
            divTiposCorreo.Visible = true;
            btnPreview.Visible = false;

            CambiaTitulo("Tipos de correo");
            Session.Add("TipoPreview", "tipo");
        }
        protected void btnAgregarTipo_Click(object sender, EventArgs e)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;
            divFormularioP.Visible = false;
            divFormularioG.Visible = false;
            divFormCliente.Visible = false;
            divFormBusqCl.Visible = false;
            divConfirmaDeleteTipo.Visible = false;
            divFormTipo.Visible = true;

            btnLimpiaTipo.Visible = true;
            btnTipoAgregar.Visible = true;
            btnTipoModificar.Visible = false;
            btnLimpiaTipo.Visible = true;
            imgCorreoTipo.Visible = true;

            lblTipoTitulo.InnerText = "Agregar Tipo de Correo";
        }
        protected void btnTipoCerrar_Click(object sender, EventArgs e)
        {
            //Limpia campos
            txtAsuntoTipo.Text = "";
            txtTipo.Text = "";
            txtMsjTipo.Text = "";
            txtFirmaTipo.Text = "";

            //Habilita campos
            txtAsuntoTipo.Enabled = true;
            txtTipo.Enabled = true;
            txtMsjTipo.Enabled = true;
            txtFirmaTipo.Enabled = true;

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;

            divFormTipo.Visible = false;
        }
        protected void btnTipoLimpiar_Click(object sender, EventArgs e)
        {
            txtAsuntoTipo.Text = "";
            txtTipo.Text = "";
            txtMsjTipo.Text = "";
            txtFirmaTipo.Text = "";

        }
        protected void gvTipo_Consult(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            Label TipoCorreo = (Label)row.FindControl("lblTipo");
            Label Asunto = (Label)row.FindControl("lblSujeto");
            Label Mensaje = (Label)row.FindControl("lblMensaje");
            Label Firma = (Label)row.FindControl("lblFirma");
            Label Img = (Label)row.FindControl("lblImagen");

            Session["AsuntoTipo"] = Asunto.Text;
            Session["MensajeTipo"] = Mensaje.Text;
            Session["FirmaTipo"] = Firma.Text;
            Session["ImgTipo"] = Img.Text;


        }

        protected void gvTipo_Edit(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            Label IdTipo = (Label)row.FindControl("lblIdTipo");
            Label TipoCorreo = (Label)row.FindControl("lblTipo");
            Label Asunto = (Label)row.FindControl("lblSujeto");
            Label Mensaje = (Label)row.FindControl("lblMensaje");
            Label Firma = (Label)row.FindControl("lblFirma");
            Label Img = (Label)row.FindControl("lblImagen");

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;
            divFormularioP.Visible = false;
            divFormularioG.Visible = false;
            divFormCliente.Visible = false;
            divFormBusqCl.Visible = false;
            divConfirmaDeleteTipo.Visible = false;
            divFormTipo.Visible = true;

            txtTipo.Text = TipoCorreo.Text;
            txtAsuntoTipo.Text = Asunto.Text;
            lblIDTipo.Text = IdTipo.Text;
            txtMsjTipo.Text = Mensaje.Text;
            txtFirmaTipo.Text = Firma.Text;
            lblRutaImgTipo.Text = Img.Text;
            imgCorreoTipo.Visible = true;

            btnTipoAgregar.Visible = false;
            btnTipoModificar.Visible = true;
            btnLimpiaTipo.Visible = true;

            lblTipoTitulo.InnerText = "Actualizar Tipo de Correo";
        }

        protected void gvTipo_Delete(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            Label IDtipo = (Label)row.FindControl("lblIdTipo");
            Label TipoCorreo = (Label)row.FindControl("lblTipo");

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;
            divFormularioP.Visible = false;
            divFormularioG.Visible = false;
            divFormCliente.Visible = false;
            divFormBusqCl.Visible = false;
            divConfirmaDeleteTipo.Visible = true;
            divFormTipo.Visible = false;

            lblIDtipoDelete.Text = IDtipo.Text;
            lblMsjDeleteTipo.Text = "¿Está seguro que desea eliminar el tipo de correo\n'" + TipoCorreo.Text + "' ?";
        }
        protected void btnFormTipoAgregar_Click(object sender, EventArgs e)
        {
            String resultado = "";

            String Tipo = "";
            String Asunto = "";
            String Mensaje = "";
            String Firma = "";
            String ImgCorreo = "";

            if ((fuploadTipo.PostedFile.ContentLength / 1024) < 1024)
            {
                lblErrorImage.Text = "";

                Tipo = txtTipo.Text;
                Asunto = txtAsuntoTipo.Text;
                Mensaje = txtMsjTipo.Text;
                Firma = txtFirmaTipo.Text;

                if (fuploadTipo.FileName.Trim().Length != 0)
                {
                    if (fuploadTipo.HasFile)
                    {
                        string nombreArchivo = fuploadTipo.FileName;
                        string ruta = "Imagenes/Correo/" + nombreArchivo;
                        fuploadTipo.SaveAs(Server.MapPath(ruta));

                        ImgCorreo = ruta;
                    }
                }
                resultado = connMysql.GuardaTipoCorreo(Tipo, Asunto, Mensaje, ImgCorreo, Firma);


                sombraMensaje.Visible = true;
                mostrarMensaje((resultado.Trim().Equals("OK") ? "Tipo de correo guardado exitosamente" : resultado));

                txtAsuntoTipo.Text = "";
                txtTipo.Text = "";
                txtMsjTipo.Text = "";
                txtFirmaTipo.Text = "";

                gvTiposCorreo.EditIndex = -1;
                CargarTipoCorreo("");
            }
            else
            {
                lblErrorUploadTipo.Text = "* El tamaño de la imagen debe ser menor a 1 MB";
            }


        }

        protected void btnOKDeleteTipo_Click(object sender, EventArgs e)
        {
            String id_tipo = lblIDtipoDelete.Text;
            String resultado = "";

            resultado = connMysql.EliminaTipoCorreo(id_tipo);

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Tipo de correo eliminado exitosamente" : resultado));

            gvTiposCorreo.EditIndex = -1;
            CargarTipoCorreo("");
        }
        protected void btnCancelarDeleteTipo_Click(object sender, EventArgs e)
        {
            divConfirmaDeleteTipo.Visible = false;
            sombraMensaje.Visible = false;
        }
        protected void btnFormTipoModificar_Click(object sender, EventArgs e)
        {
            String resultado = "";

            String id_tipo = lblIDTipo.Text; bool CambiaImg = false;
            String Tipo = "";
            String Asunto = "";
            String Mensaje = "";
            String Firma = "";
            String ImgCorreo = "";

            if ((fuploadTipo.PostedFile.ContentLength / 1024) < 1024)
            {
                lblErrorImage.Text = "";

                Tipo = txtTipo.Text;
                Asunto = txtAsuntoTipo.Text;
                Mensaje = txtMsjTipo.Text;
                Firma = txtFirmaTipo.Text;

                if (fuploadTipo.FileName.Trim().Length != 0)
                {
                    if (fuploadTipo.HasFile)
                    {
                        string nombreArchivo = fuploadTipo.FileName;
                        string ruta = "Imagenes/Correo/" + nombreArchivo;
                        fuploadTipo.SaveAs(Server.MapPath(ruta));

                        ImgCorreo = ruta;
                    }
                }
                resultado = connMysql.ActualizaTipoCorreo(id_tipo, Tipo, Asunto, Mensaje, CambiaImg, ImgCorreo, Firma);


                sombraMensaje.Visible = true;
                mostrarMensaje((resultado.Trim().Equals("OK") ? "Tipo de correo guardado exitosamente" : resultado));
                txtAsuntoTipo.Text = "";
                txtTipo.Text = "";
                txtMsjTipo.Text = "";
                txtFirmaTipo.Text = "";
                lblIDTipo.Text = "";

                gvTiposCorreo.EditIndex = -1;
                CargarTipoCorreo("");
            }
            else
            {
                lblErrorUploadTipo.Text = "* El tamaño de la imagen debe ser menor a 1 MB";
            }



        }
    }
}