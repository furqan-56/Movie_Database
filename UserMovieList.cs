using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Database_Project
{
    public partial class UserMovieList : Form
    {
        string connectionString = @"Server=localhost;Database=project;Uid=root;Pwd=root";

        public UserMovieList()
        {
            InitializeComponent();
        }


        // Fetch and display movies from the database in DataGridView
        private void ShowMovies()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Movies";  // Query to fetch all movies
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);  // Fill the DataTable with the result of the query

                    // Display the data in the DataGridView
                    moviegrid.AutoGenerateColumns = true;
                    moviegrid.DataSource = dataTable;

                    // Set the grid as read-only
                    moviegrid.ReadOnly = true;

                    // Alternate row color
                    moviegrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 245);  // Light Gray

                    // Set the header style
                    moviegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255);  // Blue Header
                    moviegrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    moviegrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                    // Set row style
                    moviegrid.RowsDefaultCellStyle.BackColor = Color.White;
                    moviegrid.RowsDefaultCellStyle.ForeColor = Color.Black;
                    moviegrid.RowsDefaultCellStyle.Font = new Font("Segoe UI", 9);

                    // On hover, change the row background color
                    moviegrid.CellMouseEnter += (s, e) =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            moviegrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(220, 230, 250);  // Subtle light blue
                        }
                    };
                    moviegrid.CellMouseLeave += (s, e) =>
                    {
                        if (e.RowIndex >= 0)
                        {
                            moviegrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        }
                    };

                    // Optionally, make specific columns read-only
                    moviegrid.Columns["MovieID"].ReadOnly = true;  // Replace "MovieID" with the actual column name

                    // Adjust column widths
                    moviegrid.Columns["Title"].Width = 250;
                    moviegrid.Columns["ReleaseYear"].Width = 100;
                    moviegrid.Columns["Rating"].Width = 80;
                    moviegrid.Columns["PlotSummary"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // Optional: Adjust column header height
                    moviegrid.ColumnHeadersHeight = 30;

                    // Optional: Adjust row height
                    moviegrid.RowTemplate.Height = 30;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching movie data: " + ex.Message);
            }
        }


        private void exit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();  // Exits the entire application
        }

        private void hide_Click_1(object sender, EventArgs e)
        {
            this.Hide();  // Hides the current form
        }

        private void back_Click_1(object sender, EventArgs e)
        {
            UserMoviePage userMoviePage = new UserMoviePage();
            this.Close();  // Close the current form
            userMoviePage.Show();  // Show the AdminMoviePage
        }

        private void UserMovieList_Load(object sender, EventArgs e)
        {
            ShowMovies();  // Fetch and display movies when the page loads
        }
    }
}