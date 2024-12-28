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
    public partial class UserDetailPage : Form
    {
        string connectionString = @"Server=localhost;Database=project;Uid=root;Pwd=root";
        private int movieID;
        public UserDetailPage()
        {
            InitializeComponent();
        }


        // Method to load full movie details based on MovieID
        private void LoadMovieDetails(int movieID)
        {
            // SQL Query to fetch the full details of the selected movie
            string query = @"
                            SELECT Movies.MovieID, Movies.Title, Movies.ReleaseYear, Movies.Rating, Movies.Runtime, 
                            Movies.PlotSummary, Movies.GenreName, Directors.FullName AS DirectorName, 
                            Producers.FullName AS ProducerName, ProductionCompanies.CompanyName AS ProductionCompanyName
                            FROM Movies
                            JOIN Directors ON Movies.DirectorName = Directors.FullName
                            JOIN Producers ON Movies.ProducerName = Producers.FullName
                            JOIN ProductionCompanies ON Movies.CompanyName = ProductionCompanies.CompanyName
                            WHERE Movies.MovieID = @movieID"; // Use the passed movieID to fetch the data

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@movieID", movieID);  // Add the MovieID as a parameter

                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())  // Check if the movie data is available
                        {
                            // Assign data to TextBoxes
                            txttitle.Text = reader["Title"].ToString();
                            txtreleaseyear.Text = reader["ReleaseYear"].ToString();
                            txtrating.Text = reader["Rating"].ToString();
                            txtruntime.Text = reader["Runtime"].ToString() + " minutes";
                            txtgenrename.Text = reader["GenreName"].ToString();
                            txtdirectorname.Text = reader["DirectorName"].ToString();
                            txtproducername.Text = reader["ProducerName"].ToString();
                            txtprodcompany.Text = reader["ProductionCompanyName"].ToString();
                            txtplotsummary.Text = reader["PlotSummary"].ToString();  // Multi-line text in TextBox
                        }
                        else
                        {
                            MessageBox.Show("Movie details not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading movie details: " + ex.Message);
            }
        }

        private void hide_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  // Minimizes the form
        }

        private void exit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void back_Click(object sender, EventArgs e)
        {
            // Create an instance of UserMoviePage (the correct page to go back to)
            UserMoviePage ump = new UserMoviePage();
            this.Hide();
            ump.Show();
        }

        private void searchbutton_Click_1(object sender, EventArgs e)
        {

            string movieName = txtMovieName.Text.Trim();  // Get the movie name from TextBox

            if (string.IsNullOrEmpty(movieName))  // Check if the input is empty
            {
                MessageBox.Show("Please enter a movie name to search.");
                return;  // Exit the method if the user did not enter a movie name
            }

            // SQL query to search for the movie by title
            string query = @"
                            SELECT MovieID, Title FROM Movies
                            WHERE Title LIKE @movieName";  // Use LIKE to allow partial matching

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@movieName", "%" + movieName + "%");  // Add the movie name parameter with wildcard for partial match

                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())  // If a matching movie is found
                        {
                            movieID = Convert.ToInt32(reader["MovieID"]);  // Get the MovieID of the matched movie
                            LoadMovieDetails(movieID);  // Load the details of the selected movie
                        }
                        else
                        {
                            MessageBox.Show("No movie found with that name. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching for the movie: " + ex.Message);
            }
        }
    }
}
