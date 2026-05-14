using HotelManager.Data;
using HotelManager.Models;
using HotelManager.ViewModels;

namespace HotelManager.WinForms.Forms
{
    public partial class DynamicFilterForm : Form
    {
        public DynamicFilterForm()
        {
            InitializeComponent();
            LoadRoomFilters();

            btnFilterRooms.Click += (s, e) => LoadRoomFilters();
            btnFilterGuests.Click += (s, e) => LoadGuestFilters();
        }

        private void LoadRoomFilters()
        {
            var repo = RepositoryFactory.GetRoomRepository(Program.CurrentDatabase);
            var vm = new DynamicFilterViewModel<Room>(repo,
                nameof(Room.Number),
                nameof(Room.Type),
                nameof(Room.PricePerNight),
                nameof(Room.IsAvailable),
                nameof(Room.Capacity));

            BuildFilterControls(vm);
        }

        private void LoadGuestFilters()
        {
            var repo = RepositoryFactory.GetGuestRepository(Program.CurrentDatabase);
            var vm = new DynamicFilterViewModel<Guest>(repo,
                nameof(Guest.FirstName),
                nameof(Guest.LastName),
                nameof(Guest.Email),
                nameof(Guest.Phone));

            BuildFilterControls(vm);
        }

        private void BuildFilterControls<T>(
            DynamicFilterViewModel<T> vm) where T : class
        {
            pnlFilters.Controls.Clear();

            int y = 0;
            foreach (var field in vm.FilterFields)
            {
                var lbl = new Label
                {
                    Text = field.DisplayName,
                    Location = new Point(0, y),
                    AutoSize = true,
                    ForeColor = Color.FromArgb(127, 140, 141),
                    Font = new Font("Segoe UI", 8.5f)
                };
                pnlFilters.Controls.Add(lbl);
                y += 20;

                Control ctrl;
                switch (field.ControlType)
                {
                    case "CheckBox":
                        var chk = new CheckBox
                        {
                            Location = new Point(0, y),
                            AutoSize = true,
                            Font = new Font("Segoe UI", 9.5f)
                        };
                        chk.CheckedChanged += (s, e) =>
                            field.Value = chk.Checked;
                        ctrl = chk;
                        break;

                    case "DatePicker":
                        var dtp = new DateTimePicker
                        {
                            Location = new Point(0, y),
                            Size = new Size(240, 28),
                            Font = new Font("Segoe UI", 9.5f),
                            Format = DateTimePickerFormat.Short
                        };
                        dtp.ValueChanged += (s, e) =>
                            field.Value = dtp.Value;
                        ctrl = dtp;
                        break;

                    default:
                        var txt = new TextBox
                        {
                            Location = new Point(0, y),
                            Size = new Size(240, 28),
                            Font = new Font("Segoe UI", 9.5f)
                        };
                        txt.TextChanged += (s, e) =>
                            field.Value = txt.Text;
                        ctrl = txt;
                        break;
                }

                pnlFilters.Controls.Add(ctrl);
                y += 36;
            }

            // Бутони Търси / Изчисти
            var btnSearch = new Button();
            RoomsForm.StyleButtonPublic(btnSearch, "🔍  Търси",
                Color.FromArgb(52, 152, 219));
            btnSearch.Location = new Point(0, y + 8);
            btnSearch.Click += async (s, e) =>
            {
                vm.SearchCommand.Execute(null);
                await Task.Delay(200);
                dgvResults.DataSource = vm.SearchResults.ToList();
                lblResultStatus.Text = vm.StatusMessage;
            };
            pnlFilters.Controls.Add(btnSearch);

            var btnClear = new Button();
            RoomsForm.StyleButtonPublic(btnClear, "🔄  Изчисти",
                Color.FromArgb(127, 140, 141));
            btnClear.Location = new Point(130, y + 8);
            btnClear.Click += (s, e) =>
            {
                vm.ClearFiltersCommand.Execute(null);
                BuildFilterControls(vm);
                dgvResults.DataSource = null;
                lblResultStatus.Text = string.Empty;
            };
            pnlFilters.Controls.Add(btnClear);
        }
    }
}