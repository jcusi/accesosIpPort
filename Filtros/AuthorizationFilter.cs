using accesosIp.Data;
using accesosIp.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accesosIp.Filtros
{
    public class AuthorizationFilter: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filtroContext)
        {
            //!filtroContext.HttpContext.Session.TryGetValue(SessionValor.SessionKeyPersona,out byte[] val) ||

            if (  filtroContext.HttpContext.Session == null || 
                 filtroContext.HttpContext.Request.Cookies[SessionValor.SessionKeyAcceso] == null ||
                 filtroContext.HttpContext.Request.Cookies[SessionValor.SessionKeyPersona] == null)
               {

                helper.Helper.CrearLog("valores nulos");
                filtroContext.Result =
                new RedirectToRouteResult(new RouteValueDictionary(new
                {

                    controller = "Login",
                    action = "Index"
                }));
            }
            //else
            //{
            //    bool boolValidar = validarAcceso(Convert.ToInt32(helper.Encriptacion.Desencrip(filtroContext.HttpContext.Request.Cookies[SessionValor.SessionKeyPersona])),
            //           Convert.ToInt32(helper.Encriptacion.Desencrip(filtroContext.HttpContext.Request.Cookies[SessionValor.SessionKeyPersona])));
            //    if (boolValidar == false)
            //    {
            //        filtroContext.Result =
            //  new RedirectToRouteResult(new RouteValueDictionary(new
            //  {
            //      controller = "Login",
            //      action = "Index"
            //  }));
            //    }
            //}

            base.OnActionExecuted(filtroContext);
        }

        //public bool validarAcceso(int idusuario, int idAcceso)
        //{
        //   // Tuple<int, string> tuple = new Tuple<int, string>(-1, "");
        //   // tuple = _repository.sesionActiva_validar(idusuario, idAcceso);

        //    if (tuple.Item1 == 1)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
    }
}
