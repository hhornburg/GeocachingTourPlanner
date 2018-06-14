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
		SerializableKeyValuePair<Object, RouterPoint> Item = new SerializableKeyValuePair<Object, RouterPoint>();
		/// <summary>
		/// Initializes a new item to be dispayed in the list of waypoints
		/// </summary>
		/// <param name="Item"></param>
		/// <param name="Name"></param>
		/// <param name="Description"></param>
		public RouteWaypointListItem(SerializableKeyValuePair<Object,RouterPoint> Item, string Name, string Description)
		{
			InitializeComponent();
			WaypointName.Text = Name;
			this.Description.Text = Description;
			this.Item = Item;
		}

		private void MoveUp_Click(object sender, RoutedEventArgs e)
		{
			int OldIndex=App.ActiveRoute.CompleteRouteData.Waypoints.IndexOf(Item);
			App.ActiveRoute.CompleteRouteData.Waypoints.Insert(OldIndex-1,Item);
		}

		private void MoveDown_Click(object sender, RoutedEventArgs e)
		{
			int OldIndex = App.ActiveRoute.CompleteRouteData.Waypoints.IndexOf(Item);
			App.ActiveRoute.CompleteRouteData.Waypoints.Insert(OldIndex + 1, Item);
		}
	}
}
