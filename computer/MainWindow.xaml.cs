using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace computer
{
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

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = username_txt.Text;
            string password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Kérlek, töltsd ki a felhasználónevet és jelszót!");
                return;
            }

            string checkQuery = "SELECT COUNT(*) FROM users WHERE Username = @Username";
            string insertQuery = "INSERT INTO users (Username, Password) VALUES (@Username, @Password)";

            using (MySqlConnection connection = Connect.GetConnection())
            {
                connection.Open();

                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Létezik már ilyen felhasználó.");
                        return;
                    }
                }

                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Password", password); // itt lehetne hash-elni
                    insertCmd.ExecuteNonQuery();
                    MessageBox.Show("Sikeres regisztráció!");
                }
            }

            username_txt.Text = string.Empty;
            passwordBox.Password = string.Empty;
        }

        private void kilepes_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Visibility = Visibility.Collapsed;
            data.Visibility = Visibility.Collapsed;
        }
    }
}
