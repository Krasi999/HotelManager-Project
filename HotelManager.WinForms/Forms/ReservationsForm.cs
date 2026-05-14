using HotelManager.Data;
using HotelManager.Models;
using HotelManager.ViewModels;

namespace HotelManager.WinForms.Forms
{
    public partial class ReservationsForm : Form
    {
        private readonly ReservationViewModel _vm;

        public ReservationsForm()
        {
            InitializeComponent();
            var resRepo =
                RepositoryFactory.GetReservationRepository(Program.CurrentDatabase);
            var roomRepo =
                RepositoryFactory.GetRoomRepository(Program.CurrentDatabase);
            var guestRepo =
                RepositoryFactory.GetGuestRepository(Program.CurrentDatabase);

            _vm = new ReservationViewModel(resRepo, roomRepo, guestRepo);
            BindControls();
            LoadData();
        }

        private void BindControls()
        {
            dtpCheckIn.DataBindings.Add("Value", _vm,
                nameof(_vm.CheckIn), false,
                DataSourceUpdateMode.OnPropertyChanged);
            dtpCheckOut.DataBindings.Add("Value", _vm,
                nameof(_vm.CheckOut), false,
                DataSourceUpdateMode.OnPropertyChanged);
            lblTotalPrice.DataBindings.Add("Text", _vm,
                nameof(_vm.TotalPrice), false,
                DataSourceUpdateMode.OnPropertyChanged);
            lblStatus.DataBindings.Add("Text", _vm,
                nameof(_vm.StatusMessage), false,
                DataSourceUpdateMode.OnPropertyChanged);

            cmbStatus.Items.AddRange(new object[]
            {
                "Confirmed", "Cancelled", "Completed"
            });
            cmbStatus.DataBindings.Add("Text", _vm,
                nameof(_vm.Status), false,
                DataSourceUpdateMode.OnPropertyChanged);

            // Зареждаме стаи и гости в ComboBox
            _vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_vm.AvailableRooms))
                {
                    cmbRooms.DataSource = _vm.AvailableRooms.ToList();
                    cmbRooms.DisplayMember = nameof(Room.Number);
                }
                if (e.PropertyName == nameof(_vm.Guests))
                {
                    cmbGuests.DataSource = _vm.Guests.ToList();
                    cmbGuests.DisplayMember = nameof(Guest.FullName);
                }
            };

            cmbRooms.SelectedIndexChanged += (s, e) =>
            {
                if (cmbRooms.SelectedItem is Room room)
                    _vm.SelectedRoom = room;
            };
            cmbGuests.SelectedIndexChanged += (s, e) =>
            {
                if (cmbGuests.SelectedItem is Guest guest)
                    _vm.SelectedGuest = guest;
            };

            btnAdd.Click += async (s, e) =>
            {
                _vm.AddCommand.Execute(null);
                await Task.Delay(100);
                RefreshGrid();
            };
            btnUpdate.Click += async (s, e) =>
            {
                _vm.UpdateCommand.Execute(null);
                await Task.Delay(100);
                RefreshGrid();
            };
            btnDelete.Click += async (s, e) =>
            {
                _vm.DeleteCommand.Execute(null);
                await Task.Delay(100);
                RefreshGrid();
            };
            btnClear.Click += (s, e) =>
            {
                _vm.ClearCommand.Execute(null);
                dgvReservations.ClearSelection();
            };

            dgvReservations.SelectionChanged += (s, e) =>
            {
                if (dgvReservations.SelectedRows.Count > 0 &&
                    dgvReservations.SelectedRows[0].DataBoundItem
                        is Reservation res)
                {
                    _vm.SelectedReservation = res;
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                }
            };
        }

        private async void LoadData()
        {
            _vm.LoadCommand.Execute(null);
            await Task.Delay(300);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dgvReservations.DataSource = null;
            dgvReservations.DataSource = _vm.Reservations;

            if (dgvReservations.Columns.Contains("Id"))
                dgvReservations.Columns["Id"]!.Visible = false;
            if (dgvReservations.Columns.Contains("RoomId"))
                dgvReservations.Columns["RoomId"]!.Visible = false;
            if (dgvReservations.Columns.Contains("GuestId"))
                dgvReservations.Columns["GuestId"]!.Visible = false;
        }
    }
}