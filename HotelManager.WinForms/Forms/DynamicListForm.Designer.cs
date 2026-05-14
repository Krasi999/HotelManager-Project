using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManager.WinForms.Forms
{
    partial class DynamicListForm
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
            pnlTop = new Panel();
            pnlMain = new Panel();
            pnlBottom = new Panel();
            btnRoomsBasic = new Button();
            btnRoomsAll = new Button();
            btnReservations = new Button();
            btnClear = new Button();
            lblTitle = new Label();
            lblStatus = new Label();
            lblSelected = new Label();
            dgvDynamic = new DataGridView();

            SuspendLayout();
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(245, 246, 250);
            Font = new Font("Segoe UI", 9.5f);

            // pnlTop
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 64;
            pnlTop.BackColor = Color.White;
            pnlTop.BorderStyle = BorderStyle.FixedSingle;
            pnlTop.Padding = new Padding(12, 0, 0, 0);

            lblTitle.Text = "Покажи:";
            lblTitle.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            lblTitle.Location = new Point(12, 20);
            lblTitle.AutoSize = true;

            RoomsForm.StyleButtonPublic(btnRoomsBasic,
                "🛏  Стаи — основни", Color.FromArgb(52, 152, 219));
            RoomsForm.StyleButtonPublic(btnRoomsAll,
                "🛏  Стаи — всички", Color.FromArgb(52, 152, 219));
            RoomsForm.StyleButtonPublic(btnReservations,
                "📋  Резервации", Color.FromArgb(52, 152, 219));
            RoomsForm.StyleButtonPublic(btnClear,
                "🗑️  Изчисти", Color.FromArgb(231, 76, 60));

            btnRoomsBasic.Location = new Point(80, 14);
            btnRoomsAll.Location = new Point(230, 14);
            btnReservations.Location = new Point(380, 14);
            btnClear.Location = new Point(530, 14);

            pnlTop.Controls.AddRange(new Control[]
            {
                lblTitle, btnRoomsBasic, btnRoomsAll,
                btnReservations, btnClear
            });

            // pnlMain
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.BackColor = Color.White;
            pnlMain.BorderStyle = BorderStyle.FixedSingle;
            pnlMain.Padding = new Padding(12);

            lblStatus.Dock = DockStyle.Top;
            lblStatus.Height = 28;
            lblStatus.ForeColor = Color.FromArgb(127, 140, 141);
            lblStatus.Font = new Font("Segoe UI", 9f);

            dgvDynamic.Dock = DockStyle.Fill;
            dgvDynamic.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvDynamic.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvDynamic.MultiSelect = false;
            dgvDynamic.ReadOnly = true;
            dgvDynamic.BackgroundColor = Color.White;
            dgvDynamic.BorderStyle = BorderStyle.None;
            dgvDynamic.RowHeadersVisible = false;
            dgvDynamic.Font = new Font("Segoe UI", 9.5f);
            dgvDynamic.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(214, 234, 248);
            dgvDynamic.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(44, 62, 80);
            dgvDynamic.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(248, 249, 250);
            dgvDynamic.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(236, 240, 241);
            dgvDynamic.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgvDynamic.ColumnHeadersHeight = 36;
            dgvDynamic.RowTemplate.Height = 34;
            dgvDynamic.EnableHeadersVisualStyles = false;
            dgvDynamic.SelectionChanged +=
                dgvDynamic_SelectionChanged;

            pnlMain.Controls.AddRange(new Control[]
            {
                dgvDynamic, lblStatus
            });

            // pnlBottom
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Height = 44;
            pnlBottom.BackColor = Color.White;
            pnlBottom.BorderStyle = BorderStyle.FixedSingle;
            pnlBottom.Padding = new Padding(12, 0, 0, 0);

            lblSelected.Text = "Избран обект: —";
            lblSelected.Dock = DockStyle.Fill;
            lblSelected.Font = new Font("Segoe UI", 10f);
            lblSelected.ForeColor = Color.FromArgb(41, 128, 185);
            lblSelected.TextAlign = ContentAlignment.MiddleLeft;

            pnlBottom.Controls.Add(lblSelected);

            Controls.AddRange(new Control[]
            {
                pnlMain, pnlBottom, pnlTop
            });

            ResumeLayout(false);
        }

        private Panel pnlTop;
        private Panel pnlMain;
        private Panel pnlBottom;
        private Button btnRoomsBasic;
        private Button btnRoomsAll;
        private Button btnReservations;
        private Button btnClear;
        private Label lblTitle;
        private Label lblStatus;
        private Label lblSelected;
        private DataGridView dgvDynamic;
    }
}