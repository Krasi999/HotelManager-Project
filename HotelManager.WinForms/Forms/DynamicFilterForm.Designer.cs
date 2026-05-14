using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManager.WinForms.Forms
{
    partial class DynamicFilterForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            pnlTop = new Panel();
            lblTitle = new Label();
            btnFilterRooms = new Button();
            btnFilterGuests = new Button();
            pnlFilters = new Panel();
            lblFiltersTitle = new Label();
            pnlResults = new Panel();
            dgvResults = new DataGridView();
            lblResultStatus = new Label();
            pnlTop.SuspendLayout();
            pnlFilters.SuspendLayout();
            pnlResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.White;
            pnlTop.BorderStyle = BorderStyle.FixedSingle;
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(btnFilterRooms);
            pnlTop.Controls.Add(btnFilterGuests);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(12, 0, 0, 0);
            pnlTop.Size = new Size(698, 60);
            pnlTop.TabIndex = 2;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.Location = new Point(12, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(70, 20);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Търси в:";
            // 
            // btnFilterRooms
            // 
            btnFilterRooms.Location = new Point(90, 12);
            btnFilterRooms.Name = "btnFilterRooms";
            btnFilterRooms.Size = new Size(75, 23);
            btnFilterRooms.TabIndex = 1;
            // 
            // btnFilterGuests
            // 
            btnFilterGuests.Location = new Point(226, 12);
            btnFilterGuests.Name = "btnFilterGuests";
            btnFilterGuests.Size = new Size(75, 23);
            btnFilterGuests.TabIndex = 2;
            // 
            // pnlFilters
            // 
            pnlFilters.AutoScroll = true;
            pnlFilters.BackColor = Color.White;
            pnlFilters.BorderStyle = BorderStyle.FixedSingle;
            pnlFilters.Controls.Add(lblFiltersTitle);
            pnlFilters.Location = new Point(0, 68);
            pnlFilters.Name = "pnlFilters";
            pnlFilters.Padding = new Padding(16);
            pnlFilters.Size = new Size(280, 600);
            pnlFilters.TabIndex = 1;
            // 
            // lblFiltersTitle
            // 
            lblFiltersTitle.Dock = DockStyle.Top;
            lblFiltersTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblFiltersTitle.Location = new Point(16, 16);
            lblFiltersTitle.Name = "lblFiltersTitle";
            lblFiltersTitle.Size = new Size(246, 36);
            lblFiltersTitle.TabIndex = 0;
            lblFiltersTitle.Text = "Филтри";
            // 
            // pnlResults
            // 
            pnlResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlResults.BackColor = Color.White;
            pnlResults.BorderStyle = BorderStyle.FixedSingle;
            pnlResults.Controls.Add(dgvResults);
            pnlResults.Controls.Add(lblResultStatus);
            pnlResults.Location = new Point(296, 68);
            pnlResults.Name = "pnlResults";
            pnlResults.Padding = new Padding(12);
            pnlResults.Size = new Size(778, 963);
            pnlResults.TabIndex = 0;
            // 
            // dgvResults
            // 
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.BackgroundColor = Color.White;
            dgvResults.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(236, 240, 241);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvResults.ColumnHeadersHeight = 36;
            dgvResults.Dock = DockStyle.Fill;
            dgvResults.EnableHeadersVisualStyles = false;
            dgvResults.Font = new Font("Segoe UI", 9.5F);
            dgvResults.Location = new Point(12, 40);
            dgvResults.Name = "dgvResults";
            dgvResults.ReadOnly = true;
            dgvResults.RowHeadersVisible = false;
            dgvResults.RowTemplate.Height = 34;
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResults.Size = new Size(752, 909);
            dgvResults.TabIndex = 0;
            // 
            // lblResultStatus
            // 
            lblResultStatus.Dock = DockStyle.Top;
            lblResultStatus.Font = new Font("Segoe UI", 9F);
            lblResultStatus.ForeColor = Color.FromArgb(41, 128, 185);
            lblResultStatus.Location = new Point(12, 12);
            lblResultStatus.Name = "lblResultStatus";
            lblResultStatus.Size = new Size(752, 28);
            lblResultStatus.TabIndex = 1;
            // 
            // DynamicFilterForm
            // 
            BackColor = Color.FromArgb(245, 246, 250);
            ClientSize = new Size(698, 863);
            Controls.Add(pnlResults);
            Controls.Add(pnlFilters);
            Controls.Add(pnlTop);
            Font = new Font("Segoe UI", 9.5F);
            Name = "DynamicFilterForm";
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlFilters.ResumeLayout(false);
            pnlResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            ResumeLayout(false);
        }

        private Panel pnlTop;
        private Panel pnlFilters;
        private Panel pnlResults;
        private Button btnFilterRooms;
        private Button btnFilterGuests;
        private Label lblTitle;
        private Label lblFiltersTitle;
        private Label lblResultStatus;
        private DataGridView dgvResults;
    }
}
