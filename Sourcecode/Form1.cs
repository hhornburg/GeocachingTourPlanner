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
using System.Diagnostics;
using System.Drawing.Drawing2D;

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

			//Browser
			try
			{
				webBrowser1.Navigate(new Uri(Application.StartupPath + "\\first-steps.html"));
			}
			catch (Exception)
			{
				MessageBox.Show("Can't show you the first steps, cause the needed html file is missing.");
			}
			

			//Map
			Map.DisableFocusOnMouseEnter = true;//So Windows put in foreground stay in foreground
			Map.DragButton = MouseButtons.Left;
			Map.IgnoreMarkerOnMouseWheel = true;

			Map.MapProvider = OpenCycleLandscapeMapProvider.Instance;
			GMaps.Instance.Mode = AccessMode.ServerAndCache;
			//Remove Cross in the middle of the Map
			Map.ShowCenter = false;

		}

		private void OpenWikiButton_Click(object sender, EventArgs e)
		{
			Process.Start("https://github.com/pingurus/GeocachingTourPlanner/wiki");
		}

		private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			if (e.Url != new Uri(Application.StartupPath + "\\first-steps.html"))
			{
				Process.Start(e.Url.ToString());
				e.Cancel = true;
			}
		}

		#region Overview

		private void ImportOSMDataButton_Click(object sender, EventArgs e)
		{
			Fileoperations.ImportOSMData();
		}

		private void ImportGeocachesButton_Click(object sender, EventArgs e)
		{
			Fileoperations.ImportGeocaches();
			LoadMap();
		}

		private void setGeocachedatabaseButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.OpenExistingDBFile(Databases.Geocaches))
			{
				Fileoperations.ReadGeocaches();
				LoadMap();
			}
		}

		private void setRoutingprofiledatabaseButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.OpenExistingDBFile(Databases.Routingprofiles))
			{
				Fileoperations.ReadRoutingprofiles();
			}
		}

		private void setRatingprofiledatabaseButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.OpenExistingDBFile(Databases.Ratingprofiles))
			{
				Fileoperations.ReadRatingprofiles();
			}
		}

		private void setRouterDBButton_Click(object sender, EventArgs e)
		{
			if (Program.DB.OpenExistingDBFile(Databases.RouterDB))
			{
				Fileoperations.ReadRouterDB();
			}
		}

		private void GetPQLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://www.geocaching.com/pocket/");
		}


		private void GetPbfLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://download.geofabrik.de/");
		}

		private void NewRatingprofileDatabaseButton_Click(object sender, EventArgs e)
		{
			Fileoperations.NewRatingprofileDatabase();
		}

		private void NewRoutingprofilesDatabaseButton_Click(object sender, EventArgs e)
		{
			Fileoperations.NewRoutingprofileDatabase();
		}

		delegate void SetRouterDBLabel_delegate(string text);
		public void SetRouterDBLabel(string text)
		{
			if (!RouterDBStateLabel.InvokeRequired)
			{
				RouterDBStateLabel.Text = text;
			}
			else
			{
				Invoke(new SetRouterDBLabel_delegate(SetRouterDBLabel), text);
			}
		}
		#endregion

		#region Update of Rating/Routingprofiles

		public void CreateRatingprofile(object sender, EventArgs e)
		{

			SetAllEmptyChildTextboxesToZero(RatingprofilesSettingsTabelLayout);
			Ratingprofile Profile = new Ratingprofile();
			if (RatingProfileName.Text == "")
			{
				MessageBox.Show("Please set a name");
				return;
			}
			try
			{
				Profile.Name = RatingProfileName.Text;
				Profile.TypePriority = int.Parse(TypePriorityValue.Text);
				Profile.TypeRatings = new List<KeyValuePair<GeocacheType, int>>();
				Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.EarthCache, int.Parse(EarthcacheValue.Text)));
				Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Letterbox, int.Parse(LetterboxValue.Text)));
				Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Multi, int.Parse(MultiValue.Text)));
				Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Mystery, int.Parse(MysteryValue.Text)));
				Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Other, int.Parse(OtherTypeValue.Text)));
				Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Traditional, int.Parse(TraditionalValue.Text)));
				Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Virtual, int.Parse(VirtualValue.Text)));
				Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Webcam, int.Parse(WebcamValue.Text)));
				Profile.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Wherigo, int.Parse(WherigoValue.Text)));

				Profile.SizePriority = int.Parse(SizePriorityValue.Text);
				Profile.SizeRatings = new List<KeyValuePair<GeocacheSize, int>>();
				Profile.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Large, int.Parse(LargeValue.Text)));
				Profile.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Micro, int.Parse(MicroValue.Text)));
				Profile.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Other, int.Parse(OtherSizeValue.Text)));
				Profile.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Regular, int.Parse(RegularValue.Text)));
				Profile.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Small, int.Parse(SmallValue.Text)));

				Profile.DPriority = int.Parse(DPriorityValue.Text);
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

				Profile.TPriority = int.Parse(TPriorityValue.Text);
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
					Profile.Yearmode = Yearmode.multiply;
				}
				else
				{
					Profile.Yearmode = Yearmode.square_n_divide;
				}

				Profile.Yearfactor = int.Parse(AgeFactorValue.Text);

				if (Profile.Yearmode == Yearmode.square_n_divide && Profile.Yearfactor == 0)
				{
					MessageBox.Show("Don't you dare to divide by 0!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

			}
			catch (FormatException)
			{
				MessageBox.Show("Please fill all fields");
				return;
			}
			catch (NullReferenceException)
			{
				MessageBox.Show("Please select something from all comboboxes");
				return;
			}

			//Eintragen des neuen Profils
			foreach (Ratingprofile BP in Program.Ratingprofiles.Where(x => x.Name == Profile.Name).ToList())//Make sure only one profile with a name exists
			{
				Program.Ratingprofiles.Remove(BP);
			}
			UpdateStatus("Ratingprofile saved");
			Program.Ratingprofiles.Add(Profile);
			//The Dropdownmenu gets updated through an event handler
			Fileoperations.Backup(Program.Ratingprofiles);
			EditRatingprofileCombobox.SelectedItem = Profile.Name; //Eventhandler takes care of same profile selected
		}

		public void CreateRoutingprofile(object sender, EventArgs e)
		{
			SetAllEmptyChildTextboxesToZero(RoutingprofilesSettingsTableLayout);

			Routingprofile Profile = new Routingprofile();
			if (RoutingProfileName.Text == null)
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
				MessageBox.Show("Some fields are filled with incompatible Values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//Eintragen des neuen Profils
			foreach (Routingprofile BP in Program.Routingprofiles.Where(x => x.Name == Profile.Name).ToList())
			{
				Profile.RoutesOfthisType = BP.RoutesOfthisType;
				Program.Routingprofiles.Remove(BP);
			}
			UpdateStatus("Routingprofile saved");
			Program.Routingprofiles.Add(Profile);
			//The Dropdownmenu is updated via an event handler
			Fileoperations.Backup(Program.Routingprofiles);
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

		private void DeleteRoutingprofileButton_Click(object sender, EventArgs e)
		{
			Routingprofile Profile = new Routingprofile();
			if (RoutingProfileName.Text == null)
			{
				MessageBox.Show("Please set Name");
				return;
			}
			Profile.Name = RoutingProfileName.Text;

			ClearAllChildTextAndComboboxes(RoutingprofilesSettingsTableLayout);
			ClearAllChildTextAndComboboxes(SaveRoutingProfileTableLayout);
			SelectedRoutingprofileCombobox.Text = null;

			foreach (Routingprofile BP in Program.Routingprofiles.Where(x => x.Name == Profile.Name).ToList())
			{
				Program.Routingprofiles.Remove(BP);
			}

			UpdateStatus("Routingprofile deleted");
			Fileoperations.Backup(Program.Routingprofiles);
		}

		private void DeleteRatingprofileButton_Click(object sender, EventArgs e)
		{
			Ratingprofile Profile = new Ratingprofile();
			if (RatingProfileName.Text == null)
			{
				MessageBox.Show("Please set Name");
				return;
			}
			Profile.Name = RatingProfileName.Text;
			foreach (Ratingprofile RP in Program.Ratingprofiles.Where(x => x.Name == Profile.Name).ToList())
			{
				Program.Ratingprofiles.Remove(RP);
			}

			ClearAllChildTextAndComboboxes(RatingprofilesSettingsTabelLayout);
			ClearAllChildTextAndComboboxes(SaveRatingprofileLayoutPanel);
			SelectedRatingprofileCombobox.Text = null;
			UpdateStatus("deleted ratingprofile");
			Fileoperations.Backup(Program.Ratingprofiles);
		}

		private void Ratingprofile_Click(object sender, EventArgs e)
		{
			Ratingprofile SelectedRatingprofile = Program.Ratingprofiles.First(x => x.Name == ((ComboBox)sender).Text);

			try
			{
				//Name des Profils
				RatingProfileName.Text = SelectedRatingprofile.Name;

				//Prioritäten
				TypePriorityValue.Text =SelectedRatingprofile.TypePriority.ToString();
				SizePriorityValue.Text =SelectedRatingprofile.SizePriority.ToString();
				DPriorityValue.Text =SelectedRatingprofile.DPriority.ToString();
				TPriorityValue.Text =SelectedRatingprofile.TPriority.ToString();

				//TypenValueungen
				TraditionalValue.Text =SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Traditional).Value.ToString();
				EarthcacheValue.Text =SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.EarthCache).Value.ToString();
				MultiValue.Text =SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Multi).Value.ToString();
				MysteryValue.Text =SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Mystery).Value.ToString();
				LetterboxValue.Text =SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Letterbox).Value.ToString();
				VirtualValue.Text =SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Virtual).Value.ToString();
				OtherTypeValue.Text =SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Other).Value.ToString();
				WebcamValue.Text =SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Webcam).Value.ToString();
				WherigoValue.Text =SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Wherigo).Value.ToString();

				//Größe
				LargeValue.Text =SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Large).Value.ToString();
				RegularValue.Text =SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Regular).Value.ToString();
				SmallValue.Text =SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Small).Value.ToString();
				MicroValue.Text =SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Micro).Value.ToString();
				OtherSizeValue.Text =SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Other).Value.ToString();

				//D
				D1Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 1).Value.ToString();
				D15Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 1.5).Value.ToString();
				D2Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 2).Value.ToString();
				D25Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 2.5).Value.ToString();
				D3Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 3).Value.ToString();
				D35Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 3.5).Value.ToString();
				D4Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 4).Value.ToString();
				D45Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 4.5).Value.ToString();
				D5Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 5).Value.ToString();

				//T
				T1Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 1).Value.ToString();
				T15Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 1.5).Value.ToString();
				T2Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 2).Value.ToString();
				T25Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 2.5).Value.ToString();
				T3Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 3).Value.ToString();
				T35Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 3.5).Value.ToString();
				T4Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 4).Value.ToString();
				T45Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 4.5).Value.ToString();
				T5Value.Text =SelectedRatingprofile.DRatings.First(x => x.Key == 5).Value.ToString();

				//Sonstige
				NMFlagValue.Text = SelectedRatingprofile.NMPenalty.ToString();
				if (SelectedRatingprofile.Yearmode == Yearmode.multiply)
				{
					AgeValue.SelectedItem = AgeValue.Items[0];
				}
				else
				{
					AgeValue.SelectedItem = AgeValue.Items[1];
				}
				AgeFactorValue.Text =SelectedRatingprofile.Yearfactor.ToString();
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
		delegate void Loadmap_Delegate();
		/// <summary>
		/// Updates Map
		/// </summary>
		public void LoadMap()
		{
			if (!Map.InvokeRequired)
			{
				//TODO make this more intelligent

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
					if (GC.Rating > (Program.DB.MinimalRating) + 0.67 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
					{
						GCMarker = Markers.GetGeocacheMarker(GC);
						TopOverlay.Markers.Add(GCMarker);
					}
					else if (GC.Rating > (Program.DB.MinimalRating) + 0.33 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
					{
						GCMarker = Markers.GetGeocacheMarker(GC);
						MediumOverlay.Markers.Add(GCMarker);
					}
					else
					{
						GCMarker = Markers.GetGeocacheMarker(GC);
						LowOverlay.Markers.Add(GCMarker);
					}
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
			else
			{
				Loadmap_Delegate dg = new Loadmap_Delegate(LoadMap);
				Invoke(dg);
			}

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
			LoadMap();
			UpdateStatus("Geocaches rated");
			Fileoperations.Backup(Program.Geocaches);
		}
		
		private void CreateRouteButtonClick(object sender, EventArgs e)
		{
			if (!Program.RouteCalculationRunning && !Program.ImportOfOSMDataRunning)
			{
				UpdateStatus("Started Route Calculation");
				Program.RouteCalculationRunning = true;
				Application.UseWaitCursor = true;

				#region get values
				if (SelectedRoutingprofileCombobox.Text.Length == 0)
				{
					MessageBox.Show("No Routingprofile set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Program.RouteCalculationRunning = false;
					Program.RouteCalculationRunning = false;  Application.UseWaitCursor = false;
					return;
				}

				Routingprofile SelectedProfile = Program.Routingprofiles.First(x => x.Name == SelectedRoutingprofileCombobox.Text);

				List<Geocache> GeocachesToInclude = new List<Geocache>();
				foreach (Geocache GC in Program.Geocaches.Where(x => x.ForceInclude))
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
					Program.RouteCalculationRunning = false;  Application.UseWaitCursor = false;
					return;
				}

				if (!float.TryParse(StartpointTextbox.Text.Substring(0, StartpointTextbox.Text.IndexOf(";") - 1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out StartLat))
				{
					MessageBox.Show("Couldn't parse latitude of Startcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Program.RouteCalculationRunning = false;  Application.UseWaitCursor = false;
					return;
				}
				if (!float.TryParse(StartpointTextbox.Text.Substring(StartpointTextbox.Text.IndexOf(";") + 1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out StartLon))
				{
					MessageBox.Show("Couldn't parse longitude Startcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Program.RouteCalculationRunning = false;  Application.UseWaitCursor = false;
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
						Program.RouteCalculationRunning = false;  Application.UseWaitCursor = false;
						return;
					}
				}
				else
				{
					if (!float.TryParse(EndpointTextbox.Text.Substring(0, EndpointTextbox.Text.IndexOf(";") - 1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out EndLat))
					{

						MessageBox.Show("Couldn't parse latitude of Endcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						Program.RouteCalculationRunning = false;  Application.UseWaitCursor = false;
						return;

					}
					if (!float.TryParse(EndpointTextbox.Text.Substring(EndpointTextbox.Text.IndexOf(";") + 1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out EndLon))
					{
						//Big procedure only once, as the result would be the same
						MessageBox.Show("Couldn't parse longitude of Endcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						Program.RouteCalculationRunning = false;  Application.UseWaitCursor = false;
						return;
					}
				}

				//Check if Start and Endpoint have been selected
				if (StartLat == 0 && StartLon == 0 && EndLat == 0 && EndLon == 0)
				{
					MessageBox.Show("Please select a Startingpoint and a Finalpoint", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Program.RouteCalculationRunning = false;  Application.UseWaitCursor = false;
					return;
				}
				#endregion

				new Thread(new ThreadStart(() =>
				{
					new Tourplanning().GetRoute_Recursive(SelectedProfile, Program.Geocaches.ToList(), new Coordinate(StartLat, StartLon), new Coordinate(EndLat, EndLon), GeocachesToInclude);
				}
				)).Start();
			}else if (Program.ImportOfOSMDataRunning)
			{
				MessageBox.Show("Please wait until the OSM Data is imported.");
			}
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
				groupBox.AutoSize = true;
				groupBox.Dock = DockStyle.Fill;

				TableLayoutPanel Table = new TableLayoutPanel();
				Table.RowCount = 2;
				Table.ColumnCount = 2;
				Table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
				Table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
				Table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				Table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				Table.Dock = DockStyle.Fill;
				Table.AutoSize = true;
				groupBox.Controls.Add(Table);

				CheckBox RouteControl = new CheckBox();
				RouteControl.Text = "show";
				RouteControl.AutoSize = true;
				RouteControl.Checked = true;
				RouteControl.CheckedChanged += (sender, e) => RouteControlElement_CheckedChanged(sender, OverlayTag);
				RouteControl.Dock = DockStyle.Fill;
				Table.Controls.Add(RouteControl, 0, 0);

				Label Info = new Label();
				Tourplanning.RouteData ThisRouteData = Program.Routes.First(x => x.Key == OverlayTag).Value;
				List<Geocache> GeocachesOnRoute = ThisRouteData.GeocachesOnRoute();
				int NumberOfGeocaches = GeocachesOnRoute.Count;
				float SumOfPoints = 0;
				foreach (Geocache GC in GeocachesOnRoute)
				{
					SumOfPoints += GC.Rating;
				}
				float Length = ThisRouteData.TotalDistance / 1000;
				TimeSpan TimeNeeded = TimeSpan.FromSeconds(ThisRouteData.TotalTime) + TimeSpan.FromMinutes(GeocachesOnRoute.Count * ThisRouteData.Profile.TimePerGeocache); 
				Info.Text = "Geocaches: " + NumberOfGeocaches + "\nPoints: " + SumOfPoints + "\nLength in km: " + Length.ToString("#.##") + "\n Time in min:" + TimeNeeded.TotalMinutes.ToString("#.");
				Info.Dock = DockStyle.Fill;
				Info.AutoSize = true;
				Table.Controls.Add(Info, 1, 0);

				Button DeleteButton = new Button();
				DeleteButton.Text = "Delete";
				DeleteButton.Click += (sender, e) => DeleteButton_Click(sender, e, OverlayTag);
				DeleteButton.Dock = DockStyle.Fill;
				DeleteButton.Height = 20;
				Table.Controls.Add(DeleteButton, 0, 1);

				Button ExportButton = new Button();
				ExportButton.Text = "Export";
				ExportButton.Click += (sender, e) => Export_Click(OverlayTag);
				ExportButton.Dock = DockStyle.Fill;
				ExportButton.Height = 20;
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

		delegate void AddFinalRouteDelegate(Tourplanning.RouteData Result);
		public void AddFinalRoute(Tourplanning.RouteData Result)
		{
			if (Map.InvokeRequired == false)
			{
				while(Map.Overlays.Count(x => x.Id == "PreliminaryRoute") > 0)
				{
					Map.Overlays.Remove(Map.Overlays.First(x => x.Id == "PreliminaryRoute"));//Remove the live displayed routes
				}

				Route FinalRoute = Result.partialRoutes[0].partialRoute;

				for (int i = 1; i < Result.partialRoutes.Count; i++)
				{
					FinalRoute = FinalRoute.Concatenate(Result.partialRoutes[i].partialRoute);
				}

				List<Geocache> GeocachesOnRoute = Result.GeocachesOnRoute();

				//Name of the route which will be used for all further referencing
				string Routetag = Result.Profile.Name + " Route " + (Result.Profile.RoutesOfthisType + 1);

				Program.Routes.Add(new KeyValuePair<string, Tourplanning.RouteData>(Routetag, Result));
				List<PointLatLng> LatLongPointList = new List<PointLatLng>();

				foreach (Coordinate COO in FinalRoute.Shape)
				{
					LatLongPointList.Add(new PointLatLng(COO.Latitude, COO.Longitude));
				}

				Result.Profile.RoutesOfthisType++;

				GMapOverlay RouteOverlay = new GMapOverlay(Routetag);
				GMapRoute Route = new GMapRoute(LatLongPointList, Routetag);
				RouteOverlay.Routes.Add(Route);
				foreach (Geocache GC in GeocachesOnRoute)
				{
					GMapMarker GCMarker = Markers.GetGeocacheMarker(GC);
					RouteOverlay.Markers.Add(GCMarker);
				}

				Map.Overlays.Add(RouteOverlay);
				newRouteControlElement(Routetag);
				Program.RouteCalculationRunning = false;  Application.UseWaitCursor = false;
				Map.Cursor = Cursors.Default;
				LoadMap();
			}
			else
			{
				AddFinalRouteDelegate dg = new AddFinalRouteDelegate(AddFinalRoute);
				BeginInvoke(dg, Result);
			}
		}

		//Doesn't work, kills performance
		delegate void DisplayPreliminaryRouteDelegate(Route PreliminaryRoute);
		public void DisplayPreliminaryRoute(Route PreliminaryRoute)
		{
			if (Program.DB.DisplayLiveCalculation && Program.RouteCalculationRunning)//If the calculation is not running anymore, the thread was late and no route should be displayed
			{
				if (Map.InvokeRequired == false)
				{
					while (Program.MainWindow.Map.Overlays.Count(x => x.Id == "PreliminaryRoute") != 0)
					{
						Program.MainWindow.Map.Overlays.Remove(Program.MainWindow.Map.Overlays.First(x => x.Id == "PreliminaryRoute"));
					}

					List<PointLatLng> GMAPRoute = new List<PointLatLng>();

						foreach (Coordinate COO in PreliminaryRoute.Shape)
						{
							GMAPRoute.Add(new PointLatLng(COO.Latitude, COO.Longitude));
						}
					GMapOverlay RouteOverlay = new GMapOverlay("PreliminaryRoute");
					RouteOverlay.Routes.Add(new GMapRoute(GMAPRoute, "PreliminaryRoute"));
					Program.MainWindow.Map.Overlays.Add(RouteOverlay);
				}
				else
				{
					DisplayPreliminaryRouteDelegate dg = new DisplayPreliminaryRouteDelegate(DisplayPreliminaryRoute);
					BeginInvoke(dg, PreliminaryRoute);
				}
				LoadMap();
			}
		}

		private void DeleteButton_Click(object sender, EventArgs e, string OverlayTag)
		{
			Program.Routes.Remove(Program.Routes.First(x => x.Key == OverlayTag));
			Map.Overlays.Remove(Map.Overlays.First(x => x.Id == OverlayTag));
			((Button)sender).Parent.Parent.Dispose();//=The Groupbox
			LoadMap();
		}

		private void Export_Click(string OverlayTag)
		{
			Fileoperations.ExportGPX(OverlayTag);
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
			LoadMap();
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
				Fileoperations.Backup(null);
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
			Fileoperations.Backup(null);
		}


		private void Map_Load(object sender, EventArgs e)//Called at the first time the tab gets clicked. This way the user doesn't see an empty map
		{
			LoadMap();
		}

		
		private void Map_OnMarkerClick(GMapMarker item, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && item.Tag.ToString() != "Endpoint" && item.Tag.ToString() != "Startpoint")
			{
				System.Diagnostics.Process.Start("https://www.coord.info/" + item.Tag);
			}
			else if (e.Button == MouseButtons.Right && item.Tag.ToString()!= "Endpoint" && item.Tag.ToString() != "Startpoint")
			{
				ContextMenu MapContextMenu = new ContextMenu();
				// initialize the commands
				MenuItem SetEndpoint = new MenuItem("Set Endpoint here");
				SetEndpoint.Click += (new_sender, new_e) => SetEndpoint_Click(item.Position);
				MenuItem SetStartpoint = new MenuItem("Set Startpoint here");
				SetStartpoint.Click += (new_sender, new_e) => SetStartpoint_Click(item.Position);

				Geocache geocache = Program.Geocaches.First(x => x.GCCODE == item.Tag.ToString());
				MenuItem SetForceInclude = new MenuItem("ForceInclude");
				if (geocache.ForceInclude)
				{
					SetForceInclude.Checked=true;
				}
				SetForceInclude.Click += (new_sender, new_e) => toggleForceInclude(geocache);

				MapContextMenu.MenuItems.Add(SetStartpoint);
				MapContextMenu.MenuItems.Add(SetEndpoint);
				MapContextMenu.MenuItems.Add(SetForceInclude);
				MapContextMenu.Show(Map, e.Location);
			}
		}

		private void Map_Click(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && !((GMapControl)sender).IsMouseOverMarker)
			{
				ContextMenu MapContextMenu = new ContextMenu();
				PointLatLng Coordinates = Map.FromLocalToLatLng(e.X, e.Y);
				// initialize the commands
				MenuItem SetEndpoint = new MenuItem("Set Endpoint here");
				SetEndpoint.Click += (new_sender, new_e) => SetEndpoint_Click(Coordinates);
				MenuItem SetStartpoint = new MenuItem("Set Startpoint here");
				SetStartpoint.Click += (new_sender, new_e) => SetStartpoint_Click(Coordinates);
				MapContextMenu.MenuItems.Add(SetStartpoint);
				MapContextMenu.MenuItems.Add(SetEndpoint);
				MapContextMenu.Show(Map,e.Location);
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
				Startpoint.Tag = "Startpoint";
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

		private void toggleForceInclude(Geocache geocache)
		{
			if (geocache.ForceInclude)
			{
				geocache.ForceInclude = false;
			}
			else
			{
				geocache.ForceInclude = true;
			}
		}
		#endregion

		#endregion

		#region Settings
		private void EveryNthPointTextBox_TextChanged(object sender, EventArgs e)
		{

			if(int.TryParse(EveryNthPointTextBox.Text,out int Value)){
				Program.DB.EveryNthShapepoint = Value;
				Fileoperations.Backup(null);

			}
			else if(EveryNthPointTextBox.Text.Length!=0)
			{
				MessageBox.Show("Enter valid integers only.");
			}
		}

		private void PercentageOFRemainingDistance_TextChanged(object sender, EventArgs e)
		{
			if (int.TryParse(PercentageOFRemainingDistance.Text, out int Value))
			{
				if (Value > 100)
				{
					MessageBox.Show("Percentage can't be bigger than 100");
					AutotargetselectionMaxTextBox.Text = 100.ToString();
				}
				else if (Value < 0)
				{
					MessageBox.Show("Percentage can't be smaller than 0");
					PercentageOFRemainingDistance.Text = 0.ToString();
				}
				else
				{
					Program.DB.PercentageOfRemainingDistance = Value/100;
					Fileoperations.Backup(null);
				}
			}
			else if(PercentageOFRemainingDistance.Text.Length!=0)
			{
				MessageBox.Show("Enter valid integers only.");
			}
		}

		#region Autotargetselection Max
		bool AutotargetselectionMax_TextChanged = false;
		private void AutotargetselectionMaxTextbox_TextChanged(object sender, EventArgs e)
		{
			AutotargetselectionMax_TextChanged = true;

		}

		private void AutotargetselectionMaxTextBox_Leave(object sender, EventArgs e)
		{
			if (AutotargetselectionMax_TextChanged)
			{
				if (int.TryParse(AutotargetselectionMaxTextBox.Text, out int Value))
				{
					if (Value > 100)
					{
						MessageBox.Show("Percentage can't be bigger than 100");
						AutotargetselectionMaxTextBox.Text = 100.ToString();
					}
					else if (Value < 0)
					{
						MessageBox.Show("Percentage can't be smaller than 0");
						AutotargetselectionMaxTextBox.Text = (Program.DB.PercentageOfDistanceInAutoTargetselection_Min * 100 + 1).ToString();
					}
					else if (Value < 100 * Program.DB.PercentageOfDistanceInAutoTargetselection_Min)
					{
						MessageBox.Show("Percentage can't be smaller than that of the Minimum");
						AutotargetselectionMaxTextBox.Text = (Program.DB.PercentageOfDistanceInAutoTargetselection_Min * 100 + 1).ToString();
					}
					else
					{

						Program.DB.PercentageOfDistanceInAutoTargetselection_Max = Value / 100;
						Fileoperations.Backup(null);
					}
				}
				else if (AutotargetselectionMaxTextBox.Text.Length != 0)
				{
					MessageBox.Show("Enter valid integers only.");
				}
			}
			AutotargetselectionMax_TextChanged = false;
		}
		#endregion

		#region Autotargetselection Min
		bool AutotargetselectionMin_TextChanged = false;
		private void AutotargetselectionMinTextBox_TextChanged(object sender, EventArgs e)
		{
			AutotargetselectionMin_TextChanged = true;
		}

		private void AutotargetselectionMinTextBox_Leave(object sender, EventArgs e)
		{
			if (AutotargetselectionMin_TextChanged)
			{
				if (int.TryParse(AutotargetselectionMinTextBox.Text, out int Value))
				{
					if (Value > 100)
					{
						MessageBox.Show("Percentage can't be bigger than 100");
						AutotargetselectionMaxTextBox.Text = (Program.DB.PercentageOfDistanceInAutoTargetselection_Max * 100 - 1).ToString();
					}
					else if (Value < 0)
					{
						MessageBox.Show("Percentage can't be smaller than 0");
						AutotargetselectionMaxTextBox.Text = 0.ToString();
					}
					else if (Value > 100 * Program.DB.PercentageOfDistanceInAutoTargetselection_Max * 100)
					{
						MessageBox.Show("Percentage can't be bigger than that of the Maximum");
						AutotargetselectionMaxTextBox.Text = (Program.DB.PercentageOfDistanceInAutoTargetselection_Max - 1).ToString();
					}
					else
					{

						Program.DB.PercentageOfDistanceInAutoTargetselection_Min = Value / 100;
						Fileoperations.Backup(null);
					}
				}
				else if (AutotargetselectionMinTextBox.Text.Length != 0)
				{
					MessageBox.Show("Enter valid integers only.");
				}
			}
			AutotargetselectionMin_TextChanged = false;
		}
		#endregion

		private void RoutefindingWidth_Textbox_TextChanged(object sender, EventArgs e)
		{
			if (int.TryParse(RoutefindingWidth_Textbox.Text, out int Value))
			{
				Program.DB.RoutefindingWidth = Value;
				Fileoperations.Backup(null);

			}
			else if (RoutefindingWidth_Textbox.Text.Length != 0)
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
			Fileoperations.Backup(null);
		}


		private void MarkerSizeTrackBar_Scroll(object sender, EventArgs e)
		{
			Program.MarkerImageCache.Clear();
			Program.DB.MarkerSize = MarkerSizeTrackBar.Value;
			LoadMap();
			Fileoperations.Backup(null);
		}

		public void UpdateSettingsTextBoxes()
		{
			if (Program.DB.PercentageOfRemainingDistance == 0)
			{
				Program.DB.PercentageOfRemainingDistance = 0.75f;
			}
			EveryNthPointTextBox.Text = Program.DB.EveryNthShapepoint.ToString();
			PercentageOFRemainingDistance.Text = (Program.DB.PercentageOfRemainingDistance*100).ToString();
			RoutefindingWidth_Textbox.Text = Program.DB.RoutefindingWidth.ToString();
			Autotargetselection.Checked = Program.DB.Autotargetselection;
			MarkerSizeTrackBar.Value = Program.DB.MarkerSize;
			AutotargetselectionMinTextBox.Text= (Program.DB.PercentageOfDistanceInAutoTargetselection_Min * 100).ToString();
			AutotargetselectionMaxTextBox.Text = (Program.DB.PercentageOfDistanceInAutoTargetselection_Max * 100).ToString();
			/*LiveDisplayRouteCalculationCheckbox.Checked = Program.DB.DisplayLiveCalculation;*/
		}



		#endregion

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Fileoperations.Backup(Program.Geocaches);
			Fileoperations.Backup(Program.Ratingprofiles);
			Fileoperations.Backup(Program.Routingprofiles);

		}

		ToolTip ProgressTooltip = new ToolTip();
		delegate void UpdateStatusDelegate(string message, int progress = 0);
		public void UpdateStatus(string message, int progress = 0)
		{
			if (!StatusLabel.InvokeRequired)
			{
				File.AppendAllText("Log.txt", "[" + DateTime.Now + "]: " + message + "\n");
				if (progress != 0)//If the status changes while this is still processing and the new one isn't time consuming (currently there are only two time consuming methods, which cannot run simultaneously), the tooltip still chows the old information, so one can still check what happens
				{

					ProgressBar.MouseHover += (sender, e) => ShowTooltip(message, sender);
					ProgressBar.Value = progress;
				}
				else if (ProgressBar.Value == 100)//Thus the previous operation has finished
				{
					ProgressBar.Value = 0;
					ProgressBar.MouseHover += (sender, e) => ShowTooltip(message, sender);
				}
				StatusLabel.Text = message;
			}
			else
			{
				Invoke(new UpdateStatusDelegate(UpdateStatus),new object[]{ message, progress});
			}
		}

		private void ShowTooltip(string message, object control)
		{
			ProgressTooltip.Show(message, ProgressBar);
		}

		#region helpers
		//UNDONE Attach this to EVERY Dropdownlist
		private void Dropdown_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)sender).SelectedItem != null)//happens when profile is deleted
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

		/// <summary>
		/// Sets the text of all TextBox- and ComboBox- children of the specified parent to null. Does this recursively through 3 layers.
		/// </summary>
		/// <param name="parent"></param>
		private void ClearAllChildTextAndComboboxes(Control parent)
		{
			foreach (Control C in parent.Controls)
			{
				if (C is TextBox || C is ComboBox)
				{
					C.Text = null;
				}
				else if (C is GroupBox || C is TableLayoutPanel)
				{
					foreach (Control C2 in C.Controls)
					{
						if (C2 is TextBox || C2 is ComboBox)
						{
							C2.Text = null;
						}
						else if (C2 is GroupBox || C2 is TableLayoutPanel)
						{
							foreach (Control C3 in C2.Controls)
							{
								if (C3 is TextBox || C3 is ComboBox)
								{
									C3.Text = null;
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Sets the text of all TextBoxchildren of the specified parent to "0". Does this recursively through 3 layers.
		/// </summary>
		/// <param name="parent"></param>
		private void SetAllEmptyChildTextboxesToZero(Control parent)
		{
			foreach (Control C in parent.Controls)
			{
				if (C is TextBox && C.Text == "")
				{
					C.Text = "0";
				}
				else if (C is GroupBox || C is TableLayoutPanel)
				{
					foreach (Control C2 in C.Controls)
					{
						if (C2 is TextBox && C2.Text == "")
						{
							C2.Text = "0";
						}
						else if (C2 is GroupBox || C2 is TableLayoutPanel)
						{
							foreach (Control C3 in C2.Controls)
							{
								if (C3 is TextBox && C3.Text == "")
								{
									C3.Text = "0";
								}
							}
						}
					}
				}
			}
		}
		#endregion

		
	}
}
