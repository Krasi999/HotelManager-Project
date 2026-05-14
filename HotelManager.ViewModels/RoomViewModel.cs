using HotelManager.Models;
using HotelManager.ViewModels.Base;
using System.Collections.ObjectModel;

namespace HotelManager.ViewModels
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly IRepository<Room> _repository;

        public bool IsBulgarian { get; set; } = true;

        private LocalizedRoom? _selectedLocalizedRoom;
        public LocalizedRoom? SelectedLocalizedRoom
        {
            get => _selectedLocalizedRoom;
            set
            {
                SetProperty(ref _selectedLocalizedRoom, value);

                if (value != null)
                    SelectedRoom = value.Original;
            }
        }

        private string _statusKey = string.Empty;
        private string _statusArg = string.Empty;

        private ObservableCollection<Room> _rooms = new();
        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set => SetProperty(ref _rooms, value);
        }

        private Room? _selectedRoom;
        public Room? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                SetProperty(ref _selectedRoom, value);
                if (value != null) LoadSelectedRoom(value);
                DeleteCommand.RaiseCanExecuteChanged();
                UpdateCommand.RaiseCanExecuteChanged();
            }
        }

        private string _number = string.Empty;
        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        private string _type = "Single";
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private decimal _pricePerNight;
        public decimal PricePerNight
        {
            get => _pricePerNight;
            set => SetProperty(ref _pricePerNight, value);
        }

        private int _capacity = 1;
        public int Capacity
        {
            get => _capacity;
            set => SetProperty(ref _capacity, value);
        }

        private bool _isAvailable = true;
        public bool IsAvailable
        {
            get => _isAvailable;
            set => SetProperty(ref _isAvailable, value);
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
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


        public RoomViewModel(IRepository<Room> repository)
        {
            _repository = repository;

            LoadCommand = new RelayCommand(
                async () => await LoadRoomsAsync());
            AddCommand = new RelayCommand(
                async () => await AddRoomAsync());
            UpdateCommand = new RelayCommand(
                async () => await UpdateRoomAsync(),
                () => SelectedRoom != null);
            DeleteCommand = new RelayCommand(
                async () => await DeleteRoomAsync(),
                () => SelectedRoom != null);
            ClearCommand = new RelayCommand(ClearFields);
        }

        public void RefreshStatusLanguage()
        {
            StatusMessage = (_statusKey, IsBulgarian) switch
            {
                ("loaded", true) =>
                    $"Заредени {_statusArg} стаи.",
                ("loaded", false) =>
                    $"Loaded {_statusArg} rooms.",
                ("added", true) =>
                    $"Стая {_statusArg} е добавена успешно.",
                ("added", false) =>
                    $"Room {_statusArg} added successfully.",
                ("updated", true) =>
                    $"Стая {_statusArg} е обновена успешно.",
                ("updated", false) =>
                    $"Room {_statusArg} updated successfully.",
                ("deleted", true) =>
                    "Стаята е изтрита успешно.",
                ("deleted", false) =>
                    "Room deleted successfully.",
                ("req_number", true) =>
                    "Номерът на стаята е задължителен!",
                ("req_number", false) =>
                    "Room number is required!",
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

        private System.Collections.ObjectModel.ObservableCollection<LocalizedRoom>
            _localizedRooms = new();
        public System.Collections.ObjectModel.ObservableCollection<LocalizedRoom>
            LocalizedRooms
        {
            get => _localizedRooms;
            set => SetProperty(ref _localizedRooms, value);
        }

        private async Task LoadRoomsAsync()
        {
            var rooms = await _repository.GetAllAsync();
            Rooms = new ObservableCollection<Room>(rooms);

            var localized = rooms
                .Select(r => new LocalizedRoom(r))
                .ToList();

            LocalizedRooms = new ObservableCollection<LocalizedRoom>(
                localized);

            SetStatus("loaded", Rooms.Count.ToString());

            _ = Task.Run(async () =>
            {
                foreach (var lr in localized)
                {
                    await lr.ApplyAsync();
                }
            });
        }

        public void RefreshLocalizedRooms()
        {
            foreach (var lr in LocalizedRooms)
                lr.ApplyStatic();

            var temp = LocalizedRooms.ToList();
            LocalizedRooms = new ObservableCollection<LocalizedRoom>(temp);

            _ = Task.Run(async () =>
            {
                foreach (var lr in temp)
                    await lr.ApplyAsync();
            });
        }

        private async Task AddRoomAsync()
        {
            if (string.IsNullOrWhiteSpace(Number))
            {
                SetStatus("req_number");
                return;
            }

            var room = new Room
            {
                Number = Number,
                Type = Type,
                PricePerNight = PricePerNight,
                Capacity = Capacity,
                IsAvailable = IsAvailable,
                Description = Description
            };

            await _repository.AddAsync(room);
            await LoadRoomsAsync();
            var addedNumber = Number;
            ClearFields();
            SetStatus("added", addedNumber);
        }

        private async Task UpdateRoomAsync()
        {
            if (SelectedRoom == null) return;

            SelectedRoom.Number = Number;
            SelectedRoom.Type = Type;
            SelectedRoom.PricePerNight = PricePerNight;
            SelectedRoom.Capacity = Capacity;
            SelectedRoom.IsAvailable = IsAvailable;
            SelectedRoom.Description = Description;

            await _repository.UpdateAsync(SelectedRoom);
            await LoadRoomsAsync();
            SetStatus("updated", Number);
        }

        private async Task DeleteRoomAsync()
        {
            if (SelectedRoom == null) return;
            try
            {
                await _repository.DeleteAsync(
                    SelectedRoom.Id, IsBulgarian);
                await LoadRoomsAsync();
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

        private void LoadSelectedRoom(Room room)
        {
            Number = room.Number;
            Type = room.Type;
            PricePerNight = room.PricePerNight;
            Capacity = room.Capacity;
            IsAvailable = room.IsAvailable;
            Description = room.Description;
        }

        private void ClearFields()
        {
            Number = string.Empty;
            Type = "Single";
            PricePerNight = 0;
            Capacity = 1;
            IsAvailable = true;
            Description = string.Empty;
            SelectedRoom = null;
            SelectedLocalizedRoom = null;
            _statusKey = string.Empty;
            _statusArg = string.Empty;
        }
    }
}