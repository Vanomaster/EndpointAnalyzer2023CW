using System.Threading.Tasks;
using System.Windows;
using CleanModels.Benchmark;
using CleanModels.Commands.Base;
using Client.Clients.Base;
using Gui.Common;
using Gui.Models;
using Gui.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Mvvm;

namespace Gui.ViewModels
{
    class BenchmarkCreateEditPageVm : BindableBase
    {
        #region Fields
        private MainWindowVm mainVm;
        private Benchmark curBenchmark;
        private bool isCardVisible;
        private bool isModalVisible;
        private bool isConfigurationCardVisible;
        private bool isBenchmarkInfoCardVisible;
        private Visibility infoCardVisibility;
        private Visibility isVisSaveButton;
        private BenchmarkCreateEditConfigurationPage configurationPage;
        private BenchmarkCreateEditHardwarePage hardwarePage;
        private BenchmarkCreateEditSoftwarePage softwarePage;
        #endregion

        #region Properties
        public Visibility IsVisCardActions
        {
            get => isVisSaveButton;
            set
            {
                if (value == isVisSaveButton)
                {
                    return;
                }

                isVisSaveButton = value;
                RaisePropertyChanged(nameof(IsVisCardActions));
            }
        }

        public BenchmarkCreateEditConfigurationPage ConfigurationPage
        {
            get => configurationPage;
            set
            {
                if (value == configurationPage) return;

                configurationPage = value;
                RaisePropertyChanged(nameof(ConfigurationPage));
            }
        }

        public BenchmarkCreateEditHardwarePage HardwarePage
        {
            get => hardwarePage;
            set
            {
                if (value == hardwarePage) return;

                hardwarePage = value;
                RaisePropertyChanged(nameof(HardwarePage));
            }
        }

        public BenchmarkCreateEditSoftwarePage SoftwarePage
        {
            get => softwarePage;
            set
            {
                if (value == softwarePage) return;

                softwarePage = value;
                RaisePropertyChanged(nameof(SoftwarePage));
            }
        }

        public Visibility InfoCardVisibility
        {
            get => infoCardVisibility;
            set
            {
                if (value == infoCardVisibility)
                {
                    return;
                }

                infoCardVisibility = value;
                RaisePropertyChanged(nameof(InfoCardVisibility));
            }
        }

        public bool IsCardVisible
        {
            get => isCardVisible;
            set
            {
                if (value == isCardVisible) return;
                {
                    isCardVisible = value;
                    RaisePropertyChanged(nameof(IsCardVisible));
                }
            }
        }

        public Benchmark CurBenchmark
        {
            get => curBenchmark;
            set
            {
                if (value == curBenchmark) return;
                {
                    curBenchmark = value;
                    RaisePropertyChanged(nameof(CurBenchmark));
                }
            }
        }

        public bool IsModalVisible
        {
            get => isModalVisible;
            set
            {
                if (value == isModalVisible)
                {
                    return;
                }

                {
                    isModalVisible = value;
                    RaisePropertyChanged(nameof(IsModalVisible));
                }
            }
        }

        public bool IsConfigurationCardVisible
        {
            get => isConfigurationCardVisible;
            set
            {
                if (value == isConfigurationCardVisible)
                {
                    return;
                }

                {
                    isConfigurationCardVisible = value;
                    RaisePropertyChanged(nameof(IsConfigurationCardVisible));
                }
            }
        }

        public bool IsBenchmarkInfoCardVisible
        {
            get => isBenchmarkInfoCardVisible;
            set
            {
                if (value == isBenchmarkInfoCardVisible)
                {
                    return;
                }

                {
                    isBenchmarkInfoCardVisible = value;
                    RaisePropertyChanged(nameof(IsBenchmarkInfoCardVisible));
                }
            }
        }
        #endregion

        #region Commands
        public DelegateCommand AddCommand { get; set; }

        public DelegateCommand GetInfoCommand { get; set; }

        public DelegateCommand CloseModalCommand { get; set; }

        public DelegateCommand BenchmardAddEditInfoCardSaveCommand { get; set; }

        #endregion

        public BenchmarkCreateEditPageVm(MainWindowVm mainWindowVm, Benchmark? benchmark)
        {
            mainVm = mainWindowVm;
            if (benchmark == null)
            {
                IsVisCardActions = Visibility.Visible;
                CurBenchmark = new Benchmark { Name = "Новый шаблон" };
                InfoCardVisibility = Visibility.Visible;
            }
            else
            {
                IsVisCardActions = Visibility.Hidden;
                CurBenchmark = benchmark;
                InfoCardVisibility = Visibility.Collapsed;
            }

            BenchmarksModel = new BenchmarksModel(App.ServiceProvider, App.ServiceProvider.GetRequiredService<IEntityServiceClient<BenchmarkService.Benchmark.BenchmarkClient, Benchmark>>());
            ConfigurationPage = new BenchmarkCreateEditConfigurationPage(CurBenchmark);
            HardwarePage = new BenchmarkCreateEditHardwarePage(CurBenchmark);
            SoftwarePage = new BenchmarkCreateEditSoftwarePage(CurBenchmark);
            GetInfoCommand = new DelegateCommand(OpenBenchmarkInfoCard);
            CloseModalCommand = new DelegateCommand(CloseModal);
            BenchmardAddEditInfoCardSaveCommand = new DelegateCommand(async () => await AddEditInfoCardSave());
        }

        private BenchmarksModel BenchmarksModel { get; }

        private async Task AddEditInfoCardSave()
        {
            string errorString = Validator.ValidateString(CurBenchmark.Name, "Название");
            if (!string.IsNullOrEmpty(errorString))
            {
                await DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, errorString), TextConstants.DialogIdentifier);

                return;
            }

            CommandResult commandResult;
            if (CurBenchmark.Id == default)
            {
                commandResult = await Task.Run(() => BenchmarksModel.AddAsync(CurBenchmark));
            }
            else
            {
                commandResult = await Task.Run(() => BenchmarksModel.UpdateAsync(CurBenchmark));
            }

            if (!commandResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, commandResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            MessageDialog messageDialogSuccess;
            if (CurBenchmark.Id == default)
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.BenchmarksModelAddSuccess);
            }
            else
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.BenchmarksModelUpdateSuccess);
            }

            await DialogHost.Show(messageDialogSuccess, TextConstants.DialogIdentifier);
            InfoCardVisibility = Visibility.Collapsed;
            mainVm.LoadScreenCommand.Execute("Benchmark"); // TODO запрос на получение id шаблона
        }

        private void OpenBenchmarkInfoCard()
        {
            IsModalVisible = true;
            IsCardVisible = true;
        }

        private void CloseModal()
        {
            IsModalVisible = false;
            IsCardVisible = false;
        }
    }
}
