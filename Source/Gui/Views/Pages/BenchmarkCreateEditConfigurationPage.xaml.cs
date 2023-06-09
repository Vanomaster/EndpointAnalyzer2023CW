using Gui.ViewModels;
using System.Windows.Controls;
using CleanModels.Benchmark;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateEditBenchmarkConfigurationPage.xaml
    /// </summary>
    public partial class BenchmarkCreateEditConfigurationPage : Page
    {
        public BenchmarkCreateEditConfigurationPage(Benchmark benchmark)
        {
            InitializeComponent();
            this.DataContext = new BenchmarkCreateEditConfigurationPageVm(benchmark, this.ConfigurationGrid);
        }
    }
}
