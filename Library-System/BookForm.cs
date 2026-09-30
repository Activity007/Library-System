using System;
using System.Drawing;
using System.Windows.Forms;

namespace library_system
{
    public class BookForm : Form
    {
        // =========================
        // CONTROLS
        // =========================

        private Panel headerPanel = null!;
        private Panel formPanel = null!;
        private Panel tablePanel = null!;

        private Label lblTitle = null!;
        private Label lblSubtitle = null!;

        private Label lblISBN = null!;
        private Label lblBookTitle = null!;
        private Label lblAuthor = null!;
        private Label lblCategory = null!;
        private Label lblPublisher = null!;
        private Label lblYear = null!;
        private Label lblQuantity = null!;
        private Label lblAvailable = null!;

        private TextBox txtISBN = null!;
        private TextBox txtBookTitle = null!;
        private TextBox txtAuthor = null!;
        private TextBox txtPublisher = null!;
        private TextBox txtYear = null!;
        private TextBox txtQuantity = null!;
        private TextBox txtAvailable = null!;

        private ComboBox cmbCategory = null!;

        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnClear = null!;

        private TextBox txtSearch = null!;
        private Button btnSearch = null!;

        private DataGridView dgvBooks = null!;

        public BookForm()
        {
            CreateUI();
            Shown += (_, _) => UiAction.Run(this, LoadBooks);
        }

        // =========================
        // CREATE UI
        // =========================

