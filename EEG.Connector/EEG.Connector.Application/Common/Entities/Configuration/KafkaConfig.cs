using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Application.Common.Entities
{
    public class KafkaConfig
    {
        public string BootstarpServer { get; set; }
        public string SchemaRegistryUrl { get; set; }
        public string ClientId { get; set; }
        public KafkaTopics AICoreTopics { get; set; }
        public KafkaTopics ArchiveTopics { get; set; }
    }

    public class KafkaTopics
    {
        public string Producer { get; set; }
        public string Consumer { get; set; }
    }
}
