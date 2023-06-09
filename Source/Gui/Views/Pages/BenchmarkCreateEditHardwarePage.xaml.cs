using Gui.ViewModels;
using System.Windows.Controls;
using CleanModels.Benchmark;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для BenchmarkCreateEdiHardwarePage.xaml
    /// </summary>
    public partial class BenchmarkCreateEditHardwarePage : Page
    {
        public BenchmarkCreateEditHardwarePage(Benchmark benchmark)
        {
            InitializeComponent();
            this.DataContext = new BenchmarkCreateEditHardwarePageVm(benchmark, this.HardwareGrid);
        }

    }
}
