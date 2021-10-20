using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Application.Common.Interface.Handlers
{
    public interface IConnectorHandler : IHostedService
    {
        public void Inserter();
    }
}
