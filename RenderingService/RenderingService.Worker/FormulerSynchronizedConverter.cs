using DinkToPdf;
using DinkToPdf.EventDefinitions;
using Microsoft.Extensions.Logging;

namespace RenderingService.Worker
{
    public class FormulerSynchronizedConverter : SynchronizedConverter
    {
        private readonly ILogger _logger;
        public FormulerSynchronizedConverter(ILogger<FormulerSynchronizedConverter> logger) : base(new PdfTools())
        {
            _logger = logger;
            Error += FormulerSynchronizedConverter_Error;
            Finished += FormulerSynchronizedConverter_Finished;
            PhaseChanged += FormulerSynchronizedConverter_PhaseChanged;
            ProgressChanged += FormulerSynchronizedConverter_ProgressChanged;
            Warning += FormulerSynchronizedConverter_Warning;
        }

        private void FormulerSynchronizedConverter_Warning(object sender, WarningArgs e)
        {
            _logger.LogWarning(e.Message, e.Document);
        }

        private void FormulerSynchronizedConverter_ProgressChanged(object sender, ProgressChangedArgs e)
        {
            _logger.LogInformation(e.Description, e.Document);
        }

        private void FormulerSynchronizedConverter_PhaseChanged(object sender, PhaseChangedArgs e)
        {
            _logger.LogInformation(e.Description, e);
        }

        private void FormulerSynchronizedConverter_Finished(object sender, FinishedArgs e)
        {
            if (e.Success)
                _logger.LogInformation("The pdf generation have succeeded", e);
            else
                _logger.LogWarning("The pdf generation have failed", e);
        }

        private void FormulerSynchronizedConverter_Error(object sender, ErrorArgs e)
        {
            _logger.LogError(e.Message, e.Document);
        }
    }
}
