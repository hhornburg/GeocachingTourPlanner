using GeocachingTourPlanner.IO;
using GeocachingTourPlanner.Routing;
using GeocachingTourPlanner.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void Routingprofile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Routingprofile SelectedRoutingprofile = null;

            if (EditRoutingprofileCombobox.SelectedItem == null)
            {
                if (App.DB.ActiveRoutingprofile != null)
                {
                    EditRoutingprofileCombobox.SelectedItem = App.DB.ActiveRoutingprofile.Name;
                }
            }
            else
            {
                SelectedRoutingprofile = App.Routingprofiles.First(x => x.Name == EditRoutingprofileCombobox.SelectedItem.ToString());
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
    }
}
