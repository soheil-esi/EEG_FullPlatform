using EEG.ProxySocket.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Dashboard.Application.Common.Interfaces.Cache
{
    public interface IDashboardCache
    {
        void SignalData_Update(signalDtos signalDataDtos);
        ObservableCollection<signalDto> GetDataSource(int key);
    }
}
