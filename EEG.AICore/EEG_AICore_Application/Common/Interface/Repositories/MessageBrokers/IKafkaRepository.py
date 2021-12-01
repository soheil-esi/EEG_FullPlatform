import abc 

class IKafkaRepository:
    def __init__(self , KafkaRepository) -> None:
        self.KafkaRepository = KafkaRepository

    @abc.abstractmethod
    def StartConsuming(self):
        raise NotImplementedError

    @abc.abstractmethod
    def Send(self):
        raise NotImplementedError