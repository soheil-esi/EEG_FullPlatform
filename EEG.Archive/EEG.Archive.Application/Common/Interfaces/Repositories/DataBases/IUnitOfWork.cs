using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Archive.Application.Common.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IInfluxRepository InfluxRepository1_8 { get; }
        IInfluxRepository InfluxRepository9_16 { get; }
    }
}
