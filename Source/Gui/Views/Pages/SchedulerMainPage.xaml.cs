using System.Collections;
using System.Windows.Controls;
using Gui.ViewModels;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для AuditSchedulerMainPage.xaml
    /// </summary>
    public partial class SchedulerMainPage : Page
    {
        SchedulerMainPageVm x;

        public SchedulerMainPage(MainWindowVm mainWindowVM)
        {
            InitializeComponent();
            x = new SchedulerMainPageVm(mainWindowVM);
            this.DataContext = x;
        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedItems = SchedulerDatagrid.SelectedItems;
            x.DeleteCommand.Execute((IList)selectedItems);
        }
    }
}
