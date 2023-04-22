using System.IO;
using System.Windows;
using SystemResourceMonitor.util;

namespace SystemResourceMonitor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            string? auth = FileUtil.GetFileContent(Path.Combine(Path.GetFullPath(@"..\..\..\"), @"config\config.txt"))?[0];
            DBUtil.Connection = DBUtil.Connect(auth);
        }
    }
}
