import abc 

class IDspHandler:
    def __init__(self , DspHandler) -> None:
        self.DspHandler = DspHandler

    @abc.abstractmethod
    def Start(self):
        raise NotImplementedError