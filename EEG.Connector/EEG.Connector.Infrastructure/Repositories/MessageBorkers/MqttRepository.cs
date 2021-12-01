using EEG.Connector.Application.Common.Entities;
using EEG.Connector.Application.Common.Interface.Cache;
using EEG.Connector.Application.Common.Interface.Configuration;
using EEG.Connector.Application.Common.Interface.Repositories;
using EEG.Connector.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace EEG.Connector.Infrastructure.Repositories.MessageBorkers
{
    public class MqttRepository : IMqttRepository
    {
        private MqttConfig _mqttConfig;
        private MqttClient _mqttClient;
        private IInMemoryCache<sensorDto> _cache;
        DateTime _startTime;
        Stopwatch _stopwatch;
        public MqttRepository(IEnvironmentConfig environmentConfig , IInMemoryCache<sensorDto> cache)
        {
            _mqttConfig = environmentConfig.MqttConfig;
            _mqttClient = new MqttClient(_mqttConfig.host);
            _cache = cache;
            _mqttClient.MqttMsgPublishReceived += Consume;
            _mqttClient.Subscribe(new string[] { _mqttConfig.topicName }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            _startTime = DateTime.Now;
            _stopwatch = Stopwatch.StartNew();
            try
            {
                _mqttClient.Connect("C#Client1");
            }
            catch
            {

            }
        }

        public void Consume(object sender, MqttMsgPublishEventArgs e)
        {
            sensorDto sensorDto = new sensorDto()
            {
                data = e.Message,
                timestamp = _startTime.AddMilliseconds(_stopwatch.Elapsed.TotalMilliseconds)
            };
            _cache.Set(sensorDto);
        }
    }
}
