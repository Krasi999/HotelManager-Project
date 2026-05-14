using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManager.WinForms.Forms
{
    partial class GuestsForm
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
            lblFormTitle = new Label();
            lblFName = new Label();
            txtFirstName = new TextBox();
            lblLName = new Label();
            txtLastName = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblEGN = new Label();
            txtEGN = new TextBox();
            lblBirth = new Label();
            dtpBirth = new DateTimePicker();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            lblStatus = new Label();
            dgvGuests = new DataGridView();

            SuspendLayout();

            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(245, 246, 250);
            Font = new Font("Segoe UI", 9.5f);

            // pnlForm
            pnlForm.Location = new Point(0, 0);
            pnlForm.Size = new Size(300, 690);
            pnlForm.BackColor = Color.White;
            pnlForm.BorderStyle = BorderStyle.FixedSingle;
            pnlForm.Padding = new Padding(16);

            lblFormTitle.Text = "Детайли за гост";
            lblFormTitle.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblFormTitle.Location = new Point(16, 16);
            lblFormTitle.AutoSize = true;

            int y = 52;
            AddRow(pnlForm, lblFName, "Име", txtFirstName, ref y);
            AddRow(pnlForm, lblLName, "Фамилия", txtLastName, ref y);
            AddRow(pnlForm, lblEmail, "Имейл", txtEmail, ref y);
            AddRow(pnlForm, lblPhone, "Телефон", txtPhone, ref y);
            AddRow(pnlForm, lblEGN, "ЕГН", txtEGN, ref y);

            lblBirth.Text = "Дата на раждане";
            lblBirth.Location = new Point(16, y);
            lblBirth.AutoSize = true;
            lblBirth.ForeColor = Color.FromArgb(127, 140, 141);
            lblBirth.Font = new Font("Segoe UI", 8.5f);
            y += 18;

            dtpBirth.Location = new Point(16, y);
            dtpBirth.Size = new Size(268, 28);
            dtpBirth.Font = new Font("Segoe UI", 9.5f);
            dtpBirth.Format = DateTimePickerFormat.Short;
            y += 40;

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
                lblFormTitle,
                lblFName, txtFirstName, lblLName, txtLastName,
                lblEmail, txtEmail, lblPhone, txtPhone,
                lblEGN, txtEGN, lblBirth, dtpBirth,
                btnAdd, btnUpdate, btnDelete, btnClear, lblStatus
            });

            // pnlGrid
            pnlGrid.Location = new Point(316, 0);
            pnlGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left |
                                AnchorStyles.Right | AnchorStyles.Bottom;
            pnlGrid.BackColor = Color.White;
            pnlGrid.BorderStyle = BorderStyle.FixedSingle;

            dgvGuests.Dock = DockStyle.Fill;
            dgvGuests.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvGuests.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvGuests.MultiSelect = false;
            dgvGuests.ReadOnly = true;
            dgvGuests.BackgroundColor = Color.White;
            dgvGuests.BorderStyle = BorderStyle.None;
            dgvGuests.RowHeadersVisible = false;
            dgvGuests.Font = new Font("Segoe UI", 9.5f);
            dgvGuests.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(214, 234, 248);
            dgvGuests.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(44, 62, 80);
            dgvGuests.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(248, 249, 250);
            dgvGuests.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(236, 240, 241);
            dgvGuests.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvGuests.ColumnHeadersHeight = 36;
            dgvGuests.RowTemplate.Height = 34;
            dgvGuests.EnableHeadersVisualStyles = false;

            pnlGrid.Controls.Add(dgvGuests);
            Controls.AddRange(new Control[] { pnlForm, pnlGrid });
            ResumeLayout(false);
        }

        private static void AddRow(Panel panel, Label lbl,
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
        private Label lblFormTitle;
        private Label lblFName;
        private TextBox txtFirstName;
        private Label lblLName;
        private TextBox txtLastName;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPhone;
        private TextBox txtPhone;
        private Label lblEGN;
        private TextBox txtEGN;
        private Label lblBirth;
        private DateTimePicker dtpBirth;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Label lblStatus;
        private DataGridView dgvGuests;
    }
}