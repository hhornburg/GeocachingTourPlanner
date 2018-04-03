using Itinero;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static SortableBindingList<Ratingprofile> Ratingprofiles = new SortableBindingList<Ratingprofile>();
        public static SortableBindingList<Routingprofile> Routingprofiles = new SortableBindingList<Routingprofile>();
        public static SortableBindingList<Geocache> Geocaches = new SortableBindingList<Geocache>();

		// Itinero
		public static RouterDb RouterDB = new RouterDb();
		public static BindingList<KeyValueTriple<string, Route, List<Geocache>>> Routes = new BindingList<KeyValueTriple<string, Route, List<Geocache>>>();

		public static bool RouteCalculationRunning = false;
		

		public static Form1 MainWindow;

		/// <summary>
		/// Main entrypoint
		/// </summary>
		[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow = new Form1();

			//select the Overview Tab
			MainWindow.LeftTabs.SelectedIndex = 1;

			if (File.Exists(Database_Filepath))//Thus it is not the first start of the program
			{
				Fileoperations.ReadMainDatabase();
			}
			else
			{
				Startup.First();
			}

			MainWindow.UpdateSettingsTextBoxes();

			Startup.ReadRemainingDatabases();
			Startup.CheckSettings();
			
			Application.Run(MainWindow);
        }
		
	}
}
