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
using System.Threading;

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
			Program.Geocaches.ResetBindings();

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
				//Create a new File
				SaveFileDialog NewFileDialog = new SaveFileDialog
				{
					InitialDirectory = Program.DB.LastUsedFilepath,
					Filter = "Routerdb files (*.routerdb)|*.routerdb|All files (*.*)|*.*",
					FilterIndex = 1,
					RestoreDirectory = true,
					Title = "Create new Routerdb file"
				};

				bool retry = false;
				do
				{
					retry = false;
					if (NewFileDialog.ShowDialog() == DialogResult.OK)
					{
						if (MessageBox.Show("If you selected an existing file it will be overwritten.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
						{
							File.Create(StandardFileDialog.FileName);
							Program.Backup(Program.Geocaches);
							Program.Geocaches = new SortableBindingList<Geocache>();
							Program.DB.GeocacheDB_Filepath = StandardFileDialog.FileName;
						}
						else
						{
							retry = true;
						}
					}
				} while (retry);

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
				Task Serialize = Task.Factory.StartNew(() =>
				{
					//just let it run in background
					using (var stream = new FileInfo(Program.DB.RouterDB_Filepath).Open(FileMode.Create))
					{
						Program.RouterDB.Serialize(stream);
					}
				});

				RouterDBStateLabel.Text = "Successfully loaded RouterDB";

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
				DialogResult Importmodus = MessageBox.Show("Should the Geocaches be loaded into a new Database?", "Import", MessageBoxButtons.YesNoCancel);
				if (Importmodus == DialogResult.Yes)
				{
					//Create a new File
					SaveFileDialog NewFileDialog = new SaveFileDialog
					{
						InitialDirectory = Program.DB.LastUsedFilepath,
						Filter = "gcdb files (*.gcdb)|*.gcdb|All files (*.*)|*.*",
						FilterIndex = 1,
						RestoreDirectory = true,
						Title = "Create new, empty geocachedatabase"
					};

					bool retry = false;
					do
					{
						retry = false;
						if (NewFileDialog.ShowDialog() == DialogResult.OK)
						{
							if (MessageBox.Show("If you selected an existing file it will be overwritten.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
							{
								File.Create(StandardFileDialog.FileName);
								Program.Backup(Program.Geocaches);
								Program.Geocaches = new SortableBindingList<Geocache>();
								Program.DB.GeocacheDB_Filepath = StandardFileDialog.FileName;
							}
							else
							{
								retry = true;
							}
						}
					} while (retry);
				}
				else if (Importmodus == DialogResult.No)
				{
					//Do nothing
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

					GeocachesStateLabel.Text = Program.Geocaches.Count + " Geocaches loaded";
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
				Program.Backup(Program.Geocaches);
			}
		}

		private void setGeocachedatabaseButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Databases.Geocaches))
			{
				Program.ReadGeocaches();
			}
		}

		private void setRoutingprofiledatabaseButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Databases.Routingprofiles))
			{
				Program.ReadRoutingprofiles();
			}
		}

		private void setRatingprofiledatabaseButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Databases.Ratingprofiles))
			{
				Program.ReadRatingprofiles();
			}
		}

		private void setRouterDBButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.SetDatabaseFilepath(Databases.RouterDB))
			{
				using (var stream = new FileInfo(Program.DB.RouterDB_Filepath).OpenRead())
				{
					Program.RouterDB = RouterDb.Deserialize(stream);
				}
				Program.Backup(null);
			}
		}

		private void GetPQLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.geocaching.com/pocket/");
		}


		private void GetPbfLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://download.geofabrik.de/");
		}

		private void NewRatingprofileDatabaseButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog StandardFileDialog = new SaveFileDialog
			{
				InitialDirectory = Program.DB.LastUsedFilepath,
				Filter = "ratingprofiles files (*.ratingprf)|*.ratingprf|All files (*.*)|*.*",
				FilterIndex = 1,
				RestoreDirectory = true,
				Title = "Create new, empty ratingprofilesdatabase"
			};

			bool retry = false;
			do
			{
				retry = false;
				if (StandardFileDialog.ShowDialog() == DialogResult.OK)
				{
					if (MessageBox.Show("If you selected an existing file it will be overwritten.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
					{
						File.Create(StandardFileDialog.FileName);
						Program.Backup(Program.Ratingprofiles);
						Program.Ratingprofiles = new SortableBindingList<Ratingprofile>();
						Program.DB.RatingprofileDB_Filepath = StandardFileDialog.FileName;
					}
					else
					{
						retry = true;
					}
				}
			} while (retry);
			
		}

		private void NewRoutingprofilesDatabaseButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog StandardFileDialog = new SaveFileDialog
			{
				InitialDirectory = Program.DB.LastUsedFilepath,
				Filter = "routingprofile files (*.routingprf)|*.routingprf|All files (*.*)|*.*",
				FilterIndex = 1,
				RestoreDirectory = true,
				Title = "Create new, empty routingprofilesdatabase"
			};

			bool retry = false;
			do
			{
				retry = false;
				if (StandardFileDialog.ShowDialog() == DialogResult.OK)
				{
					if (MessageBox.Show("If you selected an existing file it will be overwritten.", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
					{
						File.Create(StandardFileDialog.FileName);
						Program.Backup(Program.Routingprofiles);
						Program.Routingprofiles = new SortableBindingList<Routingprofile>();
						Program.DB.RoutingprofileDB_Filepath = StandardFileDialog.FileName;
					}
					else
					{
						retry = true;
					}
				}
			} while (retry);

		}
		#endregion

		#region Update of Rating/Routingprofiles

		public void CreateRatingprofile(object sender, EventArgs e)
	{
		Ratingprofile Profile = new Ratingprofile();
		if (RatingProfileName.Text == null)
		{
			MessageBox.Show("Bitte Namen festlegen");
			return;
		}
		try
		{
			Profile.Name = RatingProfileName.Text;
			Profile.TypePriority = int.Parse(TypePriorityvalue.Text);
			Profile.TypeRatings = new List<KeyValuePair<GeocacheType, int>>();
			Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.EarthCache, int.Parse(EarthcacheValue.Text)));
			Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Letterbox, int.Parse(LetterboxValue.Text)));
			Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Multi, int.Parse(Multivalue.Text)));
			Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Mystery, int.Parse(MysteryValue.Text)));
			Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Other, int.Parse(OtherTypeValue.Text)));
			Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Traditional, int.Parse(Traditionalvalue.Text)));
			Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Virtual, int.Parse(VirtualValue.Text)));
			Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Webcam, int.Parse(WebcamValue.Text)));
			Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Wherigo, int.Parse(WherigoValue.Text)));

			Profile.SizePriority = int.Parse(GrößenPrioritätValue.Text);
			Profile.SizeRatings = new List<KeyValuePair<GeocacheSize, int>>();
			Profile.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Large, int.Parse(LargeValue.Text)));
			Profile.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Micro, int.Parse(MicroValue.Text)));
			Profile.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Other, int.Parse(OtherGrößeValue.Text)));
			Profile.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Regular, int.Parse(RegularValue.Text)));
			Profile.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Small, int.Parse(SmallValue.Text)));

			Profile.DPriority = int.Parse(DPrioritätenValue.Text);
			Profile.DRatings = new List<KeyValuePair<float, int>>();
			Profile.DRatings.Add(new KeyValuePair<float, int>(1f, int.Parse(D1Value.Text)));
			Profile.DRatings.Add(new KeyValuePair<float, int>(1.5f, int.Parse(D15Value.Text)));
			Profile.DRatings.Add(new KeyValuePair<float, int>(2f, int.Parse(D2Value.Text)));
			Profile.DRatings.Add(new KeyValuePair<float, int>(2.5f, int.Parse(D25Value.Text)));
			Profile.DRatings.Add(new KeyValuePair<float, int>(3f, int.Parse(D3Value.Text)));
			Profile.DRatings.Add(new KeyValuePair<float, int>(3.5f, int.Parse(D35Value.Text)));
			Profile.DRatings.Add(new KeyValuePair<float, int>(4f, int.Parse(D4Value.Text)));
			Profile.DRatings.Add(new KeyValuePair<float, int>(4.5f, int.Parse(D45Value.Text)));
			Profile.DRatings.Add(new KeyValuePair<float, int>(5f, int.Parse(D5Value.Text)));

			Profile.TPriority = int.Parse(TPrioritätenValue.Text);
			Profile.TRatings = new List<KeyValuePair<float, int>>();
			Profile.TRatings.Add(new KeyValuePair<float, int>(1f, int.Parse(T1Value.Text)));
			Profile.TRatings.Add(new KeyValuePair<float, int>(1.5f, int.Parse(T15Value.Text)));
			Profile.TRatings.Add(new KeyValuePair<float, int>(2f, int.Parse(T2Value.Text)));
			Profile.TRatings.Add(new KeyValuePair<float, int>(2.5f, int.Parse(T25Value.Text)));
			Profile.TRatings.Add(new KeyValuePair<float, int>(3f, int.Parse(T3Value.Text)));
			Profile.TRatings.Add(new KeyValuePair<float, int>(3.5f, int.Parse(T35Value.Text)));
			Profile.TRatings.Add(new KeyValuePair<float, int>(4f, int.Parse(T4Value.Text)));
			Profile.TRatings.Add(new KeyValuePair<float, int>(4.5f, int.Parse(T45Value.Text)));
			Profile.TRatings.Add(new KeyValuePair<float, int>(5f, int.Parse(T5Value.Text)));

			if (!int.TryParse(NMFlagValue.Text.Replace("-", ""), out int Value))
			{
				MessageBox.Show("Please write only positive whole numbers into the field with the NMPenalty");
			}
			else
			{
				Profile.NMPenalty = Value;
			}

			if (AgeValue.SelectedItem.ToString() == "multiply with")
			{
				Profile.Yearmode = true;
			}
			else
			{
				Profile.Yearmode = false;
			}

			Profile.Yearfactor = int.Parse(AlterZahlValue.Text);

		}
		catch (NullReferenceException)
		{
			MessageBox.Show("Please fill all fields");
			return;
		}

		//Eintragen des neuen Profils
		foreach (Ratingprofile BP in Program.Ratingprofiles.Where(x => x.Name == Profile.Name).ToList())//Make sure only one profile with a name exists
		{
			Program.Ratingprofiles.Remove(BP);
		}
		Program.Ratingprofiles.Add(Profile);
		//The Dropdownmenu gets updated through an event handler
		Program.Backup(Program.Ratingprofiles);
		EditRatingprofileCombobox.SelectedItem = Profile.Name; //Eventhandler takes care of same profile selected
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
				Profile.Name = RoutingProfileName.Text;

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
				Profile.RoutesOfthisType = BP.RoutesOfthisType;
				Program.Routingprofiles.Remove(BP);
			}
			Program.Routingprofiles.Add(Profile);
			//The Dropdownmenu is updated via an event handler
			Program.Backup(Program.Routingprofiles);
			EditRoutingprofileCombobox.SelectedItem = Profile.Name; //Eventhandler takes care of same selection in comboboxes

		}
		/// <summary>
		/// Keeps the Dropdownmenu updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Ratingprofiles_ListChanged(object sender, ListChangedEventArgs e)
		{
			EditRatingprofileCombobox.Items.Clear();
			SelectedRatingprofileCombobox.Items.Clear();

			foreach (Ratingprofile profile in Program.Ratingprofiles)
			{
				EditRatingprofileCombobox.Items.Add(profile.Name);
				SelectedRatingprofileCombobox.Items.Add(profile.Name);
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
			SelectedRoutingprofileCombobox.Items.Clear();

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
		public void LoadMap()
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
			if (SelectedRatingprofileCombobox.Text == "")
			{
				MessageBox.Show("Please select a Ratingprofile");
				return;
			}

			Ratingprofile bewertungsprofil = Program.Ratingprofiles.First(x => x.Name == SelectedRatingprofileCombobox.Text);
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
			Application.UseWaitCursor = true;
			
			#region get values
			if (SelectedRoutingprofileCombobox.Text.Length == 0)
			{
				MessageBox.Show("No Routingprofile set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.UseWaitCursor = false;
				return;
			}

			Routingprofile SelectedProfile = Program.Routingprofiles.First(x => x.Name == SelectedRoutingprofileCombobox.Text);

			List<Geocache> GeocachesToInclude = new List<Geocache>();
			foreach (Geocache GC in Program.Geocaches.Where(x=>x.ForceInclude))
			{
				GeocachesToInclude.Add(GC);
			}
			
			float StartLat = 0;
			float StartLon = 0;
			float EndLat = 0;
			float EndLon = 0;

			if (StartpointTextbox.Text.Length == 0)
			{
				MessageBox.Show("No Startpoint set. Please type one in or select one with right click on the map", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.UseWaitCursor = false;
				return;
			}

			if (!float.TryParse(StartpointTextbox.Text.Substring(0, StartpointTextbox.Text.IndexOf(";")-1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,out StartLat))
			{
				MessageBox.Show("Couldn't parse latitude of Startcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.UseWaitCursor = false;
				return;
			}
			if (!float.TryParse(StartpointTextbox.Text.Substring(StartpointTextbox.Text.IndexOf(";")+1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out StartLon))
			{
				MessageBox.Show("Couldn't parse longitude Startcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.UseWaitCursor = false;
				return;
			}

			if (EndpointTextbox.Text.Length == 0)
			{
				if (MessageBox.Show("No Endpoint set. Do you want to set Startpoint as Endpoint as well?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					EndLat = StartLat;
					EndLon = StartLon;
					EndpointTextbox.Text = EndLat.ToString(CultureInfo.InvariantCulture) + ";" + EndLon.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					Application.UseWaitCursor = false;
					return;
				}
			}
			else
			{
				if (!float.TryParse(EndpointTextbox.Text.Substring(0, EndpointTextbox.Text.IndexOf(";")-1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out EndLat))
				{
				
					MessageBox.Show("Couldn't parse latitude of Endcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Application.UseWaitCursor = false;
					return;
					
				}
				if (!float.TryParse(EndpointTextbox.Text.Substring(EndpointTextbox.Text.IndexOf(";")+1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out EndLon))
				{
					//Big procedure only once, as the result would be the same
					MessageBox.Show("Couldn't parse longitude of Endcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Application.UseWaitCursor = false;
					return;
				}
			}

			//Check if Start and Endpoint have been selected
			if (StartLat == 0 && StartLon == 0 && EndLat == 0 && EndLon == 0)
			{
				MessageBox.Show("Please select a Startingpoint and a Finalpoint", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.UseWaitCursor = false;
				return;
			}
			#endregion

			new Thread(new ThreadStart(() =>
			{
				new Tourplanning().GetRoute(SelectedProfile, Program.Geocaches.ToList(), new Coordinate(StartLat, StartLon), new Coordinate(EndLat, EndLon), GeocachesToInclude);
			}
			)).Start();

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
		delegate void newRouteControlElementDelegate(string OverlayTag);
		public void newRouteControlElement(string OverlayTag)
		{
			if (MapTab_SideMenu.InvokeRequired == false)
			{


				GroupBox groupBox = new GroupBox();
				groupBox.Text = OverlayTag;
				groupBox.Width = 200;
				groupBox.Height = 80;

				TableLayoutPanel Table = new TableLayoutPanel();
				Table.RowCount = 2;
				Table.ColumnCount = 2;
				Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 0.5f));
				Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 0.5f));
				Table.RowStyles.Add(new RowStyle(SizeType.Percent, 0.5f));
				Table.RowStyles.Add(new RowStyle(SizeType.Percent, 0.5f));
				Table.Dock = DockStyle.Fill;
				groupBox.Controls.Add(Table);

				CheckBox RouteControl = new CheckBox();
				RouteControl.Text = "show";
				RouteControl.AutoSize = true;
				RouteControl.Checked = true;
				RouteControl.CheckedChanged += (sender, e) => RouteControlElement_CheckedChanged(sender, OverlayTag);
				RouteControl.Dock = DockStyle.Fill;
				Table.Controls.Add(RouteControl, 0, 0);

				Label Info = new Label();
				List<Geocache> GeocachesOnRoute = Program.Routes.First(x => x.Key == OverlayTag).Value2;
				int NumberOfGeocaches = GeocachesOnRoute.Count;
				float SumOfPoints = 0;
				foreach (Geocache GC in GeocachesOnRoute)
				{
					SumOfPoints += GC.Rating;
				}
				Info.Text = "Geocaches: " + NumberOfGeocaches + "\n Points: " + SumOfPoints;
				Info.Dock = DockStyle.Fill;
				Table.Controls.Add(Info, 1, 0);

				Button DeleteButton = new Button();
				DeleteButton.Text = "Delete";
				DeleteButton.Click += (sender, e) => DeleteButton_Click(sender, e, OverlayTag);
				DeleteButton.Dock = DockStyle.Fill;
				Table.Controls.Add(DeleteButton, 0, 1);

				Button ExportButton = new Button();
				ExportButton.Text = "Export";
				ExportButton.Click += (sender, e) => Export_Click(OverlayTag);
				ExportButton.Dock = DockStyle.Fill;
				Table.Controls.Add(ExportButton, 1, 1);

				MapTab_SideMenu.RowCount++;
				MapTab_SideMenu.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				MapTab_SideMenu.Controls.Add(groupBox, 0, MapTab_SideMenu.RowCount);
				RouteControl.Show();
			}
			else
			{
				newRouteControlElementDelegate dg = new newRouteControlElementDelegate(newRouteControlElement);
				BeginInvoke(dg, OverlayTag);
			}
		}

		delegate void AddFinalRouteDelegate(KeyValuePair<Route, List<Geocache>> Result, Routingprofile profile);
		public void AddFinalRoute(KeyValuePair<Route, List<Geocache>> Result, Routingprofile profile)
		{
			if (Map.InvokeRequired == false)
			{
				if(Map.Overlays.Count(x => x.Id == "PreliminaryRoute") > 0)
				{
					Map.Overlays.Remove(Map.Overlays.First(x => x.Id == "PreliminaryRoute"));//Remove the live displayed routes
				}

				Route FinalRoute = Result.Key;
				List<Geocache> GeocachesOnRoute = Result.Value; //Should be the same but anyways.

				//Name of the route which will be used for all further referencing
				string Routetag = profile.Name + " Route " + (profile.RoutesOfthisType + 1);

				Program.Routes.Add(new KeyValueTriple<string, Route, List<Geocache>>(Routetag, Result.Key, Result.Value));
				List<PointLatLng> GMAPRoute = new List<PointLatLng>();

				foreach (Coordinate COO in FinalRoute.Shape)
				{
					GMAPRoute.Add(new PointLatLng(COO.Latitude, COO.Longitude));
				}


				profile.RoutesOfthisType++;

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

				Map.Overlays.Add(RouteOverlay);
				newRouteControlElement(Routetag);
				Application.UseWaitCursor = false;
				Map.UseWaitCursor = false;
			}
			else
			{
				AddFinalRouteDelegate dg = new AddFinalRouteDelegate(AddFinalRoute);
				BeginInvoke(dg,new object[]{ Result,profile});
			}
		}

		//Doesn't work, kills performance
		delegate void DisplayPreliminaryRouteDelegate(List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>> RoutingData);
		public void DisplayPreliminaryRoute(List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>> RoutingData)
		{
			if (Program.DB.DisplayLiveCalculation)
			{
				if (Map.InvokeRequired == false)
				{
					if (Program.MainWindow.Map.Overlays.Count(x => x.Id == "PreliminaryRoute") != 0)
					{
						Program.MainWindow.Map.Overlays.Remove(Program.MainWindow.Map.Overlays.First(x => x.Id == "PreliminaryRoute"));
					}

					List<PointLatLng> GMAPRoute = new List<PointLatLng>();

					foreach (KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>> route in RoutingData)
					{
						foreach (Coordinate COO in route.Key.Shape)
						{
							GMAPRoute.Add(new PointLatLng(COO.Latitude, COO.Longitude));
						}
					}
					GMapOverlay RouteOverlay = new GMapOverlay("PreliminaryRoute");
					RouteOverlay.Routes.Add(new GMapRoute(GMAPRoute, "PreliminaryRoute"));
					Program.MainWindow.Map.Overlays.Add(RouteOverlay);
				}
				else
				{
					DisplayPreliminaryRouteDelegate dg = new DisplayPreliminaryRouteDelegate(DisplayPreliminaryRoute);
					BeginInvoke(dg, new object[] { RoutingData });
				}
			}
		}
		

		private void DeleteButton_Click(object sender, EventArgs e, string OverlayTag)
		{
			Program.Routes.Remove(Program.Routes.First(x => x.Key == OverlayTag));
			Map.Overlays.Remove(Map.Overlays.First(x => x.Id == OverlayTag));
			((Button)sender).Parent.Parent.Dispose();//=The Groupbox
		}

		private void Export_Click(string OverlayTag)
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

				KeyValueTriple<string, Route, List<Geocache>> RouteToSeialize = Program.Routes.First(x => x.Key == OverlayTag);

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
		
		private void RouteControlElement_CheckedChanged(object sender, string Overlaytag)
		{
			if (((CheckBox)sender).Checked)
			{
				Map.Overlays.First(x => x.Id == Overlaytag).IsVisibile = true;
			}
			else
			{
				Map.Overlays.First(x => x.Id == Overlaytag).IsVisibile = false;
			}
		}
		#endregion

		#region MapUIEvents
		bool Mapdragging = false;

		private void Map_OnMapDrag()
		{
			Mapdragging = true;
		}

		private void Map_MouseUp(object sender, MouseEventArgs e)
		{
			if (Mapdragging)//So calculation only kicks in if dragging is over
			{
				Program.DB.LastMapPosition = Map.Position;
				Program.Backup(null);
				Mapdragging = false;
			}
		}

		private void Map_Enter(object sender, EventArgs e)
		{
			LoadMap();
		}

		private void Map_OnMapZoomChanged()
		{
			Program.DB.LastMapPosition = Map.Position;//Since you can change position when zooming
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

		private void Map_Click(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				PointLatLng Coordinates = Map.FromLocalToLatLng(e.X, e.Y);
				ContextMenu contextMenu = new ContextMenu();
				// initialize the commands
				MenuItem SetEndpoint = new MenuItem("Set Endpoint here");
				SetEndpoint.Click += (new_sender, new_e) => SetEndpoint_Click(Coordinates);
				MenuItem SetStartpoint = new MenuItem("Set Startpoint here");
				SetStartpoint.Click += (new_sender, new_e) => SetStartpoint_Click(Coordinates);
				contextMenu.MenuItems.Add(SetStartpoint);
				contextMenu.MenuItems.Add(SetEndpoint);
				contextMenu.Show(Map,e.Location);
			}
		}

		private void SetStartpoint_Click(PointLatLng coordinates)
		{
			if (Map.Overlays.Count(x => x.Id == "StartEnd") > 0)
			{
				GMapOverlay Overlay = Map.Overlays.First(x => x.Id == "StartEnd");
				if (Overlay.Markers.Count(x => x.Tag.ToString() == "Startpoint") > 0)
				{
					Overlay.Markers.Remove(Map.Overlays.First(x => x.Id == "StartEnd").Markers.First(x => x.Tag.ToString() == "Startpoint"));
				}
				GMapMarker Startpoint = new GMarkerGoogle(coordinates, GMarkerGoogleType.green_big_go);
				Startpoint.Tag = "Endpoint";
				Overlay.Markers.Add(Startpoint);
			}
			else
			{
				GMapOverlay Overlay = new GMapOverlay("StartEnd");
				GMapMarker Startpoint = new GMarkerGoogle(coordinates, GMarkerGoogleType.green_big_go);
				Startpoint.Tag = "Startpoint";
				Overlay.Markers.Add(Startpoint);
				Map.Overlays.Add(Overlay);
			}

			StartpointTextbox.Text = coordinates.Lat.ToString(CultureInfo.InvariantCulture) + ";" + coordinates.Lng.ToString(CultureInfo.InvariantCulture);
		}

		private void SetEndpoint_Click(PointLatLng coordinates)
		{
			if (Map.Overlays.Count(x => x.Id == "StartEnd") > 0)
			{
				GMapOverlay Overlay = Map.Overlays.First(x => x.Id == "StartEnd");
				if(Overlay.Markers.Count(x => x.Tag.ToString() == "Endpoint")>0)
				{
					Overlay.Markers.Remove(Map.Overlays.First(x => x.Id == "StartEnd").Markers.First(x => x.Tag.ToString() == "Endpoint"));
				}
				GMapMarker Endpoint = new GMarkerGoogle(coordinates, GMarkerGoogleType.red_big_stop);
				Endpoint.Tag = "Endpoint";
				Overlay.Markers.Add(Endpoint);
			}
			else
			{
				GMapOverlay Overlay = new GMapOverlay("StartEnd");
				GMapMarker Endpoint = new GMarkerGoogle(coordinates, GMarkerGoogleType.red_big_stop);
				Endpoint.Tag = "Endpoint";
				Overlay.Markers.Add(Endpoint);
				Map.Overlays.Add(Overlay);
			}

			EndpointTextbox.Text = coordinates.Lat.ToString(CultureInfo.InvariantCulture) + ";" + coordinates.Lng.ToString(CultureInfo.InvariantCulture);
		}
		#endregion

		#endregion

		#region Settings
		private void EveryNthPointTextBox_TextChanged(object sender, EventArgs e)
		{
			if(int.TryParse(EveryNthPointTextBox.Text,out int value)){
				Program.DB.EveryNthShapepoint = value;
				Program.Backup(null);
			}
			else if(EveryNthPointTextBox.Text.Length!=0)
			{
				MessageBox.Show("Enter valid integers only.");
			}
		}

		private void DivisorTextBox_TextChanged(object sender, EventArgs e)
		{
			if (int.TryParse(DivisorTextBox.Text, out int value))
			{
				if (value == 0)
				{
					MessageBox.Show("Can't divide through 0");
				}
				else
				{
					Program.DB.Divisor = value;
					Program.Backup(null);
				}
			}
			else if(DivisorTextBox.Text.Length!=0)
			{
				MessageBox.Show("Enter valid integers only.");
			}
		}

		private void ToleranceTextBox_TextChanged(object sender, EventArgs e)
		{
			if (int.TryParse(ToleranceTextBox.Text, out int value))
			{
				Program.DB.Tolerance = value;
				Program.Backup(null);
			}
			else if (ToleranceTextBox.Text.Length != 0)
			{
				MessageBox.Show("Enter valid integers only.");
			}
			
		}

		private void RoutefindingWidth_Textbox_TextChanged(object sender, EventArgs e)
		{
			if (int.TryParse(RoutefindingWidth_Textbox.Text, out int value))
			{
				Program.DB.RoutefindingWidth = value;
				Program.Backup(null);
			}
			else if (ToleranceTextBox.Text.Length != 0)
			{
				MessageBox.Show("Enter valid integers only.");
			}
		}

		private void Autotargetselection_CheckedChanged(object sender, EventArgs e)
		{
			Program.DB.Autotargetselection = Autotargetselection.Checked;
		}


		private void LiveDisplayRouteCalculationCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Program.DB.DisplayLiveCalculation = LiveDisplayRouteCalculationCheckbox.Checked;
			Program.Backup(null);
		}
		
		public void UpdateSettingsTextBoxes()
		{
			if (Program.DB.Divisor == 0)
			{
				Program.DB.Divisor = 3;//Here agian, as this would be fatal.
			}
			EveryNthPointTextBox.Text = Program.DB.EveryNthShapepoint.ToString();
			DivisorTextBox.Text = Program.DB.Divisor.ToString();
			ToleranceTextBox.Text = Program.DB.Tolerance.ToString();
			RoutefindingWidth_Textbox.Text = Program.DB.RoutefindingWidth.ToString();
			Autotargetselection.Checked = Program.DB.Autotargetselection;
			/*LiveDisplayRouteCalculationCheckbox.Checked = Program.DB.DisplayLiveCalculation;*/
		}



		#endregion


		//UNDONE Attach this to EVERY Dropdownlist
		private void Dropdown_SelectedIndexChanged(object sender, EventArgs e)
		{
			((ComboBox)sender).Text = ((ComboBox)sender).SelectedItem.ToString();//So I can just check the text and it doesn't matter whether the user typed it or selected it

			if (sender == EditRoutingprofileCombobox)
			{
				Routingprofile_Click(sender, e);
				SelectedRoutingprofileCombobox.Text = EditRoutingprofileCombobox.Text;
			}
			else if (sender == EditRatingprofileCombobox)
			{
				Ratingprofile_Click(sender, e);
				SelectedRatingprofileCombobox.Text = EditRatingprofileCombobox.Text;
			}
			else if (sender == SelectedRoutingprofileCombobox)
			{
				Routingprofile_Click(sender, e);
				EditRoutingprofileCombobox.Text = SelectedRoutingprofileCombobox.Text;
			}
			else if (sender == SelectedRatingprofileCombobox)
			{
				Ratingprofile_Click(sender, e);
				EditRatingprofileCombobox.Text = SelectedRatingprofileCombobox.Text;
			}
		}

		
	}
}
