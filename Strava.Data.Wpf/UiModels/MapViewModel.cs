using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System.Runtime.CompilerServices;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Symbology;

namespace Strava.Data.Wpf.UiModels
{
    public class MapViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Graphic RunGraphic;
        private Map _map;
        public Map Map
        {
            get { return _map; }
            set
            {
                _map = value;
                OnPropertyChanged();
            }

        }
        public MapViewModel()
        {
            SetupMap();
            CreateGraphics();

            this.PropertyChanged += MapViewModel_PropertyChanged;
        }

        private void MapViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Points")
                AddPoints();

        }

        public List<Strava.Data.Shared.Models.Location> _points;
        public List<Strava.Data.Shared.Models.Location> Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged("Points");
            }
        }

        private void SetupMap()
        {

            // Create a new map with a 'topographic vector' basemap.
            Map = new Map(BasemapStyle.ArcGISTopographic);

        }

        private GraphicsOverlay Overlay;

        private GraphicsOverlayCollection _graphicsOverlays;
        public GraphicsOverlayCollection GraphicsOverlays
        {
            get { return _graphicsOverlays; }
            set
            {
                _graphicsOverlays = value;
                OnPropertyChanged();
            }
        }

        private void CreateGraphics()
        {
            // Create a new graphics overlay to contain a variety of graphics.
            Overlay = new GraphicsOverlay();

            // Add the overlay to a graphics overlay collection.
            GraphicsOverlayCollection overlays = new GraphicsOverlayCollection
            {
                Overlay
            };

            // Set the view model's "GraphicsOverlays" property (will be consumed by the map view).
            this.GraphicsOverlays = overlays;

            RunGraphic = new Graphic(null, new SimpleLineSymbol(
                style: SimpleLineSymbolStyle.Solid,
                color: System.Drawing.Color.Blue,
                width: 4
            ));
        }

        public void AddPoints()
        {
            RunGraphic.Geometry = null;
            List<MapPoint> mapPoints = Enumerable.Range(0, Points.Count).Select(x =>
            {
                return new MapPoint(Points[x].Latitude, Points[x].Longitude, SpatialReferences.Wgs84);
            }).ToList();

            // Create polyline geometry from the points.
            var map = new Polyline(mapPoints);

            RunGraphic.Geometry = map;
        }


    }
}
