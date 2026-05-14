using HotelManager.Models;
using HotelManager.ViewModels.Base;
using System.Collections.ObjectModel;

namespace HotelManager.ViewModels
{
    public class GuestViewModel : ViewModelBase
    {
        private readonly IRepository<Guest> _repository;

        public bool IsBulgarian { get; set; } = true;

        private LocalizedGuest? _selectedLocalizedGuest;
        public LocalizedGuest? SelectedLocalizedGuest
        {
            get => _selectedLocalizedGuest;
            set
            {
                SetProperty(ref _selectedLocalizedGuest, value);
                if (value != null)
                    SelectedGuest = value.Original;
            }
        }

        private string _statusKey = string.Empty;
        private string _statusArg = string.Empty;

        private ObservableCollection<Guest> _guests = new();
        public ObservableCollection<Guest> Guests
        {
            get => _guests;
            set => SetProperty(ref _guests, value);
        }

        private Guest? _selectedGuest;
        public Guest? SelectedGuest
        {
            get => _selectedGuest;
            set
            {
                SetProperty(ref _selectedGuest, value);
                if (value != null) LoadSelectedGuest(value);
                DeleteCommand.RaiseCanExecuteChanged();
                UpdateCommand.RaiseCanExecuteChanged();
            }
        }

        private string _firstName = string.Empty;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName = string.Empty;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _phone = string.Empty;
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        private string _egn = string.Empty;
        public string EGN
        {
            get => _egn;
            set => SetProperty(ref _egn, value);
        }

        private DateTime _dateOfBirth = DateTime.Today.AddYears(-30);
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => SetProperty(ref _dateOfBirth, value);
        }

        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public RelayCommand LoadCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ClearCommand { get; }

        public GuestViewModel(IRepository<Guest> repository)
        {
            _repository = repository;

            LoadCommand = new RelayCommand(
                async () => await LoadGuestsAsync());
            AddCommand = new RelayCommand(
                async () => await AddGuestAsync());
            UpdateCommand = new RelayCommand(
                async () => await UpdateGuestAsync(),
                () => SelectedGuest != null);
            DeleteCommand = new RelayCommand(
                async () => await DeleteGuestAsync(),
                () => SelectedGuest != null);
            ClearCommand = new RelayCommand(ClearFields);
        }


        public void RefreshStatusLanguage()
        {
            StatusMessage = (_statusKey, IsBulgarian) switch
            {
                ("loaded", true) =>
                    $"Заредени {_statusArg} гости.",
                ("loaded", false) =>
                    $"Loaded {_statusArg} guests.",
                ("added", true) =>
                    $"Гост {_statusArg} е добавен успешно.",
                ("added", false) =>
                    $"Guest {_statusArg} added successfully.",
                ("updated", true) =>
                    $"Гост {_statusArg} е обновен успешно.",
                ("updated", false) =>
                    $"Guest {_statusArg} updated successfully.",
                ("deleted", true) =>
                    "Гостът е изтрит успешно.",
                ("deleted", false) =>
                    "Guest deleted successfully.",
                ("req_name", true) =>
                    "Името и фамилията са задължителни!",
                ("req_name", false) =>
                    "First and last name are required!",
                ("err_delete", true) =>
                    "Възникна грешка при изтриването.",
                ("err_delete", false) =>
                    "An error occurred while deleting.",
                _ => StatusMessage
            };
        }

        private void SetStatus(string key, string arg = "")
        {
            _statusKey = key;
            _statusArg = arg;
            RefreshStatusLanguage();
        }

        private ObservableCollection<LocalizedGuest> _localizedGuests = new();
        public ObservableCollection<LocalizedGuest> LocalizedGuests
        {
            get => _localizedGuests;
            set => SetProperty(ref _localizedGuests, value);
        }

        private async Task LoadGuestsAsync()
        {
            var guests = await _repository.GetAllAsync();
            Guests = new ObservableCollection<Guest>(guests);

            var localized = guests
                .Select(g => new LocalizedGuest(g))
                .ToList();

            LocalizedGuests = new ObservableCollection<LocalizedGuest>(
                localized);

            SetStatus("loaded", Guests.Count.ToString());

            _ = Task.Run(async () =>
            {
                foreach (var lg in localized)
                    await lg.ApplyAsync();
            });
        }

        public void RefreshLocalizedGuests()
        {
            foreach (var lg in LocalizedGuests)
                lg.ApplyStatic();

            var temp = LocalizedGuests.ToList();
            LocalizedGuests =
                new ObservableCollection<LocalizedGuest>(temp);

            _ = Task.Run(async () =>
            {
                foreach (var lg in temp)
                    await lg.ApplyAsync();
            });
        }

        private async Task AddGuestAsync()
        {
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName))
            {
                SetStatus("req_name");
                return;
            }

            var guest = new Guest
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = Phone,
                EGN = EGN,
                DateOfBirth = DateOfBirth
            };

            await _repository.AddAsync(guest);
            await LoadGuestsAsync();
            var fullName = $"{FirstName} {LastName}";
            ClearFields();
            SetStatus("added", fullName);
        }

        private async Task UpdateGuestAsync()
        {
            if (SelectedGuest == null) return;

            SelectedGuest.FirstName = FirstName;
            SelectedGuest.LastName = LastName;
            SelectedGuest.Email = Email;
            SelectedGuest.Phone = Phone;
            SelectedGuest.EGN = EGN;
            SelectedGuest.DateOfBirth = DateOfBirth;

            await _repository.UpdateAsync(SelectedGuest);
            await LoadGuestsAsync();
            SetStatus("updated", $"{FirstName} {LastName}");
        }

        private async Task DeleteGuestAsync()
        {
            if (SelectedGuest == null) return;
            try
            {
                await _repository.DeleteAsync(
                    SelectedGuest.Id, IsBulgarian);
                await LoadGuestsAsync();
                ClearFields();
                SetStatus("deleted");
            }
            catch (InvalidOperationException ex)
            {
                _statusKey = "fk_error";
                StatusMessage = ex.Message;
            }
            catch (Exception)
            {
                SetStatus("err_delete");
            }
        }

        private void LoadSelectedGuest(Guest guest)
        {
            FirstName = guest.FirstName;
            LastName = guest.LastName;
            Email = guest.Email;
            Phone = guest.Phone;
            EGN = guest.EGN;
            DateOfBirth = guest.DateOfBirth;
        }

        private void ClearFields()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            EGN = string.Empty;
            DateOfBirth = DateTime.Today.AddYears(-30);
            SelectedGuest = null;
            SelectedLocalizedGuest = null;
            _statusKey = string.Empty;
            _statusArg = string.Empty;
        }
    }
}