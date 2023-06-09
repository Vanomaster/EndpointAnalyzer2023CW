using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CleanModels;
using CleanModels.Analysis;
using Client.Clients.Base;
using Gui.Common;
using Gui.Models;
using Gui.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Mvvm;
using AnalysisResultModel = CleanModels.Analysis.AnalysisResult;

namespace Gui.ViewModels
{
    public class ResultsMainPageVm : BindableBase
    {
        #region Fields
        private MainWindowVm mainVm;
        private int pageNumber = 1;
        private Type pageType;
        private ObservableCollection<AnalysisResult> analysisResults;
        private string searchRequest;
        #endregion

        #region Properties
        // public SnackbarMessageQueue MessageQueue { get; set; }

        public string SearchRequest
        {
            get => searchRequest;
            set
            {
                if (value == searchRequest)
                {
                    return;
                }

                {
                    searchRequest = value;
                    RaisePropertyChanged(nameof(SearchRequest));
                    RefreshCommand.Execute();
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

        public ObservableCollection<AnalysisResult> AnalysisResults
        {
            get => analysisResults;
            set
            {
                if (value == analysisResults)
                {
                    return;
                }

                {
                    analysisResults = value;
                    RaisePropertyChanged(nameof(AnalysisResults));
                }
            }
        }
        #endregion

        #region Commands
        public DelegateCommand RefreshCommand { get; }

        // public DelegateCommand<DataGridSortingEventArgs> DataGridSorting { get; set; }
        public DelegateCommand<object> SwitchPageCommand { get; }

        public DelegateCommand<AnalysisResult> GetInfoCommand { get; }

        #endregion

        public ResultsMainPageVm(MainWindowVm mainWindowVm)
        {
            AnalysisResultsModel = new AnalysisResultsModel(
                App.ServiceProvider,
                App.ServiceProvider.GetRequiredService<IReadOnlyServiceClient<AnalysisResultService.AnalysisResult.AnalysisResultClient, PageModel, AnalysisResultModel>>());

            pageType = typeof(ResultsMainPageVm);
            mainVm = mainWindowVm;
            RefreshCommand = new DelegateCommand(async () => await Refresh());
            SwitchPageCommand = new DelegateCommand<object>(async n => await SwitchPage(n));
            // DataGridSorting = new DelegateCommand<DataGridSortingEventArgs>(DataGrid_Sorting);
            GetInfoCommand = new DelegateCommand<AnalysisResult>(GetInfo);
            AnalysisResults = new ObservableCollection<AnalysisResult>();
            RefreshCommand.Execute();
        }

        private AnalysisResultsModel AnalysisResultsModel { get; }

        private void GetInfo(AnalysisResult selectedAnalysisResult)
        {
            // var a = selectedAnalysisResult.AnalyzerName;
            // _mainVM.MainFrame = AnalyzerProvider.GetPageByName(selectedAnalysisResult.AnalyzerName);
            switch (selectedAnalysisResult.AnalyzerName)
            {
                case "Анализ конфигураций":
                    mainVm.MainFrame = new ResultConfigurationDetailPage(selectedAnalysisResult);
                    break;
                case "Анализ устройств":
                    mainVm.MainFrame = new ResultHardwareDetailPage(selectedAnalysisResult);
                    break;
                case "Анализ ПО":
                    mainVm.MainFrame = new ResultSoftwareDetailPage(selectedAnalysisResult);
                    break;
                case "Анализ обновлений ПО":
                    mainVm.MainFrame = new ResultUpgradableSoftwarePage(selectedAnalysisResult);
                    break;
                default:
                    break;
            }
        }

        private async Task SwitchPage(object param)
        {
            switch (param)
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

            var queryResult = await Task.Run(() => AnalysisResultsModel.GetAsync(pageModel));
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, "Root");

                return;
            }

            await Task.Run(() =>
            {
                foreach (var result in queryResult.Data)
                {
                    result.IsOK = result.Text.Length == 4;
                }

                var list = new ObservableCollection<AnalysisResult>();
                list.AddRange(queryResult.Data);
                Application.Current.Dispatcher.Invoke(() => AnalysisResults = list);
            });
            //AnalysisResults = list;
            // AnalysisResults.Clear();
            // foreach (var result in queryResult.Data)
            // {
            //     AnalysisResults.Add(result);
            //     await Task.Delay(TimeSpan.FromMicroseconds(30));
            //     RaisePropertyChanged(nameof(AnalysisResults));
            // }
        }

        // private void DataGrid_Sorting(DataGridSortingEventArgs e)
        // {
        //     // TODO переделать алгоритм
        //     if (!e.Column.SortDirection.HasValue)
        //     {
        //         e.Column.SortDirection = ListSortDirection.Ascending;
        //     }
        //     if (e.Column.SortDirection.HasValue && e.Column.SortDirection.Value == ListSortDirection.Ascending)
        //     {
        //         e.Column.SortDirection = ListSortDirection.Descending;
        //     }
        //
        //     var a3 = e.Column.SortMemberPath;
        //
        //     e.Handled = true;
        // }
    }
}