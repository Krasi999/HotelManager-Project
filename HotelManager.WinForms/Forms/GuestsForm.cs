using HotelManager.Data;
using HotelManager.Models;
using HotelManager.ViewModels;

namespace HotelManager.WinForms.Forms
{
    public partial class GuestsForm : Form
    {
        private readonly GuestViewModel _vm;

        public GuestsForm()
        {
            InitializeComponent();
            var repo = RepositoryFactory.GetGuestRepository(Program.CurrentDatabase);
            _vm = new GuestViewModel(repo);
            BindControls();
            LoadData();
        }

        private void BindControls()
        {
            txtFirstName.DataBindings.Add("Text", _vm,
                nameof(_vm.FirstName), false,
                DataSourceUpdateMode.OnPropertyChanged);
            txtLastName.DataBindings.Add("Text", _vm,
                nameof(_vm.LastName), false,
                DataSourceUpdateMode.OnPropertyChanged);
            txtEmail.DataBindings.Add("Text", _vm,
                nameof(_vm.Email), false,
                DataSourceUpdateMode.OnPropertyChanged);
            txtPhone.DataBindings.Add("Text", _vm,
                nameof(_vm.Phone), false,
                DataSourceUpdateMode.OnPropertyChanged);
            txtEGN.DataBindings.Add("Text", _vm,
                nameof(_vm.EGN), false,
                DataSourceUpdateMode.OnPropertyChanged);
            dtpBirth.DataBindings.Add("Value", _vm,
                nameof(_vm.DateOfBirth), false,
                DataSourceUpdateMode.OnPropertyChanged);
            lblStatus.DataBindings.Add("Text", _vm,
                nameof(_vm.StatusMessage), false,
                DataSourceUpdateMode.OnPropertyChanged);

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
                dgvGuests.ClearSelection();
            };

            dgvGuests.SelectionChanged += (s, e) =>
            {
                if (dgvGuests.SelectedRows.Count > 0 &&
                    dgvGuests.SelectedRows[0].DataBoundItem is Guest guest)
                {
                    _vm.SelectedGuest = guest;
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
            dgvGuests.DataSource = null;
            dgvGuests.DataSource = _vm.Guests;

            if (dgvGuests.Columns.Contains("Id"))
                dgvGuests.Columns["Id"]!.Visible = false;
        }
    }
}