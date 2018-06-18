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
	static class MapContextMenu
	{
		public static void ShowContextMenu(MapInfo mapInfo, Point Location)
		{
			if (App.DB.ActiveRoute == null)
			{
				MessageBox.Show("Please create a route before trying to add waypoints to the route","Error",MessageBoxButton.OK,MessageBoxImage.Error);
				return;
			}

			MapTooltip.HideTooltip();
			App.mainWindow.TooltipCanvas.Visibility = Visibility.Visible;//Just to make sure it is visible
			App.mainWindow.CustomMenuStackpanel.Visibility = Visibility.Visible;

			if (mapInfo.Feature != null && mapInfo.Layer.Name == Layers.GeocacheLayer)
			{
				//Aka the Geocache was already added to the route
				if (App.DB.ActiveRoute.CompleteRouteData.Waypoints.Where(x => x.Key.GetType() == typeof(Geocache)).Count(x => ((Geocache)x.Key).GCCODE == mapInfo.Feature[Markers.MarkerFields.Label].ToString()) > 0)
				{
					App.mainWindow.ToBeginning.Header = "Move " + mapInfo.Feature[Markers.MarkerFields.Label] + "to the beginning";
					App.mainWindow.ToEnd.Header = "Move " + mapInfo.Feature[Markers.MarkerFields.Label] + "to the beginning";
					App.mainWindow.Remove.Header = "Remove " + mapInfo.Feature[Markers.MarkerFields.Label] + "from the route";
					App.mainWindow.Remove.Click += (s, ev) => RemoveGeocache_Click(mapInfo.Feature[Markers.MarkerFields.Label].ToString());
				}
				else
				{
					App.mainWindow.ToBeginning.Header = "Add " + mapInfo.Feature[Markers.MarkerFields.Label] + "to the beginning";
					App.mainWindow.ToEnd.Header = "Add " + mapInfo.Feature[Markers.MarkerFields.Label] + "to the beginning";
				}
				App.mainWindow.ToBeginning.Click += (s, ev) => AddGeocacheToBeginning_Click(mapInfo.Feature[Markers.MarkerFields.Label].ToString());
				App.mainWindow.ToEnd.Click += (s, ev) => AddGeocacheToEnd_Click(mapInfo.Feature[Markers.MarkerFields.Label].ToString());
			}

			if (Location.X > App.mainWindow.mapControl.ActualWidth - 150)
			{
				Location.X = App.mainWindow.mapControl.ActualWidth - 150;
			}
			if (Location.Y > App.mainWindow.mapControl.ActualHeight)
			{
				Location.Y = App.mainWindow.mapControl.ActualHeight;
			}
			Canvas.SetLeft(App.mainWindow.CustomMenuStackpanel, Location.X);
			Canvas.SetTop(App.mainWindow.CustomMenuStackpanel, Location.Y);
		}

		public static void HideContextMenu()
		{
			App.mainWindow.CustomMenuStackpanel.Visibility = Visibility.Collapsed;
			App.mainWindow.Remove = new MenuItem();
		}

		private static void AddGeocacheToBeginning_Click(string Name)
		{
			foreach (SerializableKeyValuePair<object, RouterPoint> item in App.DB.ActiveRoute.CompleteRouteData.Waypoints.Where(x => x.Key.GetType() == typeof(Geocache)).Where(x => ((Geocache)x.Key).GCCODE == Name))
			{
				App.DB.ActiveRoute.CompleteRouteData.Waypoints.Remove(item);
			}

			App.DB.ActiveRoute.CompleteRouteData.Waypoints.Insert(0, new SerializableKeyValuePair<object, RouterPoint>(App.Geocaches.First(x => x.GCCODE == Name), null));
		}

		private static void AddGeocacheToEnd_Click(string Name)
		{
			foreach (SerializableKeyValuePair<object, RouterPoint> item in App.DB.ActiveRoute.CompleteRouteData.Waypoints.Where(x => x.Key.GetType() == typeof(Geocache)).Where(x => ((Geocache)x.Key).GCCODE == Name))
			{
				App.DB.ActiveRoute.CompleteRouteData.Waypoints.Remove(item);
			}

			App.DB.ActiveRoute.CompleteRouteData.Waypoints.Add(new SerializableKeyValuePair<object, RouterPoint>(App.Geocaches.First(x => x.GCCODE == Name), null));
		}

		private static void RemoveGeocache_Click(string Name)
		{
			foreach (SerializableKeyValuePair<object, RouterPoint> item in App.DB.ActiveRoute.CompleteRouteData.Waypoints.Where(x => x.Key.GetType() == typeof(Geocache)).Where(x => ((Geocache)x.Key).GCCODE == Name))
			{
				App.DB.ActiveRoute.CompleteRouteData.Waypoints.Remove(item);
			}
		}
	}
}
