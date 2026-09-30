
using System;
using System.Drawing;
using System.Windows.Forms;

namespace library_system
{
    public class MainMenuForm : Form
    {
        private Panel sidebar = null!;
        private Panel contentPanel = null!;

        private Label lblPageTitle = null!;
        private Label lblWelcome = null!;

        public MainMenuForm(Librarian librarian)
        {
            this.librarian = librarian;
            CreateUI();
            lblWelcome.Text = $"Welcome, {librarian.Name}";
            Shown += (_, _) => RefreshDashboard();
        }

        private void CreateUI()
        {
            // =========================
            // FORM
            // =========================

            this.Text = "Library Management System";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // =========================
            // SIDEBAR
            // =========================

            sidebar = new Panel();
            sidebar.Size = new Size(230, 700);
            sidebar.Location = new Point(0, 0);
            sidebar.BackColor = Color.FromArgb(31, 41, 55);

            this.Controls.Add(sidebar);

            // Logo
            Label lblLogo = new Label();
            lblLogo.Text = "LIBRARY";
            lblLogo.Font = new Font(
                "Segoe UI",
                20,
                FontStyle.Bold
            );
            lblLogo.ForeColor = Color.White;
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(35, 30);

            sidebar.Controls.Add(lblLogo);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Management System";
            lblSubtitle.Font = new Font(
                "Segoe UI",
                9
            );
            lblSubtitle.ForeColor = Color.FromArgb(156, 163, 175);
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(38, 67);

            sidebar.Controls.Add(lblSubtitle);

            // =========================
            // MENU BUTTONS
            // =========================

            int buttonY = 130;

            Button btnDashboard = CreateSidebarButton(
                "Dashboard",
                buttonY
            );

            btnDashboard.Click += (_, _) => RefreshDashboard();
            sidebar.Controls.Add(btnDashboard);

            buttonY += 55;

            Button btnBooks = CreateSidebarButton(
                "Books",
                buttonY
            );

            btnBooks.Click += BtnBooks_Click;
            sidebar.Controls.Add(btnBooks);

            buttonY += 55;

            Button btnCategories = CreateSidebarButton(
                "Categories",
                buttonY
            );

            btnCategories.Click += BtnCategories_Click;
            sidebar.Controls.Add(btnCategories);

            buttonY += 55;

            Button btnMembers = CreateSidebarButton(
                "Members",
                buttonY
            );

            btnMembers.Click += BtnMembers_Click;
            sidebar.Controls.Add(btnMembers);

            buttonY += 55;

            Button btnBorrow = CreateSidebarButton(
                "Borrow Books",
                buttonY
            );

            btnBorrow.Click += BtnBorrow_Click;
            sidebar.Controls.Add(btnBorrow);

            buttonY += 55;

            Button btnReturn = CreateSidebarButton(
                "Return Books",
                buttonY
            );

            btnReturn.Click += BtnReturn_Click;
            sidebar.Controls.Add(btnReturn);

            buttonY += 55;

            Button btnHistory = CreateSidebarButton(
                "History",
                buttonY
            );

            btnHistory.Click += BtnHistory_Click;
            sidebar.Controls.Add(btnHistory);

            // =========================
            // LOGOUT
            // =========================

            Button btnLogout = new Button();

            btnLogout.Text = "Logout";
            btnLogout.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnLogout.ForeColor = Color.White;
            btnLogout.BackColor = Color.FromArgb(220, 53, 69);

            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;

            btnLogout.Size = new Size(180, 42);
            btnLogout.Location = new Point(25, 590);

            btnLogout.Cursor = Cursors.Hand;

            btnLogout.Click += BtnLogout_Click;

            sidebar.Controls.Add(btnLogout);

            // =========================
            // MAIN CONTENT
            // =========================

            contentPanel = new Panel();

            contentPanel.Location = new Point(230, 0);
            contentPanel.Size = new Size(970, 700);
            contentPanel.BackColor = Color.FromArgb(245, 247, 250);

            this.Controls.Add(contentPanel);

            // Page title

            lblPageTitle = new Label();

            lblPageTitle.Text = "Dashboard";

            lblPageTitle.Font = new Font(
                "Segoe UI",
                24,
                FontStyle.Bold
            );

            lblPageTitle.ForeColor =
                Color.FromArgb(31, 41, 55);

            lblPageTitle.AutoSize = true;

            lblPageTitle.Location =
                new Point(40, 35);

            contentPanel.Controls.Add(lblPageTitle);

            // Welcome

            lblWelcome = new Label();

            lblWelcome.Text =
                "Welcome to the Library Management System";

            lblWelcome.Font =
                new Font("Segoe UI", 11);

            lblWelcome.ForeColor =
                Color.FromArgb(107, 114, 128);

            lblWelcome.AutoSize = true;

            lblWelcome.Location =
                new Point(42, 78);

            contentPanel.Controls.Add(lblWelcome);

            // =========================
            // DASHBOARD CARDS
            // =========================

            CreateCard(
                "Books",
                "Manage library books",
                40,
                130
            );

            CreateCard(
                "Categories",
                "Manage book categories",
                340,
                130
            );

            CreateCard(
                "Members",
                "Manage library members",
                640,
                130
            );

            CreateCard(
                "Borrow",
                "Record borrowed books",
                40,
                300
            );

            CreateCard(
                "Return",
                "Process returned books",
                340,
                300
            );

            CreateCard(
                "History",
                "View borrowing history",
                640,
                300
            );
        }

