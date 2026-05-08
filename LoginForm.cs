using System;
using System.Configuration;
using System.Drawing;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SkillsInternationalSchool
{
    public partial class LoginForm : Form
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["SchoolDb"].ConnectionString;

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnClear;
        private Button btnExit;

        public LoginForm()
        {
            InitializeComponent();
            InitializeLoginControls();
        }

        private void InitializeLoginControls()
        {
            Text = "Skills International School - Login";
            ClientSize = new Size(420, 240);
            StartPosition = FormStartPosition.CenterScreen;

            Label lblUsername = new Label
            {
                Text = "Username",
                Location = new Point(40, 45),
                AutoSize = true
            };

            Label lblPassword = new Label
            {
                Text = "Password",
                Location = new Point(40, 85),
                AutoSize = true
            };

            txtUsername = new TextBox
            {
                Location = new Point(130, 40),
                Width = 220,
                Name = "txtUsername"
            };

            txtPassword = new TextBox
            {
                Location = new Point(130, 80),
                Width = 220,
                PasswordChar = '*',
                Name = "txtPassword"
            };

            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(40, 140),
                Width = 90
            };
            btnLogin.Click += btnLogin_Click;

            btnClear = new Button
            {
                Text = "Clear",
                Location = new Point(145, 140),
                Width = 90
            };
            btnClear.Click += btnClear_Click;

            btnExit = new Button
            {
                Text = "Exit",
                Location = new Point(250, 140),
                Width = 90
            };
            btnExit.Click += btnExit_Click;

            Controls.Add(lblUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtUsername);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Controls.Add(btnClear);
            Controls.Add(btnExit);

            AcceptButton = btnLogin;
            CancelButton = btnExit;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Logins WHERE username = @user AND password = @pwd";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pwd", password);
                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        this.Hide();
                        RegistrationForm regForm = new RegistrationForm();
                        regForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed.\n\n{ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                Application.Exit();
        }
    }
}