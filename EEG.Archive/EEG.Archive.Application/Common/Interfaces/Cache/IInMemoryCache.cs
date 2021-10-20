using EEG.Archive.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Application.Common.Interfaces.Cache
{
    public interface IInMemoryCache
    {
        public event EventHandler DataConsumed;
        public void Set(memoryDto toBeWritten);
        public Dictionary<int, List<channelDto>> Get();
    }
}
