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

namespace accesosIp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository _repository;
        private readonly AppDBContext _context;
       // private readonly IHttpContextAccessor _accesor;
         public HomeController(Repository repository,AppDBContext context)
        {
            _repository = repository;
            _context = context;
         //   _accesor = accesor;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult CerrarSesion()
        {
            string misession = HttpContext.Session.GetString(SessionValor.SessionKeyAcceso);
            tAcceso acceso = _context.Acceso.Single(d => d.sIdAcceso ==Guid.Parse(misession.Trim(new Char[] { ' ','"','\\'})));
            acceso.nSession = 0;
            //agregar un insert para el historico de accesos en una tabla historicodeaccesos
            _context.Acceso.Update(acceso);
            _context.Save();
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
