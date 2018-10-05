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
using GeocachingTourPlanner.IO;
using GeocachingTourPlanner.Types;

namespace GeocachingTourPlanner.UI
{
	/// <summary>
	/// Interaktionslogik für DatabaseFileDialog.xaml
	/// </summary>
	public partial class DatabaseFileDialog : Window
	{
		Databases ThisDB;

		/// <summary>
		/// Use only with .ShowDialog(). Returns OK if a database is set.
		/// </summary>
		/// <param name="Database">Database for which thiis dialog is shown</param>
		/// <param name="AllowImport">Defaults to true. If  false, the import button is removed</param>
		public DatabaseFileDialog(Databases Database, bool AllowImport = true)
		{
			InitializeComponent();
            ThisDB = Database;

			switch (Database)
			{
				case Databases.Geocaches:
					MessageText.Text = "Couldn't find a geocaches database.";
					if (AllowImport)
					{
						NewImportButtonText.Text = "Import Pocket query";
					}
					else
					{
						NewImportButton.Visibility = Visibility.Collapsed;
					}
					break;
				case Databases.Ratingprofiles:
					MessageText.Text = "Couldn't find a ratingprofiles database.";
					NewImportButtonText.Text = "Create new File";
					break;
				case Databases.Routingprofiles:
					MessageText.Text = "Couldn't find a routingprofiles database.";
					NewImportButtonText.Text = "Create new File";
					break;
				case Databases.RouterDB:
					MessageText.Text = "Couldn't find a RouterDB database.";
					if (AllowImport)
					{
						NewImportButtonText.Text = "Import .pbf File";
					}
					else
					{
						NewImportButton.Visibility = Visibility.Collapsed;
					}
					break;
                case Databases.Routes:
                    MessageText.Text = "Couldn't find a route database.";
                    NewImportButtonText.Text = "Create new File";
                    break;

            }
		}

		private void OpenButton_Click(object sender, RoutedEventArgs e)
		{
			if (App.DB.OpenExistingDBFile(ThisDB))
			{
				DialogResult = true;
			}
			else
			{
				DialogResult = false;
			}
            Close();
		}

		private void NewImport_Button_Click(object sender, RoutedEventArgs e)
		{
			switch (ThisDB)
			{
				case Databases.Geocaches:
					Fileoperations.ImportGeocaches();
					break;
				case Databases.Ratingprofiles:
					Fileoperations.NewRatingprofileDatabase();
					break;
				case Databases.Routingprofiles:
					Fileoperations.NewRoutingprofileDatabase();
					break;
				case Databases.RouterDB:
					Fileoperations.ImportOSMData();
					break;
                case Databases.Routes:
                    Fileoperations.NewRouteDatabase();
                    break;
			}
			DialogResult = true;
            Close();
		}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
