using EEG.ProxySocket.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Common.Interfaces
{
    public interface IProxySocketBuisness
    {
        void SendEventToFrontFromSignalData(signalDtos recEvent);
        public List<int> ChannelIds { get; }
        public void ChangeRegStatus(string signalRConnection, List<int> channelIds, bool toRegister, bool all = false);
    }
}
