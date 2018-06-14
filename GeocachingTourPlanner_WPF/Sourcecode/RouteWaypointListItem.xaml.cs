using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeocachingTourPlanner.UI
{
	/// <summary>
	/// Interaktionslogik für RouteWaypointListItem.xaml
	/// </summary>
	public partial class RouteWaypointListItem : UserControl
	{
		object Item = new object();
		/// <summary>
		/// Initializes a new item to be dispayed in the list of waypoints
		/// </summary>
		/// <param name="Item"></param>
		/// <param name="Name"></param>
		/// <param name="Description"></param>
		public RouteWaypointListItem(object Item, string Name, string Description)
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
