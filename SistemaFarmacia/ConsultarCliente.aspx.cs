﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SistemaFarmacia
{


    public partial class ConsultarCliente : System.Web.UI.Page
    {

        Conexion connMySql = new Conexion();
        String permisos = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MasterFarmacia master = (MasterFarmacia)this.Master;
                //String permisos = "";
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

                        master.mostrarMensaje(false);
                        sombraMensaje.Visible = false;

                        cargaClientes();
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
                DataSet ds = connMySql.TraerClientes(Session["Condicion"].ToString());

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

                //
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
                    //DataRowView drvFechaI = (DataRowView)e.Row.DataItem;
                    

                    
                    //if (etiquetaFechaI.Text.Trim().Length > 0)
                    //{
                    //    etiquetaFechaI.Text = etiquetaFechaI.Text.Split(' ')[0];
                    //}

                    //if (etiquetaFechaN.Text.Trim().Length > 0)
                    //{
                    //    etiquetaFechaN.Text = etiquetaFechaN.Text.Split(' ')[0];
                    //}





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

            FTitulo.Text = "Editar cliente";

            GridViewRow row = (GridViewRow)gvGerentes.Rows[e.NewEditIndex];

            Label IdCliente = (Label)row.FindControl("lblIdCliente");
            Label Nombre = (Label)row.FindControl("lblNombre");
            Label ApellidoP = (Label)row.FindControl("lblApellidoP");
            Label ApellidoM = (Label)row.FindControl("lblApellidoM");
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


            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;

            divFormularioG.Visible = true;

            TxtIdCliente.Text = IdCliente.Text;
            TxtNombre.Text = Nombre.Text;
            TxtApellidoP.Text = ApellidoP.Text;
            TxtApellidoM.Text = ApellidoM.Text;
            TxtMunicipio.Text = Municipio.Text;
            TxtEdad.Text = Edad.Text;

            try { 
                TxtFechaI.Text = FechaI.Text.Substring(6) + "-" + FechaI.Text.Substring(3,2) + "-" + FechaI.Text.Substring(0,2);
            }catch(Exception ex) { }

            TxtMedio.Text = Medio.Text;
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



            FGAgregar.Visible = false;
            FGActualizar.Visible = true;
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
            TxtMunicipio.Text = "";
            TxtEdad.Text = "";
            TxtFechaI.Text = "";
            TxtMedio.Text = "";
            TxtTelFijo.Text = "";
            TxtExtension.Text = "";
            TxtCelular.Text = "";
            TxtFechaN.Text = "";
            TxtEmail.Text = "";
            TxtObservaciones.Text = "";
            TxtNota.Text = "";


            gvGerentes.EditIndex = -1;
            cargaClientes();
        }

        protected void FGAgregar_Click(object sender, EventArgs e)
        {
            String resultado = "";

            String Nombre = TxtNombre.Text;
            String ApellidoP = TxtApellidoP.Text;
            String ApellidoM = TxtApellidoM.Text;
            String Municipio = TxtMunicipio.Text;
            String Edad = TxtEdad.Text;
            String FechaI = TxtFechaI.Text;
            String Medio = TxtMedio.Text;
            String TelFijo = TxtTelFijo.Text;
            String Extension = TxtExtension.Text;
            String Celular = TxtCelular.Text;
            String FechaN = TxtFechaN.Text;
            String Email = TxtEmail.Text;
            String Observaciones = TxtObservaciones.Text;
            String Nota = TxtNota.Text;
            //String Estatus = ddlEstatus.SelectedValue;


            resultado = connMySql.GuardaCliente(Nombre.ToUpper(), ApellidoP.ToUpper(), ApellidoM.ToUpper(), Edad.ToUpper(), FechaN.ToUpper(), FechaI.ToUpper(), Municipio.ToUpper(), TelFijo, Extension, Celular, Email, Observaciones.ToUpper(), Nota.ToUpper(), Medio.ToUpper());


            TxtIdCliente.Text = "";
            TxtNombre.Text = "";
            TxtApellidoP.Text = "";
            TxtApellidoM.Text = "";
            TxtMunicipio.Text = "";
            TxtEdad.Text = "";
            TxtFechaI.Text = "";
            TxtMedio.Text = "";
            TxtTelFijo.Text = "";
            TxtExtension.Text = "";
            TxtCelular.Text = "";
            TxtFechaN.Text = "";
            TxtEmail.Text = "";
            TxtObservaciones.Text = "";
            TxtNota.Text = "";

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


            FTitulo.Text = "Agregar cliente";

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;

            divFormularioG.Visible = true;

            FGActualizar.Visible = false;
            FGAgregar.Visible = true;
            TxtIdCliente.Text = "";
            TxtNombre.Text = "";
            TxtApellidoP.Text = "";
            TxtApellidoM.Text = "";
            TxtMunicipio.Text = "";
            TxtEdad.Text = "";
            TxtFechaI.Text = "";
            TxtMedio.Text = "";
            TxtTelFijo.Text = "";
            TxtExtension.Text = "";
            TxtCelular.Text = "";
            TxtFechaN.Text = "";
            TxtEmail.Text = "";
            TxtObservaciones.Text = "";
            TxtNota.Text = "";
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
            String resultado = connMySql.EliminaCliente(Id_Cliente.Text);

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Usuario eliminado exitosamente" : resultado));

            cargaClientes();

        }

        protected void FGActualizar_Click(object sender, EventArgs e)
        {
            String IDCliente = TxtIdCliente.Text;
            String Nombre = TxtNombre.Text;
            String ApellidoP = TxtApellidoP.Text;
            String ApellidoM = TxtApellidoM.Text;
            String Municipio = TxtMunicipio.Text;
            String Edad = TxtEdad.Text;
            String FechaI = TxtFechaI.Text;
            String Medio = TxtMedio.Text;
            String TelFijo = TxtTelFijo.Text;
            String Extension = TxtExtension.Text;
            String Celular = TxtCelular.Text;
            String FechaN = TxtFechaN.Text;
            String Email = TxtEmail.Text;
            String Observaciones = TxtObservaciones.Text;
            String Nota = TxtNota.Text;
            //String Estatus = ddlEstatus.SelectedValue;

            String resultado = connMySql.ActualizaCliente(IDCliente, Nombre, ApellidoP, ApellidoM, Edad, FechaN, FechaI, Municipio, TelFijo, Extension, Celular, Email, Observaciones, Nota, Medio);

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Cliente actualizado exitosamente" : resultado));

            TxtIdCliente.Text = "";
            TxtNombre.Text = "";
            TxtApellidoP.Text = "";
            TxtApellidoM.Text = "";
            TxtMunicipio.Text = "";
            TxtEdad.Text = "";
            TxtFechaI.Text = "";
            TxtMedio.Text = "";
            TxtTelFijo.Text = "";
            TxtExtension.Text = "";
            TxtCelular.Text = "";
            TxtFechaN.Text = "";
            TxtEmail.Text = "";
            TxtObservaciones.Text = "";
            TxtNota.Text = "";

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

                if (TxtMunicipio.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " municipio like '%" + TxtMunicipio.Text.Trim() + "%' ";
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

                if (TxtMedio.Text.Trim().Length > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " medio like '%" + TxtMedio.Text.Trim() + "%' ";
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



                Session["Condicion"] = condicion;
                cargaClientes();


                btnLimpiarF.Visible = false;
                btnBuscarF.Visible = false;
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
            TxtMunicipio.Text = "";
            TxtEdad.Text = "";
            TxtFechaI.Text = "";
            TxtMedio.Text = "";
            TxtTelFijo.Text = "";
            TxtExtension.Text = "";
            TxtCelular.Text = "";
            TxtFechaN.Text = "";
            TxtEmail.Text = "";
            TxtObservaciones.Text = "";
            TxtNota.Text = "";

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;

            btnLimpiarF.Visible = false;
            btnBuscarF.Visible = false;
            FGCancelar.Visible = true;
            FGAgregar.Visible = true;
            FGActualizar.Visible = true;
            divFormularioG.Visible = false;
            Session["Condicion"] = "";
            cargaClientes();
            }
        }

        //Muestra la ventana del buscador
        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            panelMsj.DefaultButton = btnBuscarF.ID;

            btnLimpiarF.Visible = true;
            btnBuscarF.Visible = true;
            FGCancelar.Visible = false;
            FGAgregar.Visible = false;
            FGActualizar.Visible = false;
            divFormularioG.Visible = true;

            FTitulo.Text = "Buscar clientes";



            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;

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
        
    }
}