using EEG.ProxySocket.Application;
using EEG.ProxySocket.Application.Cache;
using EEG.ProxySocket.Application.Common.Interfaces;
using EEG.ProxySocket.Application.Common.Interfaces.Cache;
using EEG.ProxySocket.Application.Common.Interfaces.Configuration;
using EEG.ProxySocket.Application.Common.Interfaces.Repositories;
using EEG.ProxySocket.Application.Common.Interfaces.Repositories.MessageBrokers.Kafka;
using EEG.ProxySocket.Application.Handlers;
using EEG.ProxySocket.Domain.Dtos;
using EEG.ProxySocket.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Infrastructure.Configuration
{
    public static class ProxySocketStartup
    {
        public static IServiceCollection AddProxySocketStartupConfiguration(this IServiceCollection services)
        {
            services.AddSignalR(
              options =>
              {
                  options.EnableDetailedErrors = true;
                  options.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
                  options.KeepAliveInterval = TimeSpan.FromSeconds(3);
              }
              );
            services.AddSingleton<IEnvironmentConfig, EnvironmentConfig>();
            services.AddSingleton<IInMemoryCache<List<signalDto>>, InMemoryCache<List<signalDto>>>();
            services.AddSingleton<IProxySocketBuisness, ProxySocketBuisness>();
            services.AddSingleton<IProxySocketCache, ProxySocketCache>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddHostedService<ProxySocketHandler>();

            return services;
        }
    }
}
