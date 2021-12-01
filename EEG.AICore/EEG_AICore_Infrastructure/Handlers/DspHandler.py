from EEG_AICore_Application.Common.Interface.Handlers.IDspHandler import IDspHandler
from time import sleep
class DspHandler(IDspHandler):
    def __init__(self , IEnvironmentConfig , IKafkaRepository , IAIProcessor) -> None:
        self.IEnvironmentConfig = IEnvironmentConfig
        self.IKafkaRepository = IKafkaRepository
        self.IAIProcessor = IAIProcessor
        self.IKafkaRepository.startTask.start()
        self.cache = dict()
    def Start(self):
        while(1):
            self.IKafkaRepository.lock.acquire()
            consumedData = dict()
            consumedData = self.IKafkaRepository.GetAll()
            if(len(consumedData) > 0):
                for key in consumedData.keys():
                    self.IKafkaRepository.Send(int(key) , self.IAIProcessor.Denoise(consumedData[key]))
                self.IKafkaRepository.lock.release()
            else :
                self.IKafkaRepository.lock.release()
                sleep(1)
