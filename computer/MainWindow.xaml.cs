using MySql.Data.MySqlClient;
using System.Data;
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

namespace computer
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
        private void computer(object sender, RoutedEventArgs e)
        {
            string query = "SELECT Id, Brand, Type, Display, Memory, CreatedTime FROM comp";
            DataTable table = Connect.GetData(query);
            data.ItemsSource = table.DefaultView;
        }

        private void operaciosrendszer(object sender, RoutedEventArgs e)
        {
            string query = "SELECT Id, Name FROM osystem";
            DataTable table = Connect.GetData(query);
            data.ItemsSource = table.DefaultView;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = username_txt.Text;
            string password = passwordBox.Password;

            if (Login_ellenorzes(username, password))
            {
                MessageBox.Show("Sikeres bejelentkezés!");
                MainMenu.Visibility = Visibility.Visible;
                data.Visibility = Visibility.Visible;
                username_txt.Text = string.Empty;
                passwordBox.Password = string.Empty;

            }
            else
            {
                MessageBox.Show("Hibás felhasználónév vagy jelszó.");
            }
        }

        private bool Login_ellenorzes(string username, string password)
        {

            string query = "SELECT COUNT(*) FROM users WHERE Username = @Username AND Password = @Password";


            using (MySqlConnection connection = Connect.GetConnection())
            {

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);


                    connection.Open();
                    int userCount = Convert.ToInt32(command.ExecuteScalar());
                    return userCount > 0;
                }


            }
        }
        private void kilepes_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            data.Visibility = Visibility.Collapsed;

        }
    }
}