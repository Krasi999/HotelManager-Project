using HotelManager.Data;
using HotelManager.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HotelManager.WPF.Views
{
    public partial class ReservationsView : UserControl
    {

        private ReservationViewModel _vm = null!;

        public ReservationsView()
        {
            InitializeComponent();
            var resRepo = RepositoryFactory.GetReservationRepository(App.CurrentDatabase);
            var roomRepo = RepositoryFactory.GetRoomRepository(App.CurrentDatabase);
            var guestRepo = RepositoryFactory.GetGuestRepository(App.CurrentDatabase);

            _vm = new ReservationViewModel(resRepo, roomRepo, guestRepo);
            DataContext = _vm;

            _vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != nameof(_vm.StatusMessage))
                    return;
                UpdateStatusStyle();
            };
        }

        private void UpdateStatusStyle()
        {
            bool isError =
                _vm.StatusMessage.Contains("не може") ||
                _vm.StatusMessage.Contains("cannot") ||
                _vm.StatusMessage.Contains("задължителен") ||
                _vm.StatusMessage.Contains("required") ||
                _vm.StatusMessage.Contains("грешка") ||
                _vm.StatusMessage.Contains("error");

            if (isError)
            {
                StatusBorder.Background = new SolidColorBrush(
                    Color.FromRgb(253, 237, 236));
                StatusIcon.Text = "⚠️";
                StatusIconLabel.Text = _vm.IsBulgarian
                    ? "Внимание" : "Warning";
                StatusIconLabel.Foreground = new SolidColorBrush(
                    Color.FromRgb(192, 57, 43));
                StatusText.Foreground = new SolidColorBrush(
                    Color.FromRgb(192, 57, 43));
            }
            else
            {
                StatusBorder.Background = new SolidColorBrush(
                    Color.FromRgb(234, 246, 255));
                StatusIcon.Text = "ℹ️";
                StatusIconLabel.Text = _vm.IsBulgarian
                    ? "Информация" : "Info";
                StatusIconLabel.Foreground = new SolidColorBrush(
                    Color.FromRgb(26, 111, 168));
                StatusText.Foreground = new SolidColorBrush(
                    Color.FromRgb(26, 111, 168));
            }
        }

        private void UserControl_Loaded(object sender,
            RoutedEventArgs e)
        {
            _vm.LoadCommand.Execute(null);
        }

        public void ApplyLanguage(bool bg)
        {
            _vm.IsBulgarian = bg;

            lblFormTitle.Text = bg ? "Нова резервация" : "New Reservation";
            lblFormSubtitle.Text = bg ? "Попълни данните за резервацията" : "Fill in reservation details";
            lblRoom.Text = bg ? "Стая" : "Room";
            lblGuest.Text = bg ? "Гост" : "Guest";
            lblCheckIn.Text = bg ? "Настаняване" : "Check In";
            lblCheckOut.Text = bg ? "Напускане" : "Check Out";
            lblStatus.Text = bg ? "Статус" : "Status";
            cmbConfirmed.Content = bg ? "Потвърдена" : "Confirmed";
            cmbCancelled.Content = bg ? "Отменена" : "Cancelled";
            cmbCompleted.Content = bg ? "Завършена" : "Completed";
            lblTotalLabel.Text = bg ? "Обща цена:" : "Total Price:";
            btnAdd.Content = bg ? "➕  Добави" : "➕  Add";
            btnUpdate.Content = bg ? "✏️  Обнови" : "✏️  Update";
            btnDelete.Content = bg ? "🗑️  Изтрий" : "🗑️  Delete";
            btnClear.Content = bg ? "🔄  Нов запис" : "🔄  New Record";
            btnLoad.Content = bg ? "🔄  Зареди" : "🔄  Load";
            lblGridTitle.Text = bg ? "Списък на резервации" : "Reservations List";
            lblGridSubtitle.Text = bg ? "Избери ред за редактиране" : "Select a row to edit";
            colRoom.Header = bg ? "Стая" : "Room";
            colGuest.Header = bg ? "Гост" : "Guest";
            colCheckIn.Header = bg ? "Настаняване" : "Check In";
            colCheckOut.Header = bg ? "Напускане" : "Check Out";
            colNights.Header = bg ? "Нощувки" : "Nights";
            colPrice.Header = bg ? "Цена" : "Price";
            colStatus.Header = bg ? "Статус" : "Status";

            _vm.RefreshStatusLanguage();
            if (!string.IsNullOrEmpty(_vm.StatusMessage))
                UpdateStatusStyle();

            _vm.RefreshLocalizedReservations();
            _vm.RefreshStatusLanguage();
        }
    }
}