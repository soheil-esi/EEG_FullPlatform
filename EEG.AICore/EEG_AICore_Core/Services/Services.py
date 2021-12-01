from EEG_AICore_Application.Common.Interface.Handlers.IDspHandler import IDspHandler
from EEG_AICore_Core.Services.IServices import IServices
from EEG_AICore_Application.Common.Interface.Configuration.IEnvironmentConfig import IEnviromentConfig
from EEG_AICore_Infrastructure.AIProcessors.AIProcessor import AIProcessor
from EEG_AICore_Infrastructure.Configuration.EnviromentConfig import EnviromentConfig
from EEG_AICore_Application.Common.Interface.Repositories.MessageBrokers.IKafkaRepository import IKafkaRepository
from EEG_AICore_Infrastructure.Handlers.DspHandler import DspHandler
from EEG_AICore_Infrastructure.Repositories.MessageBrokers.KafkaRepository import KafkaRepository

class Services(IServices):
        def __init__(self):
            self.IEnviromentConfig = IEnviromentConfig(EnviromentConfig()).EnviromentConfig
            self.IKafkaRepository = IKafkaRepository(KafkaRepository(self.IEnviromentConfig)).KafkaRepository
            self.IAIProcessor = AIProcessor()
            self.IDspHandler = IDspHandler(DspHandler(self.IEnviromentConfig , self.IKafkaRepository , self.IAIProcessor)).DspHandler
            self.IDspHandler.Start()
