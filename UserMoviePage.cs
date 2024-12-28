using System;
using System.Windows.Forms;

namespace Database_Project
{
    public partial class UserMoviePage : Form
    {
        public UserMoviePage()
        {
            InitializeComponent();
        }

        // Exit the application
        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Navigate to FilterPage when the Filter button is clicked
        private void filter_Click(object sender, EventArgs e)
        {
            // Check if FilterPage is already open, if not open it
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminFilterPage)
                {
                    form.BringToFront();
                    form.Focus();
                    return;
                }
            }

            UserFilterPage ufilterPage = new UserFilterPage();
            ufilterPage.Show();
            this.Hide();  // Hide the current page (MMoviePage)
        }

        // Navigate to SearchPage when the Search button is clicked
        private void search_Click(object sender, EventArgs e)
        {
            // Check if SearchPage is already open, if not open it
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminDetailPage)
                {
                    form.BringToFront();
                    form.Focus();
                    return;
                }
            }

            UserDetailPage usearchPage = new UserDetailPage();
            usearchPage.Show();
            this.Hide();  // Hide the current page (MMoviePage)
        }

        private void hide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  // Minimizes the form
        }
    }
}
