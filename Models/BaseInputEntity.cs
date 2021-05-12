using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace accesosIp.Models
{
    public class BaseInputEntity
    {
        public string ip { get; set; }
        public string localPort { get; set; }
        public string remotePort { get; set; }
        public string localIp { get; set; }
        public BaseInputEntity()
        {
            remotePort = string.Empty;
            localPort = string.Empty;
            localIp = string.Empty;
            ip = string.Empty;
        }
    }
}
