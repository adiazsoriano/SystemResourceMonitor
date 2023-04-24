using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SystemResourceMonitor.util;
using System.Windows.Threading;
using SystemPerformance;
using ScottPlot;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Microsoft.Win32;

namespace SystemResourceMonitor.pages {
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page {

        private int timeelapsed;
        private int interval;
        private int limit;
        private DispatcherTimer timer;
        private PerformanceTracker performanceTracker;

        private WpfPlot? currentPlot;
        private int graphIndex;

        private List<double> XAxis;
        private List<double> YAxis;
        

        public StartPage() {
            InitializeComponent();
            this.KeepAlive = true;
            this.Loaded += StartPage_Loaded;

            timeelapsed = 0;
            interval = 0;
            limit = 0;
            timer = new();

            timer.Tick += Timer_Tick;
            timer.Interval = new System.TimeSpan(0, 0, 1);

            performanceTracker = new();

            currentPlot = null;
            graphIndex = -1;

            XAxis = new();
            YAxis = new();
        }

        private void clearGraphInfo() {
            XAxis.Clear();
            YAxis.Clear();
            currentPlot?.Plot.Clear();
            currentPlot?.Refresh();
        }

        private void handleStartUtilTracker(int graphIndex, WpfPlot plot) {
            this.graphIndex = graphIndex;
            this.currentPlot = plot;

            timer.Start();
        }

        private void handleStopUtilTracker() {
            timer.Stop();
        }

        private void updateGraph() {
            if (currentPlot != null) {
                currentPlot.Plot.AddScatter(XAxis.ToArray(), YAxis.ToArray(), Color.Blue);
                currentPlot.Plot.SetAxisLimits(-30+XAxis.Count, XAxis.Max(),-1,YAxis.Max()+1);
                currentPlot.Refresh();
            }
        }

        private void StartPage_Loaded(object sender, RoutedEventArgs e) {

            if(timer.IsEnabled) {
                timer.Stop();
            }

            if (DBUtil.Connection == null) {
                btnAccount.IsEnabled = false;
            }
            if (UserConfig.UserData != null) {
                btnAccount.Content = "Account";
            } else {
                btnAccount.Content = "Login/Signup";
            }

            btnBeginCPUPerc.IsEnabled = true;
            btnStopCPUPerc.IsEnabled = false;
            btnRecordCPUPerc.IsEnabled = false;
            btnDownloadCPUPerc.IsEnabled = false;
            btnUploadCPUPerc.IsEnabled = false;

            pCPUPercent.Plot.Title("CPU Utilization %");
            pCPUPercent.Plot.YLabel("% Value");
            pCPUPercent.Plot.XLabel("Time in Seconds");
            pCPUPercent.Refresh();
        }

        private void Timer_Tick(object? sender, System.EventArgs e) {
            timeelapsed++;

            if(limit != 0 && timeelapsed >= limit) {
                timer.Stop();
                btnBeginCPUPerc.IsEnabled = true;
                btnRecordCPUPerc.IsEnabled = true;

                btnDownloadCPUPerc.IsEnabled = true;
                if(UserConfig.UserLoggedin) {
                    btnUploadCPUPerc.IsEnabled = true;
                }

                cbiIntCPUPercDefault.IsSelected = true;
                cbiDurCPUPercDefault.IsSelected = true;
                interval = 0;
                limit = 0;
            }

            float data;
            switch(graphIndex) {
                case 0:
                    data = performanceTracker.Current_CPU_Usage;
                    break;
                //future cases can reflect future configurations
                default:
                    data = 0;
                    break;
            }
            
            if((this.interval == 0) || (this.interval != 0 && this.timeelapsed % this.interval == 0)) {
                XAxis.Add(timeelapsed);
                YAxis.Add(data);
                updateGraph();
            }
        }

        private void btnAccount_Click(object sender, RoutedEventArgs e) {
            if (UserConfig.UserData != null) {
                if (NavigationService.CanGoForward) {
                    NavigationService.GoForward();
                    NavigationService.GoForward();
                }
            } else {
                if (NavigationService.CanGoForward) {
                    NavigationService.GoForward();
                } else {
                    NavigationService.Navigate(PageUriIndex.account);
                }
            }
        }

        private void btnBeginCPUPerc_Click(object sender, RoutedEventArgs e) {
            btnBeginCPUPerc.IsEnabled = false;
            btnStopCPUPerc.IsEnabled = true;

            btnDownloadCPUPerc.IsEnabled = false;
            btnUploadCPUPerc.IsEnabled = false;

            handleStartUtilTracker(0, pCPUPercent);
        }

        private void btnStopCPUPerc_Click(object sender, RoutedEventArgs e) {
            btnBeginCPUPerc.IsEnabled = true;
            btnStopCPUPerc.IsEnabled = false;

            handleStopUtilTracker();
        }


        private void btnRecordCPUPerc_Click(object sender, RoutedEventArgs e) {
            if(timer.IsEnabled) {
                timer.Stop();
               
            }
            timeelapsed = 0;
            btnBeginCPUPerc.IsEnabled = false;
            btnStopCPUPerc.IsEnabled = false;
            clearGraphInfo();

            if(cbiIntCPUPerc1s.IsSelected) {
                interval = 1;
            } else if(cbiIntCPUPerc5s.IsSelected) {
                interval = 5;
            } else if(cbiIntCPUPerc10s.IsSelected) {
                interval = 10;
            }

            if(cbiDurCPUPerc1m.IsSelected) {
                limit = 60;
            } else if(cbiDurCPUPerc5m.IsSelected) {
                limit = 300;
            } else if(cbiDurCPUPerc10m.IsSelected) {
                limit = 600;
            }

            handleStartUtilTracker(0, pCPUPercent);
            btnRecordCPUPerc.IsEnabled = false;
            
        }

        private void cbIntCPUPerc_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(btnRecordCPUPerc != null) {
                if (!cbiIntCPUPercDefault.IsSelected && !cbiDurCPUPercDefault.IsSelected) {
                    btnRecordCPUPerc.IsEnabled = true;
                } else {
                    btnRecordCPUPerc.IsEnabled = false;
                }
            }
            
        }

        private void cbDurCPUPerc_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (btnRecordCPUPerc != null) {
                if (!cbiIntCPUPercDefault.IsSelected && !cbiDurCPUPercDefault.IsSelected) {
                    btnRecordCPUPerc.IsEnabled = true;
                } else {
                    btnRecordCPUPerc.IsEnabled = false;
                }
            }
        }

        private void btnDownloadCPUPerc_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "CPUUtilData";
            s.DefaultExt = ".csv";
            s.Filter = "Comma Seperated Values (.csv)|*.csv";

            var successful  = s.ShowDialog();

            if(successful == true) {
                FileUtil.HandleUtilDownload(this.XAxis,this.YAxis,s.FileName, "CPU_%_Usage");
                Debug.WriteLine(s.FileName);
            }
        }
    }
}
