using EEG.ProxySocket.Domain.Dtos;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Dashboard.Application.Common.Interfaces.Handlers
{
    public interface IDashboardHandler
    {
        public void ReceiveSignalDtos(signalDtos signalDtos);
    }
}
