using HotelManager.Data;
using HotelManager.Models;
using HotelManager.ViewModels;
using HotelManager.ViewModels.Translation;
using System.Windows;
using System.Windows.Controls;

namespace HotelManager.WPF.Views
{
    public partial class DynamicListView : UserControl
    {
        private bool _isBulgarian = true;
        private object? _currentVm;

        public DynamicListView()
        {
            InitializeComponent();
        }

        private async void RoomsBasic_Click(
            object sender, RoutedEventArgs e)
        {
            var repo = RepositoryFactory
                .GetRoomRepository(App.CurrentDatabase);
            var vm = new DynamicListViewModel<Room>();

            await vm.LoadDataAsync(
                await repo.GetAllAsync(),
                new GridColumnDefinition[]
                {
                    new()
                    {
                        PropertyName = nameof(Room.Number),
                        Header = Translator.FieldName(
                            nameof(Room.Number)),
                        Width = 80
                    },
                    new()
                    {
                        PropertyName = nameof(Room.Type),
                        Header = Translator.FieldName(
                            nameof(Room.Type)),
                        Width = 100
                    },
                    new()
                    {
                        PropertyName = nameof(Room.PricePerNight),
                        Header = Translator.FieldName(
                            nameof(Room.PricePerNight)),
                        Width = 140
                    }
                });

            _currentVm = vm;
            BuildGrid(vm);
        }

        private async void RoomsAll_Click(
            object sender, RoutedEventArgs e)
        {
            var repo = RepositoryFactory
                .GetRoomRepository(App.CurrentDatabase);
            var vm = new DynamicListViewModel<Room>();

            await vm.LoadDataAsync(
                await repo.GetAllAsync(),
                new GridColumnDefinition[]
                {
                    new()
                    {
                        PropertyName = nameof(Room.Number),
                        Header = Translator.FieldName(
                            nameof(Room.Number)),
                        Width = 80
                    },
                    new()
                    {
                        PropertyName = nameof(Room.Type),
                        Header = Translator.FieldName(
                            nameof(Room.Type)),
                        Width = 100
                    },
                    new()
                    {
                        PropertyName = nameof(Room.PricePerNight),
                        Header = Translator.FieldName(
                            nameof(Room.PricePerNight)),
                        Width = 140
                    },
                    new()
                    {
                        PropertyName = nameof(Room.Capacity),
                        Header = Translator.FieldName(
                            nameof(Room.Capacity)),
                        Width = 90
                    },
                    new()
                    {
                        PropertyName = nameof(Room.IsAvailable),
                        Header = Translator.FieldName(
                            nameof(Room.IsAvailable)),
                        Width = 90
                    },
                    new()
                    {
                        PropertyName = nameof(Room.Description),
                        Header = Translator.FieldName(
                            nameof(Room.Description)),
                        Width = 160
                    }
                });

            _currentVm = vm;
            BuildGrid(vm);
        }

        private async void Reservations_Click(
            object sender, RoutedEventArgs e)
        {
            var repo = RepositoryFactory
                .GetReservationRepository(App.CurrentDatabase);
            var vm = new DynamicListViewModel<Reservation>();

            await vm.LoadDataAsync(
                await repo.GetAllAsync(),
                new GridColumnDefinition[]
                {
                    new()
                    {
                        PropertyName = "Id",
                        Header       = "ID",
                        Width        = 50
                    },
                    new()
                    {
                        PropertyName = nameof(Reservation.CheckIn),
                        Header = Translator.FieldName(
                            nameof(Reservation.CheckIn)),
                        Width = 110
                    },
                    new()
                    {
                        PropertyName = nameof(Reservation.CheckOut),
                        Header = Translator.FieldName(
                            nameof(Reservation.CheckOut)),
                        Width = 110
                    },
                    new()
                    {
                        PropertyName =
                            nameof(Reservation.TotalPrice),
                        Header = Translator.FieldName(
                            nameof(Reservation.TotalPrice)),
                        Width = 130
                    },
                    new()
                    {
                        PropertyName = nameof(Reservation.Status),
                        Header = Translator.FieldName(
                            nameof(Reservation.Status)),
                        Width = 110
                    }
                });

            _currentVm = vm;
            BuildGrid(vm);
        }

