using Gui.ViewModels;
using System.Windows.Controls;
using CleanModels.Analysis;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для ResultSoftwareDetailPage.xaml
    /// </summary>
    public partial class ResultSoftwareDetailPage : Page
    {
        public ResultSoftwareDetailPage(AnalysisResult analysisResult)
        {
            InitializeComponent();
            this.DataContext = new ResultSoftwareDetailPageVm(analysisResult);
        }
    }
}
