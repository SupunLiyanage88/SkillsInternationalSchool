using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SkillsInternationalSchool
{
    public partial class RegistrationForm : Form
    {
        readonly string connectionString = ConfigurationManager.ConnectionStrings["SchoolDb"].ConnectionString;

        private ErrorProvider errorProvider;
        private readonly DatabaseHelper _dbHelper;
        private TableLayoutPanel formLayout;
        private FlowLayoutPanel genderPanel;
        private FlowLayoutPanel buttonPanel;
        private FlowLayoutPanel linkPanel;
        private ComboBox cboRegNo;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private DateTimePicker dtpDOB;
        private RadioButton rbMale;
        private RadioButton rbFemale;
        private TextBox txtAddress;
        private TextBox txtEmail;
        private TextBox txtMobile;
        private TextBox txtHomePhone;
        private TextBox txtParentName;
        private TextBox txtNIC;
        private TextBox txtContactNo;
        private Button btnRegister;
        private Button btnDelete;
        private Button btnClear;
        private LinkLabel llLogout;
        private LinkLabel llExit;

        private bool isLoadingRegNumbers;

        public RegistrationForm()
        {
            InitializeComponent();
            errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _dbHelper = new DatabaseHelper();
            InitializeRegistrationControls();
            LoadRegNumbers();
        }

        private void InitializeRegistrationControls()
        {
            Text = "Skills International School - Registration";
            ClientSize = new Size(900, 720);
            MinimumSize = new Size(760, 640);
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.WhiteSmoke;
            Padding = new Padding(20);

            formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                RowCount = 0,
                Padding = new Padding(8, 6, 12, 12),
                Margin = new Padding(0)
            };
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            Controls.Add(formLayout);

            Label lblTitle = new Label
            {
                Text = "Student Registration",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 12)
            };
            AddSpanningControl(lblTitle);

            cboRegNo = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cboRegNo",
                Margin = new Padding(0, 3, 0, 3)
            };
            cboRegNo.SelectedIndexChanged += cboRegNo_SelectedIndexChanged;
            AddFieldRow("Registration No", cboRegNo);

            txtFirstName = CreateTextBox("txtFirstName");
            txtFirstName.Leave += TxtFirstName_Leave;
            AddFieldRow("First Name *", txtFirstName);

            txtLastName = CreateTextBox("txtLastName");
            txtLastName.Leave += TxtLastName_Leave;
            AddFieldRow("Last Name *", txtLastName);

            dtpDOB = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Name = "dtpDOB",
                Margin = new Padding(0, 3, 0, 3)
            };
            AddFieldRow("Date of Birth", dtpDOB);

            genderPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0, 1, 0, 1)
            };
            rbMale = new RadioButton
            {
                Text = "Male",
                AutoSize = true,
                Name = "rbMale",
                Margin = new Padding(0, 4, 18, 4)
            };
            rbFemale = new RadioButton
            {
                Text = "Female",
                AutoSize = true,
                Name = "rbFemale",
                Margin = new Padding(0, 4, 0, 4)
            };
            genderPanel.Controls.Add(rbMale);
            genderPanel.Controls.Add(rbFemale);
            AddFieldRow("Gender *", genderPanel);

            txtAddress = CreateTextBox("txtAddress", true);
            txtAddress.ScrollBars = ScrollBars.Vertical;
            AddFieldRow("Address", txtAddress);

            txtEmail = CreateTextBox("txtEmail");
            txtEmail.Leave += TxtEmail_Leave;
            AddFieldRow("Email *", txtEmail);

            txtMobile = CreateTextBox("txtMobile");
            AddFieldRow("Mobile Phone", txtMobile);

            txtHomePhone = CreateTextBox("txtHomePhone");
            AddFieldRow("Home Phone", txtHomePhone);

            txtParentName = CreateTextBox("txtParentName");
            AddFieldRow("Parent Name", txtParentName);

            txtNIC = CreateTextBox("txtNIC");
            AddFieldRow("NIC", txtNIC);

            txtContactNo = CreateTextBox("txtContactNo");
            AddFieldRow("Contact No", txtContactNo);

            buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 16, 0, 0)
            };
            btnRegister = new Button
            {
                Text = "Register",
                Width = 110,
                Height = 34,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnRegister.Click += btnRegister_Click;

            btnDelete = new Button
            {
                Text = "Delete",
                Width = 110,
                Height = 34,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnDelete.Click += btnDelete_Click;

            btnClear = new Button
            {
                Text = "Clear",
                Width = 110,
                Height = 34,
                Margin = new Padding(0)
            };
            btnClear.Click += btnClear_Click;
            buttonPanel.Controls.Add(btnRegister);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnClear);
            AddSpanningControl(buttonPanel);

            linkPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 10, 0, 0)
            };
            llLogout = new LinkLabel
            {
                Text = "Logout",
                AutoSize = true,
                Margin = new Padding(0, 0, 12, 0)
            };
            llLogout.LinkClicked += llLogout_LinkClicked;

            llExit = new LinkLabel
            {
                Text = "Exit",
                AutoSize = true,
                Margin = new Padding(0)
            };
            llExit.LinkClicked += llExit_LinkClicked;

            linkPanel.Controls.Add(llLogout);
            linkPanel.Controls.Add(llExit);
            AddSpanningControl(linkPanel);
        }

        private Label AddLabel(string text, int x, int y)
        {
            Label label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true
            };

            Controls.Add(label);
            return label;
        }

        private TextBox AddTextBox(int x, int y, int width, string name, bool multiline = false)
        {
            TextBox textBox = new TextBox
            {
                Location = new Point(x, y),
                Width = width,
                Name = name
            };

            if (multiline)
            {
                textBox.Multiline = true;
                textBox.Height = 52;
            }

            Controls.Add(textBox);
            return textBox;
        }

        private TextBox CreateTextBox(string name, bool multiline = false)
        {
            TextBox textBox = new TextBox
            {
                Name = name,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 3, 0, 3)
            };

            if (multiline)
            {
                textBox.Multiline = true;
                textBox.Height = 72;
            }

            return textBox;
        }

        private void AddFieldRow(string labelText, Control control)
        {
            int rowIndex = formLayout.RowCount;
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Label label = new Label
            {
                Text = labelText,
                AutoSize = true,
                Anchor = AnchorStyles.Right,
                TextAlign = ContentAlignment.MiddleRight,
                Margin = new Padding(0, 7, 12, 0)
            };

            formLayout.Controls.Add(label, 0, rowIndex);
            control.Margin = new Padding(0, 3, 0, 3);
            formLayout.Controls.Add(control, 1, rowIndex);
            formLayout.RowCount++;
        }

        private void AddSpanningControl(Control control)
        {
            int rowIndex = formLayout.RowCount;
            formLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            control.Margin = new Padding(0, 6, 0, 0);
            formLayout.Controls.Add(control, 0, rowIndex);
            formLayout.SetColumnSpan(control, 2);
            formLayout.RowCount++;
        }

        private void TxtFirstName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                errorProvider.SetError(txtFirstName, "First Name is required");
            }
            else
            {
                errorProvider.SetError(txtFirstName, "");
            }
        }

        private void TxtLastName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                errorProvider.SetError(txtLastName, "Last Name is required");
            }
            else
            {
                errorProvider.SetError(txtLastName, "");
            }
        }

        private void TxtEmail_Leave(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                errorProvider.SetError(txtEmail, "Email is required");
            }
            else if (!_dbHelper.IsValidEmail(email))
            {
                errorProvider.SetError(txtEmail, "Invalid email format");
            }
            else
            {
                errorProvider.SetError(txtEmail, "");
            }
        }

        private void LoadRegNumbers()
        {
            try
            {
                isLoadingRegNumbers = true;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT regNo FROM Registration ORDER BY regNo";
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cboRegNo.DataSource = dt;
                    cboRegNo.DisplayMember = "regNo";
                    cboRegNo.ValueMember = "regNo";
                    if (dt.Rows.Count == 0)
                    {
                        cboRegNo.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to load registration numbers.\n{ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                isLoadingRegNumbers = false;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateFields()) 
                {
                    MessageBox.Show("Please fix the validation errors before registering.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Registration 
                                (regNo, firstName, lastName, dateOfBirth, gender, address, email, mobilePhone, homePhone, parentName, nic, contactNo)
                                VALUES (@regNo, @fName, @lName, @dob, @gender, @addr, @email, @mobile, @home, @parent, @nic, @contact)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@regNo", GetNextRegNo());
                    cmd.Parameters.AddWithValue("@fName", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@lName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@dob", dtpDOB.Value);
                    cmd.Parameters.AddWithValue("@gender", rbMale.Checked ? "Male" : "Female");
                    cmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@mobile", txtMobile.Text.Trim());
                    cmd.Parameters.AddWithValue("@home", txtHomePhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@parent", txtParentName.Text.Trim());
                    cmd.Parameters.AddWithValue("@nic", txtNIC.Text.Trim());
                    cmd.Parameters.AddWithValue("@contact", txtContactNo.Text.Trim());

                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadRegNumbers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed.\n\n{ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int regNo = GetSelectedRegNo();
                if (regNo <= 0) 
                {
                    MessageBox.Show("Please select a registration to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult res = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Registration WHERE regNo = @regNo";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@regNo", regNo);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record Deleted Successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                        LoadRegNumbers();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete failed.\n\n{ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingRegNumbers) return;

            int regNo = GetSelectedRegNo();
            if (regNo <= 0) return;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Registration WHERE regNo = @regNo";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@regNo", regNo);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        txtFirstName.Text = dr["firstName"].ToString();
                        txtLastName.Text = dr["lastName"].ToString();
                        dtpDOB.Value = Convert.ToDateTime(dr["dateOfBirth"]);
                        string gender = dr["gender"].ToString();
                        rbMale.Checked = gender == "Male";
                        rbFemale.Checked = gender == "Female";
                        txtAddress.Text = dr["address"].ToString();
                        txtEmail.Text = dr["email"].ToString();
                        txtMobile.Text = dr["mobilePhone"].ToString();
                        txtHomePhone.Text = dr["homePhone"].ToString();
                        txtParentName.Text = dr["parentName"].ToString();
                        txtNIC.Text = dr["nic"].ToString();
                        txtContactNo.Text = dr["contactNo"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to load registration details.\n{ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void llLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }

        private void llExit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearFields();

        private void ClearFields()
        {
            cboRegNo.SelectedIndex = -1;
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtMobile.Clear();
            txtHomePhone.Clear();
            txtParentName.Clear();
            txtNIC.Clear();
            txtContactNo.Clear();
            rbMale.Checked = false;
            rbFemale.Checked = false;
            dtpDOB.Value = DateTime.Now;
            errorProvider.Clear();
        }

        private bool ValidateFields()
        {
            errorProvider.Clear();
            bool isValid = true;

            // First Name validation
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                errorProvider.SetError(txtFirstName, "First Name is required");
                isValid = false;
            }

            // Last Name validation
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                errorProvider.SetError(txtLastName, "Last Name is required");
                isValid = false;
            }

            // Gender validation
            if (!rbMale.Checked && !rbFemale.Checked)
            {
                errorProvider.SetError(rbMale, "Please select a gender");
                isValid = false;
            }

            // Email validation
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                errorProvider.SetError(txtEmail, "Email is required");
                isValid = false;
            }
            else if (!_dbHelper.IsValidEmail(email))
            {
                errorProvider.SetError(txtEmail, "Invalid email format (e.g., user@domain.com)");
                isValid = false;
            }

            return isValid;
        }

        private int GetNextRegNo()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT ISNULL(MAX(regNo), 0) + 1 FROM Registration";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        private int GetSelectedRegNo()
        {
            if (cboRegNo.SelectedValue == null)
            {
                return 0;
            }

            if (int.TryParse(cboRegNo.SelectedValue.ToString(), out int regNo))
            {
                return regNo;
            }

            return 0;
        }
    }
}
