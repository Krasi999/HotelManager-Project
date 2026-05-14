using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManager.WinForms.Forms
{
    partial class RoomsForm
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
            lblNumLabel = new Label();
            txtNumber = new TextBox();
            lblTypeLabel = new Label();
            cmbType = new ComboBox();
            lblPriceLabel = new Label();
            txtPrice = new TextBox();
            lblCapLabel = new Label();
            txtCapacity = new TextBox();
            lblDescLabel = new Label();
            txtDescription = new TextBox();
            chkAvailable = new CheckBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            lblStatus = new Label();
            dgvRooms = new DataGridView();

            SuspendLayout();

            // Form
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(245, 246, 250);
            Font = new Font("Segoe UI", 9.5f);

            // pnlForm — ляво
            pnlForm.Location = new Point(0, 0);
            pnlForm.Size = new Size(300, 690);
            pnlForm.BackColor = Color.White;
            pnlForm.Padding = new Padding(16);

            StylePanel(pnlForm);

            lblFormTitle.Text = "Детайли за стая";
            lblFormTitle.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblFormTitle.Location = new Point(16, 16);
            lblFormTitle.AutoSize = true;

            int y = 52;
            AddFormRow(pnlForm, lblNumLabel, "Номер на стая", txtNumber, ref y);
            AddFormRow(pnlForm, lblTypeLabel, "Тип стая", cmbType, ref y);
            AddFormRow(pnlForm, lblPriceLabel, "Цена на нощувка", txtPrice, ref y);
            AddFormRow(pnlForm, lblCapLabel, "Капацитет", txtCapacity, ref y);
            AddFormRow(pnlForm, lblDescLabel, "Описание", txtDescription, ref y);

            txtDescription.Multiline = true;
            txtDescription.Height = 60;

            chkAvailable.Text = "Свободна стая";
            chkAvailable.Location = new Point(16, y + 4);
            chkAvailable.AutoSize = true;
            y += 32;

            StyleButton(btnAdd, "➕  Добави", Color.FromArgb(39, 174, 96));
            StyleButton(btnUpdate, "✏️  Обнови", Color.FromArgb(52, 152, 219));
            StyleButton(btnDelete, "🗑️  Изтрий", Color.FromArgb(231, 76, 60));
            StyleButton(btnClear, "🔄  Изчисти", Color.FromArgb(127, 140, 141));

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
                lblFormTitle, lblNumLabel, txtNumber,
                lblTypeLabel, cmbType,
                lblPriceLabel, txtPrice,
                lblCapLabel, txtCapacity,
                lblDescLabel, txtDescription,
                chkAvailable,
                btnAdd, btnUpdate, btnDelete, btnClear,
                lblStatus
            });

            // pnlGrid — дясно
            pnlGrid.Location = new Point(316, 0);
            pnlGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left |
                                AnchorStyles.Right | AnchorStyles.Bottom;
            pnlGrid.BackColor = Color.White;
            pnlGrid.Padding = new Padding(16);
            StylePanel(pnlGrid);

            dgvRooms.Dock = DockStyle.Fill;
            dgvRooms.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvRooms.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvRooms.MultiSelect = false;
            dgvRooms.ReadOnly = true;
            dgvRooms.BackgroundColor = Color.White;
            dgvRooms.BorderStyle = BorderStyle.None;
            dgvRooms.RowHeadersVisible = false;
            dgvRooms.Font = new Font("Segoe UI", 9.5f);
            dgvRooms.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(214, 234, 248);
            dgvRooms.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(44, 62, 80);
            dgvRooms.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(248, 249, 250);
            dgvRooms.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(236, 240, 241);
            dgvRooms.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvRooms.ColumnHeadersHeight = 36;
            dgvRooms.RowTemplate.Height = 34;
            dgvRooms.EnableHeadersVisualStyles = false;

            pnlGrid.Controls.Add(dgvRooms);

            Controls.AddRange(new Control[] { pnlForm, pnlGrid });
            ResumeLayout(false);
        }

        // Helper — добавя label + контрола вертикално
        private static void AddFormRow(Panel panel, Label lbl,
            string labelText, Control ctrl, ref int y)
        {
            lbl.Text = labelText;
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

        public static void StyleButton(Button btn, string text, Color color)
        {
            btn.Text = text;
            btn.Size = new Size(124, 36);
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9.5f);
            btn.Cursor = Cursors.Hand;
        }

        private static void StylePanel(Panel panel)
        {
            // Симулира shadow с border
            panel.BorderStyle = BorderStyle.FixedSingle;
        }

        private Panel pnlForm;
        private Panel pnlGrid;
        private Label lblFormTitle;
        private Label lblNumLabel;
        private TextBox txtNumber;
        private Label lblTypeLabel;
        private ComboBox cmbType;
        private Label lblPriceLabel;
        private TextBox txtPrice;
        private Label lblCapLabel;
        private TextBox txtCapacity;
        private Label lblDescLabel;
        private TextBox txtDescription;
        private CheckBox chkAvailable;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Label lblStatus;
        private DataGridView dgvRooms;
    }
}