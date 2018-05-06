using Itinero;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GeocachingTourPlanner
{
	static class Program
    {
        /// <summary>
        /// Database in which all relevant data is saved to the disk
        /// </summary>
        public static Database DB = new Database();
        public static string Database_Filepath = "Database";

		/// <summary>
		/// To make backups faster and minimize damage in loss of compatibility, the Profiles and the Geocaches are put in different Databases
		/// </summary>

		public static SortableBindingList<Ratingprofile> Ratingprofiles { get;set; }
		public static SortableBindingList<Routingprofile> Routingprofiles { get; set; }
		public static SortableBindingList<Geocache> Geocaches { get; set; }


		// Itinero
		public static RouterDb RouterDB = new RouterDb();
		public static BindingList<KeyValuePair<string, Tourplanning.RouteData>> Routes = new BindingList<KeyValuePair<string, Tourplanning.RouteData>>();

		//Program Variables
		public static bool RouteCalculationRunning = false;
		public static bool ImportOfOSMDataRunning = false;

		//Cache
		public static List<KeyValueTriple<Bitmap, GeocacheType, int>> MarkerImageCache = new List<KeyValueTriple<Bitmap, GeocacheType, int>>();


		public static Form1 MainWindow;

		/// <summary>
		/// Main entrypoint
		/// </summary>
		[STAThread]
        static void Main()
        {
			//Just to make sure they're not empty
			Ratingprofiles = new SortableBindingList<Ratingprofile>();
			Routingprofiles = new SortableBindingList<Routingprofile>();
			Geocaches = new SortableBindingList<Geocache>();

			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow = new Form1();

			//select the Overview Tab
			MainWindow.LeftTabs.SelectedIndex = 1;

			MainWindow.UpdateStatus("Started reading databases");
			if (File.Exists(Database_Filepath))//Thus it is not the first start of the program
			{
				Fileoperations.ReadMainDatabase();
				Startup.ReadRemainingDatabases();
			}
			else
			{
				Startup.First();
			}

			MainWindow.UpdateSettingsTextBoxes();
			Startup.CheckSettings();
			Startup.BindLists();
			
			Application.Run(MainWindow);
        }
		
	}
}
