using Gui.ViewModels;
using System.Windows.Controls;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для AuditResultsMainPage.xaml
    /// </summary>
    public partial class ResultsMainPage : Page
    {
        private ResultsMainPageVm a;

        public ResultsMainPage(MainWindowVm mainWindowVM)
        {
            InitializeComponent();
            this.a = new ResultsMainPageVm(mainWindowVM);
            this.DataContext = a;
        }

        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            //a.DataGridSorting.Execute(e);
        }
    }
}
