using GeocachingTourPlanner;
using GeocachingTourPlanner.IO;
using GeocachingTourPlanner.Types;
using GeocachingTourPlanner.UI;
using Itinero.LocalGeo;
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
			App.Routes = new SortableBindingList<Routing.RoutePlanner>();

			App.mainWindow = new MainWindow();
			App.mainWindow.UpdateStatus("Started reading databases");
			if (File.Exists(App.Database_Filepath))//Thus it is not the first start of the App
			{
				Fileoperations.ReadMainDatabase();
				ReadRemainingDatabases();
			}
			else
			{
				First();
			}

			App.mainWindow.UpdateSettingsTextBoxes();
			CheckSettings();
			BindLists();

			App.mainWindow.Show();
			App.StartupCompleted = true;
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
			App.DB.PercentageOfDistanceInAutoTargetselection_Max = 0.9f;
			App.DB.PercentageOfDistanceInAutoTargetselection_Min = 0.75f;
			App.DB.RoutefindingWidth = 4;
			App.DB.DisplayLiveCalculation = true;

			//Mapspecific
			App.DB.LastMapResolution = 1000;
			App.DB.LastMapPosition = new Coordinate(49.0f, 8.5f);
			App.DB.MarkerSize = 16;
		}

		/// <summary>
		/// Calls the sbroutines for reading each database and sets the list event handlers
		/// </summary>
		public static void ReadRemainingDatabases()
		{
			Fileoperations.ReadRouterDB();

			//Load Ratingprofiles from the File specified in the Database
			Fileoperations.ReadRatingprofiles();
			
			//Geocaches
			Fileoperations.ReadGeocaches();
			
			//Routingprofile
			Fileoperations.ReadRoutingprofiles();

			Fileoperations.ReadRoutes();
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

		/// <summary>
		/// Binds List to the methods that keep the comboboxes updated and that back them up
		/// </summary>
		public static void BindLists()
		{
			//To make them show up in the menu. Here, as the binding should also happen if none could be loaded
			App.Ratingprofiles.ListChanged += new ListChangedEventHandler(App.mainWindow.Ratingprofiles_ListChanged);
			App.Ratingprofiles.ListChanged += new ListChangedEventHandler((s, e) => { Fileoperations.Backup(Databases.Ratingprofiles); });
			App.Ratingprofiles.ResetBindings();

			App.Geocaches.ListChanged += new ListChangedEventHandler(App.mainWindow.Geocaches_ListChanged);
			App.Geocaches.ListChanged += new ListChangedEventHandler((s, e) => { Fileoperations.Backup(Databases.Geocaches); });
			App.Geocaches.ResetBindings();

			//To make them show up in the menu
			App.Routingprofiles.ListChanged += new ListChangedEventHandler(App.mainWindow.Routingprofiles_ListChanged);
			App.Routingprofiles.ListChanged += new ListChangedEventHandler((s, e) => { Fileoperations.Backup(Databases.Routingprofiles); });
			App.Routingprofiles.ResetBindings();
		}

		private static void SetLastSelections()
		{
			if (App.DB.ActiveRatingprofile!=null)
			{
				App.mainWindow.SetRatingprofile(App.DB.ActiveRatingprofile);
			}
			if (App.DB.ActiveRoutingprofile != null)
			{
				App.mainWindow.SetRoutingprofile(App.DB.ActiveRoutingprofile);
			}
			if (App.DB.ActiveRoute != null)
			{
				App.mainWindow.SetRoute(App.DB.ActiveRoute);
			}
		}
	}
}
