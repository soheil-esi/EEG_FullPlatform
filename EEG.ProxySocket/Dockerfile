FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

RUN mkdir app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS Build

COPY . /app

COPY . .

RUN dotnet restore "/app/EEG.ProxySocket.WebHost1/EEG.ProxySocket.WebHost.csproj" 

RUN dotnet publish -c Release -o /app/publish

ENV TZ=Asia/Tehran
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN echo $(ls )

RUN chmod +x entrypoint.sh
WORKDIR /app/publish
ENTRYPOINT ["dotnet", "EEG.ProxySocket.WebHost.dll"]
