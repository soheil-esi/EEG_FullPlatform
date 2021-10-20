using EEG.ProxySocket.Application.Common.Interfaces;
using EEG.ProxySocket.Application.Common.Interfaces.Repositories;
using EEG.ProxySocket.Domain.Dtos;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.ProxySocket.Infrastructure.Hubs
{
    public class ProxySocketHub : Hub<IProxySocketHub>
    {
        IProxySocketBuisness _proxySocketBuisness;
        IUnitOfWork _unitOfWork;
        public ProxySocketHub(IProxySocketBuisness proxySocketBuisness , IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _proxySocketBuisness = proxySocketBuisness;
        }

        public Task SendCommand(commandDto commandDto)
        {
            _proxySocketBuisness.ChangeRegStatus(commandDto.signalRConnection,commandDto.channelIds,commandDto.toRegister);
            _unitOfWork.period = commandDto.lastminutes;

            return Task.CompletedTask;
        }
    }
}
