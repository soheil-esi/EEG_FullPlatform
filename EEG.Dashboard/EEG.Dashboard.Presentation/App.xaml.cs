using EEG.Dashboard.Application.Cache;
using EEG.Dashboard.Application.Common.Interfaces;
using EEG.Dashboard.Application.Common.Interfaces.Cache;
using EEG.Dashboard.Application.Handlers;
using EEG.Dashboard.Application.Consumers;
using EEG.ProxySocket.Domain.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using EEG.Dashboard.Application.Common.Interfaces.Handlers;
using EEG.Dashboard.Application.Common.Interfaces.Consumers;

namespace EEG.Dashboard.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            
            services.AddSingleton<IInMemoryCache<ObservableCollection<signalDto>>, InMemoryCache<ObservableCollection<signalDto>>>();
            services.AddSingleton<IDashboardCache, DashboardCache>();
            services.AddSingleton<IDashboardHandler, DashboardHandler>();
            services.AddSingleton<ISignalRConsumer, SignalRConsumer>();
            services.AddSingleton<MainWindow>();
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
