using HotelManager.ViewModels.Base;
using HotelManager.ViewModels.Translation;
using System.Collections.ObjectModel;

namespace HotelManager.ViewModels
{
    public class GridColumnDefinition
    {
        public string PropertyName { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public int Width { get; set; } = 120;
    }

    public class DynamicRow
    {
        public object OriginalObject { get; set; } = null!;
        public Dictionary<string, string> Values { get; set; }
            = new();
    }

    public class DynamicListViewModel<T> : ViewModelBase
        where T : class
    {
        private List<T> _rawItems = new();
        private List<GridColumnDefinition> _colDefs = new();

        private ObservableCollection<GridColumnDefinition>
            _columns = new();
        public ObservableCollection<GridColumnDefinition> Columns
        {
            get => _columns;
            set => SetProperty(ref _columns, value);
        }

        private ObservableCollection<DynamicRow> _rows = new();
        public ObservableCollection<DynamicRow> Rows
        {
            get => _rows;
            set => SetProperty(ref _rows, value);
        }

        private DynamicRow? _selectedRow;
        public DynamicRow? SelectedRow
        {
            get => _selectedRow;
            set
            {
                SetProperty(ref _selectedRow, value);
                SelectedObject = value?.OriginalObject as T;
            }
        }

        private T? _selectedObject;
        public T? SelectedObject
        {
            get => _selectedObject;
            set => SetProperty(ref _selectedObject, value);
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public async Task LoadDataAsync(
            IEnumerable<T> items,
            IEnumerable<GridColumnDefinition> columns)
        {
            _rawItems = items.ToList();
            _colDefs = columns.ToList();
            await RebuildRowsAsync();
        }

        public async Task RefreshLanguageAsync()
        {
            foreach (var col in _colDefs)
                col.Header = Translator.FieldName(
                    col.PropertyName);

            await RebuildRowsAsync();
        }

        public void Clear()
        {
            _rawItems.Clear();
            _colDefs.Clear();
            Columns.Clear();
            Rows.Clear();
            SelectedRow = null;
            SelectedObject = null;
            StatusMessage = string.Empty;
        }

        private async Task RebuildRowsAsync()
        {
            Columns.Clear();
            Rows.Clear();
            SelectedRow = null;
            SelectedObject = null;

            foreach (var col in _colDefs)
                Columns.Add(new GridColumnDefinition
                {
                    PropertyName = col.PropertyName,
                    Header = col.Header,
                    Width = col.Width
                });

            var type = typeof(T);
            foreach (var item in _rawItems)
            {
                var row = new DynamicRow
                { OriginalObject = item };

                foreach (var col in _colDefs)
                {
                    var prop = type.GetProperty(
                        col.PropertyName);
                    var val = prop?.GetValue(item);

                    row.Values[col.PropertyName] =
                        Translator.Value(val, col.PropertyName);
                }

                Rows.Add(row);
            }

            StatusMessage = Translator.IsBulgarian
                ? $"Показани {Rows.Count} записа."
                : $"Showing {Rows.Count} records.";

            await TranslateRowsAsync();
        }

        private async Task TranslateRowsAsync()
        {
            var skipFields = new HashSet<string>
            {
                "Type", "Status", "IsAvailable",
                "PricePerNight", "TotalPrice",
                "CheckIn", "CheckOut", "DateOfBirth",
                "Nights", "Id", "Number",
                "Capacity", "Email", "Phone", "EGN"
            };

            var type = typeof(T);

            foreach (var row in Rows.ToList())
            {
                foreach (var col in _colDefs)
                {
                    if (skipFields.Contains(col.PropertyName))
                        continue;

                    var prop = type.GetProperty(
                        col.PropertyName);
                    var val = prop?.GetValue(
                        row.OriginalObject);

                    if (val == null) continue;

                    string original = val.ToString()!;
                    if (string.IsNullOrWhiteSpace(original))
                        continue;

                    string translated = await Translator
                        .TranslateAsync(original,
                            Translator.IsBulgarian);

                    row.Values[col.PropertyName] = translated;
                }
            }

            var temp = Rows.ToList();
            Rows = new ObservableCollection<DynamicRow>(temp);
        }
    }
}