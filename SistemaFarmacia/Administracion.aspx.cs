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

        protected void Page_Load(object sender, EventArgs e)
        {
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

                btnOkSalir.Visible = true;
                MOk.Visible = false;
            }
            
            MasterFarmacia master = (MasterFarmacia)this.Master;
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

                btnOpcionUsuario.CssClass = "seleccionado";
                btnOpcionPerfil.CssClass = "";
                btnOpcionCorreo.CssClass = "";
                cargaGerentes();

                btnOpcionUsuario.Visible = false;
                btnOpcionPerfil.Visible = false;
                btnOpcionCorreo.Visible = false;

                String PrimeraOpcion = "";

                if (permisos.Contains("31") || permisos.Contains("32") || permisos.Contains("33") || permisos.Contains("34"))
                {
                    PrimeraOpcion = "Usuario";
                    btnOpcionUsuario.Visible = true;
                    if (!permisos.Contains("32"))
                    {
                        btnAgrUsuarioG.Visible = false;
                    }
                }

                if(permisos.Contains("35") || permisos.Contains("36") || permisos.Contains("37") || permisos.Contains("38"))
                {
                    if(PrimeraOpcion.Length == 0)
                    {
                        PrimeraOpcion = "Perfil";
                    }
                    btnOpcionPerfil.Visible = true;

                    if (!permisos.Contains("36"))
                    {
                        btnAgregarP.Visible = false;
                    }
                }

                if(permisos.Contains("39") || permisos.Contains("310") || permisos.Contains("311"))
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

                    default:
                        break;
                }
            }
        }

        protected void btnOpcionUsuario_Click(object sender, EventArgs e)
        {
            divGerentes.Visible = true;
            divPerfiles.Visible = false;
            divCorreo.Visible = false;

            btnOpcionUsuario.CssClass = "seleccionado";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "";

            cargaGerentes();
            ocultarOpcionesCorreo();
        }

        protected void btnOpcionPerfil_Click(object sender, EventArgs e)
        {
            divGerentes.Visible = false;
            divPerfiles.Visible = true;
            divCorreo.Visible = false;

            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "seleccionado";
            btnOpcionCorreo.CssClass = "";

            cargaPerfiles();
            ocultarOpcionesCorreo();
        }
        
        protected void btnOpcionCorreo_Click(object sender, EventArgs e)
        {
            divGerentes.Visible = false;
            divPerfiles.Visible = false;
            divCorreo.Visible = true;

            btnOpcionUsuario.CssClass = "";
            btnOpcionPerfil.CssClass = "";
            btnOpcionCorreo.CssClass = "seleccionado";

            btnConfiguracionCorreo.Visible = false;
            btnConfiguracionTecnica.Visible = false;
            btnContenidCorreo.Visible = false;
            btnPreview.Visible = false;

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
                btnPreview.Visible = true;
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

        
    }
}