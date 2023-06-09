using Gui.ViewModels;
using System.Windows.Controls;
using CleanModels.Schedule;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для SchedulerAddEditPage.xaml
    /// </summary>
    public partial class SchedulerAddEditPage : Page
    {
        public SchedulerAddEditPage(MainWindowVm mainWindowVM, AnalysisScheduleRecord analysisScheduleRecord)
        {
            InitializeComponent();
            this.DataContext = new SchedulerAddEditPageVm(mainWindowVM, analysisScheduleRecord);
        }
    }
}
