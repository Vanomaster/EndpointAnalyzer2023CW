using Gui.ViewModels;
using System.Windows.Controls;
using CleanModels.Benchmark;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateEditBenchmarkSoftwarePage.xaml
    /// </summary>
    public partial class BenchmarkCreateEditSoftwarePage : Page
    {
        public BenchmarkCreateEditSoftwarePage(Benchmark benchmark)
        {
            InitializeComponent();
            this.DataContext = new BenchmarkCreateEditSoftwarePageVm(benchmark, SoftwareGrid);
        }
    }
}
