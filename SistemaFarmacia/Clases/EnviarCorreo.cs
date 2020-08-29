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
                        Message.Subject = "Recordatorio de cita para el día de mañana";  //'Sujeto del e-mail



                        AlternateView plainView =
                            AlternateView.CreateAlternateViewFromString("Esto es un recordatorio de su cita", Encoding.UTF8, MediaTypeNames.Text.Plain);

                        string html = "<div style='text-align:center; background:rgb(65,97,156, 0.9); color: #fff; padding: 10px 0'><h2>Recordatorio de cita</h2></div>" +
                           "<br/>" +
                           "<div>  <h3> Hola,  " + item.Nombre + " " + item.ApPaterno + " " + item.ApMaterno + ":</h3>" +
                           "<p> Le recordamos que tiene una cita para el d&iacute;a <strong>" + item.Fecha +
                           "</strong> a las <strong>" + item.Hora_Inicio + "</strong>.Le mandamos un cordial saludo.</p> </div>" +
                           "<hr>" +
                           "<h3 style = 'text-align: center;'> " + datosCorreo.SMTP_FIRMA + " </h3> ";


                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);

                        Message.AlternateViews.Add(plainView);
                        Message.AlternateViews.Add(htmlView);
                        Message.IsBodyHtml = true;
                        Message.Priority = System.Net.Mail.MailPriority.Normal;


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
            //}
            //else
            //{
            //    Message.To.Add(datosCorreo.SMTP_CORREO_PRUEBA);

            //    Message.Subject = "Recordatorio de cita para el día de mañana";  //'Sujeto del e-mail



            //    AlternateView plainView =
            //        AlternateView.CreateAlternateViewFromString("Esto es un recordatorio de su cita", Encoding.UTF8, MediaTypeNames.Text.Plain);

            //    string html = "<div style='text - align: center; background: rgb(65, 97, 156, 0.9); color: #fff; padding: 10px 0;'>" +
            //            "<h2> Recordatorio de cita</h2></div>" +
            //            "<div> <h3> Hola, (Nombre)(Apellido Paterno) (Apellido Materno):</h3>" +
            //            "<p> Le recordamos que tiene una cita para el d&iacute;a <strong>(Fecha) </strong> a las <strong>(Hora de inicio) </strong>." +
            //            "Le mandamos un cordial saludo.</p> </div> <hr/> <h3 style = 'text-align: center;'> " + datosCorreo.SMTP_FIRMA + " </h3> ";

            //    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);

            //    Message.AlternateViews.Add(plainView);
            //    Message.AlternateViews.Add(htmlView);
            //    Message.IsBodyHtml = true;
            //    Message.Priority = System.Net.Mail.MailPriority.Normal;
                
            //    //CONFIGURACIÓN DEL STMP
            //    try
            //    {
            //        puerto = int.Parse(datosCorreo.SMTP_PUERTO);
            //    }
            //    catch (Exception ex)
            //    {
            //    }

            //    SmtpClient SMTP = new SmtpClient(datosCorreo.SMTP_HOST, puerto);
            //    SMTP.Credentials = new System.Net.NetworkCredential(datosCorreo.SMTP_CORREO, datosCorreo.SMTP_PASS);
            //    SMTP.EnableSsl = Boolean.Parse(datosCorreo.SMTP_SSL);

            //    //'ENVIO
            //    try
            //    {
            //        SMTP.Send(Message);
            //    }
            //    catch (System.Net.Mail.SmtpException Ex)
            //    {
            //        String error1 = Ex.Message;
            //        connMysql.guardaError("EnviarCorreo", "Send", error1);
            //        enviado = false;
            //    }
            //    SMTP.Dispose();
            //}



            return enviado;
        }
    }
}
