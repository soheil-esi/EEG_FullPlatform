using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Domain.Dtos
{
    public class signalDto 
    {
        public double value { get; set; }
        public DateTime timestamp { get; set; }
    }
}
