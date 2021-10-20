using EEG.Archive.Domain.Dtos;
using InfluxDB.Client.Writes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Application.Common.Interfaces.Repositories
{
    public interface IRepository
    {
        public void Insert(List<PointData> channelDtos);
    }
}
