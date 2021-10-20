using EEG.Connector.Application.Common.Entities;
using EEG.Connector.Application.Common.Interface.Cache;
using EEG.Connector.Application.Common.Interface.Configuration;
using EEG.Connector.Application.Common.Interface.Convertors;
using EEG.Connector.Application.Common.Interface.Handlers;
using EEG.Connector.Application.Common.Interface.Repositories;
using EEG.Connector.Domain.Dtos;
using System;
using System.Collections.Generic;
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
                    dataConverted = _converter.Convert(_cache.Get());
                    if(dataConverted == null || dataConverted.Count() == 0)
                    {
                        continue;
                    }

                    dataConverted.ForEach(d =>
                    {
                        if (d.data.Count() > 0)
                        {
                            _kafkaRepository.Produce(d, _kafkaConfig.ProducersTopics[0], 0 , channelId).Wait();
                            channelId++;
                            if(channelId == 17)
                            {
                                channelId = 1;
                            }
                        }
                    });
                    Thread.Sleep(5000);
                }
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _isRunning = true;
            _kafkaRepository.CreateTpoic(_kafkaConfig.ProducersTopics[0] , 1 , 1);
            Inserter();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
