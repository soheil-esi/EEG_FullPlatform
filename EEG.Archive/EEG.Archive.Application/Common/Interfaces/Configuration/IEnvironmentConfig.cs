using EEG.Archive.Application.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Application.Common.Interfaces.Configuration
{
    public interface IEnvironmentConfig
    {
        public KafkaConfig KafkaConfig { get; }
        public ListInfluxConfigs influxConfigs { get; }
    }
}
