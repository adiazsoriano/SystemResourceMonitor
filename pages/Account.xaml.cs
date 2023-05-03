using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using SystemResourceMonitor.util;

namespace SystemResourceMonitor.pages {
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Page {
        public Account() {
            InitializeComponent();
            this.KeepAlive = true;
            this.Loaded += Account_Loaded;
        }

        /// <summary>
        /// Clears up signup fields, local to this page.
        /// </summary>
        private void ClearSignupFields() {
            txtSignupUser.Text = string.Empty;
            txtSignupName.Text = string.Empty;
            txtSignupPass.Password = string.Empty;
            txtSignupPassConf.Password = string.Empty;
            lblErrConf.Content = string.Empty;
        }      

        /// <summary>
        /// Event handler for once the page is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Account_Loaded(object sender, RoutedEventArgs e) {
            btnSignup.IsEnabled = false;
            lblErrLogin.Content = string.Empty;
            lblMessSignup.Content = string.Empty;
            lblErrConf.Content = string.Empty;
        }


        /// <summary>
        /// Event handler for back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e) {
            if (NavigationService.CanGoBack) {
                NavigationService.GoBack();
            }
        }


        /// <summary>
        /// Event handler for login button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            string query = "SELECT UID, Username, Name FROM Users WHERE Username=@user AND Password=@pass;";
            var (result, _) = DBUtil.ExecuteStatement(query,
                                                     false,
                                                     new("@user", txtLoginUser.Text.ToString()),
                                                     new("@pass", UserAccountUtil.HashString(txtLoginPass.Password.ToString()))
                                                     );

            //make sure the credentials are good
            if (result != null && result.HasRows) {
                UserConfig.UserData = new UserData();

                string? uid = "";
                string? username = "";
                string? name = "";
                if(result.Read()) {
                    uid = result["UID"].ToString();
                    username = result["Username"].ToString();
                    name = result["Name"].ToString();
                }
                result?.Close();
                UserConfig.UserData.LoadData(uid, username, name);
                UserConfig.UserLoggedin = true;

                NavigationService.Navigate(PageUriIndex.accountpage);
            } else {
                lblErrLogin.Content = "Please enter the right credentials.";
            }
            result?.Close();
        }


        /// <summary>
        /// Event handler for signup button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSignup_Click(object sender, RoutedEventArgs e) {
            if (!UserAccountUtil.UserExist(txtSignupUser.Text.ToString())) {
                string insertion = "INSERT INTO Users(Username,Name,Password) VALUES(@user,@name,@pass);";

                var (_, affectedrows) = DBUtil.ExecuteStatement(insertion,
                                                                true,
                                                                new("@user", txtSignupUser.Text.ToString()),
                                                                new("@name", txtSignupName.Text.ToString()),
                                                                new("@pass", UserAccountUtil.HashString(txtSignupPass.Password.ToString())));

                if (affectedrows != null && affectedrows > 0) {
                    lblMessSignup.Foreground = Brushes.Green;
                    lblMessSignup.Content = "Account added.";
                    ClearSignupFields();
                    txtLoginUser.Focus();

                } else {
                    lblMessSignup.Foreground = Brushes.Red;
                    lblMessSignup.Content = "Account not added.";
                    ClearSignupFields();
                    txtSignupUser.Focus();

                }
            } else {
                lblMessSignup.Foreground = Brushes.Red;
                lblMessSignup.Content = "Cannot create account.";
                ClearSignupFields();
                txtSignupUser.Focus();
            }
        }


        /// <summary>
        /// Event handler for signup password textfield change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSignupPass_PasswordChanged(object sender, RoutedEventArgs e) {
            if (txtSignupPassConf.Password.Equals(string.Empty) ||
                !txtSignupPass.Password.Equals(txtSignupPassConf.Password)) {
                lblErrConf.Content = "Passwords do not match.";
                btnSignup.IsEnabled = false;
            } else {
                lblErrConf.Content = string.Empty;
                btnSignup.IsEnabled = true;
            }
        }


        /// <summary>
        /// Event handler for signup password confirm textfield change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
