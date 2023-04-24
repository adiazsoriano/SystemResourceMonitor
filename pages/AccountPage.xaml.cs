using System.Windows.Controls;
using SystemResourceMonitor.util;

namespace SystemResourceMonitor.pages {
    /// <summary>
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page {
        public AccountPage() {
            InitializeComponent();
            this.KeepAlive = true;
        }

        private void btnBack_Click(object sender, System.Windows.RoutedEventArgs e) {
            if(NavigationService.CanGoBack) {
                NavigationService.GoBack();
                NavigationService.GoBack();
            }
        }

        private void btnAccount_Click(object sender, System.Windows.RoutedEventArgs e) {
            UserConfig.UserData = null;
            UserConfig.UserLoggedin = false;
            if(NavigationService.CanGoBack) {
                NavigationService.GoBack();
            }
        }
    }
}
