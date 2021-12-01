import json
from pathlib import Path
import os
from EEG_AICore_Application.Common.Interface.Configuration.IEnvironmentConfig import IEnviromentConfig

BASE_DIR = Path(__file__).resolve().parent.parent.parent

class EnviromentConfig(IEnviromentConfig):
    def __init__(self):
        with open(os.path.join(BASE_DIR , 'Settings.json')) as f:
            self.Settings = json.load(f)



