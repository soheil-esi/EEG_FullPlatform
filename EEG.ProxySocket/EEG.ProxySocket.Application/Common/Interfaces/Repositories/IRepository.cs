using EEG.ProxySocket.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Common.Interfaces.Repositories
{
    public interface IRepository
    {
        public signalDtos Query(int channelId , int lastmintues);
    }
}
