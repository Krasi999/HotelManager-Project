using HotelManager.Data;
using HotelManager.Models;
using HotelManager.ViewModels;

namespace HotelManager.WinForms.Forms
{
    public partial class DynamicListForm : Form
    {
        public DynamicListForm()
        {
            InitializeComponent();

            btnRoomsBasic.Click += async (s, e) => await LoadRoomsBasic();
            btnRoomsAll.Click += async (s, e) => await LoadRoomsAll();
            btnReservations.Click += async (s, e) => await LoadReservations();
            btnClear.Click += (s, e) => ClearGrid();
        }

        private async Task LoadRoomsBasic()
        {
            var repo = RepositoryFactory.GetRoomRepository(Program.CurrentDatabase);
            var vm = new DynamicListViewModel<Room>();
            vm.LoadData(await repo.GetAllAsync(), new[]
            {
                new ColumnDefinition
                    { PropertyName = nameof(Room.Number), Header = "Номер" },
                new ColumnDefinition
                    { PropertyName = nameof(Room.Type),   Header = "Тип" },
                new ColumnDefinition
                    { PropertyName = nameof(Room.PricePerNight), Header = "Цена/нощ" }
            });
            BuildGrid(vm);
        }

        private async Task LoadRoomsAll()
        {
            var repo = RepositoryFactory.GetRoomRepository(Program.CurrentDatabase);
            var vm = new DynamicListViewModel<Room>();
            vm.LoadData(await repo.GetAllAsync(), new[]
            {
                new ColumnDefinition
                    { PropertyName = nameof(Room.Number),        Header = "Номер" },
                new ColumnDefinition
                    { PropertyName = nameof(Room.Type),          Header = "Тип" },
                new ColumnDefinition
                    { PropertyName = nameof(Room.PricePerNight), Header = "Цена/нощ" },
                new ColumnDefinition
                    { PropertyName = nameof(Room.Capacity),      Header = "Капацитет" },
                new ColumnDefinition
                    { PropertyName = nameof(Room.IsAvailable),   Header = "Свободна" },
                new ColumnDefinition
                    { PropertyName = nameof(Room.Description),   Header = "Описание" }
            });
            BuildGrid(vm);
        }

        private async Task LoadReservations()
        {
            var repo = RepositoryFactory
                .GetReservationRepository(Program.CurrentDatabase);
            var vm = new DynamicListViewModel<Reservation>();
            vm.LoadData(await repo.GetAllAsync(), new[]
            {
                new ColumnDefinition
                    { PropertyName = nameof(Reservation.Id),
                      Header = "ID" },
                new ColumnDefinition
                    { PropertyName = nameof(Reservation.CheckIn),
                      Header = "Настаняване" },
                new ColumnDefinition
                    { PropertyName = nameof(Reservation.CheckOut),
                      Header = "Напускане" },
                new ColumnDefinition
                    { PropertyName = nameof(Reservation.TotalPrice),
                      Header = "Цена" },
                new ColumnDefinition
                    { PropertyName = nameof(Reservation.Status),
                      Header = "Статус" }
            });
            BuildGrid(vm);
        }

        private void BuildGrid<T>(DynamicListViewModel<T> vm) where T : class
        {
            dgvDynamic.Columns.Clear();
            dgvDynamic.Rows.Clear();
            dgvDynamic.AutoGenerateColumns = false;

            foreach (var col in vm.Columns)
            {
                dgvDynamic.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = col.PropertyName,
                    HeaderText = col.Header,
                    FillWeight = 1
                });
            }

            foreach (var row in vm.Rows)
            {
                var values = vm.Columns
                    .Select(c => row.Values.TryGetValue(
                        c.PropertyName, out var v) ? v : "")
                    .ToArray<object>();

                var dgvRow = new DataGridViewRow();
                dgvRow.Tag = row;
                dgvDynamic.Rows.Add(values);
                dgvDynamic.Rows[dgvDynamic.Rows.Count - 1].Tag = row;
            }

            lblStatus.Text = vm.StatusMessage;
        }

        private void ClearGrid()
        {
            dgvDynamic.Columns.Clear();
            dgvDynamic.Rows.Clear();
            lblStatus.Text = string.Empty;
            lblSelected.Text = "Избран обект: —";
        }

        private void dgvDynamic_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDynamic.SelectedRows.Count > 0)
            {
                var row = dgvDynamic.SelectedRows[0].Tag as DynamicRow;
                lblSelected.Text = "Избран обект: " +
                    (row?.OriginalObject?.ToString() ?? "—");
            }
        }
    }
}