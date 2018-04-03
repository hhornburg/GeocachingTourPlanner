using Itinero;
using System;
using System.Collections.Generic;
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

			Program.MainWindow.LeftTabs.SelectedIndex = 0;
		}
		
		public static void ReadRemainingDatabases()
		{

			if (Program.DB.RouterDB_Filepath != null)
			{
				using (var stream = new FileInfo(Program.DB.RouterDB_Filepath).OpenRead())
				{
					Program.RouterDB = RouterDb.Deserialize(stream);
				}

				Program.MainWindow.RouterDBStateLabel.Text = "Successfully loaded RouterDB";
			}

			//Load Ratingprofiles from the File specified in the Database
			Fileoperations.ReadRatingprofiles();

			//Geocaches
			Fileoperations.ReadGeocaches();
			Program.MainWindow.GeocacheTable.DataSource = Program.Geocaches;

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
		}
	}
}
