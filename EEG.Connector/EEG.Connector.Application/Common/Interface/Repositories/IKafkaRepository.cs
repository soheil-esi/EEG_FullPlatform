using EEG.Connector.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Application.Common.Interface.Repositories
{
    public interface IKafkaRepository
    {
        public void CreateTpoic(string topicName, int replicationFactor, int numPartitions);
        public Task<bool> Produce(signalDtos toBeProduced, string topicName, int partition , int key);
    }
}
