using CleanModels.Analysis;
using CleanModels.Hardware;

namespace Gui.ViewModels
{
    internal class ResultHardwareDetailPageVm : ResultAbstractDetailPage<UnknownHardware>
    {
        public ResultHardwareDetailPageVm(AnalysisResult analysisResult)
            : base(analysisResult)
        {
        }

        /// <inheritdoc/>
        protected override string GetEntityTextToSearch(UnknownHardware entity)
        {
            return entity.HardwareId;
        }
    }
}
