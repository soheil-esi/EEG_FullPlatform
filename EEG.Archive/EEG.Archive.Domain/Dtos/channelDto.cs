using InfluxDB.Client.Writes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Domain.Dtos
{
    public class channelDto
    {
        public int channelId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
