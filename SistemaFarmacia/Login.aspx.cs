using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaFarmacia.Clases;
using System.Data;

namespace SistemaFarmacia
{
    public partial class Login : System.Web.UI.Page
    {
        Conexion conexion = new Conexion();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ValidaCorreo();

                Session.Clear();

                MasterFarmacia master = (MasterFarmacia)this.Master;
                master.mostrarbtnSalir(false);
                master.mostrarbtnRegresar(false);
            }
        }
        private void RegistrarScript()
        {

        }
        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            if (Usuario.Text.Length > 0 && Pass.Text.Length > 0)
            {

                if (!Usuario.Text.Equals("") && !Pass.Text.Equals(""))
                {
                    if (conexion.ConsultaUsuario(Usuario.Text, Pass.Text))
                    {
                        String permisos = conexion.ConsultaPermisos(Usuario.Text);
                        if (permisos.Length > 0)
                        {
                            Session.Add("Permisos", permisos);
                            String direccion = HttpContext.Current.Request.Url.ToString();
                            Response.Redirect("Principal.aspx");
                        }
                        else
                        {
                            lanzaScript("ModalUpdate('Mensaje','El usuario no cuenta con permisos, favor de contactar con el administrador')");
                        }
                    }
                    else
                    {
                        lanzaScript("ModalUpdate('Mensaje','El usuario o contraseña son incorrectos, favor de intentarlo nuevamente')");
                        Pass.Text = "";
                        //Response.Redirect("Login.aspx");
                    }
                }
            }
            else
            {
                lanzaScript("LoginError()");
            }
        }

        public void lanzaScript(String Script)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "X", "<script language='javascript'>" + Script + "</script>", false);
        }

        protected void btnCrearPass_Click(object sender, EventArgs e)
        {
            Boolean update = false;

            update = conexion.ConsultaContraseña(usuarioModal.Text, nombreModal.Text, apellidoModal.Text, apellidoModal2.Text, exampleInputPassword1.Text);

            if (update)
            {
                lanzaScript("ModalUpdate('Mensaje','La contraseña se modificó satisfactoriamente')");
            }
            else
            {
                lanzaScript("ModalUpdate('Error','No fue posible modificar la contraseña')");
            }
        }


        public void ValidaCorreo()
        {
            DatosCorreo dCorreo = new DatosCorreo();
            dCorreo = conexion.ConsultaDatosCorreo();

            //Se valida que sea el momento de enviar los correos
            if (conexion.ValidaDiaCorreo(dCorreo.DiasAntes))
            {
                //Actualizamos ultima ejecución
                conexion.ActualizaUltimoCorreo();

                EnviarCorreo enviaCorreo = new EnviarCorreo();

                DataSet datosCliente = conexion.TraerClientesDelMesSiguiente("");
                List<String> listaCorreos = new List<string>();

                foreach (DataRow dr in datosCliente.Tables[0].Rows)
                {
                    String temporal = dr["EMAIL"].ToString();
                    listaCorreos.Add(temporal);
                }
                if (listaCorreos.Count > 0)
                {
                    //String direccionImagen = Server.MapPath("/Imagenes/Correo/image.jpg");
                    //enviaCorreo.Enviar(listaCorreos, direccionImagen);
                    enviaCorreo.Enviar(listaCorreos, dCorreo.SMTP_IMAGEN);
                }
            }

        }


    }
}