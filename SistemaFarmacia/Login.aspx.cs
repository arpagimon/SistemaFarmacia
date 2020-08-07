﻿using System;
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
            }
            else if (usuario == "")
            {
                preguntaS = conexion.TraerPreguntaS("", usuario, email);
                if (preguntaS == "")
                {
                    lanzaScript("ModalUpdate('Error','No fue posible obtener la pregunta de seguridad del usuario/correo proporcionado.')");
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
                    btnCambiarPass.Enabled = true;
                }
            }
            else if (email == "")
            {
                preguntaS = conexion.TraerPreguntaS("", usuario, email);
                if (preguntaS == "")
                {
                    lanzaScript("ModalUpdate('Error','No fue posible obtener la pregunta de seguridad del usuario/correo proporcionado.')");
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
                    btnCambiarPass.Enabled = true;
                }
            }
            else
            {
                preguntaS = conexion.TraerPreguntaS("", usuario, email);
                if (preguntaS == "")
                {
                    lanzaScript("ModalUpdate('Error','No fue posible obtener la pregunta de seguridad del usuario/correo proporcionado.')");
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
                    btnCambiarPass.Enabled = true;
                }
            }
            //Boolean update = false;

            //update = conexion.ConsultaContraseña(usuarioModal.Text, nombreModal.Text, apellidoModal.Text, apellidoModal2.Text, exampleInputPassword1.Text);

            //if (update)
            //{
            //    lanzaScript("ModalUpdate('Mensaje','La contraseña se modificó satisfactoriamente')");
            //}
            //else
            //{
            //    lanzaScript("ModalUpdate('Error','No fue posible modificar la contraseña')");
            //}
        }

        protected void btnCambiarPass_Click(object sender, EventArgs e)
        {
            Boolean update = false;

            update = conexion.CambiarContraseña(usuarioModal.Text, correoModal.Text, preguntaModal.Text, respuestaModal.Text, exampleInputPassword1.Text);

            if (update)
            {
                lanzaScript("ModalUpdate('Mensaje','La contraseña se modificó satisfactoriamente')");
                usuarioModal.Text = "";
                correoModal.Text = "";
                preguntaModal.Text = "";
                respuestaModal.Text = "";
                exampleInputPassword1.Text = "";
                usuarioModal.Enabled = true;
                correoModal.Enabled = true;
                respuestaModal.Enabled = false;
                exampleInputPassword1.Enabled = false;
                @continue.Visible = true;
                btnCambiarPass.Enabled = false;
            }
            else
            {
                lanzaScript("ModalUpdate('Error','No fue posible modificar la contraseña')");
                usuarioModal.Text = "";
                correoModal.Text = "";
                preguntaModal.Text = "";
                respuestaModal.Text = "";
                exampleInputPassword1.Text = "";
                usuarioModal.Enabled = true;
                correoModal.Enabled = true;
                respuestaModal.Enabled = false;
                exampleInputPassword1.Enabled = false;
                @continue.Visible = true;
                btnCambiarPass.Enabled = false;
            }
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

                DataSet datosCliente = conexion.TraerClientesDelMesSiguiente("",conexion.TraerEnvioCorreo());

                List<String> listaCorreos = new List<string>();
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

                }
                if (listaCorreos.Count > 0)
                {
                    //String direccionImagen = Server.MapPath("/Imagenes/Correo/image.jpg");
                    //enviaCorreo.Enviar(listaCorreos, direccionImagen);
                    bool ok = enviaCorreo.Enviar(listaCorreos, dCorreo.SMTP_IMAGEN);
                    if (ok && dCorreo.PRUEBAS == "0")
                    {
                        conexion.ActualizaCorreoEnvCliente(cadenaIDs);
                    }
                }
            }

        }


    }
}