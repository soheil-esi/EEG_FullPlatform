using EEG.ProxySocket.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Dashboard.Application.Common.Interfaces.Consumers
{
    public interface IKafkaConsumer
    {
        public signalDtos Consume(int key);
    }
}
