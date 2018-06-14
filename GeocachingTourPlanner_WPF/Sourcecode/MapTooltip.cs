using System.Windows;
using System.Windows.Controls;

namespace GeocachingTourPlanner.UI
{
	static class MapTooltip
    {
		public static void ShowTooltip(string text, Point Location)
		{
			App.mainWindow.TooltipBorder.Visibility = Visibility.Visible;

			if (Location.X > App.mainWindow.mapControl.ActualWidth - 150)
			{
				Location.X = App.mainWindow.mapControl.ActualWidth - 150;
			}
			if (Location.Y > App.mainWindow.mapControl.ActualHeight)
			{
				Location.Y = App.mainWindow.mapControl.ActualHeight;
			}
			Canvas.SetLeft(App.mainWindow.TooltipBorder,Location.X);
			Canvas.SetTop(App.mainWindow.TooltipBorder, Location.Y);

			App.mainWindow.TooltipText.Text = text;
		}
		public static void HideTooltip()
		{
			App.mainWindow.TooltipBorder.Visibility = Visibility.Collapsed;
			App.mainWindow.TooltipText.Text = "";
		}
	}
}
