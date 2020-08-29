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
                //ValidaCorreo();
                ValidaCitasManana();

                Session.Clear();

                MasterFarmacia master = (MasterFarmacia)this.Master;
                master.mostrarbtnSalir(false);
                master.mostrarbtnRegresar(false);
                master.cambiarLblTitle("<h1>Farmacia</h1>");
            }

            Session.Timeout = 1440;

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

                            ///////Esto va a cambiar/////
                            Session["usuario"] = Usuario.Text;
                            ///////Esto va a cambiar/////


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
                        Pass.Text = "";
                        lanzaScript("ModalUpdate('Mensaje','El usuario o contraseña son incorrectos, favor de intentarlo nuevamente')");
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
            String preguntaS = "";
            String usuario = usuarioModal.Text;
            String email = correoModal.Text;

            if (usuario == "" && email == "")
            {
                lanzaScript("ModalUpdate('Error','Favor de proporcionar el nombre de usuario o correo electrónico para continuar.')");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ModalOlvide();", true);
            }
            else if (usuario == "")
            {
                preguntaS = conexion.TraerPreguntaS("", usuario, email);
                if (preguntaS == "")
                {
                    lanzaScript("ModalUpdate('Error','No fue posible obtener la pregunta de seguridad del usuario/correo proporcionado.')");
                    btnLimpiar_Click(sender, e);
                }
                else
                {
                    usuarioModal.Enabled = false;
                    correoModal.Enabled = false;
                    respuestaModal.Enabled = true;
                    preguntaModal.Text = preguntaS;
                    exampleInputPassword1.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ModalOlvide();", true);
                    @continue.Visible = false;
                    btnValidar.Visible = true;
                    divPreguntaS.Visible = true;
                    divRespuestaS.Visible = true;

                }
            }
            else if (email == "")
            {
                preguntaS = conexion.TraerPreguntaS("", usuario, email);
                if (preguntaS == "")
                {
                    lanzaScript("ModalUpdate('Error','No fue posible obtener la pregunta de seguridad del usuario/correo proporcionado.')");
                    btnLimpiar_Click(sender, e);
                }
                else
                {
                    usuarioModal.Enabled = false;
                    correoModal.Enabled = false;
                    preguntaModal.Text = preguntaS;
                    respuestaModal.Enabled = true;
                    exampleInputPassword1.Enabled = true;
                    //Volver a abrir modal
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ModalOlvide();", true);
                    @continue.Visible = false;
                    btnValidar.Visible = true;
                    divPreguntaS.Visible = true;
                    divRespuestaS.Visible = true;
                }
            }
            else
            {
                preguntaS = conexion.TraerPreguntaS("", usuario, email);
                if (preguntaS == "")
                {
                    lanzaScript("ModalUpdate('Error','No fue posible obtener la pregunta de seguridad del usuario/correo proporcionado.')");
                    btnLimpiar_Click(sender, e);
                }
                else
                {
                    usuarioModal.Enabled = false;
                    correoModal.Enabled = false;
                    preguntaModal.Text = preguntaS;
                    respuestaModal.Enabled = true;
                    exampleInputPassword1.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ModalOlvide();", true);
                    @continue.Visible = false;
                    btnValidar.Visible = true;
                    divPreguntaS.Visible = true;
                    divRespuestaS.Visible = true;
                }
            }
        }

        protected void btnCambiarPass_Click(object sender, EventArgs e)
        {
            if (exampleInputPassword1.Text == InputPasswordR.Text)
            {
                Boolean update = false;

                update = conexion.CambiarContraseña(usuarioModal.Text, correoModal.Text, exampleInputPassword1.Text);

                if (update)
                {
                    lanzaScript("ModalUpdate('Mensaje','La contraseña se modificó satisfactoriamente')");
                    usuarioModal.Text = "";
                    correoModal.Text = "";
                    preguntaModal.Text = "";
                    respuestaModal.Text = "";
                    exampleInputPassword1.Text = "";
                    exampleInputPassword1.Attributes.Remove("style");
                    InputPasswordR.Text = "";
                    InputPasswordR.Attributes.Remove("style");
                    usuarioModal.Enabled = true;
                    correoModal.Enabled = true;
                    respuestaModal.Enabled = false;
                    exampleInputPassword1.Enabled = false;
                    @continue.Visible = true;
                    btnValidar.Visible = false;
                    btnCambiarPass.Visible = false;
                    divNContra.Visible = false;
                    divPreguntaS.Visible = false;
                    divRespuestaS.Visible = false;
                    divRContra.Visible = false;
                }
                else
                {
                    lanzaScript("ModalUpdate('Error','No fue posible modificar la contraseña')");
                    btnLimpiar_Click(sender, e);
                }
            }
            else
            {
                lanzaScript("ModalUpdate('Error','Las contaseñas ingresadas no coinciden')");
                exampleInputPassword1.Attributes.Add("style", "border: 1px red solid;");
                InputPasswordR.Attributes.Add("style", "border: 1px red solid;");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ModalOlvide();", true);
            }
        }

        protected void btnValidaPregunta_Click(object sender, EventArgs e)
        {
            String usuario = usuarioModal.Text;
            String email = correoModal.Text;
            String respuesta = respuestaModal.Text;
            if (conexion.EvaluaRespuestaS(usuario, email, respuesta))
            {
                respuestaModal.Enabled = false;
                btnValidar.Visible = false;
                btnCambiarPass.Visible = true;
                divNContra.Visible = true;
                divRContra.Visible = true;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ModalOlvide();", true);
            }
            else
            {
                lanzaScript("ModalUpdate('Error','La respuesta a la pregunta de seguridad es erronea.')");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ModalOlvide();", true);
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            usuarioModal.Text = "";
            correoModal.Text = "";
            preguntaModal.Text = "";
            respuestaModal.Text = "";
            exampleInputPassword1.Text = "";
            exampleInputPassword1.Attributes.Remove("style");
            InputPasswordR.Text = "";
            InputPasswordR.Attributes.Remove("style");
            usuarioModal.Enabled = true;
            correoModal.Enabled = true;
            respuestaModal.Enabled = false;
            exampleInputPassword1.Enabled = false;
            @continue.Visible = true;
            btnValidar.Visible = false;
            btnCambiarPass.Visible = false;
            divNContra.Visible = false;
            divPreguntaS.Visible = false;
            divRespuestaS.Visible = false;
            divRContra.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ModalOlvide();", true);
        }






        public void ValidaCorreo()
        {
            DatosCorreo dCorreo = conexion.ConsultaDatosCorreo();

            //Se valida que sea el momento de enviar los correos
            if (conexion.ValidaDiaCorreo(dCorreo.DiasAntes))
            {
                //Actualizamos ultima ejecución
                conexion.ActualizaUltimoCorreo();

                EnviarCorreo enviaCorreo = new EnviarCorreo();

                DataSet datosCliente = conexion.TraerClientesDelMesSiguiente(" enviar_correo = '1' ", conexion.TraerEnvioCorreo());

                List<String> listaCorreos = new List<string>();
                List<String> listaIDs = new List<string>();
                String cadenaIDs = "";

                foreach (DataRow dr in datosCliente.Tables[0].Rows)
                {
                    String temporal = dr["EMAIL"].ToString();
                    listaCorreos.Add(temporal);
                    String temporal2 = dr["CHECK_EMAIL"].ToString();
                    if(cadenaIDs.Length == 0)
                    {
                        cadenaIDs += temporal2;
                    }else
                    {
                        cadenaIDs += "," + temporal2;
                    }
                    String temporalID = dr["ID_CLIENTE"].ToString();
                    listaIDs.Add(temporalID);

                }
                if (listaCorreos.Count > 0)
                {
                    //String direccionImagen = Server.MapPath("/Imagenes/Correo/image.jpg");
                    //enviaCorreo.Enviar(listaCorreos, direccionImagen);
                    bool ok = enviaCorreo.Enviar(listaCorreos, dCorreo.SMTP_IMAGEN);
                    if (ok && dCorreo.PRUEBAS == "0")
                    {
                        conexion.ActualizaCorreoEnvCliente(cadenaIDs);
                        conexion.GuardaBitacora(Session["usuario"].ToString(), listaIDs, dCorreo);
                    }
                }
            }

        }

        public void ValidaCitasManana()
        {
            DatosCorreo dCorreo = conexion.ConsultaDatosCorreo();

            //Se valida que sea momento de enviar recordatorio de las citas de mañana
            if (conexion.ValidaCitaCorreo())
            {

                EnviarCorreo enviaCorreo = new EnviarCorreo();

                DataSet datosCliente = conexion.TraerClientesConCitaM();

                List<DatosCita> listaDatosCita = new List<DatosCita>();

                foreach (DataRow dr in datosCliente.Tables[0].Rows)
                {
                    String id_cita = dr["ID_cita"].ToString();
                    String id_cliente = dr["ID_CLIENTE"].ToString();
                    String nombre_Cliente = dr["nombre"].ToString();
                    String appaterno_cliente = dr["apellido_paterno"].ToString();
                    String apmaterno_cliente = dr["apellido_materno"].ToString();
                    String email = dr["email"].ToString();
                    String fecha = dr["fecha"].ToString();
                    String hora_in = dr["hora_inicio"].ToString();

                    DatosCita citaTemporal = new DatosCita();
                    citaTemporal.ID_Cliente = int.Parse(id_cliente);
                    citaTemporal.ID_Cita = int.Parse(id_cita);
                    citaTemporal.Nombre = nombre_Cliente;
                    citaTemporal.ApMaterno = apmaterno_cliente;
                    citaTemporal.ApPaterno = appaterno_cliente;
                    citaTemporal.Correo = email;
                    citaTemporal.Fecha = fecha;
                    citaTemporal.Hora_Inicio = hora_in;

                    listaDatosCita.Add(citaTemporal);

                }
                if (listaDatosCita.Count > 0)
                {
                    bool ok = enviaCorreo.EnviarRecordatorio(listaDatosCita);
                    //if (ok && dCorreo.PRUEBAS == "0")
                    //{
                        foreach (DatosCita item in listaDatosCita)
                        {
                            //Marca que ya se evió el recordatorio al cliente
                            conexion.ActualizaRecordatorio(item.ID_Cita);
                        }
                    //}
                }
            }

        }
    }
}