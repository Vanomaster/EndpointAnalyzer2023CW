using Gui.ViewModels;
using System.Windows.Controls;
using CleanModels.Analysis;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для ResultUpgradableSoftwarePage.xaml
    /// </summary>
    public partial class ResultUpgradableSoftwarePage : Page
    {
        public ResultUpgradableSoftwarePage(AnalysisResult analysisResult)
        {
            InitializeComponent();
            this.DataContext = new ResultUpgradableSoftwarePageVm(analysisResult);
        }
    }
}
