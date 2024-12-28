using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace Database_Project
{
    public partial class AdminSignupPage : Form
    {
        public AdminSignupPage()
        {
            InitializeComponent();
            password.UseSystemPasswordChar = true;
            hideeyeicon.Image = Properties.Resources.hideEYEicon;
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Validate First Name
        private void firstname_Leave(object sender, EventArgs e)
        {
            if (!IsValidName(firstname.Text))
            {
                firstname.BackColor = System.Drawing.Color.Red; // Highlight the field
                MessageBox.Show("First name must only contain alphabets.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                firstname.BackColor = System.Drawing.Color.White; // Reset background color
            }
        }

        // Validate Last Name
        private void lastname_Leave(object sender, EventArgs e)
        {
            if (!IsValidName(lastname.Text))
            {
                lastname.BackColor = System.Drawing.Color.Red; // Highlight the field
                MessageBox.Show("Last name must only contain alphabets.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                lastname.BackColor = System.Drawing.Color.White; // Reset background color
            }
        }

        // Validate Username
        private void username_Leave(object sender, EventArgs e)
        {
            if (!IsValidUsername(username.Text))
            {
                username.BackColor = System.Drawing.Color.Red; // Highlight the field
                MessageBox.Show("Username must be at least 8 characters long and contain only alphanumeric characters and special characters (!@#$%^&*()_+-=).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                username.BackColor = System.Drawing.Color.White; // Reset background color
            }
        }

        // Validate Email
        private void email_Leave(object sender, EventArgs e)
        {
            if (!IsValidEmail(email.Text))
            {
                email.BackColor = System.Drawing.Color.Red; // Highlight the field
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                email.BackColor = System.Drawing.Color.White; // Reset background color
            }
        }

        // Validate Password
        private void password_Leave(object sender, EventArgs e)
        {
            if (!IsValidPassword(password.Text))
            {
                password.BackColor = System.Drawing.Color.Red; // Highlight the field
                MessageBox.Show("Password must be at least 8 characters long and include at least one letter, one number, and one special character.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                password.BackColor = System.Drawing.Color.White; // Reset background color
            }

            // Check password strength
            string strength = GetPasswordStrength(password.Text);
            MessageBox.Show($"Password Strength: {strength}", "Password Strength", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Check password strength
        private string GetPasswordStrength(string password)
        {
            if (password.Length >= 12 && Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{12,}$"))
            {
                return "Strong";
            }
            else if (password.Length >= 8 && Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$"))
            {
                return "Medium";
            }
            else
            {
                return "Weak";
            }
        }

        // Validate and Save Data
        private void save_Click(object sender, EventArgs e)
        {
            // Validate all fields before saving
            if (!ValidateAllFields())
            {
                return; // Stop if validation fails
            }

            try
            {
                // Check if username or email already exists
                if (CheckIfUserExists(username.Text, email.Text))
                {
                    MessageBox.Show("Username or email already exists. Please use a different one.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Save data to database
                bool isSaved = SaveDataToDatabase(firstname.Text, lastname.Text, username.Text, email.Text, password.Text);

                if (isSaved)
                {
                    // Clear all textboxes
                    firstname.Text = string.Empty;
                    lastname.Text = string.Empty;
                    username.Text = string.Empty;
                    email.Text = string.Empty;
                    password.Text = string.Empty;

                    MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Navigate to login page
                    AdminLoginPage loginPage = new AdminLoginPage();
                    loginPage.Show();
                    this.Hide();
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("admins_chk_1")) // Example: map to a specific constraint
                {
                    MessageBox.Show("First name must only contain alphabets.", "Constraint Violation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (ex.Message.Contains("admins_chk_2"))
                {
                    MessageBox.Show("Last name must only contain alphabets.", "Constraint Violation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"An error occurred while saving: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Validate All Fields
        private bool ValidateAllFields()
        {
            if (!IsValidName(firstname.Text))
            {
                firstname.BackColor = System.Drawing.Color.Red;
                MessageBox.Show("First name must only contain alphabets.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidName(lastname.Text))
            {
                lastname.BackColor = System.Drawing.Color.Red;
                MessageBox.Show("Last name must only contain alphabets.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidUsername(username.Text))
            {
                username.BackColor = System.Drawing.Color.Red;
                MessageBox.Show("Username must be at least 8 characters long and contain only alphanumeric characters and special characters (!@#$%^&*()_+-=).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidEmail(email.Text))
            {
                email.BackColor = System.Drawing.Color.Red;
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidPassword(password.Text))
            {
                password.BackColor = System.Drawing.Color.Red;
                MessageBox.Show("Password must be at least 8 characters long and include at least one letter, one number, and one special character.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Reset colors if all fields are valid
            firstname.BackColor = System.Drawing.Color.White;
            lastname.BackColor = System.Drawing.Color.White;
            username.BackColor = System.Drawing.Color.White;
            email.BackColor = System.Drawing.Color.White;
            password.BackColor = System.Drawing.Color.White;

            return true;
        }

        // Validation Functions
        private bool IsValidName(string name)
        {
            return Regex.IsMatch(name, "^[A-Za-z]+$"); // Only alphabets
        }

        private bool IsValidUsername(string username)
        {
            return username.Length >= 8 && Regex.IsMatch(username, "^[A-Za-z0-9!@#\\$%^&*()_+\\-=]+$");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 && Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$");
        }

        // Check if user exists in database
        private bool CheckIfUserExists(string username, string email)
        {
            string connectionString = "server=localhost;database=project;uid=root;pwd=root;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Admins WHERE Username = @Username OR Email = @Email";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        // Save Data to Database
        private bool SaveDataToDatabase(string firstname, string lastname, string username, string email, string password)
        {
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                string connectionString = "server=localhost;database=project;uid=root;pwd=root;";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Admins (FirstName, LastName, Username, Email, Password) VALUES (@FirstName, @LastName, @Username, @Email, @Password)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstname);
                        cmd.Parameters.AddWithValue("@LastName", lastname);
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private void hide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  // Minimizes the form
        }

        private void back_Click(object sender, EventArgs e)
        {
            // Create an instance of MainPage
            AdminLoginPage adminpage = new AdminLoginPage();

            // Close the current form
            this.Close();

            // Show the MainPage
            adminpage.Show();
        }

        private void hideeyeicon_MouseUp_1(object sender, MouseEventArgs e)
        {
            password.UseSystemPasswordChar = true;

            hideeyeicon.Image = Properties.Resources.hideEYEicon;
        }

        private void hideeyeicon_MouseDown_1(object sender, MouseEventArgs e)
        {
            password.UseSystemPasswordChar = false;

            hideeyeicon.Image = Properties.Resources.showEYEicon;
        }

        private void showeyeicon_MouseDown_1(object sender, MouseEventArgs e)
        {
            password.UseSystemPasswordChar = false;

            hideeyeicon.Image = Properties.Resources.showEYEicon;
        }

        private void showeyeicon_MouseUp_1(object sender, MouseEventArgs e)
        {
            password.UseSystemPasswordChar = true;

            hideeyeicon.Image = Properties.Resources.hideEYEicon;
        }
    }
}
