using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace accesosIp.helper
{
    public class EmailSend
    {
        public Boolean EnviarMail(string Remitente, string destinatario1, string destinatario2, string destinatario3, string Titulo, string Mensaje, string adjunto)
        {
            Boolean enviado = false;
            MailMessage correo = new MailMessage();
            try
            {
                // De
                correo.From = new MailAddress(Remitente);
                // Para
                if (destinatario1 != "")
                    correo.To.Add(destinatario1);
                if (destinatario2 != "")
                    correo.To.Add(destinatario2);
                if (destinatario3 != "")
                    correo.To.Add(destinatario3);
                // Asunto
                correo.Subject = Titulo;
                // Cuerpo del correo
                correo.IsBodyHtml = true;
                correo.Body = Mensaje;
                //Attachment adj = new Attachment(adjunto);
                // correo.Attachments.Add(adj);

                System.Net.Mail.SmtpClient MailObj = new System.Net.Mail.SmtpClient();
                MailObj.Host = "email@gmail.com";
                MailObj.Host = "smtp.gmail.com";
                MailObj.EnableSsl = true;
                MailObj.Port = 587; // 587
                                    // MailObj.Credentials = New System.Net.NetworkCredential("reportes@delcorp.com.pe", "reportes")
                MailObj.UseDefaultCredentials = false;
                MailObj.Credentials = new System.Net.NetworkCredential(Remitente, "misgurus311");
                MailObj.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailObj.Timeout = 30000;
                MailObj.Send(correo);
                correo.Attachments.Clear();
                enviado = true;
            }
            catch (Exception ex)
            {
                enviado = false;
                Helper.CrearLog(ex.Message);
            }
            finally
            {
                correo.Attachments.Clear();
            }
            return enviado;
        }
    }
}
