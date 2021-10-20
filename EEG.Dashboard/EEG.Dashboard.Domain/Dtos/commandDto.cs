using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Dashboard.Presentation
{
    public class commandDto
    {
        public string signalRConnection { get; set; }
        public int lastminutes { get; set; }
        public List<int> channelIds { get; set; }
        public bool toRegister { get; set; }
    }
}
