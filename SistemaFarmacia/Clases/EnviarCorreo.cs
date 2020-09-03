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


        public Boolean EnviarSelectivo(List<String> Correo, String Archivo)
        {
            SistemaFarmacia.Conexion connMysql = new SistemaFarmacia.Conexion();
            DatosCorreo datosCorreo_ = new DatosCorreo();
            datosCorreo_ = connMysql.ConsultaDatosCorreoSelec();

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
            }
            else
            {
                _Message.To.Add(datosCorreo_.SMTP_CORREO_PRUEBA);
            }

            _Message.Subject = datosCorreo_.SMTP_SUJETO;  //'Sujeto del e-mail



            AlternateView plainView =
                AlternateView.CreateAlternateViewFromString(datosCorreo_.SMTP_MENSAJE, Encoding.UTF8, MediaTypeNames.Text.Plain);

            string html = "<div style='text-align:center;'><h2>" + datosCorreo_.SMTP_MENSAJE + "</h2>" +
               "<img src='cid:imagen' /><br/>" +
               "<h3 style='text-align: center;'>" + datosCorreo_.SMTP_FIRMA.Replace("\n", "<br/>") + "</h3></div>";

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);

            LinkedResource img = new LinkedResource(@Archivo, MediaTypeNames.Image.Jpeg);
            img.ContentId = "imagen";
            htmlView.LinkedResources.Add(img);
            _Message.AlternateViews.Add(plainView);
            _Message.AlternateViews.Add(htmlView);

            //Message.Body = datosCorreo.SMTP_MENSAJE; //'contenido del mail
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

            SmtpClient _SMTP = new SmtpClient(datosCorreo_.SMTP_HOST, puerto);
            _SMTP.Credentials = new System.Net.NetworkCredential(datosCorreo_.SMTP_CORREO, datosCorreo_.SMTP_PASS);
            _SMTP.EnableSsl = Boolean.Parse(datosCorreo_.SMTP_SSL);

            //'ENVIO
            try
            {
                _SMTP.Send(_Message);
            }
            catch (System.Net.Mail.SmtpException Ex)
            {
                String error1 = Ex.Message;
                connMysql.guardaError("EnviarCorreo", "Send", error1);
                enviado = false;
            }
            _SMTP.Dispose();

            return enviado;
        }

        public Boolean EnviarRecordatorio(List<DatosCita> cita)
        {
            SistemaFarmacia.Conexion connMysql = new SistemaFarmacia.Conexion();
            DatosCorreo datosCorreo = new DatosCorreo();
            datosCorreo = connMysql.ConsultaDatosCorreo();

            //Configuración correo
            Boolean enviado = true;
            int puerto = 465;

            // CONFIGURACION DEL MENSAJE
            MailMessage Message = new MailMessage();
            Message.From = new System.Net.Mail.MailAddress(datosCorreo.SMTP_CORREO); //Quien lo envía

            if (cita.Count > 0)
            {
                foreach (DatosCita item in cita)
                {

                    if (datosCorreo.PRUEBAS.Equals(0))
                    {
                        Message.To.Add(item.Correo); //Cuenta de Correo al que se le quiere enviar el e-mail
                    }else
                    {
                        Message.To.Add(datosCorreo.SMTP_CORREO_PRUEBA);
                    }

                    Message.Subject = "Recordatorio de cita para el día de mañana";  //'Asunto del e-mail

                    String datos_cita = "<h3>Datos de la cita: </h3>" +
                        "<p>Nombre del paciente: <strong>" + item.Nombre + " " + item.ApPaterno + " " + item.ApMaterno + "</strong><br/>" +
                        "Fecha: <strong>" + item.Fecha + "</strong><br/>Hora: <strong>" + item.Hora_Inicio + "</strong></p>";

                    AlternateView plainView =
                            AlternateView.CreateAlternateViewFromString("Esto es un recordatorio de su cita", Encoding.UTF8, MediaTypeNames.Text.Plain);

                    string html = "<div style='text-align:center;'><h2>" + datosCorreo.SMTP_MENSAJE + "</h2>" +
                       datos_cita + "< img src = 'cid:imagen' /><br/>" +
                       "<h3 style='text-align: center;'>" + datosCorreo.SMTP_FIRMA.Replace("\n", "<br/>") + "</h3></div>";
                    

                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);

                    LinkedResource img = new LinkedResource(datosCorreo.SMTP_IMAGEN, MediaTypeNames.Image.Jpeg);
                    img.ContentId = "imagen";
                    htmlView.LinkedResources.Add(img);
                    Message.AlternateViews.Add(plainView);
                    Message.AlternateViews.Add(htmlView);
                    Message.IsBodyHtml = true;
                    Message.Priority = System.Net.Mail.MailPriority.Normal;
                    Message.Attachments.Add(new Attachment(datosCorreo.SMTP_IMAGEN));

                    //CONFIGURACIÓN DEL STMP
                    try
                    {
                        puerto = int.Parse(datosCorreo.SMTP_PUERTO);
                    }
                    catch (Exception ex)
                    {
                    }

                    SmtpClient SMTP = new SmtpClient(datosCorreo.SMTP_HOST, puerto);
                    SMTP.Credentials = new System.Net.NetworkCredential(datosCorreo.SMTP_CORREO, datosCorreo.SMTP_PASS);
                    SMTP.EnableSsl = Boolean.Parse(datosCorreo.SMTP_SSL);

                    //'ENVIO
                    try
                    {
                        SMTP.Send(Message);
                    }
                    catch (System.Net.Mail.SmtpException Ex)
                    {
                        String error1 = Ex.Message;
                        connMysql.guardaError("EnviarCorreo", "Send", error1);
                        enviado = false;
                    }

                    SMTP.Dispose();
                }
            }
            return enviado;
        }
    }
}
