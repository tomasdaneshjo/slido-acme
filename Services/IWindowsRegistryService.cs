using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Services
{
    public interface IWindowsRegistryService
    {
        IEnumerable<string> GetCurrentUserSubKeys(string key);
        RegistryKey? GetClassesRootSubKey(string key);
    }
}
