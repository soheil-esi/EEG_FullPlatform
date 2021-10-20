using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Application.Common.Entities
{
    public class KafkaConfig
    {
        public string BootstarpServer { get; set; }
        public string SchemaRegistryUrl { get; set; }
        public string ClientId { get; set; }
        public List<string> ProducersTopics { get; set; }
        public List<string> ConsumersTopics { get; set; }
    }
}
