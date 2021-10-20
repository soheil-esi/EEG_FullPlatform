using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Application.Common.Interface.Convertors
{
    public interface IConverter<T , Y>
    {
        public List<T> Convert(List<Y> dataToBeConverted);
    }
}
