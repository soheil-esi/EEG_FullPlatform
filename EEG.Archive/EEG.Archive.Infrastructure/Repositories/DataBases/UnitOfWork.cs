using EEG.Archive.Application.Common.Interfaces.Configuration;
using EEG.Archive.Application.Common.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IInfluxRepository InfluxRepository1_8 { get; }
        public IInfluxRepository InfluxRepository9_16 { get; }
        public UnitOfWork(IEnvironmentConfig environmentConfig)
        {
            InfluxRepository1_8 = new InfluxRepository(environmentConfig.influxConfigs.influxConfigs[0]);
            InfluxRepository9_16 = new InfluxRepository(environmentConfig.influxConfigs.influxConfigs[1]);
        }
    }
}
