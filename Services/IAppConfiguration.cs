using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Services
{
    public interface IAppConfiguration
    {
        string TempFolderPath { get; }
        string AcmeFolderName { get; }
        string LogArchiveFolderName { get; }
        string PresentationFileName { get; }
        string PresentationFileExtension { get; }
        string SlidoApiUrl { get; }
        string PresentationEndpoint { get; }
        string PowerPointAddonsFileName { get; }
        string PowerPointAddonsRegistryKey { get; }
        string LogArchiveDefaultFileName { get; }
        string LogArchiveDefaultExtension { get; }
        string LogArchiveFilter { get; }
        string AppIdentifier { get; }
        string LogFileName { get; }
    }
}
