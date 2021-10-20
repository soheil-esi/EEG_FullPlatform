using EEG.ProxySocket.Application.Common.Entities;
using EEG.ProxySocket.Application.Common.Interfaces.Configuration;
using EEG.ProxySocket.Application.Common.Interfaces.Repositories;
using EEG.ProxySocket.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Dictionary<int , IRepository> _repositories;
        public int period { get; set; }
        public UnitOfWork(IEnvironmentConfig environmentConfig)
        {
            _repositories = new Dictionary<int, IRepository>();
            environmentConfig.getInfluxDbsConfig().influxConfigs.ForEach(conf =>
            {
                _repositories.Add(conf.databaseId , new InfluxRepository(conf));
            });
            period = 30;
        }
        public signalDtos Query(int channelId)
        {
            signalDtos result = new signalDtos();
            switch (channelId)
            {
                case <9:
                    result = _repositories[1].Query(channelId, period);
                    break;
                case >8:
                    result = _repositories[2].Query(channelId, period);
                    break;
            }
            return result;
        }
    }
}
