using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public LoginController(IActionContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginInput input)
        {
            input.ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            input.localIp = _accessor.ActionContext.HttpContext.Connection.LocalIpAddress.ToString();
            input.localPort = _accessor.ActionContext.HttpContext.Connection.LocalPort.ToString();
            input.remotePort = _accessor.ActionContext.HttpContext.Connection.RemotePort.ToString();
            if (ModelState.IsValid)
            {
                ViewBag.ip = input.ip;
                ViewBag.localIp = input.localIp;
                ViewBag.LocalPort = input.localPort;
                ViewBag.RemotePort = input.remotePort;
                ViewBag.ruc = input.ruc;
            }
            return View();
        }
    }
}