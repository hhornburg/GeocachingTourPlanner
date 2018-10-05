using GeocachingTourPlanner.Types;
using Itinero;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GeocachingTourPlanner.UI
{
	/// <summary>
	/// Interaktionslogik für RouteWaypointListItem.xaml
	/// </summary>
	public partial class RouteWaypointListItem : UserControl
	{
		Waypoint waypoint = new Waypoint();
		/// <summary>
		/// Initializes a new item to be dispayed in the list of waypoints
		/// </summary>
		/// <param name="waypoint"></param>
		/// <param name="Name"></param>
		/// <param name="Description"></param>
		public RouteWaypointListItem(Waypoint waypoint, string Name, string Description)
		{
			InitializeComponent();
			WaypointName.Text = Name;
			this.Description.Text = Description;
			this.waypoint = waypoint;

			CheckClickability();
		}

		private void MoveUp_Click(object sender, RoutedEventArgs e)
		{
			int OldIndex=App.DB.ActiveRoute.CompleteRouteData.Waypoints.IndexOf(waypoint);
			App.DB.ActiveRoute.CompleteRouteData.Waypoints.RemoveAt(OldIndex);
			App.DB.ActiveRoute.CompleteRouteData.Waypoints.Insert(OldIndex-1,waypoint);
			CheckClickability();
		}

		private void MoveDown_Click(object sender, RoutedEventArgs e)
		{
			int OldIndex = App.DB.ActiveRoute.CompleteRouteData.Waypoints.IndexOf(waypoint);
			App.DB.ActiveRoute.CompleteRouteData.Waypoints.RemoveAt(OldIndex);
			App.DB.ActiveRoute.CompleteRouteData.Waypoints.Insert(OldIndex + 1, waypoint);
			CheckClickability();
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			App.DB.ActiveRoute.CompleteRouteData.Waypoints.Remove(waypoint);
            App.mainWindow.Map_RenewWaypointLayer();
		}

		private void CheckClickability()
		{
			if (App.DB.ActiveRoute.CompleteRouteData.Waypoints.IndexOf(waypoint) == 0)
			{
				MoveUp.IsEnabled = false;
			}
			else
			{
				MoveUp.IsEnabled = true;
			}

			if (App.DB.ActiveRoute.CompleteRouteData.Waypoints.IndexOf(waypoint) == App.DB.ActiveRoute.CompleteRouteData.Waypoints.Count - 1)
			{
				MoveDown.IsEnabled = false;
			}
			else
			{
				MoveDown.IsEnabled = true;
			}
		}
	}
}
