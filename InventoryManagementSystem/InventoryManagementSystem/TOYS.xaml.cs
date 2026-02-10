using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace InventoryManagementSystem
{
    /// <summary>
    /// Interaction logic for TOYS.xaml
    /// </summary>
    public partial class TOYS : Window
    {
        private string connectionString = DatabaseConfig.ConnectionString;

        public TOYS()
        {
            InitializeComponent();
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Toys (ToyName, Description, Price, AgeGroup, StockQuantity) " +
                                   "VALUES (@ToyName, @Description, @Price, @AgeGroup, @StockQuantity)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ToyName", ToyNameTextBox.Text);
                    command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@Price", Convert.ToDecimal(PriceTextBox.Text));
                    command.Parameters.AddWithValue("@AgeGroup", AgeGroupTextBox.Text);
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
                    string query = "UPDATE Toys SET ToyName = @ToyName, Description = @Description, Price = @Price, " +
                                   "AgeGroup = @AgeGroup, StockQuantity = @StockQuantity, UpdatedAt = GETDATE() WHERE ToyID = @ToyID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ToyID", Convert.ToInt32(ToyIDTextBox.Text));
                    command.Parameters.AddWithValue("@ToyName", ToyNameTextBox.Text);
                    command.Parameters.AddWithValue("@Description", DescriptionTextBox.Text);
                    command.Parameters.AddWithValue("@Price", Convert.ToDecimal(PriceTextBox.Text));
                    command.Parameters.AddWithValue("@AgeGroup", AgeGroupTextBox.Text);
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
                    string query = "DELETE FROM Toys WHERE ToyID = @ToyID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ToyID", Convert.ToInt32(ToyIDTextBox.Text));

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
                    string query = "SELECT * FROM Toys";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    ToysDataGrid.ItemsSource = dataTable.DefaultView;
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
