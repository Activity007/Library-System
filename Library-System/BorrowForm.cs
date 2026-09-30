using System;
using System.Drawing;
using System.Windows.Forms;

namespace library_system
{
    public class BorrowForm : Form
    {
        private ComboBox cmbMember = null!;
        private ComboBox cmbBook = null!;

        private DateTimePicker dtBorrowDate = null!;
        private DateTimePicker dtDueDate = null!;

        private Label lblAvailable = null!;

        private Button btnAddBook = null!;
        private Button btnRemoveBook = null!;
        private Button btnClear = null!;
        private Button btnConfirm = null!;

        private DataGridView dgvBorrowBooks = null!;

        public BorrowForm(Librarian librarian)
        {
            this.librarian = librarian;
            CreateUI();
            dtBorrowDate.MaxDate = DateTime.Today;
            dtBorrowDate.ValueChanged += (_, _) => UpdateDates();
            dtDueDate.ValueChanged += (_, _) => UpdateDates();
            dgvBorrowBooks.Columns[2].DefaultCellStyle.Format = "yyyy-MM-dd";
            dgvBorrowBooks.Columns[3].DefaultCellStyle.Format = "yyyy-MM-dd";
            Shown += (_, _) => UiAction.Run(this, LoadChoices);
        }

        private void CreateUI()
        {
            this.Text = "Borrow Books";
            this.Size = new Size(1100, 700);
            this.StartPosition =
                FormStartPosition.CenterScreen;
            this.FormBorderStyle =
                FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor =
                Color.FromArgb(245, 247, 250);

            // HEADER
            Panel header = new Panel();
            header.Size =
                new Size(1100, 100);
            header.BackColor =
                Color.FromArgb(31, 41, 55);

            this.Controls.Add(header);

            Label title = new Label();

            title.Text =
                "BORROW BOOK";

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

            Label subtitle = new Label();

            subtitle.Text =
                "Create a new book borrowing transaction";

            subtitle.Font =
                new Font("Segoe UI", 10);

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

            // FORM PANEL
            Panel formPanel = new Panel();

            formPanel.Location =
                new Point(30, 120);

            formPanel.Size =
                new Size(1040, 220);

            formPanel.BackColor =
                Color.White;

            formPanel.BorderStyle =
                BorderStyle.FixedSingle;

            this.Controls.Add(formPanel);

            // MEMBER
            AddLabel(
                formPanel,
                "Member",
                25,
                20
            );

            cmbMember = new ComboBox();

            cmbMember.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            cmbMember.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbMember.Size =
                new Size(280, 30);

            cmbMember.Location =
                new Point(25, 45);





            formPanel.Controls.Add(
                cmbMember
            );

            // BORROW DATE
            AddLabel(
                formPanel,
                "Borrow Date",
                340,
                20
            );

            dtBorrowDate =
                new DateTimePicker();

            dtBorrowDate.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            dtBorrowDate.Format =
                DateTimePickerFormat.Short;

            dtBorrowDate.Size =
                new Size(200, 30);

            dtBorrowDate.Location =
                new Point(340, 45);

            dtBorrowDate.Value =
                DateTime.Today;

            formPanel.Controls.Add(
                dtBorrowDate
            );

            // DUE DATE
            AddLabel(
                formPanel,
                "Due Date",
                580,
                20
            );

            dtDueDate =
                new DateTimePicker();

            dtDueDate.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            dtDueDate.Format =
                DateTimePickerFormat.Short;

            dtDueDate.Size =
                new Size(200, 30);

            dtDueDate.Location =
                new Point(580, 45);

            dtDueDate.Value =
                DateTime.Today.AddDays(7);

            formPanel.Controls.Add(
                dtDueDate
            );

            // BOOK
            AddLabel(
                formPanel,
                "Book",
                25,
                95
            );

            cmbBook = new ComboBox();

            cmbBook.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            cmbBook.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbBook.Size =
                new Size(280, 30);

            cmbBook.Location =
                new Point(25, 120);





            cmbBook.SelectedIndexChanged +=
                CmbBook_SelectedIndexChanged;

            formPanel.Controls.Add(
                cmbBook
            );

            // AVAILABLE
            AddLabel(
                formPanel,
                "Available",
                340,
                95
            );

            lblAvailable = new Label();

            lblAvailable.Text =
                "Loading...";

            lblAvailable.Font =
                new Font(
                    "Segoe UI",
                    11,
                    FontStyle.Bold
                );

            lblAvailable.ForeColor =
                Color.FromArgb(
                    16,
                    185,
                    129
                );

            lblAvailable.AutoSize = true;

            lblAvailable.Location =
                new Point(340, 122);

            formPanel.Controls.Add(
                lblAvailable
            );

            // ADD BOOK
            btnAddBook =
                CreateButton(
                    "ADD BOOK",
                    580,
                    112,
                    Color.FromArgb(
                        37,
                        99,
                        235
                    )
                );

            btnAddBook.Click +=
                BtnAddBook_Click;

            formPanel.Controls.Add(
                btnAddBook
            );

            // CLEAR
            btnClear =
                CreateButton(
                    "CLEAR",
                    700,
                    112,
                    Color.FromArgb(
                        107,
                        114,
                        128
                    )
                );

            btnClear.Click +=
                BtnClear_Click;

            formPanel.Controls.Add(
                btnClear
            );

            // BORROWED BOOKS PANEL
            Panel tablePanel =
                new Panel();

            tablePanel.Location =
                new Point(30, 360);

            tablePanel.Size =
                new Size(1040, 240);

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
                "Selected Books";

            tableTitle.Font =
                new Font(
                    "Segoe UI",
                    11,
                    FontStyle.Bold
                );

            tableTitle.AutoSize = true;

            tableTitle.Location =
                new Point(20, 15);

            tablePanel.Controls.Add(
                tableTitle
            );

            dgvBorrowBooks =
                new DataGridView();

            dgvBorrowBooks.Location =
                new Point(20, 50);

            dgvBorrowBooks.Size =
                new Size(990, 130);

            dgvBorrowBooks.BackgroundColor =
                Color.White;

            dgvBorrowBooks.BorderStyle =
                BorderStyle.None;

            dgvBorrowBooks.AllowUserToAddRows =
                false;

            dgvBorrowBooks.ReadOnly =
                true;

            dgvBorrowBooks.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvBorrowBooks.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvBorrowBooks.RowHeadersVisible =
                false;

            dgvBorrowBooks.ColumnHeadersDefaultCellStyle =
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

            dgvBorrowBooks.EnableHeadersVisualStyles =
                false;

            tablePanel.Controls.Add(
                dgvBorrowBooks
            );

            dgvBorrowBooks.Columns.Add(
                "BookID",
                "Book ID"
            );

            dgvBorrowBooks.Columns.Add(
                "Book",
                "Book"
            );

            dgvBorrowBooks.Columns.Add(
                "BorrowDate",
                "Borrow Date"
            );

            dgvBorrowBooks.Columns.Add(
                "DueDate",
                "Due Date"
            );

            // REMOVE BUTTON
            btnRemoveBook =
                CreateButton(
                    "REMOVE",
                    20,
                    190,
                    Color.FromArgb(
                        220,
                        53,
                        69
                    )
                );

            btnRemoveBook.Click +=
                BtnRemoveBook_Click;

            tablePanel.Controls.Add(
                btnRemoveBook
            );

            // CONFIRM
            btnConfirm =
                CreateButton(
                    "CONFIRM BORROW",
                    850,
                    190,
                    Color.FromArgb(
                        16,
                        185,
                        129
                    )
                );

            btnConfirm.Size =
                new Size(140, 40);

            btnConfirm.Click +=
                BtnConfirm_Click;

            tablePanel.Controls.Add(
                btnConfirm
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
                new Point(x, y);

            panel.Controls.Add(
                label
            );
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
                new Size(110, 40);

            button.Location =
                new Point(x, y);

            button.Cursor =
                Cursors.Hand;

            return button;
        }

        private readonly BorrowService service = new();
        private readonly Librarian librarian;

        private void LoadChoices()
        {
            var members = service.GetActiveMembers();
            var books = service.GetAvailableBooks();
            cmbMember.DisplayMember = nameof(Member.Name);
            cmbMember.ValueMember = nameof(Member.MemberID);
            cmbMember.DataSource = members;
            cmbBook.DisplayMember = nameof(Book.Title);
            cmbBook.ValueMember = nameof(Book.BookID);
            cmbBook.DataSource = books;
            CmbBook_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void CmbBook_SelectedIndexChanged(object? sender, EventArgs e)
        {
            lblAvailable.Text = cmbBook.SelectedItem is Book book
                ? $"{book.AvailableQuantity} copies" : "No available books";
        }

        private void BtnAddBook_Click(object? sender, EventArgs e) => UiAction.Run(this, () =>
        {
            if (cmbBook.SelectedItem is not Book book)
                throw new InvalidOperationException("Select an available book.");
            if (dgvBorrowBooks.Rows.Cast<DataGridViewRow>().Any(row => Convert.ToInt32(row.Cells[0].Value) == book.BookID))
                throw new InvalidOperationException("This book is already in the list.");
            dgvBorrowBooks.Rows.Add(book.BookID, book.Title,
                dtBorrowDate.Value.Date, dtDueDate.Value.Date);
        });

        private void BtnRemoveBook_Click(object? sender, EventArgs e)
        {
            if (dgvBorrowBooks.SelectedRows.Count > 0)
                dgvBorrowBooks.Rows.Remove(dgvBorrowBooks.SelectedRows[0]);
        }

        private void UpdateDates()
        {
            foreach (DataGridViewRow row in dgvBorrowBooks.Rows)
            {
                row.Cells[2].Value = dtBorrowDate.Value.Date;
                row.Cells[3].Value = dtDueDate.Value.Date;
            }
        }

        private void BtnConfirm_Click(object? sender, EventArgs e)
        {
            btnConfirm.Enabled = false;
            try
            {
                UiAction.Run(this, () =>
                {
                    if (cmbMember.SelectedItem is not Member member)
                        throw new InvalidOperationException("Register and select an active member first.");
                    var ids = dgvBorrowBooks.Rows.Cast<DataGridViewRow>()
                        .Select(row => Convert.ToInt32(row.Cells[0].Value)).ToList();
                    int id = service.BorrowBooks(member.MemberID, librarian.LibrarianID,
                        dtBorrowDate.Value.Date, dtDueDate.Value.Date, ids);
                    dgvBorrowBooks.Rows.Clear();
                    MessageBox.Show(this, $"Borrowing saved. Borrow ID: {id}", "Success");
                    LoadChoices();
                });
            }
            finally { btnConfirm.Enabled = true; }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            dgvBorrowBooks.Rows.Clear();
            UiAction.Run(this, LoadChoices);
        }
    }
}