        // =========================
        // SIDEBAR BUTTON
        // =========================

        private Button CreateSidebarButton(
            string text,
            int y
        )
        {
            Button button = new Button();

            button.Text = text;

            button.Font =
                new Font("Segoe UI", 10);

            button.ForeColor =
                Color.FromArgb(229, 231, 235);

            button.BackColor =
                Color.FromArgb(31, 41, 55);

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.TextAlign =
                ContentAlignment.MiddleLeft;

            button.Padding =
                new Padding(15, 0, 0, 0);

            button.Size =
                new Size(210, 45);

            button.Location =
                new Point(10, y);

            button.Cursor =
                Cursors.Hand;

            return button;
        }

        // =========================
        // DASHBOARD CARD
        // =========================

        private void CreateCard(
            string title,
            string description,
            int x,
            int y
        )
        {
            Panel card = new Panel();

            card.Size =
                new Size(270, 130);

            card.Location =
                new Point(x, y);

            card.BackColor =
                Color.White;

            card.BorderStyle =
                BorderStyle.FixedSingle;

            contentPanel.Controls.Add(card);

            Label lblTitle =
                new Label();

            lblTitle.Text =
                title;

            lblTitle.Font =
                new Font(
                    "Segoe UI",
                    14,
                    FontStyle.Bold
                );

            lblTitle.ForeColor =
                Color.FromArgb(31, 41, 55);

            lblTitle.AutoSize =
                true;

            lblTitle.Location =
                new Point(20, 20);

            card.Controls.Add(lblTitle);

            Label lblDescription =
                new Label();

            lblDescription.Text =
                description;

            lblDescription.Font =
                new Font(
                    "Segoe UI",
                    9
                );

            lblDescription.ForeColor =
                Color.FromArgb(107, 114, 128);

            lblDescription.AutoSize =
                true;

            lblDescription.Location =
                new Point(20, 55);

            dashboardLabels[title] = lblDescription;
            card.Controls.Add(lblDescription);

            Button btnOpen =
                new Button();

            btnOpen.Text =
                "Open";

            btnOpen.Font =
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold
                );

            btnOpen.ForeColor =
                Color.White;

            btnOpen.BackColor =
                Color.FromArgb(
                    37,
                    99,
                    235
                );

            btnOpen.FlatStyle =
                FlatStyle.Flat;

            btnOpen.FlatAppearance.BorderSize =
                0;

            btnOpen.Size =
                new Size(80, 30);

            btnOpen.Location =
                new Point(170, 85);

            btnOpen.Cursor =
                Cursors.Hand;

            card.Controls.Add(btnOpen);

            // Connect card buttons

            if (title == "Books")
                btnOpen.Click += BtnBooks_Click;

            else if (title == "Categories")
                btnOpen.Click += BtnCategories_Click;

            else if (title == "Members")
                btnOpen.Click += BtnMembers_Click;

            else if (title == "Borrow")
                btnOpen.Click += BtnBorrow_Click;

            else if (title == "Return")
                btnOpen.Click += BtnReturn_Click;

            else if (title == "History")
                btnOpen.Click += BtnHistory_Click;
        }

        // =========================
        // BUTTON EVENTS
        // =========================

        private readonly Librarian librarian;
        private readonly Dictionary<string, Label> dashboardLabels = new();

        private void RefreshDashboard()
        {
            foreach (var label in dashboardLabels.Values) label.Text = "Loading...";
            UiAction.Run(this, () =>
            {
                var values = new HistoryService().GetDashboard();
                foreach (var item in values) dashboardLabels[item.Key].Text = item.Value;
            });
        }

        private void OpenModule(Form form)
        {
            using (form) form.ShowDialog(this);
            RefreshDashboard();
        }

        private void BtnBooks_Click(object? sender, EventArgs e) => OpenModule(new BookForm());
        private void BtnCategories_Click(object? sender, EventArgs e) => OpenModule(new CategoriesForm());
        private void BtnMembers_Click(object? sender, EventArgs e) => OpenModule(new MemberForm());
        private void BtnBorrow_Click(object? sender, EventArgs e) => OpenModule(new BorrowForm(librarian));
        private void BtnReturn_Click(object? sender, EventArgs e) => OpenModule(new ReturnForm());
        private void BtnHistory_Click(object? sender, EventArgs e) => OpenModule(new HistoryForm());

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            if (!UiAction.Confirm(this, "Are you sure you want to logout?")) return;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}