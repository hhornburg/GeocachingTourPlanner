using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GeocachingTourPlanner.Types;
using Itinero;
using Mapsui;
using Mapsui.UI;

namespace GeocachingTourPlanner.UI
{
	public static class MapContextMenu
	{
		/// <summary>
		/// Shows context menu at the given point
		/// </summary>
		/// <param name="mapInfo"></param>
		public static void ShowContextMenu(MapInfo mapInfo)
		{
            if (mapInfo == null)
            {
                throw new ArgumentNullException(nameof(mapInfo));
            }

            if (App.DB.ActiveRoute == null)
			{
				MessageBox.Show("Please create a route before trying to add waypoints to the route", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			double X = mapInfo.ScreenPosition.X;
			double Y = mapInfo.ScreenPosition.Y;

			MapTooltip.HideTooltip();
			App.mainWindow.TooltipCanvas.Visibility = Visibility.Visible;//Just to make sure it is visible
			App.mainWindow.CustomMenuStackpanel.Visibility = Visibility.Visible;

			if (mapInfo.Feature != null && mapInfo.Layer.Name == Layers.GeocacheLayer)
			{
				//Aka the Geocache was already added to the route
				if (App.DB.ActiveRoute.CompleteRouteData.Waypoints.Where(x => x.GetType() == typeof(Geocache)).Count(x => ((Geocache)x).GCCODE == mapInfo.Feature[Markers.MarkerFields.Label].ToString()) > 0)
				{
					App.mainWindow.ToBeginning.Header = "Move " + mapInfo.Feature[Markers.MarkerFields.Label] + " to the beginning";
					App.mainWindow.ToEnd.Header = "Move " + mapInfo.Feature[Markers.MarkerFields.Label] + " to the end";
					App.mainWindow.Remove.Header = "Remove " + mapInfo.Feature[Markers.MarkerFields.Label] + " from the route";
					App.mainWindow.Remove.Visibility = Visibility.Visible;
					App.mainWindow.Remove.Click += (s, ev) => RemoveGeocache_Click(mapInfo.Feature[Markers.MarkerFields.Label].ToString());
				}
				else
				{
					App.mainWindow.ToBeginning.Header = "Add " + mapInfo.Feature[Markers.MarkerFields.Label] + " to the beginning";
					App.mainWindow.ToEnd.Header = "Add " + mapInfo.Feature[Markers.MarkerFields.Label] + " to the end";
				}
				App.mainWindow.ToBeginning.Click += (s, ev) => AddGeocacheToBeginning_Click(mapInfo.Feature[Markers.MarkerFields.Label].ToString());
				App.mainWindow.ToEnd.Click += (s, ev) => AddGeocacheToEnd_Click(mapInfo.Feature[Markers.MarkerFields.Label].ToString());
			}
			else
			{
				//If the click is not on the feature, it can't be in the route
				App.mainWindow.ToBeginning.Header = "Add Waypoint to the beginning";
				App.mainWindow.ToEnd.Header = "Add Waypoint to the end";
				App.mainWindow.ToBeginning.Click += (s, ev) => AddWaypointToBeginning_Click((float)Y, (float)X);
				App.mainWindow.ToEnd.Click += (s, ev) => AddWaypointToEnd_Click((float)Y, (float)X);
			}

			if (X > App.mainWindow.mapControl.ActualWidth - 150)
			{
				X = App.mainWindow.mapControl.ActualWidth - 150;
			}
			if (Y > App.mainWindow.mapControl.ActualHeight)
			{
				Y = App.mainWindow.mapControl.ActualHeight;
			}
			Canvas.SetLeft(App.mainWindow.CustomMenuStackpanel, X);
			Canvas.SetTop(App.mainWindow.CustomMenuStackpanel, Y);
		}

		/// <summary>
		/// Hides the context menu
		/// </summary>
		public static void HideContextMenu()
		{
			App.mainWindow.ToBeginning.RemoveRoutedEventHandlers(MenuItem.ClickEvent);
			App.mainWindow.ToEnd.RemoveRoutedEventHandlers(MenuItem.ClickEvent);
			App.mainWindow.CustomMenuStackpanel.Visibility = Visibility.Collapsed;
			App.mainWindow.Remove.Visibility = Visibility.Collapsed;
		}

		private static void AddGeocacheToBeginning_Click(string Name)
		{
			//Since you can't delete items from a list that you are iterating over
			List<Waypoint> WaypointsToDelete = new List<Waypoint>();
			foreach (Waypoint item in App.DB.ActiveRoute.CompleteRouteData.Waypoints.Where(x => x.GetType() == typeof(Geocache)).Where(x => ((Geocache)x).GCCODE == Name))
			{
				WaypointsToDelete.Add(item);
			}
			foreach (Waypoint item in WaypointsToDelete)
			{
				App.DB.ActiveRoute.CompleteRouteData.Waypoints.Remove(item);
			}

			App.DB.ActiveRoute.CompleteRouteData.Waypoints.Insert(0, App.Geocaches.First(x => x.GCCODE == Name));
			HideContextMenu();
		}

		private static void AddGeocacheToEnd_Click(string Name)
		{
			//Since you can't delete items from a list that you are iterating over
			List<Waypoint> WaypointsToDelete = new List<Waypoint>();
			foreach (Waypoint item in App.DB.ActiveRoute.CompleteRouteData.Waypoints.Where(x => x.GetType() == typeof(Geocache)).Where(x => ((Geocache)x).GCCODE == Name))
			{
				WaypointsToDelete.Add(item);
			}
			foreach (Waypoint item in WaypointsToDelete)
			{
				App.DB.ActiveRoute.CompleteRouteData.Waypoints.Remove(item);
			}

			App.DB.ActiveRoute.CompleteRouteData.Waypoints.Add(App.Geocaches.First(x => x.GCCODE == Name));
			HideContextMenu();
		}

		private static void RemoveGeocache_Click(string Name)
		{
			//Since you can't delete items from a list that you are iterating over
			List<Waypoint> WaypointsToDelete = new List<Waypoint>();
			foreach (Waypoint item in App.DB.ActiveRoute.CompleteRouteData.Waypoints.Where(x => x.GetType() == typeof(Geocache)).Where(x => ((Geocache)x).GCCODE == Name))
			{
				WaypointsToDelete.Add(item);
			}
			foreach (Waypoint item in WaypointsToDelete)
			{
				App.DB.ActiveRoute.CompleteRouteData.Waypoints.Remove(item);
			}
		}

		private static void AddWaypointToBeginning_Click(float lat, float lon)
		{
			//Since you can't delete items from a list that you are iterating over
			List<Waypoint> WaypointsToDelete = new List<Waypoint>();
			foreach (Waypoint item in App.DB.ActiveRoute.CompleteRouteData.Waypoints.Where(x => x.lat == lat && x.lon==lon))
			{
				WaypointsToDelete.Add(item);
			}
			foreach (Waypoint item in WaypointsToDelete)
			{
				App.DB.ActiveRoute.CompleteRouteData.Waypoints.Remove(item);
			}

			App.DB.ActiveRoute.CompleteRouteData.Waypoints.Insert(0, new Waypoint(lat,lon));
			HideContextMenu();
            App.mainWindow.Map_RenewWaypointLayer();
		}

		private static void AddWaypointToEnd_Click(float lat, float lon)
		{
			//Since you can't delete items from a list that you are iterating over
			List<Waypoint> WaypointsToDelete = new List<Waypoint>();
			foreach (Waypoint item in App.DB.ActiveRoute.CompleteRouteData.Waypoints.Where(x => x.lat == lat && x.lon == lon))
			{
				WaypointsToDelete.Add(item);
			}
			foreach (Waypoint item in WaypointsToDelete)
			{
				App.DB.ActiveRoute.CompleteRouteData.Waypoints.Remove(item);
			}

			App.DB.ActiveRoute.CompleteRouteData.Waypoints.Add(new Waypoint(lat, lon));
			HideContextMenu();
            App.mainWindow.Map_RenewWaypointLayer();
        }
	}
}
