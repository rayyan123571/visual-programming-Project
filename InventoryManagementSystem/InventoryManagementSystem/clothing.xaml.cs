using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace InventoryManagementSystem
{
    public partial class clothing : Window
    {
        // Database connection string from centralized config
        private string connectionString = DatabaseConfig.ConnectionString;

        public clothing()
        {
            InitializeComponent();
        }

        // Button 0: Insert Item
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string itemName = ItemNameTextBox.Text;
            string description = DescriptionTextBox.Text;
            decimal price;
            string size = SizeTextBox.Text;
            int stockQuantity;

            // Validate inputs
            if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(size) || !decimal.TryParse(PriceTextBox.Text, out price) || !int.TryParse(StockQuantityTextBox.Text, out stockQuantity))
            {
                MessageBox.Show("Please fill all fields correctly.");
                return;
            }

            try
            {
                // Insert the new item into the database
                string query = "INSERT INTO Clothing (ItemName, Description, Price, Size, StockQuantity) " +
                               "VALUES (@ItemName, @Description, @Price, @Size, @StockQuantity)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Size", size);
                        command.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Item added successfully.");
                ClearForm();
                ShowAllItems();  // Refresh the DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Button 1: Update Item
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int itemID;
            string itemName = ItemNameTextBox.Text;
            string description = DescriptionTextBox.Text;
            decimal price;
            string size = SizeTextBox.Text;
            int stockQuantity;

            // Validate inputs
            if (!int.TryParse(ItemIDTextBox.Text, out itemID) || string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(size) || !decimal.TryParse(PriceTextBox.Text, out price) || !int.TryParse(StockQuantityTextBox.Text, out stockQuantity))
            {
                MessageBox.Show("Please fill all fields correctly.");
                return;
            }

            try
            {
                // Update the item in the database
                string query = "UPDATE Clothing SET ItemName = @ItemName, Description = @Description, Price = @Price, Size = @Size, StockQuantity = @StockQuantity, UpdatedAt = GETDATE() WHERE ItemID = @ItemID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemID", itemID);
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Size", size);
                        command.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Item updated successfully.");
                ClearForm();
                ShowAllItems();  // Refresh the DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Button 2: Delete Item
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int itemID;

            // Validate Item ID input
            if (!int.TryParse(ItemIDTextBox.Text, out itemID))
            {
                MessageBox.Show("Please enter a valid Item ID.");
                return;
            }

            try
            {
                // Delete the item from the database
                string query = "DELETE FROM Clothing WHERE ItemID = @ItemID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemID", itemID);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Item deleted successfully.");
                ClearForm();
                ShowAllItems();  // Refresh the DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Button 3: Show All Items (Show all clothing items in DataGrid)
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ShowAllItems();
        }

        // Show all items in the DataGrid
        private void ShowAllItems()
        {
            try
            {
                // Query to fetch all clothing items
                string query = "SELECT ItemID, ItemName, Description, Price, Size, StockQuantity, CreatedAt, UpdatedAt FROM Clothing";

                List<ClothingItem> items = new List<ClothingItem>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new ClothingItem
                                {
                                    ItemID = reader.GetInt32(0),
                                    ItemName = reader.GetString(1),
                                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2), // Handle null descriptions
                                    Price = reader.GetDecimal(3),
                                    Size = reader.GetString(4),
                                    StockQuantity = reader.GetInt32(5),
                                    CreatedAt = reader.GetDateTime(6), // Added
                                    UpdatedAt = reader.GetDateTime(7)  // Added
                                });
                            }
                        }
                    }
                }

                // Bind the list of items to the DataGrid
                ClothingDataGrid.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Clear all input fields
        private void ClearForm()
        {
            ItemIDTextBox.Clear();
            ItemNameTextBox.Clear();
            DescriptionTextBox.Clear();
            PriceTextBox.Clear();
            SizeTextBox.Clear();
            StockQuantityTextBox.Clear();
        }

        // ClothingItem class to represent clothing item data
        public class ClothingItem
        {
            public int ItemID { get; set; }
            public string ItemName { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Size { get; set; }
            public int StockQuantity { get; set; }
            public DateTime CreatedAt { get; set; } // Added
            public DateTime UpdatedAt { get; set; } // Added
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Category a = new Category();
            a.Show();
            this.Close();
        }
    }
}
