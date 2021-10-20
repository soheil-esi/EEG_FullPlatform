using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Dashboard.Application.Common.Entities
{
    public class CachedData<T>
    {
        public T data;
        public DateTime updatetime;
        public CachedData()
        {

        }
    }
}
