using EEG.Archive.Application.Common.Interfaces.Cache;
using EEG.Archive.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Application.Common.Interfaces.Repositories.MessageBrokers
{
    public interface IKafkaConsumer
    {
        public void Consume();
        public void StopConsumer();
    }
}
