FROM python:3
ENV PYTHONUNBUFFERED=1
WORKDIR /code
COPY requirements.txt /code/

RUN wget https://github.com/edenhill/librdkafka/archive/v1.6.0.tar.gz\ 
&& tar xvzf v1.6.0.tar.gz && cd librdkafka-1.6.0/ && ./configure && make && make install && ldconfig

RUN pip install -r requirements.txt
COPY . /code/