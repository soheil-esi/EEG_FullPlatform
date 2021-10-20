using EEG.ProxySocket.Application.Common.Interfaces.Cache;
using EEG.ProxySocket.Application.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Cache
{
    public class InMemoryCache<T> : IInMemoryCache<T> where T : class
    {
        #region Attributes
        private object localCacheLock = new object();
        private Dictionary<int, CachedData<T>> localCache;
        #endregion

        public InMemoryCache()
        {
            localCache = new Dictionary<int, CachedData<T>>();
        }

        public CachedData<T> Get(int key)
        {
            lock (localCacheLock)
            {
                if (localCache.ContainsKey(key))
                {
                    return localCache[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public void RegisterForData(int key, string signalRconnection)
        {
            //this.logger.LogDebug($"key: {key} is registered by signalR: {signalRconnection}");
            lock (localCacheLock)
            {
                if (localCache.ContainsKey(key))
                {
                    if (!localCache[key].ListOfSignalRconnections.Contains(signalRconnection))
                    {
                        localCache[key].ListOfSignalRconnections.Add(signalRconnection);
                    }
                }
                else
                {
                    var cData = new CachedData<T>()
                    {
                        data = null,
                        ListOfSignalRconnections = new List<string>()
                    };
                    cData.ListOfSignalRconnections.Add(signalRconnection);
                    localCache.Add(key, cData);
                }
            }
        }

        public bool Set(int key, T dataToBeCached, out List<string> signalRConnections)
        {
            bool ischanged = false;
            signalRConnections = new List<string>();
            lock (localCacheLock)
            {
                if (!localCache.ContainsKey(key))
                {
                    localCache.Add(
                        key,
                        new CachedData<T>
                        {
                            data = dataToBeCached,
                            updatetime = DateTime.Now,
                            ListOfSignalRconnections = new List<string>()
                        });
                }
                else
                {
                    localCache[key].updatetime = DateTime.Now;
                    localCache[key].data = dataToBeCached;
                    signalRConnections = localCache[key].ListOfSignalRconnections;
                }
            }
            return ischanged;
        }

        public void UnRegisterForData(int key, string signalRconnection , bool all = false)
        {
            //this.logger.LogDebug($"key: {key} is un-registered by signalR: {signalRconnection}");
            lock (localCacheLock)
            {
                if (all == true)
                {
                    localCache.Keys.ToList().ForEach(key => {
                        if (localCache[key].ListOfSignalRconnections.Contains(signalRconnection))
                        {
                            localCache[key].ListOfSignalRconnections.Remove(signalRconnection);
                        }
                    });
                }
                else if (localCache.ContainsKey(key))
                {
                    if (localCache[key].ListOfSignalRconnections.Contains(signalRconnection))
                    {
                        localCache[key].ListOfSignalRconnections.Remove(signalRconnection);
                    }
                }
            }
        }
        public Dictionary<int, CachedData<T>> GetKeysAndConnections()
        {
            return localCache;
        }
    }
}
