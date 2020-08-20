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

                    btnOkSalir.Visible = true;
                    MOk.Visible = false;
                }

                if (SesionViva())
                {
                    if (!IsPostBack)
                    {

                        master.cambiarLblTitle("<img src='Imagenes/Administracion.png' alt='clientes'><h1>Administración</h1>");

                        if (!(permisos.Contains("31") || permisos.Contains("32") || permisos.Contains("33") || permisos.Contains("34") || permisos.Contains("35") || permisos.Contains("36") || permisos.Contains("37") || permisos.Contains("38") || permisos.Contains("39") || permisos.Contains("310") || permisos.Contains("311")))
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

                        btnOpcionUsuario.CssClass = "seleccionado";
                        btnOpcionPerfil.CssClass = "";
                        btnOpcionCorreo.CssClass = "";
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

                        if (permisos.Contains("39") || permisos.Contains("310") || permisos.Contains("311"))
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
                            llenarEstados();
                            btnEnvioCorreo.Visible = true;
                        }


                        switch (PrimeraOpcion)
                        {
                            case "Usuario":
                                btnOpcionUsuario.CssClass = "seleccionado";
                                break;

                            case "Perfil":
                                btnOpcionPerfil.CssClass = "seleccionado";
                                break;

                            case "Correo":
                                btnOpcionCorreo.CssClass = "seleccionado";
                                break;

                            case "EnvioCorreo":
                                btnEnvioCorreo.CssClass = "seleccionado";
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

            btnOpcionUsuario.CssClass = "seleccionado";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "";
            btnEnvioCorreo.CssClass = "";

            cargaGerentes();
            ocultarOpcionesCorreo();
        }

        protected void btnOpcionPerfil_Click(object sender, EventArgs e)
        {
            divGerentes.Visible = false;
            divPerfiles.Visible = true;
            divCorreo.Visible = false;
            divEnvioCorreo.Visible = false;

            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "seleccionado";
            btnOpcionCorreo.CssClass = "";
            btnEnvioCorreo.CssClass = "";

            cargaPerfiles();
            ocultarOpcionesCorreo();
        }
        
        protected void btnOpcionCorreo_Click(object sender, EventArgs e)
        {
            divGerentes.Visible = false;
            divPerfiles.Visible = false;
            divCorreo.Visible = true;
            divEnvioCorreo.Visible = false;

            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "seleccionado";
            btnEnvioCorreo.CssClass = "";

            btnConfiguracionCorreo.Visible = false;
            btnConfiguracionTecnica.Visible = false;
            btnContenidCorreo.Visible = false;

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
            
            if (permisos.Contains("39"))
            {
                btnConfiguracionTecnica.CssClass = "subOpcion subOpcionSeleccionado";
                btnConfiguracionCorreo.CssClass = "subOpcion";
                btnContenidCorreo.CssClass = "subOpcion";

                divCorreoConfTec.Visible = true;
                divCorreoConfCorreo.Visible = false;
                divCorreoCont.Visible = false;

                btnActCorreoTecn.Visible = true;
                btnActCorreoConf.Visible = false;
                btnActCorreoCont.Visible = false;
            }
            else
            {
                if (permisos.Contains("310"))
                {
                    btnConfiguracionTecnica.CssClass = "subOpcion";
                    btnConfiguracionCorreo.CssClass = "subOpcion subOpcionSeleccionado";
                    btnContenidCorreo.CssClass = "subOpcion";

                    divCorreoConfTec.Visible = false;
                    divCorreoConfCorreo.Visible = true;
                    divCorreoCont.Visible = false;

                    btnActCorreoTecn.Visible = false;
                    btnActCorreoConf.Visible = true;
                    btnActCorreoCont.Visible = false;
                }
                else
                {
                    if (permisos.Contains("311"))
                    {
                        btnConfiguracionTecnica.CssClass = "subOpcion";
                        btnConfiguracionCorreo.CssClass = "subOpcion";
                        btnContenidCorreo.CssClass = "subOpcion subOpcionSeleccionado";
                        
                        divCorreoConfTec.Visible = false;
                        divCorreoConfCorreo.Visible = false;
                        divCorreoCont.Visible = true;

                        btnActCorreoTecn.Visible = false;
                        btnActCorreoConf.Visible = false;
                        btnActCorreoCont.Visible = true;
                        btnPreview.Visible = true;
                    }
                }
            }
        }


        protected void btnEnvioCorreo_Click(object sender, EventArgs e)
        {
            divGerentes.Visible = false;
            divPerfiles.Visible = false;
            divCorreo.Visible = false;
            divEnvioCorreo.Visible = true;

            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "";
            btnEnvioCorreo.CssClass = "seleccionado";

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

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;
            divFormularioG.Visible = true;
            divFormularioP.Visible = false;
            divFormCliente.Visible = false;

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
            
            resultado = connMysql.GuardaUsuario(Nombre.ToUpper(), ApellidoP.ToUpper(), ApellidoM.ToUpper(), usuario.ToUpper(), Perfil, contrasenia, correo, preguntaS, respuestaS);

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
            
            bool CambioRespuesta = !txtRespuestaS.Text.Trim().Equals("");

            String resultado = connMysql.ActualizaUsuario(idUsuario, Nombre.ToUpper(), ApellidoP.ToUpper(), ApellidoM.ToUpper(), usuario.ToUpper(), Perfil, contrasenia, CambioContrasenia, CambioRespuesta, correo, preguntaS, respuestaS);

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
                        foreach (ListItem lsItem in chkPerforCorreo.Items)
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
            btnConfiguracionTecnica.CssClass = "subOpcion subOpcionSeleccionado";
            btnConfiguracionCorreo.CssClass = "subOpcion";
            btnContenidCorreo.CssClass = "subOpcion";

            divCorreoConfTec.Visible = true;
            divCorreoCont.Visible = false;
            divCorreoConfCorreo.Visible = false;
            btnPreview.Visible = false;
        }

        protected void btnConfiguracionCorreo_Click(object sender, EventArgs e)
        {
            btnActCorreoConf.Visible = true;
            btnActCorreoCont.Visible = false;
            btnActCorreoTecn.Visible = false;
            btnConfiguracionTecnica.CssClass = "subOpcion";
            btnConfiguracionCorreo.CssClass = "subOpcion subOpcionSeleccionado";
            btnContenidCorreo.CssClass = "subOpcion";

            divCorreoConfTec.Visible = false;
            divCorreoCont.Visible = false;
            divCorreoConfCorreo.Visible = true;
            btnPreview.Visible = false;
        }

        protected void btnContenidCorreo_Click(object sender, EventArgs e)
        {
            btnActCorreoConf.Visible = false;
            btnActCorreoCont.Visible = true;
            btnActCorreoTecn.Visible = false;

            btnConfiguracionTecnica.CssClass = "subOpcion";
            btnConfiguracionCorreo.CssClass = "subOpcion";
            btnContenidCorreo.CssClass = "subOpcion subOpcionSeleccionado";

            divCorreoConfTec.Visible = false;
            divCorreoCont.Visible = true;
            divCorreoConfCorreo.Visible = false;
            btnPreview.Visible = true;
        }

        public void ocultarOpcionesCorreo()
        {
            btnConfiguracionTecnica.Visible = false;
            btnConfiguracionCorreo.Visible = false;
            btnContenidCorreo.Visible = false;

            btnConfiguracionTecnica.CssClass = "subOpcion";
            btnConfiguracionCorreo.CssClass = "subOpcion";
            btnContenidCorreo.CssClass = "subOpcion";

            divCorreoConfTec.Visible = false;
            divCorreoCont.Visible = false;
            divCorreoConfCorreo.Visible = true;
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
        protected void llenarEstados()
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
            DatosCorreo dCorreo = connMysql.ConsultaDatosCorreo();

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
                //String direccionImagen = Server.MapPath("Imagenes/Correo/image.jpg");
                bool respuesta = enviaCorreo.Enviar(listaCorreos, Server.MapPath(dCorreo.SMTP_IMAGEN));

                if (respuesta)
                {
                    lblMensajeJS.Text = "Los correos se han enviado exitosamente";
                    //if (dCorreo.PRUEBAS == "0") { 
                    connMysql.ActualizaCorreoEnvCliente(cadenaIDs);
                    //}
                    connMysql.GuardaBitacora(Session["usuario"].ToString(), listaIDs, dCorreo);
                }
                else
                {
                    lblMensajeJS.Text = "Ocurrio un problema al enviar los correo, favor de intentarlo nuevamente.";
                }
            }

            //sombraJS.Visible = true;
            //MContenidoJS.Style.Remove("display");
            //divFormularioCorreo.Visible = false;
            //divFormCorreoCondicion.Visible = false;

            //CargaJS.Visible = false;
            //lblMensajeJS.Text = "Los correos se han enviado exitosamente";
            //divMensajeJS.Style.Add("Height", "125px");

            //cargaClientes();
        }



        #endregion
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

    }
}