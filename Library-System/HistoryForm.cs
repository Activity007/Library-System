using System;
using System.Drawing;
using System.Windows.Forms;

namespace library_system
{
    public class HistoryForm : Form
    {
        private TextBox txtSearch = null!;
        private ComboBox cmbStatus = null!;
        private DateTimePicker dtFrom = null!;
        private DateTimePicker dtTo = null!;

        private Button btnSearch = null!;
        private Button btnClear = null!;

        private DataGridView dgvHistory = null!;

        public HistoryForm()
        {
            CreateUI();
            dgvHistory.Columns.Add("Librarian", "Librarian");
            foreach (string name in new[] { "BorrowDate", "DueDate", "ReturnDate" })
                dgvHistory.Columns[name]!.DefaultCellStyle.Format = "yyyy-MM-dd";
            dgvHistory.Columns["Fine"]!.DefaultCellStyle.Format = "0.00";
            Shown += (_, _) => UiAction.Run(this, LoadHistory);
        }

        private void CreateUI()
        {
            // =========================
            // FORM
            // =========================

            this.Text = "Borrow / Return History";
            this.Size = new Size(1200, 700);
            this.StartPosition =
                FormStartPosition.CenterScreen;

            this.FormBorderStyle =
                FormBorderStyle.FixedSingle;

            this.MaximizeBox = false;

            this.BackColor =
                Color.FromArgb(245, 247, 250);

            // =========================
            // HEADER
            // =========================

            Panel header =
                new Panel();

            header.Size =
                new Size(1200, 100);

            header.Location =
                new Point(0, 0);

            header.BackColor =
                Color.FromArgb(31, 41, 55);

            this.Controls.Add(header);

            Label title =
                new Label();

            title.Text =
                "BORROW / RETURN HISTORY";

            title.Font =
                new Font(
                    "Segoe UI",
                    22,
                    FontStyle.Bold
                );

            title.ForeColor =
                Color.White;

            title.AutoSize = true;

            title.Location =
                new Point(35, 22);

            header.Controls.Add(title);

            Label subtitle =
                new Label();

            subtitle.Text =
                "View and search library borrowing transactions";

            subtitle.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            subtitle.ForeColor =
                Color.FromArgb(
                    209,
                    213,
                    219
                );

            subtitle.AutoSize = true;

            subtitle.Location =
                new Point(38, 62);

            header.Controls.Add(subtitle);

            // =========================
            // FILTER PANEL
            // =========================

            Panel filterPanel =
                new Panel();

            filterPanel.Location =
                new Point(30, 120);

            filterPanel.Size =
                new Size(1140, 150);

            filterPanel.BackColor =
                Color.White;

            filterPanel.BorderStyle =
                BorderStyle.FixedSingle;

            this.Controls.Add(filterPanel);

            // SEARCH
            AddLabel(
                filterPanel,
                "Search",
                20,
                20
            );

            txtSearch =
                new TextBox();

            txtSearch.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            txtSearch.Size =
                new Size(
                    260,
                    30
                );

            txtSearch.Location =
                new Point(
                    20,
                    45
                );

            filterPanel.Controls.Add(
                txtSearch
            );

            // STATUS
            AddLabel(
                filterPanel,
                "Status",
                310,
                20
            );

            cmbStatus =
                new ComboBox();

            cmbStatus.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            cmbStatus.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbStatus.Size =
                new Size(
                    180,
                    30
                );

            cmbStatus.Location =
                new Point(
                    310,
                    45
                );

            cmbStatus.Items.Add("All");
            cmbStatus.Items.Add("Borrowed");
            cmbStatus.Items.Add("Returned");
            cmbStatus.Items.Add("Overdue");

            cmbStatus.SelectedIndex = 0;

            filterPanel.Controls.Add(
                cmbStatus
            );

            // FROM DATE
            AddLabel(
                filterPanel,
                "From Date",
                520,
                20
            );

            dtFrom =
                new DateTimePicker();

            dtFrom.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            dtFrom.Format =
                DateTimePickerFormat.Short;

            dtFrom.Size =
                new Size(
                    160,
                    30
                );

            dtFrom.Location =
                new Point(
                    520,
                    45
                );

            dtFrom.Value =
                DateTime.Today.AddMonths(-1);

            filterPanel.Controls.Add(
                dtFrom
            );

            // TO DATE
            AddLabel(
                filterPanel,
                "To Date",
                710,
                20
            );

            dtTo =
                new DateTimePicker();

            dtTo.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            dtTo.Format =
                DateTimePickerFormat.Short;

            dtTo.Size =
                new Size(
                    160,
                    30
                );

            dtTo.Location =
                new Point(
                    710,
                    45
                );

            dtTo.Value =
                DateTime.Today;

            filterPanel.Controls.Add(
                dtTo
            );

            // SEARCH BUTTON
            btnSearch =
                CreateButton(
                    "SEARCH",
                    895,
                    40,
                    Color.FromArgb(
                        37,
                        99,
                        235
                    )
                );

            btnSearch.Click +=
                BtnSearch_Click;

            filterPanel.Controls.Add(
                btnSearch
            );

            // CLEAR BUTTON
            btnClear =
                CreateButton(
                    "CLEAR",
                    1015,
                    40,
                    Color.FromArgb(
                        107,
                        114,
                        128
                    )
                );

            btnClear.Click +=
                BtnClear_Click;

            filterPanel.Controls.Add(
                btnClear
            );

            // =========================
            // HISTORY TABLE
            // =========================

            Panel tablePanel =
                new Panel();

            tablePanel.Location =
                new Point(
                    30,
                    290
                );

            tablePanel.Size =
                new Size(
                    1140,
                    330
                );

            tablePanel.BackColor =
                Color.White;

            tablePanel.BorderStyle =
                BorderStyle.FixedSingle;

            this.Controls.Add(
                tablePanel
            );

            Label tableTitle =
                new Label();

            tableTitle.Text =
                "Transaction History";

            tableTitle.Font =
                new Font(
                    "Segoe UI",
                    11,
                    FontStyle.Bold
                );

            tableTitle.ForeColor =
                Color.FromArgb(
                    31,
                    41,
                    55
                );

            tableTitle.AutoSize = true;

            tableTitle.Location =
                new Point(
                    20,
                    15
                );

            tablePanel.Controls.Add(
                tableTitle
            );

            // =========================
            // DATA GRID
            // =========================

            dgvHistory =
                new DataGridView();

            dgvHistory.Location =
                new Point(
                    20,
                    50
                );

            dgvHistory.Size =
                new Size(
                    1095,
                    255
                );

            dgvHistory.BackgroundColor =
                Color.White;

            dgvHistory.BorderStyle =
                BorderStyle.None;

            dgvHistory.AllowUserToAddRows =
                false;

            dgvHistory.AllowUserToDeleteRows =
                false;

            dgvHistory.ReadOnly =
                true;

            dgvHistory.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvHistory.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvHistory.RowHeadersVisible =
                false;

            dgvHistory.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(
                            31,
                            41,
                            55
                        ),

                    ForeColor =
                        Color.White,

                    Font =
                        new Font(
                            "Segoe UI",
                            9,
                            FontStyle.Bold
                        )
                };

