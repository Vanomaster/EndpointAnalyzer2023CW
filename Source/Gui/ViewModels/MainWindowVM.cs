using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Gui.Views;
using Prism.Commands;
using Prism.Mvvm;

namespace Gui.ViewModels
{
    public class MainWindowVm : BindableBase
    {
        #region Fields
        private readonly MainWindow mainWindow;
        private object mainFrame;
        private readonly Dictionary<string, Page> pagesByName = new (); // TODO заменить di контейнером
        #endregion

        #region Properties
        public object MainFrame
        {
            get => mainFrame;

            set
            {
                if (value == mainFrame)
                {
                    return;
                }

                mainFrame = value;
                RaisePropertyChanged(nameof(MainFrame));
            }
        }
        #endregion

        #region Commands
        public DelegateCommand CloseWindowCommand { get; set; }

        public DelegateCommand MinimizeWindowCommand { get; set; }

        public DelegateCommand<string> LoadScreenCommand { get; set; }
        #endregion

        public MainWindowVm(MainWindow main)
        {
            mainWindow = main;
            CloseWindowCommand = new DelegateCommand(CloseWindow);
            MinimizeWindowCommand = new DelegateCommand(MinimizeWindow);
            LoadScreenCommand = new DelegateCommand<string>(LoadPage);
            InitPages();
            LoadPage("home");
        }

        private void InitPages()
        {
            pagesByName.TryAdd("home", new MainPage());
            pagesByName.TryAdd("AnalysisResult", new ResultsMainPage(this));
            pagesByName.TryAdd("Benchmark", new BenchmarkMainPage(this));
            pagesByName.TryAdd("scheduler", new SchedulerMainPage(this));
        }

        private void LoadPage(string text)
        {
            if (pagesByName.TryGetValue(text, out var page))
            {
                MainFrame = page;
            }
        }

        public void NavigationGoBack()
        {
            if (mainWindow.frameMain.CanGoBack)
            {
                mainWindow.frameMain.GoBack();
            }
        }

        private void CloseWindow()
        {
            mainWindow.Close();
        }

        private void MinimizeWindow()
        {
            mainWindow.WindowState = WindowState.Minimized;
        }
    }
}
