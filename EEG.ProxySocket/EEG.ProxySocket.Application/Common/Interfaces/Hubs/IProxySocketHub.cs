using EEG.ProxySocket.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Application.Common.Interfaces
{
    public interface IProxySocketHub
    {
        Task OnSignalDataReceive(signalDtos signaldto);
        Task SendCommand(commandDto commandDto);
    }
}
