using EEG.ProxySocket.Application.Common.Interfaces;
using EEG.ProxySocket.Application.Common.Interfaces.Configuration;
using EEG.ProxySocket.Application.Common.Interfaces.Handlers;
using EEG.ProxySocket.Application.Common.Interfaces.Repositories;
using EEG.ProxySocket.Application.Common.Interfaces.Repositories.MessageBrokers.Kafka;
using EEG.ProxySocket.Domain.Dtos;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Handlers
{
    public class ProxySocketHandler : IProxySocketHandler
    {
        IUnitOfWork _datasource;
        IProxySocketBuisness _proxySocketBuisness;
        IEnvironmentConfig _environmentConfig;
        signalDtos localCache;
        List<int> channelIds;
        bool _isRunning = true;
        private readonly object balanceLock = new object();

        public ProxySocketHandler(IUnitOfWork datasource , IEnvironmentConfig environmentConfig , 
            IProxySocketBuisness proxySocketBuisness )
        {
            _datasource = datasource;
            _environmentConfig = environmentConfig;
            _proxySocketBuisness = proxySocketBuisness;
            channelIds = new List<int>();
        }
        public void SendToFront() 
        {
            Task.Run(() =>
            {
                while (_isRunning)
                {
                    lock (balanceLock)
                    {
                        var channelIds = _proxySocketBuisness.ChannelIds;
                        channelIds.ForEach(id =>
                        {
                            _proxySocketBuisness.SendEventToFrontFromSignalData(new signalDtos()
                            {
                                data = _datasource.Query(id)?.data,
                                key = id
                            });
                        });
                        Thread.Sleep(5000);
                    }
                }
                
            });
            
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            SendToFront();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
