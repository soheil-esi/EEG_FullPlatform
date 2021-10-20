using EEG.Archive.Application.Common.Interfaces.Repositories;
using EEG.Archive.Domain.Dtos;
using InfluxDB.Client.Writes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Writes;
using EEG.Archive.Application.Common.Entities;

namespace EEG.Archive.Infrastructure.Repositories
{
    public class InfluxRepository : IInfluxRepository
    {
        private InfluxConfig influxConfig;
        private InfluxDBClient influxDBClient;
        private WriteApi writeApi;
        public InfluxRepository(InfluxConfig _influxConfig)
        {
            influxConfig = _influxConfig;
            influxDBClient = InfluxDBClientFactory.Create("http://" + influxConfig.host + ":" + influxConfig.port, "sa", new char[] { 'A', 'r', 'a', 'n', 'u', 'm', 'a', '@', '1', '2', '3' });
            writeApi = influxDBClient.GetWriteApi();
        }
        public void Insert(List<PointData> channelDtos)
        {
            writeApi.WritePoints(influxConfig.org, influxConfig.org, channelDtos);
            Console.WriteLine($"{channelDtos.Count} has been inserted into InfluxDB!");
        }
    }
}
