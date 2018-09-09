using GeocachingTourPlanner.IO;
using GeocachingTourPlanner.Routing;
using GeocachingTourPlanner.Types;
using Itinero;
using Itinero.LocalGeo;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.UI;
using Mapsui.UI.Wpf;
using Mapsui.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace GeocachingTourPlanner.UI
{

	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
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
			mapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
			TooltipCanvas.Visibility = Visibility.Collapsed;
		}

		#region Overview
		#region UI Events
		private void OpenWikiButton_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("https://github.com/pingurus/GeocachingTourPlanner/wiki");
		}

		private void ImportOSMDataButton_Click(object sender, RoutedEventArgs e)
		{
			Fileoperations.ImportOSMData();
		}

		private void ImportGeocachesButton_Click(object sender, RoutedEventArgs e)
		{
			Fileoperations.ImportGeocaches();
			Map_RenewGeocacheLayer();
		}

		private void setGeocachedatabaseButton_Click(object sender, RoutedEventArgs e)
		{
			if (App.DB.OpenExistingDBFile(Databases.Geocaches))
			{
				Fileoperations.ReadGeocaches();
				Map_RenewGeocacheLayer();
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
		#region Accessors
		public void SetRouterDBLabel(string text)
		{
			Application.Current.Dispatcher.BeginInvoke( //You never know from which thread it is called
				new Action(() =>
				{
					RouterDBStateLabel.Text = text;
				}));
		}

		public void Geocaches_ListChanged(object sender, ListChangedEventArgs e)
		{
            Application.Current.Dispatcher.BeginInvoke( //You never know from which thread it is called
                   new Action(() =>
                   {
                       GeocachesStateLabel.Text = App.Geocaches.Count.ToString() + " Geocaches loaded";
                       Map_RenewGeocacheLayer();
                   }));
		}
		#endregion
		#endregion

		#region Ratingprofiles
		#region Events
		private void RatingprofileSaveOnly_Click(object sender, RoutedEventArgs e)
		{
			CreateRatingprofile();
		}

		private void RatingprofileSaveApply_Click(object sender, RoutedEventArgs e)
		{
			CreateRatingprofile();
			RateGeocaches();
		}

		private void DeleteRatingprofileButton_Click(object sender, RoutedEventArgs e)
		{
			foreach (Ratingprofile RP in App.Ratingprofiles.Where(x => x.Name == App.DB.ActiveRatingprofile.Name).ToList())
			{
				App.Ratingprofiles.Remove(RP);
			}

			ClearAllChildTextboxes(RatingprofilesSettingsGrid);
			UpdateStatus("deleted ratingprofile");
		}

		private void Ratingprofile_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (EditRatingprofileCombobox.SelectedItem != null)
			{
				Ratingprofile SelectedRatingprofile = App.Ratingprofiles.First(x => x.Name == EditRatingprofileCombobox.SelectedItem.ToString());
				App.DB.ActiveRatingprofile = SelectedRatingprofile;
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
				catch (Exception ex)
				{
					MessageBoxResult aw = MessageBox.Show("There seems to be an error in this file. Do you want to delete it?", "Fehler", MessageBoxButton.YesNo, MessageBoxImage.Error);
					if (aw == MessageBoxResult.Yes)
					{
						App.Ratingprofiles.Remove(SelectedRatingprofile);
					}
				}
			}
			else
			{
				App.DB.ActiveRatingprofile = null;
			}
		}

		/// <summary>
		/// Keeps the Dropdownmenu updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Ratingprofiles_ListChanged(object sender, ListChangedEventArgs e)
		{
			EditRatingprofileCombobox.Items.Clear();

			foreach (Ratingprofile profile in App.Ratingprofiles)
			{
				EditRatingprofileCombobox.Items.Add(profile.Name);
			}
			RatingprofilesStateLabel.Text = App.Ratingprofiles.Count.ToString() + " Ratingprofiles loaded";
		}

		#endregion
		#region Methods
		private void CreateRatingprofile()
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

				if (AgeValue.SelectedItem.ToString().ToLower().Contains("multiply"))
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
            App.DB.ActiveRatingprofile = Profile;
			EditRatingprofileCombobox.SelectedItem = Profile.Name;
		}

		private bool RateGeocaches()
		{
			Ratingprofile ratingprofile;

			if (App.DB.ActiveRatingprofile != null)
			{
				ratingprofile = App.DB.ActiveRatingprofile;
			}
			else
			{
				MessageBox.Show("Please select a Ratingprofile");
				return false;
			}
			foreach (Geocache GC in App.Geocaches)
			{
				GC.Rate(ratingprofile);
			}
			App.Geocaches = new SortableBindingList<Geocache>( App.Geocaches.OrderByDescending(x => x.Rating).ToList());
			Startup.BindLists();//Since bindiing is lost when new list is created
			App.DB.MaximalRating = App.Geocaches[0].Rating;//Da sortierte Liste
			App.DB.MinimalRating = App.Geocaches[App.Geocaches.Count - 1].Rating;
			Map_RenewGeocacheLayer();
			UpdateStatus("Geocaches rated");
			Fileoperations.Backup(Databases.Geocaches);
			return true;
		}

		/// <summary>
		/// Sets the specified Ratingprofile 
		/// </summary>
		/// <param name="RP"></param>
		public void SetRatingprofile(Ratingprofile RP)
		{
			if(EditRatingprofileCombobox.Items.Cast<ComboBoxItem>().Count(x => x.Content.ToString() == RP.Name) > 0)
			{
				EditRatingprofileCombobox.SelectedItem = EditRatingprofileCombobox.Items.Cast<ComboBoxItem>().First(x => x.Content.ToString() == RP.Name);
			}
		}
		#endregion
		#endregion

		#region Routingprofiles
		#region Events
		private void RoutingprofilesSaveOnly_Click(object sender, RoutedEventArgs e)
		{
			CreateRoutingprofile();
		}

		private void RoutingprofilesSaveRecaculate_Click(object sender, RoutedEventArgs e)
		{
			CreateRoutingprofile();
			//TODO recalculate
		}

		private void Routingprofile_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Routingprofile SelectedRoutingprofile = App.Routingprofiles.First(x => x.Name == EditRoutingprofileCombobox.SelectedItem.ToString());
			App.DB.ActiveRoutingprofile = SelectedRoutingprofile;

			try
			{
				RoutingprofileName.Text = SelectedRoutingprofile.Name;

				//Distance
				DistanceValue.Text = SelectedRoutingprofile.MaxDistance.ToString();

				//Time
				TimeValue.Text = SelectedRoutingprofile.MaxTime.ToString();
				GeocacheTimeValue.Text = SelectedRoutingprofile.TimePerGeocache.ToString();

				//Profile

				//Workaround Issue #161 @ Itinero
				if (SelectedRoutingprofile.ItineroProfile.profile.FullName.Contains("."))
				{
					VehicleValue.SelectedItem = SelectedRoutingprofile.ItineroProfile.profile.FullName.Remove(SelectedRoutingprofile.ItineroProfile.profile.FullName.IndexOf("."));//gets the parent of the profile (thus the vehicle)

				}
				else
				{
					VehicleValue.SelectedItem = SelectedRoutingprofile.ItineroProfile.profile.FullName;
				}
				switch (SelectedRoutingprofile.ItineroProfile.profile.Metric)
				{
					case Itinero.Profiles.ProfileMetric.DistanceInMeters:

						MetricValue.SelectedItem = "Shortest";
						break;

					case Itinero.Profiles.ProfileMetric.TimeInSeconds:
						MetricValue.SelectedItem = "Fastest";
						break;
				}
			}
			catch (NullReferenceException)
			{
				MessageBox.Show("Couldn't load the complete profile.", "Warning");
			}
		}

		private void DeleteRoutingprofileButton_Click(object sender, RoutedEventArgs e)
		{
			Routingprofile Profile = new Routingprofile();
			if (RoutingprofileName.Text == null)
			{
				MessageBox.Show("Please set Name");
				return;
			}
			Profile.Name = RoutingprofileName.Text;

			ClearAllChildTextboxes(RoutingprofilesSettingsGrid);

			foreach (Routingprofile BP in App.Routingprofiles.Where(x => x.Name == Profile.Name).ToList())
			{
				App.Routingprofiles.Remove(BP);
			}

			UpdateStatus("Routingprofile deleted");
		}

		/// <summary>
		/// keeps the Comboboxes updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Routingprofiles_ListChanged(object sender, ListChangedEventArgs e)
		{
			EditRoutingprofileCombobox.Items.Clear();
			SelectRoutingprofileCombobox.Items.Clear();

			foreach (Routingprofile profile in App.Routingprofiles)
			{
				EditRoutingprofileCombobox.Items.Add(profile.Name);
				SelectRoutingprofileCombobox.Items.Add(profile.Name);
			}

			RoutingprofilesStateLabel.Text = App.Routingprofiles.Count.ToString() + " Routingprofiles loaded";
		}


		/// <summary>
		/// Selects the specified Routingprofile in the Combobox 
		/// </summary>
		/// <param name="RP"></param>
		public void SetRoutingprofile(Routingprofile RP)
		{
			if (EditRoutingprofileCombobox.Items.Cast<ComboBoxItem>().Count(x => x.Content.ToString() == RP.Name) > 0)
			{
				EditRoutingprofileCombobox.SelectedItem = EditRoutingprofileCombobox.Items.Cast<ComboBoxItem>().First(x => x.Content.ToString() == RP.Name);
			}
		}
		#endregion
		#region Methods
		private void CreateRoutingprofile()
		{
			SetAllEmptyChildTextboxesToZero(RoutingprofilesSettingsGrid);

			Routingprofile Profile = new Routingprofile();
			if (RoutingprofileName.Text == null)
			{
				MessageBox.Show("Please set Name");
				return;
			}
			try
			{
				Profile.Name = RoutingprofileName.Text;

				Profile.MaxDistance = int.Parse(DistanceValue.Text);

				Profile.MaxTime = int.Parse(TimeValue.Text);
				Profile.TimePerGeocache = int.Parse(GeocacheTimeValue.Text);

				Profile.ItineroProfile = new SerializableItineroProfile(VehicleValue.Text, MetricValue.Text);
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
			EditRoutingprofileCombobox.SelectedItem = Profile.Name; //Eventhandler takes care of same selection in comboboxes

		}
		#endregion
		#endregion

		#region Settings
		#region UI Events
		#region Autotargetselection Max
		bool AutotargetselectionMax_TextChanged = false;
		private void AutotargetselectionMaxTextbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			AutotargetselectionMax_TextChanged = true;

		}

		private void AutotargetselectionMaxTextBox_Leave(object sender, RoutedEventArgs e)
		{
			if (AutotargetselectionMax_TextChanged)
			{
				if (int.TryParse(AutotargetselectionMaxValue.Text, out int Value))
				{
					if (Value > 100)
					{
						MessageBox.Show("Percentage can't be bigger than 100");
						AutotargetselectionMaxValue.Text = 100.ToString();
					}
					else if (Value < 0)
					{
						MessageBox.Show("Percentage can't be smaller than 0");
						AutotargetselectionMaxValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Min * 100 + 1).ToString();
					}
					else if (Value < 100 * App.DB.PercentageOfDistanceInAutoTargetselection_Min)
					{
						MessageBox.Show("Percentage can't be smaller than that of the Minimum");
						AutotargetselectionMaxValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Min * 100 + 1).ToString();
					}
					else
					{

						App.DB.PercentageOfDistanceInAutoTargetselection_Max = (Value / 100f);
					}
				}
				else if (AutotargetselectionMaxValue.Text.Length != 0)
				{
					MessageBox.Show("Enter valid integers only.");
				}
			}
			AutotargetselectionMax_TextChanged = false;
		}
		#endregion

		#region Autotargetselection Min
		bool AutotargetselectionMin_TextChanged = false;
		private void AutotargetselectionMinTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			AutotargetselectionMin_TextChanged = true;
		}

		private void AutotargetselectionMinTextBox_Leave(object sender, RoutedEventArgs e)
		{
			if (AutotargetselectionMin_TextChanged)
			{
				if (int.TryParse(AutotargetselectionMinValue.Text, out int Value))
				{
					if (Value > 100)
					{
						MessageBox.Show("Percentage can't be bigger than 100");
						AutotargetselectionMinValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Max * 100 - 1).ToString();
					}
					else if (Value < 0)
					{
						MessageBox.Show("Percentage can't be smaller than 0");
						AutotargetselectionMinValue.Text = 0.ToString();
					}
					else if (Value > 100 * App.DB.PercentageOfDistanceInAutoTargetselection_Max * 100)
					{
						MessageBox.Show("Percentage can't be bigger than that of the Maximum");
						AutotargetselectionMinValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Max - 1).ToString();
					}
					else
					{

						App.DB.PercentageOfDistanceInAutoTargetselection_Min = (Value / 100f);
					}
				}
				else if (AutotargetselectionMinValue.Text.Length != 0)
				{
					MessageBox.Show("Enter valid integers only.");
				}
			}
			AutotargetselectionMin_TextChanged = false;
		}
		#endregion

		private void RoutefindingWidth_Textbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (int.TryParse(RouteFindingWidthValue.Text, out int Value))
			{
				App.DB.RoutefindingWidth = Value;
			}
			else if (RouteFindingWidthValue.Text.Length != 0)
			{
				MessageBox.Show("Enter valid integers only.");
			}
		}
		
		private void LiveDisplayRouteCalculationCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			App.DB.DisplayLiveCalculation = true;
		}
		private void LiveDisplayRouteCalculationCheckbox_Unchecked(object sender, RoutedEventArgs e)
		{
			App.DB.DisplayLiveCalculation = false;
		}

		private void MarkerSizeValue_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			App.MarkerStyleCache.Clear();
			App.DB.MarkerSize = (int)MarkerSizeValue.Value;
			Map_RenewGeocacheLayer();
		}
		#endregion
		#region Methods
		/// <summary>
		/// Reads the Settings from the DB and writes them to the according textboxes
		/// </summary>
		public void UpdateSettingsTextBoxes()
		{
			RouteFindingWidthValue.Text = App.DB.RoutefindingWidth.ToString();
			MarkerSizeValue.Value = App.DB.MarkerSize;
			AutotargetselectionMinValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Min * 100).ToString();
			AutotargetselectionMaxValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Max * 100).ToString();
			LiveDisplayRouteCalculationValue.IsChecked = App.DB.DisplayLiveCalculation;
		}
		#endregion
		#endregion

		#region Status
		/// <summary>
		/// Updates the Statusbar
		/// </summary>
		/// <param name="message"></param>
		/// <param name="ProgressToShow"></param>
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

		#region Map
		#region custom tooltip
		
		#endregion
		#region UI Events
		private void Map_OnMapDrag(object sender, DragEventArgs e)
		{
			App.DB.LastMapPosition = new Coordinate((float)mapControl.Viewport.ScreenToWorld(mapControl.Viewport.Center).X, (float)mapControl.Viewport.ScreenToWorld(mapControl.Viewport.Center).Y);
		}

		private void Map_Enter(object sender, EventArgs e)
		{
			Map_RenewGeocacheLayer();
		}

		private void Map_OnMapZoomChanged(object sender, ZoomedEventArgs e)
		{
			App.DB.LastMapPosition = new Coordinate((float)mapControl.Viewport.ScreenToWorld(mapControl.Viewport.Center).X, (float)mapControl.Viewport.ScreenToWorld(mapControl.Viewport.Center).Y);
			App.DB.LastMapResolution = mapControl.Viewport.Resolution;
		}

		private void mapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            MapContextMenu.HideContextMenu();
            MapTooltip.HideTooltip();

            MapInfo mapInfo = GetMapInfo(e);
			if (mapInfo!=null&&mapInfo.Layer!=null && mapInfo.Layer.Name == Layers.GeocacheLayer)
			{
				Process.Start("http://coord.info/" + mapInfo.Feature[Markers.MarkerFields.Label]);
			}
			e.Handled = true;
		}

		private void mapControl_MouseMove(object sender, MouseEventArgs e)
		{
            MapInfo mapInfo = GetMapInfo(e);
            if (mapInfo != null && mapInfo.Feature != null)
			{
				MapTooltip.ShowTooltip((string)mapInfo.Feature[Markers.MarkerFields.TooltipText], new Point(mapInfo.ScreenPosition.X, mapInfo.ScreenPosition.Y));
			}
		}

		private void mapControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
            MapInfo mapInfo = GetMapInfo(e);
            if (mapInfo != null)
            {
                MapContextMenu.ShowContextMenu(mapInfo);
            }
		}

        private MapInfo GetMapInfo(MouseEventArgs e)
        {
            try
            {
                Mapsui.Geometries.Point Location = e.GetPosition(mapControl).ToMapsui();
                return mapControl.GetMapInfo(Location);
            }
            catch (Exception)
            {
                return null;
            }
        }
		#endregion

		#region Methods

		/// <summary>
		/// Updates Map
		/// </summary>
		public void Map_RenewGeocacheLayer()
		{
			if (mapControl != null)//Occurs during startup
			{
                //GeocacheLayer
				if (mapControl.Map.Layers.Count(x => x.Name == Layers.GeocacheLayer) > 0)
				{
					foreach (WritableLayer GClayer in mapControl.Map.Layers.Where(x => x.Name == Layers.GeocacheLayer).ToList())
					{
						mapControl.Map.Layers.Remove(GClayer);
					}
				}
				WritableLayer GeocacheLayer = new WritableLayer
				{
					Name = Layers.GeocacheLayer,
					Style = null
				};

				foreach (Geocache GC in App.Geocaches)
				{
					GeocacheLayer.Add(Markers.GetGeocacheMarker(GC));
				}
                GeocacheLayer.IsMapInfoLayer = true;
				mapControl.Map.Layers.Add(GeocacheLayer);

                //Set Views
                if (App.DB.LastMapResolution == 0)
                {
                    App.DB.LastMapResolution = 5;
                }
                mapControl.Refresh();
            }
		}
        
        /// <summary>
        /// Updates Waypoints shown on Map
        /// </summary>
        public void Map_RenewWaypointLayer()
        {
            //Waypointlayer
            if (mapControl.Map.Layers.Count(x => x.Name == Layers.WaypointLayer) > 0)
            {
                foreach (WritableLayer WPLayer in mapControl.Map.Layers.Where(x => x.Name == Layers.WaypointLayer).ToList())
                {
                    mapControl.Map.Layers.Remove(WPLayer);
                }
            }
            WritableLayer Waypointlayer = new WritableLayer
            {
                Name = Layers.WaypointLayer,
                Style = null
            };

            foreach (Waypoint WP in App.DB.ActiveRoute.CompleteRouteData.Waypoints)
            {
                if (WP.GetType() != typeof(Geocache))
                {
                    Waypointlayer.Add(Markers.GetWaypointMarker(WP));
                }
            }
            Waypointlayer.IsMapInfoLayer = true;
            mapControl.Map.Layers.Add(Waypointlayer);

            //Set Views
            if (App.DB.LastMapResolution == 0)
            {
                App.DB.LastMapResolution = 5;
            }
            mapControl.Refresh();
        }

        public void Map_NavigateToLastVisited()
		{
			mapControl.Navigator.NavigateTo(App.DB.LastMapResolution);
			mapControl.Navigator.NavigateTo(SphericalMercator.FromLonLat(App.DB.LastMapPosition.Longitude, App.DB.LastMapPosition.Latitude));
		}

        #endregion
        #endregion

        #region Routes
        #region UI Events
        private void CalculateDirectRoute_Click(object sender, RoutedEventArgs e)
        {
            if (SelectRoutingprofileCombobox.SelectedItem == null)
            {
                MessageBox.Show("Please select a routingprofile");
                return;
            }
            App.DB.ActiveRoute.CompleteRouteData.Profile = App.Routingprofiles.First(x=>x.Name==SelectRoutingprofileCombobox.SelectedItem.ToString());
            App.DB.ActiveRoute.CalculateDirectRoute();
        }
        private void AddgeocachesDirectlyOnRoute_Click(object sender, RoutedEventArgs e)
        {
            App.DB.ActiveRoute.AddGeocachesDirectlyOnRoute();
        }
        private void AddGeocachesCloseToRoute_Click(object sender, RoutedEventArgs e)
        {

        }
        private void FindBestGeocachesToRoute_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportRoute_Click(object sender, RoutedEventArgs e)
        {
            Fileoperations.ExportGPX(App.DB.ActiveRoute);
        }

        private void DeleteRoute_Click(object sender, RoutedEventArgs e)
        {
            App.Routes.Remove(App.DB.ActiveRoute);
            mapControl.Map.Layers.Remove(mapControl.Map.Layers.First(x => x.Name == "Route:"+App.DB.ActiveRoute.Name));
        }
        #endregion
        #region Events
        /// <summary>
        /// keeps the Comboboxes updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Routes_ListChanged(object sender, ListChangedEventArgs e)
		{
			SelectRoute_Combobox.Items.Clear();

			foreach (RoutePlanner profile in App.Routes)
			{
				SelectRoute_Combobox.Items.Add(profile.Name);
			}
		}

		private void NewButton_Click(object sender, RoutedEventArgs e)
		{
			NewRouteWindow newRouteWindow = new NewRouteWindow();
			newRouteWindow.ShowDialog();
			App.DB.ActiveRoute = new Routing.RoutePlanner(newRouteWindow.Name);
			if (App.DB.ActiveRoute == null)
			{
				return;
			}
			App.Routes.Add(App.DB.ActiveRoute);
		}

		public void Waypoints_ListChanged(object sender, ListChangedEventArgs e)
		{
			WaypointStackpanel.Children.Clear();
            if (App.DB.ActiveRoute == null)
            {
                return; //Occurs during Startup
            }
			foreach(Waypoint Item in App.DB.ActiveRoute.CompleteRouteData.Waypoints)
			{
				string Name = "Waypoint";
				string Description = "";
				if (Item.GetType() == typeof(Geocache))
				{
					Name = ((Geocache)Item).Name;
					Description = "Type: " + ((Geocache)Item).Type + "Size: " + ((Geocache)Item).Size + "\nD: " + ((Geocache)Item).DRating + "T: " + ((Geocache)Item).TRating+ "Points: " + ((Geocache)Item).Rating;
				}
				WaypointStackpanel.Children.Add(new RouteWaypointListItem(Item, Name, Description));
			}
		}

		/// <summary>
		/// Selects the specified Route 
		/// </summary>
		/// <param name="RP"></param>
		public void SetRoute(RoutePlanner RP)
		{
			if (SelectRoute_Combobox.Items.Cast<ComboBoxItem>().Count(x => x.Content.ToString() == RP.Name) > 0)
			{
				SelectRoute_Combobox.SelectedItem = SelectRoute_Combobox.Items.Cast<ComboBoxItem>().First(x => x.Content.ToString() == RP.Name);
			}
		}
		#endregion
		#endregion

		#region unspecific helpers
		/// <summary>
		/// Sets the text of all TextBoxchildren of the specified parent to "0"
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
					if (childReturn != null && childReturn.Text == "")
					{
						return childReturn;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Empties all textboxes child textboxes
		/// </summary>
		/// <param name="parent"></param>
		private void ClearAllChildTextboxes(DependencyObject parent)
		{
			TextBox TB;
			do
			{
				TB = ReturnFirstNotEmptyTextBox(parent);
				if (TB != null)
				{
					TB.Text = "";
				}
			} while (TB != null);
		}

		private TextBox ReturnFirstNotEmptyTextBox(DependencyObject Control)
		{
			if (Control != null)
			{
				if (Control.GetType() == typeof(TextBox))
				{
					return (TextBox)Control;
				}
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(Control); i++)
				{
					TextBox childReturn = ReturnFirstNotEmptyTextBox(VisualTreeHelper.GetChild(Control, i));
					if (childReturn != null && childReturn.Text != "")
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

        
    }
    /// <summary>
    /// Quasi enum for layer names
    /// </summary>
    public static class Layers
	{
		public static readonly string GeocacheLayer = "Geocaches";
		public static readonly string WaypointLayer = "Waypoints";
	}
}
