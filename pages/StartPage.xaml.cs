using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SystemResourceMonitor.util;

namespace SystemResourceMonitor.pages {
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page {
        public StartPage() {
            InitializeComponent();
            this.KeepAlive = true;
            this.Loaded += StartPage_Loaded;
        }

        private void StartPage_Loaded(object sender, RoutedEventArgs e) {

            if (DBUtil.Connection == null) {
                btnAccount.IsEnabled = false;
            }
            if (UserConfig.UserData != null) {
                btnAccount.Content = "Account";
            } else {
                btnAccount.Content = "Login/Signup";
            }
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e) {
            if (UserConfig.UserData != null) {
                if (NavigationService.CanGoBack) {
                    NavigationService.GoBack();
                }
            } else {
                if (NavigationService.CanGoForward) {
                    NavigationService.GoForward();
                } else {
                    NavigationService.Navigate(PageUriIndex.account);
                }
            }
        }
    }
}
