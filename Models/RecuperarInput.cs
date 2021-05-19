using accesosIp.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace accesosIp.Models
{
    public class RecuperarInput
    {
        [Requerido]
        [Display(Name = "email")]
        [StringLength(255)]
        public string email { get; set; }
        [Requerido]
        [Display(Name = "sms")]
        [StringLength(30)]
        public string sms { get; set; }
    }
}
