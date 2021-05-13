using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using accesosIp.Data;
using accesosIp.DBContexts;
using accesosIp.Entities;
using accesosIp.Extension;
using accesosIp.Models;
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
        public LoginController(AppDBContext context,IActionContextAccessor accessor,Repository repository)
        {
            _context = context;
            _accessor = accessor;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginInput input)
        {
            string mensaje = "";
            try
            {
                var usuario = _context.Usuario.Where(s => s.sRuc == input.ruc && s.sDni == input.dni && s.clave == input.contrasenia);
                if (usuario.Any())
                {

                    //  var ippc = System.Net.Dns.GetHostEntry(_accessor.ActionContext.HttpContext.Connection.RemoteIpAddress).AddressList[3];
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
                        /*Validar si usuario ya accedído*/
                        int result = await _repository.IdValidarAcceso(Convert.ToString(user.sIdUsuario),input.ip);
                       
                        //0 existe en la bd para el dia de hoy pero con sesion cerrada
                        //-1 no existe en la bd para el dia de hoy
                        if (result == 0 || result == -1)
                        {
                            tAcceso acceso = new tAcceso();
                            acceso.sIdAcceso = Guid.NewGuid();
                            acceso.sIdUsuario = Guid.Parse(user.sIdUsuario.ToString());
                            acceso.sIp = input.ip;
                            acceso.sPort = input.remotePort;
                            acceso.dtFechaCreacion = DateTime.Now;
                            acceso.dtFechaExpiracion = DateTime.Now.AddDays(1);
                            acceso.skeysession = user.sIdUsuario + input.ip;
                            acceso.nSession = 1; //sesion iniciada
                            _context.Acceso.Add(acceso);
                            _context.Save();
                            HttpContext.Session.Set(SessionValor.SessionKeyPersona, user.sIdUsuario);
                            HttpContext.Session.Set(SessionValor.SessionKeyAcceso, acceso.sIdAcceso);
                            mensaje = "Bienvenido";
                            return RedirectToAction("Index", "Home");
                        }
                        else if(result == 1)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            mensaje = "ya tiene un acceso no intente ingresar a otra pc";
                        }
                    }
                }
                else
                {
                    // return Json(new { status = false, message = "Usuario o clave Incorrecta" });
                    mensaje = "Usuario o Clave Incorrecta";
                }
            }
            catch (Exception ex)
            {
                //mensaje = ex.Message + ex.InnerException;
                mensaje = "Comuniquese con soporte";
            }
           
            ViewBag.mensaje = mensaje;
            return View(input);
        }
    }
}