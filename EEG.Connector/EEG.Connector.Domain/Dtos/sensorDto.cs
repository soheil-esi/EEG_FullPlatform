using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Domain.Dtos
{
    public class sensorDto
    {
        public byte[] data { get; set; }
        public DateTime timestamp { get; set; }
    }
}
