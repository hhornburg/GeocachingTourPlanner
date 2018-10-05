using System;
using System.IO;
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

            string License = "";
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("LICENSE"))
                {
                    // Read the stream to a string, and write the string to the console.
                    License = sr.ReadToEnd();
                    LicenseTextBlock.Text = License;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not find license file. \nPlease check https://github.com/pingurus/GeocachingTourPlanner/blob/master/LICENSE");
            }

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
