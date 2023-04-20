using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySqlConnector;

namespace SystemResourceMonitor
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Page
    {
        public Account()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            using SHA256 hash = SHA256.Create();
            MySqlCommand credentials = new("SELECT UID,Username,Name FROM Users WHERE Username=@user AND Password=@pass", DBConnection.Connection);

            byte[] passHash = hash.ComputeHash(Encoding.UTF8.GetBytes(txtLoginPass.Password.ToString()));
            string pass = Encoding.UTF8.GetString(passHash);
            string user = txtLoginUser.Text.ToString();

            credentials.Parameters.AddWithValue("@user", user);
            credentials.Parameters.AddWithValue("@pass", pass);
            credentials.Prepare();

            MySqlDataReader dbresponse = credentials.ExecuteReader();

            //placeholder
            if(dbresponse.HasRows) {
                lblErr.Content = "This user exists.";
            } else {
                lblErr.Content = "This user does not exist.";
            }

        }
    }
}
