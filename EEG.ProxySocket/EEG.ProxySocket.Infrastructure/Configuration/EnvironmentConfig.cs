using EEG.ProxySocket.Application.Common.Entities;
using EEG.ProxySocket.Application.Common.Entities.Configuration;
using EEG.ProxySocket.Application.Common.Interfaces.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Infrastructure.Configuration
{
    public class EnvironmentConfig : IEnvironmentConfig
    {
        InfluxConfigs influxConfig;
        private IConfigurationRoot _configuration;
        public EnvironmentConfig()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, true);
            _configuration = configurationBuilder.Build();
            influxConfig = _configuration.GetSection("InfluxConfigs").Get<InfluxConfigs>();
        }
        public InfluxConfigs getInfluxDbsConfig()
        {
            return influxConfig;
        }
    }
}
