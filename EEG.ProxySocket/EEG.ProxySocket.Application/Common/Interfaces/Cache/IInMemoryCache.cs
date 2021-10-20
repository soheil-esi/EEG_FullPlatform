using EEG.ProxySocket.Application.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Common.Interfaces.Cache
{
    public interface IInMemoryCache<T>
    {
        bool Set(int key, T dataToBeCached, out List<string> signalRConnections);
        CachedData<T> Get(int key);
        void RegisterForData(int key, string signalRconnection);
        void UnRegisterForData(int key, string signalRconnection, bool all = false);
        public Dictionary<int, CachedData<T>> GetKeysAndConnections();
    }
}
