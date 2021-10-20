using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Domain.Dtos
{
    public class signalDtos
    {
        public int key { get; set; }
        public List<signalDto> data { get; set; }
    }
}
