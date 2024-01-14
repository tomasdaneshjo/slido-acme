using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Acme.Services
{
    internal class PowerPointService : IPowerPointService
    {
        private ILogger<PowerPointService> _logger;
        private IWindowsRegistryService _windowsRegistryService;
        private IAppConfiguration _appConfiguration;

        public PowerPointService(ILogger<PowerPointService> logger, IWindowsRegistryService windowsRegistryService, IAppConfiguration appConfiguration) 
        {
            _logger = logger; 
            _windowsRegistryService = windowsRegistryService;
            _appConfiguration = appConfiguration;   
        }

        public bool OpenPresentation(string pathToPresentation)
        {
            _logger.LogInformation($"Opening presentation \"{pathToPresentation}\"");
            if (!HasProgramAssociatedWithPowerPointExtension())
            {
                _logger.LogInformation($"No program associated with extension \"{_appConfiguration.PresentationFileExtension}\"");
                return false;
            }
            try
            {
                var psi = new ProcessStartInfo();
                psi.FileName = pathToPresentation;
                psi.UseShellExecute = true;
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to open presentation.");
                return false;
            }
            return true;
        }



        public bool HasProgramAssociatedWithPowerPointExtension()
        {
            var key = _windowsRegistryService.GetClassesRootSubKey(_appConfiguration.PresentationFileExtension);
            return key != null;
        }
    }
}
