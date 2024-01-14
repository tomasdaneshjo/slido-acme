using Acme.Services;
using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Acme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ISlidoService _slidoService;
        private IPowerPointService _powerPointService;
        private ILogArchiverService _logArchiverService;
        private IAppConfiguration _appConfiguration;

        public MainWindow(ISlidoService slidoService, IPowerPointService powerPointService, ILogArchiverService logArchiverService, IAppConfiguration appConfiguration)
        {
            InitializeComponent();
            _slidoService = slidoService;
            _powerPointService = powerPointService;
            _logArchiverService = logArchiverService;
            _appConfiguration = appConfiguration;
        }

        private async void OpenPowerPointButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                (sender as Button).IsEnabled = false;
            }

            var presentationPath = await _slidoService.DownloadPresentationAsync();

            var canOpenPresentation = _powerPointService.HasProgramAssociatedWithPowerPointExtension();

            if (!canOpenPresentation)
            {

                MessageBox.Show("There is no application that is able to open the presentation.", "Error", MessageBoxButton.OK);
            } 
            else
            {
                var presentationOpened = _powerPointService.OpenPresentation(presentationPath);

                if (!presentationOpened)
                {

                    MessageBox.Show("Something went wrong while opening the presentation.", "Error", MessageBoxButton.OK);
                }
            }

            if (sender.GetType() == typeof(Button))
            {
                (sender as Button).IsEnabled = true;
            }
        }

        private void ArchiveLogs(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = _appConfiguration.LogArchiveDefaultFileName;
            saveFileDialog.DefaultExt = _appConfiguration.LogArchiveDefaultExtension;
            saveFileDialog.Filter = _appConfiguration.LogArchiveFilter;

            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filename = saveFileDialog.FileName;
                _logArchiverService.ArchiveLogs(filename);
            }
        }
    }
}