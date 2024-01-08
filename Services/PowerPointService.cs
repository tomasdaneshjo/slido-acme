using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using Microsoft.Extensions.Logging;

namespace Acme.Services
{
    internal class PowerPointService : IPowerPointService
    {
        private ILogger<PowerPointService> _logger;

        public PowerPointService(ILogger<PowerPointService> logger) { _logger = logger; }

        public bool OpenPresentation(string pathToPresentation)
        {
            _logger.LogInformation($"Opening presentation \"{pathToPresentation}\"");

            try
            {
                var powerPointApplication = new PowerPoint.Application();
                powerPointApplication.Visible = MsoTriState.msoTrue;

                var powerPointPresentations = powerPointApplication.Presentations;
                var presentation = powerPointPresentations.Open(pathToPresentation, MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoTrue);
                var slides = presentation.Slides;
                var slideShowSettings = presentation.SlideShowSettings;
                slideShowSettings.Run();
                var slideShowWindows = powerPointApplication.Windows;
                _logger.LogInformation("Presentation opened.");
                return true;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to open presentation.");
            }
            return false;
        }

    }
}
