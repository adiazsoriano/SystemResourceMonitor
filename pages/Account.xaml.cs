using System;
using System.Collections.Generic;
using System.Data;
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
        public static SHA256 hashtool = SHA256.Create();
        public Account()
        {
            InitializeComponent();
            this.Loaded += Account_Loaded;
        }

        private void Account_Loaded(object sender, RoutedEventArgs e) {
            btnSignup.IsEnabled = false;
            lblErrLogin.Content = string.Empty;
            lblErrSignup.Content = string.Empty;
            lblErrConf.Content = string.Empty;
        }

        private bool userExist(string username) {
            string query = "SELECT Username FROM Users WHERE Username=@user;";
            using MySqlCommand check = new(query, DBConnection.Connection);

            check.Parameters.AddWithValue("@user", username);
            check.Prepare();

            using MySqlDataReader reader = check.ExecuteReader();
            return reader.HasRows;
        }

        private string hashString(string s) {
            byte[] passHash = hashtool.ComputeHash(Encoding.UTF8.GetBytes(s));
            return String.Join("", BitConverter.ToString(passHash).Split('-'));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            string query = "SELECT UID,Username,Name FROM Users WHERE Username=@user AND Password=@pass;";
            using MySqlCommand credentials = new(query, DBConnection.Connection);

            string user = txtLoginUser.Text.ToString();
            string pass = hashString(txtLoginPass.Password.ToString());

            credentials.Parameters.AddWithValue("@user", user);
            credentials.Parameters.AddWithValue("@pass", pass);
            credentials.Prepare();

            using MySqlDataReader dbresponse = credentials.ExecuteReader();

            //placeholder
            if(dbresponse.HasRows) {
                lblErrLogin.Content = "This user exists.";
            } else {
                lblErrLogin.Content = "This user does not exist.";
            }
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e) {
            if(!userExist(txtSignupUser.Text)) {
                string insertion = "INSERT INTO Users(Username,Name,Password) VALUES(@user,@name,@pass);";
                using MySqlCommand credentials = new(insertion, DBConnection.Connection);

                string username = txtSignupUser.Text.ToString();
                string name = txtSignupName.Text.ToString();
                string password = hashString(txtSignupPass.Password.ToString());

                credentials.Parameters.AddWithValue("@user", username);
                credentials.Parameters.AddWithValue("@name", name);
                credentials.Parameters.AddWithValue("@pass", password);
                credentials.Prepare();

                int response = credentials.ExecuteNonQuery();

                if (response == 0) {
                    lblErrSignup.Content = "Account not added.";
                } else {
                    lblErrSignup.Content = "Account added.";
                }
            } else {
                lblErrSignup.Content = "Cannot create account.";
            }
        }

        private void txtSignupPass_PasswordChanged(object sender, RoutedEventArgs e) {
            if(txtSignupPassConf.Password.Equals(string.Empty) ||
                !txtSignupPass.Password.Equals(txtSignupPassConf.Password)) {
                lblErrConf.Content = "Passwords do not match.";
                btnSignup.IsEnabled = false;
            } else {
                lblErrConf.Content = string.Empty;
                btnSignup.IsEnabled = true;
            }
        }

        private void txtSignupPassConf_PasswordChanged(object sender, RoutedEventArgs e) {
            if (txtSignupPass.Password.Equals(string.Empty) ||
                !txtSignupPassConf.Password.Equals(txtSignupPass.Password)) {
                lblErrConf.Content = "Passwords do not match.";
                btnSignup.IsEnabled = false;
            } else {
                lblErrConf.Content = string.Empty;
                btnSignup.IsEnabled = true;
            }
        }
    }
}
