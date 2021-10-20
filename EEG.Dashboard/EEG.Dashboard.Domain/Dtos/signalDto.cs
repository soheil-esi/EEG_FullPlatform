using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Domain.Dtos
{
    public class signalDto : IComparer<List<signalDto>>
    {
        public double value { get; set; }
        public DateTime timestamp { get; set; }

        public int Compare(List<signalDto> x, List<signalDto> y)
        {
            if(x.Count != y.Count || x.Last<signalDto>() != y.Last<signalDto>() )
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
