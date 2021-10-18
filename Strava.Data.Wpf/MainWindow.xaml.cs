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
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Strava.Data.Core.Helpers;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;

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

        Activity SelectedActivity;
        List<Location> Points;

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

            MapPoint mapCenterPoint = new MapPoint(-118.805, 34.027, SpatialReferences.Wgs84);
            MainMapView.SetViewpoint(new Viewpoint(mapCenterPoint, 100000));
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

            SelectedActivity = Service.GetById(Activities.First().Id).Result;
            Worker.ReportProgress(100);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Refresh();
            SetMapPoints();
        }

        private void Refresh()
        {
            gridActivities.ItemsSource = Activities ?? new Activity[] { };
        }

        private void SetMapPoints()
        {
            var first = Activities.First();

            Points = PolylineHelper.DecodePolylinePoints(SelectedActivity.Map.Polyline);
            MainMapView.GraphicsOverlays.FirstOrDefault().Graphics.Add(AddPoints());
           
        }

        public Graphic AddPoints()
        {
            List<MapPoint> mapPoints = Enumerable.Range(0, Points.Count).Select(x =>
            {
                return new MapPoint(Points[x].Latitude, Points[x].Longitude, SpatialReferences.Wgs84);
            }).ToList();

            // Create polyline geometry from the points.
            var map = new Esri.ArcGISRuntime.Geometry.Polyline(mapPoints);

            // Create a symbol for displaying the line.
            var polylineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Blue, 3.0);

            // Create a polyline graphic with geometry and symbol.
            var polylineGraphic = new Graphic(map, polylineSymbol);

            return polylineGraphic;
        }
    }
}
