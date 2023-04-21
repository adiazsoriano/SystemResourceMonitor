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
using MySqlConnector;

namespace SystemResourceMonitor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            string? auth = FileUtil.GetFileContent(Path.Combine(Path.GetFullPath(@"..\..\..\"), @"config\config.txt"))?[0];
            DBConnection.Connection = DBConnection.Connect(auth);
            Debug.WriteLine(DBConnection.Connection == null ? "Connection from DBConnection not reached" :
                                                              "Connection from DBConnection reached");
        }
    }
}
