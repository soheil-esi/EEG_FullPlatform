using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using EEG.Connector.Application.Common.Entities;
using EEG.Connector.Application.Common.Interface.Configuration;
using EEG.Connector.Application.Common.Interface.Repositories;
using EEG.Connector.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EEG.Connector.Infrastructure.Repositories.MessageBorkers
{
    public class KafkaRepository : IKafkaRepository
    {
        KafkaConfig _kafkaConfig;
        AdminClientConfig _adminConfig;
        SchemaRegistryConfig _schemaRegistryConfig;
        ProducerConfig _producerConfig;
        CachedSchemaRegistryClient _schemaRegistry;
        IProducer<int, signalDtos> _producer;
        public KafkaRepository(IEnvironmentConfig environmentConfig)
        {
            _kafkaConfig = environmentConfig.KafkaConfig;
            _adminConfig = new AdminClientConfig { BootstrapServers = _kafkaConfig.BootstarpServer };
            _schemaRegistryConfig = new SchemaRegistryConfig { Url = _kafkaConfig.SchemaRegistryUrl};
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = _kafkaConfig.BootstarpServer,
                // Guarantees delivery of message to topic.
                EnableDeliveryReports = true,
                ClientId = Dns.GetHostName()
            };
            _schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfig);
            _producer = new ProducerBuilder<int, signalDtos>(_producerConfig)
                .SetKeySerializer(new AvroSerializer<int>(_schemaRegistry))
                .SetValueSerializer(new AvroSerializer<signalDtos>(_schemaRegistry))
                .Build();
        }

        public async void CreateTpoic(string topicName, int replicationFactor, int numPartitions)
        {
            try
            {
                using var _adminClient = new AdminClientBuilder(_adminConfig).Build();
                await _adminClient.CreateTopicsAsync(new[]
                {
                    new TopicSpecification
                    {
                        Name = topicName,
                        ReplicationFactor = 1,
                        NumPartitions = 3
                    }
                });
            }
            catch (CreateTopicsException e) when (e.Results.Select(r => r.Error.Code)
                .Any(el => el == ErrorCode.TopicAlreadyExists))
            {
                Console.WriteLine($"Topic {e.Results[0].Topic} already exists");
            }
        }

        public async Task<bool> Produce(signalDtos toBeProduced, string topicName, int _partition , int key)
        {
            var partition = new TopicPartition(
                topicName,
                new Partition(_partition));
            await _producer.ProduceAsync(partition,
                new Message<int, signalDtos>
                {
                    Key =key ,
                    Value = toBeProduced
                });
            return true;
        }
    }
}
