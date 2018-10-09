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
        #region UI Events
        private void Route_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectRoute_Combobox.SelectedItem == null)
            {
                if (App.DB.ActiveRoute != null)
                {
                    SelectRoute_Combobox.SelectedItem = App.DB.ActiveRoute.Name;
                }
            }
            else
            {
                App.DB.ActiveRoute = App.Routes.First(x => x.Name == SelectRoute_Combobox.SelectedItem.ToString());
                if (App.DB.ActiveRoute.CompleteRouteData.Profile != null)
                {
                    App.DB.ActiveRoutingprofile = App.DB.ActiveRoute.CompleteRouteData.Profile;
                }
                if (App.DB.ActiveRoutingprofile != null)
                {
                    SetRoutingprofile(App.DB.ActiveRoutingprofile);
                }
                Waypoints_ListChanged(null, null);
            }
        }

        private void CalculateDirectRoute_Click(object sender, RoutedEventArgs e)
        {
            if (App.DB.ActiveRoute.CompleteRouteData.Profile == null)
            {
                MessageBox.Show("Please select a routingprofile");
                return;
            }
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
            mapControl.Map.Layers.Remove(mapControl.Map.Layers.First(x => x.Name == "Route:" + App.DB.ActiveRoute.Name));
        }

        private void NewRouteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!App.DB.IsFilepathSet(Databases.Routes))
            {
                new DatabaseFileDialog(Databases.Routes).ShowDialog();
            }
            if (App.DB.IsFilepathSet(Databases.Routes))
            {
                NewRouteNameTextBox.Visibility = Visibility.Visible;
                AddRouteButton.Visibility = Visibility.Visible;
            }
        }

        private void AddRouteButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Routes.Count(x => x.Name == NewRouteNameTextBox.Text) > 0)
            {
                MessageBox.Show("A Route with this name already exists. please select a different name.");
            }
            else
            {
                RoutePlanner NewRoute = new RoutePlanner(NewRouteNameTextBox.Text);
                App.Routes.Add(NewRoute);
                App.DB.ActiveRoute = NewRoute;
                if (App.DB.ActiveRoute == null)
                {
                    return;
                }
                SelectRoute_Combobox.SelectedItem = NewRouteNameTextBox.Text;

                NewRouteNameTextBox.Text = "";
                NewRouteNameTextBox.Visibility = Visibility.Collapsed;
                AddRouteButton.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region List changed Events
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

            RoutesStateLabel.Text = App.Routingprofiles.Count.ToString() + " Routes loaded";
        }
        
        /// <summary>
        /// Makes sure the List of Waypoints is kept synched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Waypoints_ListChanged(object sender, EventArgs e)
        {
            WaypointStackpanel.Children.Clear();
            if (App.DB.ActiveRoute == null)
            {
                return; //Occurs during Startup
            }
            foreach (Waypoint Item in App.DB.ActiveRoute.CompleteRouteData.Waypoints)
            {
                string Name = "Waypoint";
                string Description = "";
                if (Item.GetType() == typeof(Geocache))
                {
                    Name = ((Geocache)Item).Name;
                    Description = "Type: " + ((Geocache)Item).Type + "Size: " + ((Geocache)Item).Size + "\nD: " + ((Geocache)Item).DRating + "T: " + ((Geocache)Item).TRating + "Points: " + ((Geocache)Item).Rating;
                }
                WaypointStackpanel.Children.Add(new RouteWaypointListItem(Item, Name, Description));
            }
        }

        /// <summary>
        /// Renews the Information displayed to the user
        /// </summary>
        public void RenewRouteInfo(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Properties.General.Length);
            stringBuilder.Append((App.DB.ActiveRoute.CompleteRouteData.TotalDistance / 1000).ToString("F"));
            stringBuilder.AppendLine("km");
            stringBuilder.Append(Properties.General.TimeNeeded);
            stringBuilder.Append((App.DB.ActiveRoute.CompleteRouteData.TotalTime / 60).ToString("F0"));
            stringBuilder.AppendLine("min");
            stringBuilder.Append(Properties.General.TotalPoints);
            stringBuilder.Append(App.DB.ActiveRoute.CompleteRouteData.TotalPoints.ToString("F0"));
            RouteInfo.Text = stringBuilder.ToString();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Selects the specified Route 
        /// </summary>
        /// <param name="RP"></param>
        public void SetRoute(RoutePlanner RP)
        {
            if (SelectRoute_Combobox.Items.Contains(RP.Name))
            {
                SelectRoute_Combobox.SelectedItem = RP.Name;
            }
        }
        #endregion
    }
}
