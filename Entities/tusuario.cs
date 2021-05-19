using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace accesosIp.Entities
{   
    [Table("tusuario", Schema ="Accesos")]
    public class tusuario
    {
        [Key]
        public int sIdUsuario { get; set; }
        public string  sRuc { get; set; }
        public string sDni { get; set; }
        public string sNombre { get; set; }
        public string sApellidos { get; set; }
        public string semail { get; set; }
        public string stelefono { get; set; }
        public string clave { get; set; }
    }
}
