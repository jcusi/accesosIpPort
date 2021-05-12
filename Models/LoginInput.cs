using accesosIp.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace accesosIp.Models
{
    public class LoginInput:BaseInputEntity
    {
        [Requerido]
        [Display(Name = "ruc")]
        [StringLength(11)]
        public string ruc { get; set; }
        [Requerido]
        [Display(Name = "dni")]
        [StringLength(8)]
        public string dni { get; set; }

        [Requerido]
        [DataType(DataType.Password)]
        [Display(Name = "contrasenia")]
        [StringLength(50)]
        public string contrasenia { get; set; }

        public LoginInput()
        {
            ruc = string.Empty;
            contrasenia = string.Empty;
        }
    }
}
