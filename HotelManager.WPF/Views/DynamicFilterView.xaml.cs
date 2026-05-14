using HotelManager.Data;
using HotelManager.Models;
using HotelManager.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HotelManager.WPF.Views
{
    public partial class DynamicFilterView : UserControl
    {
        private bool _isBulgarian = true;

        public DynamicFilterView()
        {
            InitializeComponent();
            LoadRoomFilters();
        }


        private void LoadRoomFilters()
        {
            var repo = RepositoryFactory
                .GetRoomRepository(App.CurrentDatabase);

            var vm = new DynamicFilterViewModel<Room>(repo,
                nameof(Room.Number),
                nameof(Room.Type),
                nameof(Room.PricePerNight),
                nameof(Room.IsAvailable),
                nameof(Room.Capacity));

            SetupViewModel(vm);
        }

        private void LoadGuestFilters()
        {
            var repo = RepositoryFactory
                .GetGuestRepository(App.CurrentDatabase);

            var vm = new DynamicFilterViewModel<Guest>(repo,
                nameof(Guest.FirstName),
                nameof(Guest.LastName),
                nameof(Guest.Email),
                nameof(Guest.Phone));

            SetupViewModel(vm);
        }


        private void SetupViewModel<T>(
            DynamicFilterViewModel<T> vm) where T : class
        {
            ResultsGrid.Columns.Clear();
            ResultsGrid.ItemsSource = null;

            DataContext = vm;

            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName !=
                    nameof(vm.LocalizedResults))
                    return;

                Dispatcher.Invoke(() =>
                {
                    ResultsGrid.Columns.Clear();
                    ResultsGrid.ItemsSource = null;
                    BuildResultColumns(vm);
                    ResultsGrid.ItemsSource =
                        vm.LocalizedResults;
                });
            };
        }

        private void BuildResultColumns<T>(
            DynamicFilterViewModel<T> vm) where T : class
        {
            ResultsGrid.Columns.Clear();

            if (typeof(T) == typeof(Room))
            {
                AddColumn(
                    nameof(LocalizedRoom.Number),
                    ViewModels.Translation.Translator
                        .FieldName("Number"),
                    70);
                AddColumn(
                    nameof(LocalizedRoom.Type),
                    ViewModels.Translation.Translator
                        .FieldName("Type"),
                    90);
                AddColumn(
                    nameof(LocalizedRoom.PricePerNight),
                    ViewModels.Translation.Translator
                        .FieldName("PricePerNight"),
                    130);
                AddColumn(
                    nameof(LocalizedRoom.Capacity),
                    ViewModels.Translation.Translator
                        .FieldName("Capacity"),
                    80);
                AddColumn(
                    nameof(LocalizedRoom.IsAvailable),
                    ViewModels.Translation.Translator
                        .FieldName("IsAvailable"),
                    80);
                AddColumn(
                    nameof(LocalizedRoom.Description),
                    ViewModels.Translation.Translator
                        .FieldName("Description"),
                    150);
            }
            else if (typeof(T) == typeof(Guest))
            {
                AddColumn(
                    nameof(LocalizedGuest.FirstName),
                    ViewModels.Translation.Translator
                        .FieldName("FirstName"),
                    100);
                AddColumn(
                    nameof(LocalizedGuest.LastName),
                    ViewModels.Translation.Translator
                        .FieldName("LastName"),
                    100);
                AddColumn(
                    nameof(LocalizedGuest.Email),
                    ViewModels.Translation.Translator
                        .FieldName("Email"),
                    180);
                AddColumn(
                    nameof(LocalizedGuest.Phone),
                    ViewModels.Translation.Translator
                        .FieldName("Phone"),
                    110);
                AddColumn(
                    nameof(LocalizedGuest.EGN),
                    ViewModels.Translation.Translator
                        .FieldName("EGN"),
                    100);
            }
        }

        private void AddColumn(string binding,
            string header, int width)
        {
            ResultsGrid.Columns.Add(
                new DataGridTextColumn
                {
                    Header = header,
                    Binding = new System.Windows.Data.Binding(
                        binding),
                    Width = width
                });
        }

        private void FilterRooms_Click(object sender,
            RoutedEventArgs e)
        {
            LoadRoomFilters();
        }

        private void FilterGuests_Click(object sender,
            RoutedEventArgs e)
        {
            LoadGuestFilters();
        }

        public void ApplyLanguage(bool bg)
        {
            _isBulgarian = bg;

            lblTitle.Text = bg
                ? "Търси в:" : "Search in:";
            lblFiltersTitle.Text = bg
                ? "Филтри" : "Filters";
            lblResultsTitle.Text = bg
                ? "Резултати" : "Results";
            btnRooms.Content = bg
                ? "🛏  Стаи" : "🛏  Rooms";
            btnGuests.Content = bg
                ? "👤  Гости" : "👤  Guests";
            btnSearch.Content = bg
                ? "🔍  Търси" : "🔍  Search";
            btnClearFilters.Content = bg
                ? "🔄  Изчисти" : "🔄  Clear";

            if (DataContext is
                DynamicFilterViewModel<Room> vmRoom)
            {
                vmRoom.ApplyLanguage();
                BuildResultColumns(vmRoom);

                ResultsGrid.ItemsSource = null;
                ResultsGrid.ItemsSource =
                    vmRoom.LocalizedResults;
            }
            else if (DataContext is
                DynamicFilterViewModel<Guest> vmGuest)
            {
                vmGuest.ApplyLanguage();
                BuildResultColumns(vmGuest);
                ResultsGrid.ItemsSource = null;
                ResultsGrid.ItemsSource =
                    vmGuest.LocalizedResults;
            }
        }
    }
}