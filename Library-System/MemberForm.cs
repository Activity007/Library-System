using System;
using System.Drawing;
using System.Windows.Forms;

namespace library_system
{
    public class MemberForm : Form
    {
        private TextBox txtName = null!;
        private ComboBox cmbGender = null!;
        private TextBox txtPhone = null!;
        private TextBox txtEmail = null!;
        private TextBox txtAddress = null!;
        private TextBox txtSearch = null!;

        private Button btnAdd = null!;
        private Button btnUpdate = null!;
        private Button btnDeactivate = null!;
        private Button btnClear = null!;
        private Button btnSearch = null!;

        private DataGridView dgvMembers = null!;

        public MemberForm()
        {
            CreateUI();
            Shown += (_, _) => UiAction.Run(this, LoadMembers);
        }

        private void CreateUI()
        {
            this.Text = "Member Management";
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
            header.Size = new Size(1100, 100);
            header.BackColor =
                Color.FromArgb(31, 41, 55);
            this.Controls.Add(header);

            Label title = new Label();
            title.Text = "MEMBER MANAGEMENT";
            title.Font =
                new Font("Segoe UI", 22, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.AutoSize = true;
            title.Location = new Point(35, 22);
            header.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text =
                "Register and manage library members";
            subtitle.Font =
                new Font("Segoe UI", 10);
            subtitle.ForeColor =
                Color.FromArgb(209, 213, 219);
            subtitle.AutoSize = true;
            subtitle.Location = new Point(38, 62);
            header.Controls.Add(subtitle);

            // FORM PANEL
            Panel formPanel = new Panel();
            formPanel.Location = new Point(30, 120);
            formPanel.Size = new Size(1040, 260);
            formPanel.BackColor = Color.White;
            formPanel.BorderStyle =
                BorderStyle.FixedSingle;
            this.Controls.Add(formPanel);

            // NAME
            AddLabel(
                formPanel,
                "Full Name",
                25,
                20
            );

            txtName = CreateTextBox(
                25,
                45,
                240
            );

            formPanel.Controls.Add(txtName);

            // GENDER
            AddLabel(
                formPanel,
                "Gender",
                295,
                20
            );

            cmbGender = new ComboBox();
            cmbGender.Font =
                new Font("Segoe UI", 10);
            cmbGender.DropDownStyle =
                ComboBoxStyle.DropDownList;
            cmbGender.Size =
                new Size(240, 30);
            cmbGender.Location =
                new Point(295, 45);

            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");
            cmbGender.Items.Add("Other");

            cmbGender.SelectedIndex = 0;

            formPanel.Controls.Add(cmbGender);

            // PHONE
            AddLabel(
                formPanel,
                "Phone",
                565,
                20
            );

            txtPhone = CreateTextBox(
                565,
                45,
                240
            );

            formPanel.Controls.Add(txtPhone);

            // EMAIL
            AddLabel(
                formPanel,
                "Email",
                835,
                20
            );

            TextBox emailBox = CreateTextBox(
                835,
                45,
                180
            );

            txtEmail = emailBox;

            formPanel.Controls.Add(txtEmail);

            // ADDRESS
            AddLabel(
                formPanel,
                "Address",
                25,
                95
            );

            txtAddress = CreateTextBox(
                25,
                120,
                510
            );

            formPanel.Controls.Add(txtAddress);

            // BUTTONS
            btnAdd = CreateButton(
                "REGISTER",
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

            btnDeactivate = CreateButton(
                "DEACTIVATE",
                265,
                190,
                Color.FromArgb(220, 53, 69)
            );

            btnDeactivate.Click +=
                BtnDeactivate_Click;

            formPanel.Controls.Add(
                btnDeactivate
            );

            btnClear = CreateButton(
                "CLEAR",
                385,
                190,
                Color.FromArgb(107, 114, 128)
            );

            btnClear.Click += BtnClear_Click;
            formPanel.Controls.Add(btnClear);

            // TABLE
            Panel tablePanel = new Panel();
            tablePanel.Location =
                new Point(30, 400);
            tablePanel.Size =
                new Size(1040, 240);
            tablePanel.BackColor =
                Color.White;
            tablePanel.BorderStyle =
                BorderStyle.FixedSingle;

            this.Controls.Add(tablePanel);

            Label searchLabel = new Label();
            searchLabel.Text = "Search";
            searchLabel.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                );
            searchLabel.AutoSize = true;
            searchLabel.Location =
                new Point(20, 15);

            tablePanel.Controls.Add(
                searchLabel
            );

            txtSearch = CreateTextBox(
                80,
                12,
                280
            );

            tablePanel.Controls.Add(
                txtSearch
            );

            btnSearch = CreateButton(
                "SEARCH",
                375,
                10,
                Color.FromArgb(37, 99, 235)
            );

            btnSearch.Size =
                new Size(100, 32);

            btnSearch.Click +=
                BtnSearch_Click;

            tablePanel.Controls.Add(
                btnSearch
            );

            // GRID
            dgvMembers = new DataGridView();

            dgvMembers.Location =
                new Point(20, 55);

            dgvMembers.Size =
                new Size(990, 165);

            dgvMembers.BackgroundColor =
                Color.White;

            dgvMembers.BorderStyle =
                BorderStyle.None;

            dgvMembers.AllowUserToAddRows =
                false;

            dgvMembers.ReadOnly = true;

            dgvMembers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvMembers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvMembers.RowHeadersVisible =
                false;

            dgvMembers.ColumnHeadersDefaultCellStyle =
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

            dgvMembers.EnableHeadersVisualStyles =
                false;

            dgvMembers.CellClick +=
                DgvMembers_CellClick;

            tablePanel.Controls.Add(
                dgvMembers
            );

            dgvMembers.Columns.Add(
                "ID",
                "ID"
            );

            dgvMembers.Columns.Add(
                "Name",
                "Name"
            );

            dgvMembers.Columns.Add(
                "Gender",
                "Gender"
            );

            dgvMembers.Columns.Add(
                "Phone",
                "Phone"
            );

            dgvMembers.Columns.Add(
                "Email",
                "Email"
            );

            dgvMembers.Columns.Add(
                "Address",
                "Address"
            );

            dgvMembers.Columns.Add(
                "Status",
                "Status"
            );
        }

        private void AddLabel(
            Panel panel,
            string text,
            int x,
            int y)
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

            panel.Controls.Add(label);
        }

