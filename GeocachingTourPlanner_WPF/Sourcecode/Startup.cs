using GeocachingTourPlanner_WPF;
using System.ComponentModel;
using System.IO;

namespace GeocachingTourPlanner
{
	static class Startup
	{
		public static void Start()
		{
			//Just to make sure they're not empty
			App.Ratingprofiles = new SortableBindingList<Ratingprofile>();
			App.Routingprofiles = new SortableBindingList<Routingprofile>();
			App.Geocaches = new SortableBindingList<Geocache>();

			App.mainWindow = new MainWindow();
			//FIX MainWindow.UpdateStatus("Started reading databases");
			if (File.Exists(App.Database_Filepath))//Thus it is not the first start of the App
			{
				Fileoperations.ReadMainDatabase();
				ReadRemainingDatabases();
			}
			else
			{
				First();
			}

			//FIX MainWindow.UpdateSettingsTextBoxes();
			CheckSettings();
			BindLists();
			Fileoperations.Backup(null);

			App.mainWindow.Show();
			App.mainWindow.Map_NavigateToLastVisited();
		}
		/// <summary>
		/// Called on first startup of App
		/// </summary>
		public static void First()
		{
			LicenseWindow licenseWindow = new LicenseWindow();
			licenseWindow.ShowDialog();
			if (!licenseWindow.AcceptedLicense)
			{
				App.mainWindow.Close();
			}

			//Set default settings
			App.DB.Autotargetselection = true;
			App.DB.PercentageOfDistanceInAutoTargetselection_Max = 0.9f;
			App.DB.PercentageOfDistanceInAutoTargetselection_Min = 0.75f;
			App.DB.RoutefindingWidth = 4;
			App.DB.DisplayLiveCalculation = false;

			//Mapspecific
			App.DB.LastMapResolution = 5;
			//FIX App.DB.LastMapPosition = new Coordinate(49.0, 8.5);
			App.DB.MarkerSize = 16;
		}

		/// <summary>
		/// Calls the sbroutines for reading each database and sets the list event handlers
		/// </summary>
		public static void ReadRemainingDatabases()
		{
			Fileoperations.ReadRouterDB();//Same thread, so it is ready when the App starts

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
			if (App.DB.PercentageOfDistanceInAutoTargetselection_Max == 0)
			{
				App.DB.PercentageOfDistanceInAutoTargetselection_Max = 0.9f;
			}
			if (App.DB.PercentageOfDistanceInAutoTargetselection_Min == 0)
			{
				App.DB.PercentageOfDistanceInAutoTargetselection_Min = 0.75f;
			}
			if (App.DB.RoutefindingWidth == 0)
			{
				App.DB.RoutefindingWidth = 3;
			}
			if (App.DB.MarkerSize == 0)
			{
				App.DB.MarkerSize = 16;
			}
		}

		public static void BindLists()
		{
			//To make them show up in the menu. Here, as the binding should also happen if none could be loaded
			App.Ratingprofiles.ListChanged += new ListChangedEventHandler(App.mainWindow.Ratingprofiles_ListChanged);
			App.Ratingprofiles.ResetBindings();

			App.Geocaches.ListChanged += new ListChangedEventHandler(App.mainWindow.Geocaches_ListChanged);
			App.Geocaches.ResetBindings();

			//To make them show up in the menu
			App.Routingprofiles.ListChanged += new ListChangedEventHandler(App.mainWindow.Routingprofiles_ListChanged);
			App.Routingprofiles.ResetBindings();
		}
	}
}
