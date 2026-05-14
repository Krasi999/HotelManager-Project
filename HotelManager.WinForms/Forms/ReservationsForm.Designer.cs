using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManager.WinForms.Forms
{
    partial class ReservationsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlForm = new Panel();
            pnlGrid = new Panel();
            lblTitle = new Label();
            lblRoom = new Label();
            cmbRooms = new ComboBox();
            lblGuest = new Label();
            cmbGuests = new ComboBox();
            lblIn = new Label();
            dtpCheckIn = new DateTimePicker();
            lblOut = new Label();
            dtpCheckOut = new DateTimePicker();
            lblStat = new Label();
            cmbStatus = new ComboBox();
            lblPriceLbl = new Label();
            lblTotalPrice = new Label();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            lblStatus = new Label();
            dgvReservations = new DataGridView();

            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(245, 246, 250);
            Font = new Font("Segoe UI", 9.5f);

            pnlForm.Location = new Point(0, 0);
            pnlForm.Size = new Size(300, 690);
            pnlForm.BackColor = Color.White;
            pnlForm.BorderStyle = BorderStyle.FixedSingle;

            lblTitle.Text = "Нова резервация";
            lblTitle.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblTitle.Location = new Point(16, 16);
            lblTitle.AutoSize = true;

            int y = 52;
            AddLabelControl(pnlForm, lblRoom, "Стая", cmbRooms, ref y);
            AddLabelControl(pnlForm, lblGuest, "Гост", cmbGuests, ref y);
            AddLabelControl(pnlForm, lblIn, "Настаняване", dtpCheckIn, ref y);
            AddLabelControl(pnlForm, lblOut, "Напускане", dtpCheckOut, ref y);
            AddLabelControl(pnlForm, lblStat, "Статус", cmbStatus, ref y);

            dtpCheckIn.Format = DateTimePickerFormat.Short;
            dtpCheckOut.Format = DateTimePickerFormat.Short;

            // Обща цена
            lblPriceLbl.Text = "Обща цена:";
            lblPriceLbl.Location = new Point(16, y);
            lblPriceLbl.AutoSize = true;
            lblPriceLbl.ForeColor = Color.FromArgb(127, 140, 141);
            lblPriceLbl.Font = new Font("Segoe UI", 8.5f);
            y += 20;

            lblTotalPrice.Location = new Point(16, y);
            lblTotalPrice.AutoSize = true;
            lblTotalPrice.ForeColor = Color.FromArgb(41, 128, 185);
            lblTotalPrice.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            y += 36;

            RoomsForm.StyleButtonPublic(btnAdd, "➕  Добави", Color.FromArgb(39, 174, 96));
            RoomsForm.StyleButtonPublic(btnUpdate, "✏️  Обнови", Color.FromArgb(52, 152, 219));
            RoomsForm.StyleButtonPublic(btnDelete, "🗑️  Изтрий", Color.FromArgb(231, 76, 60));
            RoomsForm.StyleButtonPublic(btnClear, "🔄  Изчисти", Color.FromArgb(127, 140, 141));

            btnAdd.Location = new Point(16, y + 10);
            btnUpdate.Location = new Point(156, y + 10);
            btnDelete.Location = new Point(16, y + 52);
            btnClear.Location = new Point(156, y + 52);

            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;

            lblStatus.Location = new Point(16, y + 104);
            lblStatus.Size = new Size(268, 40);
            lblStatus.ForeColor = Color.FromArgb(41, 128, 185);
            lblStatus.Font = new Font("Segoe UI", 9f);

            pnlForm.Controls.AddRange(new Control[]
            {
                lblTitle, lblRoom, cmbRooms, lblGuest, cmbGuests,
                lblIn, dtpCheckIn, lblOut, dtpCheckOut,
                lblStat, cmbStatus, lblPriceLbl, lblTotalPrice,
                btnAdd, btnUpdate, btnDelete, btnClear, lblStatus
            });

            pnlGrid.Location = new Point(316, 0);
            pnlGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left |
                                  AnchorStyles.Right | AnchorStyles.Bottom;
            pnlGrid.BackColor = Color.White;
            pnlGrid.BorderStyle = BorderStyle.FixedSingle;

            dgvReservations.Dock = DockStyle.Fill;
            dgvReservations.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvReservations.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvReservations.MultiSelect = false;
            dgvReservations.ReadOnly = true;
            dgvReservations.BackgroundColor = Color.White;
            dgvReservations.BorderStyle = BorderStyle.None;
            dgvReservations.RowHeadersVisible = false;
            dgvReservations.Font = new Font("Segoe UI", 9.5f);
            dgvReservations.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(214, 234, 248);
            dgvReservations.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(44, 62, 80);
            dgvReservations.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(248, 249, 250);
            dgvReservations.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(236, 240, 241);
            dgvReservations.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvReservations.ColumnHeadersHeight = 36;
            dgvReservations.RowTemplate.Height = 34;
            dgvReservations.EnableHeadersVisualStyles = false;

            pnlGrid.Controls.Add(dgvReservations);
            Controls.AddRange(new Control[] { pnlForm, pnlGrid });
            ResumeLayout(false);
        }

        private static void AddLabelControl(Panel panel, Label lbl,
            string text, Control ctrl, ref int y)
        {
            lbl.Text = text;
            lbl.Location = new Point(16, y);
            lbl.AutoSize = true;
            lbl.ForeColor = Color.FromArgb(127, 140, 141);
            lbl.Font = new Font("Segoe UI", 8.5f);
            y += 18;
            ctrl.Location = new Point(16, y);
            ctrl.Size = new Size(268, 28);
            ctrl.Font = new Font("Segoe UI", 9.5f);
            y += 36;
        }

        private Panel pnlForm;
        private Panel pnlGrid;
        private Label lblTitle;
        private Label lblRoom;
        private ComboBox cmbRooms;
        private Label lblGuest;
        private ComboBox cmbGuests;
        private Label lblIn;
        private DateTimePicker dtpCheckIn;
        private Label lblOut;
        private DateTimePicker dtpCheckOut;
        private Label lblStat;
        private ComboBox cmbStatus;
        private Label lblPriceLbl;
        private Label lblTotalPrice;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Label lblStatus;
        private DataGridView dgvReservations;
    }
}