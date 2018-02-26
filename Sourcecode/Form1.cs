﻿using System;
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

			//Tabelleneinstellungen
			GeocacheTable.DataSource = Program.Geocaches;
			GeocacheTable.Columns["GCCODE"].DisplayIndex = 0;
			GeocacheTable.Columns["Name"].DisplayIndex = 1;
			GeocacheTable.Columns["lat"].DisplayIndex = 2;
			GeocacheTable.Columns["lon"].DisplayIndex = 3;
			GeocacheTable.Columns["Type"].DisplayIndex = 4;
			GeocacheTable.Columns["Size"].DisplayIndex = 5;
			GeocacheTable.Columns["DRating"].DisplayIndex = 6;
			GeocacheTable.Columns["TRating"].DisplayIndex = 7;
			GeocacheTable.Columns["Rating"].DisplayIndex = GeocacheTable.ColumnCount - 1;
			GeocacheTable.Columns["ForceInclude"].DisplayIndex = GeocacheTable.ColumnCount - 1;
			GeocacheTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

			//Map
			Map.DisableFocusOnMouseEnter = true;//So Windows put in foreground stay in foreground
			Map.DragButton = MouseButtons.Left;

			Map.MapProvider = OpenCycleLandscapeMapProvider.Instance;
			GMaps.Instance.Mode = AccessMode.ServerOnly;
			//Remove Cross in the middle of the Map
			Map.ShowCenter = false;

		}



		#region Overview

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
				Title = "Import geocaches"
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


		//PRIORITY get back export
		/*
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
		*/

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
		#endregion

		#region Update of Rating/Routingprofiles

		public void CreateRatingprofile(object sender, EventArgs e)
		{
			Ratingprofile Profil = new Ratingprofile();
			if (RatingProfileName.Text == null)
			{
				MessageBox.Show("Bitte Namen festlegen");
				return;
			}
			try
			{
				Profil.Name = RatingProfileName.Text;
				Profil.TypePriority = int.Parse(TypePriorityvalue.Text);
				Profil.TypeRatings = new List<KeyValuePair<GeocacheType, int>>();
				Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.EarthCache, int.Parse(EarthcacheValue.Text)));
				Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Letterbox, int.Parse(LetterboxValue.Text)));
				Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Multi, int.Parse(Multivalue.Text)));
				Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Mystery, int.Parse(MysteryValue.Text)));
				Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Other, int.Parse(OtherTypeValue.Text)));
				Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Traditional, int.Parse(Traditionalvalue.Text)));
				Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Virtual, int.Parse(VirtualValue.Text)));
				Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Webcam, int.Parse(WebcamValue.Text)));
				Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Wherigo, int.Parse(WherigoValue.Text)));

				Profil.SizePriority = int.Parse(GrößenPrioritätValue.Text);
				Profil.SizeRatings = new List<KeyValuePair<GeocacheSize, int>>();
				Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Large, int.Parse(LargeValue.Text)));
				Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Micro, int.Parse(MicroValue.Text)));
				Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Other, int.Parse(OtherGrößeValue.Text)));
				Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Regular, int.Parse(RegularValue.Text)));
				Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Small, int.Parse(SmallValue.Text)));

				Profil.DPriority = int.Parse(DPrioritätenValue.Text);
				Profil.DRatings = new List<KeyValuePair<float, int>>();
				Profil.DRatings.Add(new KeyValuePair<float, int>(1f, int.Parse(D1Value.Text)));
				Profil.DRatings.Add(new KeyValuePair<float, int>(1.5f, int.Parse(D15Value.Text)));
				Profil.DRatings.Add(new KeyValuePair<float, int>(2f, int.Parse(D2Value.Text)));
				Profil.DRatings.Add(new KeyValuePair<float, int>(2.5f, int.Parse(D25Value.Text)));
				Profil.DRatings.Add(new KeyValuePair<float, int>(3f, int.Parse(D3Value.Text)));
				Profil.DRatings.Add(new KeyValuePair<float, int>(3.5f, int.Parse(D35Value.Text)));
				Profil.DRatings.Add(new KeyValuePair<float, int>(4f, int.Parse(D4Value.Text)));
				Profil.DRatings.Add(new KeyValuePair<float, int>(4.5f, int.Parse(D45Value.Text)));
				Profil.DRatings.Add(new KeyValuePair<float, int>(5f, int.Parse(D5Value.Text)));

				Profil.TPriority = int.Parse(TPrioritätenValue.Text);
				Profil.TRatings = new List<KeyValuePair<float, int>>();
				Profil.TRatings.Add(new KeyValuePair<float, int>(1f, int.Parse(T1Value.Text)));
				Profil.TRatings.Add(new KeyValuePair<float, int>(1.5f, int.Parse(T15Value.Text)));
				Profil.TRatings.Add(new KeyValuePair<float, int>(2f, int.Parse(T2Value.Text)));
				Profil.TRatings.Add(new KeyValuePair<float, int>(2.5f, int.Parse(T25Value.Text)));
				Profil.TRatings.Add(new KeyValuePair<float, int>(3f, int.Parse(T3Value.Text)));
				Profil.TRatings.Add(new KeyValuePair<float, int>(3.5f, int.Parse(T35Value.Text)));
				Profil.TRatings.Add(new KeyValuePair<float, int>(4f, int.Parse(T4Value.Text)));
				Profil.TRatings.Add(new KeyValuePair<float, int>(4.5f, int.Parse(T45Value.Text)));
				Profil.TRatings.Add(new KeyValuePair<float, int>(5f, int.Parse(T5Value.Text)));

				if (!int.TryParse(NMFlagValue.Text.Replace("-", ""), out int Value))
				{
					MessageBox.Show("Please write only positive whole numbers into the field with the NMPenalty");
				}
				else
				{
					Profil.NMPenalty = Value;
				}

				if (AgeValue.SelectedItem.ToString() == "multiply with")
				{
					Profil.Yearmode = true;
				}
				else
				{
					Profil.Yearmode = false;
				}

				Profil.Yearfactor = int.Parse(AlterZahlValue.Text);

			}
			catch (NullReferenceException)
			{
				MessageBox.Show("Please fill all fields");
				return;
			}

			//Eintragen des neuen Profils
			foreach (Ratingprofile BP in Program.Ratingprofiles.Where(x => x.Name == Profil.Name).ToList())//Make sure only one profile with a name exists
			{
				Program.Ratingprofiles.Remove(BP);
			}
			Program.Ratingprofiles.Add(Profil);
			//The Dropdownmenu gets updated through an event handler
			Program.Backup(Program.Ratingprofiles);
		}

		public void CreateRoutingprofile(object sender, EventArgs e)
		{
			Routingprofile Profile = new Routingprofile();
			if (RatingProfileName.Text == null)
			{
				MessageBox.Show("Please set Name");
				return;
			}
			try
			{
				Profile.Name = RatingProfileName.Text;

				Profile.MaxDistance = int.Parse(MaxDistance.Text);
				Profile.PenaltyPerExtraKM = int.Parse(PenaltyPerExtraKm.Text);

				Profile.MaxTime = int.Parse(MaxTime.Text);
				Profile.PenaltyPerExtra10min = int.Parse(PenaltyPerExtra10min.Text);
				Profile.TimePerGeocache = int.Parse(TimePerGeocache.Text);

				Profile.ItineroProfile = new SerializableItineroProfile(VehicleCombobox.Text, MetricCombobox.Text);
				if (Profile.ItineroProfile.profile == null)
				{
					MessageBox.Show("Please select valid Values for the Vehicle and Mode", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
			catch (NullReferenceException)
			{
				MessageBox.Show("Please fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			catch (FormatException)
			{
				MessageBox.Show("Some fields are filled with incompatible values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//Eintragen des neuen Profils
			foreach (Routingprofile BP in Program.Routingprofiles.Where(x => x.Name == Profile.Name).ToList())
			{
				Program.Routingprofiles.Remove(BP);
			}
			Program.Routingprofiles.Add(Profile);
			//The Dropdownmenu is updated via an event handler
			Program.Backup(Program.Routingprofiles);
			
		}
		/// <summary>
		/// Keeps the Dropdownmenu updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Ratingprofiles_ListChanged(object sender, ListChangedEventArgs e)
		{
			EditRatingprofileCombobox.Items.Clear();
			foreach (Ratingprofile profile in Program.Ratingprofiles)
			{
				EditRatingprofileCombobox.Items.Add(profile.Name);
				RatingprofileCombobox.Items.Add(profile.Name);
			}
			RatingprofilesStateLabel.Text = Program.Ratingprofiles.Count.ToString() + " Ratingprofiles loaded";
		}

		/// <summary>
		/// keeps the Comboboxes updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Routingprofiles_ListChanged(object sender, ListChangedEventArgs e)
		{
			EditRoutingprofileCombobox.Items.Clear();

			foreach (Routingprofile profile in Program.Routingprofiles)
			{
				EditRoutingprofileCombobox.Items.Add(profile.Name);
				SelectedRoutingprofileCombobox.Items.Add(profile.Name);
			}
			RoutingprofilesStateLabel.Text = Program.Routingprofiles.Count.ToString() + " Routingprofiles loaded";
		}

		private void Ratingprofile_Click(object sender, EventArgs e)
		{
			Ratingprofile SelectedRatingprofile = Program.Ratingprofiles.First(x => x.Name == ((ComboBox)sender).Text);

			try
			{
				//Name des Profils
				RatingProfileName.Text = SelectedRatingprofile.Name;

				//Prioritäten
				TypePriorityvalue.SelectedItem = TypePriorityvalue.Items[TypePriorityvalue.Items.IndexOf(SelectedRatingprofile.TypePriority.ToString())];
				GrößenPrioritätValue.SelectedItem = GrößenPrioritätValue.Items[TypePriorityvalue.Items.IndexOf(SelectedRatingprofile.SizePriority.ToString())];
				DPrioritätenValue.SelectedItem = DPrioritätenValue.Items[TypePriorityvalue.Items.IndexOf(SelectedRatingprofile.DPriority.ToString())];
				TPrioritätenValue.SelectedItem = TPrioritätenValue.Items[TypePriorityvalue.Items.IndexOf(SelectedRatingprofile.TPriority.ToString())];

				//TypenValueungen
				Traditionalvalue.SelectedItem = Traditionalvalue.Items[Traditionalvalue.Items.IndexOf(SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Traditional).Value.ToString())];
				EarthcacheValue.SelectedItem = EarthcacheValue.Items[EarthcacheValue.Items.IndexOf(SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.EarthCache).Value.ToString())];
				Multivalue.SelectedItem = Multivalue.Items[Multivalue.Items.IndexOf(SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Multi).Value.ToString())];
				MysteryValue.SelectedItem = MysteryValue.Items[MysteryValue.Items.IndexOf(SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Mystery).Value.ToString())];
				LetterboxValue.SelectedItem = LetterboxValue.Items[LetterboxValue.Items.IndexOf(SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Letterbox).Value.ToString())];
				VirtualValue.SelectedItem = VirtualValue.Items[VirtualValue.Items.IndexOf(SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Virtual).Value.ToString())];
				OtherTypeValue.SelectedItem = OtherTypeValue.Items[OtherTypeValue.Items.IndexOf(SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Other).Value.ToString())];
				WebcamValue.SelectedItem = WebcamValue.Items[WebcamValue.Items.IndexOf(SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Webcam).Value.ToString())];
				WherigoValue.SelectedItem = WherigoValue.Items[WherigoValue.Items.IndexOf(SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Wherigo).Value.ToString())];

				//Größe
				LargeValue.SelectedItem = LargeValue.Items[LargeValue.Items.IndexOf(SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Large).Value.ToString())];
				RegularValue.SelectedItem = RegularValue.Items[RegularValue.Items.IndexOf(SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Regular).Value.ToString())];
				SmallValue.SelectedItem = SmallValue.Items[SmallValue.Items.IndexOf(SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Small).Value.ToString())];
				MicroValue.SelectedItem = MicroValue.Items[MicroValue.Items.IndexOf(SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Micro).Value.ToString())];
				OtherGrößeValue.SelectedItem = OtherGrößeValue.Items[OtherGrößeValue.Items.IndexOf(SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Other).Value.ToString())];

				//D
				D1Value.SelectedItem = D1Value.Items[D1Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 1).Value.ToString())];
				D15Value.SelectedItem = D15Value.Items[D15Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 1.5).Value.ToString())];
				D2Value.SelectedItem = D2Value.Items[D2Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 2).Value.ToString())];
				D25Value.SelectedItem = D25Value.Items[D25Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 2.5).Value.ToString())];
				D3Value.SelectedItem = D3Value.Items[D3Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 3).Value.ToString())];
				D35Value.SelectedItem = D35Value.Items[D35Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 3.5).Value.ToString())];
				D4Value.SelectedItem = D4Value.Items[D4Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 4).Value.ToString())];
				D45Value.SelectedItem = D45Value.Items[D45Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 4.5).Value.ToString())];
				D5Value.SelectedItem = D5Value.Items[D5Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 5).Value.ToString())];

				//T
				T1Value.SelectedItem = T1Value.Items[T1Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 1).Value.ToString())];
				T15Value.SelectedItem = T15Value.Items[T15Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 1.5).Value.ToString())];
				T2Value.SelectedItem = T2Value.Items[T2Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 2).Value.ToString())];
				T25Value.SelectedItem = T25Value.Items[T25Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 2.5).Value.ToString())];
				T3Value.SelectedItem = T3Value.Items[T3Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 3).Value.ToString())];
				T35Value.SelectedItem = T35Value.Items[T35Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 3.5).Value.ToString())];
				T4Value.SelectedItem = T4Value.Items[T4Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 4).Value.ToString())];
				T45Value.SelectedItem = T45Value.Items[T45Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 4.5).Value.ToString())];
				T5Value.SelectedItem = T5Value.Items[T5Value.Items.IndexOf(SelectedRatingprofile.DRatings.First(x => x.Key == 5).Value.ToString())];

				//Sonstige
				NMFlagValue.Text = SelectedRatingprofile.NMPenalty.ToString();
				if (SelectedRatingprofile.Yearmode == true)
				{
					AgeValue.SelectedItem = AgeValue.Items[0];
				}
				else
				{
					AgeValue.SelectedItem = AgeValue.Items[1];
				}
				AlterZahlValue.SelectedItem = AlterZahlValue.Items[AlterZahlValue.Items.IndexOf(SelectedRatingprofile.Yearfactor.ToString())];
			}
			catch (Exception)
			{
				DialogResult aw = MessageBox.Show("Es scheint einen Fehler in der Datei zu diesem Profil zu geben. Wollen sie es Löschen?", "Fehler", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
				if (aw == DialogResult.Yes)
				{
					Program.Ratingprofiles.Remove(SelectedRatingprofile);
				}
			}
		}

		private void Routingprofile_Click(object sender, EventArgs e)
		{
			Routingprofile SelectedRoutingprofile = Program.Routingprofiles.First(x => x.Name == ((ComboBox)sender).Text);

			try
			{
				RoutingProfileName.Text = SelectedRoutingprofile.Name;

				//Distance
				MaxDistance.Text = SelectedRoutingprofile.MaxDistance.ToString();
				PenaltyPerExtraKm.Text = SelectedRoutingprofile.PenaltyPerExtraKM.ToString();

				//Time
				MaxTime.Text = SelectedRoutingprofile.MaxTime.ToString();
				PenaltyPerExtra10min.Text = SelectedRoutingprofile.PenaltyPerExtra10min.ToString();
				TimePerGeocache.Text = SelectedRoutingprofile.TimePerGeocache.ToString();

				//Profile

				//VehicleValue.Text = SelectedRoutingprofile.ItineroProfile.profile.FullName.Remove(SelectedRoutingprofile.ItineroProfile.profile.FullName.IndexOf("."));//gets the parent of the profile (thus the vehicle)
				//ModeValue.SelectedText = SelectedRoutingprofile.ItineroProfile.profile.Name;//Gives the metric

				//Workaround Issue #161 @ Itinero
				if (SelectedRoutingprofile.ItineroProfile.profile.FullName.Contains("."))
				{
					VehicleCombobox.Text = SelectedRoutingprofile.ItineroProfile.profile.FullName.Remove(SelectedRoutingprofile.ItineroProfile.profile.FullName.IndexOf("."));//gets the parent of the profile (thus the vehicle)

				}
				else
				{
					VehicleCombobox.Text = SelectedRoutingprofile.ItineroProfile.profile.FullName;
				}
				switch (SelectedRoutingprofile.ItineroProfile.profile.Metric)
				{
					case Itinero.Profiles.ProfileMetric.DistanceInMeters:

						MetricCombobox.Text = "Shortest";
						break;

					case Itinero.Profiles.ProfileMetric.TimeInSeconds:
						MetricCombobox.Text = "Fastest";
						break;
				}
			}
			catch (NullReferenceException)
			{
				MessageBox.Show("Couldn't load the complete profile.", "Warning");
			}
		}

		#endregion
		
		#region Map
		/// <summary>
		/// Updates Map
		/// </summary>
		private void LoadMap()
		{
			//Remove all geocache (and only the geocache!) overlays
			if (Map.Overlays.Where(x => x.Id == "TopOverlay").Count() > 0)
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

		private void RateGeocachesButtonClick(object sender, EventArgs e)
		{
			if (RatingprofileCombobox.Text == "")
			{
				MessageBox.Show("Please select a Ratingprofile");
				return;
			}

			Ratingprofile bewertungsprofil = Program.Ratingprofiles.First(x => x.Name == RatingprofileCombobox.Text);
			foreach (Geocache GC in Program.Geocaches)
			{
				GC.Rate(bewertungsprofil);
			}
			Program.Geocaches.OrderByDescending(x => x.Rating);
			GeocacheTable.Sort(GeocacheTable.Columns["Rating"], ListSortDirection.Descending);
			Program.DB.MaximalRating = Program.Geocaches[0].Rating;//Da sortierte Liste
			Program.DB.MinimalRating = Program.Geocaches[Program.Geocaches.Count - 1].Rating;
			Program.Backup(Program.Geocaches);
		}


		private void CreateRouteButtonClick(object sender, EventArgs e)
		{
			Enabled = false;
			if (SelectedRoutingprofileCombobox.Text.Length == 0)
			{
				MessageBox.Show("No Routingprofile set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Enabled = true;
				return;
			}

			Routingprofile SelectedProfile = Program.Routingprofiles.First(x => x.Name == SelectedRoutingprofileCombobox.Text);

			List<Geocache> GeocachesOnRoute = new List<Geocache>();
			foreach (Geocache GC in Program.Geocaches.Where(x=>x.ForceInclude))
			{
				GeocachesOnRoute.Add(GC);
			}
			
			float StartLat = 0;
			float StartLon = 0;
			float FinalLat = 0;
			float FinalLon = 0;

			if (StartpointTextbox.Text.Length == 0)
			{
				MessageBox.Show("No Startpoint set. Please type one in or select one with right click on the map", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Enabled = true;
				return;
			}

			if (!float.TryParse(StartpointTextbox.Text.Substring(0, StartpointTextbox.Text.IndexOf(";")), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,out StartLat))
			{
				MessageBox.Show("Couldn't Parse Startcoordinates. Are thes separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Enabled = true;
				return;
			}
			if (!float.TryParse(StartpointTextbox.Text.Substring(StartpointTextbox.Text.IndexOf(";")+1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out StartLon))
			{
				MessageBox.Show("Couldn't Parse Startcoordinates. Are thes separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Enabled = true;
				return;
			}

			if (EndpointTextbox.Text.Length == 0)
			{
				if (MessageBox.Show("No Endpoint set. Do you want to set Startpoint as Endpoint as well?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					FinalLat = StartLat;
					FinalLon = StartLon;
				}
				else
				{
					Enabled = true;
					return;
				}
			}
			else
			{
				if (!float.TryParse(EndpointTextbox.Text.Substring(0, StartpointTextbox.Text.IndexOf(";")), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out FinalLat))
				{
				
					MessageBox.Show("Couldn't Parse Endcoordinates. Are thes separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Enabled = true;
					return;
					
				}
				//As FinalLon is already set if the program enters the if scope and comes here
				if (!float.TryParse(EndpointTextbox.Text.Substring(StartpointTextbox.Text.IndexOf(";") + 1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out FinalLon))
				{
					//Big procedure only once, as the result would be the same
					MessageBox.Show("Couldn't Parse Endcoordinates. Are thes separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			//Check if Start and Endpoint have been selected
			if (StartLat == 0 && StartLon == 0 && FinalLat == 0 && FinalLon == 0)
			{
				MessageBox.Show("Please select a Startingpoint and a Finalpoint", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//Create Copy of Geocaches
			List<Geocache> NotAddedGeocaches = new List<Geocache>();
			//The Caches not in range get kicked out at the beginning of every iteration
			List<Geocache> GeocachesInRange = new List<Geocache>(Program.Geocaches);
			if (GeocachesOnRoute.Count != 0)
			{
				foreach(Geocache GC in GeocachesOnRoute)
				{
					GeocachesInRange.Remove(GC);//The Target Geocaches should be removed, as they are no new target
				}
			}

			float RoutePoints = 0;
			float LastRoutePoints = 0;
			Route CurrentRoute = null;
			do
			{
				//Create Copy of Geocaches
				NotAddedGeocaches = new List<Geocache>(GeocachesInRange);
				GeocachesInRange = new List<Geocache>();
				//Remember Points of last route
				LastRoutePoints = RoutePoints;

				//Remove all Geocaches out of reach of StartingPoint
				foreach (Geocache GC in new List<Geocache>(NotAddedGeocaches))
				{
					if (CurrentRoute != null)
					{
						if (ApproxDistance(GC.lat, GC.lon, StartLat, StartLon) <= (SelectedProfile.MaxDistance - CurrentRoute.TotalDistance / 1000) / 2)//As you have to geet there and back again
						{
							GeocachesInRange.Add(GC);
							NotAddedGeocaches.Remove(GC);
						}
					}
					else
					{
						if (ApproxDistance(GC.lat, GC.lon, StartLat, StartLon) <= (SelectedProfile.MaxDistance) / 2)//As you have to geet there and back again
						{
							GeocachesInRange.Add(GC);
							NotAddedGeocaches.Remove(GC);
						}
					}
				}
				foreach (Geocache RoutePoint in GeocachesOnRoute)
				{
					foreach (Geocache GC in new List<Geocache>(NotAddedGeocaches))
					{
						if (CurrentRoute != null)
						{
							if (ApproxDistance(GC.lat, GC.lon, StartLat, StartLon) < (SelectedProfile.MaxDistance - CurrentRoute.TotalDistance / 1000) / 2)
							{
								GeocachesInRange.Add(GC);
								NotAddedGeocaches.Remove(GC);
							}
						}
						else
						{
							if (ApproxDistance(GC.lat, GC.lon, RoutePoint.lat, RoutePoint.lon) < (SelectedProfile.MaxDistance) / 2)
							{
								GeocachesInRange.Add(GC);
								NotAddedGeocaches.Remove(GC);
							}
						}
					}
				}

				if (GeocachesInRange.Count != 0)
				{
					GeocachesInRange.OrderByDescending(x => x.Rating);
					GeocachesOnRoute.Add(GeocachesInRange[0]);
					GeocachesInRange.RemoveAt(0);

					if (Program.RouterDB.IsEmpty)
					{
						if (Program.DB.RouterDB_Filepath != null)
						{
							using (var stream = new FileInfo(Program.DB.RouterDB_Filepath).OpenRead())
							{
								Program.RouterDB = RouterDb.Deserialize(stream);
							}
						}
						else
						{
							MessageBox.Show("Import or set RouterDB before creating route!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}

					Router router = new Router(Program.RouterDB);

					//Make the Points which the route should pass
					List<RouterPoint> PointsOnRoute = new List<RouterPoint>();
					try
					{
						PointsOnRoute.Add(router.Resolve(SelectedProfile.ItineroProfile.profile, StartLat, StartLon));
					}
					catch (Itinero.Exceptions.ResolveFailedException)
					{
						MessageBox.Show("Please select a Startingpoint close to a road");
						return;
					}

					foreach (Geocache GC in new List<Geocache>(GeocachesOnRoute))
					{
						try
						{
							PointsOnRoute.Add(router.Resolve(SelectedProfile.ItineroProfile.profile, GC.lat, GC.lon));
						}
						catch (Itinero.Exceptions.ResolveFailedException)
						{
							GeocachesOnRoute.Remove(GC);//As it is not reachable
						}
					}


					PointsOnRoute.Add(router.Resolve(SelectedProfile.ItineroProfile.profile, StartLat, StartLon));//As start is currently also the End

					//Calculate Route
					try
					{
						CurrentRoute = router.Calculate(SelectedProfile.ItineroProfile.profile, PointsOnRoute.ToArray());
					}
					catch (Itinero.Exceptions.RouteNotFoundException)
					{
						//Route creation error, Itinero intern problem
						GeocachesOnRoute.RemoveAt(GeocachesOnRoute.Count - 1);//As the last geocache hasn't been fitted into the Route. From List of Geocaches in Range should remain, as this one is causing trouble.
																			  //Effectively, this causes it to take the current route. As far as seen until now, not a too big problem.
					}

					//Calculate Points of Route
					RoutePoints = 0;
					foreach (Geocache GC in GeocachesOnRoute)
					{
						RoutePoints += GC.Rating;
					}
					if (CurrentRoute.TotalDistance / 1000 > SelectedProfile.MaxDistance)
					{
						RoutePoints -= (CurrentRoute.TotalDistance / 1000 - SelectedProfile.MaxDistance) * SelectedProfile.PenaltyPerExtraKM;
					}
					if (CurrentRoute.TotalTime / 60 + GeocachesOnRoute.Count * SelectedProfile.TimePerGeocache > SelectedProfile.MaxTime)
					{
						RoutePoints -= (CurrentRoute.TotalTime / 60 - SelectedProfile.MaxTime) * SelectedProfile.PenaltyPerExtra10min / 10;
					}
				}
			} while (GeocachesInRange.Count > 0 && LastRoutePoints <= RoutePoints);

			if (CurrentRoute != null)
			{
				//Name of the route which will be used for all further referencing
				string Routetag = SelectedProfile.Name + " Route " + (SelectedProfile.RoutesOfthisType + 1);

				Program.Routes.Add(new KeyValueTriple<string, Route, List<Geocache>>(Routetag, CurrentRoute, GeocachesOnRoute));
				List<PointLatLng> GMAPRoute = new List<PointLatLng>();

				foreach (Coordinate COO in CurrentRoute.Shape)
				{
					GMAPRoute.Add(new PointLatLng(COO.Latitude, COO.Longitude));
				}


				SelectedProfile.RoutesOfthisType++;

				GMapOverlay RouteOverlay = new GMapOverlay(Routetag);
				RouteOverlay.Routes.Add(new GMapRoute(GMAPRoute, Routetag));
				foreach (Geocache GC in GeocachesOnRoute)
				{
					GMapMarker GCMarker = null;
					//Three Categories => Thirds of the Point range
					if (GC.Rating > (Program.DB.MinimalRating) + 0.66 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
					{
						GCMarker = new GMarkerGoogle(new PointLatLng(GC.lat, GC.lon), GMarkerGoogleType.green_small);
						RouteOverlay.Markers.Add(GCMarker);
					}
					else if (GC.Rating > (Program.DB.MinimalRating) + 0.33 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
					{
						GCMarker = new GMarkerGoogle(new PointLatLng(GC.lat, GC.lon), GMarkerGoogleType.yellow_small);
						RouteOverlay.Markers.Add(GCMarker);
					}
					else
					{
						GCMarker = new GMarkerGoogle(new PointLatLng(GC.lat, GC.lon), GMarkerGoogleType.red_small);
						RouteOverlay.Markers.Add(GCMarker);
					}

					GCMarker.ToolTipText = GC.GCCODE + "\n" + GC.Name + "\n" + GC.Type + "(" + GC.DateHidden.Date.ToString().Remove(10) + ")\nD-Wertung: " + GC.DRating + "\nT-Wertung: " + GC.TRating + "\nBewertung: " + GC.Rating;
					GCMarker.Tag = GC.GCCODE;
				}

				Enabled = true;
				Map.Overlays.Add(RouteOverlay);
				newRouteControlElement(Routetag);
				Map_Load(null,null);
			}
		}


		#region Helperfunctions for Routing
		/// <summary>
		/// Returns approximate distance in Km
		/// </summary>
		/// <param name="lat1"></param>
		/// <param name="lon1"></param>
		/// <param name="lat2"></param>
		/// <param name="lon2"></param>
		/// <returns></returns>
		private double ApproxDistance(double lat1, double lon1, double lat2, double lon2)
		{
			//Approximation for short distances
			double distance = Math.Sqrt(Math.Abs(lat1 - lat2) * Math.Abs(lat1 - lat2) + Math.Abs(lon1 - lon2) * Math.Abs(lon1 - lon2)) * 40030 / 360;
			return distance;
		}



		#endregion

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

		private void Map_Click(object sender, EventArgs e)
		{
			if (((MouseEventArgs)e).Button == MouseButtons.Right)
			{
				
			}
		}
		#endregion

		#endregion

		//UNDONE Attach this to EVERY Dropdownlist
		private void Dropdown_SelectedIndexChanged(object sender, EventArgs e)
		{
			((ComboBox)sender).Text = ((ComboBox)sender).SelectedItem.ToString();//So I can just check the text and it doesn't matter whether the user typed it or selected it

			if(sender == EditRoutingprofileCombobox)
			{
				Routingprofile_Click(sender, e);
			}else if (sender == EditRatingprofileCombobox)
			{
				Ratingprofile_Click(sender, e);
			}
		}
	}
}
