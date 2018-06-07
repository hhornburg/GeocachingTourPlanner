using System.Windows;

namespace GeocachingTourPlanner.UI
{
	static class MapTooltip
    {
		private static Visibility CustomTooltipVisibility { get; set; } = Visibility.Collapsed;
		private static string CustomTooltipText { get; set; }
		private static Point _CustomTooltipLocation;
		private static Point CustomTooltipLocation
		{
			get { return _CustomTooltipLocation; }
			set
			{
				double x = value.X;
				double y = value.Y;
				if (x > App.mainWindow.mapControl.ActualWidth - 150)
				{
					x = App.mainWindow.mapControl.ActualWidth - 150;
				}
				if (y < App.mainWindow.mapControl.ActualHeight)
				{
					y = App.mainWindow.mapControl.ActualHeight;
				}
				_CustomTooltipLocation = new Point(x, y);
			}
		}
		public static void ShowTooltip(string text, Point Location)
		{
			CustomTooltipVisibility = Visibility.Visible;
			CustomTooltipLocation = Location;
			CustomTooltipText = text;
		}
		public static void HideTooltip()
		{
			CustomTooltipVisibility = Visibility.Collapsed;
			CustomTooltipText = "";
		}
	}
}
