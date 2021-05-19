using accesosIp.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace accesosIp.helper
{
    public class Helper
    {
        private IActionContextAccessor _accesor;
        public Helper(IActionContextAccessor accesor)
        {
            _accesor = accesor;
        }
        public bool cookie_guardar(string keyPersona,string keyAcceso,string port,HttpResponse response)
        {
            //cookies para establecer el navegador
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            cookieOptions.SameSite = SameSiteMode.None;
            cookieOptions.Secure = true;
            cookieOptions.IsEssential = true;
            response.Cookies.Append(SessionValor.SessionKeyAcceso, keyAcceso, cookieOptions);
            response.Cookies.Append(SessionValor.SessionKeyPersona, keyPersona, cookieOptions);
            response.Cookies.Append(SessionValor.SessionPort,port,cookieOptions);
            return true;
        }

        public string DevolverUsuario()
        {
            Char[] michar = { '"', '\'' };
            string idUsuario = "-1";
            if (_accesor.ActionContext != null)
            {
                if (_accesor.ActionContext.HttpContext.Session.GetString(SessionValor.SessionKeyPersona) != null)
                {
                    idUsuario = _accesor.ActionContext.HttpContext.Session.GetString(SessionValor.SessionKeyPersona).Trim(michar);
                }
                else
                {
                    idUsuario = Encriptacion.Desencrip(_accesor.ActionContext.HttpContext.Request.Cookies[SessionValor.SessionKeyPersona]);
                }
            }
            return idUsuario;
        }
        public string DevolverSession()
        {
            Char[] michar = { '"', '\'' };
            string sesion = "-1";
            if (_accesor.ActionContext != null)
            {
                if (_accesor.ActionContext.HttpContext.Session.GetString(SessionValor.SessionKeyAcceso) != null)
                {
                    sesion = _accesor.ActionContext.HttpContext.Session.GetString(SessionValor.SessionKeyAcceso).Trim(michar);
                }
                else
                {
                    sesion = Encriptacion.Desencrip(_accesor.ActionContext.HttpContext.Request.Cookies[SessionValor.SessionKeyAcceso]);
                }
            }
            return sesion;
        }
        public string DevolverPort()
        {
            Char[] michar = { '"', '\'' };
            string port = "";
            if (_accesor.ActionContext != null)
            {
                if (_accesor.ActionContext.HttpContext.Session.GetString(SessionValor.SessionPort) != null)
                {
                    port = _accesor.ActionContext.HttpContext.Session.GetString(SessionValor.SessionPort).Trim(michar);
                }
                else
                {
                    port = Encriptacion.Desencrip(_accesor.ActionContext.HttpContext.Request.Cookies[SessionValor.SessionPort]);
                }
            }
            return port;
        }

        public static void CrearLog(string texto,string ruta ="")
        {
            try
            {
                if (ruta == "")
                {
                    if (File.Exists(@"C:\logAccesos\logfile.txt"))
                    {
                        editar_archivo(texto, "C:\\logAccesos\\logfile.txt");
                    }
                    else
                    {
                        crear_archivo(texto, "C:\\logAccesos\\logfile.txt");
                    }
                }
                else
                {
                    crear_archivo(texto, "C:\\logAccesos\\logfile");
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        private static void editar_archivo(string texto,string ruta)
        {
            string rutaCompleta = ruta;

            texto = texto + " fecha:   " + DateTime.Now;
            using (StreamWriter file = new StreamWriter(rutaCompleta, true))
            {
                file.WriteLine(texto); //se agrega información al documento

                file.Close();
            }
        }
        private static void crear_archivo(string texto, string ruta)
        {
            string rutaCompleta = ruta+"_Error_"+ Convert.ToDateTime(DateTime.Now).ToString("yyMMdd_Hmmss_zzz")+".txt";
            using (StreamWriter mylogs = File.AppendText(rutaCompleta))         //se crea el archivo
            {

                //se adiciona alguna información y la fecha


                DateTime dateTime = new DateTime();
                dateTime = DateTime.Now;
                string strDate = Convert.ToDateTime(dateTime).ToString("yyMMdd");

                mylogs.WriteLine(texto + ", fecha: " + strDate);

                mylogs.Close();


            }
        }

    }
}
