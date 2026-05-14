using HotelManager.Data;
using HotelManager.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HotelManager.WPF.Views
{
    public partial class GuestsView : UserControl
    {
        private GuestViewModel _vm = null!;

        public GuestsView()
        {
            InitializeComponent();
            var repo = RepositoryFactory
                .GetGuestRepository(App.CurrentDatabase);
            _vm = new GuestViewModel(repo);
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
                _vm.StatusMessage.Contains("задължителни") ||
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

            lblFormTitle.Text = bg ? "Детайли за гост" : "Guest Details";
            lblFormSubtitle.Text = bg ? "Добави или редактирай гост" : "Add or edit a guest";
            lblFirstName.Text = bg ? "Име" : "First Name";
            lblLastName.Text = bg ? "Фамилия" : "Last Name";
            lblEmail.Text = bg ? "Имейл" : "Email";
            lblPhone.Text = bg ? "Телефон" : "Phone";
            lblEGN.Text = bg ? "ЕГН" : "ID Number";
            lblBirth.Text = bg ? "Дата на раждане" : "Date of Birth";
            btnAdd.Content = bg ? "➕  Добави" : "➕  Add";
            btnUpdate.Content = bg ? "✏️  Обнови" : "✏️  Update";
            btnDelete.Content = bg ? "🗑️  Изтрий" : "🗑️  Delete";
            btnClear.Content = bg ? "🔄  Нов запис" : "🔄  New Record";
            btnLoad.Content = bg ? "🔄  Зареди" : "🔄  Load";
            lblGridTitle.Text = bg ? "Списък на гости" : "Guests List";
            lblGridSubtitle.Text = bg ? "Избери ред за редактиране" : "Select a row to edit";
            colFirstName.Header = bg ? "Име" : "First Name";
            colLastName.Header = bg ? "Фамилия" : "Last Name";
            colEmail.Header = bg ? "Имейл" : "Email";
            colPhone.Header = bg ? "Телефон" : "Phone";
            colEGN.Header = bg ? "ЕГН" : "ID Number";
            colBirth.Header = bg ? "Дата на раждане" : "Date of Birth";

            _vm.RefreshLocalizedGuests();
            _vm.RefreshStatusLanguage();
            if (!string.IsNullOrEmpty(_vm.StatusMessage))
                UpdateStatusStyle();

        }
    }
}