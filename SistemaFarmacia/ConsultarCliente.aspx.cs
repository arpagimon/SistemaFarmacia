using System;
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
            MasterFarmacia master = (MasterFarmacia)this.Master;
            //String permisos = "";
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
               


                master.mostrarMensaje(false);
                sombraMensaje.Visible = false;

                cargaClientes();
            }
        }



        public void cargaClientes()
        {

            //String condicion = txtBusquedaC.Text.Trim().ToUpper();

            //if (condicion.Length > 0)
            //{
            //    condicion = " (NOMBRE LIKE '%" + condicion + "%' OR APELLIDO_PATERNO LIKE '%" + condicion + "%' OR APELLIDO_MATERNO LIKE '%" + condicion + "%' OR EMAIL LIKE '%" + condicion + "%' OR  OBSERVACIONES LIKE '%" + condicion + "%' OR NOTA LIKE '%" + condicion + "%')";
            //}

            DataSet ds = connMySql.TraerClientes(Session["Condicion"].ToString());

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
                if(e.Row.RowType != DataControlRowType.Pager){
                    e.Row.Cells[15].Visible = false;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!editar)
                {
                    e.Row.Cells[15].Controls[0].Visible = false;
                }

                if (!eliminar)
                {
                    e.Row.Cells[15].Controls[2].Visible = false;
                }

            }
        }

        protected void gvGerentes_RowEditing(object sender, GridViewEditEventArgs e)
        {

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
            TxtFechaI.Text = FechaI.Text;
            TxtMedio.Text = Medio.Text;
            TxtTelFijo.Text = TelFijo.Text;
            TxtExtension.Text = Extension.Text;
            TxtCelular.Text = Celular.Text;
            TxtFechaN.Text = FechaN.Text;
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

        protected void txtBusquedaC_TextChanged(object sender, EventArgs e)
        {
            String condicion ="";

            if (condicion.Length > 0)
            {
                condicion = " (nombre like '%" + condicion + "%' or apellido_paterno like '%" + condicion + "%' or apellido_materno like '%" + condicion + "%' or EMAIL like '%" + condicion + "%' or OBSERVACIONES like '%" + condicion + "%' or NOTA like '%" + condicion + "%')";
            }

            DataSet ds = connMySql.TraerClientes(condicion);

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

            String resultado = connMySql.ActualizaCliente(IDCliente, Nombre, ApellidoP, ApellidoM, Edad, FechaN, FechaI, Municipio, TelFijo, Extension, Celular, Email, Observaciones, Nota, Medio);

            sombraMensaje.Visible = true;
            mostrarMensaje((resultado.Trim().Equals("OK") ? "Cliente actualizado exitosamente" : resultado));


            gvGerentes.EditIndex = -1;
            cargaClientes();
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
                condicion += (condicion.Length > 0?" and ":"")+" apellido_paterno like '%" + TxtApellidoP.Text.Trim() + "%' ";
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
            FGCancelar.Visible = true;
            FGAgregar.Visible = true;
            FGActualizar.Visible = true;
            divFormularioG.Visible = false;
            Session["Condicion"] = "";
            cargaClientes();
        }

        //Muestra la ventana del buscador
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            btnLimpiarF.Visible = true;
            btnBuscarF.Visible = true;
            FGCancelar.Visible = false;
            FGAgregar.Visible = false;
            FGActualizar.Visible = false;
            divFormularioG.Visible = true;

            MasterFarmacia master = (MasterFarmacia)this.Master;
            master.mostrarMensaje(true);
            sombraMensaje.Visible = true;

        }
    }
}