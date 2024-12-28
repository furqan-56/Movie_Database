using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;

namespace Database_Project
{
    public partial class UserLoginPage : Form
    {
        string connectionString = @"Server=localhost;Database=project;Uid=root;Pwd=root";

        public UserLoginPage()
        {
            InitializeComponent();
            pwd_textbox.UseSystemPasswordChar = true;
            hideeyeicon.Image = Properties.Resources.hideEYEicon;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void user_textbox_Click(object sender, EventArgs e)
        {
            user_textbox.BackColor = System.Drawing.Color.White;
            panel3.BackColor = System.Drawing.Color.White;
            panel4.BackColor = System.Drawing.SystemColors.Control;
            pwd_textbox.BackColor = System.Drawing.SystemColors.Control;
        }

        private void pwd_textbox_Click(object sender, EventArgs e)
        {
            pwd_textbox.BackColor = System.Drawing.Color.White;
            panel4.BackColor = System.Drawing.Color.White;
            user_textbox.BackColor = System.Drawing.SystemColors.Control;
            panel3.BackColor = System.Drawing.SystemColors.Control;
        }

        private void pwd_MouseDown(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = false;
        }

        private void pwd_MouseUp(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = true;
        }

        private void signup_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is UserSignupPage)
                {
                    form.BringToFront();
                    form.Focus();
                    return;
                }
            }

            UserSignupPage mainPage = new UserSignupPage();
            mainPage.Show();
            this.Hide();
        }

        private void login_Click(object sender, EventArgs e)
        {
            string username = user_textbox.Text;
            string password = pwd_textbox.Text;

            // Ensure fields are not empty
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate username and password against constraints
            if (!IsValidUsername(username))
            {
                MessageBox.Show("Username must be at least 8 characters long and contain only alphanumeric characters and special symbols (!@#$%^&*()_+-=).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsValidPassword(password))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain only alphanumeric characters and special symbols (!@#$%^&*()_+-=).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate login credentials against the Users table ONLY
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check in Users table ONLY (modified query)
                    string query = "SELECT * FROM Users WHERE Username = @username";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        string storedPasswordHash = reader["Password"].ToString();

                        // Verify the password using BCrypt
                        if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash))
                        {
                            // If password matches, open the User Dashboard page
                            UserMoviePage usermoviepage = new UserMoviePage();
                            usermoviepage.Show();
                            this.Hide();
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // No user found with the given username
                        MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    // Handle specific database constraint violations
                    if (ex.Message.Contains("users_chk_1"))
                    {
                        MessageBox.Show("Invalid username. Please follow the required format.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (ex.Message.Contains("users_chk_2"))
                    {
                        MessageBox.Show("Invalid password. Please follow the required format.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void hideeyeicon_MouseDown(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = false;
            hideeyeicon.Image = Properties.Resources.showEYEicon;
        }

        private void hideeyeicon_MouseUp(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = true;
            hideeyeicon.Image = Properties.Resources.hideEYEicon;
        }

        private void showeyeicon_MouseDown(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = false;
            hideeyeicon.Image = Properties.Resources.showEYEicon;
        }

        private void showeyeicon_MouseUp(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = true;
            hideeyeicon.Image = Properties.Resources.hideEYEicon;
        }

        private void hide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  // Minimizes the form
        }

        private void back_Click(object sender, EventArgs e)
        {
            // Create an instance of StartPage
            StartPage startpage = new StartPage();

            // Close the current form
            this.Close();

            // Show the StartPage
            startpage.Show();
        }

        // Method to validate the username
        private bool IsValidUsername(string username)
        {
            string pattern = @"^[A-Za-z0-9!@#\$%^&*()_+\-=]+$"; // Fixed regex
            return username.Length >= 8 && Regex.IsMatch(username, pattern);
        }

        // Method to validate the password
        private bool IsValidPassword(string password)
        {
            string pattern = @"^[A-Za-z0-9!@#\$%^&*()_+\-=]+$"; // Fixed regex
            return password.Length >= 8 && Regex.IsMatch(password, pattern);
        }
    }
}
