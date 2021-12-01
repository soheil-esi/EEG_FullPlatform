import threading
from time import sleep
from EEG_AICore_Application.Common.Interface.Repositories.MessageBrokers.IKafkaRepository import IKafkaRepository
from EEG_AICore_Application.Common.Interface.Configuration.IEnvironmentConfig import IEnviromentConfig
from confluent_kafka.avro import AvroConsumer
from confluent_kafka import TopicPartition
from pandas import DataFrame
from threading import Event 
from confluent_kafka.avro import AvroProducer
from confluent_kafka import avro
from pathlib import Path
import os

BASE_DIR = Path(__file__).resolve().parent

class KafkaRepository(IKafkaRepository):
    def __init__(self , IEnvironmentConfig : IEnviromentConfig) -> None:
        self.host : str = IEnvironmentConfig.Settings["kafkaEndPoints"]["host"]
        self.port : int = IEnvironmentConfig.Settings["kafkaEndPoints"]["port"]
        self.OnRecieve : Event = Event()
        self.schemaHost : str = IEnvironmentConfig.Settings["kafkaEndPoints"]["schemaRegistryHost"]
        self.schemaPort : int = IEnvironmentConfig.Settings["kafkaEndPoints"]["schemaRegistryPort"]
        self.consumerTopic : str = IEnvironmentConfig.Settings["kafkaEndPoints"]["Consumer"]
        self.producerTopic : str = IEnvironmentConfig.Settings["kafkaEndPoints"]["Producer"]
        self.default_group_name = "AICore121`121"
        self._localCache : dict = dict()
        self.consumer_config = {"bootstrap.servers": ""+ self.host +":"+ self.port,
                        "schema.registry.url": "http://"+ self.schemaHost +":" + self.schemaPort,
                        "group.id": self.default_group_name,
                        "auto.offset.reset": "earliest"}
        self._isRunning : bool = False
        self.consumer = AvroConsumer(self.consumer_config)
        self.consumer.assign([TopicPartition(self.consumerTopic , 0) , TopicPartition(self.consumerTopic , 1) , 
                        TopicPartition(self.consumerTopic , 2) , TopicPartition(self.consumerTopic , 3)])
        self.consumer.subscribe([self.consumerTopic])
        self.producer_config = {"bootstrap.servers": ""+ self.host +":"+ self.port,
                        "schema.registry.url": "http://"+ self.schemaHost +":" + self.schemaPort,}
        self.key_schema, self.value_schema = self.load_avro_schema_from_file(os.path.join(BASE_DIR , "signalDtos.avsc"))
        self.producer = AvroProducer(self.producer_config, default_key_schema=self.key_schema, default_value_schema=self.value_schema)
        self.startTask = threading.Thread(target = self.StartConsuming , args=() , daemon=True)
        self.lock = threading.Lock()

    def load_avro_schema_from_file(self , schema_file):
        key_schema_string = """
        {"type": "int"}
        """

        self.key_schema = avro.loads(key_schema_string)
        self.value_schema = avro.load(schema_file)

        return self.key_schema, self.value_schema

    def StartConsuming(self):
        while(1):
            try:
                message = self.consumer.poll(1)
            except Exception as e:
                print(f"Exception while trying to poll messages - {e}")
            else:
                if message:
                    print(f"Successfully poll a record from "
                        f"Kafka topic: {message.topic()}, partition: {message.partition()}, offset: {message.offset()}\n"
                        f"message key: {message.key()}")
                    self.lock.acquire()
                    if(message.key() != None):

                        if int(message.key()) in self._localCache:
                            self._localCache[int(message.key())] = self._localCache[int(message.key())].append(
                                DataFrame(message.value()["data"].items() , columns=['TimeStamp', 'data']).set_index("TimeStamp")
                            )
                        else:
                            self._localCache[int(message.key())] = DataFrame()
                            self._localCache[int(message.key())] = self._localCache[int(message.key())].append(
                                DataFrame(message.value()["data"].items() , columns=['TimeStamp', 'data']).set_index("TimeStamp")
                            )
                        
                    self.lock.release()
                    self.consumer.commit()
                else:
                    # print("No new messages at this point. Try again later.")
                    pass
    
    def GetAll(self):
        aux = self._localCache.copy()
        self._localCache = dict()
        return aux
    
    def Send(self , key , dataToBeSent):
        try:
            self.producer.produce(topic=self.producerTopic, partition = int((key - 1)/4),key=int(key), value=dataToBeSent.to_dict())
        except Exception as e:
            print(f"Exception while producing record value with the key of {int((key - 1)/4)} : {e}")
        else:
            print(f"Successfully producing record value to {self.producerTopic} in partition {int((key - 1)/4)}")
        self.producer.flush()
