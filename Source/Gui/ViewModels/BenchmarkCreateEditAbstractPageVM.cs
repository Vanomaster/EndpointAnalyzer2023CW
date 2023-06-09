using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CleanModels.Benchmark;
using Prism.Commands;
using Prism.Mvvm;

namespace Gui.ViewModels
{
    abstract class BenchmarkCreateEditAbstractPageVm<TBenchmark> : BindableBase
    {
        #region Fields
        private bool isModalVisible;
        private bool isConfigurationCardVisible;
        private bool isSoftwareCardVisible;
        private bool isHardwareCardVisible;
        private bool isBenchmarkInfoCardVisible;
        private bool isSimpleToolbar;
        private bool isAddCsvButton;
        protected int pageNumber = 1;
        private ObservableCollection<string> subBenchmarkNameList;
        private string selectedSubBenchmarkName;
        private int numOfElements = 0;
        private Visibility borderVisibility = Visibility.Collapsed;
        protected string searchRequest;
        private string сSvPanelHeaderText;
        protected Benchmark benchmark;
        private Visibility subBenchmarkInfoCardVisibility;
        protected Visibility isVisCardActions;
        #endregion

        #region Properties
        public Visibility IsVisCardActions
        {
            get => isVisCardActions;
            set
            {
                if (value == isVisCardActions)
                {
                    return;
                }

                isVisCardActions = value;
                RaisePropertyChanged(nameof(IsVisCardActions));
            }
        }

        public Visibility SubBenchmarkCreateCardVisibility
        {
            get => subBenchmarkInfoCardVisibility;
            set
            {
                if (value == subBenchmarkInfoCardVisibility) return;

                subBenchmarkInfoCardVisibility = value;
                RaisePropertyChanged(nameof(SubBenchmarkCreateCardVisibility));
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
                }
            }
        }

        public ObservableCollection<string> SubBenchmarkNameList
        {
            get => subBenchmarkNameList;
            set
            {
                if (value == subBenchmarkNameList) return;
                {
                    subBenchmarkNameList = value;
                    RaisePropertyChanged(nameof(SubBenchmarkNameList));
                }
            }
        }

        public string SelectedSubBenchmarkName
        {
            get => selectedSubBenchmarkName;
            set
            {
                if (value == selectedSubBenchmarkName) return;
                {
                    selectedSubBenchmarkName = value;
                    RaisePropertyChanged(nameof(SelectedSubBenchmarkName));
                }
            }
        }

        public int NumOfElements
        {
            get => numOfElements;
            set
            {
                numOfElements = value;
                // if (numOfElements == 0)
                //     BorderVisibility = Visibility.Visible;
                // else BorderVisibility = Visibility.Collapsed;
            }
        }

