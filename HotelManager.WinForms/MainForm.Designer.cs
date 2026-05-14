namespace HotelManager.WinForms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pnlSidebar = new Panel();
            pnlContent = new Panel();
            pnlHeader = new Panel();
            lblTitle = new Label();
            lblDatabase = new Label();
            btnRooms = new Button();
            btnGuests = new Button();
            btnReservations = new Button();
            btnDynamicFilter = new Button();
            btnDynamicList = new Button();

            SuspendLayout();

            ClientSize = new Size(1200, 750);
            Text = "Hotel Manager";
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 246, 250);
            Font = new Font("Segoe UI", 9.5f);

            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 60;
            pnlHeader.BackColor = Color.FromArgb(44, 62, 80);
            pnlHeader.Controls.Add(lblDatabase);
            pnlHeader.Controls.Add(lblTitle);

            lblTitle.Text = "🏨  Hotel Manager";
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            lblTitle.Location = new Point(20, 12);
            lblTitle.AutoSize = true;

            lblDatabase.ForeColor = Color.FromArgb(189, 195, 199);
            lblDatabase.Font = new Font("Segoe UI", 9f);
            lblDatabase.AutoSize = true;
            lblDatabase.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            lblDatabase.Location = new Point(1080, 22);

            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Width = 200;
            pnlSidebar.BackColor = Color.FromArgb(52, 73, 94);
            pnlSidebar.Controls.AddRange(new Control[]
            {
                btnRooms, btnGuests, btnReservations,
                btnDynamicFilter, btnDynamicList
            });

            SetupSidebarButton(btnRooms, "🛏  Стаи", 0);
            SetupSidebarButton(btnGuests, "👤  Гости", 1);
            SetupSidebarButton(btnReservations, "📋  Резервации", 2);
            SetupSidebarButton(btnDynamicFilter, "🔍  Динамични филтри", 3);
            SetupSidebarButton(btnDynamicList, "📊  Динамичен списък", 4);

            btnRooms.Click += btnRooms_Click;
            btnGuests.Click += btnGuests_Click;
            btnReservations.Click += btnReservations_Click;
            btnDynamicFilter.Click += btnDynamicFilter_Click;
            btnDynamicList.Click += btnDynamicList_Click;

            pnlContent.Dock = DockStyle.Fill;
            pnlContent.BackColor = Color.FromArgb(245, 246, 250);
            pnlContent.Padding = new Padding(16);

            Controls.AddRange(new Control[]
            {
                pnlContent, pnlSidebar, pnlHeader
            });

            ResumeLayout(false);
        }

        private static void SetupSidebarButton(Button btn, string text, int index)
        {
            btn.Text = text;
            btn.Dock = DockStyle.Top;
            btn.Height = 52;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(44, 62, 80);
            btn.BackColor = Color.Transparent;
            btn.ForeColor = Color.FromArgb(189, 195, 199);
            btn.Font = new Font("Segoe UI", 10f);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(16, 0, 0, 0);
            btn.Cursor = Cursors.Hand;
            btn.TabIndex = index;
        }

        private Panel pnlSidebar;
        private Panel pnlContent;
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblDatabase;
        private Button btnRooms;
        private Button btnGuests;
        private Button btnReservations;
        private Button btnDynamicFilter;
        private Button btnDynamicList;
    }
}