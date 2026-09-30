using System;
using System.Drawing;
using System.Windows.Forms;

namespace library_system
{
    public class CategoriesForm : Form
    {
        private TextBox txtCategoryName = null!;
        private TextBox txtDescription = null!;
        private TextBox txtSearch = null!;

        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDelete = null!;
        private Button btnClear = null!;
        private Button btnSearch = null!;

        private DataGridView dgvCategories = null!;

        public CategoriesForm()
        {
            CreateUI();
            Shown += (_, _) => UiAction.Run(this, LoadCategories);
        }

        private void CreateUI()
        {
            // FORM
            this.Text = "Category Management";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // HEADER
            Panel header = new Panel();
            header.Size = new Size(1000, 100);
            header.Location = new Point(0, 0);
            header.BackColor = Color.FromArgb(31, 41, 55);
            this.Controls.Add(header);

            Label title = new Label();
            title.Text = "CATEGORY MANAGEMENT";
            title.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.AutoSize = true;
            title.Location = new Point(35, 22);
            header.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Create and manage book categories";
            subtitle.Font = new Font("Segoe UI", 10);
            subtitle.ForeColor = Color.FromArgb(209, 213, 219);
            subtitle.AutoSize = true;
            subtitle.Location = new Point(38, 62);
            header.Controls.Add(subtitle);

            // FORM PANEL
            Panel formPanel = new Panel();
            formPanel.Location = new Point(30, 120);
            formPanel.Size = new Size(940, 200);
            formPanel.BackColor = Color.White;
            formPanel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(formPanel);

            // CATEGORY NAME
            Label lblName = CreateLabel("Category Name", 25, 25);
            formPanel.Controls.Add(lblName);

            txtCategoryName = CreateTextBox(25, 50, 300);
            formPanel.Controls.Add(txtCategoryName);

            // DESCRIPTION
            Label lblDescription = CreateLabel("Description", 360, 25);
            formPanel.Controls.Add(lblDescription);

            txtDescription = new TextBox();
            txtDescription.Font = new Font("Segoe UI", 10);
            txtDescription.Multiline = true;
            txtDescription.Size = new Size(400, 65);
            txtDescription.Location = new Point(360, 50);
            formPanel.Controls.Add(txtDescription);

            // BUTTONS
            btnAdd = CreateButton(
                "ADD",
                25,
                135,
                Color.FromArgb(37, 99, 235)
            );
            btnAdd.Click += BtnAdd_Click;
            formPanel.Controls.Add(btnAdd);

            btnUpdate = CreateButton(
                "UPDATE",
                145,
                135,
                Color.FromArgb(16, 185, 129)
            );
            btnUpdate.Click += BtnUpdate_Click;
            formPanel.Controls.Add(btnUpdate);

            btnDelete = CreateButton(
                "DELETE",
                265,
                135,
                Color.FromArgb(220, 53, 69)
            );
            btnDelete.Click += BtnDelete_Click;
            formPanel.Controls.Add(btnDelete);

            btnClear = CreateButton(
                "CLEAR",
                385,
                135,
                Color.FromArgb(107, 114, 128)
            );
            btnClear.Click += BtnClear_Click;
            formPanel.Controls.Add(btnClear);

            // TABLE PANEL
            Panel tablePanel = new Panel();
            tablePanel.Location = new Point(30, 340);
            tablePanel.Size = new Size(940, 250);
            tablePanel.BackColor = Color.White;
            tablePanel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(tablePanel);

            // SEARCH
            Label lblSearch = new Label();
            lblSearch.Text = "Search";
            lblSearch.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(20, 15);
            tablePanel.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.Size = new Size(300, 30);
            txtSearch.Location = new Point(80, 12);
            tablePanel.Controls.Add(txtSearch);

            btnSearch = CreateButton(
                "SEARCH",
                395,
                10,
                Color.FromArgb(37, 99, 235)
            );

            btnSearch.Size = new Size(100, 32);
            btnSearch.Click += BtnSearch_Click;
            tablePanel.Controls.Add(btnSearch);

            // GRID
            dgvCategories = new DataGridView();
            dgvCategories.Location = new Point(20, 55);
            dgvCategories.Size = new Size(895, 175);

            dgvCategories.BackgroundColor = Color.White;
            dgvCategories.BorderStyle = BorderStyle.None;
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.ReadOnly = true;
            dgvCategories.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategories.RowHeadersVisible = false;

            dgvCategories.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(31, 41, 55),
                    ForeColor = Color.White,
                    Font = new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold
                    )
                };

