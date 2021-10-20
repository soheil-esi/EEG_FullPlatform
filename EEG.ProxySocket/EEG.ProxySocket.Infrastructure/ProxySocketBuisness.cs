using EEG.ProxySocket.Application.Common.Interfaces;
using EEG.ProxySocket.Application.Common.Interfaces.Cache;
using EEG.ProxySocket.Application.Common.Interfaces.Repositories.MessageBrokers.Kafka;
using EEG.ProxySocket.Domain.Dtos;
using EEG.ProxySocket.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application
{
    public class ProxySocketBuisness : IProxySocketBuisness
    {
        #region Attributes
        private IProxySocketCache _proxySocketCache;
        public List<int> ChannelIds { set; get; }
        IHubContext<ProxySocketHub, IProxySocketHub> _hub;
        #endregion
        public ProxySocketBuisness(IProxySocketCache proxySocketCache , IHubContext<ProxySocketHub ,IProxySocketHub> hub)
        {
            _hub = hub;
            _proxySocketCache = proxySocketCache;
            ChannelIds = new List<int>();
        }
        public void ChangeRegStatus(string signalRConnection, List<int> channelIds, bool toRegister, bool all = false)
        {
            if (toRegister && !all)
            {
                ChannelIds.AddRange(channelIds);
            }
            else if(!toRegister && !all)
            {
                channelIds.ForEach(id =>
                {
                    ChannelIds.Remove(id);
                });
            }
            else
            {
                ChannelIds = new List<int>();
            }
            channelIds.ForEach(id =>
            {
                _proxySocketCache.SignalDataStatus_ChangeRegStatus(signalRConnection, id, toRegister , all);
            });
            
        }

        public void SendEventToFrontFromSignalData(signalDtos recEvent)
        {
            try
            {
                var signalRConnections = _proxySocketCache.GetSignalRConnections(recEvent.key);
                signalRConnections?.ForEach(con =>
                {
                    _hub.Clients.Client(con).OnSignalDataReceive(recEvent);
                });

            }

            catch (Exception ex)
            {
                //_logger.LogError($"exception in SendTilePointToRealTime. ex.message: {ex.Message} ", ex);
            }
        }

    }
}
