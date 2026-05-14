using HotelManager.Models;
using HotelManager.ViewModels.Base;
using System.Collections.ObjectModel;

namespace HotelManager.ViewModels
{
    public class ReservationViewModel : ViewModelBase
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<Guest> _guestRepository;

        public bool IsBulgarian { get; set; } = true;

        private LocalizedReservation? _selectedLocalizedReservation;
        public LocalizedReservation? SelectedLocalizedReservation
        {
            get => _selectedLocalizedReservation;
            set
            {
                SetProperty(ref _selectedLocalizedReservation, value);
                if (value != null)
                    SelectedReservation = value.Original;
            }
        }

        private string _statusKey = string.Empty;
        private string _statusArg = string.Empty;

        private ObservableCollection<Reservation> _reservations = new();
        public ObservableCollection<Reservation> Reservations
        {
            get => _reservations;
            set => SetProperty(ref _reservations, value);
        }

        private ObservableCollection<Room> _availableRooms = new();
        public ObservableCollection<Room> AvailableRooms
        {
            get => _availableRooms;
            set => SetProperty(ref _availableRooms, value);
        }

        private ObservableCollection<Guest> _guests = new();
        public ObservableCollection<Guest> Guests
        {
            get => _guests;
            set => SetProperty(ref _guests, value);
        }

