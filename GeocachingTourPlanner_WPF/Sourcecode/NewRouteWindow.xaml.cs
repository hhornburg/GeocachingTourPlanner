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
using System.Windows.Shapes;

namespace GeocachingTourPlanner.UI
{
	/// <summary>
	/// Interaktionslogik für NewRouteWindow.xaml
	/// </summary>
	public partial class NewRouteWindow : Window
	{
		public string Name;

		public NewRouteWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Name = RouteName.Text;
			Close();
		}
	}
}
