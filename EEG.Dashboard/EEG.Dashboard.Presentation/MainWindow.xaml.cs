using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls.Chart;
using Microsoft.AspNetCore.SignalR.Client;
using Telerik.Windows.Controls.ChartView;
using EEG.ProxySocket.Domain.Dtos;
using EEG.Dashboard.Application.Common.Interfaces.Handlers;
using EEG.Dashboard.Application.Common.Interfaces.Cache;
using EEG.Dashboard.Application.Common.Interfaces;
using EEG.Dashboard.Application.Common.Interfaces.Consumers;

namespace EEG.Dashboard.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private commandDto commandToSend;
        IDashboardCache _dashboardCache;
        ISignalRConsumer _signalRConsumer;
        public int recentChannelId;
        LineSeries series;
        public MainWindow(IDashboardCache dashboardCache , IDashboardHandler dashboardHandler ,
            ISignalRConsumer signalRConsumer)
        {
            InitializeComponent();
            _signalRConsumer = signalRConsumer;
            
            _dashboardCache = dashboardCache;
            series = (LineSeries)this.chart.Series[0];
            series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "timestamp" };
            series.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "value" };
            recentChannelId = 1;
            GetDataSource();
            StartConnection();
            _signalRConsumer.ReceiveFromProxySocket();
            
        }

        private void GetDataSource()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var o = _dashboardCache.GetDataSource(recentChannelId);
                    if(o != null)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            series.ItemsSource = o;
                        });
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        Thread.Sleep(5000);
                    }
                }
            });
            
            
        }
        private async void StartConnection()
        {
            try
            {
                await _signalRConsumer.GetConnection().StartAsync();
                connectionStatus.Text = "You are connected!";
            }
            catch
            {
                connectionStatus.Text = "You are disconnected!";
            }
        }

        private void submitButton(object sender, RoutedEventArgs e)
        {
            List<int> channelIds = new List<int>();
            if(channelInput.Text != null && timeInput.Text != null)
            {
                channelIds.Add(recentChannelId);
                commandToSend = new commandDto()
                {
                    channelIds = channelIds,
                    signalRConnection = _signalRConsumer.GetConnection().ConnectionId,
                    lastminutes = int.Parse(timeInput.Text.Split(" ")[1]),
                    toRegister = false
                };
                _signalRConsumer.SendCommand(commandToSend) ;
                recentChannelId = int.Parse(channelInput.Text);
                channelIds = new List<int>();
                channelIds.Add(recentChannelId);
                commandToSend = new commandDto()
                {
                    channelIds = channelIds,
                    signalRConnection = _signalRConsumer.GetConnection().ConnectionId,
                    lastminutes = int.Parse(timeInput.Text.Split(" ")[1]),
                    toRegister = true
                };
                _signalRConsumer.SendCommand(commandToSend);
            }

        }

        private async void Reconnect(object sender, RoutedEventArgs e)
        {
            connectionStatus.Text = "connecting...";
            try
            {
                await _signalRConsumer.GetConnection().StartAsync();
                connectionStatus.Text = "You are connected!";
            }
            catch
            {
                if(_signalRConsumer.GetConnection().State == HubConnectionState.Connected)
                {
                    connectionStatus.Text = "You are connected!";
                }
                else
                {
                    connectionStatus.Text = "You are disconnected!";
                }
                
            }
        }
    }
}
