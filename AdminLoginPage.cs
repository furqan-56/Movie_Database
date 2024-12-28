using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Database_Project
{
    public partial class AdminLoginPage : Form
    {
        string connectionString = @"Server=localhost;Database=project;Uid=root;Pwd=root";

        public AdminLoginPage()
        {
            InitializeComponent();
            pwd_textbox.UseSystemPasswordChar = true;
            hideeyeicon.Image = Properties.Resources.hideEYEicon;
        }

        private void user_textbox_Click_1(object sender, EventArgs e)
        {
            user_textbox.BackColor = Color.White;
            panel3.BackColor = Color.White;
            panel4.BackColor = SystemColors.Control;
            pwd_textbox.BackColor = SystemColors.Control;
        }

        private void pwd_textbox_Click_1(object sender, EventArgs e)
        {
            pwd_textbox.BackColor = Color.White;
            panel4.BackColor = Color.White;
            user_textbox.BackColor = SystemColors.Control;
            panel3.BackColor = SystemColors.Control;
        }

        private void pwd_MouseUp_1(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = true;
        }

        private void pwd_MouseDown_1(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = false;
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void hide_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  // Minimizes the form
        }

        private void back_Click_1(object sender, EventArgs e)
        {
            // Create an instance of MainPage
            StartPage startpage = new StartPage();

            // Close the current form
            this.Close();

            // Show the MainPage
            startpage.Show();
        }

        private void login_Click_1(object sender, EventArgs e)
        {
            string username = user_textbox.Text;
            string password = pwd_textbox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate login credentials against the Admins table
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to check if the username exists in the Admins table
                    string query = "SELECT * FROM Admins WHERE Username = @username";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@username", username);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();  // Move to the first record

                        string storedPasswordHash = reader["Password"].ToString();  // Get the stored hashed password

                        // Verify the entered password with the stored hashed password
                        if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash))
                        {
                            // If the passwords match, open the AdminMoviePage
                            AdminMoviePage moviePage = new AdminMoviePage();
                            moviePage.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void signup_Click_1(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminSignupPage)
                {
                    form.BringToFront();
                    form.Focus();
                    return;
                }
            }

            AdminSignupPage adminsignupage = new AdminSignupPage();
            adminsignupage.Show();

            this.Hide();
        }

        private void hideeyeicon_MouseDown_1(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = false;
            hideeyeicon.Image = Properties.Resources.showEYEicon;
        }

        private void hideeyeicon_MouseUp_1(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = true;
            hideeyeicon.Image = Properties.Resources.hideEYEicon;
        }

        private void showeyeicon_MouseUp_1(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = true;
            hideeyeicon.Image = Properties.Resources.showEYEicon;
        }

        private void showeyeicon_MouseDown_1(object sender, MouseEventArgs e)
        {
            pwd_textbox.UseSystemPasswordChar = false;
            hideeyeicon.Image = Properties.Resources.showEYEicon;
        }
    }
}
