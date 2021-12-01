using EEG.Connector.Application.Common.Entities;
using EEG.Connector.Application.Common.Interface.Cache;
using EEG.Connector.Application.Common.Interface.Configuration;
using EEG.Connector.Application.Common.Interface.Convertors;
using EEG.Connector.Application.Common.Interface.Handlers;
using EEG.Connector.Application.Common.Interface.Repositories;
using EEG.Connector.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EEG.Connector.Infrastructure.Handlers
{
    public class ConnectorHandler : IConnectorHandler
    {
        IInMemoryCache<sensorDto> _cache;
        IConverter<signalDtos, sensorDto> _converter;
        private KafkaConfig _kafkaConfig;
        private IKafkaRepository _kafkaRepository;
        private IMqttRepository _mqttRepository;
        private List<signalDtos> dataConverted;
        bool _isRunning = false;

        public ConnectorHandler(IInMemoryCache<sensorDto> cache,
                                IConverter<signalDtos, sensorDto> converter,
                                IKafkaRepository kafkaRepository, IMqttRepository mqttRepository, 
                                IEnvironmentConfig environmentConfig)
        {
            _cache = cache;
            _converter = converter;
            _kafkaRepository = kafkaRepository;
            _mqttRepository = mqttRepository;
            _kafkaConfig = environmentConfig.KafkaConfig;
        }
        public void Inserter()
        {
            Task.Run(() =>
            {
                int channelId = 1;
                while (_isRunning)
                {
                    if(_cache.Count() > 0)
                    {
                        Thread.Sleep(1000);
                        dataConverted = _converter.Convert(_cache.Get(200));
                        dataConverted.ForEach(d =>
                        {
                            if (d.data.Count() > 0)
                            {
                                try
                                {
                                    _kafkaRepository.Produce(d, _kafkaConfig.AICoreTopics.Producer,
                                        (int)((channelId - 1) / 4), channelId).Wait();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
                                channelId++;
                                if (channelId == 17)
                                {
                                    channelId = 1;
                                }
                            }
                        });
                        }
                    else
                    {
                        dataConverted = null;
                    }
                }
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;
            _kafkaRepository.CreateTpoic(_kafkaConfig.AICoreTopics.Producer, 1, 4);
            _kafkaRepository.CreateTpoic(_kafkaConfig.AICoreTopics.Consumer, 1, 4);
            Inserter();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _isRunning = false;
            return Task.CompletedTask;
        }
    }
}