            dgvCategories.EnableHeadersVisualStyles = false;
            dgvCategories.CellClick += DgvCategories_CellClick;

            tablePanel.Controls.Add(dgvCategories);

            dgvCategories.Columns.Add("ID", "ID");
            dgvCategories.Columns.Add(
                "CategoryName",
                "Category Name"
            );
            dgvCategories.Columns.Add(
                "Description",
                "Description"
            );
        }

        private Label CreateLabel(
            string text,
            int x,
            int y)
        {
            Label label = new Label();

            label.Text = text;
            label.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);
            label.ForeColor =
                Color.FromArgb(55, 65, 81);
            label.AutoSize = true;
            label.Location = new Point(x, y);

            return label;
        }

        private TextBox CreateTextBox(
            int x,
            int y,
            int width)
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

        private Button CreateButton(
            string text,
            int x,
            int y,
            Color color)
        {
            Button button = new Button();

            button.Text = text;
            button.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);
            button.ForeColor = Color.White;
            button.BackColor = color;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Size = new Size(110, 40);
            button.Location = new Point(x, y);
            button.Cursor = Cursors.Hand;

            return button;
        }

        private readonly CategoryService service = new();
        private Category? selectedCategory;

        private void LoadCategories()
        {
            var categories = service.SearchCategories(txtSearch.Text.Trim());
            dgvCategories.Rows.Clear();
            foreach (var category in categories)
            {
                int index = dgvCategories.Rows.Add(category.CategoryID, category.CategoryName, category.Description);
                dgvCategories.Rows[index].Tag = category;
            }
            ClearForm();
        }

        private Category ReadCategory()
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                throw new InvalidOperationException("Enter a category name.");
            return new Category { CategoryID = selectedCategory?.CategoryID ?? 0,
                CategoryName = txtCategoryName.Text.Trim(), Description = txtDescription.Text.Trim() };
        }

        private void BtnAdd_Click(object? sender, EventArgs e) => UiAction.Run(this, () =>
        {
            service.AddCategory(ReadCategory());
            LoadCategories();
        });

        private void BtnUpdate_Click(object? sender, EventArgs e) => UiAction.Run(this, () =>
        {
            if (selectedCategory == null) throw new InvalidOperationException("Select a category to update.");
            service.UpdateCategory(ReadCategory());
            LoadCategories();
        });

        private void BtnDelete_Click(object? sender, EventArgs e) => UiAction.Run(this, () =>
        {
            if (selectedCategory == null) throw new InvalidOperationException("Select a category to delete.");
            if (!UiAction.Confirm(this, $"Delete category '{selectedCategory.CategoryName}'?")) return;
            service.DeleteCategory(selectedCategory.CategoryID);
            LoadCategories();
        });

        private void BtnSearch_Click(object? sender, EventArgs e) => UiAction.Run(this, LoadCategories);

        private void DgvCategories_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvCategories.Rows[e.RowIndex].Tag is not Category category) return;
            selectedCategory = category;
            txtCategoryName.Text = category.CategoryName;
            txtDescription.Text = category.Description;
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            UiAction.Run(this, LoadCategories);
        }

        private void ClearForm()
        {
            selectedCategory = null;
            txtCategoryName.Clear();
            txtDescription.Clear();
            dgvCategories.ClearSelection();
        }
    }
}
