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
            string? auth = FileUtil.GetFileContent(Path.Combine(Path.GetFullPath(@"..\..\..\"), @"config\config.txt"))?[0];
            DBConnection.Connection = DBConnection.Connect(auth);
            Debug.WriteLine(DBConnection.Connection == null ? "Connection from DBConnection not reached" :
                                                              "Connection from DBConnection reached");
            this.Loaded += StartPage_Loaded;
        }

        private void StartPage_Loaded(object sender, RoutedEventArgs e) {

            if(DBConnection.Connection == null) {
                btnAccount.IsEnabled = false;
            }

            plExample.Plot.AddSignal(DataGen.Sin(51));
            plExample.Plot.AddSignal(DataGen.Cos(51));

            plExample.Plot.Title("Example Graph");
            plExample.Plot.YLabel("Y Axis Data");
            plExample.Plot.XLabel("X Axis Data");

            plExample.Refresh();
        }
    }
}
