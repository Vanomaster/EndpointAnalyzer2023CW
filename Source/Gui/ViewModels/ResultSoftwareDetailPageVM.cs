using CleanModels.Analysis;
using CleanModels.Software;

namespace Gui.ViewModels
{
    internal class ResultSoftwareDetailPageVm : ResultAbstractDetailPage<Software>
    {
        public ResultSoftwareDetailPageVm(AnalysisResult analysisResult)
            : base(analysisResult)
        {
        }

        /// <inheritdoc/>
        protected override string GetEntityTextToSearch(Software entity)
        {
            return entity.Name + " " + entity.Version;
        }
    }
}
