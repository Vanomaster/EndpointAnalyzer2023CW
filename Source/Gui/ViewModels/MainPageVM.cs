using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanModels.Analysis;
using CleanModels.Network;
using CleanModels.Schedule;
using Client.Clients;
using Client.Clients.Base;
using Gui.Common;
using Gui.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using NetworkService;
using Prism.Commands;
using Prism.Mvvm;
using static BenchmarkService.Benchmark;
using Benchmark = CleanModels.Benchmark.Benchmark;
using MessageDialog = Gui.Common.MessageDialog;

namespace Gui.ViewModels
{
    class MainPageVm : BindableBase
    {
        #region Fields
        private AnalysisScheduleRecord analysisScheduleRecord;
        private ObservableCollection<string> pCNameList;
        private ObservableCollection<string> benchmarkNameList;
        private string selectedPcName;
        private string selectedBenchmarkName;
        private bool isSoftwareUpgradeSelected;
        private bool isHardwareSelected;
        private bool isSoftwareSelected;
        private bool isConfigurationSelected;
        private readonly Dictionary<string, string> pcIpsByNames = new ();
        #endregion

        #region Properties
        public string SelectedPcName
        {
            get => selectedPcName;
            set
            {
                if (value == selectedPcName)
                {
                    return;
                }

                {
                    selectedPcName = value;
                    RaisePropertyChanged(nameof(SelectedPcName));
                }
            }
        }

        public string SelectedBenchmarkName
        {
            get => selectedBenchmarkName;
            set
            {
                if (value == selectedBenchmarkName)
                {
                    return;
                }

                {
                    selectedBenchmarkName = value;
                    RaisePropertyChanged(nameof(SelectedBenchmarkName));
                }
            }
        }

        public ObservableCollection<string> PcNameList
        {
            get => pCNameList;
            set
            {
                if (value == pCNameList)
                {
                    return;
                }

                {
                    pCNameList = value;
                    RaisePropertyChanged(nameof(PcNameList));
                }
            }
        }

        public ObservableCollection<string> BenchmarkNameList
        {
            get => benchmarkNameList;
            set
            {
                if (value == benchmarkNameList)
                {
                    return;
                }

                {
                    benchmarkNameList = value;
                    RaisePropertyChanged(nameof(BenchmarkNameList));
                }
            }
        }

        public AnalysisScheduleRecord CurAnalysisScheduleRecord
        {
            get => analysisScheduleRecord;
            set
            {
                if (value == analysisScheduleRecord) return;
                {
                    analysisScheduleRecord = value;
                    RaisePropertyChanged(nameof(CurAnalysisScheduleRecord));
                }
            }
        }

        public bool IsSoftwareUpgradeSelected
        {
            get => isSoftwareUpgradeSelected;
            set
            {
                if (value == isSoftwareUpgradeSelected) return;
                {
                    isSoftwareUpgradeSelected = value;
                    RaisePropertyChanged(nameof(IsSoftwareUpgradeSelected));
                }
            }
        }

        public bool IsHardwareSelected
        {
            get => isHardwareSelected;
            set
            {
                if (value == isHardwareSelected) return;
                {
                    isHardwareSelected = value;
                    RaisePropertyChanged(nameof(IsHardwareSelected));
                }
            }
        }

        public bool IsSoftwareSelected
        {
            get => isSoftwareSelected;
            set
            {
                if (value == isSoftwareSelected) return;
                {
                    isSoftwareSelected = value;
                    RaisePropertyChanged(nameof(IsSoftwareSelected));
                }
            }
        }

        public bool IsConfigurationSelected
        {
            get => isConfigurationSelected;
            set
            {
                if (value == isConfigurationSelected) return;
                {
                    isConfigurationSelected = value;
                    RaisePropertyChanged(nameof(IsConfigurationSelected));
                }
            }
        }
        #endregion

        #region Commands

        public DelegateCommand CreateTaskCommand { get; set; }

        public DelegateCommand InitCommand { get; set; }
        #endregion

        public MainPageVm()
        {
            HomeModel = new HomeModel(App.ServiceProvider.GetRequiredService<IReadOnlyServiceClient<Network.NetworkClient, Host>>(),
                App.ServiceProvider.GetRequiredService<IEntityServiceClient<BenchmarkClient, Benchmark>>(),
                App.ServiceProvider.GetRequiredService<AnalysisServiceClient>());

            CreateTaskCommand = new DelegateCommand(async () => await CreateTask());
            InitCommand = new DelegateCommand(async () => await Init());
            IsSoftwareUpgradeSelected = false;
            IsHardwareSelected = false;
            IsSoftwareSelected = false;
            IsConfigurationSelected = false;
            PcNameList = new ObservableCollection<string>();
            BenchmarkNameList = new ObservableCollection<string>();
            InitCommand.Execute();
        }

        private HomeModel HomeModel { get; }

        public async Task Init()
        {
            var hostsQueryResult = await Task.Run(() => HomeModel.GetHostsAsync());
            if (!hostsQueryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, hostsQueryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, "Root");

                return;
            }

            foreach (var host in hostsQueryResult.Data)
            {
                pcIpsByNames.TryAdd(host.Name, host.Ip);
            }

            PcNameList.Clear();
            PcNameList.AddRange(pcIpsByNames.Keys.ToList());

            var queryResult = await Task.Run(() => HomeModel.GetAllBenchmarkNamesAsync());
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, "Root");

                return;
            }

            BenchmarkNameList.Clear();
            BenchmarkNameList.AddRange(queryResult.Data);
        }

        private async Task CreateTask()
        {
            StringBuilder errorMessage = new ();
            if (string.IsNullOrWhiteSpace(SelectedPcName))
            {
                errorMessage.AppendLine(TextConstants.PcSelectionError);
            }

            if (string.IsNullOrWhiteSpace(SelectedBenchmarkName))
            {
                errorMessage.AppendLine(TextConstants.BenchmarkSelectionError);
            }

            if (IsSoftwareUpgradeSelected == false &&
                IsHardwareSelected == false &&
                IsSoftwareSelected == false &&
                IsConfigurationSelected == false)
            {
                errorMessage.AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                await DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, errorMessage.ToString()), "Root");

                return;
            }

            if (!pcIpsByNames.TryGetValue(SelectedPcName, out string? pcIp))
            {
                throw new Exception();
            }

            var selectedAnalysisModel = new SelectedAnalysisModel
            {
                IsConfigurationSelected = IsConfigurationSelected,
                IsHardwareSelected = IsHardwareSelected,
                IsSoftwareSelected = IsSoftwareSelected,
                IsSoftwareUpgradeSelected = IsSoftwareUpgradeSelected,
            };

            var analysisModel = new AnalysisModel
            {
                PcIp = pcIp,
                BenchmarkName = SelectedBenchmarkName,
                AnalyzerNames = AnalysisProvider.GetNamesBySelectedAnalysisModel(selectedAnalysisModel),
            };

            var queryResult = await Task.Run(() => HomeModel.AnalyzeAsync(analysisModel));
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, "Root");

                return;
            }

            var messageDialog1 = new MessageDialog(TextConstants.InfoHeader, queryResult.Data);
            await DialogHost.Show(messageDialog1, "Root");
        }
    }
}