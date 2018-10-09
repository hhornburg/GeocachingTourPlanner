using GeocachingTourPlanner.Types;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GeocachingTourPlanner.UI
{
    public partial class MainWindow : Window
    {
        #region Events
        private void RoutingprofilesSaveOnly_Click(object sender, RoutedEventArgs e)
        {
            CreateRoutingprofile();
        }

        private void RoutingprofilesSaveRecaculate_Click(object sender, RoutedEventArgs e)
        {
            CreateRoutingprofile();
            App.DB.ActiveRoute.CalculateDirectRoute();
        }

        private void EditRoutingprofileCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetRoutingprofile(App.Routingprofiles.First(x => x.Name == EditRoutingprofileCombobox.SelectedItem.ToString()));
        }

        private void SelectRoutingprofileCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetRoutingprofile(App.Routingprofiles.First(x => x.Name == SelectRoutingprofileCombobox.SelectedItem.ToString()));
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

        /// <summary>
        /// Selects the specified Routingprofile in the Comboboxes
        /// </summary>
        /// <param name="SelectedRoutingprofile"></param>
        public void SetRoutingprofile(Routingprofile SelectedRoutingprofile)
        {
            App.DB.ActiveRoutingprofile = SelectedRoutingprofile;
            if (App.DB.ActiveRoute != null)
            {
                App.DB.ActiveRoute.CompleteRouteData.Profile = SelectedRoutingprofile;
            }
            if (SelectRoutingprofileCombobox.Items.Contains(SelectedRoutingprofile.Name))
            {
                SelectRoutingprofileCombobox.SelectedItem = SelectedRoutingprofile.Name;
            }
            if (EditRoutingprofileCombobox.Items.Contains(SelectedRoutingprofile.Name))
            {
                EditRoutingprofileCombobox.SelectedItem = SelectedRoutingprofile.Name;
            }

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
        #endregion
    }
}
