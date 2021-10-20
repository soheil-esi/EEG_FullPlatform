using EEG.ProxySocket.Application.Common.Entities;
using EEG.ProxySocket.Application.Common.Interfaces.Configuration;
using EEG.ProxySocket.Application.Common.Interfaces.Repositories;
using EEG.ProxySocket.Domain.Dtos;
using InfluxDB.Client;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Infrastructure.Repositories
{
    public class InfluxRepository : IRepository
    {
        InfluxDBClient influxDBClient;
        InfluxDBSetUp influxConfig;
        QueryApiSync queryApi;
        public InfluxRepository(InfluxDBSetUp _influxConfig)
        {
            influxConfig = _influxConfig;
            influxDBClient = InfluxDBClientFactory.Create("http://" + influxConfig.host + ":" + influxConfig.port, "sa", new char[] { 'A', 'r', 'a', 'n', 'u', 'm', 'a', '@', '1', '2', '3' });
            queryApi = influxDBClient.GetQueryApiSync();
        }
        public signalDtos Query(int channelId , int lastmintues)
        {
            List<signalDto> cache = new List<signalDto>();
            List<FluxTable> tables = queryApi.QuerySync($"from(bucket : \"{influxConfig.bucket}\") |> range(start:-{lastmintues}s) |> filter(fn: (r) => r._measurement == \"Channel{channelId}\")", influxConfig.org);
            if (tables.Count > 0)
            {
                tables.ForEach(table =>
                {
                    table.Records.ForEach(data =>
                    {
                        cache.Add(new signalDto()
                        {
                            timestamp = (DateTime)data.GetTimeInDateTime(),
                            value = (double)data.GetValue()
                        });
                    });
                });
                signalDtos signalsCache = new signalDtos()
                {
                    data = cache,
                    key = channelId
                };
                return signalsCache;
            }
            else
            {
                return null;
            }
        }
    }
}
