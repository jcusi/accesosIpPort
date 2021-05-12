using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace accesosIp.Extension
{
    public class Requerido:RequiredAttribute
    {
        public Requerido()
        {
            ErrorMessage = "El campo '{0}' es obligatorio";
        }
    }
}
