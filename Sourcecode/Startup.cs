using GMap.NET;
using Itinero;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeocachingTourPlanner
{
	static class Startup
	{
		/// <summary>
		/// Called on first startup of program
		/// </summary>
		public static void First()
		{
			AcceptLicenseWindow licenseWindow = new AcceptLicenseWindow();
			licenseWindow.ShowDialog();
			if (!licenseWindow.AcceptedLicense)
			{
				Program.MainWindow.Close();
			}

			//Set default settings
			Program.DB.EveryNthShapepoint = 1;
			Program.DB.Autotargetselection = true;
			Program.DB.Tolerance = 200;
			Program.DB.Divisor = 5;
			Program.DB.RoutefindingWidth = 4;
			Program.DB.DisplayLiveCalculation = false;

			//Mapspecific
			Program.DB.LastMapZoom = 5;
			Program.DB.LastMapPosition = new PointLatLng(49.0, 8.5);
			Program.DB.MarkerSize = 16;

			Program.MainWindow.LeftTabs.SelectedIndex = 0;
		}

		/// <summary>
		/// Calls the sbroutines for reading each database and sets the list event handlers
		/// </summary>
		public static void ReadRemainingDatabases()
		{
			try
			{
				if (Program.DB.RouterDB_Filepath != null)
				{
					using (var stream = new FileInfo(Program.DB.RouterDB_Filepath).OpenRead())
					{
						Program.RouterDB = RouterDb.Deserialize(stream);
					}

					Program.MainWindow.RouterDBStateLabel.Text = "Successfully loaded RouterDB";
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Failed to read RouterDB", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			

			//Load Ratingprofiles from the File specified in the Database
			Fileoperations.ReadRatingprofiles();
			
			//Geocaches
			Fileoperations.ReadGeocaches();
			
			//Routingprofile
			Fileoperations.ReadRoutingprofiles();
			
			Fileoperations.Backup(null);//so settings get saved in the DB. Nothing else, as it just came from the file

		}

		public static void CheckSettings()
		{
			if (Program.DB.EveryNthShapepoint == 0)
			{
				Program.DB.EveryNthShapepoint = 5;
			}
			if (Program.DB.Divisor == 0)
			{
				Program.DB.Divisor = 5;
			}
			if (Program.DB.RoutefindingWidth == 0)
			{
				Program.DB.RoutefindingWidth = 3;
			}
			if (Program.DB.MarkerSize == 0)
			{
				Program.DB.MarkerSize = 16;
			}
		}

		public static void BindLists()
		{
			//To make them show up in the menu. Here, as the binding should also happen if none could be loaded
			Program.Ratingprofiles.ListChanged += new ListChangedEventHandler(Program.MainWindow.Ratingprofiles_ListChanged);
			Program.Ratingprofiles.ResetBindings();

			Program.MainWindow.GeocacheTable.DataSource = Program.Geocaches;

			//To make them show up in the menu
			Program.Routingprofiles.ListChanged += new ListChangedEventHandler(Program.MainWindow.Routingprofiles_ListChanged);
			Program.Routingprofiles.ResetBindings();

		}
	}
}
