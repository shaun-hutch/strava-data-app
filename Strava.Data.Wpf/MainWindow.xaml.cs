using Strava.Data.Core.Services;
using Strava.Data.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Strava.Data.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ActivityService Service;
        BackgroundWorker Worker;
        Activity[] Activities;

        public MainWindow()
        {
            InitializeComponent();
            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;

            Worker.DoWork += Worker_DoWork;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            Worker.ProgressChanged += Worker_ProgressChanged;

            Worker.RunWorkerAsync();

            LoadingProgress.Visibility = Visibility.Visible;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            LoadingProgress.Visibility = Visibility.Hidden;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {

            Service = new ActivityService();
            Activities = Service.Get().Result;
            Activities = Activities.Where(x => x.Distance > 0).ToArray();
            Worker.ReportProgress(100);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            gridActivities.ItemsSource = Activities ?? new Activity[] { };
        }
    }
}
