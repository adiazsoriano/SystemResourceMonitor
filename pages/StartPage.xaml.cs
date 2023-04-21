using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using ScottPlot;

namespace SystemResourceMonitor.pages {
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page {
        public StartPage() {
            InitializeComponent();
            this.Loaded += StartPage_Loaded;
        }

        private void StartPage_Loaded(object sender, RoutedEventArgs e) {

            if(DBConnection.Connection == null) {
                btnAccount.IsEnabled = false;
            }
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new Uri("/pages/Account.xaml", UriKind.Relative));
        }
    }
}
