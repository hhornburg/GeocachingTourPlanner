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
		/// Initializes a new item to be dispayed in the list of Waypoints
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
            App.DB.ActiveRoute.CompleteRouteData.MoveWaypointUp(waypoint);
            CheckClickability();
		}

		private void MoveDown_Click(object sender, RoutedEventArgs e)
		{
            App.DB.ActiveRoute.CompleteRouteData.MoveWaypointDown(waypoint);
			CheckClickability();
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			App.DB.ActiveRoute.CompleteRouteData.RemoveWaypoint(waypoint);
            App.mainWindow.Map_RenewWaypointLayer();
		}

		private void CheckClickability()
		{
			if (App.DB.ActiveRoute.CompleteRouteData.IndexOfWaypoint(waypoint) == 0)
			{
				MoveUp.IsEnabled = false;
			}
			else
			{
				MoveUp.IsEnabled = true;
			}

			if (App.DB.ActiveRoute.CompleteRouteData.IndexOfWaypoint(waypoint) == App.DB.ActiveRoute.CompleteRouteData.Waypoints.Count - 1)
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
