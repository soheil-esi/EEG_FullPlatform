using EEG.Dashboard.Presentation;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Dashboard.Application.Common.Interfaces
{
    public interface ISignalRConsumer
    {
        public HubConnection GetConnection();
        public void ReceiveFromProxySocket();
        public void SendCommand(commandDto commandDto);
    }
}
