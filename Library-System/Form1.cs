using System.Diagnostics.CodeAnalysis;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace library_system
{
    public partial class Form1 : Form
    {
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;

        private TextBox txtUsername;
        private TextBox txtPassword;

        private Button btnLogin;
        private Button btnExit;

        public Form1()
        {
            CreateLoginUI();
        }

        [MemberNotNull(nameof(lblTitle), nameof(lblUsername), nameof(lblPassword),
            nameof(txtUsername), nameof(txtPassword), nameof(btnLogin), nameof(btnExit))]
        private void CreateLoginUI()
        {
            // =========================
            // FORM
            // =========================

            this.Text = "Library Management System - Login";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // =========================
            // TITLE
            // =========================

            lblTitle = new Label();

            lblTitle.Text = "LIBRARY MANAGEMENT SYSTEM";
            lblTitle.Font = new Font(
                "Segoe UI",
                18,
                FontStyle.Bold
            );

            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(85, 45);

            this.Controls.Add(lblTitle);

            // =========================
            // USERNAME
            // =========================

            lblUsername = new Label();

            lblUsername.Text = "Username";
            lblUsername.Font = new Font("Segoe UI", 10);
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(90, 120);

            this.Controls.Add(lblUsername);

            txtUsername = new TextBox();

            txtUsername.Name = "txtUsername";
            txtUsername.Font = new Font("Segoe UI", 11);
            txtUsername.Size = new Size(300, 30);
            txtUsername.Location = new Point(90, 145);

            this.Controls.Add(txtUsername);

            // =========================
            // PASSWORD
            // =========================

            lblPassword = new Label();

            lblPassword.Text = "Password";
            lblPassword.Font = new Font("Segoe UI", 10);
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(90, 190);

            this.Controls.Add(lblPassword);

            txtPassword = new TextBox();

            txtPassword.Name = "txtPassword";
            txtPassword.Font = new Font("Segoe UI", 11);
            txtPassword.Size = new Size(300, 30);
            txtPassword.Location = new Point(90, 215);

            txtPassword.UseSystemPasswordChar = true;

            this.Controls.Add(txtPassword);

            // =========================
            // LOGIN BUTTON
            // =========================

            btnLogin = new Button();

            btnLogin.Text = "Login";
            btnLogin.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnLogin.Size = new Size(140, 40);
            btnLogin.Location = new Point(90, 275);

            btnLogin.BackColor = Color.FromArgb(52, 152, 219);
            btnLogin.ForeColor = Color.White;

            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Cursor = Cursors.Hand;

            btnLogin.Click += BtnLogin_Click;

            this.Controls.Add(btnLogin);

            // =========================
            // EXIT BUTTON
            // =========================

            btnExit = new Button();

            btnExit.Text = "Exit";
            btnExit.Font = new Font("Segoe UI", 10);

            btnExit.Size = new Size(140, 40);
            btnExit.Location = new Point(250, 275);

            btnExit.BackColor = Color.LightGray;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Cursor = Cursors.Hand;

            btnExit.Click += BtnExit_Click;

            this.Controls.Add(btnExit);

            // Enter = Login
            this.AcceptButton = btnLogin;

            // Escape = Exit
            this.CancelButton = btnExit;
        }

        // =========================
        // LOGIN BUTTON
        // =========================

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(this, "Enter your username and password.", "Login");
                return;
            }
            btnLogin.Enabled = false;
            try
            {
                UiAction.Run(this, () =>
                {
                    var librarian = new LoginService().Authenticate(txtUsername.Text, txtPassword.Text);
                    if (librarian == null)
                    {
                        MessageBox.Show(this, "Invalid username or password.", "Login");
                        txtPassword.Clear();
                        txtPassword.Focus();
                        return;
                    }
                    var mainMenu = new MainMenuForm(librarian);
                    mainMenu.FormClosed += (_, _) =>
                    {
                        if (mainMenu.DialogResult == DialogResult.OK)
                        {
                            txtPassword.Clear();
                            Show();
                            txtUsername.Focus();
                        }
                        else Close();
                    };
                    mainMenu.Show();
                    Hide();
                });
            }
            finally { btnLogin.Enabled = true; }
        }

        private void BtnExit_Click(object? sender, EventArgs e) => Close();
    }
}