using Gui.ViewModels;
using System.Windows.Controls;
using CleanModels.Analysis;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для ResultHardwareDetailPage.xaml
    /// </summary>
    public partial class ResultHardwareDetailPage : Page
    {
        public ResultHardwareDetailPage(AnalysisResult analysisResult)
        {
            InitializeComponent();
            this.DataContext = new ResultHardwareDetailPageVm(analysisResult);
        }
    }
}
