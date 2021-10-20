using EEG.Dashboard.Application.Common.Entities;
using EEG.Dashboard.Application.Common.Interfaces.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Dashboard.Application.Cache
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
        public bool Set(int key, T dataToBeCached)
        {
            bool ischanged = false;
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
                        });
                }
                else
                {
                    ischanged = true;
                    localCache[key].updatetime = DateTime.Now;
                    localCache[key].data = dataToBeCached;
                }
            }
            return ischanged;
        }
    }
}
