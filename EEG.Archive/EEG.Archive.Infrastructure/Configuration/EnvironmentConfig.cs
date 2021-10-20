using EEG.Archive.Application.Common.Entities;
using EEG.Archive.Application.Common.Interfaces.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Infrastructure.Configuration
{
    public class EnvironmentConfig : IEnvironmentConfig
    {
        private IConfigurationRoot _configuration;
        public ListInfluxConfigs influxConfigs { get; }
        public KafkaConfig KafkaConfig { get; }

        public EnvironmentConfig()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, true);
            _configuration = configurationBuilder.Build();
            influxConfigs = _configuration.GetSection("InfluxConfigs").Get<ListInfluxConfigs>();
            KafkaConfig = _configuration.GetSection("KafkaConfig").Get<KafkaConfig>();
        }
    }
}
