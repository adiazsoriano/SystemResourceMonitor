using System.Windows.Controls;
using SystemResourceMonitor.util;
using System.Windows;
using System;

namespace SystemResourceMonitor.pages {
    /// <summary>
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page {
        public AccountPage() {
            InitializeComponent();
            this.KeepAlive = true;
            this.Loaded += AccountPage_Loaded;
        }

        private void AccountPage_Loaded(object sender, RoutedEventArgs e) {
            tbUserInfo.Text = string.Empty;
            tbUserInfo.Text += "Welcome, " + UserConfig.UserData.Name + "\t\t\t\t";
            tbUserInfo.Text += "Username: " + UserConfig.UserData.UserName + "\t\t\t\t";
            tbUserInfo.Text += "Date: " + DateTime.Now.ToShortDateString();
        }

        private void btnStartPage_Click(object sender, RoutedEventArgs e) {
            if (NavigationService.CanGoBack) {
                NavigationService.GoBack();
                NavigationService.GoBack();
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e) {
            UserConfig.UserData = null;
            UserConfig.UserLoggedin = false;
            if (NavigationService.CanGoBack) {
                NavigationService.GoBack();
            }
        }

        private void RowButton_Click(object sender, RoutedEventArgs e) {

        }
    }
}
