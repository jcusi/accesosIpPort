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
        [Column("sIdAcceso")]
        public int sIdAcceso { get; set; }
        [ForeignKey("sIdUsuario")]
        [Column("sIdUsuario")]
        public int sIdUsuario { get; set; }
        [Column("sIp")]
        public string sIp { get; set; }
        [Column("sPort")]
        public string sPort { set; get; }
        [Column("sNavegador")]
        public string sNavegador { set; get; }
        [Column("dtFechaCreacion")]
        public DateTime dtFechaCreacion { get; set; }
        [Column("dtFechaExpiracion")]
        public DateTime dtFechaExpiracion { get; set; }
        [Column("nSession")]
        public int nSession { get; set; }
    }
}
