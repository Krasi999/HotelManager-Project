using HotelManager.Data;
using HotelManager.Models;
using HotelManager.ViewModels;

namespace HotelManager.WinForms.Forms
{
    public partial class RoomsForm : Form
    {
        private readonly RoomViewModel _vm;

        public RoomsForm()
        {
            InitializeComponent();
            var repo = RepositoryFactory.GetRoomRepository(Program.CurrentDatabase);
            _vm = new RoomViewModel(repo);

            // Binding
            dgvRooms.DataSource = new
                System.ComponentModel.BindingSource();

            BindControls();
            LoadData();
        }

        internal static void StyleButtonPublic(Button btnAdd, string v, Color color)
        {
            throw new NotImplementedException();
        }

        private void BindControls()
        {
            txtNumber.DataBindings.Add("Text", _vm,
                nameof(_vm.Number), false,
                DataSourceUpdateMode.OnPropertyChanged);
            txtPrice.DataBindings.Add("Text", _vm,
                nameof(_vm.PricePerNight), false,
                DataSourceUpdateMode.OnPropertyChanged);
            txtCapacity.DataBindings.Add("Text", _vm,
                nameof(_vm.Capacity), false,
                DataSourceUpdateMode.OnPropertyChanged);
            txtDescription.DataBindings.Add("Text", _vm,
                nameof(_vm.Description), false,
                DataSourceUpdateMode.OnPropertyChanged);
            chkAvailable.DataBindings.Add("Checked", _vm,
                nameof(_vm.IsAvailable), false,
                DataSourceUpdateMode.OnPropertyChanged);
            lblStatus.DataBindings.Add("Text", _vm,
                nameof(_vm.StatusMessage), false,
                DataSourceUpdateMode.OnPropertyChanged);

            cmbType.Items.AddRange(new object[]
            {
                "Single", "Double", "Suite", "Apartment"
            });
            cmbType.DataBindings.Add("Text", _vm,
                nameof(_vm.Type), false,
                DataSourceUpdateMode.OnPropertyChanged);

            // Бутони
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
                dgvRooms.ClearSelection();
            };

            // При избор на ред
            dgvRooms.SelectionChanged += (s, e) =>
            {
                if (dgvRooms.SelectedRows.Count > 0 &&
                    dgvRooms.SelectedRows[0].DataBoundItem is Room room)
                {
                    _vm.SelectedRoom = room;
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                }
            };
        }

        private async void LoadData()
        {
            _vm.LoadCommand.Execute(null);
            await Task.Delay(200);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dgvRooms.DataSource = null;
            dgvRooms.DataSource = _vm.Rooms;

            // Скриваме Id колоната
            if (dgvRooms.Columns.Contains("Id"))
                dgvRooms.Columns["Id"]!.Visible = false;
        }
    }
}