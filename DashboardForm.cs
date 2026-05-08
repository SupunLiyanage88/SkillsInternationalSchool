using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SkillsInternationalSchool
{
    public partial class DashboardForm : Form
    {
        private readonly DatabaseHelper _dbHelper;
        private DataGridView dgvStudents;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnRefresh;
        private Button btnExportExcel;
        private Button btnExportPDF;
        private Label lblTotal;
        private LinkLabel llLogout;
        private LinkLabel llExit;
        private LinkLabel llManageStudents;

        public DashboardForm()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            InitializeDashboardControls();
            LoadStudentData();
        }

        private void InitializeDashboardControls()
        {
            Text = "Skills International School - Dashboard";
            ClientSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            // Title
            Label lblTitle = new Label
            {
                Text = "Student Dashboard",
                Font = new Font("Arial", 18, FontStyle.Bold),
                Location = new Point(35, 15),
                AutoSize = true,
                ForeColor = Color.DarkBlue
            };
            Controls.Add(lblTitle);

            // Total students label
            lblTotal = new Label
            {
                Text = "Total Students: 0",
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(35, 50),
                AutoSize = true,
                ForeColor = Color.Green
            };
            Controls.Add(lblTotal);

            // Search panel
            Label lblSearchTitle = new Label
            {
                Text = "Search by Name:",
                Location = new Point(35, 85),
                AutoSize = true
            };
            Controls.Add(lblSearchTitle);

            txtSearch = new TextBox
            {
                Location = new Point(150, 82),
                Width = 200,
                Name = "txtSearch"
            };
            Controls.Add(txtSearch);

            btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(360, 82),
                Width = 80
            };
            btnSearch.Click += BtnSearch_Click;
            Controls.Add(btnSearch);

            btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(450, 82),
                Width = 80
            };
            btnRefresh.Click += BtnRefresh_Click;
            Controls.Add(btnRefresh);

            // DataGridView
            dgvStudents = new DataGridView
            {
                Location = new Point(35, 125),
                Width = 830,
                Height = 380,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = true,
                ReadOnly = true,
                BackgroundColor = Color.White,
                Name = "dgvStudents"
            };
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(dgvStudents);

            // Export buttons
            btnExportExcel = new Button
            {
                Text = "Export to Excel",
                Location = new Point(35, 520),
                Width = 120
            };
            btnExportExcel.Click += BtnExportExcel_Click;
            Controls.Add(btnExportExcel);

            btnExportPDF = new Button
            {
                Text = "Export to PDF",
                Location = new Point(165, 520),
                Width = 120
            };
            btnExportPDF.Click += BtnExportPDF_Click;
            Controls.Add(btnExportPDF);

            // Link labels for navigation
            llManageStudents = new LinkLabel
            {
                Text = "Manage Students",
                Location = new Point(35, 555),
                AutoSize = true
            };
            llManageStudents.LinkClicked += LlManageStudents_LinkClicked;
            Controls.Add(llManageStudents);

            llLogout = new LinkLabel
            {
                Text = "Logout",
                Location = new Point(200, 555),
                AutoSize = true
            };
            llLogout.LinkClicked += LlLogout_LinkClicked;
            Controls.Add(llLogout);

            llExit = new LinkLabel
            {
                Text = "Exit",
                Location = new Point(260, 555),
                AutoSize = true
            };
            llExit.LinkClicked += LlExit_LinkClicked;
            Controls.Add(llExit);
        }

        private void LoadStudentData()
        {
            try
            {
                DataTable dt = _dbHelper.GetStudents();
                dgvStudents.DataSource = dt;
                UpdateTotalCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    MessageBox.Show("Please enter a name to search.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataTable dt = _dbHelper.SearchStudentsByName(searchTerm);
                dgvStudents.DataSource = dt;
                lblTotal.Text = $"Search Results: {dt.Rows.Count} student(s) found";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadStudentData();
        }

        private void UpdateTotalCount()
        {
            try
            {
                int total = _dbHelper.GetTotalStudents();
                lblTotal.Text = $"Total Students: {total}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating student count: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Excel Files (*.csv)|*.csv",
                    FileName = $"Students_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = dgvStudents.DataSource as DataTable;
                    ExportHelper.ExportToExcel(dt, sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt",
                    FileName = $"Students_{DateTime.Now:yyyyMMdd_HHmmss}.txt",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = dgvStudents.DataSource as DataTable;
                    ExportHelper.ExportToPDF(dt, sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LlManageStudents_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            RegistrationForm regForm = new RegistrationForm();
            regForm.Show();
        }

        private void LlLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }

        private void LlExit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                Application.Exit();
        }
    }
}
