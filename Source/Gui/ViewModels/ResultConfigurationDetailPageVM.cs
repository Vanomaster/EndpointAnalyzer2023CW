using CleanModels.Analysis;
using CleanModels.Benchmark;

namespace Gui.ViewModels
{
    internal class ResultConfigurationDetailPageVm : ResultAbstractDetailPage<UnrecommendedConfiguration>
    {
        public ResultConfigurationDetailPageVm(AnalysisResult analysisResult)
            : base(analysisResult)
        {
        }

        /// <inheritdoc/>
        protected override string GetEntityTextToSearch(UnrecommendedConfiguration entity)
        {
            return entity.Name + " " + entity.ActualVerificationResult + " " + entity.ExpectedVerificationResult;
        }
    }
}
