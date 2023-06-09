using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkService;
using CleanModels;
using CleanModels.Network;
using CleanModels.Schedule;
using Client.Clients;
using Client.Clients.Base;
using Common.Extensions;
using Gui.Common;
using Gui.Models;
using Gui.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using NetworkService;
using Prism.Commands;
using Prism.Mvvm;
using static BenchmarkService.Benchmark;
using Benchmark = CleanModels.Benchmark.Benchmark;
using MessageDialog = Gui.Common.MessageDialog;
using MessageDialogWithResponse = Gui.Common.MessageDialogWithResponse;

namespace Gui.ViewModels
{
    internal class SchedulerMainPageVm : BindableBase
    {
        #region Fields
        private MainWindowVm mainWindowVm;
        private ObservableCollection<AnalysisScheduleRecord> analysisScheduleRecords;
        private AnalysisScheduleRecord curAnalysisScheduleRecord;
        private Type pageType;
        private string searchRequest;
        private bool isModalVisible;
        private bool isCardVisible;
        private int pageNumber = 1;
        #endregion

        #region Properties

        public ObservableCollection<AnalysisScheduleRecord> AnalysisScheduleRecords
        {
            get => analysisScheduleRecords;
            set
            {
                if (value == analysisScheduleRecords) return;
                {
                    analysisScheduleRecords = value;
                    RaisePropertyChanged(nameof(AnalysisScheduleRecords));
                }
            }
        }

        public AnalysisScheduleRecord CurAnalysisScheduleRecord
        {
            get => curAnalysisScheduleRecord;
            set
            {
                if (value == curAnalysisScheduleRecord) return;
                {
                    curAnalysisScheduleRecord = value;
                    RaisePropertyChanged(nameof(CurAnalysisScheduleRecord));
                }
            }
        }

        public string SearchRequest
        {
            get => searchRequest;
            set
            {
                if (value == searchRequest) return;
                {
                    searchRequest = value;
                    RaisePropertyChanged(nameof(SearchRequest));
                    RefreshCommand.Execute();
                }
            }
        }

        public bool IsModalVisible
        {
            get => isModalVisible;
            set
            {
                if (value == isModalVisible) return;
                {
                    isModalVisible = value;
                    RaisePropertyChanged(nameof(IsModalVisible));
                }
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

        public int PageNumber
        {
            get => pageNumber;
            set
            {
                if (pageNumber == 1 && value < pageNumber)
                {
                    return;
                }

                if (value == pageNumber)
                {
                    return;
                }

                {
                    pageNumber = value;
                    RaisePropertyChanged(nameof(PageNumber));
                }
            }
        }
        #endregion

        #region Commands
        public DelegateCommand AddNewCommand { get; set; }

        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand<IList> DeleteCommand { get; set; }
        public DelegateCommand<object> SwitchPageCommand { get; set; }
        public DelegateCommand CloseModalCommand { get; set; }
        public DelegateCommand<AnalysisScheduleRecord> GetInfoCommand { get; set; }
        #endregion

        public SchedulerMainPageVm(MainWindowVm mainWindow)
        {
            mainWindowVm = mainWindow;
            SchedulerModel = new SchedulerModel(
                App.ServiceProvider,
                App.ServiceProvider.GetRequiredService<ScheduleServiceClient>(),
                App.ServiceProvider.GetRequiredService<IReadOnlyServiceClient<Network.NetworkClient, Host>>(),
                App.ServiceProvider.GetRequiredService<IEntityServiceClient<BenchmarkClient, Benchmark>>());

            AnalysisScheduleRecords = new ObservableCollection<AnalysisScheduleRecord>();
            pageType = typeof(SchedulerMainPageVm);
            CurAnalysisScheduleRecord = new AnalysisScheduleRecord();
            GetInfoCommand = new DelegateCommand<AnalysisScheduleRecord>(ShowCurAnalysisScheduleRecord);
            AddNewCommand = new DelegateCommand(AddNewEntity);
            RefreshCommand = new DelegateCommand(async () => await Refresh());
            SwitchPageCommand = new DelegateCommand<object>(async n => await SwitchPage(n));
            CloseModalCommand = new DelegateCommand(CloseModal);
            DeleteCommand = new DelegateCommand<IList>(async (selectedItems) => await DeleteEntity(selectedItems));
            RefreshCommand.Execute();
        }

        private SchedulerModel SchedulerModel { get; }

        private async Task DeleteEntity(IList selectedItems)
        {
            var itemsToDelete = selectedItems.Cast<AnalysisScheduleRecord>().ToList();
            if (!itemsToDelete.Any())
            {
                await DialogHost.Show(
                    new MessageDialog(TextConstants.ErrorHeader, TextConstants.SelectionToDeleteError),
                    TextConstants.DialogIdentifier);

                return;
            }

            var isAccepted = (bool?)await DialogHost.Show(
                new MessageDialogWithResponse(
                    TextConstants.AttenstionHeader,
                    TextConstants.GetReallyWantDelete(itemsToDelete)),
                TextConstants.DialogIdentifier);

            if (!isAccepted ?? false)
            {
                return;
            }

            var queryResult = await Task.Run(() => SchedulerModel.RemoveAsync(itemsToDelete));
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            MessageDialog messageDialogSuccess;
            if (itemsToDelete.Count > 1)
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.SchedulerModelRemoveManySuccess);
            }
            else
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.SchedulerModelRemoveOneSuccess);
            }

