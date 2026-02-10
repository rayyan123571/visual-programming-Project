using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace InventoryManagementSystem
{
    /// <summary>
    /// Interaction logic for furniture.xaml
    /// </summary>
    public partial class furniture : Window
    {
        // Database connection string from centralized config
        private string connectionString = DatabaseConfig.ConnectionString;

        public furniture()
        {
            InitializeComponent();
        }

        // Insert furniture item into database
        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            string itemName = ItemNameTextBox.Text;
            string description = DescriptionTextBox.Text;
            string material = MaterialTextBox.Text;
            decimal price;
            int stockQuantity;

            // Validate inputs
            if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(material) ||
                !decimal.TryParse(PriceTextBox.Text, out price) ||
                !int.TryParse(StockQuantityTextBox.Text, out stockQuantity))
            {
                MessageBox.Show("Please fill all fields correctly.");
                return;
            }

            try
            {
                string query = "INSERT INTO Furniture (ItemName, Description, Price, Material, StockQuantity) VALUES (@ItemName, @Description, @Price, @Material, @StockQuantity)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Material", material);
                        command.Parameters.AddWithValue("@StockQuantity", stockQuantity);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Item added successfully.");
                ClearForm();
                ShowAllItems(); // Refresh DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Update furniture item in database
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int itemID;
            string itemName = ItemNameTextBox.Text;
            string description = DescriptionTextBox.Text;
            string material = MaterialTextBox.Text;
            decimal price;
            int stockQuantity;

            // Validate inputs
            if (!int.TryParse(ItemIDTextBox.Text, out itemID) || string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(material) ||
                !decimal.TryParse(PriceTextBox.Text, out price) ||
                !int.TryParse(StockQuantityTextBox.Text, out stockQuantity))
            {
                MessageBox.Show("Please fill all fields correctly.");
                return;
            }

            try
            {
                string query = "UPDATE Furniture SET ItemName = @ItemName, Description = @Description, Price = @Price, Material = @Material, StockQuantity = @StockQuantity, UpdatedAt = GETDATE() WHERE ItemID = @ItemID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemID", itemID);
                        command.Parameters.AddWithValue("@ItemName", itemName);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Material", material);
                        command.Parameters.AddWithValue("@StockQuantity", stockQuantity);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Item updated successfully.");
                ClearForm();
                ShowAllItems(); // Refresh DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Delete furniture item from database
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
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
                string query = "DELETE FROM Furniture WHERE ItemID = @ItemID";

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
                ShowAllItems(); // Refresh DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Display all furniture items in DataGrid
        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAllItems();
        }

        private void ShowAllItems()
        {
            try
            {
                string query = "SELECT ItemID, ItemName, Description, Price, Material, StockQuantity, CreatedAt, UpdatedAt FROM Furniture";

                List<FurnitureItem> items = new List<FurnitureItem>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new FurnitureItem
                                {
                                    ItemID = reader.GetInt32(0),
                                    ItemName = reader.GetString(1),
                                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    Price = reader.GetDecimal(3),
                                    Material = reader.GetString(4),
                                    StockQuantity = reader.GetInt32(5),
                                    CreatedAt = reader.GetDateTime(6),  // Retrieve CreatedAt
                                    UpdatedAt = reader.GetDateTime(7)   // Retrieve UpdatedAt
                                });
                            }
                        }
                    }
                }

                FurnitureDataGrid.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Clear input fields
        private void ClearForm()
        {
            ItemIDTextBox.Clear();
            ItemNameTextBox.Clear();
            DescriptionTextBox.Clear();
            PriceTextBox.Clear();
            MaterialTextBox.Clear();
            StockQuantityTextBox.Clear();
        }

        // Furniture item class for binding
        public class FurnitureItem
        {
            public int ItemID { get; set; }
            public string ItemName { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Material { get; set; }
            public int StockQuantity { get; set; }
            public DateTime CreatedAt { get; set; }  // Include CreatedAt
            public DateTime UpdatedAt { get; set; }  // Include UpdatedAt
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Category a = new Category();
            a.Show();
            this.Close();
        }
    }
}
