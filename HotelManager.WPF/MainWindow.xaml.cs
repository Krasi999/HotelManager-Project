using HotelManager.ViewModels.Translation;
using HotelManager.WPF.Views;
using System.Windows;
using System.Windows.Controls;

namespace HotelManager.WPF
{
    public partial class MainWindow : Window
    {
        private bool _isBulgarian = true;

        private RoomsView? _roomsView;
        private GuestsView? _guestsView;
        private ReservationsView? _reservationsView;
        private DynamicFilterView? _filterView;
        private DynamicListView? _dynListView;

        public MainWindow()
        {
            InitializeComponent();
            DbIndicator.Text = $"База: {App.CurrentDatabase}";

            _roomsView = new RoomsView();
            RoomsContent.Content = _roomsView;
        }

        private void MainTabControl_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedItem == TabRooms &&
                RoomsContent.Content == null)
            {
                _roomsView = new RoomsView();
                RoomsContent.Content = _roomsView;
                _roomsView.ApplyLanguage(_isBulgarian);
            }
            else if (MainTabControl.SelectedItem == TabGuests &&
                     GuestsContent.Content == null)
            {
                _guestsView = new GuestsView();
                GuestsContent.Content = _guestsView;
                _guestsView.ApplyLanguage(_isBulgarian);
            }
            else if (MainTabControl.SelectedItem == TabReservations &&
                     ReservationsContent.Content == null)
            {
                _reservationsView = new ReservationsView();
                ReservationsContent.Content = _reservationsView;
                _reservationsView.ApplyLanguage(_isBulgarian);
            }
            else if (MainTabControl.SelectedItem == TabFilter &&
                     FilterContent.Content == null)
            {
                _filterView = new DynamicFilterView();
                FilterContent.Content = _filterView;
                _filterView.ApplyLanguage(_isBulgarian);
            }
            else if (MainTabControl.SelectedItem == TabDynList &&
                     DynListContent.Content == null)
            {
                _dynListView = new DynamicListView();
                DynListContent.Content = _dynListView;
                _dynListView.ApplyLanguage(_isBulgarian);
            }
        }

        private async void BtnLang_Click(object sender,
            RoutedEventArgs e)
        {
            _isBulgarian = !_isBulgarian;

            Translator.IsBulgarian = _isBulgarian;

            ShowLoading(
                _isBulgarian
                    ? "Превеждане на български..."
                    : "Translating to English...",
                _isBulgarian
                    ? "LibreTranslate работи..."
                    : "LibreTranslate is working...");

            btnLang.Content = _isBulgarian
                ? "🌐 EN" : "🌐 БГ";
            TabRooms.Header = _isBulgarian
                ? "🛏  Стаи" : "🛏  Rooms";
            TabGuests.Header = _isBulgarian
                ? "👤  Гости" : "👤  Guests";
            TabReservations.Header = _isBulgarian
                ? "📋  Резервации" : "📋  Reservations";
            TabFilter.Header = _isBulgarian
                ? "🔍  Филтри" : "🔍  Filters";
            TabDynList.Header = _isBulgarian
                ? "📊  Динамичен списък" : "📊  Dynamic List";
            DbIndicator.Text = _isBulgarian
                ? $"База: {App.CurrentDatabase}"
                : $"Database: {App.CurrentDatabase}";

            await TranslateView(
                "🛏",
                _isBulgarian ? "Стаи..." : "Rooms...",
                () => _roomsView?.ApplyLanguage(_isBulgarian));

            await TranslateView(
                "👤",
                _isBulgarian ? "Гости..." : "Guests...",
                () => _guestsView?.ApplyLanguage(_isBulgarian));

            await TranslateView(
                "📋",
                _isBulgarian ? "Резервации..." : "Reservations...",
                () => _reservationsView?.ApplyLanguage(_isBulgarian));

            await TranslateView(
                "🔍",
                _isBulgarian ? "Филтри..." : "Filters...",
                () => _filterView?.ApplyLanguage(_isBulgarian));

            await TranslateView(
                "📊",
                _isBulgarian
                    ? "Динамичен списък..." : "Dynamic List...",
                () => _dynListView?.ApplyLanguage(_isBulgarian));

            HideLoading();
        }

        private void ShowLoading(string text, string subText)
        {
            LoadingText.Text = text;
            LoadingSubText.Text = subText;
            LoadingStep.Text = string.Empty;
            LoadingOverlay.Visibility = Visibility.Visible;
        }

        private void HideLoading()
        {
            LoadingOverlay.Visibility = Visibility.Collapsed;
            LoadingStep.Text = string.Empty;
        }

        private async Task TranslateView(
            string icon, string stepText, Action translateAction)
        {
            LoadingStep.Text = $"{icon}  {stepText}";

            await Task.Run(() =>
            {
                Dispatcher.Invoke(translateAction);
            });

            await Task.Delay(50);
        }
    }
}