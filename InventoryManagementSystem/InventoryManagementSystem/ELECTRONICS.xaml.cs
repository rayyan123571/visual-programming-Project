using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace InventoryManagementSystem
{
    /// <summary>
    /// Interaction logic for ELECTRONICS.xaml
    /// </summary>
    public partial class ELECTRONICS : Window
    {
        public ELECTRONICS()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Step 1: Get the values from the input fields
            string itemName = ItemNameTextBox.Text;
            string description = DescriptionTextBox.Text;
            decimal price;
            int quantity;

            // Step 2: Validate input (you can improve this validation)
            if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(description) ||
                !decimal.TryParse(PriceTextBox.Text, out price) || !int.TryParse(QuantityTextBox.Text, out quantity))
            {
                MessageBox.Show("Please fill in all fields with valid data.");
                return;
            }

            // Step 3: Define your SQL connection string from centralized config
            string connectionString = DatabaseConfig.ConnectionString;

            // Step 4: Define the SQL Insert Query
            string query = "INSERT INTO Electronics (ItemName, Description, Price, StockQuantity) VALUES (@ItemName, @Description, @Price, @StockQuantity)";


            try
            {
                // Step 5: Create and open a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Step 6: Create the command and set parameters
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@StockQuantity", quantity); // Fixed the parameter name

                        // Step 7: Execute the query
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Item successfully added to the database!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to add the item.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the insert operation
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Step 1: Get the values from the input fields
            string itemName = ItemNameTextBox.Text;
            string description = DescriptionTextBox.Text;
            decimal price;
            int quantity;

            // Step 2: Validate input
            if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(description) ||
                !decimal.TryParse(PriceTextBox.Text, out price) || !int.TryParse(QuantityTextBox.Text, out quantity))
            {
                MessageBox.Show("Please fill in all fields with valid data.");
                return;
            }

            // Step 3: Define your SQL connection string from centralized config
            string connectionString = DatabaseConfig.ConnectionString;

            // Step 4: Define the SQL Update Query
            string query = "UPDATE Electronics SET ItemName = @ItemName, Description = @Description, Price = @Price, StockQuantity = @StockQuantity WHERE ItemID = @ItemID";

            try
            {
                // Step 5: Create and open a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Step 6: Create the command and set parameters
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@StockQuantity", quantity);

                        // Assuming ItemID is entered by the user (you can fetch it through a TextBox)
                        int itemId = int.Parse(ItemIDTextBox.Text); // You need to add ItemIDTextBox in XAML

                        command.Parameters.AddWithValue("@ItemID", itemId);

                        // Step 7: Execute the query
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Item successfully updated!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to update the item. Please check the ItemID.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the update operation
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Step 1: Get the ItemID from the input field (assuming you have an ItemIDTextBox in the form)
            int itemId;
            if (!int.TryParse(ItemIDTextBox.Text, out itemId))
            {
                MessageBox.Show("Please enter a valid Item ID to delete.");
                return;
            }

            // Step 2: Define your SQL connection string from centralized config
            string connectionString = DatabaseConfig.ConnectionString;

            // Step 3: Define the SQL Delete Query
            string query = "DELETE FROM Electronics WHERE ItemID = @ItemID";

            try
            {
                // Step 4: Create and open a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Step 5: Create the command and set parameters
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemID", itemId);

                        // Step 6: Execute the query
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Item successfully deleted from the database.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the item. Please check the ItemID.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the delete operation
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Step 1: Define your SQL connection string from centralized config
            string connectionString = DatabaseConfig.ConnectionString;

            // Step 2: Define the SQL Select Query
            string query = "SELECT * FROM Electronics";

            try
            {
                // Step 3: Create and open a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Step 4: Execute the query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        // Step 5: Create a list to hold the data
                        List<ElectronicsItem> electronicsItems = new List<ElectronicsItem>();

                        while (reader.Read())
                        {
                            electronicsItems.Add(new ElectronicsItem
                            {
                                ItemID = reader.GetInt32(0),
                                ItemName = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                StockQuantity = reader.GetInt32(4)
                            });
                        }

                        // Step 6: Bind the data to the DataGrid
                        ElectronicsDataGrid.ItemsSource = electronicsItems;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the select operation
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Class to represent an Electronics Item
        public class ElectronicsItem
        {
            public int ItemID { get; set; }
            public string ItemName { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Category a = new Category();
            a.Show();
            this.Close();
        }
    }
}
