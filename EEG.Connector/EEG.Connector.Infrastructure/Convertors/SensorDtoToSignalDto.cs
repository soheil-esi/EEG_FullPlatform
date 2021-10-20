using EEG.Connector.Application.Common.Interface.Convertors;
using EEG.Connector.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEG.Connector.Infrastructure.Convertors
{
    public class SensorDtoToSignalDto : IConverter<signalDtos, sensorDto>
    {
        List<signalDtos> result;
        public SensorDtoToSignalDto()
        {
            
        }
        public List<signalDtos> Convert(List<sensorDto> dataToBeConverted)
        {
            int countRepeat = 0;
            int channelId = 1;
            result = new List<signalDtos>();
            double calculated;
            dataToBeConverted.ForEach(metric =>
            {
                if (metric != null)
                {
                    for (int i = 3; i < metric.data.Length; i = i + 3)
                    {
                        if (i != 27)
                        {
                            int value = metric.data[i] | metric.data[i + 1] << 8 | metric.data[i + 2] << 16;
                            calculated = ((int)Math.Pow(2, 24) - value) * 4.5 / (int)Math.Pow(2, 24);
                            switch (countRepeat)
                            {
                                case 1:
                                    result[channelId - 1].data.Add(metric.timestamp.Ticks.ToString(), calculated);
                                    break;
                                case 0:
                                    result.Add(new signalDtos()
                                    {
                                        data = new Dictionary<string, double>()
                                    });
                                    result.Last().data.Add(metric.timestamp.Ticks.ToString(), calculated);
                                    break;
                            }
                            
                            channelId++;
                            if (channelId == 17)
                            {
                                countRepeat = 1;
                                channelId = 1;
                            }
                        }
                    }
                }

            });
            return result;
        }
    }
}