        public Visibility BorderVisibility
        {
            get => borderVisibility;
            set
            {
                if (value == borderVisibility) return;

                borderVisibility = value;
                RaisePropertyChanged(nameof(BorderVisibility));
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

        public string CsvPanelHeaderText
        {
            get => сSvPanelHeaderText;
            set
            {
                if (value == CsvPanelHeaderText) return;
                {
                    сSvPanelHeaderText = value;
                    RaisePropertyChanged(nameof(CsvPanelHeaderText));
                }
            }
        }

        public bool IsSimpleToolbar
        {
            get => isSimpleToolbar;
            set
            {
                if (value == isSimpleToolbar) return;
                {
                    isSimpleToolbar = value;
                    RaisePropertyChanged(nameof(IsSimpleToolbar));
                }
            }
        }

        public bool IsAddCsvButton
        {
            get => isAddCsvButton;
            set
            {
                if (value == isAddCsvButton) return;
                {
                    isAddCsvButton = value;
                    if(isAddCsvButton == true)
                    {
                        CsvPanelHeaderText = "Добавление из CSV файла";
                    }
                    else
                    {
                        CsvPanelHeaderText = "Обновление из CSV файла";
                    }
                    RaisePropertyChanged(nameof(IsAddCsvButton));
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

        public bool IsConfigurationCardVisible
        {
            get => isConfigurationCardVisible;
            set
            {
                if (value == isConfigurationCardVisible) return;
                {
                    isConfigurationCardVisible = value;
                    RaisePropertyChanged(nameof(IsConfigurationCardVisible));
                }
            }
        }

        public bool IsSoftwareCardVisible
        {
            get => isSoftwareCardVisible;
            set
            {
                if (value == isSoftwareCardVisible) return;
                {
                    isSoftwareCardVisible = value;
                    RaisePropertyChanged(nameof(IsSoftwareCardVisible));
                }
            }

        }

        public bool IsHardwareCardVisible
        {
            get => isHardwareCardVisible;
            set
            {
                if (value == isHardwareCardVisible) return;
                {
                    isHardwareCardVisible = value;
                    RaisePropertyChanged(nameof(IsHardwareCardVisible));
                }
            }
        }

        public bool IsBenchmarkInfoCardVisible
        {
            get => isBenchmarkInfoCardVisible;
            set
            {
                if (value == isBenchmarkInfoCardVisible) return;
                {
                    isBenchmarkInfoCardVisible = value;
                    RaisePropertyChanged(nameof(IsBenchmarkInfoCardVisible));
                }
            }
        }


        #endregion

        #region Commands
        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand CreateSubBenchmarkCommand { get; set; }
        public DelegateCommand GetSubBenchmarkInfoCommand { get; set; }
        public DelegateCommand GetEntityInfoCommand { get; set; }
        public DelegateCommand AddNewCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand<object> SwitchPageCommand { get; set; }
        public DelegateCommand CloseModalCommand { get; set; }
        public DelegateCommand SaveEntityCommand { get; set; }
        public DelegateCommand SaveSubBenchmarkCommand { get; set; }
        public DelegateCommand DetachSubBenchmarkCommand { get; set; }
        public DelegateCommand DeleteSubBenchmarkCommand { get; set; }
        public DelegateCommand AttachSubBenchmarkCommand { get; set; }
        public DelegateCommand CopySubBenchmarkCommand { get; set; }
        public DelegateCommand CreateNewSubBenchmarkCommand { get; set; }
        public DelegateCommand CreateNewClearSubBenchmarkCommand { get; set; }
        public DelegateCommand AddNewFromCsvCommand { get; set; }
        public DelegateCommand EditFromCsvCommand { get; set; }
        public DelegateCommand CancelAddEditCsvItemsCommand { get; set; }
        public DelegateCommand ConfirmAddCsvItemsCommand { get; set; }
        public DelegateCommand ConfirmEditCsvItemsCommand { get; set; }
        //public ICommand AcceptSample4DialogCommand { get; set; }
        //public ICommand CancelSample4DialogCommand { get; set; }

        #endregion

        protected BenchmarkCreateEditAbstractPageVm(Benchmark benchmark)
        {
            this.benchmark = benchmark;
            SubBenchmarkNameList = new ObservableCollection<string>();
            RefreshCommand = new DelegateCommand(async () => await Refresh());
            GetSubBenchmarkInfoCommand = new DelegateCommand(GetSubBenchmarkInfo);
            AddNewCommand = new DelegateCommand(async () => await AddNewEntity());
            DeleteCommand = new DelegateCommand(async () => await DeleteEntities());
            SwitchPageCommand = new DelegateCommand<object>(async (p) => await SwitchPage(p));
            CloseModalCommand = new DelegateCommand(CloseModal);
            SaveEntityCommand = new DelegateCommand(async () => await SaveEntity());
            SaveSubBenchmarkCommand = new DelegateCommand(async () => await SaveSubBenchmark());
            DetachSubBenchmarkCommand = new DelegateCommand(async () => await DetachSubBenchmark());
            DeleteSubBenchmarkCommand = new DelegateCommand(async () => await DeleteSubBenchmark());
            AttachSubBenchmarkCommand = new DelegateCommand(async () => await AttachSubBenchmark());
            CopySubBenchmarkCommand = new DelegateCommand(async () => await CopySubBenchmark());
            CreateNewSubBenchmarkCommand = new DelegateCommand(async () => await CreateNewSubBenchmark());
            CreateNewClearSubBenchmarkCommand = new DelegateCommand(CreateNewClearSubBenchmark);
            AddNewFromCsvCommand = new DelegateCommand(AddNewFromCsv);
            EditFromCsvCommand = new DelegateCommand(EditFromCsv);
            CancelAddEditCsvItemsCommand = new DelegateCommand(CancelAddEditCsvItems);
            ConfirmAddCsvItemsCommand = new DelegateCommand(ConfirmAddCsvItems);
            ConfirmEditCsvItemsCommand = new DelegateCommand(ConfirmEditCsvItems);
            IsSimpleToolbar = true;
            IsAddCsvButton = true;
            NumOfElements = 0;
        }

        #region SubBenchmarkMethods
        protected abstract Task SaveSubBenchmark();

        protected abstract Task DetachSubBenchmark();

        protected abstract Task DeleteSubBenchmark();

        protected abstract Task AttachSubBenchmark();

        protected abstract Task CopySubBenchmark();

        protected abstract Task CreateNewSubBenchmark();

        private void CreateNewClearSubBenchmark()
        {
            NumOfElements = 1;
        }

        protected void GetSubBenchmarkInfo()
        {
            IsModalVisible = true;
            IsBenchmarkInfoCardVisible = true;
        }
        #endregion

        #region CSVMethods
        protected abstract void AddNewFromCsv();

        protected abstract void EditFromCsv();

        protected abstract void ConfirmAddCsvItems();

        protected abstract void ConfirmEditCsvItems();

        protected abstract void CancelAddEditCsvItems();
        #endregion

        #region ItemsMethods
        protected abstract Task DeleteEntities();

        protected abstract Task SaveEntity();

        protected abstract Task AddNewEntity();
        #endregion

        //protected abstract Task InitSubBenchmarkNames();

        protected void ShowModal()
        {
            IsModalVisible = true;
        }

        protected void CloseModal()
        {
            IsModalVisible = false;
            IsConfigurationCardVisible = false;
            IsHardwareCardVisible = false;
            IsSoftwareCardVisible = false;
            IsBenchmarkInfoCardVisible = false;
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

        protected abstract Task Refresh();
    }
}
