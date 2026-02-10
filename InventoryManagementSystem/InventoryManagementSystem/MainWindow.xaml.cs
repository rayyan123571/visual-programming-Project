using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InventoryManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameTextBox.Text == "admin" && PasswordBox.Password == "aumc")
            {
                Category a = new Category();
                a.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show("try again ","LOGIN", MessageBoxButton.YesNoCancel);
            }
        }
    }
}