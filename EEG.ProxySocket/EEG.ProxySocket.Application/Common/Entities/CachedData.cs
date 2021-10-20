using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Common.Entities
{
    public class CachedData<T>
    {
        public T data;
        public DateTime updatetime;
        public List<string> ListOfSignalRconnections;
        public CachedData()
        {

        }
    }
}