        private Reservation? _selectedReservation;
        public Reservation? SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                SetProperty(ref _selectedReservation, value);
                if (value != null) LoadSelectedReservation(value);
                DeleteCommand.RaiseCanExecuteChanged();
                UpdateCommand.RaiseCanExecuteChanged();
            }
        }

        private Room? _selectedRoom;
        public Room? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                SetProperty(ref _selectedRoom, value);
                CalculateTotalPrice();
            }
        }

        private Guest? _selectedGuest;
        public Guest? SelectedGuest
        {
            get => _selectedGuest;
            set => SetProperty(ref _selectedGuest, value);
        }

        private DateTime _checkIn = DateTime.Today;
        public DateTime CheckIn
        {
            get => _checkIn;
            set
            {
                SetProperty(ref _checkIn, value);
                CalculateTotalPrice();
            }
        }

        private DateTime _checkOut = DateTime.Today.AddDays(1);
        public DateTime CheckOut
        {
            get => _checkOut;
            set
            {
                SetProperty(ref _checkOut, value);
                CalculateTotalPrice();
            }
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        private string _status = "Confirmed";
        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
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

        public ReservationViewModel(
            IRepository<Reservation> reservationRepository,
            IRepository<Room> roomRepository,
            IRepository<Guest> guestRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;

            LoadCommand = new RelayCommand(
                async () => await LoadAllAsync());
            AddCommand = new RelayCommand(
                async () => await AddReservationAsync());
            UpdateCommand = new RelayCommand(
                async () => await UpdateReservationAsync(),
                () => SelectedReservation != null);
            DeleteCommand = new RelayCommand(
                async () => await DeleteReservationAsync(),
                () => SelectedReservation != null);
            ClearCommand = new RelayCommand(ClearFields);
        }

        public void RefreshStatusLanguage()
        {
            StatusMessage = (_statusKey, IsBulgarian) switch
            {
                ("loaded", true) =>
                    $"Заредени {_statusArg} резервации.",
                ("loaded", false) =>
                    $"Loaded {_statusArg} reservations.",
                ("added", true) =>
                    "Резервацията е добавена успешно.",
                ("added", false) =>
                    "Reservation added successfully.",
                ("updated", true) =>
                    "Резервацията е обновена успешно.",
                ("updated", false) =>
                    "Reservation updated successfully.",
                ("deleted", true) =>
                    "Резервацията е изтрита успешно.",
                ("deleted", false) =>
                    "Reservation deleted successfully.",
                ("req_select", true) =>
                    "Изберете стая и гост!",
                ("req_select", false) =>
                    "Please select a room and a guest!",
                ("req_dates", true) =>
                    "Датата на излизане трябва да е след датата на влизане!",
                ("req_dates", false) =>
                    "Check-out must be after check-in!",
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

        private ObservableCollection<LocalizedReservation>
    _localizedReservations = new();
        public ObservableCollection<LocalizedReservation>
            LocalizedReservations
        {
            get => _localizedReservations;
            set => SetProperty(ref _localizedReservations, value);
        }

        private async Task LoadAllAsync()
        {
            var reservations =
                await _reservationRepository.GetAllAsync();
            Reservations =
                new ObservableCollection<Reservation>(reservations);

            var localized = reservations
                .Select(r => new LocalizedReservation(r))
                .ToList();

            LocalizedReservations =
                new ObservableCollection<LocalizedReservation>(localized);

            var rooms = await _roomRepository.GetAllAsync();
            AvailableRooms = new ObservableCollection<Room>(
                rooms.Where(r => r.IsAvailable));

            var guests = await _guestRepository.GetAllAsync();
            Guests = new ObservableCollection<Guest>(guests);

            SetStatus("loaded", Reservations.Count.ToString());

            _ = Task.Run(async () =>
            {
                foreach (var lr in localized)
                    await lr.ApplyAsync();
            });
        }

        public void RefreshLocalizedReservations()
        {
            foreach (var lr in LocalizedReservations)
                lr.ApplyStatic();

            var temp = LocalizedReservations.ToList();
            LocalizedReservations =
                new ObservableCollection<LocalizedReservation>(temp);

            _ = Task.Run(async () =>
            {
                foreach (var lr in temp)
                    await lr.ApplyAsync();
            });
        }

        private async Task AddReservationAsync()
        {
            if (SelectedRoom == null || SelectedGuest == null)
            {
                SetStatus("req_select");
                return;
            }
            if (CheckOut <= CheckIn)
            {
                SetStatus("req_dates");
                return;
            }

            var reservation = new Reservation
            {
                RoomId = SelectedRoom.Id,
                GuestId = SelectedGuest.Id,
                CheckIn = CheckIn,
                CheckOut = CheckOut,
                TotalPrice = TotalPrice,
                Status = Status
            };

            await _reservationRepository.AddAsync(reservation);
            await LoadAllAsync();
            ClearFields();
            SetStatus("added");
        }

        private async Task UpdateReservationAsync()
        {
            if (SelectedReservation == null ||
                SelectedRoom == null ||
                SelectedGuest == null) return;

            SelectedReservation.RoomId = SelectedRoom.Id;
            SelectedReservation.GuestId = SelectedGuest.Id;
            SelectedReservation.CheckIn = CheckIn;
            SelectedReservation.CheckOut = CheckOut;
            SelectedReservation.TotalPrice = TotalPrice;
            SelectedReservation.Status = Status;

            await _reservationRepository.UpdateAsync(
                SelectedReservation);
            await LoadAllAsync();
            SetStatus("updated");
        }

        private async Task DeleteReservationAsync()
        {
            if (SelectedReservation == null) return;
            try
            {
                await _reservationRepository.DeleteAsync(
                    SelectedReservation.Id, IsBulgarian);
                await LoadAllAsync();
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

        private void CalculateTotalPrice()
        {
            if (SelectedRoom == null || CheckOut <= CheckIn)
            {
                TotalPrice = 0;
                return;
            }
            int nights = (CheckOut - CheckIn).Days;
            TotalPrice = nights * SelectedRoom.PricePerNight;
        }

        private void LoadSelectedReservation(Reservation r)
        {
            CheckIn = r.CheckIn;
            CheckOut = r.CheckOut;
            TotalPrice = r.TotalPrice;
            Status = r.Status;
            SelectedRoom = AvailableRooms
                .FirstOrDefault(x => x.Id == r.RoomId);
            SelectedGuest = Guests
                .FirstOrDefault(x => x.Id == r.GuestId);
        }

        private void ClearFields()
        {
            SelectedReservation = null;
            SelectedRoom = null;
            SelectedGuest = null;
            SelectedLocalizedReservation = null;
            CheckIn = DateTime.Today;
            CheckOut = DateTime.Today.AddDays(1);
            TotalPrice = 0;
            Status = "Confirmed";
            _statusKey = string.Empty;
            _statusArg = string.Empty;
        }
    }
}