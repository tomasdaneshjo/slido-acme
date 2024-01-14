using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Services
{
    internal class WindowsRegistryService : IWindowsRegistryService
    {
        private ILogger<WindowsRegistryService> _logger;

        public WindowsRegistryService(ILogger<WindowsRegistryService> logger) {  _logger = logger; }

        public IEnumerable<string> GetCurrentUserSubKeys(string key)
        {
            _logger.LogInformation($"Getting sub keys for key \"{key}\"");
            try
            {
                using var registryKey = Registry.CurrentUser.OpenSubKey(key);

                if (registryKey == null)
                {
                    return Enumerable.Empty<string>();
                }
                return registryKey.GetSubKeyNames();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get sub keyes for key \"{key}\"");
            }
            return Enumerable.Empty<string>();
        }



        public RegistryKey? GetClassesRootSubKey(string key)
        {
            _logger.LogInformation($"Getting classes root sub key \"{key}\"");
            try
            {
                var registryKey = Registry.ClassesRoot.OpenSubKey(key);

                return registryKey;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get classes root sub key  \"{key}\"");
            }
            return null;
        }
    }
}
