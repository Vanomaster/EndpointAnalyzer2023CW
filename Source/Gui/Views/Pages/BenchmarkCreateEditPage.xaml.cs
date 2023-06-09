using Gui.ViewModels;
using System.Windows.Controls;
using CleanModels.Benchmark;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateEditBenchmarkPage.xaml
    /// </summary>
    public partial class BenchmarkCreateEditPage : Page
    {
        public BenchmarkCreateEditPage(MainWindowVm mainWindowVM, Benchmark curBenchmark)
        {
            InitializeComponent();
            this.DataContext = new BenchmarkCreateEditPageVm(mainWindowVM, curBenchmark);
        }
    }
}