            dgvHistory.EnableHeadersVisualStyles =
                false;

            tablePanel.Controls.Add(
                dgvHistory
            );

            // =========================
            // COLUMNS
            // =========================

            dgvHistory.Columns.Add(
                "BorrowID",
                "Borrow ID"
            );

            dgvHistory.Columns.Add(
                "Member",
                "Member"
            );

            dgvHistory.Columns.Add(
                "Book",
                "Book"
            );

            dgvHistory.Columns.Add(
                "BorrowDate",
                "Borrow Date"
            );

            dgvHistory.Columns.Add(
                "DueDate",
                "Due Date"
            );

            dgvHistory.Columns.Add(
                "ReturnDate",
                "Return Date"
            );

            dgvHistory.Columns.Add(
                "Fine",
                "Fine"
            );

            dgvHistory.Columns.Add(
                "Status",
                "Status"
            );

            // =========================
            // TEMPORARY DATA
            // =========================


        }

        // =========================
        // LABEL
        // =========================

        private void AddLabel(
            Panel panel,
            string text,
            int x,
            int y)
        {
            Label label =
                new Label();

            label.Text =
                text;

            label.Font =
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold
                );

            label.ForeColor =
                Color.FromArgb(
                    55,
                    65,
                    81
                );

            label.AutoSize = true;

            label.Location =
                new Point(
                    x,
                    y
                );

            panel.Controls.Add(
                label
            );
        }

        // =========================
        // BUTTON
        // =========================

        private Button CreateButton(
            string text,
            int x,
            int y,
            Color color)
        {
            Button button =
                new Button();

            button.Text =
                text;

            button.Font =
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold
                );

            button.ForeColor =
                Color.White;

            button.BackColor =
                color;

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize =
                0;

            button.Size =
                new Size(
                    100,
                    40
                );

            button.Location =
                new Point(
                    x,
                    y
                );

            button.Cursor =
                Cursors.Hand;

            return button;
        }

        // =========================
        // TEMPORARY DATA
        // =========================

        private void LoadHistory()
        {
            if (dtFrom.Value.Date > dtTo.Value.Date)
                throw new InvalidOperationException("From date must be on or before To date.");
            var loans = new HistoryService().GetLoans(txtSearch.Text, cmbStatus.Text, dtFrom.Value.Date, dtTo.Value.Date);
            dgvHistory.Rows.Clear();
            foreach (var loan in loans)
            {
                decimal fine = loan.ReturnDate.HasValue ? loan.Fine : LoanRules.CalculateFine(loan.DueDate, DateTime.Today);
                dgvHistory.Rows.Add(loan.BorrowID, loan.MemberName, loan.BookTitle,
                    loan.BorrowDate, loan.DueDate, (object?)loan.ReturnDate ?? DBNull.Value, fine, loan.Status, loan.LibrarianName);
            }
            dgvHistory.ClearSelection();
        }

        private void BtnSearch_Click(object? sender, EventArgs e) => UiAction.Run(this, LoadHistory);

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbStatus.SelectedIndex = 0;
            dtFrom.Value = DateTime.Today.AddMonths(-1);
            dtTo.Value = DateTime.Today;
            UiAction.Run(this, LoadHistory);
        }
    }
}
