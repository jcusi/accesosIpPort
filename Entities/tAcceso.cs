using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace accesosIp.Entities
{
    [Table("tusuario", Schema = "Accesos")]
    public class tAcceso
    {
        [Key]
        public Guid sIdAcceso { get; set; }
        public Guid sIdUsuario { get; set; }
        public string sIp { get; set; }
        public string sPort { set; get; }
        public string skeysession { set; get; }
        public DateTime dtFechaCreacion { get; set; }
        public DateTime dtFechaExpiracion { get; set; }
        public int nSession { get; set; }
    }
}
