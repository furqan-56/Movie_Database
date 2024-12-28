using System;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Database_Project
{
    public partial class AdminInsertPage : Form
    {
        string connectionString = @"Server=localhost;Database=project;Uid=root;Pwd=root";
        public AdminInsertPage()
        {
            InitializeComponent();
            moviepanel.Visible = false; actorpanel.Visible = false; genrepanel.Visible = false; producerpanel.Visible = false;
            directorpanel.Visible = false; moviecastpanel.Visible = false; prodcompanypanel.Visible = false;

            gendercombobox.Items.AddRange(new string[] { "Male", "Female", "Other" });
        }

        private void moviebutton_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            moviebutton.BackColor = Color.LightGreen;
            moviepanel.Visible = true; actorpanel.Visible = false; genrepanel.Visible = false; producerpanel.Visible = false;
            directorpanel.Visible = false; moviecastpanel.Visible = false; prodcompanypanel.Visible = false;
        }

        private void actorbutton_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            actorbutton.BackColor = Color.LightGreen;
            moviepanel.Visible = false; actorpanel.Visible = true; genrepanel.Visible = false; producerpanel.Visible = false;
            directorpanel.Visible = false; moviecastpanel.Visible = false; prodcompanypanel.Visible = false;
        }
        private void genrebutton_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            genrebutton.BackColor = Color.LightGreen; // Highlight the actor button
            moviepanel.Visible = false; actorpanel.Visible = false; genrepanel.Visible = true; producerpanel.Visible = false;
            directorpanel.Visible = false; moviecastpanel.Visible = false; prodcompanypanel.Visible = false;
        }
        private void producerbutton_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            producerbutton.BackColor = Color.LightGreen; // Highlight the actor button
            moviepanel.Visible = false; actorpanel.Visible = false; genrepanel.Visible = false; producerpanel.Visible = true;
            directorpanel.Visible = false; moviecastpanel.Visible = false; prodcompanypanel.Visible = false;
        }

        private void directorbutton_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            directorbutton.BackColor = Color.LightGreen; // Highlight the actor button
            moviepanel.Visible = false; actorpanel.Visible = false; genrepanel.Visible = false; producerpanel.Visible = false;
            directorpanel.Visible = true; moviecastpanel.Visible = false; prodcompanypanel.Visible = false;
        }

        private void mooviecastbutton_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            mooviecastbutton.BackColor = Color.LightGreen; // Highlight the actor button
            moviepanel.Visible = false; actorpanel.Visible = false; genrepanel.Visible = false; producerpanel.Visible = false;
            directorpanel.Visible = false; moviecastpanel.Visible = true; prodcompanypanel.Visible = false;
        }

        private void prodcompanybutton_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            prodcompanybutton.BackColor = Color.LightGreen; // Highlight the actor button
            moviepanel.Visible = false; actorpanel.Visible = false; genrepanel.Visible = false; producerpanel.Visible = false;
            directorpanel.Visible = false; moviecastpanel.Visible = false; prodcompanypanel.Visible = true;
        }



        private void ResetButtonColors()
        {
            moviebutton.BackColor = SystemColors.Control;
            actorbutton.BackColor = SystemColors.Control;
            genrebutton.BackColor = SystemColors.Control;
            producerbutton.BackColor = SystemColors.Control;
            directorbutton.BackColor = SystemColors.Control;
            mooviecastbutton.BackColor = SystemColors.Control;
            prodcompanybutton.BackColor = SystemColors.Control;

        }

        ////////////////////// MOVIES ///////////////////
        // Method to handle the insertion of movie data
        private void InsertPage_Load(object sender, EventArgs e)
        {
            PopulateFoundedYearOptions();
            PopulateCountryOptions();
        }

        // Helper method to populate ComboBox with database data
        private void PopulateComboBox(ComboBox comboBox, string query)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            comboBox.Items.Clear();  // Clear existing items

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    comboBox.Items.Add(reader[0].ToString());  // Add data to ComboBox
                                }
                            }

                            // Select the first item by default if ComboBox is not empty
                            if (comboBox.Items.Count > 0)
                            {
                                comboBox.SelectedIndex = 0; // Set default selection
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        // Method to handle the insertion of movie data
        private void InsertMovieData()
        {
            string title = titletextbox.Text.Trim();
            int releaseYear;
            string genreName = genretextbox.Text.Trim();
            decimal rating;
            int runtime;
            string plotSummary = plotsummarytextbox.Text.Trim();
            string directorName = directortextbox.Text.Trim();
            string producerName = producertextbox.Text.Trim();
            string companyName = prodcompanytextbox.Text.Trim();

            // Debugging message to ensure values are being retrieved correctly
            MessageBox.Show("Selected values:\n" +
                            "Title: " + title + "\n" +
                            "Genre: " + genreName + "\n" +
                            "Director: " + directorName + "\n" +
                            "Producer: " + producerName + "\n" +
                            "Company: " + companyName);

            // Validate fields
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Title cannot be empty.");
                return;
            }
            if (!int.TryParse(releaseyeartextbox.Text, out releaseYear) || releaseYear < 1900 || releaseYear > 2024)
            {
                MessageBox.Show("Release Year must be a valid year between 1900 and 2024.");
                return;
            }
            if (string.IsNullOrEmpty(genreName))
            {
                MessageBox.Show("Genre cannot be empty.");
                return;
            }
            if (!decimal.TryParse(ratingtextbox.Text, out rating) || rating < 0 || rating > 10)
            {
                MessageBox.Show("Rating must be a valid number between 0 and 10.");
                return;
            }
            if (!int.TryParse(runtimetextbox.Text, out runtime) || runtime <= 0 || runtime > 360)
            {
                MessageBox.Show("Runtime must be a valid number between 1 and 360 minutes.");
                return;
            }
            if (string.IsNullOrEmpty(plotSummary))
            {
                MessageBox.Show("Plot Summary cannot be empty.");
                return;
            }
            if (string.IsNullOrEmpty(directorName))
            {
                MessageBox.Show("Director cannot be empty.");
                return;
            }
            if (string.IsNullOrEmpty(producerName))
            {
                MessageBox.Show("Producer cannot be empty.");
                return;
            }
            if (string.IsNullOrEmpty(companyName))
            {
                MessageBox.Show("Production Company cannot be empty.");
                return;
            }

            // Insert movie data (No manual validation checks, relying on DB constraints)
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Movies (Title, ReleaseYear, GenreName, Rating, Runtime, PlotSummary, DirectorName, ProducerName, CompanyName) " +
                                   "VALUES (@Title, @ReleaseYear, @GenreName, @Rating, @Runtime, @PlotSummary, @DirectorName, @ProducerName, @CompanyName)";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@ReleaseYear", releaseYear);
                        cmd.Parameters.AddWithValue("@GenreName", genreName);
                        cmd.Parameters.AddWithValue("@Rating", rating);
                        cmd.Parameters.AddWithValue("@Runtime", runtime);
                        cmd.Parameters.AddWithValue("@PlotSummary", plotSummary);
                        cmd.Parameters.AddWithValue("@DirectorName", directorName);
                        cmd.Parameters.AddWithValue("@ProducerName", producerName);
                        cmd.Parameters.AddWithValue("@CompanyName", companyName);  // Use the ComboBox value

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Movie data inserted successfully!");
                    ClearMovieFields();  // Clear fields after insertion
                }
                catch (MySqlException ex)
                {
                    // Handle specific MySQL error codes to identify which constraint is violated
                    if (ex.Number == 1452)  // Foreign key constraint violation
                    {
                        MessageBox.Show("Referenced data (Genre, Director, Producer, Company) does not exist in related tables.");
                    }
                    else if (ex.Number == 1062)  // Duplicate entry (unique constraint violation)
                    {
                        MessageBox.Show("The title already exists in the database.");
                    }
                    else if (ex.Number == 1366)  // Incorrect data type (e.g., string data in an integer field)
                    {
                        MessageBox.Show("Invalid data format. Please check your inputs.");
                    }
                    else
                    {
                        MessageBox.Show("Error inserting data: " + ex.Message);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Clear method to reset the form fields
        private void ClearMovieFields()
        {
            titletextbox.Text = string.Empty;
            releaseyeartextbox.Text = string.Empty;
            ratingtextbox.Text = string.Empty;
            runtimetextbox.Text = string.Empty;
            plotsummarytextbox.Text = string.Empty;
            genretextbox.Text = string.Empty;
            directortextbox.Text = string.Empty;
            producertextbox.Text = string.Empty;
            companynametextbox.Text = string.Empty;

            titletextbox.Focus();
        }

        // Button to trigger movie insertion
        private void save_Click(object sender, EventArgs e)
        {
            InsertMovieData();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void hide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;  // Minimizes the form
        }

        /////////////////////////// ACTOR //////////////////////////////
        private void InsertActorData()
        {
            string fullName = actornametextbox.Text.Trim();
            DateTime dateOfBirth = dobirth.Value;
            string gender = gendercombobox.SelectedItem?.ToString(); // Retrieve selected gender
            string nationality = nationalitycombobox.SelectedItem?.ToString(); // Retrieve selected nationality
            string biography = biographytextbox.Text.Trim();

            // Validate required fields
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(nationality))
            {
                MessageBox.Show("Please ensure all required fields (Full Name, Gender, Nationality) are filled.");
                return;
            }

            // Insert data into the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to insert actor data
                    string query = "INSERT INTO Actors (FullName, DateOfBirth, Gender, Nationality, Biography) " +
                                   "VALUES (@FullName, @DateOfBirth, @Gender, @Nationality, @Biography)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Nationality", nationality);
                        cmd.Parameters.AddWithValue("@Biography", biography);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }

                    // Show success message
                    MessageBox.Show("Actor data inserted successfully!");

                    // Clear all input fields after successful insertion
                    ClearActorFields();
                }
                catch (MySqlException ex)
                {
                    // Handle specific MySQL error codes
                    if (ex.Number == 1062)  // UNIQUE constraint violation (actor already exists)
                    {
                        MessageBox.Show("An actor with this name already exists.");
                    }
                    else if (ex.Number == 1264)  // CHECK constraint violation (invalid data)
                    {
                        MessageBox.Show("Invalid data provided (e.g., Name or Nationality).");
                    }
                    else if (ex.Number == 1406)  // Data too long for column
                    {
                        MessageBox.Show("One or more fields exceed the maximum length.");
                    }
                    else if (ex.Number == 1452)  // Foreign key constraint violation (if any)
                    {
                        MessageBox.Show("A foreign key violation occurred.");
                    }
                    else
                    {
                        // General error message for other MySQL errors
                        MessageBox.Show("Error inserting actor data: " + ex.Message);
                    }
                }
            }
        }

        private void ClearActorFields()
        {
            // Clear all textboxes
            actornametextbox.Text = string.Empty;
            nationalitycombobox.SelectedIndex = -1; // Clear selection in nationality combobox
            biographytextbox.Text = string.Empty;

            // Reset date of birth to today's date
            dobirth.Value = DateTime.Now;

            // Reset gender combobox
            gendercombobox.SelectedIndex = -1; // Clear selection

            // Optionally reset focus to the first textbox
            actornametextbox.Focus();
        }

        private void saveactor_Click(object sender, EventArgs e)
        {
            InsertActorData();
        }


        ////////////////////////// GENRE /////////////////////////////////////
        private void savegenre_Click(object sender, EventArgs e)
        {
            string genreName = genrenametextbox.Text.Trim();

            // Validate the genre name
            if (string.IsNullOrEmpty(genreName))
            {
                MessageBox.Show("Please enter a valid Genre Name.");
                return;
            }

            // Check if the genre already exists in the Genres table
            if (CheckIfExist("Genres", "GenreName", genreName))
            {
                MessageBox.Show("Genre already exists.");
                return;
            }

            // Insert the genre into the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open the database connection
                    connection.Open();

                    // Prepare the query for inserting the genre
                    string query = "INSERT INTO Genres (GenreName) VALUES (@GenreName)";

                    // Create and configure the SqlCommand
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@GenreName", genreName);

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }

                    // Show success message
                    MessageBox.Show("Genre inserted successfully!");

                    // Clear the text box after inserting the genre
                    genrenametextbox.Clear();
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    MessageBox.Show("Error inserting genre: " + ex.Message);
                }
                finally
                {
                    // Ensure the connection is always closed
                    connection.Close();
                }
            }
        }

        // Method to check if a value exists in the table
        private bool CheckIfExist(string tableName, string columnName, string value)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to check if the value already exists
                    string query = $"SELECT COUNT(1) FROM {tableName} WHERE {columnName} = @Value";

                    // Create and configure the MySqlCommand
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Value", value);  // Ensure this is expecting a string

                        // Execute the query and return whether the value exists
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }




        ///////////// PRODUCER /////////////////////
        private void producernametextbox_TextChanged(object sender, EventArgs e)
        {
            // Enable save button only when producer's name is not empty
            saveproducer.Enabled = !string.IsNullOrWhiteSpace(producernametextbox.Text);
        }

        private void prodcompanynametextbox_TextChanged(object sender, EventArgs e)
        {
            // Enable save button only when production company name is not empty
            saveproducer.Enabled = !string.IsNullOrWhiteSpace(prodcompanynametextbox.Text) && !string.IsNullOrWhiteSpace(producernametextbox.Text);
        }

        private void saveproducer_Click(object sender, EventArgs e)
        {
            string producerName = producernametextbox.Text.Trim();
            string productionCompanyName = prodcompanynametextbox.Text.Trim();

            // Validate producer name
            if (string.IsNullOrEmpty(producerName))
            {
                MessageBox.Show("Please enter a valid Producer Name.");
                return;
            }

            // Check if production company exists
            if (!CheckIfProductionCompanyExists(productionCompanyName))
            {
                MessageBox.Show("The specified Production Company does not exist.");
                return;
            }

            // Insert producer into database
            InsertProducerData(producerName, productionCompanyName);
        }

        private bool CheckIfProductionCompanyExists(string companyName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to check if the production company exists
                    string query = "SELECT COUNT(1) FROM ProductionCompanies WHERE CompanyName = @CompanyName";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CompanyName", companyName);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking production company: " + ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void InsertProducerData(string producerName, string productionCompanyName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Prepare the query for inserting the producer with the company name
                    string query = "INSERT INTO Producers (FullName, CompanyName) VALUES (@FullName, @CompanyName)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FullName", producerName);
                        cmd.Parameters.AddWithValue("@CompanyName", productionCompanyName);
                        cmd.ExecuteNonQuery();
                    }

                    // Show success message
                    MessageBox.Show("Producer saved successfully!");

                    // Clear textboxes after successful insertion
                    producernametextbox.Clear();
                    prodcompanynametextbox.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting producer: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        /////////////////// DIRECTOR /////////////////////
        private void InsertDirectorData(string fullName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO Directors (FullName) VALUES (@FullName)";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Director inserted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    directornametextbox.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void directornametextbox_TextChanged(object sender, EventArgs e)
        {
            // Enable save button only when the TextBox is not empty
            savedirector.Enabled = !string.IsNullOrWhiteSpace(directornametextbox.Text);
        }

        private void savedirector_Click(object sender, EventArgs e)
        {
            string fullName = directornametextbox.Text.Trim();

            // Validate the director's full name
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Please enter the director's full name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Call Insert method to save the director data
            InsertDirectorData(fullName);
        }

        //////////////// Movie Cast //////////////////////
        private void InsertMovieCastData(string movieTitle, string actorName, string role)
        {
            string insertQuery = "INSERT INTO MovieCast (MovieTitle, ActorName, Role) VALUES (@MovieTitle, @ActorName, @Role)";

            if (string.IsNullOrEmpty(movieTitle))
            {
                MessageBox.Show("Please provide a valid Movie Title.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(actorName))
            {
                MessageBox.Show("Please provide a valid Actor Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please provide a valid role.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Check if MovieTitle exists in Movies table
                    string checkMovieQuery = "SELECT COUNT(1) FROM Movies WHERE Title = @MovieTitle";
                    using (MySqlCommand checkMovieCmd = new MySqlCommand(checkMovieQuery, connection))
                    {
                        checkMovieCmd.Parameters.AddWithValue("@MovieTitle", movieTitle);
                        int movieExists = Convert.ToInt32(checkMovieCmd.ExecuteScalar());

                        if (movieExists == 0)
                        {
                            MessageBox.Show($"Movie titled '{movieTitle}' does not exist in the Movies table.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Check if ActorName exists in Actors table
                    string checkActorQuery = "SELECT COUNT(1) FROM Actors WHERE FullName = @ActorName";
                    using (MySqlCommand checkActorCmd = new MySqlCommand(checkActorQuery, connection))
                    {
                        checkActorCmd.Parameters.AddWithValue("@ActorName", actorName);
                        int actorExists = Convert.ToInt32(checkActorCmd.ExecuteScalar());

                        if (actorExists == 0)
                        {
                            MessageBox.Show($"Actor '{actorName}' does not exist in the Actors table.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert Movie Cast data into MovieCast table
                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@MovieTitle", movieTitle);
                        insertCmd.Parameters.AddWithValue("@ActorName", actorName);
                        insertCmd.Parameters.AddWithValue("@Role", role);

                        insertCmd.ExecuteNonQuery(); // Execute the insert

                        MessageBox.Show("Movie Cast inserted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear the textboxes after successful insertion
                        moviecastMName.Clear();
                        moviecastAName.Clear();
                        roletextbox.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void moviecastsave_Click(object sender, EventArgs e)
        {
            string movieTitle = moviecastMName.Text.Trim();
            string actorName = moviecastAName.Text.Trim();
            string role = roletextbox.Text.Trim();

            // Validate the Movie Title, Actor Name, and Role
            if (string.IsNullOrEmpty(movieTitle))
            {
                MessageBox.Show("Please enter a valid Movie Title.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(actorName))
            {
                MessageBox.Show("Please enter a valid Actor Name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please enter the Role.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Call the method to insert data into MovieCast
            InsertMovieCastData(movieTitle, actorName, role);
        }

        ////////////////////  PRODUCTION COMPANY /////////////////

        private void InsertCompanyData(string companyName, string country, int foundedYear, string headquarters)
        {
            string query = "INSERT INTO ProductionCompanies (CompanyName, Country, FoundedYear, Headquarters) " +
                           "VALUES (@CompanyName, @Country, @FoundedYear, @Headquarters)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        // Add parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@CompanyName", companyName);
                        cmd.Parameters.AddWithValue("@Country", country);
                        cmd.Parameters.AddWithValue("@FoundedYear", foundedYear);
                        cmd.Parameters.AddWithValue("@Headquarters", headquarters);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data inserted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No data inserted.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Event handler for textboxes to validate input and insert when needed
        private void prodcompanysave_Click(object sender, EventArgs e)
        {
            string companyName = companynametextbox.Text.Trim();
            string country = countrycombobox.SelectedItem?.ToString(); // Get selected country from combo box
            string headquarters = headquarterstextbox.Text.Trim();

            // Validate the fields
            if (string.IsNullOrEmpty(companyName))
            {
                MessageBox.Show("Please enter a valid company name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(country))
            {
                MessageBox.Show("Please select a valid country.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(headquarters))
            {
                MessageBox.Show("Please enter a valid headquarters location.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate the founded year (it should be a valid number and greater than 1800)
            if (!int.TryParse(founderyearcombobox.SelectedItem?.ToString(), out int foundedYear) || foundedYear <= 1800)
            {
                MessageBox.Show("Please enter a valid founded year greater than 1800.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Call InsertCompanyData to insert the data into the database
            InsertCompanyData(companyName, country, foundedYear, headquarters);

            // Clear textboxes after saving data
            companynametextbox.Clear();
            countrycombobox.SelectedIndex = -1;
            headquarterstextbox.Clear();
            founderyearcombobox.SelectedIndex = -1;
        }

        private void back_Click(object sender, EventArgs e)
        {
            // Create an instance of MainPage
            AdminMoviePage adminmoviePage = new AdminMoviePage();

            // Close the current form
            this.Close();

            // Show the MainPage
            adminmoviePage.Show();
        }

        //////// EXTRA /////////////////

        private void PopulateCountryOptions()
        {
            // Example predefined list of countries
            List<string> countries = new List<string>
    {
        "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda", "Argentina", "Armenia",
        "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium",
        "Belize", "Benin", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria",
        "Burkina Faso", "Burundi", "Cabo Verde", "Cambodia", "Cameroon", "Canada", "Central African Republic", "Chad",
        "Chile", "China", "Colombia", "Comoros", "Congo", "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic",
        "Democratic Republic of the Congo", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador",
        "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Eswatini", "Ethiopia", "Fiji", "Finland",
        "France", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guatemala", "Guinea",
        "Guinea-Bissau", "Guyana", "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq",
        "Ireland", "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, North",
        "Korea, South", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein",
        "Lithuania", "Luxembourg", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands",
        "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco",
        "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger", "Nigeria",
        "North Macedonia", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru",
        "Philippines", "Poland", "Portugal", "Qatar", "Romania", "Russia", "Rwanda", "Saint Kitts and Nevis", "Saint Lucia",
        "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal",
        "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa",
        "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan",
        "Tanzania", "Thailand", "Timor-Leste", "Togo", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan",
        "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "Uruguay", "Uzbekistan",
        "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Yemen", "Zambia", "Zimbabwe"
            };

            // Clear the current items in the combobox
            nationalitycombobox.Items.Clear();
            countrycombobox.Items.Clear();

            // Add countries to the combobox
            nationalitycombobox.Items.AddRange(countries.ToArray());
            countrycombobox.Items.AddRange(countries.ToArray());
        }
        private void PopulateFoundedYearOptions()
        {
            int currentYear = DateTime.Now.Year;
            for (int year = DateTime.Now.Year; year >= 1900; year--)
            {
                founderyearcombobox.Items.Add(year.ToString());
            }
        }
    private void founderyearcombobox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}