using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Services
{
    internal class SlidoService : ISlidoService
    {
        private IAppConfiguration _appConfiguration;
        private ILogger<SlidoService> _logger;

        public SlidoService(ILogger<SlidoService> logger, IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            _logger = logger;
        }

        public async Task<string> DownloadPresentationAsync()
        {
            _logger.LogInformation("Downloading presentation.");
            var presentationFilePath = Path.Combine(_appConfiguration.TempFolderPath, _appConfiguration.AcmeFolderName, _appConfiguration.PresentationFileName);
            using (WebClient webClient = new())
            {
                await webClient.DownloadFileTaskAsync(new Uri(_appConfiguration.SlidoApiUrl + _appConfiguration.PresentationEndpoint), presentationFilePath);
            }
            _logger.LogInformation("Presentation downloaded.");
            return presentationFilePath;
        }
    }
}
