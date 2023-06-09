using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CleanModels.Network;
using CleanModels.Schedule;
using Client.Clients;
using Client.Clients.Base;
using Common.Extensions;
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
    internal class SchedulerAddEditPageVm : BindableBase
    {
        #region Fields

        private MainWindowVm mainWindowVm;
        private AnalysisScheduleRecord analysisScheduleRecord;
        private ObservableCollection<string> benchmarkNameList;
        private string selectedBenchmarkName;
        private ObservableCollection<string> pCNameList;
        private string recordName;
        private string selectedPcName;
        private string monthDayInterval;
        private string hourInterval;
        private string minuteInterval;
        private bool isSoftwareUpgradeSelected;
        private bool isHardwareSelected;
        private bool isSoftwareSelected;
        private bool isConfigurationSelected;
        private DateOnly startDate;
        private TimeOnly startTime;
        private Visibility isVisButtonCreateTask;
        private readonly Dictionary<string, string> pcIpsByNames = new ();
        #endregion

        #region Properties
        public Visibility IsVisButtonCreateTask
        {
            get => isVisButtonCreateTask;
            set
            {
                if (value == isVisButtonCreateTask) return;
                {
                    isVisButtonCreateTask = value;
                    RaisePropertyChanged(nameof(IsVisButtonCreateTask));
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

        public ObservableCollection<string> BenchmarkNameList
        {
            get => benchmarkNameList;
            set
            {
                if (value == benchmarkNameList) return;
                {
                    benchmarkNameList = value;
                    RaisePropertyChanged(nameof(BenchmarkNameList));
                }
            }
        }

        public string SelectedBenchmarkName
        {
            get => selectedBenchmarkName;
            set
            {
                if (value == selectedBenchmarkName) return;
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
                if (value == pCNameList) return;
                {
                    pCNameList = value;
                    RaisePropertyChanged(nameof(PcNameList));
                }
            }
        }

        public string RecordName
        {
            get => recordName;
            set
            {
                if (value == recordName)
                {
                    return;
                }

                {
                    recordName = value;
                    RaisePropertyChanged(nameof(RecordName));
                }
            }
        }

        public string SelectedPcName
        {
            get => selectedPcName;
            set
            {
                if (value == selectedPcName) return;
                {
                    selectedPcName = value;
                    RaisePropertyChanged(nameof(SelectedPcName));
                }
            }
        }

        public string MonthDayInterval
        {
            get => monthDayInterval;
            set
            {
                if (value == monthDayInterval) return;
                monthDayInterval = value;
                RaisePropertyChanged(nameof(MonthDayInterval));
            }
        }

        public string HourInterval
        {
            get => hourInterval;
            set
            {
                if (value == hourInterval) return;
                hourInterval = value;
                RaisePropertyChanged(nameof(HourInterval));
            }
        }

        public string MinuteInterval
        {
            get => minuteInterval;
            set
            {
                if (value == minuteInterval) return;
                minuteInterval = value;
                RaisePropertyChanged(nameof(MinuteInterval));
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

        public DateOnly StartDate
        {
            get => startDate;
            set
            {
                if (value == startDate) return;
                {
                    startDate = value;
                    RaisePropertyChanged(nameof(StartDate));
                }
            }
        }

        public TimeOnly StartTime
        {
            get => startTime;
            set
            {
                if (value == startTime) return;
                {
                    startTime = value;
                    RaisePropertyChanged(nameof(StartTime));
                }
            }
        }
        #endregion

        #region Commands
        public DelegateCommand CreateTaskCommand { get; set; }

        public DelegateCommand InitCommand { get; set; }
        #endregion

        public SchedulerAddEditPageVm(MainWindowVm mainWindowVm, AnalysisScheduleRecord analysisScheduleRecord)
        {
            this.mainWindowVm = mainWindowVm;
            SchedulerModel = new SchedulerModel(
                App.ServiceProvider,
                App.ServiceProvider.GetRequiredService<ScheduleServiceClient>(),
                App.ServiceProvider.GetRequiredService<IReadOnlyServiceClient<Network.NetworkClient, Host>>(),
                App.ServiceProvider.GetRequiredService<IEntityServiceClient<BenchmarkClient, Benchmark>>());

            CreateTaskCommand = new DelegateCommand(async () => await CreateTask());
            PcNameList = new ObservableCollection<string>();
            BenchmarkNameList = new ObservableCollection<string>();
            this.analysisScheduleRecord = analysisScheduleRecord;
            if (analysisScheduleRecord == null)
            {
                IsVisButtonCreateTask = Visibility.Visible;
                RecordName = string.Empty;
                IsSoftwareUpgradeSelected = false;
                IsHardwareSelected = false;
                IsSoftwareSelected = false;
                IsConfigurationSelected = false;
                StartDate = DateOnly.FromDateTime(DateTime.Now);
                StartTime = TimeOnly.FromDateTime(DateTime.Now);
                InitCommand = new DelegateCommand(async () => await Init());
                InitCommand.Execute();
            }
            else
            {
                IsVisButtonCreateTask = Visibility.Hidden;
                CurAnalysisScheduleRecord = analysisScheduleRecord;
                foreach (string analysisName in CurAnalysisScheduleRecord.AnalyzerNames)
                {
                    switch (analysisName)
                    {
                        case @"Анализ конфигураций":
                            IsConfigurationSelected = true;
                            break;
                        case @"Анализ устройств":
                            IsHardwareSelected = true;
                            break;
                        case @"Анализ ПО":
                            IsSoftwareSelected = true;
                            break;
                        case @"Анализ обновлений ПО":
                            IsSoftwareUpgradeSelected = true;
                            break;
                        default: break;
                    }
                }

                RecordName = CurAnalysisScheduleRecord.Name;
                SelectedPcName = CurAnalysisScheduleRecord.Host.Name;
                SelectedBenchmarkName = CurAnalysisScheduleRecord.BenchmarkName;
                PcNameList.Add(SelectedPcName);
                BenchmarkNameList.Add(SelectedBenchmarkName);

                var recurrenceModel = analysisScheduleRecord.Recurrence.ToRecurrenceModel();
                StartDate = new DateOnly(recurrenceModel.StartDateTime.Year, recurrenceModel.StartDateTime.Month, recurrenceModel.StartDateTime.Day);
                StartTime = new TimeOnly(recurrenceModel.StartDateTime.Hour, recurrenceModel.StartDateTime.Minute, recurrenceModel.StartDateTime.Second);
                MonthDayInterval = recurrenceModel.MonthDayInterval.ToString();
                HourInterval = recurrenceModel.HourInterval.ToString();
                MinuteInterval = recurrenceModel.MinuteInterval.ToString();
            }
        }

        private SchedulerModel SchedulerModel { get; }

        public async Task Init()
        {
            var hostsQueryResult = await Task.Run(() => SchedulerModel.GetHostsAsync());
            if (!hostsQueryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, hostsQueryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            foreach (var host in hostsQueryResult.Data)
            {
                pcIpsByNames.TryAdd(host.Name, host.Ip);
            }

            PcNameList.Clear();
            PcNameList.AddRange(pcIpsByNames.Keys.ToList());

            var queryResult = await Task.Run(() => SchedulerModel.GetAllBenchmarkNamesAsync());
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            BenchmarkNameList.Clear();
            BenchmarkNameList.AddRange(queryResult.Data);
        }

        private async Task CreateTask()
        {
            var dateTime = new DateTime(
                StartDate.Year,
                StartDate.Month,
                StartDate.Day,
                StartTime.Hour,
                StartTime.Minute,
                StartTime.Second);

            StringBuilder errorMessage = new ();
            if (SelectedPcName == null)
            {
                errorMessage.AppendLine("Вы не выбрали ПК для анализа");
            }

            if (SelectedBenchmarkName == null)
            {
                errorMessage.AppendLine("Вы не выбрали Шаблон для анализа");
            }

            if (IsSoftwareUpgradeSelected == false &&
                IsHardwareSelected == false &&
                IsSoftwareSelected == false &&
                IsConfigurationSelected == false)
            {
                errorMessage.AppendLine("Вы не выбрали хотя бы один анализ");
            }

            string errorTimeMessage = Validator.ValidateRecurence(MonthDayInterval, HourInterval, MinuteInterval);
            errorMessage.AppendLine(errorTimeMessage);
            if (!string.IsNullOrWhiteSpace(errorMessage.ToString()))
            {
                await DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, errorMessage.ToString()), TextConstants.DialogIdentifier);

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

            var recurrenceModel = new RecurrenceModel
            {
                StartDateTime = dateTime,
                MonthDayInterval = string.IsNullOrWhiteSpace(MonthDayInterval) ? null : byte.Parse(MonthDayInterval),
                HourInterval = string.IsNullOrWhiteSpace(HourInterval) ? null : byte.Parse(HourInterval),
                MinuteInterval = string.IsNullOrWhiteSpace(MinuteInterval) ? null : byte.Parse(MinuteInterval),
            };

            var scheduleRecord = new AnalysisScheduleRecord
            {
                Name = RecordName,
                Host = new Host
                {
                    Ip = pcIp,
                },
                BenchmarkName = SelectedBenchmarkName,
                AnalyzerNames = AnalysisProvider.GetNamesBySelectedAnalysisModel(selectedAnalysisModel),
                Recurrence = recurrenceModel.ToCronExpression(),
                Enabled = true,
            };

            var hostsQueryResult = await Task.Run(() => SchedulerModel.AddOrUpdateAsync(scheduleRecord));
            if (!hostsQueryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, hostsQueryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);
            }

            mainWindowVm.LoadScreenCommand.Execute("scheduler");
        }
    }
}
