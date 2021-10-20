using EEG.Connector.Application.Common.Interface.Cache;
using EEG.Connector.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Infrastructure
{
    public class InMemoryCache<T> : IInMemoryCache<T> where T : class
    {
        private List<T> localCache;

        public InMemoryCache()
        {
            localCache = new List<T>();
        }

        public List<T> Get()
        {
            List<T> result = new List<T>();
            result = localCache.GetRange(0, localCache.Count());
            localCache.RemoveRange(0, localCache.Count());
            return result;
        }

        public void Set(T toBeWritten)
        {
            localCache.Add(toBeWritten);
        }
    }
}
