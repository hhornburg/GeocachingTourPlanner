﻿using GeocachingTourPlanner.IO;
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
                Waypoints_ListChanged(null, null);
            }
        }

        private void CalculateDirectRoute_Click(object sender, RoutedEventArgs e)
        {
            if (SelectRoutingprofileCombobox.SelectedItem == null)
            {
                MessageBox.Show("Please select a routingprofile");
                return;
            }
            App.DB.ActiveRoute.CompleteRouteData.Profile = App.Routingprofiles.First(x => x.Name == SelectRoutingprofileCombobox.SelectedItem.ToString());
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

        private void NewRouteButton_Click(object sender, RoutedEventArgs e)
        {
            NewRouteNameTextBox.Visibility = Visibility.Visible;
            AddRouteButton.Visibility = Visibility.Visible;
        }

        private void AddRouteButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Routes.Count(x => x.Name == NewRouteNameTextBox.Text) > 0)
            {
                MessageBox.Show("A Route with this name already exists. please select a different name.");
            }
            else
            {

                App.DB.ActiveRoute = new RoutePlanner(NewRouteNameTextBox.Text);
                if (App.DB.ActiveRoute == null)
                {
                    return;
                }
                App.Routes.Add(App.DB.ActiveRoute);
                SelectRoute_Combobox.SelectedItem = NewRouteNameTextBox.Text;

                NewRouteNameTextBox.Text = "";
                NewRouteNameTextBox.Visibility = Visibility.Collapsed;
                AddRouteButton.Visibility = Visibility.Collapsed;
            }
        }

        public void Waypoints_ListChanged(object sender, ListChangedEventArgs e)
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
    }
}
