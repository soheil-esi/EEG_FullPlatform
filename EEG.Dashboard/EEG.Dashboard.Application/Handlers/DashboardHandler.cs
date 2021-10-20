using EEG.Dashboard.Application.Common.Interfaces;
using EEG.Dashboard.Application.Common.Interfaces.Cache;
using EEG.Dashboard.Application.Common.Interfaces.Handlers;
using EEG.ProxySocket.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EEG.Dashboard.Application.Handlers
{
    public class DashboardHandler : IDashboardHandler
    {
        IDashboardCache _dashboardCache;
        public DashboardHandler(IDashboardCache dashboardCache)
        {
            _dashboardCache = dashboardCache;
        }

        public void ReceiveSignalDtos(signalDtos signalDtos)
        {
            _dashboardCache.SignalData_Update(signalDtos);
        }

        
    }
}
