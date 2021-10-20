using EEG.ProxySocket.Application.Common.Entities;
using EEG.ProxySocket.Application.Common.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Common.Interfaces.Configuration
{
    public interface IEnvironmentConfig
    {
        public InfluxConfigs getInfluxDbsConfig();
    }
}
