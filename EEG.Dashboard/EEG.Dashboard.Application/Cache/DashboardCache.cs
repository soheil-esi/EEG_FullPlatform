using EEG.Dashboard.Application.Common.Interfaces.Cache;
using EEG.ProxySocket.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Dashboard.Application.Cache
{
    public class DashboardCache : IDashboardCache
    {
        IInMemoryCache<ObservableCollection<signalDto>> localCache;
        public DashboardCache(IInMemoryCache<ObservableCollection<signalDto>> _localCache)
        {
            localCache = _localCache;
        }
        public ObservableCollection<signalDto> GetDataSource(int key)
        {
            if(localCache.Get(key) != null)
            {
                return localCache.Get(key).data;
            }
            else
            {
                return null;
            }
        }

        public void SignalData_Update(signalDtos signalDataDtos)
        {
            ObservableCollection<signalDto> aux = new ObservableCollection<signalDto>();
            signalDataDtos.data.ForEach(record =>
            {
                aux.Add(record);
            });
            localCache.Set(signalDataDtos.key, aux);
        }
    }
}
