using EEG.Dashboard.Application.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Dashboard.Application.Common.Interfaces.Cache
{
    public interface IInMemoryCache<T>
    {
        bool Set(int key, T dataToBeCached);
        CachedData<T> Get(int key);
    }
}
