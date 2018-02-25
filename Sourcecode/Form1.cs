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
using Itinero;
using System.IO;
using Itinero.IO.Osm;
using System.Xml;
using Itinero.LocalGeo;

namespace GeocachingTourPlanner
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}



		#region MenuItems

		private void ImportOSMDataButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog StandardFileDialog = new OpenFileDialog
			{
				InitialDirectory = Program.DB.LastUsedFilepath,
				Filter = "pbf files (*.pbf)|*.pbf|All files (*.*)|*.*",
				FilterIndex = 2,
				RestoreDirectory = true,
				Title = "Import OSM Data"
			};

			if (StandardFileDialog.ShowDialog() == DialogResult.OK)
			{
				MessageBox.Show("This might take a while, depending on how big your pbf file is.\n How about getting yourself a coffee?");
				using (var stream = new FileInfo(StandardFileDialog.FileName).OpenRead())
				{
					Program.RouterDB.LoadOsmData(stream, new Itinero.Profiles.Vehicle[] { Itinero.Osm.Vehicles.Vehicle.Bicycle, Itinero.Osm.Vehicles.Vehicle.Car, Itinero.Osm.Vehicles.Vehicle.Pedestrian });
				}

				// write the routerdb to disk.
				if (Program.DB.RouterDB_Filepath == null)
				{
					Program.DB.RouterDB_Filepath = "OSM.routerdb";
				}
				using (var stream = new FileInfo(Program.DB.RouterDB_Filepath).Open(FileMode.Create))
				{
					Program.RouterDB.Serialize(stream);
				}

				Program.Backup(null);
			}
		}

		private void ImportGeocachesButton_Click(object sender, EventArgs e)
		{
			string GPXDatei;
			OpenFileDialog StandardFileDialog = new OpenFileDialog
			{
				InitialDirectory = Program.DB.LastUsedFilepath,
				Filter = "gpx files (*.gpx)|*.gpx|All files (*.*)|*.*",
				FilterIndex = 2,
				RestoreDirectory = true,
				Title="Import geocaches"
			};

			if (StandardFileDialog.ShowDialog() == DialogResult.OK)
			{
				DialogResult Importmodus = MessageBox.Show("Should the loaded Geocaches be kept?", "Import", MessageBoxButtons.YesNoCancel);
				if (Importmodus == DialogResult.Yes)
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
						geocache.DateHidden = DateTime.Parse(elem.Element(gpx + "time").Value.ToString());
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
		

		private void Export_Click(object sender, EventArgs e)
		{
			SaveFileDialog StandardFileDialog = new SaveFileDialog
			{
				InitialDirectory = Program.DB.LastUsedFilepath,
				Filter = "gpx files (*.gpx)|*.gpx|All files (*.*)|*.*",
				FilterIndex = 1,
				RestoreDirectory = true,
				Title = "Export route as track"
			};
			if (StandardFileDialog.ShowDialog() == DialogResult.OK)
			{

				KeyValueTriple<string, Route, List<Geocache>> RouteToSeialize = Program.Routes.First(x => x.Key == ((ToolStripMenuItem)sender).Text);

				XmlDocument GPX = new XmlDocument();

				XmlDeclaration xmldecl = GPX.CreateXmlDeclaration("1.0", "UTF-8", null);

				XmlNode root = GPX.CreateElement("gpx");
				GPX.AppendChild(root);

				XmlAttribute xmlns = GPX.CreateAttribute("xmlns");
				xmlns.Value = "http://www.topografix.com/GPX/1/1";
				root.Attributes.Append(xmlns);

				XmlAttribute version = GPX.CreateAttribute("version");
				version.Value = "1.1";
				root.Attributes.Append(version);

				XmlAttribute creator = GPX.CreateAttribute("creator");
				creator.Value = "GeocachingTourPlanner";
				root.Attributes.Append(creator);

				foreach (Geocache GC in RouteToSeialize.Value2)
				{
					XmlElement wpt = GPX.CreateElement("wpt");
					//Coordinates
					XmlAttribute latitude = GPX.CreateAttribute("lat");
					latitude.Value = GC.lat.ToString(CultureInfo.InvariantCulture);
					XmlAttribute longitude = GPX.CreateAttribute("lon");
					longitude.Value = GC.lon.ToString(CultureInfo.InvariantCulture);
					wpt.Attributes.Append(latitude);
					wpt.Attributes.Append(longitude);

					//Name
					XmlElement gcname = GPX.CreateElement("name");
					gcname.InnerText = GC.Name;
					wpt.AppendChild(gcname);
					//link
					XmlElement link = GPX.CreateElement("link");
					XmlAttribute linkattribute = GPX.CreateAttribute("href");
					linkattribute.Value = "https://www.coord.info/" + GC.GCCODE;
					link.Attributes.Append(linkattribute);
					wpt.AppendChild(link);

					root.AppendChild(wpt);
				}

				XmlNode track = GPX.CreateElement("trk");
				root.AppendChild(track);

				//Name of track
				XmlNode name = GPX.CreateElement("name");
				name.InnerText = RouteToSeialize.Key;
				track.AppendChild(name);

				XmlNode tracksegment = GPX.CreateElement("trkseg");
				foreach (Coordinate COO in RouteToSeialize.Value1.Shape)
				{
					XmlNode trackpoint = GPX.CreateElement("trkpt");

					//Coordinates
					XmlAttribute latitude = GPX.CreateAttribute("lat");
					latitude.Value = COO.Latitude.ToString(CultureInfo.InvariantCulture);
					XmlAttribute longitude = GPX.CreateAttribute("lon");
					longitude.Value = COO.Longitude.ToString(CultureInfo.InvariantCulture);

					trackpoint.Attributes.Append(latitude);
					trackpoint.Attributes.Append(longitude);
					tracksegment.AppendChild(trackpoint);
				}
				track.AppendChild(tracksegment);

				GPX.InsertBefore(xmldecl, root);
				GPX.Save(StandardFileDialog.FileName);
			}
		}

		public void Routes_ListChanged(object sender, ListChangedEventArgs e)
		{
			ExportToolStripMenuItem.DropDownItems.Clear();

			foreach (KeyValueTriple<string,Route,List<Geocache>> KVT in Program.Routes)
			{
				ToolStripMenuItem Menuitem = new ToolStripMenuItem();
				Menuitem.Text = KVT.Key;
				Menuitem.Click += new EventHandler(Export_Click);
				ExportToolStripMenuItem.DropDownItems.Insert(0, Menuitem);
			}
		}

		private void RateGeocachesButtonClick(object sender, EventArgs e)
		{
			//PRIORITY tranyplant the code from the window here
		}

		private void CreateRouteButtonClick(object sender, EventArgs e)
		{
			//PRIORITY tranyplant the code from the window here
		}

		private void setGeocachedatabaseButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Database.Databases.Geocaches))
			{
				Program.ReadGeocaches();
			}
		}

		private void setRoutingprofiledatabaseButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Database.Databases.Routingprofiles))
			{
				Program.ReadRoutingprofiles();
			}
		}

		private void setRatingprofiledatabaseButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Database.Databases.Ratingprofiles))
			{
				Program.ReadRatingprofiles();
			}
		}

		private void setRouterDBButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Database.Databases.RouterDB))
			{
				using (var stream = new FileInfo(Program.DB.RouterDB_Filepath).OpenRead())
				{
					Program.RouterDB = RouterDb.Deserialize(stream);
				}
				Program.Backup(null);
			}
		}

		#region Update of Rating/RoutingprofileList
		/// <summary>
		/// Keeps the Dropdownmenu updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Ratingprofiles_ListChanged(object sender, ListChangedEventArgs e)
		{
			SelectedRoutingprofileCombobox.Items.Clear();
			foreach (Ratingprofile bp in Program.Ratingprofiles)
			{

				ToolStripMenuItem Menuitem = new ToolStripMenuItem();
				Menuitem.Text = bp.ToString();
				Menuitem.Click += new EventHandler(Ratingprofile_Click);
				SelectedRoutingprofileCombobox.Items.Add(Menuitem);
			}
			RatingprofilesStateLabel.Text = Program.Ratingprofiles.Count.ToString();
		}

		/// <summary>
		/// keeps the dropdownmenu updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Routingprofiles_ListChanged(object sender, ListChangedEventArgs e)
		{
			SelectedRoutingprofileCombobox.Items.Clear();

			foreach (Routingprofile profile in Program.Routingprofiles)
			{
				ToolStripMenuItem Menuitem = new ToolStripMenuItem();
				Menuitem.Text = profile.ToString();
				Menuitem.Click += new EventHandler(Routingprofile_Click);
				SelectedRoutingprofileCombobox.Items.Add(Menuitem);
			}
			RoutingprofilesStateLabel.Text = Program.Routingprofiles.Count.ToString();
		}

		private void Ratingprofile_Click(object sender, EventArgs e)
		{
			//PRIORITY Fill window
		}

		private void Routingprofile_Click(object sender, EventArgs e)
		{
			//PRIORITY Fill window
		}
		#endregion
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

			//Remove all geocache (and only the geocache!) overlays
			if(Map.Overlays.Where(x => x.Id == "TopOverlay").Count() > 0)
			{
				Map.Overlays.Remove(Map.Overlays.First(x => x.Id == "TopOverlay"));
			}
			if (Map.Overlays.Where(x => x.Id == "MediumOverlay").Count() > 0)
			{
				Map.Overlays.Remove(Map.Overlays.First(x => x.Id == "MediumOverlay"));
			}
			if (Map.Overlays.Where(x => x.Id == "LowOverlay").Count() > 0)
			{
				Map.Overlays.Remove(Map.Overlays.First(x => x.Id == "LowOverlay"));
			}

			//recreate them
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

				GCMarker.ToolTipText = GC.GCCODE + "\n" + GC.Name + "\n" + GC.Type + "(" + GC.DateHidden.Date.ToString().Remove(10) + ")\nD-Wertung: " + GC.DRating + "\nT-Wertung: " + GC.TRating + "\nBewertung: " + GC.Rating;
				GCMarker.Tag = GC.GCCODE;
			}

			Map.Overlays.Add(LowOverlay);
			Map.Overlays.Add(MediumOverlay);
			Map.Overlays.Add(TopOverlay);

			//Not that clean, but makes sure that the checked states are equal to the visibility
			BestCheckbox_CheckedChanged(null, null);
			MediumCheckbox_CheckedChanged(null, null);
			WorstCheckbox_CheckedChanged(null, null);

			//Set Views
			if (Program.DB.LastMapZoom == 0)
			{
				Program.DB.LastMapZoom = 5;
			}
			Map.Zoom = Program.DB.LastMapZoom;

			if (Program.DB.LastMapPosition.IsEmpty)//Equals that the user hasn't seen the map before (fixes #2)
			{
				Program.DB.LastMapPosition = new PointLatLng(49.0, 8.5);
			}
			Map.Position = Program.DB.LastMapPosition;


		}

		#region Geocachecheckboxes

		private void BestCheckbox_CheckedChanged(object sender, EventArgs e)
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

		private void MediumCheckbox_CheckedChanged(object sender, EventArgs e)
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

		private void WorstCheckbox_CheckedChanged(object sender, EventArgs e)
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

		#endregion

		#region Routes
		public void newRouteControlElement(string OverlayTag)
		{
			CheckBox RouteControl = new CheckBox();
			RouteControl.Text = OverlayTag;
			RouteControl.AutoSize = true;
			RouteControl.Checked = true;
			RouteControl.CheckedChanged += new System.EventHandler(RouteControlElement_CheckedChanged);

			MapTab_SideMenu.RowCount++;
			MapTab_SideMenu.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			MapTab_SideMenu.Controls.Add(RouteControl,0,MapTab_SideMenu.RowCount);
			RouteControl.Show();
		}

		private void RouteControlElement_CheckedChanged(object sender, EventArgs e)
		{
			if (((CheckBox)sender).Checked)
			{
				Map.Overlays.First(x => x.Id == ((CheckBox)sender).Text).IsVisibile = true;
			}
			else
			{
				Map.Overlays.First(x => x.Id == ((CheckBox)sender).Text).IsVisibile = false;
			}
		}
		#endregion

		#region MapUIEvents
		private void Map_OnMapDrag()
		{
			Program.DB.LastMapPosition = Map.Position;
			Program.Backup(null);
		}

		private void Map_Enter(object sender, EventArgs e)
		{
			LoadMap();
		}

		private void Map_OnMapZoomChanged()
		{
			Program.DB.LastMapZoom = Map.Zoom;
			Program.Backup(null);
		}


		private void Map_Load(object sender, EventArgs e)//Called at the first time the tab gets clicked. This way the user doesn't see an empty map
		{
			LoadMap();
		}

		private void Map_OnMarkerClick(GMapMarker item, MouseEventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.coord.info/" + item.Tag);
		}
		#endregion

		#endregion

		
	}
}
