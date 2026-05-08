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
            _dbHelper = new DatabaseHelper();
            InitializeRegistrationControls();
            LoadRegNumbers();
        }

        private void InitializeRegistrationControls()
        {
            Text = "Skills International School - Registration";
            ClientSize = new Size(760, 640);
            StartPosition = FormStartPosition.CenterScreen;
            AutoScroll = true;

            int labelX = 35;
            int inputX = 170;
            int y = 25;
            int rowGap = 34;
            int inputWidth = 240;

            AddLabel("Registration No", labelX, y);
            cboRegNo = new ComboBox
            {
                Location = new Point(inputX, y - 3),
                Width = inputWidth,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cboRegNo"
            };
            cboRegNo.SelectedIndexChanged += cboRegNo_SelectedIndexChanged;
            Controls.Add(cboRegNo);

            y += rowGap;
            AddLabel("First Name *", labelX, y);
            txtFirstName = AddTextBox(inputX, y - 3, inputWidth, "txtFirstName");
            txtFirstName.Leave += TxtFirstName_Leave;

            y += rowGap;
            AddLabel("Last Name *", labelX, y);
            txtLastName = AddTextBox(inputX, y - 3, inputWidth, "txtLastName");
            txtLastName.Leave += TxtLastName_Leave;

            y += rowGap;
            AddLabel("Date of Birth", labelX, y);
            dtpDOB = new DateTimePicker
            {
                Location = new Point(inputX, y - 3),
                Width = inputWidth,
                Name = "dtpDOB"
            };
            Controls.Add(dtpDOB);

            y += rowGap;
            AddLabel("Gender *", labelX, y);
            rbMale = new RadioButton
            {
                Text = "Male",
                Location = new Point(inputX, y - 2),
                AutoSize = true,
                Name = "rbMale"
            };
            rbFemale = new RadioButton
            {
                Text = "Female",
                Location = new Point(inputX + 90, y - 2),
                AutoSize = true,
                Name = "rbFemale"
            };
            Controls.Add(rbMale);
            Controls.Add(rbFemale);

            y += rowGap;
            AddLabel("Address", labelX, y);
            txtAddress = AddTextBox(inputX, y - 3, inputWidth, "txtAddress", true);

            y += rowGap;
            AddLabel("Email *", labelX, y);
            txtEmail = AddTextBox(inputX, y - 3, inputWidth, "txtEmail");
            txtEmail.Leave += TxtEmail_Leave;

            y += rowGap;
            AddLabel("Mobile Phone", labelX, y);
            txtMobile = AddTextBox(inputX, y - 3, inputWidth, "txtMobile");

            y += rowGap;
            AddLabel("Home Phone", labelX, y);
            txtHomePhone = AddTextBox(inputX, y - 3, inputWidth, "txtHomePhone");

            y += rowGap;
            AddLabel("Parent Name", labelX, y);
            txtParentName = AddTextBox(inputX, y - 3, inputWidth, "txtParentName");

            y += rowGap;
            AddLabel("NIC", labelX, y);
            txtNIC = AddTextBox(inputX, y - 3, inputWidth, "txtNIC");

            y += rowGap;
            AddLabel("Contact No", labelX, y);
            txtContactNo = AddTextBox(inputX, y - 3, inputWidth, "txtContactNo");

            btnRegister = new Button
            {
                Text = "Register",
                Location = new Point(35, y + 45),
                Width = 90
            };
            btnRegister.Click += btnRegister_Click;

            btnDelete = new Button
            {
                Text = "Delete",
                Location = new Point(140, y + 45),
                Width = 90
            };
            btnDelete.Click += btnDelete_Click;

            btnClear = new Button
            {
                Text = "Clear",
                Location = new Point(245, y + 45),
                Width = 90
            };
            btnClear.Click += btnClear_Click;

            llLogout = new LinkLabel
            {
                Text = "Logout",
                Location = new Point(35, y + 95),
                AutoSize = true
            };
            llLogout.LinkClicked += llLogout_LinkClicked;

            llExit = new LinkLabel
            {
                Text = "Exit",
                Location = new Point(100, y + 95),
                AutoSize = true
            };
            llExit.LinkClicked += llExit_LinkClicked;

            Controls.Add(btnRegister);
            Controls.Add(btnDelete);
            Controls.Add(btnClear);
            Controls.Add(llLogout);
            Controls.Add(llExit);
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
