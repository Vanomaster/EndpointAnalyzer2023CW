using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;

namespace Gui.Common
{
    public class ComboboxWithCheckBoxHelper : BindableBase
    {
        #region Fields
        private ObservableCollection<ItemViewModel> _items;
        private ObservableCollection<ItemViewModel> _sortedItems;
        private ObservableCollection<ItemViewModel> _selectedItems;
        private ItemViewModel _selectedItem;
        private DelegateCommand<ItemViewModel> _toggleItemCommand;
        private ObservableCollection<string> _selectedItemsStringType;
        private string _searchText = "";
        #endregion

        #region Properties
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                RaisePropertyChanged(nameof(SearchText));
                TextChanged();
            }
        }

        public ObservableCollection<ItemViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }

        public ObservableCollection<ItemViewModel> SortedItems
        {
            get => _sortedItems;
            set
            {
                _sortedItems = value;
                RaisePropertyChanged(nameof(SortedItems));
            }
        }

        public ObservableCollection<ItemViewModel> SelectedItems
        {
            get => _selectedItems;
            set
            {
                _selectedItems = value;
                RaisePropertyChanged(nameof(SelectedItems));
            }
        }

        public ObservableCollection<string> SelectedItemsStringType
        {
            get => _selectedItemsStringType;
            set
            {
                _selectedItemsStringType = value;
                RaisePropertyChanged(nameof(SelectedItemsStringType));
            }
        }

        public ItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
                ToggleSelectedItem();
            }
        }
        #endregion

        #region Commands
        public DelegateCommand<ItemViewModel> ToggleItemCommand =>
           _toggleItemCommand ?? (_toggleItemCommand = new DelegateCommand<ItemViewModel>(ToggleItem));
        public DelegateCommand SelectionChangedCommand;
        #endregion

        public ComboboxWithCheckBoxHelper(ObservableCollection<string> items)
        {
            Items = ConvertIntoItem(items);
            SortedItems = new ObservableCollection<ItemViewModel>();
            TextChanged();
            SelectedItems = new ObservableCollection<ItemViewModel>();
            SelectedItemsStringType = new ObservableCollection<string>();
        }
        private ObservableCollection<ItemViewModel> ConvertIntoItem(ObservableCollection<string> input)
        {
            ObservableCollection<ItemViewModel> output = new();
            foreach (var item in input)
            {
                output.Add(new ItemViewModel { Name = item });
            }
            return output;
        }

        public void ToggleItem(ItemViewModel item)
        {
            if (item.IsChecked)
            {
                SelectedItems.Add(item);
                SelectedItemsStringType.Add(item.Name);
            }
            else
            {
                SelectedItems.Remove(item);
                SelectedItemsStringType.Remove(item.Name);
            }
        }

        public void ToggleSelectedItem()
        {
            if (SelectedItem != null)
            {
                SelectedItem.IsChecked = !SelectedItem.IsChecked;
            }
        }

        private void TextChanged()
        {
            if (string.IsNullOrEmpty(SearchText.Trim()))
            {
                SortedItems.Clear();
                SortedItems.AddRange(Items);
                return;
            }
            else if (SearchText.Trim().Length > 0)
            {
                SortedItems.Clear();
                SortedItems.AddRange(Items.Where(x => x.Name.ToLower().Contains(SearchText.ToLower().Trim())));
            }
        }
    }
    public class ItemViewModel : BindableBase
    {
        private bool _isChecked;
        private bool _isVisible;
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                RaisePropertyChanged(nameof(IsChecked));
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                RaisePropertyChanged(nameof(IsVisible));
            }
        }
    }
}
