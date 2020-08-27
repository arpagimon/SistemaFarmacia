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
                            Session.Add("Orden", "");
                        }
                        else
                        {
                            Session["Orden"] = "";
                        }

                        if (Session["EstadoGeneral"] == null)
                        {
                            Session.Add("EstadoGeneral", "1");
                        }
                        else
                        {
                            Session["EstadoGeneral"] = "1";
                        }

                        ddlEstatusGeneral.SelectedValue = "1";
                        
                        master.mostrarLblUser("<p>Usuario: " + this.Session["usuario"].ToString() + " </p>");
                        master.mostrarMensaje(false);
                        sombraMensaje.Visible = false;

                        llenarDropDownDlist();
                        cargaClientes();
                        llenaEstados();
                        llenaPaises();
                        llenaEstadosDFa();
                        llenaPaisesDFa();

                        master.cambiarLblTitle("<img src='Imagenes/CLIENTE.png' alt='clientes'><h1>Clientes</h1>");
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
                        dtTemporal.Columns.Add("req_factura");
                        dtTemporal.Columns.Add("rfc");
                        dtTemporal.Columns.Add("entidad");
                        dtTemporal.Columns.Add("dir_factura");
                        dtTemporal.Columns.Add("NombRazon_factura");
                        dtTemporal.NewRow();
                        DataRow drTemporal = dtTemporal.NewRow();
                        dtTemporal.Rows.InsertAt(drTemporal, 0);

                        gvGerentes.DataSource = dtTemporal;
                        gvGerentes.DataBind();
                    }

                    gvGerentes.Rows[0].Cells.Clear();
                    gvGerentes.Rows[0].Cells.Add(new TableCell());
                    gvGerentes.Rows[0].Cells[0].ColumnSpan = 18;
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
                    Label lblNumeroFila = (Label)e.Row.FindControl("lblindice");

                    try
                    {
                        lblNumeroFila.Text = ((e.Row.RowIndex + 1) + (gvGerentes.PageIndex * gvGerentes.PageSize)).ToString();
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


        protected void gvGerentes_RowEditing(object sender, EventArgs e)
        {
            panelMsj.DefaultButton = FGActualizar.ID;
            divObservacionesNota.Visible = true;
            TxtEdad.Enabled = false;

            btnCerrarF.Visible = false;
            btnBuscarF.Visible = false;
            btnLimpiarF.Visible = false;
            FGCancelar.Visible = true;
            FGActualizar.Visible = true;
            FGAgregar.Visible = false;

            ocultaRango();
            
            if (ddlEstatus.Items.IndexOf(lITodos) == 0)
            {
                ddlEstatus.Items.RemoveAt(0);
            }

            if (ddlEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1")) > -1)
            {
                ddlEnviarCorreo.Items.RemoveAt(ddlEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1")));
            }

            FTitulo.Text = "Editar cliente";

            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            Label IdCliente = (Label)row.FindControl("lblIdCliente");
            Label Nombre = (Label)row.FindControl("lblNombre");
            Label ApellidoP = (Label)row.FindControl("lblApellidoP");
            Label ApellidoM = (Label)row.FindControl("lblApellidoM");
            Label Estado = (Label)row.FindControl("lblEstado");
            Label Municipio = (Label)row.FindControl("lblMunicipio");
            Label Pais = (Label)row.FindControl("lblPais");

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
            Label Env_Correo = (Label)row.FindControl("lblEnvCorreo");
            Label R_Factura = (Label)row.FindControl("lblFactura");

            Label RFC = (Label)row.FindControl("lblRfc");
            Label Entidad = (Label)row.FindControl("lblEntidad");
            //Label DirFiscal = (Label)row.FindControl("lblDirFactura");
            Label NomRazon = (Label)row.FindControl("lblNomRazon");

            Label Calle = (Label)row.FindControl("lblCalleF");
            Label NoInterior = (Label)row.FindControl("lblNoInteriorF");
            Label NoExterior = (Label)row.FindControl("lblNoExteriorF");
            Label Colonia = (Label)row.FindControl("lblColoniaF");
            Label EstadoF = (Label)row.FindControl("lblEstadoF");
            Label MunicipioF = (Label)row.FindControl("lblMunicipioF");
            Label PaisF = (Label)row.FindControl("lblPaisF");
            Label CPF = (Label)row.FindControl("lblCPf");


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
                if (Estado.Text != "--OTRO--")
                {
                    divMunicipio.Visible = true;
                    divPais.Visible = false;
                    
                    llenaMunicipio(ddlEstado.SelectedValue);
                    try
                    {
                        ddlMunicipio.SelectedValue = Municipio.Text.Trim();
                    }
                    catch { }
                }
                else
                {
                    divMunicipio.Visible = false;
                    divPais.Visible = true;
                    try
                    {
                        ddlPais.SelectedValue = Pais.Text.Trim();
                        divPais.Visible = true;
                        divMunicipio.Visible = false;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch { }
            
            TxtEdad.Text = Edad.Text;

            try
            {
                TxtFechaI.Text = FechaI.Text.Substring(6) + "-" + FechaI.Text.Substring(3, 2) + "-" + FechaI.Text.Substring(0, 2);
            }
            catch (Exception ex) { }
            
            try
            {
                ddlMedio.SelectedValue = Medio.Text;
            }
            catch { }

            TxtTelFijo.Text = TelFijo.Text;
            TxtExtension.Text = Extension.Text;
            TxtCelular.Text = Celular.Text;

            try
            {
                TxtFechaN.Text = FechaN.Text.Substring(6) + "-" + FechaN.Text.Substring(3, 2) + "-" + FechaN.Text.Substring(0, 2); ;
            }
            catch (Exception ex)
            { }
            
            TxtEmail.Text = Email.Text;
            TxtObservaciones.Text = Observaciones.Text;
            TxtNota.Text = Nota.Text;
            ddlEstatus.SelectedIndex = (Estatus.Text == "Activo" ? 0 : 1);
            ddlFactura.SelectedIndex = (R_Factura.Text == "Sí" ? 0 : 1);

            //if (R_Factura.Text == "Sí")
            //{
            //    divDatosFactura.Visible = true;
            //}
            //txtNmR.Text = NomRazon.Text;
            //ddlEntidad.SelectedIndex = (Entidad.Text == "Persona moral" ? 0 : 1);
            //txtRfc.Text = RFC.Text;
            //txtDirFiscal.Text = DirFiscal.Text;

            //if (ddlEntidad.Items.IndexOf(new ListItem("Todos", "-1")) > -1)
            //{
            //    ddlEntidad.Items.RemoveAt(ddlEntidad.Items.IndexOf(new ListItem("Todos", "-1")));
            //}

            if (ddlFactura.Items.IndexOf(new ListItem("Todos", "-1")) > -1)
            {
                ddlFactura.Items.RemoveAt(ddlFactura.Items.IndexOf(new ListItem("Todos", "-1")));
            }

            if (R_Factura.Text == "Sí")
            {
                divBtnDatosF.Visible = true;
            }
            btnAddDatosF.Text = "<i class='fa fa-file-text-o' aria-hidden='true'></i><i class='fa fa-pencil' aria-hidden='true'></i>";
            btnAddDatosF.ToolTip = "Editar datos de facturación";
            txtNombRDFa.Text = NomRazon.Text;
            ddlEntidadDFa.SelectedIndex = (Entidad.Text == "Persona moral" ? 0 : 1);
            txtRfcDFa.Text = RFC.Text;
            txtCalleDFa.Text = Calle.Text;
            txtNoExtDFa.Text = NoExterior.Text;
            txtNoIntDFa.Text = NoInterior.Text;
            txtColoniaDFa.Text = Colonia.Text;
            txtCPdfA.Text = CPF.Text;

            try
            {
                ddlEstadoDFa.SelectedValue = connMySql.traerIdEstado(EstadoF.Text.Trim());
                if (EstadoF.Text != "--OTRO--")
                {
                    divMunicipioDFa.Visible = true;
                    divPaisDFa.Visible = false;

                    llenaMunicipioDFa(ddlEstadoDFa.SelectedValue);
                    try
                    {
                        ddlMunicipioDFa.SelectedValue = MunicipioF.Text.Trim();
                    }
                    catch { }
                }
                else
                {
                    divMunicipioDFa.Visible = false;
                    divPaisDFa.Visible = true;
                    try
                    {
                        ddlPaisDFa.SelectedValue = PaisF.Text.Trim();
                        divPaisDFa.Visible = true;
                        divMunicipioDFa.Visible = false;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch { }

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

            ddlPais.SelectedIndex = -1;
            divPais.Visible = false;
            divMunicipio.Visible = true;

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

            //ddlEntidad.SelectedIndex = 0;
            //txtDirFiscal.Text = "";
            ddlFactura.SelectedIndex = 1;
            //txtNmR.Text = "";
            //txtRfc.Text = "";
            //divDatosFactura.Visible = false;

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

            gvGerentes.EditIndex = -1;
            cargaClientes();

            HabilitaCampos();
        }

        protected void FGAgregar_Click(object sender, EventArgs e)
        {
            String resultado = "";
            Boolean pasa = true;

            String Nombre = TxtNombre.Text;
            if (Nombre.Trim().Length == 0)
            {
                TxtNombre.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                TxtNombre.Attributes.Remove("style");
            }

            String ApellidoP = TxtApellidoP.Text;
            if (ApellidoP.Trim().Length == 0)
            {
                TxtApellidoP.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                TxtApellidoP.Attributes.Remove("style");
            }

            String ApellidoM = TxtApellidoM.Text;
            if (ApellidoM.Trim().Length == 0)
            {
                TxtApellidoM.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                TxtApellidoM.Attributes.Remove("style");
            }

            String Estado = (ddlEstado.SelectedIndex < 1 ? "" : ddlEstado.Items[ddlEstado.SelectedIndex].Text);
            if (Estado.Trim().Length == 0)
            {
                ddlEstado.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlEstado.Attributes.Remove("style");
            }

            String Municipio = (ddlMunicipio.SelectedIndex < 1 ? "" : ddlMunicipio.Items[ddlMunicipio.SelectedIndex].Text);
            if ((Estado != "--OTRO--" ? (Municipio.Trim().Length == 0 || Municipio == "0") : false))
            {
                ddlMunicipio.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlMunicipio.Attributes.Remove("style");
            }

            String Edad = TxtEdad.Text;
            if (Edad.Trim().Length == 0)
            {
                TxtEdad.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                TxtEdad.Attributes.Remove("style");
            }

            String FechaI = TxtFechaI.Text;
            if (FechaI.Trim().Length == 0)
            {
                TxtFechaI.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                TxtFechaI.Attributes.Remove("style");
            }

            String Medio = ddlMedio.SelectedValue;
            if (Medio.Trim().Length == 0 || Medio.Equals("0"))
            {
                ddlMedio.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlMedio.Attributes.Remove("style");
            }

            String TelFijo = TxtTelFijo.Text;

            String Extension = TxtExtension.Text;
            String Celular = TxtCelular.Text;
            if (TelFijo.Trim().Length > 0 || Celular.Trim().Length > 0)
            {
                TxtTelFijo.Attributes.Remove("style");
                TxtCelular.Attributes.Remove("style");
            }
            else
            {
                TxtTelFijo.Attributes.Add("style", "border: 1px red solid;");
                TxtCelular.Attributes.Add("style", "border: 1px red solid;");
            }
            
            String FechaN = TxtFechaN.Text;
            if (FechaN.Trim().Length == 0)
            {
                TxtFechaN.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                TxtFechaN.Attributes.Remove("style");
            }

            String Enviar_Correo = ddlEnviarCorreo.SelectedValue;
            String Email = TxtEmail.Text;

            if (Enviar_Correo == "1" && Email.Trim().Length == 0)
            {
                TxtEmail.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                TxtEmail.Attributes.Remove("style");
            }

            String Observaciones = TxtObservaciones.Text;
            String Nota = TxtNota.Text;
            String Estatus = ddlEstatus.SelectedValue;
            String Pais = (ddlPais.SelectedIndex < 1 ? "" : ddlPais.Items[ddlPais.SelectedIndex].Text);
            if ((Estado == "--OTRO--" ? (Pais.Trim().Length == 0 || Pais == "0") : false))
            {
                ddlPais.Attributes.Add("style", "border: 1px red solid;");
                pasa = false;
            }
            else
            {
                ddlPais.Attributes.Remove("style");
            }

            String Req_Factura = ddlFactura.SelectedValue;

            String RFC = "";
            String Entidad = "";
            String Dir_Factura = "";
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
                //if (NoIntF.Trim().Length == 0)
                //{
                //    txtNoIntDFa.Attributes.Add("style", "border: 1px red solid;");
                //    pasaFactura = false;
                //}
                //else
                //{
                //    txtNoIntDFa.Attributes.Remove("style");
                //}
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
                PaisF = (ddlPaisDFa.SelectedIndex < 1 ? "" : ddlPaisDFa.Items[ddlPais.SelectedIndex].Text);
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
                lblError.Text = "";
                resultado = connMySql.GuardaCliente(Nombre.ToUpper(), ApellidoP.ToUpper(), ApellidoM.ToUpper(), Edad, FechaN, FechaI, Municipio, TelFijo, Extension, Celular, Email, Observaciones, Nota, (Medio == "0" ? "" : Medio), Estatus, Estado, Pais, Enviar_Correo, connMySql.traerIDEmpleado(Session["usuario"].ToString()), Req_Factura, RFC, Entidad, CalleF, NoIntF, NoExtF, ColoniaF, CpF, EstadoF, MunicipioF, PaisF, NomRazon);

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
                ddlPais.SelectedIndex = -1;
                divPais.Visible = false;
                divMunicipio.Visible = true;

                //ddlEntidad.SelectedIndex = 0;
                //txtDirFiscal.Text = "";
                //txtRfc.Text = "";
                //txtNmR.Text = "";

                //divDatosFactura.Visible = false;
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

                sombraMensaje.Visible = true;
                mostrarMensaje((resultado.Trim().Equals("OK") ? "Cliente guardado exitosamente" : resultado));
                
                gvGerentes.EditIndex = -1;
                cargaClientes();
            }
            else
            {
                lblError.Text = "Favor de llenar los campos faltantes";
            }
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
            lblError.Text = "";
            TxtEdad.Enabled = false;
            panelMsj.DefaultButton = FGAgregar.ID;
            
            if (ddlEstatus.Items.IndexOf(lITodos) == 0)
            {
                ddlEstatus.Items.RemoveAt(0);
            }

            ocultaRango();
            llenaEstados();
            llenaEstadosDFa();


            if (ddlEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1")) > 0)
            {
                ddlEnviarCorreo.Items.RemoveAt(ddlEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1")));
            }

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
            ddlPais.SelectedIndex = -1;
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

            ddlFactura.SelectedIndex = 1;
            //ddlEntidad.SelectedIndex = 0;
            //txtRfc.Text = "";
            //txtDirFiscal.Text = "";
            //txtNmR.Text = "";
            //divDatosFactura.Visible = false;

            //if (ddlEntidad.Items.IndexOf(new ListItem("Todos", "-1")) > 0)
            //{
            //    ddlEntidad.Items.RemoveAt(ddlEntidad.Items.IndexOf(new ListItem("Todos", "-1")));
            //}

            btnAddDatosF.Text = "<i class='fa fa-file-text-o' aria-hidden='true'></i><i class='fa fa-plus' aria-hidden='true'></i>";
            btnAddDatosF.ToolTip = "Editar datos de facturación";
            txtNombRDFa.Text = "";
            txtRfcDFa.Text = "";
            ddlEntidadDFa.SelectedIndex = 1;
            txtCalleDFa.Text = "";
            txtNoIntDFa.Text = "";
            txtNoExtDFa.Text = "";
            txtColoniaDFa.Text = "";
            ddlEstadoDFa.SelectedIndex = -1;
            ddlMunicipioDFa.SelectedIndex = -1;
            ddlPaisDFa.SelectedIndex = -1;
            divPaisDFa.Visible = false;
            divMunicipioDFa.Visible = true;
            txtCPdfA.Text = "";

            if (ddlFactura.Items.IndexOf(new ListItem("Todos", "-1")) > 0)
            {
                ddlFactura.Items.RemoveAt(ddlFactura.Items.IndexOf(new ListItem("Todos", "-1")));
            }

            divObservacionesNota.Visible = true;
        }


        protected void gvGerentes_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
        }

        protected void gvGerentes_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
        }


        protected void gvGerentes_RowDeleting(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            Label Id_Cliente = (Label)row.FindControl("lblIdCliente");
            String IdClienterefinado = Id_Cliente.Text;
            recupera.Text = IdClienterefinado.ToString();

            sombraMensaje.Visible = true;
            mostrarMensaje2(((Label)row.FindControl("lblNombre")).Text + " " + ((Label)row.FindControl("lblApellidoP")).Text + " " + ((Label)row.FindControl("lblApellidoM")).Text + "?");
        }

        protected void FGActualizar_Click(object sender, EventArgs e)
        {
            bool Pasa = true;

            String IDCliente = TxtIdCliente.Text;
            String Nombre = TxtNombre.Text;
            if (Nombre.Trim().Length == 0)
            {
                TxtNombre.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                TxtNombre.Attributes.Remove("Style");
            }

            String ApellidoP = TxtApellidoP.Text;
            if (ApellidoP.Trim().Length == 0)
            {
                TxtApellidoP.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                TxtApellidoP.Attributes.Remove("Style");
            }

            String ApellidoM = TxtApellidoM.Text;
            if (ApellidoM.Trim().Length == 0)
            {
                TxtApellidoM.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                TxtApellidoM.Attributes.Remove("Style");
            }

            String Estado = (ddlEstado.SelectedIndex < 1 ? "" : ddlEstado.Items[ddlEstado.SelectedIndex].Text);
            if (Estado.Trim().Length == 0)
            {
                ddlEstado.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                ddlEstado.Attributes.Remove("Style");
            }

            String Municipio = (ddlMunicipio.SelectedIndex < 1 ? "" : ddlMunicipio.Items[ddlMunicipio.SelectedIndex].Text);
            if (Estado != "--OTRO--" ? (Municipio.Trim().Length == 0 || Municipio == "0") : false)
            {
                ddlMunicipio.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                ddlMunicipio.Attributes.Remove("Style");
            }

            String Pais = (ddlPais.SelectedIndex < 1 ? "" : ddlPais.Items[ddlPais.SelectedIndex].Text);
            if (Estado == "--OTRO--" ? (Pais.Trim().Length == 0 || Pais == "0") : false)
            {
                ddlPais.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                ddlPais.Attributes.Remove("Style");
            }

            String Edad = TxtEdad.Text;
            if (Edad.Trim().Length == 0)
            {
                TxtEdad.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                TxtEdad.Attributes.Remove("Style");
            }

            String FechaI = TxtFechaI.Text;
            if (Edad.Trim().Length == 0)
            {
                TxtFechaI.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                TxtFechaI.Attributes.Remove("Style");
            }

            String Medio = ddlMedio.SelectedValue;
            if (Edad.Trim().Length == 0)
            {
                ddlMedio.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                ddlMedio.Attributes.Remove("Style");
            }

            String TelFijo = TxtTelFijo.Text;
            String Extension = TxtExtension.Text;
            String Celular = TxtCelular.Text;
            if (TelFijo.Trim().Length > 0 || Celular.Trim().Length > 0)
            {
                TxtTelFijo.Attributes.Remove("style");
                TxtCelular.Attributes.Remove("style");
            }
            else
            {
                TxtTelFijo.Attributes.Add("style", "border: 1px red solid;");
                TxtCelular.Attributes.Add("style", "border: 1px red solid;");
                Pasa = false;
            }
            
            String FechaN = TxtFechaN.Text;
            if (FechaN.Trim().Length == 0)
            {
                TxtFechaN.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                TxtFechaN.Attributes.Remove("Style");
            }

            String Email = TxtEmail.Text;
            if (Email.Trim().Length == 0)
            {
                TxtEmail.Attributes.Add("Style", "border: red 1px solid;");
                Pasa = false;
            }
            else
            {
                TxtEmail.Attributes.Remove("Style");
            }

            String Observaciones = TxtObservaciones.Text;
            String Nota = TxtNota.Text;
            String Estatus = ddlEstatus.SelectedValue;
            String Env_Correo = ddlEnviarCorreo.SelectedValue;
            String Req_Factura = ddlFactura.SelectedValue;

            String RFC = "";
            String Entidad = "";
            //String Dir_Fiscal = "";
            String NomRazon = "";
            String CalleF = "";
            String NoExtF = "";
            String NoIntF = "";
            String ColoniaF = "";
            String CpF = "";
            String EstadoF = "";
            String MunicipioF = "";
            String PaisF = "";

            Boolean pasaFactura = true;
            if (Req_Factura == "1")
            {
                RFC = txtRfcDFa.Text;
                if (RFC.Trim().Length == 0)
                {
                    txtRfcDFa.Attributes.Add("Style", "border: red 1px solid;");
                    pasaFactura = false;
                }
                else
                {
                    txtRfcDFa.Attributes.Remove("Style");
                }
                Entidad = ddlEntidadDFa.SelectedItem.Text;
                NomRazon = txtNombRDFa.Text;
                if (NomRazon.Trim().Length == 0)
                {
                    txtNombRDFa.Attributes.Add("Style", "border: red 1px solid;");
                    pasaFactura = false;
                }
                else
                {
                    txtNombRDFa.Attributes.Remove("Style");
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
                //if (NoIntF.Trim().Length == 0)
                //{
                //    txtNoIntDFa.Attributes.Add("style", "border: 1px red solid;");
                //    pasaFactura = false;
                //}
                //else
                //{
                //    txtNoIntDFa.Attributes.Remove("style");
                //}
                ColoniaF = txtColoniaDFa.Text;
                if (ColoniaF.Trim().Length == 0)
                {
                    txtColoniaDFa.Attributes.Add("style", "border: 1px red solid;");
                    Pasa = pasaFactura;
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
                if (Estado != "--OTRO--" ? (Municipio.Trim().Length == 0 || Municipio == "0") : false)
                {
                    ddlMunicipioDFa.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    ddlMunicipioDFa.Attributes.Remove("style");
                }
                PaisF = (ddlPaisDFa.SelectedIndex < 1 ? "" : ddlPaisDFa.Items[ddlPaisDFa.SelectedIndex].Text);
                if (Estado == "--OTRO--" ? (Pais.Trim().Length == 0 || Pais == "0") : false)
                {
                    ddlPaisDFa.Attributes.Add("style", "border: 1px red solid;");
                    pasaFactura = false;
                }
                else
                {
                    ddlPaisDFa.Attributes.Remove("style");
                }
            }
            if (pasaFactura)
            {
                btnAddDatosF.Attributes.Remove("Style");
            }
            else
            {
                Pasa = false;
                btnAddDatosF.Attributes.Add("Style", "border: red 1px solid;");
            }


            if (Pasa)
            {
                lblError.Text = "";

                String resultado = connMySql.ActualizaCliente(IDCliente, Nombre, ApellidoP, ApellidoM, Edad, FechaN, FechaI, Municipio, TelFijo, Extension, Celular, Email, Observaciones, Nota, (Medio == "0" ? "" : Medio), Estatus, Estado, Pais, Env_Correo, connMySql.traerIDEmpleado(Session["usuario"].ToString()), Req_Factura, RFC, Entidad, CalleF, NoIntF, NoExtF, ColoniaF, CpF, EstadoF, MunicipioF, PaisF, NomRazon);

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
                ddlPais.SelectedIndex = -1;
                divPais.Visible = false;
                divMunicipio.Visible = true;
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

                gvGerentes.EditIndex = -1;
                cargaClientes();
            }
            else
            {
                lblError.Text = "Favor de llenar los campos faltantes";
            }
        }

        protected void btnBuscarF_Click(object sender, EventArgs e)
        {
            if (SesionViva())
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

                if (ddlEstatus.SelectedValue == "0" || ddlEstatus.SelectedValue == "1")
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " estatus LIKE '%" + ddlEstatus.SelectedValue + "%' ";
                }

                if (ddlEnviarCorreo.SelectedValue != "-1")
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " Enviar_Correo = '" + ddlEnviarCorreo.SelectedValue + "' ";
                }

                if (ddlFactura.SelectedValue != "-1")
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " req_factura like '%" + ddlFactura.SelectedValue.Trim() + "%' ";
                }
                

                if (pasa)
                {
                    lblError.Text = "";
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
                else
                {
                    lblError.Text = "Favor de llenar los campos faltantes";
                }
            }
        }

        protected void btnLimpiarF_Click(object sender, EventArgs e)
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
                ddlEstatus.SelectedIndex = 0;
                ddlPais.SelectedIndex = -1;

                lblA.Visible = false;
                TxtEdad.Attributes.Remove("style");
                txtEdad2.Visible = false;
                TxtEdad.Attributes.Add("style", "width:calc(100% - 25px); margin-right:0px;");
                divPais.Visible = false;
                divMunicipio.Visible = true;
            }
        }

        //Muestra la ventana del buscador
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TxtEdad.Enabled = true;

            if (SesionViva())
            {
                panelMsj.DefaultButton = btnBuscarF.ID;

                if (chkRango.Checked)
                {
                    TxtEdad.Attributes.Remove("style");
                    TxtEdad.Attributes.Add("style", "width:70px; margin-right: 0px;");
                    lblA.Visible = true;
                    txtEdad2.Visible = true;
                }
                else
                {
                    ocultaRango();
                    TxtEdad.Attributes.Add("style", "width:calc(100% - 25px); margin-right:0px;");
                    chkRango.Visible = true;
                }
                
                divObservacionesNota.Visible = false;
                int temporal = ddlEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1"));
                if (ddlEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1")) == -1)
                {
                    ddlEnviarCorreo.Items.Add(new ListItem("Todos", "-1"));
                    ddlEnviarCorreo.SelectedValue = "-1";
                }
                
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

                chkRango.Visible = true;

                if (ddlFactura.Items.IndexOf(new ListItem("Todos", "-1")) == -1)
                {
                    ddlFactura.Items.Add(new ListItem("Todos", "-1"));
                    ddlFactura.SelectedValue = "-1";
                }

                //if (ddlEntidad.Items.IndexOf(new ListItem("Todos", "-1")) == -1)
                //{
                //    ddlEntidad.Items.Add(new ListItem("Todos", "-1"));
                //    ddlEntidad.SelectedValue = "-1";
                //}

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
            if (SesionViva())
            {
                if (Session["Orden"].ToString() == e.SortExpression + " " + "ASC")
                {
                    Session["Orden"] = e.SortExpression + " " + "DESC";
                }
                else
                {
                    Session["Orden"] = e.SortExpression + " " + "ASC";
                }

                gvGerentes.PageIndex = 0;
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
            if (ddlEstado.SelectedValue == "33")
            {
                divMunicipio.Visible = false;
                divPais.Visible = true;
            }
            else
            {
                divMunicipio.Visible = true;
                divPais.Visible = false;
                llenaMunicipio(ddlEstado.SelectedValue);
            }
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

        public void llenaPaises()
        {
            ddlPais.DataTextField = "pais";
            ddlPais.DataValueField = "pais";
            ddlPais.DataSource = connMySql.traerPaises();
            ddlPais.DataBind();

            ddlPais.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }



        public void mostrarMensaje2(String Nombre)
        {
            lblPregunta2.Text = Nombre.ToUpper() + "";

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
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Cliente eliminado exitosamente" : resultado));

            cargaClientes();
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
                ocultaRango();
                txtEdad2.Attributes.Remove("style");
                TxtEdad.Attributes.Add("style", "width:calc(100% - 25px); margin-right:0px;");
                chkRango.Visible = true;
            }
            panelMsj.DefaultButton = btnBuscarF.ID;
        }

        public void ocultaRango()
        {
            TxtEdad.Attributes.Remove("style");
            txtEdad2.Text = "";
            lblA.Visible = false;
            txtEdad2.Visible = false;
            chkRango.Visible = false;
            chkRango.Checked = false;
        }


        protected void gvGerentes_RowConsult(object sender, EventArgs e)
        {
            panelMsj.DefaultButton = FGActualizar.ID;
            divObservacionesNota.Visible = true;

            btnCerrarF.Visible = false;
            btnBuscarF.Visible = false;
            btnLimpiarF.Visible = false;
            FGCancelar.Visible = true;
            FGActualizar.Visible = false;
            FGAgregar.Visible = false;

            ocultaRango();
            
            if (ddlEstatus.Items.IndexOf(lITodos) == 0)
            {
                ddlEstatus.Items.RemoveAt(0);
            }

            if (ddlEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1")) > -1)
            {
                ddlEnviarCorreo.Items.RemoveAt(ddlEnviarCorreo.Items.IndexOf(new ListItem("Todos", "-1")));
            }

            FTitulo.Text = "Detalle de cliente";

            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            Label IdCliente = (Label)row.FindControl("lblIdCliente");
            Label Nombre = (Label)row.FindControl("lblNombre");
            Label ApellidoP = (Label)row.FindControl("lblApellidoP");
            Label ApellidoM = (Label)row.FindControl("lblApellidoM");
            Label Estado = (Label)row.FindControl("lblEstado");
            Label Municipio = (Label)row.FindControl("lblMunicipio");
            Label Pais = (Label)row.FindControl("lblPais");
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
            Label Env_Correo = (Label)row.FindControl("lblEnvCorreo");
            Label R_Factura = (Label)row.FindControl("lblFactura");
            Label RFC = (Label)row.FindControl("lblRfc");
            Label Entidad = (Label)row.FindControl("lblEntidad");
            
            Label NomRazon = (Label)row.FindControl("lblNomRazon");
            Label Calle = (Label)row.FindControl("lblCalleF");
            Label NoInterior = (Label)row.FindControl("lblNoInteriorF");
            Label NoExterior = (Label)row.FindControl("lblNoExteriorF");
            Label Colonia = (Label)row.FindControl("lblColoniaF");
            Label EstadoF = (Label)row.FindControl("lblEstadoF");
            Label MunicipioF = (Label)row.FindControl("lblMunicipioF");
            Label PaisF = (Label)row.FindControl("lblPaisF");
            Label CPF = (Label)row.FindControl("lblCPf");

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
                if (Estado.Text != "--OTRO--")
                {
                    divMunicipio.Visible = true;
                    divPais.Visible = false;
                    
                    llenaMunicipio(ddlEstado.SelectedValue);
                    try
                    {
                        ddlMunicipio.SelectedValue = Municipio.Text.Trim();
                    }
                    catch { }
                }
                else
                {
                    divMunicipio.Visible = false;
                    divPais.Visible = true;
                    try
                    {
                        ddlPais.SelectedValue = Pais.Text.Trim();
                        divPais.Visible = true;
                        divMunicipio.Visible = false;
                    }
                    catch (Exception ex) { }
                }
            }
            catch { }
            
            TxtEdad.Text = Edad.Text;

            try
            {
                TxtFechaI.Text = FechaI.Text.Substring(6) + "-" + FechaI.Text.Substring(3, 2) + "-" + FechaI.Text.Substring(0, 2);
            }
            catch (Exception ex) { }
            
            try
            {
                ddlMedio.SelectedValue = Medio.Text;
            }
            catch { }

            TxtTelFijo.Text = TelFijo.Text;
            TxtExtension.Text = Extension.Text;
            TxtCelular.Text = Celular.Text;

            try
            {
                TxtFechaN.Text = FechaN.Text.Substring(6) + "-" + FechaN.Text.Substring(3, 2) + "-" + FechaN.Text.Substring(0, 2); ;
            }
            catch (Exception ex)
            { }
            
            TxtEmail.Text = Email.Text;
            TxtObservaciones.Text = Observaciones.Text;
            TxtNota.Text = Nota.Text;
            ddlEstatus.SelectedIndex = (Estatus.Text == "Activo" ? 0 : 1);
            ddlFactura.SelectedIndex = (R_Factura.Text == "Sí" ? 0 : 1);

            if (R_Factura.Text == "Sí")
            {
                divBtnDatosF.Visible = true;
            }

            //if (ddlEntidad.Items.IndexOf(new ListItem("Todos", "-1")) > -1)
            //{
            //    ddlEntidad.Items.RemoveAt(ddlEntidad.Items.IndexOf(new ListItem("Todos", "-1")));
            //}

            if (ddlFactura.Items.IndexOf(new ListItem("Todos", "-1")) > -1)
            {
                ddlFactura.Items.RemoveAt(ddlFactura.Items.IndexOf(new ListItem("Todos", "-1")));
            }


            //ddlEntidad.SelectedIndex = (Entidad.Text == "Persona moral" ? 0 : 1);
            //txtDirFiscal.Text = DirFiscal.Text;
            //txtRfc.Text = RFC.Text;
            //txtNmR.Text = NomRazon.Text;

            TxtNombre.Enabled = false;
            TxtApellidoM.Enabled = false;
            TxtApellidoP.Enabled = false;
            ddlEstado.Enabled = false;
            ddlMunicipio.Enabled = false;
            ddlPais.Enabled = false;
            TxtFechaN.Enabled = false;
            TxtEdad.Enabled = false;
            TxtFechaI.Enabled = false;
            ddlMedio.Enabled = false;
            TxtTelFijo.Enabled = false;
            TxtExtension.Enabled = false;
            TxtCelular.Enabled = false;
            TxtEmail.Enabled = false;
            TxtObservaciones.Enabled = false;
            TxtNota.Enabled = false;
            ddlEstatus.Enabled = false;
            ddlEnviarCorreo.Enabled = false;
            ddlFactura.Enabled = false;

            txtRfcDFa.Enabled = false;
            txtNombRDFa.Enabled = false;
            txtCalleDFa.Enabled = false;
            txtNoIntDFa.Enabled = false;
            txtNoExtDFa.Enabled = false;
            txtColoniaDFa.Enabled = false;
            ddlEstadoDFa.Enabled = false;
            ddlMunicipioDFa.Enabled = false;
            ddlPaisDFa.Enabled = false;
            txtCPdfA.Enabled = false;
            ddlEntidadDFa.Enabled = false;
            

            //ddlEntidad.Enabled = false;
            //txtDirFiscal.Enabled = false;
            //txtRfc.Enabled = false;
            //txtNmR.Enabled = false;

            btnAddDatosF.Text = "<i class='fa fa-file-text-o' aria-hidden='true'></i><i class='fa fa-eye' aria-hidden='true'></i>";
            btnAddDatosF.ToolTip = "Ver datos de facturación";
            txtNombRDFa.Text = NomRazon.Text;
            ddlEntidadDFa.SelectedIndex = (Entidad.Text == "Persona moral" ? 0 : 1);
            txtRfcDFa.Text = RFC.Text;
            txtCalleDFa.Text = Calle.Text;
            txtNoExtDFa.Text = NoExterior.Text;
            txtNoIntDFa.Text = NoInterior.Text;
            txtColoniaDFa.Text = Colonia.Text;
            txtCPdfA.Text = CPF.Text;

            try
            {
                ddlEstadoDFa.SelectedValue = connMySql.traerIdEstado(EstadoF.Text.Trim());
                if (EstadoF.Text != "--OTRO--")
                {
                    divMunicipioDFa.Visible = true;
                    divPaisDFa.Visible = false;

                    llenaMunicipioDFa(ddlEstadoDFa.SelectedValue);
                    try
                    {
                        ddlMunicipioDFa.SelectedValue = MunicipioF.Text.Trim();
                    }
                    catch { }
                }
                else
                {
                    divMunicipioDFa.Visible = false;
                    divPaisDFa.Visible = true;
                    try
                    {
                        ddlPaisDFa.SelectedValue = PaisF.Text.Trim();
                        divPaisDFa.Visible = true;
                        divMunicipioDFa.Visible = false;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch { }
            ddlMunicipioDFa.Enabled = false;
        }
        
        public void HabilitaCampos()
        {
            if (!TxtNombre.Enabled)
            {
                TxtNombre.Enabled = true;
                TxtApellidoM.Enabled = true;
                TxtApellidoP.Enabled = true;
                ddlEstado.Enabled = true;
                ddlMunicipio.Enabled = true;
                ddlPais.Enabled = true;
                TxtFechaN.Enabled = true;
                TxtEdad.Enabled = true;
                TxtFechaI.Enabled = true;
                ddlMedio.Enabled = true;
                TxtTelFijo.Enabled = true;
                TxtExtension.Enabled = true;
                TxtCelular.Enabled = true;
                TxtEmail.Enabled = true;
                TxtObservaciones.Enabled = true;
                TxtNota.Enabled = true;
                ddlEstatus.Enabled = true;
                ddlEnviarCorreo.Enabled = true;
                ddlFactura.Enabled = true;

                ddlFactura.Enabled = true;
                //ddlEntidad.Enabled = true;
                //txtDirFiscal.Enabled = true;
                //txtRfc.Enabled = true;
                //txtNmR.Enabled = true;
                //divDatosFactura.Visible = false;

                divBtnDatosF.Visible = false;
                ddlEntidadDFa.Enabled = true;
                txtRfcDFa.Enabled = true;
                txtNombRDFa.Enabled = true;
                txtCalleDFa.Enabled = true;
                txtNoIntDFa.Enabled = true;
                txtNoExtDFa.Enabled = true;
                txtColoniaDFa.Enabled = true;
                ddlEstadoDFa.Enabled = true;
                ddlMunicipioDFa.Enabled = false;
                ddlPaisDFa.Enabled = true;
                divMunicipioDFa.Visible = true;
                divPaisDFa.Visible = false;
                txtCPdfA.Enabled = true;
            }
        }


        protected void TxtFechaN_TextChanged(object sender, EventArgs e)
        {
            if (btnBuscarF.Visible == false)
            {
                String f = TxtFechaN.Text;
                DateTime now = DateTime.Today;
                DateTime before = DateTime.Parse(f);
                int age = now.Year - before.Year;
                String edad;
                if (now.Month < before.Month)
                {
                    if (now.Day < before.Day)
                    {
                        age = age - 1;
                        edad = age.ToString();

                        TxtEdad.Text = edad;
                    }
                }
                else
                {
                    if (now.Month == before.Month)
                    {
                        if (now.Day < before.Day)
                        {
                            age = age - 1;
                            edad = age.ToString();

                            TxtEdad.Text = edad;
                        }
                        else
                        {
                            edad = age.ToString();
                            TxtEdad.Text = edad;
                        }
                    }
                    else
                    {
                        edad = age.ToString();
                        TxtEdad.Text = edad;
                    }
                }
            }
        }

        protected void ddlFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFactura.SelectedValue == "1")
            {
                //divDatosFactura.Visible = true;
                if (FTitulo.Text == "Buscar clientes")
                {
                    divBtnDatosF.Visible = false;
                }
                else
                {
                    divBtnDatosF.Visible = true;
                }
            }
            else
            {
                //divDatosFactura.Visible = false;
                divBtnDatosF.Visible = false;
            }
        }

        protected void btnAddDatosF_Click(object sender, EventArgs e)
        {
            divFormularioG.Visible = false;
            divMensajeDF.Visible = true;
            if (FTitulo.Text == "Detalle de cliente")
            {
                btnLimpiaDF.Visible = false;
                btnGuardaDF.Visible = false;
            }
        }

        protected void FGCancelarDF_Click(object sender, EventArgs e)
        {
            divFormularioG.Visible = true;
            btnLimpiaDF.Visible = true;
            divMensajeDF.Visible = false;
            btnGuardaDF.Visible = true;
        }

        protected void btnLimpiarDF_Click(object sender, EventArgs e)
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

        protected void FGAgregarDF_Click(object sender, EventArgs e)
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
            //if (NoIntF.Trim().Length == 0)
            //{
            //    txtNoIntDFa.Attributes.Add("style", "border: 1px red solid;");
            //    pasa = false;
            //}
            //else
            //{
            //    txtNoIntDFa.Attributes.Remove("style");
            //}
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
                divFormularioG.Visible = true;
                //divDatosFactura.Visible = false;
                divMensajeDF.Visible = false;
                btnAddDatosF.Attributes.Remove("Style");
            }
            else
            {
                lblErrorDF.Text = "Favor de llenar los campos faltantes.";
            }
        }

        public void ddlEstadoDFa_SelectedIndexChanged(object sender, EventArgs e)
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



    }
}