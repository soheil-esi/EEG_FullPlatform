using Confluent.Kafka;
using Confluent.SchemaRegistry;
using EEG.Archive.Application.Common.Interfaces.Cache;
using EEG.Archive.Application.Common.Interfaces.Repositories.MessageBrokers;
using EEG.Archive.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.SchemaRegistry.Serdes;
using Confluent.Kafka.SyncOverAsync;
using EEG.Connector.Domain.Dtos;
using EEG.Archive.Application.Common.Interfaces.Configuration;
using EEG.Archive.Application.Common.Entities;

namespace EEG.Archive.Infrastructure.Repositories.MessageBrokers
{
    public class KafkaConsumer : IKafkaConsumer
    {
        IInMemoryCache _cache;
        SchemaRegistryConfig _schemaRegistryConfig;
        ConsumerConfig _consumerConfig;
        CachedSchemaRegistryClient _schemaRegistry;
        IConsumer <int , signalDtos> _consumer;
        bool _isRunning = true;
        KafkaConfig _kafkaConfig;
        public KafkaConsumer(IInMemoryCache cache , IEnvironmentConfig environmentConfig)
        {
            _cache = cache;
            _kafkaConfig = environmentConfig.KafkaConfig;
            _schemaRegistryConfig = new SchemaRegistryConfig { Url = _kafkaConfig.SchemaRegistryUrl };
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _kafkaConfig.BootstarpServer,
                GroupId = _kafkaConfig.ClientId,
                EnableAutoCommit = false,
                EnableAutoOffsetStore = false,
                // Read messages from start if no commit exists.
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };
            _schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfig);
            _consumer = new ConsumerBuilder<int, signalDtos>(_consumerConfig)
                .SetKeyDeserializer(new AvroDeserializer<int>(_schemaRegistry).AsSyncOverAsync())
                .SetValueDeserializer(new AvroDeserializer<signalDtos>(_schemaRegistry).AsSyncOverAsync())
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build();
            _consumer.Subscribe(_kafkaConfig.ProducersTopics[0]);
        }
        public void Consume()
        {
            Task.Run(() =>
            {
                while (_isRunning)
                {
                    var result = _consumer.Consume();
                    var signalDtos = result?.Message?.Value;
                    if (signalDtos == null)
                    {
                        continue;
                    }
                    _cache.Set(new memoryDto()
                    {
                        channelId = result.Message.Key,
                        signalDtos = signalDtos
                    });
                    _consumer.Commit(result);
                    _consumer.StoreOffset(result);
                }
            });
        }
        public void StopConsumer()
        {
            _isRunning = false;
            _consumer.Close();
        }
    }
}
