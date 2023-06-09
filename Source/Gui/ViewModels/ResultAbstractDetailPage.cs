using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CleanModels;
using CleanModels.Analysis;
using Client.Clients.Base;
using Common.Extensions;
using Gui.Models;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Prism.Mvvm;

namespace Gui.ViewModels
{
    abstract class ResultAbstractDetailPage<TAnalysisResultData> : BindableBase
    {
        #region Fields
        private bool isModalVisible;
        private bool isCardVisible;
        private int pageNumber = 1;
        private string header;
        private string searchRequest = string.Empty;
        private AnalysisResult AnalysisResult;
        private ObservableCollection<TAnalysisResultData> results;
        private TAnalysisResultData result;
        #endregion

        #region Properties

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

                pageNumber = value;
                RaisePropertyChanged(nameof(PageNumber));
            }
        }

        public string Header
        {
            get => header;
            set
            {
                if (value == header)
                {
                    return;
                }

                {
                    header = value;
                    RaisePropertyChanged(nameof(Header));
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

        public ObservableCollection<TAnalysisResultData> Results
        {
            get => results;
            set
            {
                if (value == results)
                {
                    return;
                }

                {
                    results = value;
                    RaisePropertyChanged(nameof(Results));
                }
            }
        }

        public TAnalysisResultData Result
        {
            get => result;
            set
            {
                result = value;
                RaisePropertyChanged(nameof(Result));
            }
        }
        #endregion

        #region Comands
        public DelegateCommand RefreshCommand { get; set; }

        public DelegateCommand<object> SwitchPageCommand { get; set; }

        public DelegateCommand CloseModalCommand { get; set; }

        public DelegateCommand<TAnalysisResultData> GetInfoCommand { get; set; }
        #endregion

        protected ResultAbstractDetailPage(AnalysisResult analysisResult)
        {
            AnalysisResult = analysisResult;
            AnalysisResultsModel = new AnalysisResultsModel(App.ServiceProvider, App.ServiceProvider.GetRequiredService<IReadOnlyServiceClient<AnalysisResultService.AnalysisResult.AnalysisResultClient, PageModel, AnalysisResult>>());
            RefreshCommand = new DelegateCommand(async () => await Refresh());
            SwitchPageCommand = new DelegateCommand<object>(async (text) => await SwitchPage(text));
            CloseModalCommand = new DelegateCommand(CloseModal);
            GetInfoCommand = new DelegateCommand<TAnalysisResultData>(ShowCurRecommendation);
            Header = $"{analysisResult.PcName} / {analysisResult.BenchmarkName} / {analysisResult.DateTime}";
            Results = new ObservableCollection<TAnalysisResultData>();
            RefreshCommand.Execute();
        }

        private AnalysisResultsModel AnalysisResultsModel { get; }

        protected abstract string GetEntityTextToSearch(TAnalysisResultData entity);

        private async Task Refresh()
        {
            const string Separator = " ";
            const byte PageSize = 10;
            var analysisResultData = AnalysisResult.Text.ToObject<List<TAnalysisResultData>>() ?? new List<TAnalysisResultData>();
            if (searchRequest is not null)
            {
                string[] searchWords = searchRequest.Split(Separator);
                analysisResultData = analysisResultData.Where(entity =>
                    searchWords.All(word => GetEntityTextToSearch(entity).ToLower().Contains(word.ToLower()))).ToList();
            }

            int entitiesCountToSkip = (pageNumber - 1) * PageSize;
            analysisResultData = analysisResultData
                .Skip(entitiesCountToSkip)
                .Take(PageSize)
                .ToList();

            Results.Clear();
            Results.AddRange(analysisResultData);
        }

        private async Task SwitchPage(object text)
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

        private void CloseModal()
        {
            IsModalVisible = false;
            IsCardVisible = false;
        }

        private void ShowCurRecommendation(TAnalysisResultData recommendation)
        {
            Result = recommendation;
            IsModalVisible = true;
            IsCardVisible = true;
        }
    }
}
