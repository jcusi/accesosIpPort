using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accesosIp.Data;
using accesosIp.DBContexts;
using accesosIp.Filtros;
using accesosIp.helper;
using accesosIp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace accesosIp.Controllers
{
    
    public class RecuperarController : Controller
    {
        private readonly Repository _repository;
        private readonly AppDBContext _context;
        private readonly IActionContextAccessor _accesor;
        private readonly Helper _helper;
        private readonly EmailSend _send;
        public RecuperarController(Repository repository, AppDBContext context, IActionContextAccessor accesor)
        {
            _repository = repository;
            _context = context;
            _accesor = accesor;
            _helper = new Helper(_accesor);
            _send = new EmailSend();
        }
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RecuperarInput input)
        {
            if (input.sms == null)
            {
                var guid = Guid.NewGuid();
                var justNumbers = new String(guid.ToString().Where(Char.IsDigit).ToArray());
                var seed = int.Parse(justNumbers.Substring(0, 6));

                var random = new Random(seed);
                var value = random.Next(0, 6);
                _send.EnviarMail("br0adcasttumail@gmail.com", input.email, "", "", "Recuperar Contraseña", "su Codigo de envío es:" + seed.ToString(), "");
                ViewBag.tipo = "Correo";
                return View("Views/Recuperar/ValidandoCodigo.cshtml");
            }
            else
            {

            }
            return View();
        }
    }
}
