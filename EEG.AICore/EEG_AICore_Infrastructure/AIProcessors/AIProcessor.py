import pywt
import numpy as np
from pandas import read_pickle
from sklearn.preprocessing import MinMaxScaler
from scipy.signal import butter,filtfilt,iirfilter,freqz,lfilter
from keras.models import load_model
from os import Path
BASE_DIR = Path(__file__).resolve().parent

class AIProcessor():
    def __init__(self) -> None:
        self._datasetToScale = read_pickle(BASE_DIR + '/eegDataSet.pkl')
        self._noiseScaler = MinMaxScaler()
        self._noiseScaler.fit(self._datasetToScale[1].values.reshape(178 , 100))
        self._cleanScaler = MinMaxScaler()
        self._cleanScaler.fit(self._datasetToScale[0].values.reshape(178 , 100))
        self._DenoiseModel = load_model(BASE_DIR + '/EEG_Denoiser.h5')

    def madev(self ,d, axis=None):
        """ Mean absolute deviation of a signal """
        return np.mean(np.absolute(d - np.mean(d, axis)), axis)
    
    def wavelet_denoising(self , x, wavelet='sym9', level=1):
        coeff = pywt.wavedec(x, wavelet, mode="per")
        sigma = (1/0.6745) * self.madev(coeff[-level])
        uthresh = sigma * np.sqrt(2 * np.log(len(x)))
        coeff[1:] = (pywt.threshold(i, value=uthresh, mode='hard') for i in coeff[1:])
        return pywt.waverec(coeff, wavelet, mode='per')

    def Denoise(self, data, wavelet='db8', level=1):
        if len(data['data'] > 0):
            data = data.iloc[::2 , :]
            print("*************** " + str(len(data['data'])))
            data = self.butter_lowpass_filter(data, 42 , 500 )
            data['data'] = data['data'] - np.mean(data['data'])
            scaledNoise = self._noiseScaler.transform(data['data'].values.reshape(1 , 100))
            scaledClean = self._DenoiseModel.predict(scaledNoise)
            data['data'] = self._cleanScaler.inverse_transform(scaledClean).reshape(-1 , 1)
            data = self.IIR_lowpass_filter(data)
        return data
    
    def butter_lowpass_filter(self , data, cutoff, fs):
        order = 10
        nyq = 0.5 * fs  # Nyquist Frequency
        n = len(data['data']) # total number of samples
        normal_cutoff = cutoff / nyq
        # Get the filter coefficients 
        b, a = butter(order, normal_cutoff, btype='low', analog=False)
        try:
            data['data'] = filtfilt(b, a, data['data'])
        except:
            pass
        return data
    def IIR_lowpass_filter(self , df):
        fs = 250
        cutoff = 40
        b, a = iirfilter(35, cutoff / (fs / 2), btype="lowpass", analog=False)
        w, h = freqz(b, a, worN=8000)
        df = df.apply(lambda data: lfilter(b, a, data))
        df = df - df.mean(axis=0)
        return df