        private void Clear_Click(
            object sender, RoutedEventArgs e)
        {
            DynamicGrid.Columns.Clear();
            DynamicGrid.ItemsSource = null;
            lblStatus.Text = string.Empty;
            _currentVm = null;
            DataContext = null;
            UpdateSelectedLabel(null);
        }

        private void BuildGrid<T>(DynamicListViewModel<T> vm)
            where T : class
        {
            DynamicGrid.Columns.Clear();
            DynamicGrid.ItemsSource = null;

            foreach (var col in vm.Columns)
            {
                DynamicGrid.Columns.Add(
                    new DataGridTextColumn
                    {
                        Header = col.Header,
                        Binding = new System.Windows.Data.Binding(
                            $"Values[{col.PropertyName}]"),
                        Width = col.Width
                    });
            }

            DynamicGrid.ItemsSource = vm.Rows;
            lblStatus.Text = vm.StatusMessage;
            DataContext = vm;

            vm.PropertyChanged += (s, args) =>
            {
                if (args.PropertyName ==
                    nameof(vm.StatusMessage))
                    lblStatus.Text = vm.StatusMessage;

                if (args.PropertyName == nameof(vm.Rows))
                {
                    DynamicGrid.ItemsSource = null;
                    DynamicGrid.ItemsSource = vm.Rows;
                }
            };
        }

        private void DynamicGrid_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (DynamicGrid.SelectedItem is DynamicRow row)
                UpdateSelectedLabel(row);
        }

        private void UpdateSelectedLabel(DynamicRow? row)
        {
            if (row == null)
            {
                SelectedObjectText.Text = string.Empty;
                return;
            }

            bool bg = _isBulgarian;

            if (row.OriginalObject is Room room)
            {
                SelectedObjectText.Text = bg
                    ? $"Избраният обект е стая" +
                      $" № {room.Number}" +
                      $" — {Translator.RoomType(room.Type)}" +
                      $" ({Translator.Price(room.PricePerNight)})"
                    : $"Selected object is room" +
                      $" № {room.Number}" +
                      $" — {Translator.RoomType(room.Type)}" +
                      $" ({Translator.Price(room.PricePerNight)})";
            }
            else if (row.OriginalObject is Reservation res)
            {
                SelectedObjectText.Text = bg
                    ? $"Избраният обект е резервация" +
                      $" № {res.Id}" +
                      $" — {Translator.Date(res.CheckIn)}" +
                      $" до {Translator.Date(res.CheckOut)}" +
                      $" ({Translator.Price(res.TotalPrice)})"
                    : $"Selected object is reservation" +
                      $" № {res.Id}" +
                      $" — {Translator.Date(res.CheckIn)}" +
                      $" to {Translator.Date(res.CheckOut)}" +
                      $" ({Translator.Price(res.TotalPrice)})";
            }
        }

        public async void ApplyLanguage(bool bg)
        {
            _isBulgarian = bg;

            lblPageTitle.Text = bg
                ? "Динамичен списък"
                : "Dynamic List";
            lblPageSubtitle.Text = bg
                ? "Избери какво да покажеш"
                : "Choose what to display";
            btnRoomsBasic.Content = bg
                ? "🛏  Стаи — основни"
                : "🛏  Rooms — basic";
            btnRoomsAll.Content = bg
                ? "🛏  Стаи — всички"
                : "🛏  Rooms — all";
            btnReservations.Content = bg
                ? "📋  Резервации"
                : "📋  Reservations";
            btnClear.Content = bg
                ? "🗑️  Изчисти"
                : "🗑️  Clear";
            lblGridTitle.Text = bg
                ? "Резултати"
                : "Results";
            lblSelectedLabel.Text = bg
                ? "Избран обект:"
                : "Selected object:";

            if (_currentVm is DynamicListViewModel<Room> vmRoom)
            {
                await vmRoom.RefreshLanguageAsync();
                BuildGrid(vmRoom);
            }
            else if (_currentVm is
                DynamicListViewModel<Reservation> vmRes)
            {
                await vmRes.RefreshLanguageAsync();
                BuildGrid(vmRes);
            }

            if (DynamicGrid.SelectedItem is DynamicRow row)
                UpdateSelectedLabel(row);
        }
    }
}