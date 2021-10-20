using EEG.Archive.Application.Common.Interfaces.Cache;
using EEG.Archive.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Infrastructure.Cache
{
    public class InMemoryCache : IInMemoryCache
    {
        private Dictionary<int , List<channelDto>> localCache;
        public event EventHandler DataConsumed;   
        public InMemoryCache()
        {
            localCache = new Dictionary<int, List<channelDto>>();
        }

        public Dictionary<int, List<channelDto>> Get()
        {
            Dictionary<int, List<channelDto>> result = new Dictionary<int, List<channelDto>>();
            result = localCache;
            localCache = new Dictionary<int, List<channelDto>>();
            return result;
        }

        private List<channelDto> Convert(memoryDto memoryDto)
        {
            List<channelDto> result = new List<channelDto>();
            foreach (var item in memoryDto.signalDtos.data)
            {
                result.Add(new channelDto()
                {
                    channelId = memoryDto.channelId,
                    Timestamp = new DateTime(long.Parse(item.Key)),
                    Value = item.Value
                });
            }
            return result;
        }

        public void Set(memoryDto toBeWritten)
        {
            if (localCache.ContainsKey(toBeWritten.channelId))
            {
                localCache[toBeWritten.channelId].AddRange(Convert(toBeWritten));
            }
            else
            {
                localCache.Add(toBeWritten.channelId, Convert(toBeWritten));
            }
            OnDataConsumed();
        }
        protected virtual void OnDataConsumed()
        {
            DataConsumed?.Invoke(this , EventArgs.Empty);
        }
    }
}
