using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;       // No olvidar.
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace SistemaFarmacia.Clases
{
    public class EnviarCorreo
    {

        public Boolean Enviar(List<String> Correo, String Archivo)
        {
            SistemaFarmacia.Conexion connMysql = new SistemaFarmacia.Conexion();
            DatosCorreo datosCorreo_ = new DatosCorreo();
            datosCorreo_ = connMysql.ConsultaDatosCorreo();

            //Configuración correo
            Boolean enviado = true;
            int puerto = 465;

            // CONFIGURACION DEL MENSAJE
            MailMessage _Message = new MailMessage();
            _Message.From = new System.Net.Mail.MailAddress(datosCorreo_.SMTP_CORREO); //Quien lo envía

            if (datosCorreo_.PRUEBAS.Equals(0))
            {
                _Message.To.Add(Correo[0]); //Cuenta de Correo al que se le quiere enviar el e-mail
                if (Correo.Count > 1)
                {
                    Boolean primer = true;
                    foreach (string email in Correo)
                    {
                        if (!primer)
                        {
                            _Message.CC.Add(new MailAddress(email));
                        }
                        primer = false;
                    }
                }
            }else
            {
                _Message.To.Add(datosCorreo_.SMTP_CORREO_PRUEBA);
            }

            _Message.Subject = datosCorreo_.SMTP_SUJETO;  //'Sujeto del e-mail


            
            AlternateView plainView =
                AlternateView.CreateAlternateViewFromString(datosCorreo_.SMTP_MENSAJE, Encoding.UTF8, MediaTypeNames.Text.Plain);

            string html = "<div style='text-align:center;'><h2>" + datosCorreo_.SMTP_MENSAJE + "</h2>" +
               "<img src='cid:imagen' /><br/>"+
               "<h3 style='text-align: center;'>" + datosCorreo_.SMTP_FIRMA.Replace("\n","<br/>") + "</h3></div>";

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);

            LinkedResource img = new LinkedResource(@Archivo, MediaTypeNames.Image.Jpeg);
            img.ContentId = "imagen";
            htmlView.LinkedResources.Add(img);
            _Message.AlternateViews.Add(plainView);
            _Message.AlternateViews.Add(htmlView);

            //_Message.Body = datosCorreo_.SMTP_MENSAJE; //'contenido del mail
            _Message.IsBodyHtml = true;
            _Message.Priority = System.Net.Mail.MailPriority.Normal;
            _Message.Attachments.Add(new Attachment(Archivo));
            

            //CONFIGURACIÓN DEL STMP
            try
            {
                puerto = int.Parse(datosCorreo_.SMTP_PUERTO);
            }
            catch (Exception ex)
            {
            }
                 
            SmtpClient _SMTP = new SmtpClient(datosCorreo_.SMTP_HOST,puerto);
            _SMTP.Credentials = new System.Net.NetworkCredential(datosCorreo_.SMTP_CORREO, datosCorreo_.SMTP_PASS);
            _SMTP.EnableSsl = Boolean.Parse(datosCorreo_.SMTP_SSL);

            //'ENVIO
            try{
                _SMTP.Send(_Message);
            }
            catch (System.Net.Mail.SmtpException Ex){
                String error1 = Ex.Message;
                connMysql.guardaError("EnviarCorreo","Send",error1);
                enviado = false;
            }
            _SMTP.Dispose();

            return enviado;
        }
    }
}