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
                mostrarMensaje("Su sesión a caducado, vuelva a iniciar sesion.");
                btnOkSalir.Visible = true;
                MOk.Visible = false;
            }

            if (!permisos.Contains("21"))
            {
                Response.Redirect("Principal.aspx");
            }

            if (!IsPostBack)
            {

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
                }else
                {
                    Session["MasMenosMes"] = 0;
                }


                MasterFarmacia master = (MasterFarmacia)this.Master;
                master.mostrarMensaje(false);
                sombraMensaje.Visible = false;

                cargaClientes();
            }
        }

        
        public void cargaClientes()
        {
            switch (DateTime.Now.AddMonths(int.Parse(Session["MasMenosMes"].ToString())).Month)
            {
                case 1:
                    lblMes.Text = "ENERO";
                    break;
                case 2:
                    lblMes.Text = "FEBRERO";
                    break;
                case 3:
                    lblMes.Text = "MARZO";
                    break;
                case 4:
                    lblMes.Text = "ABRIL";
                    break;
                case 5:
                    lblMes.Text = "MAYO";
                    break;
                case 6:
                    lblMes.Text = "JUNIO";
                    break;
                case 7:
                    lblMes.Text = "JULIO";
                    break;
                case 8:
                    lblMes.Text = "AGOSTO";
                    break;
                case 9:
                    lblMes.Text = "SEPTIEMBRE";
                    break;
                case 10:
                    lblMes.Text = "OCTUBRE";
                    break;
                case 11:
                    lblMes.Text = "NOVIEMBRE";
                    break;
                case 12:
                    lblMes.Text = "DICIEMBRE";
                    break;
            }


            //String condicion = "";

            //String condicion = txtBusquedaC.Text.Trim().ToUpper();

            //if (condicion.Length > 0)
            //{
            //    condicion = " (NOMBRE LIKE '%" + condicion + "%' OR APELLIDO_PATERNO LIKE '%" + condicion + "%' OR APELLIDO_MATERNO LIKE '%" + condicion + "%' OR EMAIL LIKE '%" + condicion + "%' OR  OBSERVACIONES LIKE '%" + condicion + "%' OR NOTA LIKE '%" + condicion + "%')";
            //}

            DataSet ds = connMySql.TraerClientesDelMes(Session["Condicion"].ToString().Trim(), DateTime.Now.AddMonths(int.Parse(Session["MasMenosMes"].ToString())).Month );

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


        protected void gvGerentes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

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
            //String condicion = txtBusquedaC.Text.Trim().ToUpper();

            //if (condicion.Length > 0)
            //{
            //    condicion = " (nombre like '%" + condicion + "%' or apellido_paterno like '%" + condicion + "%' or apellido_materno like '%" + condicion + "%' or EMAIL like '%" + condicion + "%' or OBSERVACIONES like '%" + condicion + "%' or NOTA like '%" + condicion + "%')";
            //}

            DataSet ds = connMySql.TraerClientesDelMesSiguiente(Session["Condicion"].ToString());

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

        protected void btnOkSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            DatosCorreo dCorreo = new DatosCorreo();
            dCorreo = connMySql.ConsultaDatosCorreo();

            connMySql.ActualizaUltimoCorreo();

            EnviarCorreo enviaCorreo = new EnviarCorreo();

            
            DataSet datosCliente = connMySql.TraerClientesDelMes("", DateTime.Now.Month);
            List<String> listaCorreos = new List<string>();

            foreach (DataRow dr in datosCliente.Tables[0].Rows)
            {
                String temporal = dr["EMAIL"].ToString();
                listaCorreos.Add(temporal);
            }
            if (listaCorreos.Count > 0)
            {
                String direccionImagen = Server.MapPath("Imagenes/Correo/image.jpg");
                bool respuesta = enviaCorreo.Enviar(listaCorreos, direccionImagen);

                if (respuesta)
                {
                    lblMensajeJS.Text = "Los correos se han enviado exitosamente";
                }
                else{
                    lblMensajeJS.Text = "Ocurrio un problema al enviar los correo, favor de intentarlo nuevamente.";
                }
                //enviaCorreo.Enviar(listaCorreos, dCorreo.SMTP_IMAGEN);
            }

            sombraJS.Style.Remove("Display");
            CargaJS.Visible = false;
            lblMensajeJS.Text = "Los correos se han enviado exitosamente";
            divMensajeJS.Style.Add("Height", "125px");
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            sombraJS.Style.Add("Display", "none");
            divMensajeJS.Style.Remove("Height");
            CargaJS.Visible = true;
            lblMensajeJS.Text = "Enviando correos a los festejados del mes actual";


        }

        protected void btnMenosMes_Click(object sender, ImageClickEventArgs e)
        {
            int meses = int.Parse(Session["MasMenosMes"].ToString());
            meses -= 1;
            Session["MasMenosMes"] = meses;
            cargaClientes();
        }

        protected void btnMasMes_Click(object sender, ImageClickEventArgs e)
        {
            int meses = int.Parse(Session["MasMenosMes"].ToString());
            meses += 1;
            Session["MasMenosMes"] = meses;
            cargaClientes();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            btnLimpiarF.Visible = true;
            btnBuscarF.Visible = true;
            divFormularioG.Visible = true;


            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;
        }

        protected void btnBuscarF_Click(object sender, EventArgs e)
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
            divFormularioG.Visible = false;

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(false);
            sombraMensaje.Visible = false;

        }

        protected void btnLimpiarF_Click(object sender, EventArgs e)
        {

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
            divFormularioG.Visible = false;

            Session["Condicion"] = "";
            cargaClientes();

        }
    }
    
}