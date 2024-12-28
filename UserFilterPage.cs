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
    public partial class UserFilterPage : Form
    {
        string connectionString = @"Server=localhost;Database=project;Uid=root;Pwd=root";
        public UserFilterPage()
        {
            InitializeComponent();
        }

        private void filterbutton_Click_1(object sender, EventArgs e)
        {

            string titleFilter = movietextbox.Text.Trim();

            // Handle Genre Filter
            string genreFilter = genrecombo.SelectedItem?.ToString();
            if (genreFilter == "Select") genreFilter = "";  // Treat "Select" as no filter

            // Handle Release Year Filter
            int releaseYear = 0;
            if (releaseyearcombo.SelectedItem != null && releaseyearcombo.SelectedItem.ToString() != "Select" &&
                int.TryParse(releaseyearcombo.SelectedItem.ToString(), out int year))
            {
                releaseYear = year;
            }

            // Handle Rating Filter
            decimal ratingFilter = 0;
            string selectedRating = ratingcombo.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedRating) && selectedRating != "Select" && selectedRating.StartsWith(">"))
            {
                if (decimal.TryParse(selectedRating.Substring(1), out ratingFilter))
                {
                    ratingFilter += 0.1m;  // Adjust rating value if necessary
                }
            }

            // Handle Alphabet Filter
            string alphabet = alphabeticcombobox.SelectedItem?.ToString();
            if (alphabet == "Select")
            {
                alphabet = ""; // If "Select" is chosen, treat it as no filter.
            }

            // Check if at least one filter is selected
            bool isAnyFilterSelected = !string.IsNullOrEmpty(titleFilter) ||
                                       !string.IsNullOrEmpty(genreFilter) ||
                                       releaseYear > 0 ||
                                       ratingFilter > 0 ||
                                       !string.IsNullOrEmpty(alphabet);

            if (!isAnyFilterSelected)
            {
                MessageBox.Show("Please provide at least one filter criteria.", "No Filters", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Proceed with fetching and displaying movies
            FetchAndDisplayMovies(titleFilter, genreFilter, releaseYear, ratingFilter, alphabet);
        }


        private void FetchAndDisplayMovies(string title, string genre, int releaseYear, decimal rating, string alphabet)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Title, GenreName AS Genre, ReleaseYear, Rating FROM Movies WHERE 1=1";

                    if (!string.IsNullOrEmpty(title)) query += " AND Title LIKE @Title";
                    if (!string.IsNullOrEmpty(genre)) query += " AND GenreName = @Genre";
                    if (releaseYear > 0) query += " AND ReleaseYear = @ReleaseYear";
                    if (rating > 0) query += " AND Rating >= @Rating";
                    if (!string.IsNullOrEmpty(alphabet)) query += " AND Title LIKE @Alphabet"; // Added alphabet filter

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(title)) cmd.Parameters.AddWithValue("@Title", $"%{title}%");
                        if (!string.IsNullOrEmpty(genre)) cmd.Parameters.AddWithValue("@Genre", genre);
                        if (releaseYear > 0) cmd.Parameters.AddWithValue("@ReleaseYear", releaseYear);
                        if (rating > 0) cmd.Parameters.AddWithValue("@Rating", rating);
                        if (!string.IsNullOrEmpty(alphabet)) cmd.Parameters.AddWithValue("@Alphabet", $"{alphabet}%"); // Pass alphabet as parameter

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable moviesTable = new DataTable();
                            adapter.Fill(moviesTable);

                            // Apply styling to the DataGridView
                            datagrid.DataSource = moviesTable;
                            StyleGridView();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StyleGridView()
        {
            // Set the default cell style for the DataGridView
            datagrid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagrid.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular); // Modern font for content
            datagrid.DefaultCellStyle.BackColor = Color.WhiteSmoke; // Light background color for rows (softer than pure white)

            // Set alternating row color for improved readability
            datagrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245); // Light gray for alternating rows
            datagrid.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black; // Standard black text for alternating rows

            // Set row height for better readability
            datagrid.RowTemplate.Height = 35; // Adequate row height for readability

            // Set column header style (normal font, professional background)
            datagrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold); // Bold font for headers for emphasis
            datagrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 123, 255); // Blue background for headers
            datagrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // White text color for contrast
            datagrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            datagrid.ColumnHeadersDefaultCellStyle.Padding = new Padding(5); // Add some padding to the header for a less cramped look

            // Set the background color for the rows on hover (when user hovers over the rows)
            datagrid.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255); // Highlight with the same blue on hover
            datagrid.RowsDefaultCellStyle.SelectionForeColor = Color.White; // White text color when row is selected

            // Apply a smooth transition for hover effect (for a better user experience)
            datagrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 123, 255); // Apply same color for selected rows as hover effect

            // Auto-size columns to fit the content, and ensure they don't overflow
            foreach (DataGridViewColumn column in datagrid.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Make columns expand to fill available space
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void hide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void back_Click(object sender, EventArgs e)
        {
            UserMoviePage usermoviePage = new UserMoviePage();
            this.Close();
            usermoviePage.Show();
        }

        private void genrecombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void releaseyearcombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ratingcombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void alphabeticcombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UserFilterPage_Load(object sender, EventArgs e)
        {

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Add "Select" option to Genre ComboBox
                    genrecombo.Items.Clear();
                    genrecombo.Items.Add("Select");
                    using (MySqlCommand cmd = new MySqlCommand("SELECT GenreName FROM Genres", conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            genrecombo.Items.Add(reader.GetString("GenreName"));
                        }
                    }

                    // Add "Select" option to Release Year ComboBox
                    releaseyearcombo.Items.Clear();
                    releaseyearcombo.Items.Add("Select");
                    for (int year = DateTime.Now.Year; year >= 1900; year--)
                    {
                        releaseyearcombo.Items.Add(year.ToString());
                    }

                    // Add "Select" option to Rating ComboBox
                    ratingcombo.Items.Clear();
                    ratingcombo.Items.Add("Select");
                    for (decimal rating = 1; rating <= 10; rating++)
                    {
                        ratingcombo.Items.Add($">{rating}");
                    }

                    // Add "Select" option to Alphabet ComboBox
                    alphabeticcombobox.Items.Clear();
                    alphabeticcombobox.Items.Add("Select");
                    for (char letter = 'A'; letter <= 'Z'; letter++)
                    {
                        alphabeticcombobox.Items.Add(letter.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while populating data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
