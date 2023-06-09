using Gui.ViewModels;
using System.Windows.Controls;
using CleanModels.Analysis;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для ResultConfigurationDetailPage.xaml
    /// </summary>
    public partial class ResultConfigurationDetailPage : Page
    {
        public ResultConfigurationDetailPage(AnalysisResult analysisResult)
        {
            InitializeComponent();
            this.DataContext = new ResultConfigurationDetailPageVm(analysisResult);
        }
    }
}
