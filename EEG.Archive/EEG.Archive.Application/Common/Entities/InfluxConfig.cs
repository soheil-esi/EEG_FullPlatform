using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Application.Common.Entities
{
    public class InfluxConfig
    {
        public string username { get; set; }
        public string password { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public string org { get; set; }
        public string measurement { get; set; }
    }
}
