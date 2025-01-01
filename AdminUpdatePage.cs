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
    public partial class AdminUpdatePage : Form
    {
        string connectionString = @"Server=localhost;Database=project;Uid=root;Pwd=root";

        private void UpdatePage_Load(object sender, EventArgs e)
        {
            PopulateGenres();
            PopulateDirectors();
            PopulateProducers();
            PopulateCompanies();
            PopulateYearComboBox();
            PopulateCountryComboBox();
            PopulateCompanyComboBox();
        }

        // Populate ComboBox for Year (1900 to 2024)
        private void PopulateYearComboBox()
        {
            // Loop from 2024 to 1900
            for (int year = 2024; year >= 1900; year--)
            {
                releaseyearcombobox.Items.Add(year.ToString());
                founderyearcombobox.Items.Add(year.ToString());
            }
        }

        private void PopulateCountryComboBox()
        {
            // A string of countries separated by commas
            string countries = "Afghanistan, Albania, Algeria, Andorra, Angola, Antigua and Barbuda, Argentina, Armenia, Australia, Austria, Azerbaijan, Bahamas, Bahrain, Bangladesh, Barbados, Belarus, Belgium, Belize, Benin, Bhutan, Bolivia, Bosnia and Herzegovina, Botswana, " +
                "Brazil, Brunei, Bulgaria, Burkina Faso, Burundi, Cabo Verde, Cambodia, Cameroon, Canada, Central African Republic, Chad, Chile, China, Colombia, Comoros, Costa Rica, Croatia, Cuba, Cyprus, Czech Republic, DR Congo, Denmark, Djibouti, Dominica, Dominican Republic, East Timor, Ecuador, Egypt, El Salvador, Equatorial Guinea, Eritrea, Estonia, Eswatini, Ethiopia, Fiji, Finland, France, Gabon, Gambia, Georgia, Germany, " +
                "Ghana, Greece, Grenada, Guatemala, Guinea, Guinea-Bissau, Guyana, Haiti, Honduras, Hungary, Iceland, India, Indonesia, Iran, Iraq, Ireland, Israel, Italy, Jamaica, Japan, Jordan, Kazakhstan, Kenya, Kiribati, Korea, Kuwait, Kyrgyzstan, Laos, Latvia, Lebanon, Lesotho, Liberia, Libya, Liechtenstein, Lithuania, Luxembourg, Madagascar, Malawi, Malaysia, Maldives, Mali, Malta, Marshall Islands, Mauritania, Mauritius, Mexico," +
                " Micronesia, Moldova, Monaco, Mongolia, Montenegro, Morocco, Mozambique, Myanmar, Namibia, Nauru, Nepal, Netherlands, New Zealand, Nicaragua, Niger, Nigeria, North Macedonia, Norway, Oman, Pakistan, Palau, Panama, Papua New Guinea, Paraguay, Peru, Philippines, Poland, Portugal, Qatar, Romania, Russia, Rwanda, Saint Kitts and Nevis, Saint Lucia, Saint Vincent and the Grenadines, Samoa, San Marino, Sao Tome and Principe, Saudi Arabia, " +
                "Senegal, Serbia, Seychelles, Sierra Leone, Singapore, Slovakia, Slovenia, Solomon Islands, Somalia, South Africa, South Korea, South Sudan, Spain, Sri Lanka, Sudan, Suriname, Sweden, Switzerland, Syria, Taiwan, Tajikistan, Tanzania, Thailand, Togo, Tonga, Trinidad and Tobago, Tunisia, Turkey, Turkmenistan, Tuvalu, Uganda, Ukraine, United Arab Emirates, United Kingdom, United States, Uruguay, Uzbekistan, Vanuatu, Vatican City," +
                " Venezuela, Vietnam, Yemen, Zambia, Zimbabwe.";

            // Split the countries string by commas to create an array of countries
            string[] countryList = countries.Split(new string[] { ", " }, StringSplitOptions.None);

            // Add each country to the comboboxes
            foreach (var country in countryList)
            {
                countrycombobox.Items.Add(country);
                nationalitycombobox.Items.Add(country);
            }
        }


        public AdminUpdatePage()
        {
            InitializeComponent();
            moviepanel.Visible = false; actorpanel.Visible = false;
            prodcompanypanel.Visible = false;
            PopulateGenderOptions();
        }

        private void moviebutton_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            moviebutton.BackColor = Color.LightGreen;
            moviepanel.Visible = true; actorpanel.Visible = false;
            prodcompanypanel.Visible = false;
        }
        private void actorbutton_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            actorbutton.BackColor = Color.LightGreen;
            moviepanel.Visible = false; actorpanel.Visible = true; prodcompanypanel.Visible = false;
        }


        private void prodcompanybutton_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            prodcompanybutton.BackColor = Color.LightGreen;
            moviepanel.Visible = false; actorpanel.Visible = false; prodcompanypanel.Visible = true;
        }
        private void ResetButtonColors()
        {
            moviebutton.BackColor = SystemColors.Control; // Default button color
            actorbutton.BackColor = SystemColors.Control;
            prodcompanybutton.BackColor = SystemColors.Control;
        }


        /////////////////////// MOVIES ////////////////////////////////////
        private void MovieClearFields()
        {
            titletextbox.Clear();
            genrenamecombobox.SelectedIndex = -1;
            releaseyearcombobox.SelectedIndex = -1;
            ratingtextbox.Clear();
            runtimetextbox.Clear();
            plotsummarytextbox.Clear();
            directornamecombobox.SelectedIndex = -1;
            producernamecombobox.SelectedIndex = -1;
            prodcompanycombobox.SelectedIndex = -1;
        }

        private void FetchMovieData(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a movie title.");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT m.*, d.FullName AS DirectorName, p.FullName AS ProducerName, pc.CompanyName, pc.Country AS CompanyCountry " +
                                   "FROM Movies m " +
                                   "LEFT JOIN Directors d ON m.DirectorName = d.FullName " +
                                   "LEFT JOIN Producers p ON m.ProducerName = p.FullName " +
                                   "LEFT JOIN ProductionCompanies pc ON m.CompanyName = pc.CompanyName " +
                                   "WHERE m.Title = @Title";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Clear previous data
                                MovieClearFields();

                                // Populate fields with data from the database
                                titletextbox.Text = title;
                                releaseyearcombobox.Text = reader["ReleaseYear"].ToString();
                                genrenamecombobox.Text = reader["GenreName"].ToString();
                                ratingtextbox.Text = reader["Rating"].ToString();
                                runtimetextbox.Text = reader["Runtime"].ToString();
                                plotsummarytextbox.Text = reader["PlotSummary"].ToString();
                                directornamecombobox.Text = reader["DirectorName"].ToString();
                                producernamecombobox.Text = reader["ProducerName"].ToString();
                                prodcompanycombobox.Text = reader["CompanyName"].ToString();

                                // Handle nationality for Production Company
                                string companyCountry = reader["CompanyCountry"].ToString();
                                nationalitycombobox.Text = companyCountry;
                            }
                            else
                            {
                                MessageBox.Show("Movie not found.");
                                MovieClearFields();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while fetching movie data: " + ex.Message);
                }
            }
        }


        private void UpdateMovieData()
        {
            // Fetch input values
            string title = titletextbox.Text;

            // Validate inputs
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a movie title.");
                return;
            }

            // Ensure rating is valid
            if (!decimal.TryParse(ratingtextbox.Text, out decimal rating) || rating < 0 || rating > 10)
            {
                MessageBox.Show("Invalid rating. It must be between 0 and 10.");
                return;
            }

            // Ensure runtime is valid
            if (!int.TryParse(runtimetextbox.Text, out int runtime) || runtime <= 0)
            {
                MessageBox.Show("Invalid runtime. It must be greater than 0.");
                return;
            }

            string genreName = genrenamecombobox.Text;
            string plotSummary = plotsummarytextbox.Text;
            string directorName = directornamecombobox.Text;
            string producerName = producernamecombobox.Text;
            string companyName = prodcompanycombobox.Text;

            // Access the selected year as string or integer
            string releaseYear = releaseyearcombobox.SelectedItem?.ToString(); // Access the selected value properly
            if (string.IsNullOrEmpty(releaseYear)) // Ensure it is not empty or null
            {
                MessageBox.Show("Please select a valid release year.");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Movies " +
                                   "SET ReleaseYear = @ReleaseYear, GenreName = @GenreName, Rating = @Rating, Runtime = @Runtime, PlotSummary = @PlotSummary, " +
                                   "DirectorName = @DirectorName, ProducerName = @ProducerName, CompanyName = @CompanyName " +
                                   "WHERE Title = @Title";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@ReleaseYear", releaseYear);  // Correctly using the selected year value
                        cmd.Parameters.AddWithValue("@GenreName", genreName);
                        cmd.Parameters.AddWithValue("@Rating", rating);
                        cmd.Parameters.AddWithValue("@Runtime", runtime);
                        cmd.Parameters.AddWithValue("@PlotSummary", plotSummary);
                        cmd.Parameters.AddWithValue("@DirectorName", directorName);
                        cmd.Parameters.AddWithValue("@ProducerName", producerName);
                        cmd.Parameters.AddWithValue("@CompanyName", companyName);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Movie data updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error updating movie data. Please check the title and try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating the movie data: " + ex.Message);
                }
            }
        }


        // Populate genres, directors, producers, and companies
        private void PopulateGenres()
        {
            genrenamecombobox.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT GenreName FROM Genres";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        genrenamecombobox.Items.Add(reader["GenreName"].ToString());
                    }
                }
            }
        }

        private void PopulateDirectors()
        {
            directornamecombobox.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT FullName FROM Directors";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        directornamecombobox.Items.Add(reader["FullName"].ToString());
                    }
                }
            }
        }

        private void PopulateProducers()
        {
            producernamecombobox.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT FullName FROM Producers";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        producernamecombobox.Items.Add(reader["FullName"].ToString());
                    }
                }
            }
        }

        private void PopulateCompanies()
        {
            prodcompanycombobox.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT CompanyName FROM ProductionCompanies";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        prodcompanycombobox.Items.Add(reader["CompanyName"].ToString());
                    }
                }
            }
        }

        private void moviessearch_Click(object sender, EventArgs e)
        {
            string title = titletextbox.Text.Trim();
            FetchMovieData(title);
        }

        private void save_Click(object sender, EventArgs e)
        {
            UpdateMovieData();
            MovieClearFields();
        }



        private void hide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        ////////////////////////  ACTOR //////////////////////////////////////
        private void PopulateGenderOptions()
        {
            gendercombobox.Items.Clear();
            gendercombobox.Items.Add("Male");
            gendercombobox.Items.Add("Female");
            gendercombobox.Items.Add("Other");

            // Optionally set a default selection
            gendercombobox.SelectedIndex = -1;  // Set to default value if needed
        }


        private void FetchActorData(string actorName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Actors WHERE FullName = @FullName";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", actorName);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dobirth.Value = Convert.ToDateTime(reader["DateOfBirth"]);

                                // Check if Gender is fetched correctly
                                string gender = reader["Gender"].ToString();
                                gendercombobox.SelectedItem = gender;  // Set Gender in combobox

                                string nationality = reader["Nationality"].ToString();
                                if (!string.IsNullOrEmpty(nationality))
                                {
                                    // Set Nationality in combobox
                                    nationalitycombobox.SelectedItem = nationality;  // Ensure the nationality is set
                                }
                                else
                                {
                                    // If no nationality is provided, clear or reset the combobox
                                    nationalitycombobox.SelectedIndex = -1;
                                }

                                biographytextbox.Text = reader["Biography"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Actor not found.");
                                ActorClearFields();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching data: " + ex.Message);
                }
            }
        }


        private void ActorClearFields()
        {
            actornametextbox.Clear();
            dobirth.Value = DateTime.Now;
            gendercombobox.SelectedIndex = -1;
            nationalitycombobox.SelectedIndex = -1;  // Clear Nationality field
            biographytextbox.Clear();
        }

        private void saveactor_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Actors(FullName, DateOfBirth, Gender, Nationality, Biography) " +
                                   "VALUES(@FullName, @DateOfBirth, @Gender, @Nationality, @Biography) " +
                                   "ON DUPLICATE KEY UPDATE DateOfBirth = VALUES(DateOfBirth), " +
                                   "Gender = VALUES(Gender), Nationality = VALUES(Nationality), Biography = VALUES(Biography);";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", actornametextbox.Text);
                        cmd.Parameters.AddWithValue("@DateOfBirth", dobirth.Value);
                        cmd.Parameters.AddWithValue("@Gender", gendercombobox.SelectedItem?.ToString() ?? "");
                        cmd.Parameters.AddWithValue("@Nationality", nationalitycombobox.Text);
                        cmd.Parameters.AddWithValue("@Biography", biographytextbox.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Actor data saved successfully.");
                        ActorClearFields(); // Clear the fields after saving data
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving data: " + ex.Message);
                }
            }
        }

        private void actorsearch_Click(object sender, EventArgs e)
        {
            string actorName = actornametextbox.Text.Trim();

            if (!string.IsNullOrEmpty(actorName))
            {
                FetchActorData(actorName); // Now search happens only on button click
            }
        }

        ///////////////////// PRODUCTION COMPANY ////////////////////////// 
        //// Triggered when the form loads (or any appropriate event)

        private void PopulateCompanyComboBox()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to fetch company names
                    string query = "SELECT CompanyName FROM ProductionCompanies";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<string> companyNames = new List<string>(); // List to store company names
                        while (reader.Read())
                        {
                            companyNames.Add(reader["CompanyName"].ToString()); // Add company names to the list
                        }

                        // Set up AutoComplete for companynametextbox
                        companynametextbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        companynametextbox.AutoCompleteSource = AutoCompleteSource.CustomSource;

                        AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
                        autoCompleteCollection.AddRange(companyNames.ToArray()); // Add company names to AutoComplete collection

                        companynametextbox.AutoCompleteCustomSource = autoCompleteCollection; // Assign the AutoComplete source to the TextBox
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching company names: " + ex.Message);
            }
        }


        // Triggered when the user clicks the search button
        private void companysearch_Click_1(object sender, EventArgs e)
        {
            string companyName = companynametextbox.Text.Trim();  // Get value from companynametextbox

            if (string.IsNullOrEmpty(companyName))
            {
                MessageBox.Show("Please enter a company name.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to fetch company details by name
                    string query = @"
            SELECT Country, FoundedYear, Headquarters
            FROM ProductionCompanies 
            WHERE CompanyName = @CompanyName";  // Corrected to use FoundedYear
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CompanyName", companyName);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Populate the fields with fetched data
                            countrycombobox.Text = reader["Country"].ToString();
                            founderyearcombobox.Text = reader["FoundedYear"].ToString();  // Correctly set the founded year
                            headquarterstextbox.Text = reader["Headquarters"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No matching company found.");
                            ProdCompanyClearFields();  // Clear the fields if no company is found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while fetching company details: " + ex.Message);
            }
        }

        // Triggered when the save button is clicked
        private void prodcompanysave_Click(object sender, EventArgs e)
        {
            string companyName = companynametextbox.Text;  // Get the company name from the TextBox
            string country = countrycombobox.Text;
            string foundedYearText = founderyearcombobox.Text;
            string headquarters = headquarterstextbox.Text;

            // Ensure all required fields are filled out
            if (string.IsNullOrEmpty(companyName) || string.IsNullOrEmpty(country) || string.IsNullOrEmpty(foundedYearText))
            {
                MessageBox.Show("Please fill out all required fields.");
                return;
            }

            // Convert FoundedYear to integer
            if (!int.TryParse(foundedYearText, out int foundedYear) || foundedYear < 1900 || foundedYear > 2024)
            {
                MessageBox.Show("Please enter a valid year (between 1900 and 2024).");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Query to update the company details
                    string updateQuery = @"
            UPDATE ProductionCompanies 
            SET Country = @Country, FoundedYear = @FoundedYear, Headquarters = @Headquarters 
            WHERE CompanyName = @CompanyName";  // Updated to use FoundedYear
                    MySqlCommand cmd = new MySqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@Country", country);
                    cmd.Parameters.AddWithValue("@FoundedYear", foundedYear);  // Correctly pass founded year as integer
                    cmd.Parameters.AddWithValue("@Headquarters", headquarters);
                    cmd.Parameters.AddWithValue("@CompanyName", companyName);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Company details updated successfully.");
                        ProdCompanyClearFields();  // Clear the fields after updating
                    }
                    else
                    {
                        MessageBox.Show("No matching company found to update.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating company details: " + ex.Message);
            }
        }


        // Clear all input fields
        private void ProdCompanyClearFields()
        {
            // Clear all textboxes
            companynametextbox.Clear();  // Clear company name textbox
            headquarterstextbox.Clear();  // Clear headquarters textbox

            // Clear ComboBox selections
            countrycombobox.SelectedIndex = -1;  // Clears the selection for country
            founderyearcombobox.SelectedIndex = -1;  // Clears the selection for founded year

            // Disable save button until valid input
            prodcompanysave.Enabled = false;
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
    }
}