using EEG.Archive.Application.Common.Interfaces.Cache;
using EEG.Archive.Application.Common.Interfaces.Configuration;
using EEG.Archive.Application.Common.Interfaces.Repositories;
using EEG.Archive.Application.Common.Interfaces.Repositories.MessageBrokers;
using EEG.Archive.Infrastructure.Cache;
using EEG.Archive.Infrastructure.Handlers;
using EEG.Archive.Infrastructure.Repositories;
using EEG.Archive.Infrastructure.Repositories.MessageBrokers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Infrastructure.Configuration
{
    public static class ArchiveStartupConfiguration
    {
        public static IServiceCollection AddArchiveStartupConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IInMemoryCache, InMemoryCache>();
            services.AddSingleton<IEnvironmentConfig, EnvironmentConfig>();
            services.AddHostedService<ArchiveHandler>();
            return services;
        }
    }
}
