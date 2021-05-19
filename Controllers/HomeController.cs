using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using accesosIp.Models;
using accesosIp.Data;
using accesosIp.Entities;
using accesosIp.DBContexts;
using accesosIp.Extension;
using Microsoft.AspNetCore.Http;
using accesosIp.Filtros;
using accesosIp.helper;
using accesosIp.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace accesosIp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository _repository;
        private readonly AppDBContext _context;
         private readonly IActionContextAccessor _accesor;
        private readonly IDatabaseChangeNotificationService _SqlService;
        private readonly Helper _helper;
         public HomeController(Repository repository,AppDBContext context, IDatabaseChangeNotificationService sqlservice, IActionContextAccessor accesor)
        {
            _repository = repository;
            _context = context;
            _SqlService = sqlservice;
            _accesor = accesor;
            _helper = new Helper(_accesor);
        }
        [AuthorizationFilter]
        public IActionResult Index()
        {
                int valorEntero = -1;
                int _acceso = int.TryParse(_helper.DevolverSession(), out valorEntero) == true ? Convert.ToInt32(_helper.DevolverSession()) : -1;
                string _port = _helper.DevolverPort();
                var acceso = _context.Acceso.Where(x => x.sIdAcceso == _acceso &&
                x.nSession == 1 &&
                x.dtFechaExpiracion > DateTime.Now &&
                x.sPort == _port &&
                x.sIp == _accesor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString());
                if (acceso.Any())
                {
                    return View();
                }
                else
                {
                Helper.CrearLog("accesor: "+_acceso+", _port: "+_port+", ip:"+ _accesor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString());
                ViewBag.mensaje = "Comuniquese con soporte";
                    return RedirectToAction("Index", "Login");
                }
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult OutPut([FromBody] CerrarSesionInput data)
        {
            bool validado = false;
            int sesion = -1;
            sesion =  int.TryParse(_helper.DevolverSession(),out sesion)== true ? Convert.ToInt32(_helper.DevolverSession()):-1;
            if (data.usuarioMensaje == sesion)
            {
                validado = true;
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(-1);
                cookieOptions.SameSite = SameSiteMode.None;
                cookieOptions.Secure = true;
                cookieOptions.IsEssential = true;
                Response.Cookies.Append(SessionValor.SessionKeyAcceso,"", cookieOptions);
                Response.Cookies.Append(SessionValor.SessionKeyPersona, "", cookieOptions);
                Response.Cookies.Append(SessionValor.SessionPort, "");
            }
            return Json(new {status = validado,message = data.mensaje });
        }

        public IActionResult CloseConection()
        {
            return View("Views/CloseConection/Index.cshtml");
        }

        [AuthorizationFilter]
        public IActionResult CerrarSesion()
        {
            string idAcceso = "";

            if (HttpContext.Session.GetString(SessionValor.SessionKeyAcceso) != null)
            {
                Char[] michar = { '"', '\'' };
                idAcceso = HttpContext.Session.GetString(SessionValor.SessionKeyAcceso).Trim(michar);
            }
            else
            {
                idAcceso = Encriptacion.Desencrip(HttpContext.Request.Cookies[SessionValor.SessionKeyAcceso]);
            }

                tAcceso acceso = _context.Acceso.Single(d => d.sIdAcceso == Convert.ToInt32(idAcceso));// misession.Trim(new Char[] { ' ', '"', '\\' }));
                acceso.nSession = 0;
                //agregar un insert para el historico de accesos en una tabla historicodeaccesos
                _context.Acceso.Update(acceso);
                _context.Save();

              CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Append(SessionValor.SessionKeyAcceso, string.Empty);
            HttpContext.Session.Remove(SessionValor.SessionKeyAcceso);
            HttpContext.Session.Remove(SessionValor.SessionKeyIntentos);
            HttpContext.Session.Remove(SessionValor.SessionKeyPersona);
        
            return RedirectToAction("Index","Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
