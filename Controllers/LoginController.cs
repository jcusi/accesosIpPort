using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using accesosIp.Data;
using accesosIp.DBContexts;
using accesosIp.Entities;
using accesosIp.Extension;
using accesosIp.Filtros;
using accesosIp.helper;
using accesosIp.Models;
using accesosIp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace accesosIp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IActionContextAccessor _accessor;
        private readonly AppDBContext _context;
        private readonly Repository _repository;
        private readonly Helper _helper;
        private readonly IDatabaseChangeNotificationService _SqlService;

        public LoginController(AppDBContext context,IActionContextAccessor accessor, IDatabaseChangeNotificationService sqlservice, Repository repository)
        {
            _context = context;
            _accessor = accessor;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _helper = new Helper(_accessor);
            _SqlService = sqlservice;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Validar()
        {
            return View("Views/Login/validarAcceso.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginInput input)
        {
            string mensaje = "";
            string navegador = "";
            int result = -10;
            Tuple<int,int, string> tuple = new Tuple<int,int, string>(-10,-1, "");
            Tuple<int, string> tupleUpdate = new Tuple<int, string>(-10, "");
            string IpCookie = "";
            string PortCookie = "";

            try
            {
              navegador = Request.Headers["User-Agent"].ToString();
            }
            catch (Exception)
            {

                navegador = "NoUserAgent";
            }

            try
            {
                var usuario = _context.Usuario.Where(s => s.sRuc == input.ruc && s.sDni == input.dni && s.clave == input.contrasenia);
                if (usuario.Any())
                {
                    string valorde = HttpContext.Session.GetString(SessionValor.SessionKeyAcceso);
                    //var ippc = System.Net.Dns.GetHostEntry(_accessor.ActionContext.HttpContext.Connection.RemoteIpAddress).AddressList[3];
                    //  string hostname = System.Net.Dns.GetHostEntry(_accessor.ActionContext.HttpContext.Connection.RemoteIpAddress).HostName;
                    input.ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
                    input.localIp = _accessor.ActionContext.HttpContext.Connection.LocalIpAddress.ToString();
                    input.localPort = _accessor.ActionContext.HttpContext.Connection.LocalPort.ToString();
                    input.remotePort = _accessor.ActionContext.HttpContext.Connection.RemotePort.ToString();
                    if (ModelState.IsValid)
                    {
                        // ViewBag.namehost = hostname;
                        //ViewBag.ippc = ippc;
                        ViewBag.id = _accessor.ActionContext.HttpContext.Connection.Id.ToString();
                        ViewBag.ip = input.ip;
                        ViewBag.localIp = input.localIp;
                        ViewBag.LocalPort = input.localPort;
                        ViewBag.RemotePort = input.remotePort;
                        ViewBag.ruc = input.ruc;
                        tusuario user = _context.Usuario.Single(d => d.sRuc == input.ruc && d.sDni == input.dni && d.clave == input.contrasenia);

                        //if (Request.Cookies[SessionValor.SessionIp] != null && Request.Cookies[SessionValor.SessionPort] != null)
                        //{
                        //    IpCookie = Request.Cookies[SessionValor.SessionIp];
                        //    PortCookie = Request.Cookies[SessionValor.SessionPort];
                        //}
                        //else
                        //{
                            IpCookie = input.ip;
                            PortCookie = input.remotePort;
                        //}

                        /*Validar si usuario ya accedído*/
                        tuple = await _repository.IdValidarAcceso(Convert.ToString(user.sIdUsuario),IpCookie,PortCookie,navegador);
                        result = tuple.Item1;
                        Helper.CrearLog("resultado acceso: " + tuple.Item1.ToString() + ", accceso: " + tuple.Item2.ToString() + ", port: " + tuple.Item3);
                        if (result == 100)
                        {
                            Guid usuarios = Guid.NewGuid();
                            tAcceso acceso = new tAcceso();
                            acceso.sIdUsuario = Convert.ToInt32(user.sIdUsuario);
                            acceso.sIp = input.ip;
                            acceso.sPort = input.remotePort;
                            acceso.dtFechaCreacion = DateTime.Now;
                            acceso.dtFechaExpiracion = DateTime.Now.AddDays(1);
                            acceso.sNavegador = navegador;
                            acceso.nSession = 1; //sesion iniciada
                            _context.Acceso.Add(acceso);
                            _context.Save();
                            HttpContext.Session.Set(SessionValor.SessionKeyPersona,Encriptacion.Encriptar(user.sIdUsuario.ToString()));
                            HttpContext.Session.Set(SessionValor.SessionKeyAcceso, acceso.sIdAcceso);
                            
                            _helper.cookie_guardar(Encriptacion.Encriptar(user.sIdUsuario.ToString()),
                                Encriptacion.Encriptar(acceso.sIdAcceso.ToString()),
                                Encriptacion.Encriptar(acceso.sPort.ToString()),
                                Response);
                            mensaje = "Bienvenido";
                            Helper.CrearLog("logro ingresar al Home");
                            return RedirectToAction("Index", "Home");
                        }
                        else if (result == 0 || result == 10 || result == 20)
                        {
                            int idAcceso = -1;                            
                                idAcceso = tuple.Item2;
                            tAcceso acceso = _context.Acceso.Single(d => d.sIdAcceso == Convert.ToInt32(idAcceso));
                            switch (result)
                            {
                                case 0:
                                    acceso.nSession = 1;
                                    break;
                                case 10:
                                    acceso.sIp = input.ip;
                                    acceso.sPort = input.remotePort;
                                    acceso.nSession = 1;
                                    break;
                                case 20:
                                    acceso.sNavegador = navegador;
                                    acceso.sIp = input.ip;
                                    acceso.sPort = input.remotePort;
                                    acceso.nSession = 1;
                                    break;
                            }
                            _context.Acceso.Update(acceso);
                            _context.Save();

                                _helper.cookie_guardar(Encriptacion.Encriptar(user.sIdUsuario.ToString()),
                                Encriptacion.Encriptar(idAcceso.ToString()),
                                Encriptacion.Encriptar(acceso.sPort.ToString()),
                                Response);
                            Helper.CrearLog("logro ingresar al Home");
                            return RedirectToAction("Index", "Home");
                        }
                        else if(result == 1)
                        {
                            HttpContext.Session.Set(SessionValor.SessionKeyPersona, user.sIdUsuario);
                            _helper.cookie_guardar(Encriptacion.Encriptar(user.sIdUsuario.ToString()),
                                Encriptacion.Encriptar(tuple.Item2.ToString()),
                                Encriptacion.Encriptar(tuple.Item3.ToString()),
                                Response);
                            Helper.CrearLog("logro ingresar al Home");
                            return RedirectToAction("Index", "Home");
                        }
                        else if (result == 3 || result == 2)
                        {
                            HttpContext.Session.Set(SessionValor.SessionKeyPersona, user.sIdUsuario);
                            _helper.cookie_guardar(Encriptacion.Encriptar(user.sIdUsuario.ToString()),
                                Encriptacion.Encriptar(tuple.Item2.ToString()),
                                Encriptacion.Encriptar(tuple.Item3.ToString()),
                                Response);
                            Helper.CrearLog("logro ingresar al validar");
                            return RedirectToAction("Validar", "Login");
                            //mensaje = "Tiene una Sesión abierta en otro navegador, Desea cambiarlo aquí";
                        }
                    }
                }
                else
                {
                     //return Json(new { status = false, message = "Usuario o clave Incorrecta" });
                    mensaje = "Usuario o Clave Incorrecta";
                }
            }
            catch (Exception ex)
            {
                Helper.CrearLog("ocurrio un error");
                Helper.CrearLog(ex.Message+"|*|"+ex.InnerException,"error");

                //mensaje = ex.Message + ex.InnerException;
                mensaje = "Comuniquese con soporte";
            }
           
            ViewBag.mensaje = mensaje;
            return View(input);
        }
        [AuthorizationFilter]
        public async Task<IActionResult> actualizarSesion()
        {
            string ip = "", port = "", navegador = "";
            Tuple<int, string> tuple = new Tuple<int, string>(0, "");
            try
            {
                navegador = Request.Headers["User-Agent"].ToString();
            }
            catch (Exception)
            {
                navegador = "NoUserAgent";
            }

            //if (Request.Cookies[SessionValor.SessionIp] != null && Request.Cookies[SessionValor.SessionPort] != null)
            //{
            //    ip = Request.Cookies[SessionValor.SessionIp];
            //    port = Request.Cookies[SessionValor.SessionPort];
            //}
            ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            port = _accessor.ActionContext.HttpContext.Connection.RemotePort.ToString();

            string idUsuario = _helper.DevolverUsuario();


                tuple = await _repository.acceso_Actualizar(idUsuario, ip, port, navegador);
                if (tuple.Item1 == 1)
                {
                    HttpContext.Session.Set(SessionValor.SessionKeyAcceso, tuple.Item2);
                _helper.cookie_guardar(Encriptacion.Encriptar(idUsuario.ToString()),
                    Encriptacion.Encriptar(tuple.Item2),
                    Encriptacion.Encriptar(port),
                    Response);
                _SqlService.Config(Convert.ToInt32(tuple.Item2));
                return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.mensaje = "No se completo la operación";
                }
            

            return View("Views/Login/validarAcceso.cshtml");
        }
    }
}