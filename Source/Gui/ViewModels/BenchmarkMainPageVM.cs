using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CleanModels;
using CleanModels.Benchmark;
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
    internal class BenchmarkMainPageVm : BindableBase
    {
        #region Fields
        private readonly MainWindowVm mainVm;
        private int pageNumber = 1;
        private string searchPhrase;
        private ObservableCollection<Benchmark> benchmarks;

        #endregion

        #region Commands
        public DelegateCommand AddNewCommand { get; set; }

        public DelegateCommand RefreshCommand { get; set; }

        public DelegateCommand<IList> DeleteCommand { get; set; }

        public DelegateCommand<object> SwitchPageCommand { get; set; }

        public DelegateCommand<Benchmark> GetInfoCommand { get; set; }
        #endregion

        #region Properties

        public string SearchRequest
        {
            get => searchPhrase;

            set
            {
                if (value == searchPhrase)
                {
                    return;
                }

                {
                    searchPhrase = value;
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

        public ObservableCollection<Benchmark> Benchmarks
        {
            get => benchmarks;
            set
            {
                if (value == benchmarks)
                {
                    return;
                }

                {
                    benchmarks = value;
                    RaisePropertyChanged(nameof(Benchmarks));
                }
            }
        }
        #endregion

        public BenchmarkMainPageVm(MainWindowVm mainWindowVm)
        {
            mainVm = mainWindowVm;
            BenchmarksModel = new BenchmarksModel(App.ServiceProvider, App.ServiceProvider.GetRequiredService<IEntityServiceClient<BenchmarkService.Benchmark.BenchmarkClient, Benchmark>>());
            GetInfoCommand = new DelegateCommand<Benchmark>(EditCurBenchmark);
            AddNewCommand = new DelegateCommand(AddNewEntity);
            DeleteCommand = new DelegateCommand<IList>(async (items) => await DeleteEntity(items));
            SwitchPageCommand = new DelegateCommand<object>(async p => await SwitchPage(p));
            RefreshCommand = new DelegateCommand(async () => await Refresh());
            Benchmarks = new ObservableCollection<Benchmark>();
            RefreshCommand.Execute();
        }

        private BenchmarksModel BenchmarksModel { get; }

        private void EditCurBenchmark(Benchmark selectedbenchmark)
        {
            mainVm.MainFrame = new BenchmarkCreateEditPage(mainVm, selectedbenchmark);
        }

        private void AddNewEntity()
        {
            mainVm.MainFrame = new BenchmarkCreateEditPage(mainVm, null);
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

        private async Task DeleteEntity(IList selectedItems)
        {
            var itemsToDelete = selectedItems.Cast<Benchmark>().ToList();
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

            var queryResult = await Task.Run(() => BenchmarksModel.RemoveAsync(itemsToDelete));
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            MessageDialog messageDialogSuccess;
            if (itemsToDelete.Count > 1)
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.BenchmarksModelRemoveManySuccess);
            }
            else
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.BenchmarksModelRemoveOneSuccess);
            }

            await DialogHost.Show(messageDialogSuccess, TextConstants.DialogIdentifier);
            RefreshCommand.Execute();
        }

        protected async Task Refresh()
        {
            var pageModel = new PageModel
            {
                PageNumber = pageNumber,
                SearchPhrase = searchPhrase,
            };

            var queryResult = await Task.Run(() => BenchmarksModel.GetAsync(pageModel));
            if (!queryResult.IsSuccessful)
            {
                // TODO extract to MessageDialogHost singleton class, no create new dialog
                await DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage), "Root");

                return;
            }

            Benchmarks.Clear();
            Benchmarks.AddRange(queryResult.Data);
        }
    }
}
