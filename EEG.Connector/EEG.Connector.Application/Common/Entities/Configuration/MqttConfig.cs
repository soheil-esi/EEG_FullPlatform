using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Application.Common.Entities
{
    public class MqttConfig
    {
        public string host { get; set; }
        public string topicName { get; set; }
        public int port { get; set; }
    }
}
