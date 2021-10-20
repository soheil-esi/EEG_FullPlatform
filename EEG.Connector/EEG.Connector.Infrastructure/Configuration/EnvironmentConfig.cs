using EEG.Connector.Application.Common.Entities;
using EEG.Connector.Application.Common.Interface.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Infrastructure.Configuration
{
    public class EnvironmentConfig : IEnvironmentConfig
    {
        private IConfigurationRoot _configuration;
        public MqttConfig MqttConfig { get; }
        public KafkaConfig KafkaConfig { get; }
        public EnvironmentConfig()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, true);
            _configuration = configurationBuilder.Build();
            MqttConfig = _configuration.GetSection("MqttConfig").Get<MqttConfig>();
            KafkaConfig = _configuration.GetSection("KafkaConfig").Get<KafkaConfig>();
        }
    }
}
