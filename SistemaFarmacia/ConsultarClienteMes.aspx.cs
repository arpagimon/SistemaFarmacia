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


    public partial class ConsultarClienteMes : System.Web.UI.Page
    {


        Conexion connMySql = new Conexion();

        public int mesesMasMenos = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            String permisos = "";
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
            }

            if (SesionViva())
            {
                if (!IsPostBack)
                {


                    if (!permisos.Contains("21"))
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

                    if (Session["MasMenosMes"] == null)
                    {
                        Session.Add("MasMenosMes", 0);
                    }
                    else
                    {
                        Session["MasMenosMes"] = 0;
                    }


                    if (Session["Orden"] == null)
                    {
                        Session.Add("Orden", "ID_CLIENTE");
                    }
                    else
                    {
                        Session["Orden"] = "ID_CLIENTE";
                    }

                    MasterFarmacia master = (MasterFarmacia)this.Master;
                    master.mostrarMensaje(false);
                    sombraMensaje.Visible = false;
                    master.mostrarLblUser("<p>Usuario: " + this.Session["usuario"].ToString() + " </p>");

                    cargaClientes();
                    llenarDropDownDlist();
                    llenaEstados();
                    llenaPaises();

                    master.cambiarLblTitle("<img src='Imagenes/cumple.png' alt='clientes'><h1>Cumpleaños de clientes por Mes</h1>");
                }
            }
        }

        
        public void cargaClientes()
        {
            if (SesionViva())
            {
                lblMes.Text = MesConsultado(Session["MasMenosMes"].ToString());
                

                DataSet ds = connMySql.TraerClientesDelMes(Session["Condicion"].ToString().Trim(), DateTime.Now.AddMonths(int.Parse(Session["MasMenosMes"].ToString())).Month, connMySql.TraerEnvioCorreo());

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
                        dtTemporal.Columns.Add("Estado");
                        dtTemporal.Columns.Add("MUNICIPIO");
                        dtTemporal.Columns.Add("tel_casa_fijo");
                        dtTemporal.Columns.Add("extension");
                        dtTemporal.Columns.Add("celular");
                        dtTemporal.Columns.Add("EMAIL");
                        dtTemporal.Columns.Add("OBSERVACIONES");
                        dtTemporal.Columns.Add("NOTA");
                        dtTemporal.Columns.Add("MEDIO");
                        dtTemporal.Columns.Add("estatus"); 
                        dtTemporal.Columns.Add("CHECK_EMAIL");
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
                            //if (lbSort.Text == "Nombre")
                            //{
                            //    //lbSort.Text = imgOrd + lbSort.Text;
                            //    lbSort.CssClass = "Seleccionada";
                            //}
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
                    catch(Exception ex)
                    {

                    }

                }
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblNumeroFila = (Label)e.Row.FindControl("lblindice");

                Label lblCheckFecha = (Label)e.Row.FindControl("lblcheck");
                Image imagenheck = (Image)e.Row.FindControl("imgImagen");
                Label lblemail1 = (Label)e.Row.FindControl("lblEmail");



                if (lblemail1.Text.Length > 0)
                {
                    if (lblCheckFecha.Text.Length > 0)
                    {
                        if (lblCheckFecha.Text.Trim() == DateTime.Today.Year.ToString())
                        {
                            imagenheck.Visible = true;
                        }
                        else
                        {
                            imagenheck.Visible = false;
                        }
                    }
                    else
                    {
                        imagenheck.Visible = false;
                    }
                }



                    try
                {
                    lblNumeroFila.Text = ((e.Row.RowIndex + 1) + (gvGerentes.PageIndex * gvGerentes.PageSize)).ToString();
                }
                catch { }

                Label etiquetaFechaI = (Label)e.Row.FindControl("lblFechaI");
                Label etiquetaFechaN = (Label)e.Row.FindControl("lblFechaN");

                if (etiquetaFechaI.Text.Trim().Length > 0)
                {
                    etiquetaFechaI.Text = etiquetaFechaI.Text.Split(' ')[0];
                }

                if (etiquetaFechaN.Text.Trim().Length > 0)
                {
                    etiquetaFechaN.Text = etiquetaFechaN.Text.Split(' ')[0];
                }
            }
        }




        public void mostrarMensaje(String mensaje)
        {
            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);

            sombraMensaje.Visible = true;

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

        
        protected void txtBusquedaC_TextChanged(object sender, EventArgs e)
        {
            if (SesionViva())
            {
                DataSet ds = connMySql.TraerClientesDelMesSiguiente(Session["Condicion"].ToString(),connMySql.TraerEnvioCorreo());

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

        protected void btnOkSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }


        protected void btnMenosMes_Click(object sender, ImageClickEventArgs e)
        {
            if (SesionViva())
            {
                int meses = int.Parse(Session["MasMenosMes"].ToString());
                meses -= 1;
                Session["MasMenosMes"] = meses;
                cargaClientes();
            }
        }

        protected void btnMasMes_Click(object sender, ImageClickEventArgs e)
        {
            if (SesionViva())
            {
                int meses = int.Parse(Session["MasMenosMes"].ToString());
                meses += 1;
                Session["MasMenosMes"] = meses;
                cargaClientes();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            panelMsj.DefaultButton = btnBuscarF.ID;
            btnLimpiarF.Visible = true;
            btnBuscarF.Visible = true;
            btnCerrarF.Visible = true;
            divFormularioG.Visible = true;


            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;

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

            chkRango.Visible = true;
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


                if (ddlEstado.SelectedIndex > 0)
                {
                    condicion += (condicion.Length > 0 ? " and " : "") + " Estado like '%" + ddlEstado.Items[ddlEstado.SelectedIndex].Text.Trim() + "%' ";
                    if (ddlMunicipio.SelectedIndex > 0)
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

                //if (TxtEdad.Text.Trim().Length > 0)
                //{
                //    condicion += (condicion.Length > 0 ? " and " : "") + " edad like '%" + TxtEdad.Text.Trim() + "%' ";
                //}
                Boolean pasa = true;
                if (chkRango.Checked)
                {
                    if (TxtEdad.Text.Trim().Length > 0 && txtEdad2.Text.Trim().Length > 0)
                    {
                        condicion += (condicion.Length > 0 ? " and " : "") + " edad between " + TxtEdad.Text.Trim() + " and " + txtEdad2.Text.Trim() + " ";

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
                        condicion += (condicion.Length > 0 ? " and " : "") + " edad like '%" + TxtEdad.Text.Trim() + "%' ";
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


                if (pasa)
                {
                    Session["Condicion"] = condicion;
                    cargaClientes();


                    btnLimpiarF.Visible = false;
                    btnBuscarF.Visible = false;
                    btnCerrarF.Visible = false;
                    divFormularioG.Visible = false;

                    MasterFarmacia master = (MasterFarmacia)this.Master;
                    master.mostrarMensaje(false);
                    sombraMensaje.Visible = false;
                }else
                {
                    lblError.Text = "Favor de llenar los campos faltantes";
                }
            }
        }

        protected void btnLimpiarF_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        public void limpiar()
        {

            TxtIdCliente.Text = "";
            TxtNombre.Text = "";
            TxtApellidoP.Text = "";
            TxtApellidoM.Text = "";
            ddlMunicipio.SelectedIndex = -1;
            ddlEstado.SelectedIndex = -1;
            TxtEdad.Text = "";
            txtEdad2.Text = "";
            TxtFechaI.Text = "";
            ddlMedio.SelectedIndex = -1;
            TxtTelFijo.Text = "";
            TxtExtension.Text = "";
            TxtCelular.Text = "";
            TxtFechaN.Text = "";
            TxtEmail.Text = "";
            TxtObservaciones.Text = "";
            TxtNota.Text = "";

            chkRango.Checked = false;
            ddlPais.SelectedIndex = -1;
            divPais.Visible = false;
            divMunicipio.Visible = true;

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
                gvGerentes.PageIndex = 0;
                cargaClientes();
                if (Session["Orden"].ToString() == e.SortExpression + " " + "ASC")
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
                
                ddlMunicipio.SelectedIndex = -1;
                ddlEstado.SelectedIndex = -1;
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
                
                Session["Condicion"] = "";
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


        public void llenaPaises()
        {
            ddlPais.DataTextField = "pais";
            ddlPais.DataValueField = "pais";
            ddlPais.DataSource = connMySql.traerPaises();
            ddlPais.DataBind();

            ddlPais.Items.Insert(0, new ListItem("--Seleccionar--", "0"));
        }







        #region Correo

        protected void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            if (int.Parse(Session["MasMenosMes"].ToString()) > 1 || int.Parse(Session["MasMenosMes"].ToString()) < -1)
            {
                mostrarMensaje("Solo se puede mandar correos al mes corriente, mes anterior o siguiente mes");
            }
            else
            {
                sombraJS.Visible = true;
                //MContenidoJS.Visible = false;
                MContenidoJS.Style.Add("display", "none");

                if (Session["Condicion"].ToString().Trim().Length > 0)
                {
                    divFormularioCorreo.Visible = false;
                    divFormCorreoCondicion.Visible = true;

                }
                else
                {
                    divFormularioCorreo.Visible = true;
                    divFormCorreoCondicion.Visible = false;

                    chkMesActual.Checked = true;
                    divMesActual.Attributes.Add("style", "background:#f0f0f0;");
                }
            }
        }


        protected void btnFormCorreoConAceptar_Click(object sender, EventArgs e)
        {
            divFormCorreoCondicion.Visible = false;
            divConfirmarCorreoCon.Visible = true;
        }

        protected void btnFormCorreoConCancelar_Click(object sender, EventArgs e)
        {
            divFormCorreoCondicion.Visible = false;
            divCanCorreoCon.Visible = true;
        }


        protected void btnAceptarCanCorreoCon_Click(object sender, EventArgs e)
        {
            divCanCorreoCon.Visible = false;
            divFormularioCorreo.Visible = true;
            sombraJS.Visible = false;

            Session["Condicion"] = "";
            limpiar();
            cargaClientes();
        }

        protected void btnCancelarCanCorreoCon_Click(object sender, EventArgs e)
        {
            divCanCorreoCon.Visible = false;
            divFormularioCorreo.Visible = true;
            sombraJS.Visible = false;
        }

        protected void btnAceptarConfCorreoCon_Click(object sender, EventArgs e)
        {
            List<String> listaCorreos = new List<string>();
            List<String> listaIDs = new List<string>();
            DataSet datosCliente = connMySql.TraerClientesDelMes((Session["Condicion"].ToString().Trim().Length > 0 ? Session["Condicion"].ToString().Trim() + " and enviar_correo = '1'" : "enviar_correo = '1'"), DateTime.Now.AddMonths(int.Parse(Session["MasMenosMes"].ToString())).Month, connMySql.TraerEnvioCorreo());
            //DataSet datosCliente = connMySql.TraerClientesDelMesSiguiente(Session["Condicion"].ToString(), connMySql.TraerEnvioCorreo());

            foreach (DataRow dr in datosCliente.Tables[0].Rows)
            {
                if (dr["EMAIL"].ToString() != "")
                {
                    String temporal = dr["EMAIL"].ToString();
                    listaCorreos.Add(temporal);

                    String temporal2 = dr["ID_CLIENTE"].ToString();
                    listaIDs.Add(temporal2);
                }
            }

            mandarCorreos(listaCorreos, listaIDs);
            divConfirmarCorreoCon.Visible = false;
        }

        protected void btnCancelarConfCorreoCon_Click(object sender, EventArgs e)
        {
            divConfirmarCorreoCon.Visible = false;
            divFormularioCorreo.Visible = true;
            sombraJS.Visible = false;
        }


        protected void btnOkCorreo_Click(object sender, EventArgs e)
        {
            if(ddlMesActual.SelectedValue == "-1")
            {
                Session["condicionCorreo"] = "";
                lblMensajeCorreo.Text = "¿Está seguro de mandar correo a TODOS los clientes que cumplen años el mes de " + MesConsultado(Session["MasMenosMes"].ToString()) + "?";
            }
            else
            {
                Session["condicionCorreo"] = "  ( CHECK_EMAIL < date_format(DATE_ADD(sysdate(), INTERVAL " + Session["MasMenosMes"].ToString() + " MONTH),'%Y') or CHECK_EMAIL is null) ";
                lblMensajeCorreo.Text = "¿Está seguro de mandar correo a los clientes PENDIENTES DE ENVIAR CORREO que cumplen años el mes de " + MesConsultado(Session["MasMenosMes"].ToString()) + "?";
            }

            cargaClientes();
            divFormularioCorreo.Visible = false;
            divCorreoMensajeConfirm.Visible = true;
        }

        protected void btnCancelarCorreo_Click(object sender, EventArgs e)
        {
            sombraJS.Visible = false;
        }

        protected void chkMesAnterior_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMesAnterior.Checked)
            {
                divMesAnterior.Attributes.Add("style", "background: #f0f0f0;");
                chkMesActual.Checked = false;
                divMesActual.Attributes.Remove("style");
            }
        }

        protected void chkMesActual_CheckedChanged(object sender, EventArgs e)
        {

            if (chkMesActual.Checked)
            {
                divMesActual.Attributes.Add("style", "background: #f0f0f0;");
                chkMesAnterior.Checked = false;
                divMesAnterior.Attributes.Remove("style");
            }
        }


        protected void btnOK_Click(object sender, EventArgs e)
        {
            sombraJS.Style.Add("Display", "none");
            divMensajeJS.Style.Remove("Height");
            CargaJS.Visible = true;
            lblMensajeJS.Text = "Enviando correos a los festejados del mes actual";
        }


        protected void btnCorreoAcepEnv_Click(object sender, EventArgs e)
        {

            divCorreoMensajeConfirm.Visible = false;

            List<String> listaCorreos = new List<string>();
            List<String> listaIDs = new List<string>();
            DataSet datosCliente = connMySql.TraerClientesDelMes((Session["condicionCorreo"].ToString().Trim().Length > 0 ? Session["condicionCorreo"].ToString().Trim() + " and enviar_correo = '1'" : "enviar_correo = '1'"), DateTime.Now.AddMonths(int.Parse(Session["MasMenosMes"].ToString())).Month, connMySql.TraerEnvioCorreo());
            
            foreach (DataRow dr in datosCliente.Tables[0].Rows)
            {
                String temporal = dr["EMAIL"].ToString();
                listaCorreos.Add(temporal);
                String temporal2 = dr["ID_CLIENTE"].ToString();
                listaIDs.Add(temporal2);
            }

            mandarCorreos(listaCorreos, listaIDs);
        }

        protected void btnCorreoCancelEnv_Click(object sender, EventArgs e)
        {
            divFormularioCorreo.Visible = true;
            divCorreoMensajeConfirm.Visible = false;
        }


        public void mandarCorreos(List<String> listaCorreos, List<String> listaIDs)
        {
            DatosCorreo dCorreo = connMySql.ConsultaDatosCorreo();

            connMySql.ActualizaUltimoCorreo();

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
                     connMySql.ActualizaCorreoEnvCliente(cadenaIDs);
                    //}
                    connMySql.GuardaBitacora(Session["usuario"].ToString(),listaIDs,dCorreo);
                }
                else
                {
                    lblMensajeJS.Text = "Ocurrio un problema al enviar los correo, favor de intentarlo nuevamente.";
                }
            }

            sombraJS.Visible = true;
            MContenidoJS.Style.Remove("display");
            divFormularioCorreo.Visible = false;
            divFormCorreoCondicion.Visible = false;

            CargaJS.Visible = false;
            lblMensajeJS.Text = "Los correos se han enviado exitosamente";
            divMensajeJS.Style.Add("Height", "125px");

            cargaClientes();
        }



        #endregion

        public String MesConsultado(String MasMenosMes)
        {
            String nombre = "";

            switch (DateTime.Now.AddMonths(int.Parse(MasMenosMes)).Month)
            {
                case 1:
                    nombre = "ENERO";
                    break;
                case 2:
                    nombre = "FEBRERO";
                    break;
                case 3:
                    nombre = "MARZO";
                    break;
                case 4:
                    nombre = "ABRIL";
                    break;
                case 5:
                    nombre = "MAYO";
                    break;
                case 6:
                    nombre = "JUNIO";
                    break;
                case 7:
                    nombre = "JULIO";
                    break;
                case 8:
                    nombre = "AGOSTO";
                    break;
                case 9:
                    nombre = "SEPTIEMBRE";
                    break;
                case 10:
                    nombre = "OCTUBRE";
                    break;
                case 11:
                    nombre = "NOVIEMBRE";
                    break;
                case 12:
                    nombre = "DICIEMBRE";
                    break;
            }

            return nombre;
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

    }

}