using EEG.Connector.Application.Common.Interface.Cache;
using EEG.Connector.Application.Common.Interface.Configuration;
using EEG.Connector.Application.Common.Interface.Convertors;
using EEG.Connector.Application.Common.Interface.Repositories;
using EEG.Connector.Domain.Dtos;
using EEG.Connector.Infrastructure.Convertors;
using EEG.Connector.Infrastructure.Handlers;
using EEG.Connector.Infrastructure.Repositories.MessageBorkers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Infrastructure.Configuration
{
    public static class ConnectorStartupConfiguration
    {
        public static IServiceCollection AddConnectorStartupConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IEnvironmentConfig, EnvironmentConfig>();
            services.AddSingleton<IKafkaRepository, KafkaRepository>();
            services.AddSingleton<IMqttRepository, MqttRepository>();
            services.AddSingleton<IConverter<signalDtos, sensorDto>, SensorDtoToSignalDto>();
            services.AddSingleton<IInMemoryCache<sensorDto>, InMemoryCache<sensorDto>>();
            services.AddHostedService<ConnectorHandler>();
            return services;
        }
    }
}
