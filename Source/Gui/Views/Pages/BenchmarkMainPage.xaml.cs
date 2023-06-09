using System.Collections;
using System.Windows.Controls;
using Gui.ViewModels;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для BenchmarkMainPage.xaml
    /// </summary>
    public partial class BenchmarkMainPage : Page
    {
        private BenchmarkMainPageVm benchmarkMainPageVm;

        public BenchmarkMainPage(MainWindowVm mainWindowVM)
        {
            InitializeComponent();
            benchmarkMainPageVm = new BenchmarkMainPageVm(mainWindowVM);
            this.DataContext = benchmarkMainPageVm;
        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedItems = BenchmarkDatagrid.SelectedItems;
            benchmarkMainPageVm.DeleteCommand.Execute((IList)selectedItems);
        }
    }
}
