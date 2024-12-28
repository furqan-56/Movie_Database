using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Database_Project
{
    public partial class StartPage : Form
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void admin_Click(object sender, EventArgs e)
        {
            // Create an instance of the UserLoginPage form
            AdminLoginPage adminLoginPage = new AdminLoginPage();

            // Show the UserLoginPage form
            adminLoginPage.Show();

            // Optionally, hide the current Admin form
            this.Hide();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void user_Click(object sender, EventArgs e)
        {
            // Create an instance of the UserLoginPage form
            UserLoginPage userLoginPage = new UserLoginPage();

            // Show the UserLoginPage form
            userLoginPage.Show();

            // Optionally, hide the current Admin form
            this.Hide();
        }
    }
}
