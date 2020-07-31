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


    public partial class ConsultarCliente : System.Web.UI.Page
    {

        Conexion connMySql = new Conexion();
        String permisos = "";
        ListItem lITodos = new ListItem("Todos", "-1");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MasterFarmacia master = (MasterFarmacia)this.Master;

                try
                {
                    permisos = Session["Permisos"].ToString();
                }
                catch (Exception ex)
                {
                    sombraMensaje.Visible = true;
                    mostrarMensaje("Su sesión ha caducado, vuelva a hacer login.");
                    btnOkSalir.Visible = true;
                    MOk.Visible = false;
                    return;
                }


                if (SesionViva())
                {
                    if (!IsPostBack)
                    {

                        if (permisos.Contains("11") || permisos.Contains("12") || permisos.Contains("13") || permisos.Contains("14"))
                        {
                            if (!permisos.Contains("11"))
                            {
                                btnAgrClienteG.Visible = false;
                            }
                        }
                        else
                        {
                            Response.Redirect("Principal.aspx");
                        }

                        if (Session["Condicion"] == null)
                        {
                            Session.Add("Condicion", "");
                        }
                        else
                        {
                            Session["Condicion"] = "";
                        }

                        if (Session["Orden"] == null)
                        {
                            Session.Add("Orden", "Nombre ASC");
                        }
                        else
                        {
                            Session["Orden"] = "Nombre ASC";
                        }

                        if(Session["EstadoGeneral"] == null)
                        {
                            Session.Add("EstadoGeneral", "1");
                        }else
                        {
                            Session["EstadoGeneral"] = "1";
                        }
                        ddlEstatusGeneral.SelectedValue = "1";


                        master.mostrarMensaje(false);
                        sombraMensaje.Visible = false;

                        llenarDropDownDlist();
                        cargaClientes();
                        llenaEstados();
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



        public void cargaClientes()
        {
            if (SesionViva())
            {
                DataSet ds = connMySql.TraerClientes(Session["Condicion"].ToString(), Session["EstadoGeneral"].ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].DefaultView.Sort = Session["Orden"].ToString();
                    
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
                        dtTemporal.NewRow();
                        DataRow drTemporal = dtTemporal.NewRow();
                        dtTemporal.Rows.InsertAt(drTemporal, 0);

                        gvGerentes.DataSource = dtTemporal;
                        gvGerentes.DataBind();
                    }

                    gvGerentes.Rows[0].Cells.Clear();
                    gvGerentes.Rows[0].Cells.Add(new TableCell());
                    gvGerentes.Rows[0].Cells[0].ColumnSpan = 15;
                    gvGerentes.Rows[0].Cells[0].CssClass = "lblSinResultado";
                    gvGerentes.Rows[0].Cells[0].Text = "Sin resultados";

                    gvGerentes.Visible = true;

                }
            }
        }


        protected void gvGerentes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (SesionViva())
            {
                //string imgOrd = @" <img src='Imagenes\IndicadorOrden.png' title='Ascending' style='float: left; height: 9px; margin-top: 1px;' />";
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        try
                        {
                            LinkButton lbSort = (LinkButton)cell.Controls[0];
                            DataTable dttempora = (DataTable)gvGerentes.DataSource;
                            if (dttempora.DefaultView.Sort == "")
                            {
                                if (lbSort.Text == "Nombre")
                                {
                                    //lbSort.Text = imgOrd + lbSort.Text;
                                    lbSort.CssClass = "Seleccionada";
                                }
                            }
                            else
                            {
                                if (lbSort.CommandArgument == dttempora.DefaultView.Sort.Substring(0, dttempora.DefaultView.Sort.IndexOf(" ")))
                                {
                                    //lbSort.Text = imgOrd + lbSort.Text;
                                    lbSort.CssClass = "Seleccionada";
                                }
                            }

                        }
                        catch
                        {

                        }

                    }
                }


                bool editar = false;
                bool eliminar = false;

                if (Session["Permisos"].ToString().Contains("12"))
                {
                    editar = true;
                }

                if (Session["Permisos"].ToString().Contains("13"))
                {
                    eliminar = true;
                }


                if (!editar && !eliminar)
                {
                    if (e.Row.RowType != DataControlRowType.Pager)
                    {
                        e.Row.Cells[15].Visible = false;
                    }
                }
                
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    
                    Label etiquetaFechaI = (Label)e.Row.FindControl("lblFechaI");
                    Label etiquetaFechaN = (Label)e.Row.FindControl("lblFechaN");

                    try{
                        etiquetaFechaN.Text =  ((DateTime)((DataRowView)e.Row.DataItem).Row.ItemArray[5]).ToString("dd/MM/yyyy");
                    }
                    catch { }

                    try
                    {
                        etiquetaFechaI.Text = ((DateTime)((DataRowView)e.Row.DataItem).Row.ItemArray[6]).ToString("dd/MM/yyyy");
                    }
                    catch { }

                    


                    if (!editar)
                    {
                        e.Row.Cells[15].Controls[0].Visible = false;
                    }

                    try
                    {
                        if (!eliminar)
                        {
                            

                            e.Row.Cells[15].Controls[2].Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        protected void gvGerentes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            panelMsj.DefaultButton = FGActualizar.ID;
            divObservacionesNota.Visible = true;

            btnCerrarF.Visible = false;
            btnBuscarF.Visible = false;
            btnLimpiarF.Visible = false;
            FGCancelar.Visible = true;
            FGActualizar.Visible = true;
            FGAgregar.Visible = false;


            
            if (ddlEstatus.Items.IndexOf(lITodos) == 0)
            {
                ddlEstatus.Items.RemoveAt(0);
            }

            FTitulo.Text = "Editar cliente";

            GridViewRow row = (GridViewRow)gvGerentes.Rows[e.NewEditIndex];

            Label IdCliente = (Label)row.FindControl("lblIdCliente");
            Label Nombre = (Label)row.FindControl("lblNombre");
            Label ApellidoP = (Label)row.FindControl("lblApellidoP");
            Label ApellidoM = (Label)row.FindControl("lblApellidoM");
            Label Estado = (Label)row.FindControl("lblEstado");
            Label Municipio = (Label)row.FindControl("lblMunicipio");
            Label Edad = (Label)row.FindControl("lblEdad");
            Label FechaI = (Label)row.FindControl("lblFechaI");
            Label Medio = (Label)row.FindControl("lblMedio");
            Label TelFijo = (Label)row.FindControl("lblTelFijo");
            Label Extension = (Label)row.FindControl("lblExtension");
            Label Celular = (Label)row.FindControl("lblCelular");
            Label FechaN = (Label)row.FindControl("lblFechaN");
            Label Email = (Label)row.FindControl("lblEmail");
            Label Observaciones = (Label)row.FindControl("lblObservaciones");
            Label Nota = (Label)row.FindControl("lblNota");
            Label Estatus = (Label)row.FindControl("lblEstatus");
            


            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;

            divFormularioG.Visible = true;

            TxtIdCliente.Text = IdCliente.Text;
            TxtNombre.Text = Nombre.Text;
            TxtApellidoP.Text = ApellidoP.Text;
            TxtApellidoM.Text = ApellidoM.Text;
            try
            {
                ddlEstado.SelectedValue = connMySql.traerIdEstado(Estado.Text.Trim());
                llenaMunicipio(ddlEstado.SelectedValue);
            }
            catch { }

            try
            {
                ddlMunicipio.SelectedValue = Municipio.Text.Trim();
            }
            catch { }




            TxtEdad.Text = Edad.Text;

            try { 
                TxtFechaI.Text = FechaI.Text.Substring(6) + "-" + FechaI.Text.Substring(3,2) + "-" + FechaI.Text.Substring(0,2);
            }catch(Exception ex) { }


            try
            {
                ddlMedio.SelectedValue = Medio.Text;
            }
            catch { }

            TxtTelFijo.Text = TelFijo.Text;
            TxtExtension.Text = Extension.Text;
            TxtCelular.Text = Celular.Text;

            try { 
                TxtFechaN.Text = FechaN.Text.Substring(6) + "-" + FechaN.Text.Substring(3, 2) + "-" + FechaN.Text.Substring(0, 2); ;
            }catch(Exception ex)
            {}

            TxtEmail.Text = Email.Text;
            TxtObservaciones.Text = Observaciones.Text;
            TxtNota.Text = Nota.Text;
            ddlEstatus.SelectedIndex = (Estatus.Text == "Activo" ? 0: 1) ;


        }


        protected void gvGerentes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvGerentes.EditIndex = -1;
            cargaClientes();
        }



        protected void FGCancelar_Click(object sender, EventArgs e)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;

            divFormularioG.Visible = false;

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
            ddlEstatus.SelectedIndex = 0;

            gvGerentes.EditIndex = -1;
            cargaClientes();
        }

        protected void FGAgregar_Click(object sender, EventArgs e)
        {
            String resultado = "";

            String Nombre = TxtNombre.Text;
            String ApellidoP = TxtApellidoP.Text;
            String ApellidoM = TxtApellidoM.Text;
            String Estado = (ddlEstado.SelectedIndex < 1?"": ddlEstado.Items[ddlEstado.SelectedIndex].Text);
            String Municipio = (ddlMunicipio.SelectedIndex < 1 ? "" : ddlMunicipio.Items[ddlMunicipio.SelectedIndex].Text);
            String Edad = TxtEdad.Text;
            String FechaI = TxtFechaI.Text;
            String Medio = ddlMedio.SelectedValue;
            String TelFijo = TxtTelFijo.Text;
            String Extension = TxtExtension.Text;
            String Celular = TxtCelular.Text;
            String FechaN = TxtFechaN.Text;
            String Email = TxtEmail.Text;
            String Observaciones = TxtObservaciones.Text;
            String Nota = TxtNota.Text;
            String Estatus = ddlEstatus.SelectedValue;


            resultado = connMySql.GuardaCliente(Nombre.ToUpper(), ApellidoP.ToUpper(), ApellidoM.ToUpper(), Edad.ToUpper(), FechaN.ToUpper(), FechaI.ToUpper(), Municipio.ToUpper(), TelFijo, Extension, Celular, Email, Observaciones.ToUpper(), Nota.ToUpper(), (Medio.ToUpper() == "0"? "": Medio.ToUpper()), Estatus,Estado.ToUpper());



            //Limpia las opciones
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
            ddlEstatus.SelectedIndex = 0;

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Usuario guardado exitosamente" : resultado));


            gvGerentes.EditIndex = -1;
            cargaClientes();
        }



        public void mostrarMensaje(String mensaje)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);

            sombraMensaje.Visible = true;
            divFormularioG.Visible = false;
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
        protected void btnOkSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnAgrClienteG_Click(object sender, EventArgs e)
        {
            panelMsj.DefaultButton = FGAgregar.ID;


            if (ddlEstatus.Items.IndexOf(lITodos) == 0)
            {
                ddlEstatus.Items.RemoveAt(0);
            }



            llenaEstados();



            FTitulo.Text = "Agregar cliente";

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;

            divFormularioG.Visible = true;

            //habilita botones
            btnBuscarF.Visible = false;
            btnLimpiarF.Visible = false;
            btnCerrarF.Visible = false;
            FGCancelar.Visible = true;
            FGActualizar.Visible = false;
            FGAgregar.Visible = true;

            //Limpia las opciones
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
            ddlEstatus.SelectedIndex = 0;

            divObservacionesNota.Visible = true;

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
            Label Id_Cliente = (Label)row.FindControl("lblIdCliente");
            String IdClienterefinado = Id_Cliente.Text;
            recupera.Text = IdClienterefinado.ToString();

            sombraMensaje.Visible = true;
            mostrarMensaje2();


            //GridViewRow row = (GridViewRow)gvGerentes.Rows[e.RowIndex];
            //Label Id_Cliente = (Label)row.FindControl("lblIdCliente");
            //String resultado = connMySql.EliminaCliente(Id_Cliente.Text);

            //sombraMensaje.Visible = true;
            //mostrarMensaje((resultado.Trim().Equals("OK") ? "Usuario eliminado exitosamente" : resultado));

            //cargaClientes();

        }

        protected void FGActualizar_Click(object sender, EventArgs e)
        {
            String IDCliente = TxtIdCliente.Text;
            String Nombre = TxtNombre.Text;
            String ApellidoP = TxtApellidoP.Text;
            String ApellidoM = TxtApellidoM.Text;
            String Estado = (ddlEstado.SelectedIndex < 1 ? "" : ddlEstado.Items[ddlEstado.SelectedIndex].Text);
            String Municipio = (ddlMunicipio.SelectedIndex < 1 ? "" : ddlMunicipio.Items[ddlMunicipio.SelectedIndex].Text);


            String Edad = TxtEdad.Text;
            String FechaI = TxtFechaI.Text;
            String Medio = ddlMedio.SelectedValue;
            String TelFijo = TxtTelFijo.Text;
            String Extension = TxtExtension.Text;
            String Celular = TxtCelular.Text;
            String FechaN = TxtFechaN.Text;
            String Email = TxtEmail.Text;
            String Observaciones = TxtObservaciones.Text;
            String Nota = TxtNota.Text;
            String Estatus = ddlEstatus.SelectedValue;

            String resultado = connMySql.ActualizaCliente(IDCliente, Nombre, ApellidoP, ApellidoM, Edad, FechaN, FechaI, Municipio, TelFijo, Extension, Celular, Email, Observaciones, Nota, (Medio == "0"? "": Medio), Estatus, Estado);

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Cliente actualizado exitosamente" : resultado));

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
            ddlEstatus.SelectedIndex = 0;

            gvGerentes.EditIndex = -1;
            cargaClientes();
        }

        protected void btnBuscarF_Click(object sender, EventArgs e)
        {
            if (SesionViva())
            {
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

                if(ddlEstado.SelectedIndex > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " Estado like '%" + ddlEstado.Items[ddlEstado.SelectedIndex].Text.Trim() + "%' ";
                    if(ddlMunicipio.SelectedIndex > 0)
                    {
                        condicion += (condicion.Length > 0 ? " and " : "") + " Municipio like '%" + ddlMunicipio.Items[ddlMunicipio.SelectedIndex].Text.Trim() + "%' ";
                    }
                }

                
                if (TxtFechaN.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " fecha_nacimiento like '%" + TxtFechaN.Text.Trim() + "%' ";
                }

                if (TxtFechaN.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " fecha_nacimiento like '%" + TxtFechaN.Text.Trim() + "%' ";
                }

                if (TxtEdad.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " edad like '%" + TxtEdad.Text.Trim() + "%' ";
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

                if(ddlEstatus.SelectedValue == "0" || ddlEstatus.SelectedValue == "1")
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " estatus LIKE '%" + ddlEstatus.SelectedValue + "%' ";
                }


                Session["Condicion"] = condicion;
                cargaClientes();


                btnLimpiarF.Visible = false;
                btnBuscarF.Visible = false;
                btnCerrarF.Visible = false;
                FGCancelar.Visible = true;
                FGAgregar.Visible = true;
                FGActualizar.Visible = true;
                divFormularioG.Visible = false;

                MasterFarmacia master = (MasterFarmacia)this.Master;
                master.mostrarMensaje(false);
                sombraMensaje.Visible = false;
            }
        }

        protected void btnLimpiarF_Click(object sender, EventArgs e)
        {
            if (SesionViva()) { 

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
                ddlEstatus.SelectedIndex = 0;
                
            }
        }

        //Muestra la ventana del buscador
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (SesionViva()) {

                panelMsj.DefaultButton = btnBuscarF.ID;

                divObservacionesNota.Visible = false;





                if (ddlEstatus.Items.IndexOf(lITodos) == -1)
                {
                    ddlEstatus.Items.Insert(0, lITodos);
                    ddlEstatus.SelectedIndex = 0;
                }

                

                btnLimpiarF.Visible = true;
                btnBuscarF.Visible = true;
                btnCerrarF.Visible = true;
                FGCancelar.Visible = false;
                FGAgregar.Visible = false;
                FGActualizar.Visible = false;
                divFormularioG.Visible = true;

                FTitulo.Text = "Buscar clientes";



                MasterFarmacia master = (MasterFarmacia)this.Master;
                master.mostrarMensaje(true);
                sombraMensaje.Visible = true;
            }

        }

        protected void gvGerentes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvGerentes.PageIndex = e.NewPageIndex;
            cargaClientes();

        }

        protected void gvGerentes_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SesionViva()) { 
            if (Session["Orden"].ToString() == e.SortExpression + " " + "ASC" )
            {
                Session["Orden"] = e.SortExpression + " " + "DESC";
            }
            else
            {
                Session["Orden"] = e.SortExpression + " " + "ASC";
            }
            cargaClientes();
            }
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
        

        protected void btnCerrarF_Click(object sender, EventArgs e)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;

            divFormularioG.Visible = false;

        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            if (SesionViva())
            {
                TxtIdCliente.Text = "";
                TxtNombre.Text = "";
                TxtApellidoP.Text = "";
                TxtApellidoM.Text = "";
                ddlEstado.SelectedIndex = -1;
                ddlMunicipio.SelectedIndex = -1;
                TxtEdad.Text = "";
                TxtFechaI.Text = "";
                TxtTelFijo.Text = "";
                TxtExtension.Text = "";
                TxtCelular.Text = "";
                TxtFechaN.Text = "";
                TxtEmail.Text = "";
                TxtObservaciones.Text = "";
                TxtNota.Text = "";
                ddlEstatus.SelectedIndex = 0;


                Session["Condicion"] = "";
                cargaClientes();
            }
        }


        #region medio
        protected void llenarDropDownDlist()
        {
            DataSet ds = connMySql.consultarmedios();
            ddlMedio.DataSource = ds;
            ddlMedio.DataTextField = "MEDIO";
            ddlMedio.DataValueField = "MEDIO";
            ddlMedio.DataBind();

            ddlMedio.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }
        #endregion

        protected void ddlMunicipio_TextChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaMunicipio(ddlEstado.SelectedValue);
        }

        public void llenaMunicipio(String Estado)
        {
            ddlMunicipio.Enabled = true;
            ddlMunicipio.DataTextField = "MUNICIPIO";
            ddlMunicipio.DataValueField = "MUNICIPIO";
            ddlMunicipio.DataSource = connMySql.traerMunicipio(Estado);
            ddlMunicipio.DataBind();
            ddlMunicipio.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        public void llenaEstados()
        {
            ddlEstado.DataTextField = "estado";
            ddlEstado.DataValueField = "ID";
            ddlEstado.DataSource = connMySql.traerEstado();
            ddlEstado.DataBind();

            ddlEstado.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }

        protected void ddlEstatusGeneral_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SesionViva())
            {
                Session["EstadoGeneral"] = ddlEstatusGeneral.SelectedValue;
                cargaClientes();
            }
        }





        public void mostrarMensaje2()
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;
            divFormularioG.Visible = false;
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

            String resultado = connMySql.EliminaCliente(idcliente);

            sombraMensaje.Visible = true;
            divmensaje2.Visible = false;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Usuario eliminado exitosamente" : resultado));

            cargaClientes();
        }
    }
}