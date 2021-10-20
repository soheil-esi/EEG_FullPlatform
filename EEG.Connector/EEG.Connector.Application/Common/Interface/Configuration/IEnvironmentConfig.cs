using EEG.Connector.Application.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Application.Common.Interface.Configuration
{
    public interface IEnvironmentConfig
    {
        public MqttConfig MqttConfig { get; }
        public KafkaConfig KafkaConfig { get; }
    }
}
