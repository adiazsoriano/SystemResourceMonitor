using System.Windows.Controls;
using SystemResourceMonitor.util;
using System.Windows;
using System;
using System.Data;
using SystemResourceMonitor.util;
using System.Diagnostics;
using Microsoft.Win32;

namespace SystemResourceMonitor.pages {
    /// <summary>
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page {
        public AccountPage() {
            InitializeComponent();
            this.Loaded += AccountPage_Loaded;
        }

        private void AccountPage_Loaded(object sender, RoutedEventArgs e) {
            tbUserInfo.Text = string.Empty;
            tbUserInfo.Text += "Welcome, " + UserConfig.UserData.Name + "\t\t\t\t";
            tbUserInfo.Text += "Username: " + UserConfig.UserData.UserName + "\t\t\t\t";
            tbUserInfo.Text += "Date: " + DateTime.Now.ToShortDateString();

            dgUploads.DataContext = UserData.UserDataToUploadInfo(UserConfig.UserData);
            dgUploads.Items.Refresh();
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
            var row = ((Button)e.Source).DataContext as UploadInfo;

            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "UtilData";
            s.DefaultExt = ".csv";
            s.Filter = "Comma Seperated Values (.csv)|*.csv";

            var successful = s.ShowDialog();

            if (successful == true) {
               FileUtil.HandleDBDownload(row.FileId, s.FileName);
            }
        }
    }
}
