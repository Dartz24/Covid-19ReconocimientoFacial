using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace Covid_19ReconocimientoFacial
{
    public partial class Mapa : Form
    {
        public Mapa()
        {
            InitializeComponent();
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CanDragMap = true;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(-1.242246, -78.629257);

            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 9;
            gMapControl1.AutoScroll = true;



            GMapOverlay markersOverlay = new GMapOverlay("markers");
            GMarkerGoogle marker =
                new GMarkerGoogle(
                 new PointLatLng(-1.242246, -78.629257), GMarkerGoogleType.green);
            GMarkerGoogle marker1 =
               new GMarkerGoogle(
                new PointLatLng(-1.552246, -78.629257), GMarkerGoogleType.green);
            markersOverlay.Markers.Add(marker);
                markersOverlay.Markers.Add(marker1);
            gMapControl1.Overlays.Add(markersOverlay);
        }
    }
}