        private void CreateUI()
        {
            // FORM
            this.Text = "Book Management";
            this.Size = new Size(1200, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // =========================
            // HEADER
            // =========================

            headerPanel = new Panel();
            headerPanel.Location = new Point(0, 0);
            headerPanel.Size = new Size(1200, 100);
            headerPanel.BackColor = Color.FromArgb(31, 41, 55);

            this.Controls.Add(headerPanel);

            lblTitle = new Label();
            lblTitle.Text = "BOOK MANAGEMENT";
            lblTitle.Font = new Font(
                "Segoe UI",
                22,
                FontStyle.Bold
            );
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(35, 22);

            headerPanel.Controls.Add(lblTitle);

            lblSubtitle = new Label();
            lblSubtitle.Text =
                "Add, update, delete and search library books";

            lblSubtitle.Font =
                new Font("Segoe UI", 10);

            lblSubtitle.ForeColor =
                Color.FromArgb(209, 213, 219);

            lblSubtitle.AutoSize = true;
            lblSubtitle.Location =
                new Point(38, 62);

            headerPanel.Controls.Add(lblSubtitle);

            // =========================
            // FORM PANEL
            // =========================

            formPanel = new Panel();
            formPanel.Location = new Point(30, 120);
            formPanel.Size = new Size(1140, 260);
            formPanel.BackColor = Color.White;
            formPanel.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(formPanel);

            // =========================
            // ISBN
            // =========================

            lblISBN = CreateLabel("ISBN", 25, 20);
            formPanel.Controls.Add(lblISBN);

            txtISBN = CreateTextBox(25, 45, 240);
            formPanel.Controls.Add(txtISBN);

            // =========================
            // TITLE
            // =========================

            lblBookTitle = CreateLabel(
                "Book Title",
                295,
                20
            );

            formPanel.Controls.Add(lblBookTitle);

            txtBookTitle = CreateTextBox(
                295,
                45,
                240
            );

            formPanel.Controls.Add(txtBookTitle);

            // =========================
            // AUTHOR
            // =========================

            lblAuthor = CreateLabel(
                "Author",
                565,
                20
            );

            formPanel.Controls.Add(lblAuthor);

            txtAuthor = CreateTextBox(
                565,
                45,
                240
            );

            formPanel.Controls.Add(txtAuthor);

            // =========================
            // CATEGORY
            // =========================

            lblCategory = CreateLabel(
                "Category",
                835,
                20
            );

            formPanel.Controls.Add(lblCategory);

            cmbCategory = new ComboBox();

            cmbCategory.Font =
                new Font("Segoe UI", 10);

            cmbCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbCategory.Size =
                new Size(240, 30);

            cmbCategory.Location =
                new Point(835, 45);

            // Categories are loaded from the database.


if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;

            formPanel.Controls.Add(cmbCategory);

            // =========================
            // PUBLISHER
            // =========================

            lblPublisher = CreateLabel(
                "Publisher",
                25,
                95
            );

            formPanel.Controls.Add(lblPublisher);

            txtPublisher = CreateTextBox(
                25,
                120,
                240
            );

            formPanel.Controls.Add(txtPublisher);

            // =========================
            // YEAR
            // =========================

            lblYear = CreateLabel(
                "Publish Year",
                295,
                95
            );

            formPanel.Controls.Add(lblYear);

            txtYear = CreateTextBox(
                295,
                120,
                240
            );

            formPanel.Controls.Add(txtYear);

            // =========================
            // QUANTITY
            // =========================

            lblQuantity = CreateLabel(
                "Quantity",
                565,
                95
            );

            formPanel.Controls.Add(lblQuantity);

            txtQuantity = CreateTextBox(
                565,
                120,
                240
            );

            txtQuantity.TextChanged +=
                TxtQuantity_TextChanged;

            formPanel.Controls.Add(txtQuantity);

            // =========================
            // AVAILABLE
            // =========================

            lblAvailable = CreateLabel(
                "Available Quantity",
                835,
                95
            );

            formPanel.Controls.Add(lblAvailable);

            txtAvailable = CreateTextBox(
                835,
                120,
                240
            );

            txtAvailable.ReadOnly = true;
            txtAvailable.BackColor =
                Color.FromArgb(243, 244, 246);

            formPanel.Controls.Add(txtAvailable);

            // =========================
            // BUTTONS
            // =========================

            btnAdd = CreateButton(
                "ADD",
                25,
                190,
                Color.FromArgb(37, 99, 235)
            );

            btnAdd.Click += BtnAdd_Click;
            formPanel.Controls.Add(btnAdd);

            btnUpdate = CreateButton(
                "UPDATE",
                145,
                190,
                Color.FromArgb(16, 185, 129)
            );

            btnUpdate.Click += BtnUpdate_Click;
            formPanel.Controls.Add(btnUpdate);

            btnDelete = CreateButton(
                "DELETE",
                265,
                190,
                Color.FromArgb(220, 53, 69)
            );

            btnDelete.Click += BtnDelete_Click;
            formPanel.Controls.Add(btnDelete);

            btnClear = CreateButton(
                "CLEAR",
                385,
                190,
                Color.FromArgb(107, 114, 128)
            );

            btnClear.Click += BtnClear_Click;
            formPanel.Controls.Add(btnClear);

            // =========================
            // TABLE PANEL
            // =========================

            tablePanel = new Panel();
            tablePanel.Location =
                new Point(30, 400);

            tablePanel.Size =
                new Size(1140, 260);

            tablePanel.BackColor =
                Color.White;

            tablePanel.BorderStyle =
                BorderStyle.FixedSingle;

            this.Controls.Add(tablePanel);

            // =========================
            // SEARCH
            // =========================

            Label lblSearch = new Label();

            lblSearch.Text =
                "Search Books";

            lblSearch.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                );

            lblSearch.ForeColor =
                Color.FromArgb(31, 41, 55);

            lblSearch.AutoSize = true;

            lblSearch.Location =
                new Point(20, 15);

            tablePanel.Controls.Add(lblSearch);

            txtSearch = new TextBox();

            txtSearch.Font =
                new Font("Segoe UI", 10);

            txtSearch.Size =
                new Size(300, 30);

            txtSearch.Location =
                new Point(130, 12);

            tablePanel.Controls.Add(txtSearch);

            btnSearch = CreateButton(
                "SEARCH",
                445,
                10,
                Color.FromArgb(37, 99, 235)
            );

            btnSearch.Size =
                new Size(100, 32);

            btnSearch.Click +=
                BtnSearch_Click;

            tablePanel.Controls.Add(btnSearch);

            // =========================
            // DATA GRID
            // =========================

            dgvBooks = new DataGridView();

            dgvBooks.Location =
                new Point(20, 55);

            dgvBooks.Size =
                new Size(1095, 185);

            dgvBooks.BackgroundColor =
                Color.White;

            dgvBooks.BorderStyle =
                BorderStyle.None;

            dgvBooks.AllowUserToAddRows =
                false;

            dgvBooks.AllowUserToDeleteRows =
                false;

            dgvBooks.ReadOnly = true;

            dgvBooks.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvBooks.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvBooks.RowHeadersVisible =
                false;

            dgvBooks.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(31, 41, 55),

                    ForeColor =
                        Color.White,

                    Font =
                        new Font(
                            "Segoe UI",
                            9,
                            FontStyle.Bold
                        )
                };

            dgvBooks.EnableHeadersVisualStyles =
                false;

            dgvBooks.CellClick +=
                DgvBooks_CellClick;

            tablePanel.Controls.Add(dgvBooks);

            // Table columns
            CreateColumns();
        }

        // =========================
        // LABEL
        // =========================

        private Label CreateLabel(
            string text,
            int x,
            int y
        )
        {
            Label label = new Label();

            label.Text = text;

            label.Font =
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold
                );

            label.ForeColor =
                Color.FromArgb(55, 65, 81);

            label.AutoSize = true;

            label.Location =
                new Point(x, y);

