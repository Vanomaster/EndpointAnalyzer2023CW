using CleanModels.Analysis;
using CleanModels.Software;

namespace Gui.ViewModels
{
    internal class ResultUpgradableSoftwarePageVm : ResultAbstractDetailPage<UpgradableSoftware>
    {
        public ResultUpgradableSoftwarePageVm(AnalysisResult analysisResult)
            : base(analysisResult)
        {
        }

        /// <inheritdoc/>
        protected override string GetEntityTextToSearch(UpgradableSoftware entity)
        {
            return entity.Name + " " + entity.CurrentVersion + " " + entity.NewVersion;
        }
    }
}
