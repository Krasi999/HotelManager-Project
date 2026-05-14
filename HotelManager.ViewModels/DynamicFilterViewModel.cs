using HotelManager.Models;
using HotelManager.ViewModels.Base;
using HotelManager.ViewModels.Translation;
using System.Collections.ObjectModel;

namespace HotelManager.ViewModels
{
    public class FilterField : ViewModelBase
    {
        public string PropertyName { get; set; } = string.Empty;

        private string _displayName = string.Empty;
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        private string _checkBoxLabel = "Да / Не";
        public string CheckBoxLabel
        {
            get => _checkBoxLabel;
            set => SetProperty(ref _checkBoxLabel, value);
        }

        public Type PropertyType { get; set; } = typeof(string);

        private object? _value;
        public object? Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public string ControlType => PropertyType switch
        {
            Type t when t == typeof(bool) => "CheckBox",
            Type t when t == typeof(DateTime) => "DatePicker",
            Type t when t == typeof(int) => "NumberBox",
            Type t when t == typeof(decimal) => "NumberBox",
            Type t when t == typeof(double) => "NumberBox",
            _ => "TextBox"
        };
    }

    public class DynamicFilterViewModel<T> : ViewModelBase
        where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly string[] _filterableProperties;
        private List<T> _rawResults = new();

        private ObservableCollection<FilterField> _filterFields = new();
        public ObservableCollection<FilterField> FilterFields
        {
            get => _filterFields;
            set => SetProperty(ref _filterFields, value);
        }

        private ObservableCollection<object> _localizedResults = new();
        public ObservableCollection<object> LocalizedResults
        {
            get => _localizedResults;
            set => SetProperty(ref _localizedResults, value);
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public RelayCommand SearchCommand { get; }
        public RelayCommand ClearFiltersCommand { get; }

        public DynamicFilterViewModel(
            IRepository<T> repository,
            params string[] filterableProperties)
        {
            _repository = repository;
            _filterableProperties = filterableProperties;

            SearchCommand = new RelayCommand(
                async () => await SearchAsync());
            ClearFiltersCommand = new RelayCommand(ClearFilters);

            GenerateFilterFields();
        }

        private void GenerateFilterFields()
        {
            FilterFields.Clear();
            var type = typeof(T);

            foreach (var propName in _filterableProperties)
            {
                var prop = type.GetProperty(propName);
                if (prop == null || !prop.CanWrite) continue;

                FilterFields.Add(new FilterField
                {
                    PropertyName = prop.Name,
                    DisplayName = Translator.FieldName(prop.Name),
                    CheckBoxLabel = Translator.IsBulgarian
                        ? "Да / Не" : "Yes / No",
                    PropertyType = Nullable.GetUnderlyingType(
                                        prop.PropertyType)
                                    ?? prop.PropertyType,
                    Value = null
                });
            }
        }

        public void ApplyLanguage()
        {
            foreach (var field in FilterFields)
            {
                field.DisplayName = Translator.FieldName(
                    field.PropertyName);
                field.CheckBoxLabel = Translator.IsBulgarian
                    ? "Да / Не" : "Yes / No";
            }

            _ = Task.Run(async () =>
                await RebuildLocalizedResultsAsync());

            if (_rawResults.Count > 0)
                StatusMessage = Translator.IsBulgarian
                    ? $"Намерени {_rawResults.Count} резултата."
                    : $"Found {_rawResults.Count} results.";
        }

        private async Task SearchAsync()
        {
            var all = await _repository.GetAllAsync();

            _rawResults = all.Where(item =>
            {
                foreach (var field in FilterFields)
                {
                    if (field.Value == null ||
                        string.IsNullOrWhiteSpace(
                            field.Value.ToString()))
                        continue;

                    var prop = typeof(T)
                        .GetProperty(field.PropertyName);
                    if (prop == null) continue;

                    var itemValue = prop.GetValue(item);
                    if (itemValue == null) return false;

                    if (field.PropertyType == typeof(string))
                    {
                        if (!itemValue.ToString()!.Contains(
                                field.Value.ToString()!,
                                StringComparison.OrdinalIgnoreCase))
                            return false;
                    }
                    else if (field.PropertyType == typeof(bool))
                    {
                        if (field.Value is bool bVal &&
                            (bool)itemValue != bVal)
                            return false;
                    }
                    else if (field.PropertyType == typeof(decimal) ||
                             field.PropertyType == typeof(int))
                    {
                        if (decimal.TryParse(
                                field.Value.ToString(), out var nVal))
                            if (Convert.ToDecimal(itemValue) < nVal)
                                return false;
                    }
                    else if (field.PropertyType == typeof(DateTime))
                    {
                        if (field.Value is DateTime dVal &&
                            (DateTime)itemValue < dVal)
                            return false;
                    }
                }
                return true;
            }).ToList();

            await RebuildLocalizedResultsAsync();

            StatusMessage = Translator.IsBulgarian
                ? $"Намерени {_rawResults.Count} резултата."
                : $"Found {_rawResults.Count} results.";
        }

        private async Task RebuildLocalizedResultsAsync()
        {
            var newResults = new List<object>();

            foreach (var item in _rawResults)
            {
                if (item is Room room)
                {
                    var lr = new LocalizedRoom(room);
                    await lr.ApplyAsync();
                    newResults.Add(lr);
                }
                else if (item is Guest guest)
                {
                    var lg = new LocalizedGuest(guest);
                    await lg.ApplyAsync();
                    newResults.Add(lg);
                }
                else if (item is Reservation res)
                {
                    var lres = new LocalizedReservation(res);
                    await lres.ApplyAsync();
                    newResults.Add(lres);
                }
                else
                {
                    newResults.Add(item);
                }
            }
            var syncContext = SynchronizationContext.Current;
            if (syncContext != null)
            {
                syncContext.Post(_ =>
                {
                    LocalizedResults =
                        new ObservableCollection<object>(newResults);
                }, null);
            }
            else
            {
                LocalizedResults =
                    new ObservableCollection<object>(newResults);
            }
        }

        private void ClearFilters()
        {
            foreach (var field in FilterFields)
                field.Value = null;
            _rawResults.Clear();
            LocalizedResults.Clear();
            StatusMessage = string.Empty;
        }
    }
}