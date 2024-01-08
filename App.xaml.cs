using Acme.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows;
using LogLevel = NLog.LogLevel;

namespace Acme
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex? _isRunningMutex = null;
        private static readonly string _appName = "acme";
        private IServiceProvider _serviceProvider;
        private ILogger<App> _logger;
        private IAppConfiguration _appConfiguration;

        public App()
        {

            _appConfiguration = new AppConfiguration();

            Directory.CreateDirectory(Path.Combine(_appConfiguration.TempFolderPath, _appConfiguration.AcmeFolderName));


            NLog.LogManager.Setup().LoadConfiguration(builder =>
            {
                builder.ForLogger().FilterMinLevel(LogLevel.Info).WriteToFile((Path.Combine(_appConfiguration.TempFolderPath, _appConfiguration.AcmeFolderName, _appConfiguration.LogFileName)));
            });

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IAppConfiguration>(_ => _appConfiguration);
            services.AddSingleton<ISlidoService, SlidoService>();
            services.AddSingleton<IPowerPointService, PowerPointService>();
            services.AddSingleton<IWindowsRegistryService, WindowsRegistryService>();
            services.AddSingleton<ILogArchiverService, LogArchiverService>();
            services.AddLogging();
            services.AddSingleton<MainWindow>();
            _serviceProvider = services.BuildServiceProvider();

            _serviceProvider.GetRequiredService<ILoggerFactory>().AddNLog();
            _logger = _serviceProvider.GetRequiredService<ILogger<App>>();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            bool createdNew;

            _isRunningMutex = new Mutex(true, _appName, out createdNew);

            if (!createdNew)
            {
                Application.Current.Shutdown();
            }
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (_logger != null)
            {
                _logger.LogError(e.Exception, "Unhandled exception occured.");
            }

            MessageBox.Show("Unexpected error occured.", "Error", MessageBoxButton.OK);

            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Directory.Delete(Path.Combine(_appConfiguration.TempFolderPath, _appConfiguration.AcmeFolderName), true);
            base.OnExit(e);
        }
    }

}
