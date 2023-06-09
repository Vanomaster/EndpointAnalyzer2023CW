using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CleanModels;
using CleanModels.Benchmark;
using CleanModels.Commands.Base;
using Client.Clients;
using Gui.Common;
using Gui.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;

namespace Gui.ViewModels
{
    internal class BenchmarkCreateEditSoftwarePageVm : BenchmarkCreateEditAbstractPageVm<TrustedSoftware>
    {
        #region Fields
        protected MainWindowVm MainWindowVm;
        private TrustedSoftwareBenchmark curSubBenchmark;
        private TrustedSoftware curRecommendation;
        private ObservableCollection<TrustedSoftware> newItemsFromCsv;
        private ObservableCollection<TrustedSoftware> itemsToDisplay;

        private DataGrid DataGrid { get; }
        #endregion

        #region Properties
        public ObservableCollection<TrustedSoftware> ItemsToDisplay
        {
            get => itemsToDisplay;
            set
            {
                if (value == itemsToDisplay) return;
                {
                    itemsToDisplay = value;
                    RaisePropertyChanged(nameof(ItemsToDisplay));
                }

            }
        }

        public TrustedSoftwareBenchmark CurSubBenchmark
        {
            get => curSubBenchmark;
            set
            {
                if (value == curSubBenchmark) return;
                {
                    curSubBenchmark = value;
                    if (curSubBenchmark.TrustedSoftware == null || curSubBenchmark.TrustedSoftware.Count() == 0)
                    {
                        NumOfElements = 0;
                    }
                    RaisePropertyChanged(nameof(CurSubBenchmark));
                }
            }
        }

        public TrustedSoftware CurRecommendation
        {
            get => curRecommendation;
            set
            {
                if (value == curRecommendation) return;
                {
                    curRecommendation = value;
                    RaisePropertyChanged(nameof(CurRecommendation));
                }
            }
        }
        #endregion

        #region Commands
        public DelegateCommand<TrustedSoftware> GetInfoCommand { get; set; }
        #endregion

        public BenchmarkCreateEditSoftwarePageVm(Benchmark? benchmark, DataGrid dataGrid)
            : base(benchmark)
        {
            TrustSoftBenchmarksModel = new TrustSoftBenchmarksModel(
                App.ServiceProvider.GetRequiredService<TrustSoftBenchmarkServiceClient>(),
                App.ServiceProvider.GetRequiredService<TrustSoftServiceClient>());

            ItemsToDisplay = new ObservableCollection<TrustedSoftware>();
            DataGrid = dataGrid;
            if (benchmark != null)
            {
                Task.Run(InitBenchmark).Wait();
            }

            if (CurSubBenchmark != null)
            {
                CurSubBenchmark.ParentId = benchmark.Id;
                SubBenchmarkCreateCardVisibility = Visibility.Collapsed;
                RefreshCommand.Execute();
            }
            else
            {
                SubBenchmarkCreateCardVisibility = Visibility.Visible;
                CurSubBenchmark = new TrustedSoftwareBenchmark
                {
                    Name = @"Новый шаблон анализа ПО",
                    ParentId = benchmark.Id,
                };
            }

            curRecommendation = new TrustedSoftware();
            GetInfoCommand = new DelegateCommand<TrustedSoftware>(EditCurRecommendation);
        }

        private TrustSoftBenchmarksModel TrustSoftBenchmarksModel { get; }

        private async Task InitBenchmark()
        {
            var queryResult = await Task.Run(() => TrustSoftBenchmarksModel.GetAsync(benchmark.Name));
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            CurSubBenchmark = queryResult.Data;
        }

        protected override async Task Refresh()
        {
            var pageModel = new PageModel
            {
                PageNumber = pageNumber,
                SearchPhrase = searchRequest,
                ParentName = CurSubBenchmark.Name,
            };

            var queryResult = await Task.Run(() => TrustSoftBenchmarksModel.GetAsync(pageModel));
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            CurSubBenchmark.TrustedSoftware = queryResult.Data;
            ItemsToDisplay.Clear();
            if (CurSubBenchmark.TrustedSoftware != null)
            {
                ItemsToDisplay.AddRange(CurSubBenchmark.TrustedSoftware);
            }
        }

        #region SubBenchmarkMethods
        protected override async Task CreateNewSubBenchmark()
        {
            string errorString = Validator.ValidateString(CurSubBenchmark.Name, "Название");
            if (!string.IsNullOrEmpty(errorString))
            {
                await DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, errorString), TextConstants.DialogIdentifier);

