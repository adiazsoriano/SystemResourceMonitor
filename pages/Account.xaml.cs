﻿using System;
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
        private static readonly SHA256 hashtool = SHA256.Create();
        public Account() {
            InitializeComponent();
            this.KeepAlive = true;
            this.Loaded += Account_Loaded;
        }

        private void ClearSignupFields() {
            txtSignupUser.Text = string.Empty;
            txtSignupName.Text = string.Empty;
            txtSignupPass.Password = string.Empty;
            txtSignupPassConf.Password = string.Empty;
            lblErrConf.Content = string.Empty;
        }

        private static bool UserExist(string username) {
            string query = "SELECT Username FROM Users WHERE Username=@user;";
            var (result, _) = DBUtil.ExecuteStatement(query,
                                                 false,
                                                 new Tuple<string, object?>("@user", username));
            bool doesUserExist = result != null && result.HasRows;
            result?.Close();
            return doesUserExist;
        }

        private static string HashString(string s) {
            byte[] passHash = hashtool.ComputeHash(Encoding.UTF8.GetBytes(s));
            return String.Join("", BitConverter.ToString(passHash).Split('-'));
        }

        private void Account_Loaded(object sender, RoutedEventArgs e) {
            btnSignup.IsEnabled = false;
            lblErrLogin.Content = string.Empty;
            lblMessSignup.Content = string.Empty;
            lblErrConf.Content = string.Empty;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e) {
            if (NavigationService.CanGoBack) {
                NavigationService.GoBack();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            string query = "SELECT UID,Username,Name FROM Users WHERE Username=@user AND Password=@pass;";
            var (result, _) = DBUtil.ExecuteStatement(query,
                                                     false,
                                                     new("@user", txtLoginUser.Text.ToString()),
                                                     new("@pass", HashString(txtLoginPass.Password.ToString()))
                                                     );

            if (result != null && result.HasRows) {
                //lblErrLogin.Content = "This user exists.";
                NavigationService.Navigate(PageUriIndex.accountpage);
            } else {
                lblErrLogin.Content = "Please enter the right credentials.";
            }
            result?.Close();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e) {
            if (!UserExist(txtSignupUser.Text.ToString())) {
                string insertion = "INSERT INTO Users(Username,Name,Password) VALUES(@user,@name,@pass);";

                var (_, affectedrows) = DBUtil.ExecuteStatement(insertion,
                                                                true,
                                                                new("@user", txtSignupUser.Text.ToString()),
                                                                new("@name", txtSignupName.Text.ToString()),
                                                                new("@pass", HashString(txtSignupPass.Password.ToString())));

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
