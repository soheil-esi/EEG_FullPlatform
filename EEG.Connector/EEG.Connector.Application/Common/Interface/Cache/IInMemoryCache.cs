using EEG.Connector.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Application.Common.Interface.Cache
{
    public interface IInMemoryCache<T>
    {
        public void Set(T toBeWritten);
        public List<T> Get();
    }
}