                return;
            }

            var queryResult = await Task.Run(() => TrustSoftBenchmarksModel.AddAsync(CurSubBenchmark));
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            var messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.BenchmarksModelAddSuccess);
            await DialogHost.Show(messageDialogSuccess, TextConstants.DialogIdentifier);
            SubBenchmarkCreateCardVisibility = Visibility.Collapsed;
        }

        protected override async Task SaveSubBenchmark()
        {
            // string errorString = Validator.ValidateString(CurSubBenchmark.Name, "Название");
            // if (!string.IsNullOrEmpty(errorString))
            // {
            //     await DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, errorString), TextConstants.DialogIdentifier);
            //
            //     return;
            // }
            //
            // CommandResult commandResult;
            // if (CurSubBenchmark.Id == default)
            // {
            //     commandResult = await Task.Run(() =>
            //         TrustSoftBenchmarksModel.AddAsync(CurSubBenchmark));
            // }
            // else
            // {
            //     commandResult = await Task.Run(() =>
            //         TrustSoftBenchmarksModel.UpdateAsync(CurSubBenchmark));
            // }
            //
            // if (!commandResult.IsSuccessful)
            // {
            //     var messageDialog = new MessageDialog(TextConstants.ErrorHeader, commandResult.ErrorMessage);
            //     await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);
            //
            //     return;
            // }
            //
            // MessageDialog messageDialogSuccess;
            // if (CurSubBenchmark.Id == default)
            // {
            //     messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.BenchmarksModelAddSuccess);
            // }
            // else
            // {
            //     messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.BenchmarksModelUpdateSuccess);
            // }
            //
            // await DialogHost.Show(messageDialogSuccess, TextConstants.DialogIdentifier);
            // SubBenchmarkCreateCardVisibility = Visibility.Collapsed;
        }

        protected override async Task DetachSubBenchmark()
        {
            //логика для открепления шаблона
            //если не удалось
            //DialogHost.Show(new SampleMessageDialog(Helpers.Constants.ErrorHeader, Helpers.Constants.BenchmarksModelDetachError), "BenchmarkCreateEditPageInfoCardDialog");
            //return;
            ////если удалось
            //DialogHost.Show(new SampleMessageDialog(Helpers.Constants.ErrorHeader, Helpers.Constants.BenchmarksModelDetachAccess), "BenchmarkCreateEditPageInfoCardDialog");
        }

        protected override async Task DeleteSubBenchmark()
        {
            //логика для удаления шаблона
            //если не удалось
            //DialogHost.Show(new SampleMessageDialog(Helpers.Constants.ErrorHeader, Helpers.Constants.BenchmarksModelRemoveError), "BenchmarkCreateEditPageInfoCardDialog");
            //return;
            ////если удалось
            //DialogHost.Show(new SampleMessageDialog(Helpers.Constants.ErrorHeader, Helpers.Constants.BenchmarksModelRemoveAccess), "BenchmarkCreateEditPageInfoCardDialog");
        }

        protected override async Task AttachSubBenchmark()
        {
            if (SelectedSubBenchmarkName == null)
            {
                DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, TextConstants.BenchmarkSelectionError), "Root");
                return;
            }
            else
            {
                //логика для прикрепления
                //если не удалось
                //DialogHost.Show(new SampleMessageDialog(Helpers.Constants.ErrorHeader, Helpers.Constants.BenchmarksModelAttachError), "BenchmarkCreateEditPageInfoCardDialog");
                //return;
                ////если удалось
                //DialogHost.Show(new SampleMessageDialog(Helpers.Constants.ErrorHeader, Helpers.Constants.BenchmarksModelAttachAccess), "BenchmarkCreateEditPageInfoCardDialog");
            }
        }

        protected override async Task CopySubBenchmark()
        {
            if (SelectedSubBenchmarkName == null)
            {
                DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, TextConstants.BenchmarkSelectionError), "Root");
                return;
            }
            else
            {
                //логика для копирования
                //если не удалось
                //DialogHost.Show(new SampleMessageDialog(Helpers.Constants.ErrorHeader, Helpers.Constants.BenchmarksModelCopyError), "BenchmarkCreateEditPageInfoCardDialog");
                //return;
                ////если удалось
                //DialogHost.Show(new SampleMessageDialog(Helpers.Constants.ErrorHeader, Helpers.Constants.BenchmarksModelCopyAccess), "BenchmarkCreateEditPageInfoCardDialog");
            }
        }
        #endregion

        #region CSVMethods
        protected override void AddNewFromCsv()
        {
            //логика если всё ок
            {
                IsSimpleToolbar = false;
                //ItemsToDisplay = GenerateTestData(7);
            }
            //если не удалось добавить
            //{
            //    DialogHost.Show(new SampleMessageDialog(Helpers.Constants.ErrorHeader, Helpers.Constants.BenchmarksModelGetFromFileError ), "BenchmarkCreateEditPageInfoCardDialog");
            //    return;
            //}
        }

        protected override void EditFromCsv()
        {
            //логика если всё ок
            {
                IsSimpleToolbar = false;
                IsAddCsvButton = false;
                //ItemsToDisplay = GenerateTestData(11);
            }
            //если не удалось добавить
            //{
            //    DialogHost.Show(new SampleMessageDialog(Helpers.Constants.ErrorHeader, Helpers.Constants.BenchmarksModelGetFromFileError ), "BenchmarkCreateEditPageInfoCardDialog");
            //    return;
            //}
        }

        protected override void ConfirmAddCsvItems()
        {
            //Тут логика когда юзер согласился добавить элементы из csv
            DialogHost.Show(new MessageDialog(TextConstants.InfoHeader, "Элементы, которые удалось доавить или нет"), "BenchmarkCreateEditPageInfoCardDialog");
            return;
        }

        protected override void ConfirmEditCsvItems()
        {
            //Тут логика когда юзер согласился обновить элементы из csv
            DialogHost.Show(new MessageDialog(TextConstants.InfoHeader, "Элементы, которые удалось обновить  или нет"), "BenchmarkCreateEditPageInfoCardDialog");
            return;
        }

        protected override void CancelAddEditCsvItems()
        {
            IsSimpleToolbar = true;
            ItemsToDisplay.Clear();
            ItemsToDisplay.AddRange(CurSubBenchmark.TrustedSoftware);
        }
        #endregion

        #region ItemsMethods
        private void EditCurRecommendation(TrustedSoftware recommendation)
        {
            CurRecommendation = recommendation;
            IsVisCardActions = Visibility.Hidden;
            IsModalVisible = true;
            IsSoftwareCardVisible = true;
        }

        protected override async Task DeleteEntities()
        {
            var itemsToDelete = DataGrid.SelectedItems.Cast<TrustedSoftware>().ToList();
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

            var queryResult = await Task.Run(() => TrustSoftBenchmarksModel.RemoveAsync(itemsToDelete));
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            MessageDialog messageDialogSuccess;
            if (itemsToDelete.Count > 1)
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.ItemsDeleteSuccess);
            }
            else
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.ItemDeleteSuccess);
            }

            await DialogHost.Show(messageDialogSuccess, TextConstants.DialogIdentifier);
            RefreshCommand.Execute();
        }

        protected override async Task AddNewEntity()
        {
            CurRecommendation = new TrustedSoftware();
            IsVisCardActions = Visibility.Visible;
            IsModalVisible = true;
            IsSoftwareCardVisible = true;
        }

        protected override async Task SaveEntity()
        {
            var stringsToValidate = new List<Tuple<string, string>>
            {
                new (CurRecommendation.Name, "Название"),
                new (CurRecommendation.Version, "Версия"),
            };

            string errorString = Validator.ValidateStrings(stringsToValidate);
            if (errorString.Length > 0)
            {
                await DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, errorString), TextConstants.DialogIdentifier);

                return;
            }

            CommandResult commandResult;
            CurRecommendation.ParentId = CurSubBenchmark.Id;
            commandResult = await Task.Run(() => TrustSoftBenchmarksModel.AddAsync(new []{CurRecommendation}));
            // if (CurRecommendation.Id == default)
            // {
            //     commandResult = await Task.Run(() =>
            //         TrustSoftBenchmarksModel.AddAsync(new []{CurRecommendation}));
            // }
            // else
            // {
            //     commandResult = await Task.Run(() =>
            //         TrustSoftBenchmarksModel.UpdateAsync(new []{CurRecommendation}));
            // }

            if (!commandResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, commandResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            MessageDialog messageDialogSuccess;
            if (CurRecommendation.Id == default)
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.ItemAddSuccess);
            }
            else
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.ItemUpdateSuccess);
            }

            await DialogHost.Show(messageDialogSuccess, TextConstants.DialogIdentifier);
            IsModalVisible = false;
            IsSoftwareCardVisible = false;
            RefreshCommand.Execute();
        }
        #endregion
    }
}
