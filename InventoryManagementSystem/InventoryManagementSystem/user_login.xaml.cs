using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace InventoryManagementSystem
{
    public partial class user_login : Window
    {
        public user_login()
        {
            InitializeComponent();
        }

        // Login Button Click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the username and password entered by the user
            string username = text1.Text;
            string password = pass1.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username and Password are required.");
                return;
            }

            // Hash the entered password to match with the stored hashed password in the database
            string hashedPassword = HashPassword(password);

            // Connection string from centralized config
            string connectionString = DatabaseConfig.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to select the user with the given username and password hash
                    string query = "SELECT COUNT(*) FROM Registration WHERE Username = @Username AND PasswordHash = @PasswordHash";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                        // Execute the query and get the count of matching rows
                        int result = (int)command.ExecuteScalar();

                        if (result > 0)
                        {
                            MessageBox.Show("Login successful!");
                            items_show a= new items_show();
                            a.Show();
                            this.Close();
                            // Proceed to the next window or functionality after successful login
                        }
                        else
                        {
                            MessageBox.Show("Invalid Username or Password.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            registration a = new registration();
            a.Show();
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }
    }
}
