using EEG.ProxySocket.Application.Common.Interfaces.Cache;
using EEG.ProxySocket.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Cache
{
    public class ProxySocketCache : IProxySocketCache
    {
        IInMemoryCache<List<signalDto>> signalDataMemoryCache;
        
        public ProxySocketCache(IInMemoryCache<List<signalDto>> _signalDataMemoryCache)
        {
            signalDataMemoryCache = _signalDataMemoryCache;
        }

        public List<string> GetSignalRConnections(int key)
        {
            return signalDataMemoryCache.GetKeysAndConnections()[key].ListOfSignalRconnections;
        }

        public void SignalDataStatus_ChangeRegStatus(string signalRConnection, int key, bool toRegister , bool all = false)
        {
            if (toRegister)
            {
                signalDataMemoryCache.RegisterForData(
                    key,
                    signalRConnection);
            }
            else
            {
                if (all)
                {
                    signalDataMemoryCache.UnRegisterForData(key, signalRConnection , all);
                }
                else
                {
                    signalDataMemoryCache.UnRegisterForData(key, signalRConnection);
                }
            }
        }

        public List<string> SignalDataStatus_Update(signalDtos signalDataDtos)
        {
            int key = signalDataDtos.key;
            List<signalDto> signalData = signalDataDtos.data;
            signalDataMemoryCache.Set(
                key,
                signalData,
                out List<string> signalRconnections);
            return signalRconnections;
        }
    }
}
