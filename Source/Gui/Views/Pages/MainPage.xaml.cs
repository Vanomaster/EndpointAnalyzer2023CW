using Gui.ViewModels;
using System.Windows.Controls;

namespace Gui.Views
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainPageVm();
        }
    }
}
