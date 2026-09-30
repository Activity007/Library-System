using System;
using System.Drawing;
using System.Windows.Forms;

namespace library_system
{
    public class ReturnForm : Form
    {
        private TextBox txtBorrowID = null!;
        private Label lblMember = null!;
        private Label lblBorrowDate = null!;
        private Label lblDueDate = null!;
        private Label lblFine = null!;

        private DateTimePicker dtReturnDate = null!;

        private Button btnSearch = null!;
        private Button btnReturn = null!;
        private Button btnClear = null!;

        private DataGridView dgvBorrowedBooks = null!;

        public ReturnForm()
        {
            CreateUI();
            dtReturnDate.MaxDate = DateTime.Today;
            dgvBorrowedBooks.MultiSelect = false;
            dgvBorrowedBooks.SelectionChanged += (_, _) => CalculateFine();
            dgvBorrowedBooks.Columns["DueDate"]!.DefaultCellStyle.Format = "yyyy-MM-dd";
            txtBorrowID.TextChanged += (_, _) => ClearDetails();
            btnReturn.Text = "Return selected";
        }

        private void CreateUI()
        {
            this.Text = "Return Books";
            this.Size = new Size(1100, 700);
            this.StartPosition =
                FormStartPosition.CenterScreen;
            this.FormBorderStyle =
                FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor =
                Color.FromArgb(
                    245,
                    247,
                    250
                );

            // HEADER
            Panel header =
                new Panel();

            header.Size =
                new Size(1100, 100);

            header.BackColor =
                Color.FromArgb(
                    31,
                    41,
                    55
                );

            this.Controls.Add(
                header
            );

            Label title =
                new Label();

            title.Text =
                "RETURN BOOK";

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
                new Point(
                    35,
                    22
                );

            header.Controls.Add(
                title
            );

            Label subtitle =
                new Label();

            subtitle.Text =
                "Process returned books and calculate fines";

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
                new Point(
                    38,
                    62
                );

            header.Controls.Add(
                subtitle
            );

            // SEARCH PANEL
            Panel searchPanel =
                new Panel();

            searchPanel.Location =
                new Point(
                    30,
                    120
                );

            searchPanel.Size =
                new Size(
                    1040,
                    150
                );

            searchPanel.BackColor =
                Color.White;

            searchPanel.BorderStyle =
                BorderStyle.FixedSingle;

            this.Controls.Add(
                searchPanel
            );

            AddLabel(
                searchPanel,
                "Borrow ID",
                25,
                20
            );

            txtBorrowID =
                new TextBox();

            txtBorrowID.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            txtBorrowID.Size =
                new Size(
                    220,
                    30
                );

            txtBorrowID.Location =
                new Point(
                    25,
                    45
                );

            searchPanel.Controls.Add(
                txtBorrowID
            );

            btnSearch =
                CreateButton(
                    "SEARCH",
                    265,
                    40,
                    Color.FromArgb(
                        37,
                        99,
                        235
                    )
                );

            btnSearch.Click +=
                BtnSearch_Click;

            searchPanel.Controls.Add(
                btnSearch
            );

            // MEMBER
            AddLabel(
                searchPanel,
                "Member",
                450,
                20
            );

            lblMember =
                CreateValueLabel(
                    "Not selected",
                    450,
                    45
                );

            searchPanel.Controls.Add(
                lblMember
            );

            // BORROW DATE
            AddLabel(
                searchPanel,
                "Borrow Date",
                700,
                20
            );

            lblBorrowDate =
                CreateValueLabel(
                    "-",
                    700,
                    45
                );

            searchPanel.Controls.Add(
                lblBorrowDate
            );

            // DUE DATE
            AddLabel(
                searchPanel,
                "Due Date",
                450,
                90
            );

            lblDueDate =
                CreateValueLabel(
                    "-",
                    450,
                    115
                );

            searchPanel.Controls.Add(
                lblDueDate
            );

            // RETURN DATE
            AddLabel(
                searchPanel,
                "Return Date",
                700,
                90
            );

            dtReturnDate =
                new DateTimePicker();

            dtReturnDate.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            dtReturnDate.Format =
                DateTimePickerFormat.Short;

            dtReturnDate.Size =
                new Size(
                    180,
                    30
                );

            dtReturnDate.Location =
                new Point(
                    700,
                    112
                );

            dtReturnDate.Value =
                DateTime.Today;

            dtReturnDate.ValueChanged +=
                DtReturnDate_ValueChanged;

            searchPanel.Controls.Add(
                dtReturnDate
            );

            // TABLE PANEL
            Panel tablePanel =
                new Panel();

            tablePanel.Location =
                new Point(
                    30,
                    290
                );

            tablePanel.Size =
                new Size(
                    1040,
                    230
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
                "Borrowed Books";

            tableTitle.Font =
                new Font(
                    "Segoe UI",
                    11,
                    FontStyle.Bold
                );

            tableTitle.AutoSize =
                true;

            tableTitle.Location =
                new Point(
                    20,
                    15
                );

            tablePanel.Controls.Add(
                tableTitle
            );

            dgvBorrowedBooks =
                new DataGridView();

            dgvBorrowedBooks.Location =
                new Point(
                    20,
                    50
                );

            dgvBorrowedBooks.Size =
                new Size(
                    990,
                    155
                );

            dgvBorrowedBooks.BackgroundColor =
                Color.White;

            dgvBorrowedBooks.BorderStyle =
                BorderStyle.None;

            dgvBorrowedBooks.AllowUserToAddRows =
                false;

            dgvBorrowedBooks.ReadOnly =
                true;

            dgvBorrowedBooks.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvBorrowedBooks.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvBorrowedBooks.RowHeadersVisible =
                false;

            dgvBorrowedBooks.ColumnHeadersDefaultCellStyle =
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

            dgvBorrowedBooks.EnableHeadersVisualStyles =
                false;

            tablePanel.Controls.Add(
                dgvBorrowedBooks
            );

            dgvBorrowedBooks.Columns.Add(
                "BookID",
                "Book ID"
            );

            dgvBorrowedBooks.Columns.Add(
                "Book",
                "Book"
            );

            dgvBorrowedBooks.Columns.Add(
                "DueDate",
                "Due Date"
            );

            dgvBorrowedBooks.Columns.Add(
                "Status",
                "Status"
            );

            // BOTTOM
            Panel bottom =
                new Panel();

            bottom.Location =
                new Point(
                    30,
                    540
                );

            bottom.Size =
                new Size(
                    1040,
                    80
                );

            bottom.BackColor =
                Color.White;

            bottom.BorderStyle =
                BorderStyle.FixedSingle;

            this.Controls.Add(
                bottom
            );

            Label fineText =
                new Label();

            fineText.Text =
                "Fine:";

            fineText.Font =
                new Font(
                    "Segoe UI",
                    12,
                    FontStyle.Bold
                );

            fineText.AutoSize = true;

            fineText.Location =
                new Point(
                    25,
                    22
                );

            bottom.Controls.Add(
                fineText
            );

            lblFine =
                CreateValueLabel(
                    "$0.00",
                    80,
                    22
                );

            lblFine.Font =
                new Font(
                    "Segoe UI",
                    12,
                    FontStyle.Bold
                );

            lblFine.ForeColor =
                Color.FromArgb(
                    220,
                    53,
                    69
                );

            bottom.Controls.Add(
                lblFine
            );

            btnClear =
                CreateButton(
                    "CLEAR",
                    650,
                    18,
                    Color.FromArgb(
                        107,
                        114,
                        128
                    )
                );

            btnClear.Click +=
                BtnClear_Click;

            bottom.Controls.Add(
                btnClear
            );

            btnReturn =
                CreateButton(
                    "RETURN BOOK",
                    770,
                    18,
                    Color.FromArgb(
                        16,
                        185,
                        129
                    )
                );

            btnReturn.Size =
                new Size(
                    140,
                    40
                );

            btnReturn.Click +=
                BtnReturn_Click;

            bottom.Controls.Add(
                btnReturn
            );
        }

        private void AddLabel(
            Panel panel,
            string text,
            int x,
            int y)
        {
            Label label =
                new Label();

            label.Text = text;

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

        private Label CreateValueLabel(
            string text,
            int x,
            int y)
        {
            Label label =
                new Label();

            label.Text = text;

            label.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            label.ForeColor =
                Color.FromArgb(
                    31,
                    41,
                    55
                );

            label.AutoSize = true;

            label.Location =
                new Point(
                    x,
                    y
                );

            return label;
        }

        private Button CreateButton(
            string text,
            int x,
            int y,
            Color color)
        {
            Button button =
                new Button();

            button.Text = text;

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
                    110,
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

        private int? loadedBorrowID;

        private void LoadBorrow()
        {
            if (!int.TryParse(txtBorrowID.Text.Trim(), out int id) || id <= 0)
                throw new InvalidOperationException("Enter a valid Borrow ID.");
            var loans = new HistoryService().GetLoans(borrowID: id);
            ClearDetails();
            if (loans.Count == 0) throw new InvalidOperationException("Borrowing transaction not found.");
            loadedBorrowID = id;
            lblMember.Text = loans[0].MemberName;
            lblBorrowDate.Text = loans[0].BorrowDate.ToString("yyyy-MM-dd");
            lblDueDate.Text = loans[0].DueDate.ToString("yyyy-MM-dd");
            foreach (var loan in loans.Where(loan => !loan.ReturnDate.HasValue))
            {
                int index = dgvBorrowedBooks.Rows.Add(loan.BookID, loan.BookTitle, loan.DueDate, loan.Status);
                dgvBorrowedBooks.Rows[index].Tag = loan;
            }
            if (dgvBorrowedBooks.Rows.Count > 0)
                dgvBorrowedBooks.CurrentCell = dgvBorrowedBooks.Rows[0].Cells[0];
            CalculateFine();
            if (dgvBorrowedBooks.Rows.Count == 0)
                MessageBox.Show(this, "All books in this transaction have been returned.", "Returns");
        }

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            ClearDetails();
            UiAction.Run(this, LoadBorrow);
        }

        private void DtReturnDate_ValueChanged(object? sender, EventArgs e) => CalculateFine();

        private void CalculateFine()
        {
            if (lblFine == null || dgvBorrowedBooks == null) return;
            var loan = dgvBorrowedBooks.CurrentRow?.Tag as LoanRecord;
            lblFine.Text = "$" + (loan == null ? 0m :
                LoanRules.CalculateFine(loan.DueDate, dtReturnDate.Value)).ToString("0.00");
        }

        private void BtnReturn_Click(object? sender, EventArgs e)
        {
            btnReturn.Enabled = false;
            try
            {
                UiAction.Run(this, () =>
                {
                    if (dgvBorrowedBooks.CurrentRow?.Tag is not LoanRecord loan || loadedBorrowID == null)
                        throw new InvalidOperationException("Search for a borrowing transaction and select a book.");
                    decimal fine = new ReturnService().ReturnBook(loan.BorrowDetailID, dtReturnDate.Value.Date);
                    txtBorrowID.Text = loadedBorrowID.Value.ToString();
                    MessageBox.Show(this, $"Book returned. Fine: ${fine:0.00}", "Success");
                    LoadBorrow();
                });
            }
            finally { btnReturn.Enabled = true; }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            txtBorrowID.Clear();
            ClearDetails();
        }

        private void ClearDetails()
        {
            loadedBorrowID = null;
            lblMember.Text = "Not selected";
            lblBorrowDate.Text = "-";
            lblDueDate.Text = "-";
            lblFine.Text = "$0.00";
            dgvBorrowedBooks.Rows.Clear();
        }
    }
}
