using EEG.Archive.Application.Common.Interfaces.Cache;
using EEG.Archive.Application.Common.Interfaces.Handlers;
using EEG.Archive.Application.Common.Interfaces.Repositories;
using EEG.Archive.Application.Common.Interfaces.Repositories.MessageBrokers;
using EEG.Archive.Domain.Dtos;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EEG.Archive.Infrastructure.Handlers
{
    public class ArchiveHandler : IArchiveHandler
    {
        private IInMemoryCache _memoryCache;
        private IKafkaConsumer _kafkaConsumer;
        private IUnitOfWork _unitOfWork;

        public ArchiveHandler(IInMemoryCache memoryCache , IKafkaConsumer kafkaConsumer,
                                IUnitOfWork unitOfWork)
        {
            _memoryCache = memoryCache;
            _kafkaConsumer = kafkaConsumer;
            _unitOfWork = unitOfWork;
            _memoryCache.DataConsumed += ConsumeAndInsert;
            
        }

        private void ConsumeAndInsert(object sender, EventArgs e)
        {
            var consumedData = _memoryCache.Get();
            var toBeInserted1_8 = Convert(consumedData.Where(d => d.Key < 9).ToDictionary(t => t.Key , t => t.Value));
            var toBeInserted8_16 = Convert(consumedData.Where(d => d.Key > 8).ToDictionary(t => t.Key, t => t.Value));
            Task.Run(() =>
            {
                _unitOfWork.InfluxRepository1_8.Insert(toBeInserted1_8);
            });
            Task.Run(() =>
            {
                _unitOfWork.InfluxRepository9_16.Insert(toBeInserted8_16);
            });
        }

        private List<PointData> Convert(Dictionary<int , List<channelDto>> keyValuePairs)
        {
            List<PointData> result = new List<PointData>();
            foreach (var item in keyValuePairs)
            {
                item.Value.ForEach(d =>
                {
                    result.Add(PointData.Measurement($"Channel{item.Key}")
                        .Field("Value", d.Value).Timestamp(d.Timestamp.ToUniversalTime(), WritePrecision.Ms));
                });
            }
            return result;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Consume();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
