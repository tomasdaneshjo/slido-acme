using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Services
{
    internal class LogArchiverService : ILogArchiverService
    {
        private IAppConfiguration _appConfiguration;
        private IWindowsRegistryService _windowsRegistryService;
        private ILogger<LogArchiverService> _logger;

        public LogArchiverService(ILogger<LogArchiverService> logger, IAppConfiguration appConfiguration, IWindowsRegistryService windowsRegistryService)
        {
            _appConfiguration = appConfiguration;
            _windowsRegistryService = windowsRegistryService;
            _logger = logger;
        }

        public void ArchiveLogs(string fileName)
        {
            var acmeTempFolderPath = Path.Combine(_appConfiguration.TempFolderPath, _appConfiguration.AcmeFolderName);
            var logArchiveDirectoryPath = Path.Combine(acmeTempFolderPath, _appConfiguration.LogArchiveFolderName);
            var logFilePath = Path.Combine(acmeTempFolderPath, _appConfiguration.LogFileName);
            var logCopyFilePath = Path.Combine(logArchiveDirectoryPath, _appConfiguration.LogFileName);
            Directory.CreateDirectory(logArchiveDirectoryPath);

            var powerpointAddonsFile = Path.Combine(logArchiveDirectoryPath, _appConfiguration.PowerPointAddonsFileName);
            if (File.Exists(powerpointAddonsFile))
            {
                File.Delete(powerpointAddonsFile);
            }

            _logger.LogInformation("Getting powerpoint addons.");
            using (FileStream fs = File.Create(powerpointAddonsFile))
            {
                var subKeys = _windowsRegistryService.GetCurrentUserSubKeys(_appConfiguration.PowerPointAddonsRegistryKey);
                fs.Write(new UTF8Encoding(true).GetBytes(string.Join("\n", subKeys)));
            }

            if (File.Exists(logFilePath))
            {
                if (File.Exists(logCopyFilePath))
                {
                    File.Delete(fileName);
                }
                _logger.LogInformation("Copying logs.");
                File.Copy(logFilePath, logCopyFilePath);
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            _logger.LogInformation("Archiving logs.");
            ZipFile.CreateFromDirectory(logArchiveDirectoryPath, fileName);
            _logger.LogInformation("Logs archived.");
        }
    }
}
