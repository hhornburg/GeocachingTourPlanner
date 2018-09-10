using GeocachingTourPlanner.Routing;
using GeocachingTourPlanner.Types;
using GeocachingTourPlanner.UI;
using Itinero;
using Mapsui.Styles;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace GeocachingTourPlanner
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Database in which all relevant data is saved to the disk
        /// </summary>
        public static Database DB = new Database();
        public static string Database_Filepath = "Database";

        /// <summary>
        /// To make backups faster and minimize damage in loss of compatibility, the Profiles and the Geocaches are put in different Databases
        /// </summary>

        public static SortableBindingList<Ratingprofile> Ratingprofiles { get; set; }
        public static SortableBindingList<Routingprofile> Routingprofiles { get; set; }
        public static SortableBindingList<Geocache> Geocaches { get; set; }
        public static SortableBindingList<RoutePlanner> Routes { get; set; }


        // Itinero
        public static RouterDb RouterDB = new RouterDb();

        //App Variables
        /// <summary>
        /// So no backup is made during startup process which would overwrite the Database
        /// </summary>
        public static bool StartupCompleted = false;
        public static bool RouteCalculationRunning = false;
        public static bool ImportOfOSMDataRunning = false;
        /// <summary>
        /// When reading a GeocacheDB from the file, the List is cleared. Thus, the Binding wants to update the hard copy of the list. This causes an error since it either interferes or the file to import is empty, since the list was cleared. Use this variable to prevent this from happening
        /// </summary>
        public static bool LockGeocacheDB_File = false;
        /// <summary>
        /// When reading a RatingprofileDB from the file, the List is cleared. Thus, the Binding wants to update the hard copy of the list. This causes an error since it either interferes or the file to import is empty, since the list was cleared. Use this variable to prevent this from happening
        /// </summary>
        public static bool LockRatingprofileDB_File = false;
        /// <summary>
        /// When reading a RoutingprofileDB from the file, the List is cleared. Thus, the Binding wants to update the hard copy of the list. This causes an error since it either interferes or the file to import is empty, since the list was cleared. Use this variable to prevent this from happening
        /// </summary>
        public static bool LockRoutingprofileDB_File = false;
        /// <summary>
        /// When reading a RouteDB from the file, the List is cleared. Thus, the Binding wants to update the hard copy of the list. This causes an error since it either interferes or the file to import is empty, since the list was cleared. Use this variable to prevent this from happening
        /// </summary>
        public static bool LockRouteDB_File = false;

        //Cache
        public static List<KeyValueTriple<SymbolStyle, GeocacheType, int>> MarkerStyleCache = new List<KeyValueTriple<SymbolStyle, GeocacheType, int>>();

        //Mainwindow
        public static MainWindow mainWindow = new MainWindow();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            GeocachingTourPlanner.Startup.Start();
        }
    }
}
