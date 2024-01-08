using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Services
{
    internal class AppConfiguration : IAppConfiguration
    {
        private string _appIdentifier = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        public string AppIdentifier => _appIdentifier;

        public string TempFolderPath => Path.GetTempPath();

        public string AcmeFolderName => "acme_" + AppIdentifier;

        public string LogArchiveFolderName => "log_archive";

        public string PresentationFileName => "how_to_use_slido.pptx";

        public string PowerPointAddonsFileName => "powerpoint_addons.txt";

        public string SlidoApiUrl => "https://api.slido.com/global/api/";

        public string PresentationEndpoint => "powerpoint-addin/presentation";

        public string PowerPointAddonsRegistryKey => "Software\\Microsoft\\Office\\PowerPoint\\Addins";

        public string LogArchiveDefaultFileName => "LogArchive";

        public string LogArchiveDefaultExtension => ".zip";

        public string LogArchiveFilter => "ZIP Files|*.zip";

        public string LogFileName => "log.log";
    }
}
