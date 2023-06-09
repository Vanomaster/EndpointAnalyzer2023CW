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
using Client.Services.Base;
using Client.Services.Mappers.Csv;
using Gui.Common;
using Gui.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using MessageDialog = Gui.Common.MessageDialog;
using MessageDialogWithResponse = Gui.Common.MessageDialogWithResponse;

namespace Gui.ViewModels
{
    internal class BenchmarkCreateEditConfigurationPageVm : BenchmarkCreateEditAbstractPageVm<ConfigurationRecommendation>
    {
        #region Fields
        protected MainWindowVm MainWindowVm;
        private ConfigurationRecommendationsBenchmark curSubBenchmark;
        private ConfigurationRecommendation curRecommendation;
        private ObservableCollection<ConfigurationRecommendation> newItemsFromCsv;
        private ObservableCollection<ConfigurationRecommendation> itemsToDisplay;

        private DataGrid DataGrid { get; }
        #endregion

        #region Properties
        public ObservableCollection<ConfigurationRecommendation> ItemsToDisplay
        {
            get => itemsToDisplay;
            set
            {
                if (value == itemsToDisplay)
                {
                    return;
                }

                {
                    itemsToDisplay = value;
                    RaisePropertyChanged(nameof(ItemsToDisplay));
                }
            }
        }

        public ConfigurationRecommendationsBenchmark CurSubBenchmark
        {
            get => curSubBenchmark;
            set
            {
                if (value == curSubBenchmark)
                {
                    return;
                }

                {
                    curSubBenchmark = value;
                    if (curSubBenchmark.ConfigurationRecommendations == null || !curSubBenchmark.ConfigurationRecommendations.Any())
                    {
                        NumOfElements = 0;
                    }

                    RaisePropertyChanged(nameof(CurSubBenchmark));
                }
            }
        }

        public ConfigurationRecommendation CurConfigurationRecommendation
        {
            get => curRecommendation;
            set
            {
                if (value == curRecommendation) return;
                {
                    curRecommendation = value;
                    RaisePropertyChanged(nameof(CurConfigurationRecommendation));
                }

            }
        }
        #endregion

        #region Commands
        public DelegateCommand<ConfigurationRecommendation> GetInfoCommand { get; set; }
        #endregion

        public BenchmarkCreateEditConfigurationPageVm(Benchmark? benchmark, DataGrid dataGrid)
            : base(benchmark)
        {
            ConfigurationRecommendationsBenchmarksModel = new ConfigurationRecommendationsBenchmarksModel(
                App.ServiceProvider.GetRequiredService<IParser<ConfigurationRecommendation, ConfigurationRecommendationMapper>>(),
                App.ServiceProvider.GetRequiredService<ConfigurationRecommendationsBenchmarkServiceClient>(),
                App.ServiceProvider.GetRequiredService<ConfigurationRecommendationsServiceClient>());

            ItemsToDisplay = new ObservableCollection<ConfigurationRecommendation>();
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
                CurSubBenchmark = new ConfigurationRecommendationsBenchmark
                {
                    Name = @"Новый шаблон анализа конфигураций",
                    ParentId = benchmark.Id,
                };
            }

            curRecommendation = new ConfigurationRecommendation();
            GetInfoCommand = new DelegateCommand<ConfigurationRecommendation>(EditCurConfigurationRecommendation);
        }

        private ConfigurationRecommendationsBenchmarksModel ConfigurationRecommendationsBenchmarksModel { get; }

        // protected override async Task InitSubBenchmarkNames()
        // {
        //     var queryResult = await Task.Run(() => ConfigurationRecommendationsBenchmarksModel.GetAllNamesAsync());
        //     if (!queryResult.IsSuccessful)
        //     {
        //         var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
        //         await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);
        //
        //         return;
        //     }
        //
        //     SubBenchmarkNameList.AddRange(queryResult.Data);
        // }

