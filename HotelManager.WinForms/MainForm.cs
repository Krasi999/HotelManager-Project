using HotelManager.Data;
using static System.Net.Mime.MediaTypeNames;

namespace HotelManager.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Text = "Hotel Manager";
            lblDatabase.Text = $"База: {Program.CurrentDatabase}";
        }

        private void btnRooms_Click(object sender, EventArgs e)
            => OpenForm(new Forms.RoomsForm());

        private void btnGuests_Click(object sender, EventArgs e)
            => OpenForm(new Forms.GuestsForm());

        private void btnReservations_Click(object sender, EventArgs e)
            => OpenForm(new Forms.ReservationsForm());

        private void btnDynamicFilter_Click(object sender, EventArgs e)
            => OpenForm(new Forms.DynamicFilterForm());

        private void btnDynamicList_Click(object sender, EventArgs e)
            => OpenForm(new Forms.DynamicListForm());

        // Показва формата в панела вдясно
        private void OpenForm(Form form)
        {
            foreach (Control ctrl in pnlContent.Controls)
                ctrl.Dispose();
            pnlContent.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(form);
            form.Show();
        }
    }
}