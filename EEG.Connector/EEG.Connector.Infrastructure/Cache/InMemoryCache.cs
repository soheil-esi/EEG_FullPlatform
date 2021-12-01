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

        public int Count()
        {
            return localCache.Count;
        }

        public List<T> Get(int count)
        {
            List<T> result = new List<T>();
            if(count > localCache.Count())
            {
                result = localCache.GetRange(0, localCache.Count());
                localCache.RemoveRange(0, localCache.Count());
            }
            else
            {
                result = localCache.GetRange(0, count);
                localCache.RemoveRange(0, count);
            }
            return result;
        }

        public void Set(T toBeWritten)
        {
            localCache.Add(toBeWritten);
        }
    }
}
