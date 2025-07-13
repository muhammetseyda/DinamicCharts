using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinamicCharts.Application.DTOs.ConnectionInfo
{
    public class ConnectionInfoDto
    {
        public string Host { get; set; }
        public string DbAdi { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
