using System.Windows;

namespace GeocachingTourPlanner.UI
{
	/// <summary>
	/// Interaktionslogik für LicenseWindow.xaml
	/// </summary>
	public partial class LicenseWindow : Window
	{
		/// <summary>
		/// Wether the user accepted the license or not
		/// </summary>
		public bool AcceptedLicense = false;

		/// <summary>
		/// Shows a window eith the license and the possibility to accept or decline
		/// </summary>
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
