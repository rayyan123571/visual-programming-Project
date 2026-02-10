using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Windows;
namespace InventoryManagementSystem
{
    /// <summary>
    /// Interaction logic for items_show.xaml
    /// </summary>
    public partial class items_show : Window
    {
        // Connection string from centralized config
        private readonly string connectionString = DatabaseConfig.ConnectionString;

        // List to hold items from the database
        private List<Item> Items;

        public items_show()
        {
            InitializeComponent();
            LoadItemsFromDatabase();
        }

        // Method to load items from the database
        private void LoadItemsFromDatabase()
        {
            Items = new List<Item>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT ItemID, ItemName, Price, StockQuantity 
                        FROM Clothing
                        UNION ALL
                        SELECT ItemID, ItemName, Price, StockQuantity 
                        FROM Furniture
                        UNION ALL
                        SELECT ItemID, ItemName, Price, StockQuantity 
                        FROM Groceries
                        UNION ALL
                        SELECT ToyID AS ItemID, ToyName AS ItemName, Price, StockQuantity 
                        FROM Toys
                        UNION ALL
                        SELECT BookID AS ItemID, Title AS ItemName, Price, StockQuantity 
                        FROM Books
                        UNION ALL
                        SELECT ItemID, ItemName, Price, StockQuantity 
                        FROM Electronics;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Items.Add(new Item
                            {
                                ItemID = reader.GetInt32(0),
                                ItemName = reader.GetString(1),
                                Price = reader.GetDecimal(2),
                                StockQuantity = reader.GetInt32(3)
                            });
                        }
                    }
                }

                // Bind the data to the DataGrid
                ItemsDataGrid.ItemsSource = Items;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading items: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for the Add to Cart button
        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsDataGrid.SelectedItem is Item selectedItem)
            {
                if (selectedItem.StockQuantity > 0)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();

                            // Determine the table name based on the selected item
                            string tableName = GetTableName(selectedItem.ItemID);

                            if (tableName != null)
                            {
                                // Update the stock quantity in the database
                                string updateQuery = $@"
                                    UPDATE {tableName}
                                    SET StockQuantity = StockQuantity - 1
                                    WHERE ItemID = @ItemID";

                                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                                {
                                    command.Parameters.AddWithValue("@ItemID", selectedItem.ItemID);
                                    command.ExecuteNonQuery();
                                }

                                // Update the stock quantity in the UI
                                selectedItem.StockQuantity -= 1;
                                ItemsDataGrid.Items.Refresh();

                                MessageBox.Show($"Item '{selectedItem.ItemName}' has been added to the cart.", "Item Added", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Table for the selected item not found. Please check the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating stock quantity: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Item '{selectedItem.ItemName}' is out of stock.", "Out of Stock", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select an item to add to the cart.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Helper method to determine the table name
        private string GetTableName(int itemID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT 'Clothing' AS TableName FROM Clothing WHERE ItemID = @ItemID
                        UNION ALL
                        SELECT 'Furniture' AS TableName FROM Furniture WHERE ItemID = @ItemID
                        UNION ALL
                        SELECT 'Groceries' AS TableName FROM Groceries WHERE ItemID = @ItemID
                        UNION ALL
                        SELECT 'Toys' AS TableName FROM Toys WHERE ToyID = @ItemID
                        UNION ALL
                        SELECT 'Books' AS TableName FROM Books WHERE BookID = @ItemID
                        UNION ALL
                        SELECT 'Electronics' AS TableName FROM Electronics WHERE ItemID = @ItemID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemID", itemID);
                        object result = command.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }

    // Class to represent an item
    public class Item
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