        private async Task InitBenchmark()
        {
            var queryResult = await Task.Run(() => ConfigurationRecommendationsBenchmarksModel.GetAsync(benchmark.Name));
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

            var queryResult = await Task.Run(() => ConfigurationRecommendationsBenchmarksModel.GetAsync(pageModel));
            if (!queryResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, queryResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            CurSubBenchmark.ConfigurationRecommendations = queryResult.Data;
            ItemsToDisplay.Clear();
            if (CurSubBenchmark.ConfigurationRecommendations != null)
            {
                ItemsToDisplay.AddRange(CurSubBenchmark.ConfigurationRecommendations);
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

            var queryResult = await Task.Run(() => ConfigurationRecommendationsBenchmarksModel.AddAsync(CurSubBenchmark));
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
            string errorString = Validator.ValidateString(CurSubBenchmark.Name, "Название");
            if (!string.IsNullOrEmpty(errorString))
            {
                await DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, errorString), TextConstants.DialogIdentifier);

                return;
            }

            CommandResult commandResult;
            if (CurSubBenchmark.Id == default)
            {
                commandResult = await Task.Run(() =>
                    ConfigurationRecommendationsBenchmarksModel.AddAsync(CurSubBenchmark));
            }
            else
            {
                commandResult = await Task.Run(() =>
                    ConfigurationRecommendationsBenchmarksModel.UpdateAsync(CurSubBenchmark));
            }

            if (!commandResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, commandResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            MessageDialog messageDialogSuccess;
            if (CurSubBenchmark.Id == default)
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.BenchmarksModelAddSuccess);
            }
            else
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.BenchmarksModelUpdateSuccess);
            }

            await DialogHost.Show(messageDialogSuccess, TextConstants.DialogIdentifier);
            SubBenchmarkCreateCardVisibility = Visibility.Collapsed;
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
                DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, TextConstants.BenchmarkSelectionError), TextConstants.DialogIdentifier);
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
                DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, TextConstants.BenchmarkSelectionError), TextConstants.DialogIdentifier);
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
                IsAddCsvButton = true;
                //ItemsToDisplay = GenerateTestData();
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
                //ItemsToDisplay = GenerateTestData();
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
            DialogHost.Show(new MessageDialog(TextConstants.InfoHeader, "Элементы, которые удалось доавить или нет"), TextConstants.DialogIdentifier);
            return;
        }

        protected override void ConfirmEditCsvItems()
        {
            //Тут логика когда юзер согласился обновить элементы из csv
            DialogHost.Show(new MessageDialog(TextConstants.InfoHeader, "Элементы, которые удалось обновить  или нет"), TextConstants.DialogIdentifier);
            return;
        }

        protected override void CancelAddEditCsvItems()
        {
            IsSimpleToolbar = true;
            //закрузка что было в шаблоне
            ItemsToDisplay.Clear();
            ItemsToDisplay.AddRange(CurSubBenchmark.ConfigurationRecommendations);
        }
        #endregion

        #region ItemsMethods
        protected override async Task DeleteEntities()
        {
            var itemsToDelete = DataGrid.SelectedItems.Cast<ConfigurationRecommendation>().ToList();
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

            var queryResult = await Task.Run(() => ConfigurationRecommendationsBenchmarksModel.RemoveAsync(itemsToDelete));
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
            CurConfigurationRecommendation = new ConfigurationRecommendation
            {
                Configuration = new Configuration(),
            };
            IsVisCardActions = Visibility.Visible;
            IsModalVisible = true;
            IsConfigurationCardVisible = true;
        }

        protected override async Task SaveEntity()
        {
            var stringsToValidate = new List<Tuple<string, string>>
            {
                new (CurConfigurationRecommendation.Configuration.Name, "Название конфигурации"),
                new (CurConfigurationRecommendation.Name, "Название рекомендации"),
                new (CurConfigurationRecommendation.VerificationCommand, "Команда"),
                new (CurConfigurationRecommendation.VerificationResult, "Результат"),
            };

            string errorString = Validator.ValidateStrings(stringsToValidate);
            if (errorString.Length > 0)
            {
                await DialogHost.Show(new MessageDialog(TextConstants.ErrorHeader, errorString), TextConstants.DialogIdentifier);

                return;
            }

            CommandResult commandResult;
            CurConfigurationRecommendation.Configuration = new Configuration
            {
                Name = CurConfigurationRecommendation.Name,
            };
            CurConfigurationRecommendation.ParentId = CurSubBenchmark.Id;
            if (CurConfigurationRecommendation.Id == default)
            {
                commandResult = await Task.Run(() =>
                    ConfigurationRecommendationsBenchmarksModel.AddAsync(new []{CurConfigurationRecommendation}));
            }
            else
            {
                commandResult = await Task.Run(() =>
                    ConfigurationRecommendationsBenchmarksModel.UpdateAsync(new []{CurConfigurationRecommendation}));
            }

            if (!commandResult.IsSuccessful)
            {
                var messageDialog = new MessageDialog(TextConstants.ErrorHeader, commandResult.ErrorMessage);
                await DialogHost.Show(messageDialog, TextConstants.DialogIdentifier);

                return;
            }

            MessageDialog messageDialogSuccess;
            if (CurConfigurationRecommendation.Id == default)
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.ItemAddSuccess);
            }
            else
            {
                messageDialogSuccess = new MessageDialog(TextConstants.InfoHeader, TextConstants.ItemUpdateSuccess);
            }

            await DialogHost.Show(messageDialogSuccess, TextConstants.DialogIdentifier);
            IsModalVisible = false;
            IsConfigurationCardVisible = false;
            RefreshCommand.Execute();
        }

        private void EditCurConfigurationRecommendation(ConfigurationRecommendation recommendation)
        {
            CurConfigurationRecommendation = recommendation;
            IsVisCardActions = Visibility.Hidden;
            IsModalVisible = true;
            IsConfigurationCardVisible = true;
        }
        #endregion
    }
}
