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
                 filtroContext.HttpContext.Request.Cookies[SessionValor.SessionKeyPersona] == null
                 )
               {

                helper.Helper.CrearLog("valores nulos");
                filtroContext.Result =
                new RedirectToRouteResult(new RouteValueDictionary(new
                {

                    controller = "Login",
                    action = "Index"
                }));
            }
             
            base.OnActionExecuted(filtroContext);
        }
    }
}
