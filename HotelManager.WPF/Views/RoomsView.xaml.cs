using HotelManager.Data;
using HotelManager.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HotelManager.WPF.Views
{
    public partial class RoomsView : UserControl
    {
        private RoomViewModel _vm = null!;

        public RoomsView()
        {
            InitializeComponent();
            var repo = RepositoryFactory
                .GetRoomRepository(App.CurrentDatabase);
            _vm = new RoomViewModel(repo);
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

            lblFormTitle.Text = bg ? "Детайли за стая" : "Room Details";
            lblFormSubtitle.Text = bg ? "Добави или редактирай стая" : "Add or edit a room";
            lblNumber.Text = bg ? "Номер на стая" : "Room Number";
            lblType.Text = bg ? "Тип стая" : "Room Type";
            lblPrice.Text = bg ? "Цена (евро/нощ)" : "Price (euro/night)";
            lblCapacity.Text = bg ? "Капацитет" : "Capacity";
            lblDesc.Text = bg ? "Описание" : "Description";
            chkAvailable.Content = bg ? "Стаята е свободна" : "Room is available";
            btnAdd.Content = bg ? "➕  Добави" : "➕  Add";
            btnUpdate.Content = bg ? "✏️  Обнови" : "✏️  Update";
            btnDelete.Content = bg ? "🗑️  Изтрий" : "🗑️  Delete";
            btnClear.Content = bg ? "🔄  Нов запис" : "🔄  New Record";
            btnLoad.Content = bg ? "🔄  Зареди" : "🔄  Load";
            lblGridTitle.Text = bg ? "Списък на стаи" : "Rooms List";
            lblGridSubtitle.Text = bg ? "Избери ред за редактиране" : "Select a row to edit";
            colNumber.Header = bg ? "Номер" : "Number";
            colType.Header = bg ? "Тип" : "Type";
            colPrice.Header = bg ? "Цена/нощ" : "Price/night";
            colCap.Header = bg ? "Капацитет" : "Capacity";
            colAvail.Header = bg ? "Свободна" : "Available";
            colDesc.Header = bg ? "Описание" : "Description";
            cmbSingle.Content = bg ? "Единична" : "Single";
            cmbDouble.Content = bg ? "Двойна" : "Double";
            cmbSuite.Content = bg ? "Студио" : "Suite";
            cmbApartment.Content = bg ? "Апартамент" : "Apartment";

            _vm.RefreshStatusLanguage();
            if (!string.IsNullOrEmpty(_vm.StatusMessage))
                UpdateStatusStyle();

            _vm.RefreshLocalizedRooms();
            _vm.RefreshStatusLanguage();
            if (!string.IsNullOrEmpty(_vm.StatusMessage))
                UpdateStatusStyle();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}