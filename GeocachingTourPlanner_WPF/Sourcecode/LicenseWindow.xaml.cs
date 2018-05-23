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

namespace GeocachingTourPlanner_WPF
{
	/// <summary>
	/// Interaktionslogik für LicenseWindow.xaml
	/// </summary>
	public partial class LicenseWindow : Window
	{
		public bool AcceptedLicense = false;

		public LicenseWindow()
		{
			InitializeComponent();
		}

		private void Decline(object sender, RoutedEventArgs e)
		{
			AcceptedLicense = false;
			Close();
		}

		private void Accept(object sender, RoutedEventArgs e)
		{
			AcceptedLicense = true;
			Close();
		}
	}
}
