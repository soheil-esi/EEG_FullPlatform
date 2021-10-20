using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Common.Entities
{
    public class InfluxDBSetUp
    {
        public string username { get; set; }
        public string password { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public string org { get; set; }
        public string bucket { get; set; }
        public int databaseId { get; set; }
    }
}