            return label;
        }

        // =========================
        // TEXTBOX
        // =========================

        private TextBox CreateTextBox(
            int x,
            int y,
            int width
        )
        {
            TextBox textbox = new TextBox();

            textbox.Font =
                new Font("Segoe UI", 10);

            textbox.Size =
                new Size(width, 30);

            textbox.Location =
                new Point(x, y);

            return textbox;
        }

        // =========================
        // BUTTON
        // =========================

        private Button CreateButton(
            string text,
            int x,
            int y,
            Color color
        )
        {
            Button button = new Button();

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

        // =========================
        // TEMPORARY TABLE
        // =========================

        private void CreateColumns()
        {
            dgvBooks.Columns.Add(
                "BookID",
                "ID"
            );

            dgvBooks.Columns.Add(
                "ISBN",
                "ISBN"
            );

            dgvBooks.Columns.Add(
                "Title",
                "Title"
            );

            dgvBooks.Columns.Add(
                "Author",
                "Author"
            );

            dgvBooks.Columns.Add(
                "Category",
                "Category"
            );

            dgvBooks.Columns.Add(
                "Publisher",
                "Publisher"
            );

            dgvBooks.Columns.Add(
                "Year",
                "Year"
            );

            dgvBooks.Columns.Add(
                "Quantity",
                "Quantity"
            );

            dgvBooks.Columns.Add(
                "Available",
                "Available"
            );
        }

        // =========================
        // QUANTITY
        // =========================

        private readonly BookService service = new();
        private Book? selectedBook;

        private void LoadBooks()
        {
            var categories = new CategoryService().GetAllCategories();
            var books = service.SearchBooks(txtSearch.Text.Trim());
            cmbCategory.DisplayMember = nameof(Category.CategoryName);
            cmbCategory.ValueMember = nameof(Category.CategoryID);
            cmbCategory.DataSource = categories;
            var names = categories.ToDictionary(category => category.CategoryID, category => category.CategoryName);
            dgvBooks.Rows.Clear();
            foreach (var book in books)
            {
                int index = dgvBooks.Rows.Add(book.BookID, book.ISBN, book.Title, book.Author,
                    names.GetValueOrDefault(book.CategoryID, "(missing category)"), book.Publisher,
                    book.PublishYear, book.Quantity, book.AvailableQuantity);
                dgvBooks.Rows[index].Tag = book;
            }
            ClearForm();
        }

        private void TxtQuantity_TextChanged(object? sender, EventArgs e)
        {
            int onLoan = selectedBook == null ? 0 : selectedBook.Quantity - selectedBook.AvailableQuantity;
            txtAvailable.Text = int.TryParse(txtQuantity.Text, out int quantity)
                ? Math.Max(0, quantity - onLoan).ToString() : "";
        }

        private Book ReadBook()
        {
            if (string.IsNullOrWhiteSpace(txtISBN.Text) || string.IsNullOrWhiteSpace(txtBookTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text))
                throw new InvalidOperationException("Enter ISBN, title, and author.");
            if (cmbCategory.SelectedItem is not Category category)
                throw new InvalidOperationException("Create a category first, then select it for this book.");
            if (!int.TryParse(txtYear.Text, out int year) || year < 1 || year > DateTime.Today.Year + 1)
                throw new InvalidOperationException("Enter a valid publication year.");
            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
                throw new InvalidOperationException("Quantity must be a nonnegative whole number.");
            return new Book
            {
                BookID = selectedBook?.BookID ?? 0, ISBN = txtISBN.Text.Trim(), Title = txtBookTitle.Text.Trim(),
                Author = txtAuthor.Text.Trim(), CategoryID = category.CategoryID,
                Publisher = txtPublisher.Text.Trim(), PublishYear = year, Quantity = quantity,
                AvailableQuantity = quantity
            };
        }

        private void BtnAdd_Click(object? sender, EventArgs e) => UiAction.Run(this, () =>
        {
            service.AddBook(ReadBook());
            LoadBooks();
        });

        private void BtnUpdate_Click(object? sender, EventArgs e) => UiAction.Run(this, () =>
        {
            if (selectedBook == null) throw new InvalidOperationException("Select a book to update.");
            service.UpdateBook(ReadBook());
            LoadBooks();
        });

        private void BtnDelete_Click(object? sender, EventArgs e) => UiAction.Run(this, () =>
        {
            if (selectedBook == null) throw new InvalidOperationException("Select a book to delete.");
            if (!UiAction.Confirm(this, $"Delete '{selectedBook.Title}'?")) return;
            service.DeleteBook(selectedBook.BookID);
            LoadBooks();
        });

        private void BtnSearch_Click(object? sender, EventArgs e) => UiAction.Run(this, LoadBooks);

        private void DgvBooks_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvBooks.Rows[e.RowIndex].Tag is not Book book) return;
            selectedBook = book;
            txtISBN.Text = book.ISBN;
            txtBookTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            cmbCategory.SelectedValue = book.CategoryID;
            txtPublisher.Text = book.Publisher;
            txtYear.Text = book.PublishYear.ToString();
            txtQuantity.Text = book.Quantity.ToString();
            txtAvailable.Text = book.AvailableQuantity.ToString();
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            UiAction.Run(this, LoadBooks);
        }

        private void ClearForm()
        {
            selectedBook = null;
            txtISBN.Clear();
            txtBookTitle.Clear();
            txtAuthor.Clear();
            txtPublisher.Clear();
            txtYear.Clear();
            txtQuantity.Clear();
            txtAvailable.Clear();
            dgvBooks.ClearSelection();
        }
    }
}
