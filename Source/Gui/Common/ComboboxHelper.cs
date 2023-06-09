using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;

namespace Gui.Common
{
    public class ComboboxHelper : BindableBase
    {
        #region Fields
        private ObservableCollection<string> _items;
        private ObservableCollection<string> _sortedItems;
        private string _selectedItem;
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
        public ObservableCollection<string> Items
        {
            get => _items;
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }
        public ObservableCollection<string> SortedItems
        {
            get => _sortedItems;
            set
            {
                _sortedItems = value;
                RaisePropertyChanged(nameof(SortedItems));
            }
        }

        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion

        #region Commands

        #endregion
        public ComboboxHelper(ObservableCollection<string> items)
        {
            Items = items;
            SortedItems = new ObservableCollection<string>();
            TextChanged();
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
                SortedItems.AddRange(Items.Where(x => x.ToLower().Contains(SearchText.ToLower().Trim())));
            }
        }
    }
}
