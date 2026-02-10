using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace InventoryManagementSystem
{
    /// <summary>
    /// Interaction logic for GROCORIES.xaml
    /// </summary>
    public partial class GROCORIES : Window
    {
        private string connectionString = DatabaseConfig.ConnectionString;

        public GROCORIES()
        {
            InitializeComponent();
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Groceries (ItemName, Description, Price, ExpiryDate, StockQuantity) " +
                                   "VALUES (@ItemName, @Description, @Price, @ExpiryDate, @StockQuantity)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ItemName", ItemNameTextBox.Text);
                    command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@Price", Convert.ToDecimal(PriceTextBox.Text));
                    command.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(ExpiryDatePicker.SelectedDate));
                    command.Parameters.AddWithValue("@StockQuantity", Convert.ToInt32(StockQuantityTextBox.Text));

                    connection.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show("Record inserted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowAllRecords();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Groceries SET ItemName = @ItemName, Description = @Description, Price = @Price, " +
                                   "ExpiryDate = @ExpiryDate, StockQuantity = @StockQuantity, UpdatedAt = GETDATE() WHERE ItemID = @ItemID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ItemID", Convert.ToInt32(ItemIDTextBox.Text));
                    command.Parameters.AddWithValue("@ItemName", ItemNameTextBox.Text);
                    command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@Price", Convert.ToDecimal(PriceTextBox.Text));
                    command.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(ExpiryDatePicker.SelectedDate));
                    command.Parameters.AddWithValue("@StockQuantity", Convert.ToInt32(StockQuantityTextBox.Text));

                    connection.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show("Record updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowAllRecords();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Groceries WHERE ItemID = @ItemID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ItemID", Convert.ToInt32(ItemIDTextBox.Text));

                    connection.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show("Record deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowAllRecords();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAllRecords();
        }

        private void ShowAllRecords()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Groceries";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    GroceriesDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Category a = new Category();
            a.Show();
            this.Close();
        }
    }
}
