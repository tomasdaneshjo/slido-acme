using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Services
{
    public interface ILogArchiverService
    {
        void ArchiveLogs(string fileName);
    }
}
