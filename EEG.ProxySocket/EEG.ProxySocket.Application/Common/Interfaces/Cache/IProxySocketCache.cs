using EEG.ProxySocket.Application.Common.Entities;
using EEG.ProxySocket.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Common.Interfaces.Cache
{
    public interface IProxySocketCache
    {
        public List<string> GetSignalRConnections(int key);
        List<string> SignalDataStatus_Update(signalDtos signalDataDtos);
        void SignalDataStatus_ChangeRegStatus(string signalRConnection, int key, bool toRegister, bool all = false);

    }
}
