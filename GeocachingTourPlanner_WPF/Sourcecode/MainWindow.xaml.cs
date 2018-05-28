using GeocachingTourPlanner;
using Itinero;
using Itinero.LocalGeo;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.UI;
using Mapsui.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace GeocachingTourPlanner_WPF
{
	
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Map map;

		public MainWindow()
		{
			InitializeComponent();
			/*
			//Tabelleneinstellungen
			GeocacheTable.DataSource = App.Geocaches;
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
			App.Geocaches.ResetBindings();
			*/

			//Browser
			try
			{
				//FIX webBrowser1.Navigate(new Uri(Application.StartupPath + "\\first-steps.html"));
			}
			catch (Exception)
			{
				MessageBox.Show("Can't show you the first steps, cause the needed html file is missing.");
			}


			//Map
			map = mapControl.Map;
			map.Layers.Add(OpenStreetMap.CreateTileLayer());

		}

		#region UI Events

		#region First Steps
		/*
		private void OpenWikiButton_Click(object sender, EventArgs e)
		{
			Process.Start("https://github.com/pingurus/GeocachingTourPlanner/wiki");
		}

		
		private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			/*FIX
			if (e.Url != new Uri(Application.StartupPath + "\\first-steps.html"))
			{
				Process.Start(e.Url.ToString());
				e.Cancel = true;
			}
		}
		*/
			#endregion

			#region Overview
			private void ImportOSMDataButton_Click(object sender, RoutedEventArgs e)
		{
			Fileoperations.ImportOSMData();
		}

		private void ImportGeocachesButton_Click(object sender, RoutedEventArgs e)
		{
			Fileoperations.ImportGeocaches();
			LoadMap();
		}

		private void setGeocachedatabaseButton_Click(object sender, RoutedEventArgs e)
		{
			if (App.DB.OpenExistingDBFile(Databases.Geocaches))
			{
				Fileoperations.ReadGeocaches();
				LoadMap();
				Fileoperations.Backup(null);
			}
		}

		private void setRoutingprofiledatabaseButton_Click(object sender, RoutedEventArgs e)
		{
			if (App.DB.OpenExistingDBFile(Databases.Routingprofiles))
			{
				Fileoperations.ReadRoutingprofiles();
			}
		}

		private void setRatingprofiledatabaseButton_Click(object sender, RoutedEventArgs e)
		{
			if (App.DB.OpenExistingDBFile(Databases.Ratingprofiles))
			{
				Fileoperations.ReadRatingprofiles();
			}
		}

		private void setRouterDBButton_Click(object sender, RoutedEventArgs e)
		{
			if (App.DB.OpenExistingDBFile(Databases.RouterDB))
			{
				Fileoperations.ReadRouterDB();
			}
		}

		private void OpenInBrowser(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		private void NewRatingprofileDatabaseButton_Click(object sender, RoutedEventArgs e)
		{
			Fileoperations.NewRatingprofileDatabase();
		}

		private void NewRoutingprofilesDatabaseButton_Click(object sender, RoutedEventArgs e)
		{
			Fileoperations.NewRoutingprofileDatabase();
		}
		#endregion
			
		#region Update of Rating/Routingprofiles
		private void CreateRatingprofile(object sender, EventArgs e)
		{
			SetAllEmptyChildTextboxesToZero(RatingprofilesSettingsGrid);
			Ratingprofile Profile = new Ratingprofile();
			if (RatingprofileName.Text == "")
			{
				MessageBox.Show("Please set a name");
				return;
			}
			try
			{
				Profile.Name = RatingprofileName.Text;
				Profile.TypePriority = int.Parse(TypePriorityValue.Text);
				Profile.TypeRatings = new List<SerializableKeyValuePair<GeocacheType, int>>();
				Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.EarthCache, int.Parse(EarthcacheValue.Text)));
				Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Letterbox, int.Parse(LetterboxValue.Text)));
				Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Multi, int.Parse(MultiValue.Text)));
				Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Mystery, int.Parse(MysteryValue.Text)));
				Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Other, int.Parse(OtherTypeValue.Text)));
				Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Traditional, int.Parse(TraditionalValue.Text)));
				Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Virtual, int.Parse(VirtualValue.Text)));
				Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Webcam, int.Parse(WebcamValue.Text)));
				Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Wherigo, int.Parse(WherigoValue.Text)));

				Profile.SizePriority = int.Parse(SizePriorityValue.Text);
				Profile.SizeRatings = new List<SerializableKeyValuePair<GeocacheSize, int>>();
				Profile.SizeRatings.Add(new SerializableKeyValuePair<GeocacheSize, int>(GeocacheSize.Large, int.Parse(LargeValue.Text)));
				Profile.SizeRatings.Add(new SerializableKeyValuePair<GeocacheSize, int>(GeocacheSize.Micro, int.Parse(MicroValue.Text)));
				Profile.SizeRatings.Add(new SerializableKeyValuePair<GeocacheSize, int>(GeocacheSize.Other, int.Parse(OtherSizeValue.Text)));
				Profile.SizeRatings.Add(new SerializableKeyValuePair<GeocacheSize, int>(GeocacheSize.Regular, int.Parse(RegularValue.Text)));
				Profile.SizeRatings.Add(new SerializableKeyValuePair<GeocacheSize, int>(GeocacheSize.Small, int.Parse(SmallValue.Text)));

				Profile.DPriority = int.Parse(DPriorityValue.Text);
				Profile.DRatings = new List<SerializableKeyValuePair<float, int>>();
				Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(1f, int.Parse(D1Value.Text)));
				Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(1.5f, int.Parse(D15Value.Text)));
				Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(2f, int.Parse(D2Value.Text)));
				Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(2.5f, int.Parse(D25Value.Text)));
				Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(3f, int.Parse(D3Value.Text)));
				Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(3.5f, int.Parse(D35Value.Text)));
				Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(4f, int.Parse(D4Value.Text)));
				Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(4.5f, int.Parse(D45Value.Text)));
				Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(5f, int.Parse(D5Value.Text)));

				Profile.TPriority = int.Parse(TPriorityValue.Text);
				Profile.TRatings = new List<SerializableKeyValuePair<float, int>>();
				Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(1f, int.Parse(T1Value.Text)));
				Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(1.5f, int.Parse(T15Value.Text)));
				Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(2f, int.Parse(T2Value.Text)));
				Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(2.5f, int.Parse(T25Value.Text)));
				Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(3f, int.Parse(T3Value.Text)));
				Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(3.5f, int.Parse(T35Value.Text)));
				Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(4f, int.Parse(T4Value.Text)));
				Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(4.5f, int.Parse(T45Value.Text)));
				Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(5f, int.Parse(T5Value.Text)));

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
					MessageBox.Show("Don't you dare to divide by 0!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
			foreach (Ratingprofile BP in App.Ratingprofiles.Where(x => x.Name == Profile.Name).ToList())//Make sure only one profile with a name exists
			{
				App.Ratingprofiles.Remove(BP);
			}
			UpdateStatus("Ratingprofile saved");
			App.Ratingprofiles.Add(Profile);
			//The Dropdownmenu gets updated through an event handler
			Fileoperations.Backup(App.Ratingprofiles);
			//FIX Remove? EditRatingprofileCombobox.SelectedItem = Profile.Name; //Eventhandler takes care of same profile selected
		}
		/*
		private void CreateRoutingprofile(object sender, EventArgs e)
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
					MessageBox.Show("Please select valid Values for the Vehicle and Mode", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
			}
			catch (NullReferenceException)
			{
				MessageBox.Show("Please fill all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			catch (FormatException)
			{
				MessageBox.Show("Some fields are filled with incompatible Values", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			//Eintragen des neuen Profils
			foreach (Routingprofile BP in App.Routingprofiles.Where(x => x.Name == Profile.Name).ToList())
			{
				Profile.RoutesOfthisType = BP.RoutesOfthisType;
				App.Routingprofiles.Remove(BP);
			}
			UpdateStatus("Routingprofile saved");
			App.Routingprofiles.Add(Profile);
			//The Dropdownmenu is updated via an event handler
			Fileoperations.Backup(App.Routingprofiles);
			EditRoutingprofileCombobox.SelectedItem = Profile.Name; //Eventhandler takes care of same selection in comboboxes

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

			foreach (Routingprofile BP in App.Routingprofiles.Where(x => x.Name == Profile.Name).ToList())
			{
				App.Routingprofiles.Remove(BP);
			}

			UpdateStatus("Routingprofile deleted");
			Fileoperations.Backup(App.Routingprofiles);
		}

		private void DeleteRatingprofileButton_Click(object sender, EventArgs e)
		{
			Ratingprofile Profile = new Ratingprofile();
			if (RatingprofileName.Text == null)
			{
				MessageBox.Show("Please set Name");
				return;
			}
			Profile.Name = RatingprofileName.Text;
			foreach (Ratingprofile RP in App.Ratingprofiles.Where(x => x.Name == Profile.Name).ToList())
			{
				App.Ratingprofiles.Remove(RP);
			}

			ClearAllChildTextAndComboboxes(RatingprofilesSettingsTabelLayout);
			ClearAllChildTextAndComboboxes(SaveRatingprofileLayoutPanel);
			SelectedRatingprofileCombobox.Text = null;
			UpdateStatus("deleted ratingprofile");
			Fileoperations.Backup(App.Ratingprofiles);
		}

		private void Ratingprofile_Click(object sender, EventArgs e)
		{
			Ratingprofile SelectedRatingprofile = App.Ratingprofiles.First(x => x.Name == ((ComboBox)sender).Text);

			try
			{
				//Name des Profils
				RatingprofileName.Text = SelectedRatingprofile.Name;

				//Prioritäten
				TypePriorityValue.Text = SelectedRatingprofile.TypePriority.ToString();
				SizePriorityValue.Text = SelectedRatingprofile.SizePriority.ToString();
				DPriorityValue.Text = SelectedRatingprofile.DPriority.ToString();
				TPriorityValue.Text = SelectedRatingprofile.TPriority.ToString();

				//TypenValueungen
				TraditionalValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Traditional).Value.ToString();
				EarthcacheValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.EarthCache).Value.ToString();
				MultiValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Multi).Value.ToString();
				MysteryValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Mystery).Value.ToString();
				LetterboxValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Letterbox).Value.ToString();
				VirtualValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Virtual).Value.ToString();
				OtherTypeValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Other).Value.ToString();
				WebcamValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Webcam).Value.ToString();
				WherigoValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Wherigo).Value.ToString();

				//Größe
				LargeValue.Text = SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Large).Value.ToString();
				RegularValue.Text = SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Regular).Value.ToString();
				SmallValue.Text = SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Small).Value.ToString();
				MicroValue.Text = SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Micro).Value.ToString();
				OtherSizeValue.Text = SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Other).Value.ToString();

				//D
				D1Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 1).Value.ToString();
				D15Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 1.5).Value.ToString();
				D2Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 2).Value.ToString();
				D25Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 2.5).Value.ToString();
				D3Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 3).Value.ToString();
				D35Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 3.5).Value.ToString();
				D4Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 4).Value.ToString();
				D45Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 4.5).Value.ToString();
				D5Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 5).Value.ToString();

				//T
				T1Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 1).Value.ToString();
				T15Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 1.5).Value.ToString();
				T2Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 2).Value.ToString();
				T25Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 2.5).Value.ToString();
				T3Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 3).Value.ToString();
				T35Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 3.5).Value.ToString();
				T4Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 4).Value.ToString();
				T45Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 4.5).Value.ToString();
				T5Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 5).Value.ToString();

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
				AgeFactorValue.Text = SelectedRatingprofile.Yearfactor.ToString();
			}
			catch (Exception)
			{
				MessageBoxResult aw = MessageBox.Show("Es scheint einen Fehler in der Datei zu diesem Profil zu geben. Wollen sie es Löschen?", "Fehler", MessageBoxButton.YesNo, MessageBoxImage.Error);
				if (aw == MessageBoxResult.Yes)
				{
					App.Ratingprofiles.Remove(SelectedRatingprofile);
				}
			}
		}

		private void Routingprofile_Click(object sender, EventArgs e)
		{
			Routingprofile SelectedRoutingprofile = App.Routingprofiles.First(x => x.Name == ((ComboBox)sender).Text);

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
		}*/
		#endregion
			
		#region Map
			/*
		private void RateGeocachesButtonClick(object sender, EventArgs e)
		{
			if (SelectedRatingprofileCombobox.Text == "")
			{
				MessageBox.Show("Please select a Ratingprofile");
				return;
			}

			Ratingprofile bewertungsprofil = App.Ratingprofiles.First(x => x.Name == SelectedRatingprofileCombobox.Text);
			foreach (Geocache GC in App.Geocaches)
			{
				GC.Rate(bewertungsprofil);
			}
			App.Geocaches.OrderByDescending(x => x.Rating);
			//If it still exists..  GeocacheTable.Sort(GeocacheTable.Columns["Rating"], ListSortDirection.Descending);
			App.DB.MaximalRating = App.Geocaches[0].Rating;//Da sortierte Liste
			App.DB.MinimalRating = App.Geocaches[App.Geocaches.Count - 1].Rating;
			LoadMap();
			UpdateStatus("Geocaches rated");
			Fileoperations.Backup(App.Geocaches);
		}

		private void CreateRouteButtonClick(object sender, EventArgs e)
		{
			if (!App.RouteCalculationRunning && !App.ImportOfOSMDataRunning)
			{
				UpdateStatus("Started Route Calculation");
				App.RouteCalculationRunning = true;
				//FIX Application.UseWaitCursor = true;

				#region get values
				if (SelectedRoutingprofileCombobox.Text.Length == 0)
				{
					MessageBox.Show("No Routingprofile set.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					App.RouteCalculationRunning = false;
					App.RouteCalculationRunning = false;
					//FIX Application.UseWaitCursor = false;
					return;
				}

				Routingprofile SelectedProfile = App.Routingprofiles.First(x => x.Name == SelectedRoutingprofileCombobox.Text);

				List<Geocache> GeocachesToInclude = new List<Geocache>();
				foreach (Geocache GC in App.Geocaches.Where(x => x.ForceInclude))
				{
					GeocachesToInclude.Add(GC);
				}

				float StartLat = 0;
				float StartLon = 0;
				float EndLat = 0;
				float EndLon = 0;

				if (StartpointTextbox.Text.Length == 0)
				{
					MessageBox.Show("No Startpoint set. Please type one in or select one with right click on the map", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					App.RouteCalculationRunning = false;
					//FIX Application.UseWaitCursor = false;
					return;
				}

				if (!float.TryParse(StartpointTextbox.Text.Substring(0, StartpointTextbox.Text.IndexOf(";") - 1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out StartLat))
				{
					MessageBox.Show("Couldn't parse latitude of Startcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					App.RouteCalculationRunning = false;
					//FIX Application.UseWaitCursor = false;
					return;
				}
				if (!float.TryParse(StartpointTextbox.Text.Substring(StartpointTextbox.Text.IndexOf(";") + 1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out StartLon))
				{
					MessageBox.Show("Couldn't parse longitude Startcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					App.RouteCalculationRunning = false;
					//FIX Application.UseWaitCursor = false;
					return;
				}

				if (EndpointTextbox.Text.Length == 0)
				{
					if (MessageBox.Show("No Endpoint set. Do you want to set Startpoint as Endpoint as well?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
					{
						EndLat = StartLat;
						EndLon = StartLon;
						EndpointTextbox.Text = EndLat.ToString(CultureInfo.InvariantCulture) + ";" + EndLon.ToString(CultureInfo.InvariantCulture);
					}
					else
					{
						App.RouteCalculationRunning = false;
						//FIX Application.UseWaitCursor = false;
						return;
					}
				}
				else
				{
					if (!float.TryParse(EndpointTextbox.Text.Substring(0, EndpointTextbox.Text.IndexOf(";") - 1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out EndLat))
					{

						MessageBox.Show("Couldn't parse latitude of Endcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						App.RouteCalculationRunning = false;
						//FIX Application.UseWaitCursor = false;
						return;

					}
					if (!float.TryParse(EndpointTextbox.Text.Substring(EndpointTextbox.Text.IndexOf(";") + 1), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out EndLon))
					{
						//Big procedure only once, as the result would be the same
						MessageBox.Show("Couldn't parse longitude of Endcoordinates. Are the coordinates separated by a \";\"?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
						App.RouteCalculationRunning = false;
						//FIX Application.UseWaitCursor = false;
						return;
					}
				}

				//Check if Start and Endpoint have been selected
				if (StartLat == 0 && StartLon == 0 && EndLat == 0 && EndLon == 0)
				{
					MessageBox.Show("Please select a Startingpoint and a Finalpoint", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					App.RouteCalculationRunning = false;
					//FIX Application.UseWaitCursor = false;
					return;
				}
				#endregion

				new Thread(new ThreadStart(() =>
				{
					new Tourplanning().GetRoute_Recursive(SelectedProfile, App.Geocaches.ToList(), new Coordinate(StartLat, StartLon), new Coordinate(EndLat, EndLon), GeocachesToInclude);
				}
				)).Start();
			}
			else if (App.ImportOfOSMDataRunning)
			{
				MessageBox.Show("Please wait until the OSM Data is imported.");
			}
		}

		#region Startpoint
		private bool StartpointChanged = false;
		private void StartpointTextbox_TextChanged(object sender, EventArgs e)
		{
			StartpointChanged = true;
		}

		private void StartpointTextbox_Leave(object sender, EventArgs e)
		{
			Result<Coordinate> Coordinates = ExtractCoordinates(StartpointTextbox.Text);
			if (!Coordinates.IsError)
			{
				SetStartpoint(Coordinates.Value);
			}
		}
		#endregion

		#region Endpoint
		private bool EndpointChanged = false;
		private void EndpointTextbox_TextChanged(object sender, EventArgs e)
		{
			EndpointChanged = true;
		}

		private void EndpointTextbox_Leave(object sender, EventArgs e)
		{
			Result<Coordinate> Coordinates = ExtractCoordinates(EndpointTextbox.Text);
			if (!Coordinates.IsError)
			{
				SetEndpoint(Coordinates.Value);
			}
		}
		#endregion
		
		#region Routes
		private void DeleteButton_Click(object sender, EventArgs e, string OverlayTag)
		{
			App.Routes.Remove(App.Routes.First(x => x.Key == OverlayTag));
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
			*/
		#region MapUIEvents
		private void Map_OnMapDrag(object sender, DragEventArgs e)
		{
			App.DB.LastMapPosition = new Coordinate((float)map.Viewport.ScreenToWorld(map.Viewport.Center).X, (float)map.Viewport.ScreenToWorld(map.Viewport.Center).Y);
			Fileoperations.Backup(null);
		}

		private void Map_Enter(object sender, EventArgs e)
		{
			LoadMap();
		}

		private void Map_OnMapZoomChanged(object sender, ZoomedEventArgs e)
		{
			App.DB.LastMapPosition = new Coordinate((float)map.Viewport.ScreenToWorld(map.Viewport.Center).X, (float)map.Viewport.ScreenToWorld(map.Viewport.Center).Y);
			App.DB.LastMapResolution = map.Viewport.Resolution;
			Fileoperations.Backup(null);
		}

		/*
		private void Map_OnMarkerClick(GMapMarker item, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && item.Tag.ToString() != "Endpoint" && item.Tag.ToString() != "Startpoint")
			{
				System.Diagnostics.Process.Start("https://www.coord.info/" + item.Tag);
			}
			else if (e.Button == MouseButtons.Right && item.Tag.ToString() != "Endpoint" && item.Tag.ToString() != "Startpoint")
			{
				ContextMenu MapContextMenu = new ContextMenu();
				// initialize the commands
				MenuItem SetEndpoint = new MenuItem("Set Endpoint here");
				SetEndpoint.Click += (new_sender, new_e) => this.SetEndpoint(item.Position);
				MenuItem SetStartpoint = new MenuItem("Set Startpoint here");
				SetStartpoint.Click += (new_sender, new_e) => this.SetStartpoint(item.Position);

				Geocache geocache = App.Geocaches.First(x => x.GCCODE == item.Tag.ToString());
				MenuItem SetForceInclude = new MenuItem("ForceInclude");
				if (geocache.ForceInclude)
				{
					SetForceInclude.Checked = true;
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
				Coordinate Coordinates = Map.Viewport.ScreenToWorld(e.X, e.Y);
				// initialize the commands
				MenuItem SetEndpoint = new MenuItem("Set Endpoint here");
				SetEndpoint.Click += (new_sender, new_e) => this.SetEndpoint(Coordinates);
				MenuItem SetStartpoint = new MenuItem("Set Startpoint here");
				SetStartpoint.Click += (new_sender, new_e) => this.SetStartpoint(Coordinates);
				MapContextMenu.MenuItems.Add(SetStartpoint);
				MapContextMenu.MenuItems.Add(SetEndpoint);
				MapContextMenu.Show(Map, e.Location);
			}
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
		*/
		#endregion

		#endregion

		#region Settings
		/*
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
					AutotargetselectionMaxTextBox.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Min * 100 + 1).ToString();
				}
				else if (Value < 100 * App.DB.PercentageOfDistanceInAutoTargetselection_Min)
				{
					MessageBox.Show("Percentage can't be smaller than that of the Minimum");
					AutotargetselectionMaxTextBox.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Min * 100 + 1).ToString();
				}
				else
				{

					App.DB.PercentageOfDistanceInAutoTargetselection_Max = (Value / 100f);
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
					AutotargetselectionMinTextBox.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Max * 100 - 1).ToString();
				}
				else if (Value < 0)
				{
					MessageBox.Show("Percentage can't be smaller than 0");
					AutotargetselectionMinTextBox.Text = 0.ToString();
				}
				else if (Value > 100 * App.DB.PercentageOfDistanceInAutoTargetselection_Max * 100)
				{
					MessageBox.Show("Percentage can't be bigger than that of the Maximum");
					AutotargetselectionMinTextBox.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Max - 1).ToString();
				}
				else
				{

					App.DB.PercentageOfDistanceInAutoTargetselection_Min = (Value / 100f);
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
			App.DB.RoutefindingWidth = Value;
			Fileoperations.Backup(null);

		}
		else if (RoutefindingWidth_Textbox.Text.Length != 0)
		{
			MessageBox.Show("Enter valid integers only.");
		}
	}

	private void Autotargetselection_CheckedChanged(object sender, EventArgs e)
	{
		App.DB.Autotargetselection = Autotargetselection.Checked;
	}

	private void LiveDisplayRouteCalculationCheckbox_CheckedChanged(object sender, EventArgs e)
	{
		App.DB.DisplayLiveCalculation = LiveDisplayRouteCalculationCheckbox.Checked;
		Fileoperations.Backup(null);
	}

	private void MarkerSizeTrackBar_Scroll(object sender, EventArgs e)
	{
		App.MarkerStyleCache.Clear();
		App.DB.MarkerSize = MarkerSizeTrackBar.Value;
		LoadMap();
		Fileoperations.Backup(null);
	}*/
		#endregion
		/*
				private void Form1_FormClosing(object sender, FormClosingEventArgs e)
				{
					Fileoperations.Backup(App.Geocaches);
					Fileoperations.Backup(App.Ratingprofiles);
					Fileoperations.Backup(App.Routingprofiles);

				}
				*/
		#endregion


		#region Accessors
		public void SetRouterDBLabel(string text)
		{
			Application.Current.Dispatcher.BeginInvoke( //You never know from which thread it is called
				new Action(() =>
				{
					RouterDBStateLabel.Text = text;
				}));
		}
		
		#region Status
		public void UpdateStatus(string message, int ProgressToShow = 0)
		{
			Application.Current.Dispatcher.BeginInvoke(
				 new Action(() =>
				 {
					 File.AppendAllText("Log.txt", "[" + DateTime.Now + "]: " + message + "\n");
					 if (ProgressToShow != 0)//If the status changes while this is still processing and the new one isn't time consuming (currently there are only two time consuming methods, which cannot run simultaneously), the tooltip still chows the old information, so one can still check what happens
					 {

						 StatusProgressBar.ToolTip = new ToolTip().Content = message;
						 StatusProgressBar.Value = ProgressToShow;
					 }
					 else if (StatusProgressBar.Value == 100)//Thus the previous operation has finished
					 {
						 StatusProgressBar.Value = 0;
						 StatusProgressBar.ToolTip = new ToolTip().Content = message;
					 }
					 StatusLabel.Text = message;
				 }));
		}
		#endregion
		
		#endregion

		#region Methods
		public void UpdateSettingsTextBoxes()
		{
			/*FIX
			RoutefindingWidth_Textbox.Text = App.DB.RoutefindingWidth.ToString();
			Autotargetselection.Checked = App.DB.Autotargetselection;
			MarkerSizeTrackBar.Value = App.DB.MarkerSize;
			AutotargetselectionMinTextBox.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Min * 100).ToString();
			AutotargetselectionMaxTextBox.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Max * 100).ToString();
			LiveDisplayRouteCalculationCheckbox.Checked = App.DB.DisplayLiveCalculation;
			*/
		}

		#region Map
		/// <summary>
		/// Updates Map
		/// </summary>
		public void LoadMap()
		{
			WritableLayer GeocacheLayer = new WritableLayer
			{
				Name = "Geocaches",
				Style = null
			};

			foreach (Geocache GC in App.Geocaches)
			{
				GeocacheLayer.Add(Markers.GetGeocacheMarker(GC));
			}
			map.Layers.Add(GeocacheLayer);

			//Set Views
			if (App.DB.LastMapResolution == 0)
			{
				App.DB.LastMapResolution = 5;
			}
			map.NavigateTo(App.DB.LastMapResolution);
			map.NavigateTo(SphericalMercator.FromLonLat(App.DB.LastMapPosition.Longitude, App.DB.LastMapPosition.Latitude));
		}

		private void SetStartpoint(Coordinate coordinates)
		{
			if (map.Layers.Count(x => x.Name == "StartEnd") > 0)
			{
				WritableLayer Overlay = (WritableLayer)map.Layers.First(x => x.Name == "StartEnd");
				if (Overlay.GetFeatures().Count(x => x["Label"].ToString() == "Startpoint") >0)
				{
					Overlay.TryRemove(Overlay.GetFeatures().First(x => x["Label"].ToString() == "Startpoint"));
				}
				Overlay.Add(Markers.GetStartMarker(coordinates));
			}
			else
			{
				WritableLayer Overlay = new WritableLayer
				{
					Name = "StartEnd",
					Style = null
				};
				Overlay.Add(Markers.GetStartMarker(coordinates));
				map.Layers.Add(Overlay);
			}

			//FIX Remove? StartpointTextbox.Text = coordinates.Latitude.ToString(CultureInfo.InvariantCulture) + ";" + coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
		}

		private void SetEndpoint(Coordinate coordinates)
		{
			if (map.Layers.Count(x => x.Name == "StartEnd") > 0)
			{
				WritableLayer Overlay = (WritableLayer)map.Layers.First(x => x.Name == "StartEnd");
				if (Overlay.GetFeatures().Count(x => x["Label"].ToString() == "Endpoint") > 0)
				{
					Overlay.TryRemove(Overlay.GetFeatures().First(x => x["Label"].ToString() == "Endpoint"));
				}
				Overlay.Add(Markers.GetStartMarker(coordinates));
			}
			else
			{
				WritableLayer Overlay = new WritableLayer
				{
					Name = "StartEnd",
					Style = null
				};
				Overlay.Add(Markers.GetStartMarker(coordinates));
				map.Layers.Add(Overlay);
			}

			//FIX Remove? EndpointTextbox.Text = coordinates.Latitude.ToString(CultureInfo.InvariantCulture) + ";" + coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
		}

		/* TODO Has to be redone in some wayy when Layout is fixed
		delegate void newRouteControlElementDelegate(string OverlayTag);
		public void newRouteControlElement(string OverlayTag)
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
			Tourplanning.RouteData ThisRouteData = App.Routes.First(x => x.Key == OverlayTag).Value;
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
			RouteControl.Show();*//*
		}

		delegate void AddFinalRouteDelegate(Tourplanning.RouteData Result);
		public void AddFinalRoute(Tourplanning.RouteData Result)
		{
			while (Map.Layers.Count(x => x.Name == "PreliminaryRoute") > 0)
			{
				Map.Layers.Remove(Map.Layers.First(x => x.Name == "PreliminaryRoute"));//Remove the live displayed routes
			}

			Route FinalRoute = Result.partialRoutes[0].partialRoute;

			for (int i = 1; i < Result.partialRoutes.Count; i++)
			{
				FinalRoute = FinalRoute.Concatenate(Result.partialRoutes[i].partialRoute);
			}

			List<Geocache> GeocachesOnRoute = Result.GeocachesOnRoute();

			//Name of the route which will be used for all further referencing
			string Routetag = Result.Profile.Name + " Route " + (Result.Profile.RoutesOfthisType + 1);

			App.Routes.Add(new KeyValuePair<string, Tourplanning.RouteData>(Routetag, Result));
			List<Coordinate> LatLongPointList = new List<Coordinate>();

			foreach (Coordinate COO in FinalRoute.Shape)
			{
				LatLongPointList.Add(new Coordinate(COO.Latitude, COO.Longitude));
			}

			Result.Profile.RoutesOfthisType++;

			WritableLayer RouteLayer = new WritableLayer
			{
				Name = "StartEnd",
				Style = null
			};

			foreach (Geocache GC in GeocachesOnRoute)
			{
				RouteLayer.Add(Markers.GetGeocacheMarker(GC));
			}

			Feature Route = new Feature(LatLongPointList, Routetag); //TODO Lookup how lines work
			RouteLayer.Add(Route);

			Map.Layers.Add(RouteLayer);

			newRouteControlElement(Routetag);
			App.RouteCalculationRunning = false;
			//FIX Application.UseWaitCursor = false;
			LoadMap();
		}

		delegate void DisplayPreliminaryRouteDelegate(Route PreliminaryRoute);
		public void DisplayPreliminaryRoute(Route PreliminaryRoute)
		{
			if (App.DB.DisplayLiveCalculation && App.RouteCalculationRunning)//If the calculation is not running anymore, the thread was late and no route should be displayed
			{
				while (Map.Layers.Count(x => x.Name == "PreliminaryRoute") > 0)
				{
					Map.Layers.Remove(Map.Layers.First(x => x.Name == "PreliminaryRoute"));//Remove the live displayed routes
				}

				List<Coordinate> GMAPRoute = new List<Coordinate>();

				foreach (Coordinate COO in PreliminaryRoute.Shape)
				{
					GMAPRoute.Add(new Coordinate(COO.Latitude, COO.Longitude));
				}
				WritableLayer PreliminaryRouteLayer = new WritableLayer
				{
					Name = "PreliminaryRoute",
					Style = null
				};
				PreliminaryRouteLayer.Add(new Feature(GMAPRoute, "PreliminaryRoute")); //TODO Lookup how routes work
				Map.Layers.Add(PreliminaryRouteLayer);

				LoadMap();
			}
		}
		#endregion
		#region Lists
		/// <summary>
		/// Keeps the Dropdownmenu updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Ratingprofiles_ListChanged(object sender, ListChangedEventArgs e)
		{
			EditRatingprofileCombobox.Items.Clear();
			SelectedRatingprofileCombobox.Items.Clear();

			foreach (Ratingprofile profile in App.Ratingprofiles)
			{
				EditRatingprofileCombobox.Items.Add(profile.Name);
				SelectedRatingprofileCombobox.Items.Add(profile.Name);
			}
			RatingprofilesStateLabel.Text = App.Ratingprofiles.Count.ToString() + " Ratingprofiles loaded";
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

			foreach (Routingprofile profile in App.Routingprofiles)
			{
				EditRoutingprofileCombobox.Items.Add(profile.Name);
				SelectedRoutingprofileCombobox.Items.Add(profile.Name);
			}

			RoutingprofilesStateLabel.Text = App.Routingprofiles.Count.ToString() + " Routingprofiles loaded";
		}

		public void Geocaches_ListChanged(object sender, ListChangedEventArgs e)
		{
			GeocachesStateLabel.Text = App.Geocaches.Count.ToString() + " Geocaches loaded";
		}
		#endregion

		#endregion

		#region unspecific helpers
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
			/*FIX
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
		*/
			   /// <summary>
			   /// Sets the text of all TextBoxchildren of the specified parent to "0". Does this recursively through 3 layers.
			   /// </summary>
			   /// <param name="parent"></param>
		private void SetAllEmptyChildTextboxesToZero(DependencyObject parent)
		{
			TextBox TB;
			do
			{
				TB = ReturnFirstEmptyTextBox(parent);
				if (TB != null)
				{
					TB.Text = "0";
				}
			} while (TB != null);
		}

		private TextBox ReturnFirstEmptyTextBox(DependencyObject Control)
		{
			if (Control != null)
			{
				if (Control.GetType() == typeof(TextBox))
				{
					return (TextBox)Control;
				}
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(Control); i++)
				{
					TextBox childReturn = ReturnFirstEmptyTextBox(VisualTreeHelper.GetChild(Control, i));
					if (childReturn != null && childReturn.Text=="")
					{
						return childReturn;
					}
				}
			}
			return null;
		}

		private Result<Coordinate> ExtractCoordinates(string text)
		{
			if (text.Length == 0)
			{
				return new Result<Coordinate>("Empty Text");
			}

			text = text.Replace(',', '.');
			text = text.ToLower();

			int IndexOfLatitude = text.IndexOfAny(new char[] { 'n', 's' });
			int IndexOfLongitude = text.IndexOfAny(new char[] { 'e', 'w' });

			if ((IndexOfLatitude == -1 ^ IndexOfLongitude == -1))//Both ave to be either -1 or not -1, since the format has to be the same
			{
				MessageBox.Show("Please decide on a coordinate format");
				return new Result<Coordinate>("FormatError");
			}
			else if (IndexOfLongitude == -1)//Don't care which one, but I know coordinates are formatted with +/-
			{
				if (text.IndexOfAny(new char[] { ' ', ';' }) == -1)
				{
					MessageBox.Show("I can't split the coordinates you entered");
					return new Result<Coordinate>("FormatError");
				}

				string lat_string = text.Substring(0, text.IndexOfAny(new char[] { ' ', ';' }));
				string lon_string = text.Substring(text.IndexOfAny(new char[] { ' ', ';' }) + 1);

				string allowedChars = "01234567890.,";
				lon_string = lon_string.Where(c => allowedChars.Contains(c)).ToString();
				lat_string = lat_string.Where(c => allowedChars.Contains(c)).ToString();

				if (float.TryParse(lat_string, NumberStyles.Any, CultureInfo.InvariantCulture, out float lat_float) && float.TryParse(lon_string, NumberStyles.Any, CultureInfo.InvariantCulture, out float lon_float))
				{
					return new Result<Coordinate>(new Coordinate(lat_float, lon_float));
				}
				else
				{
					MessageBox.Show("Please check the coordinate you entered");
					return new Result<Coordinate>("FormatError");
				}
			}
			else
			{

				string lat_string = null;
				string lon_string = null;

				if (IndexOfLatitude < IndexOfLongitude)
				{
					lat_string = text.Substring(1, IndexOfLongitude - 1).Replace(" ", "").Replace(";", "");//Don't take Letters
					lon_string = text.Substring(IndexOfLongitude + 1).Replace(" ", "").Replace(";", "");
				}
				else if (IndexOfLatitude > IndexOfLongitude)
				{
					lon_string = text.Substring(1, IndexOfLatitude - 1).Replace(" ", "").Replace(";", "");//Don't take Letters
					lat_string = text.Substring(IndexOfLatitude + 1).Replace(" ", "").Replace(";", "");
				}

				if (float.TryParse(lat_string, out float lat_float) && float.TryParse(lon_string, out float lon_float))
				{
					if (text[IndexOfLatitude] == 's')
					{
						lat_float = -lat_float;
					}

					if (text[IndexOfLongitude] == 's')
					{
						lon_float = -lon_float;
					}

					return new Result<Coordinate>(new Coordinate(lat_float, lon_float));
				}
				else
				{
					MessageBox.Show("Please check the coordinate you entered");
					return new Result<Coordinate>("FormatError");
				}
			}
		}
		#endregion

		#endregion
	}
}
