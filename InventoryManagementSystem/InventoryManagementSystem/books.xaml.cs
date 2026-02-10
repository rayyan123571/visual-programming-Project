using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace InventoryManagementSystem
{
    public partial class books : Window
    {
        // Connection string from centralized config
        private readonly string connectionString = DatabaseConfig.ConnectionString;

        public books()
        {
            InitializeComponent();
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Books (Title, Author, Price, StockQuantity, PublicationDate) " +
                                   "VALUES (@Title, @Author, @Price, @StockQuantity, @PublicationDate)";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Title", TitleTextBox.Text);
                    command.Parameters.AddWithValue("@Author", AuthorTextBox.Text);
                    command.Parameters.AddWithValue("@Price", Convert.ToDecimal(PriceTextBox.Text));
                    command.Parameters.AddWithValue("@StockQuantity", Convert.ToInt32(StockQuantityTextBox.Text));
                    command.Parameters.AddWithValue("@PublicationDate", Convert.ToDateTime(PublicationDatePicker.SelectedDate));

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
                    string query = "UPDATE Books SET Title = @Title, Author = @Author, Price = @Price, " +
                                   "StockQuantity = @StockQuantity, PublicationDate = @PublicationDate, UpdatedAt = GETDATE() " +
                                   "WHERE BookID = @BookID";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@BookID", Convert.ToInt32(BookIDTextBox.Text));
                    command.Parameters.AddWithValue("@Title", TitleTextBox.Text);
                    command.Parameters.AddWithValue("@Author", AuthorTextBox.Text);
                    command.Parameters.AddWithValue("@Price", Convert.ToDecimal(PriceTextBox.Text));
                    command.Parameters.AddWithValue("@StockQuantity", Convert.ToInt32(StockQuantityTextBox.Text));
                    command.Parameters.AddWithValue("@PublicationDate", Convert.ToDateTime(PublicationDatePicker.SelectedDate));

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
                    string query = "DELETE FROM Books WHERE BookID = @BookID";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@BookID", Convert.ToInt32(BookIDTextBox.Text));

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
                    string query = "SELECT * FROM Books";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    BooksDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Category a= new Category();
            a.Show();
            this.Close();
        }
    }
}
