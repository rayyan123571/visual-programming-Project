using System;
using System.Windows;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;

namespace InventoryManagementSystem
{
    public partial class registration : Window
    {
        public registration()
        {
            InitializeComponent();
        }

        // Registration Button Click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get data from the form fields
            string fullName = FullNameTextBox.Text;
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password; // Accessing PasswordBox correctly
            string email = EmailTextBox.Text;
            string contactNumber = ContactNumberTextBox.Text;
            string address = AddressTextBox.Text;

            // Validate input fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Username, Password, and Email are required.");
                return;
            }

            // Hash the password for security
            string hashedPassword = HashPassword(password);

            // Connection string from centralized config
            string connectionString = DatabaseConfig.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to insert the registration data
                    string query = "INSERT INTO Registration (Username, PasswordHash, Email, FullName, ContactNumber, Address) " +
                                   "VALUES (@Username, @PasswordHash, @Email, @FullName, @ContactNumber, @Address)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@FullName", string.IsNullOrEmpty(fullName) ? (object)DBNull.Value : fullName);
                        command.Parameters.AddWithValue("@ContactNumber", string.IsNullOrEmpty(contactNumber) ? (object)DBNull.Value : contactNumber);
                        command.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address);

                        // Execute the query
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Registration successful!");
                        }
                        else
                        {
                            MessageBox.Show("An error occurred during registration.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Cancel Button Click
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Close the registration window
            this.Close();
        }

        // Method to hash the password using SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            user_login user_Login = new user_login();
            user_Login.Show();
            this.Close();
        }
    }
}