        private TextBox CreateTextBox(
            int x,
            int y,
            int width)
        {
            TextBox box = new TextBox();

            box.Font =
                new Font("Segoe UI", 10);

            box.Size =
                new Size(width, 30);

            box.Location =
                new Point(x, y);

            return box;
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
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold
                );

            button.ForeColor = Color.White;
            button.BackColor = color;
            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.Size =
                new Size(110, 40);

            button.Location =
                new Point(x, y);

            button.Cursor =
                Cursors.Hand;

            return button;
        }

        private readonly MemberService service = new();
        private Member? selectedMember;

        private void LoadMembers()
        {
            var members = service.SearchMembers(txtSearch.Text.Trim());
            dgvMembers.Rows.Clear();
            foreach (var member in members)
            {
                int index = dgvMembers.Rows.Add(member.MemberID, member.Name, member.Gender,
                    member.Phone, member.Email, member.Address, member.Status);
                dgvMembers.Rows[index].Tag = member;
            }
            ClearForm();
        }

        private Member ReadMember()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
                throw new InvalidOperationException("Enter the member name.");
            return new Member
            {
                MemberID = selectedMember?.MemberID ?? 0, Name = txtName.Text.Trim(),
                Gender = cmbGender.Text, Phone = txtPhone.Text.Trim(), Email = txtEmail.Text.Trim(),
                Address = txtAddress.Text.Trim(), RegisterDate = selectedMember?.RegisterDate ?? DateTime.Today,
                Status = selectedMember?.Status ?? "Active"
            };
        }

        private void BtnAdd_Click(object? sender, EventArgs e) => UiAction.Run(this, () =>
        {
            var member = ReadMember();
            member.Status = "Active";
            member.RegisterDate = DateTime.Today;
            service.AddMember(member);
            LoadMembers();
        });

        private void BtnUpdate_Click(object? sender, EventArgs e) => UiAction.Run(this, () =>
        {
            if (selectedMember == null) throw new InvalidOperationException("Select a member to update.");
            service.UpdateMember(ReadMember());
            LoadMembers();
        });

        private void BtnDeactivate_Click(object? sender, EventArgs e) => UiAction.Run(this, () =>
        {
            if (selectedMember == null) throw new InvalidOperationException("Select a member.");
            if (!UiAction.Confirm(this, $"Deactivate '{selectedMember.Name}'? Existing loans will remain returnable.")) return;
            service.DeactivateMember(selectedMember.MemberID);
            LoadMembers();
        });

        private void BtnSearch_Click(object? sender, EventArgs e) => UiAction.Run(this, LoadMembers);

        private void DgvMembers_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvMembers.Rows[e.RowIndex].Tag is not Member member) return;
            selectedMember = member;
            txtName.Text = member.Name;
            cmbGender.Text = member.Gender;
            txtPhone.Text = member.Phone;
            txtEmail.Text = member.Email;
            txtAddress.Text = member.Address;
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            UiAction.Run(this, LoadMembers);
        }

        private void ClearForm()
        {
            selectedMember = null;
            txtName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            cmbGender.SelectedIndex = 0;
            dgvMembers.ClearSelection();
        }
    }
}
