using System;
using System.Windows;
using JsonFileMonitor.Abstractions.Services;
using JsonFileMonitor.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JsonFileMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider? ServiceProvider { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainView = ServiceProvider.GetRequiredService<MainWindow>();
            mainView.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
            mainView.Show();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRingBearersFileMonitorService, RingBearersFileMonitorService>();
            services.AddSingleton<IRingBearersFileReaderService, RingBearersFileReaderService>();
            services.AddSingleton<IRingBearersDataService, RingBearersDataService>();
            services.AddSingleton(typeof(MainViewModel));
            services.AddSingleton(typeof(MainWindow));
        }
    }
}

