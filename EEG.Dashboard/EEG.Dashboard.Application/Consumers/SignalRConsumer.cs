using EEG.Dashboard.Application.Common.Interfaces;
using EEG.Dashboard.Application.Common.Interfaces.Handlers;
using EEG.Dashboard.Presentation;
using EEG.ProxySocket.Domain.Dtos;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EEG.Dashboard.Application.Consumers
{
    public class SignalRConsumer : ISignalRConsumer
    {
        public HubConnection connection;
        IDashboardHandler _dashboardHandler;

        public SignalRConsumer(IDashboardHandler dashboardHandler)
        {
            connection = new HubConnectionBuilder().WithUrl("http://localhost:5000/proxysockethub").Build();
            _dashboardHandler = dashboardHandler;
        }

        public HubConnection GetConnection()
        {
            return connection;
        }

        public async void ReceiveFromProxySocket()
        {
            connection.On<signalDtos>("OnSignalDataReceive", (signalDtos) =>
            {
                _dashboardHandler.ReceiveSignalDtos(signalDtos);
            });
        }
        public async void SendCommand(commandDto commandDto)
        {
            await connection.InvokeAsync("SendCommand", commandDto); 
        }
    }
}
