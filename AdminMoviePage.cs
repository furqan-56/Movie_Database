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
    public partial class AdminMoviePage : Form
    {
        public AdminMoviePage()
        {
            InitializeComponent();
        }

        private void exit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void hide_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  // Minimizes the form
        }

        private void update_Click_1(object sender, EventArgs e)
        {
            // Check if UpdatePage is already open, if not open it
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminUpdatePage)
                {
                    form.BringToFront();
                    form.Focus();
                    return;
                }
            }

            AdminUpdatePage updatePage = new AdminUpdatePage();
            updatePage.Show();
            this.Hide();  // Hide the current page (MMoviePage)
        }

        private void filter_Click_1(object sender, EventArgs e)
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

            AdminFilterPage filterPage = new AdminFilterPage();
            filterPage.Show();
            this.Hide();  // Hide the current page (MMoviePage)
        }


        private void insert_Click_1(object sender, EventArgs e)
        {
            // Check if InsertPage is already open, if not open it
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminInsertPage)
                {
                    form.BringToFront();
                    form.Focus();
                    return;
                }
            }

            AdminInsertPage insertPage = new AdminInsertPage();
            insertPage.Show();
            this.Hide();  // Hide the current page (MMoviePage)
        }

        private void detail_Click(object sender, EventArgs e)
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

            AdminDetailPage searchPage = new AdminDetailPage();
            searchPage.Show();
            this.Hide();  // Hide the current page (MMoviePage)
        }

        private void movielistbutton_Click(object sender, EventArgs e)
        {
           
            foreach (Form form in Application.OpenForms)
            {
                if (form is AdminMovieList)
                {
                    form.BringToFront();
                    form.Focus();
                    return;
                }
            }

            AdminMovieList movielist = new AdminMovieList();
            movielist.Show();
            this.Hide();  // Hide the current page (MMoviePage)
        }
    }
}