            await DialogHost.Show(messageDialogSuccess, TextConstants.DialogIdentifier);
            RefreshCommand.Execute();
        }

        private void AddNewEntity()
        {
            mainWindowVm.MainFrame = new SchedulerAddEditPage(mainWindowVm, null);
        }

        private void ShowCurAnalysisScheduleRecord(AnalysisScheduleRecord analysisScheduleRecord)
        {
            mainWindowVm.MainFrame = new SchedulerAddEditPage(mainWindowVm, analysisScheduleRecord);
        }

        protected void ShowModal()
        {
            IsModalVisible = true;
        }

        protected void CloseModal()
        {
            IsModalVisible = false;
            IsCardVisible = false;
        }

        protected async Task SwitchPage(object text)
        {
            switch (text)
            {
                case "right":
                    PageNumber++;
                    break;
                case "left":
                    PageNumber--;
                    break;
                default:
                    break;
            }

            await Refresh();
        }

        protected async Task Refresh()
        {
            var pageModel = new PageModel
            {
                PageNumber = pageNumber,
                SearchPhrase = searchRequest,
            };

            var queryResult = await Task.Run(() => SchedulerModel.GetAsync(pageModel));
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, "Root");

                return;
            }

            var queryResultData = queryResult.Data;
            // foreach (var record in queryResultData)
            // {
            //     record.NextAnalysisDateTime = record.Recurrence.GetNextAnalysisDateTime();
            // }

            AnalysisScheduleRecords.Clear();
            AnalysisScheduleRecords.AddRange(queryResultData);
        }

        // static List<string> GetRandomElementsFromDictionary(Dictionary<int, string> dictionary, int count)
        // {
        //     Random random = new Random();
        //     List<string> selectedElements = new List<string>();
        //
        //     // Проверка наличия достаточного количества элементов в словаре
        //     if (count > dictionary.Count)
        //     {
        //         throw new ArgumentException("Запрошенное количество элементов превышает количество элементов в словаре.");
        //     }
        //
        //     // Выбор случайных элементов из словаря
        //     List<int> keys = dictionary.Keys.ToList();
        //     for (int i = 0; i < count; i++)
        //     {
        //         int randomIndex = random.Next(keys.Count);
        //         int key = keys[randomIndex];
        //         selectedElements.Add(dictionary[key]);
        //         keys.RemoveAt(randomIndex);
        //     }
        //
        //     return selectedElements;
        // }

        // private static Dictionary<int, string> dictionary = new Dictionary<int, string>()
        // {
        //     { 1, @"Анализатор конфигураций" },
        //     { 2, @"Анализатор устройств" },
        //     { 3, @"Анализатор ПО" },
        //     { 4, @"Анализатор обновлений ПО" },
        // };
    }
}
