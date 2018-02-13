using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;

namespace GeocachingTourPlanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

		OpenFileDialog StandardFileDialog = new OpenFileDialog
		{
			InitialDirectory = Program.DB.LastUsedFilepath,
			Filter = "gpx files (*.gpx)|*.gpx|All files (*.*)|*.*",
			FilterIndex = 2,
			RestoreDirectory = true
		};

		#region MenuItems
		private void importierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string GPXDatei;
            
            if (StandardFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult Importmodus = MessageBox.Show("Should the loaded Geocaches be kept?", "Import", MessageBoxButtons.YesNoCancel);
                if ( Importmodus==DialogResult.Yes)
                {
                    //Do nothing
                }
				else if (Importmodus == DialogResult.No)
                {
                    Program.Geocaches.Clear();
                }
                else
                {
                    return;
                }

                try
                {
                    GPXDatei = StandardFileDialog.FileName;
                    Program.DB.LastUsedFilepath = StandardFileDialog.FileName;
                    XElement RootElement = XElement.Load(GPXDatei);

                    XNamespace gpx = "http://www.topografix.com/GPX/1/0"; //default namespace
                    XNamespace groundspeak = "http://www.groundspeak.com/cache/1/0/1";

                    foreach (XElement elem in RootElement.Elements(gpx + "wpt"))
                    {
                        Geocache geocache = new Geocache { };

                        geocache.lat = float.Parse(elem.FirstAttribute.ToString().Replace("\"", "").Replace("lat=", ""), CultureInfo.InvariantCulture);
                        geocache.lon = float.Parse(elem.LastAttribute.ToString().Replace("\"", "").Replace("lon=", ""), CultureInfo.InvariantCulture);
                        geocache.GCCODE = (string)elem.Element(gpx + "name").Value;
                        geocache.Name = (string)elem.Element(gpx + "urlname").Value;
                        XElement CacheDetails = elem.Element(groundspeak + "cache");
                        geocache.DRating = float.Parse(CacheDetails.Element(groundspeak + "difficulty").Value.ToString(), CultureInfo.InvariantCulture);
                        geocache.TRating = float.Parse(CacheDetails.Element(groundspeak + "terrain").Value.ToString(), CultureInfo.InvariantCulture);
                        geocache.NeedsMaintenance = CacheDetails.Element(groundspeak + "attributes").Elements().ToList().Exists(x => x.FirstAttribute.Value == 42.ToString());
                        geocache.HidingDate = DateTime.Parse(elem.Element(gpx + "time").Value.ToString());
                        switch (elem.Element(gpx + "type").Value)
                        {
                            case "Geocache|Unknown Cache":
                                geocache.Type = GeocacheType.Mystery;
                                break;
                            case "Geocache|Traditional Cache":
                                geocache.Type = GeocacheType.Traditional;
                                break;
                            case "Geocache|Multi-cache":
                                geocache.Type = GeocacheType.Multi;
                                break;
                            case "Geocache|Virtual Cache":
                                geocache.Type = GeocacheType.Virtual;
                                break;
                            case "Geocache|Wherigo Cache":
                                geocache.Type = GeocacheType.Wherigo;
                                break;
                            case "Geocache|Webcam Cache":
                                geocache.Type = GeocacheType.Webcam;
                                break;
                            case "Geocache|Letterbox Hybrid":
                                geocache.Type = GeocacheType.Letterbox;
                                break;
                            case "Geocache|Earthcache":
                                geocache.Type = GeocacheType.EarthCache;
                                break;
                            default:
                                geocache.Type = GeocacheType.Other;
                                break;
                        }
                        switch (CacheDetails.Element(groundspeak + "container").Value)
                        {
                            case "Large":
                                geocache.Size = GeocacheSize.Large;
                                break;
                            case "Regular":
                                geocache.Size = GeocacheSize.Regular;
                                break;
                            case "Small":
                                geocache.Size = GeocacheSize.Small;
                                break;
                            case "Micro":
                                geocache.Size = GeocacheSize.Micro;
                                break;
                            default:
                                geocache.Size = GeocacheSize.Other;
                                break;
                        }
                        if (!Program.Geocaches.Any(x => x.GCCODE == geocache.GCCODE))
                        {
                            Program.Geocaches.Add(geocache);
                        }
                        else
                        {
                            //Nothing in the moment, would be good if it would update the Geocaches
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
                Program.Backup(Program.Geocaches);
            }
        }
		
        private void NewRatingprofileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new NewRatingProfileWindow().Show();
        }

		private void geocachesBewertenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RunRating Window = new RunRating();
			foreach (Ratingprofile BP in Program.Ratingprofiles)
			{
				Window.RatingProfilesCombobox.Items.Add(BP.Name);
			}
			Window.ShowDialog();
		}

		private void setGeocachedatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Program.DB.GeocacheDB_Filepath))
			{
				Program.ReadGeocaches();
			}
		}

		private void setRoutingprofiledatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Program.DB.RoutingDB_Filepath))
			{
				Program.ReadRoutingprofiles();
			}
		}

		private void setRatingprofiledatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Program.DB.RatingDB_Filepath))
			{
				Program.ReadRatingprofiles();
			}
		}

		#endregion

		#region Map
		/// <summary>
		/// Updates Map
		/// </summary>
		private void LoadMap()
		{
			Map.MapProvider = OpenCycleLandscapeMapProvider.Instance;
			GMaps.Instance.Mode = AccessMode.ServerOnly;
			//Remove Cross in the middle of the Map
			Map.ShowCenter = false;

			if (Map.Overlays.Count != 0)
			{
				Map.Overlays.Clear();
			}
			GMapOverlay TopOverlay = new GMapOverlay("TopOverlay");
			GMapOverlay MediumOverlay = new GMapOverlay("MediumOverlay");
			GMapOverlay LowOverlay = new GMapOverlay("LowOverlay");
			foreach (Geocache GC in Program.Geocaches)
			{
				GMapMarker GCMarker = null;
				//Three Categories => Thirds of the Point range
				if (GC.Rating > (Program.DB.MinimalRating) + 0.66 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
				{
					GCMarker = new GMarkerGoogle(new PointLatLng(GC.lat, GC.lon), GMarkerGoogleType.green_small);
					TopOverlay.Markers.Add(GCMarker);
				}
				else if (GC.Rating > (Program.DB.MinimalRating) + 0.33 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
				{
					GCMarker = new GMarkerGoogle(new PointLatLng(GC.lat, GC.lon), GMarkerGoogleType.yellow_small);
					MediumOverlay.Markers.Add(GCMarker);
				}
				else
				{
					GCMarker = new GMarkerGoogle(new PointLatLng(GC.lat, GC.lon), GMarkerGoogleType.red_small);
					LowOverlay.Markers.Add(GCMarker);
				}
				GCMarker.ToolTipText = GC.GCCODE + "\n" + GC.Name + "\n" + GC.Type + "(" + GC.HidingDate.Date.ToString().Remove(10) + ")\nD-Wertung: " + GC.DRating + "\nT-Wertung: " + GC.TRating + "\nBewertung: " + GC.Rating;
			}

			Map.Overlays.Add(LowOverlay);
			Map.Overlays.Add(MediumOverlay);
			Map.Overlays.Add(TopOverlay);

			//Set Views
			if (Program.DB.LastMapZoom != 0)
			{
				Map.Zoom = Program.DB.LastMapZoom;
			}
			else
			{
				Map.Zoom = 10;
				Program.DB.LastMapZoom =10;
			}
			if (Program.DB.LastMapPosition != null)
			{
				Map.Position = Program.DB.LastMapPosition;
			}
			else
			{
				Map.Position = new PointLatLng(49.0, 8.5);
				Program.DB.LastMapPosition = new PointLatLng(49.0, 8.5);
			}
		}

		private void BesteBox_CheckedChanged(object sender, EventArgs e)
		{
			if (BestGeocachesCheckbox.Checked)
			{
				Map.Overlays.First(x => x.Id == "TopOverlay").IsVisibile = true;
			}
			else
			{
				Map.Overlays.First(x => x.Id == "TopOverlay").IsVisibile = false;
			}
		}

		private void MittlereBox_CheckedChanged(object sender, EventArgs e)
		{
			if (MediumGeocachesCheckbox.Checked)
			{
				Map.Overlays.First(x => x.Id == "MediumOverlay").IsVisibile = true;
			}
			else
			{
				Map.Overlays.First(x => x.Id == "MediumOverlay").IsVisibile = false;
			}
		}

		private void SchlechteBox_CheckedChanged(object sender, EventArgs e)
		{
			if (WorstGeocachesCheckbox.Checked)
			{
				Map.Overlays.First(x => x.Id == "LowOverlay").IsVisibile = true;
			}
			else
			{
				Map.Overlays.First(x => x.Id == "LowOverlay").IsVisibile = false;
			}
		}
		
		private void Map_OnMapDrag()
		{
			Program.DB.LastMapPosition = Map.Position;
		}

		private void Map_Enter(object sender, EventArgs e)
		{
			LoadMap();
		}

		private void Map_OnMapZoomChanged()
		{
			Program.DB.LastMapZoom = Map.Zoom;
		}
		#endregion

	}
}
