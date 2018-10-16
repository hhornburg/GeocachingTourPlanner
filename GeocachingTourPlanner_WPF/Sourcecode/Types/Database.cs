using GeocachingTourPlanner.IO;
using GeocachingTourPlanner.Routing;
using Itinero.LocalGeo;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;

namespace GeocachingTourPlanner.Types
{
    //TODO Saves Routes and Ratingprofiles as a whole to the file, but will always only use a string to reference them. Use IXmlSerializable to optimize
    /// <summary>
    /// A change to any element here will result in a backup of the database
    /// </summary>
	public class Database
	{
		private string _lastUsedFilepath;
		private string _geocacheDB_Filepath;
		private string _ratingprofileDB_Filepath;
		private string _routingprofileDB_Filepath;
		private string _routerDB_Filepath;
		private string _routeDB_Filepath;
		private double _lastMapResolution;
		private Coordinate _lastMapPosition;
		private string _activeRatingprofile;
		private string _activeRoutingprofile;
		private string _activeRoute;
		private int _markerSize;
		private float _minimalRating;
		private float _maximalRating;
        private double _minAllowedRating;
		private bool _autotargetselection;
		private float _percentageOfDistanceInAutoTargetselection_Max;
		private float _percentageOfDistanceInAutoTargetselection_Min;
		private float _onRouteDistanceLimit;
		private bool _displayLiveCalculation;

		/// <summary>
		/// Initialisierer für das Serialisieren
		/// </summary>
		public Database()
		{
		}

		/// <summary>
		/// Letzer Speicherrt der in einem Dateimangaer benutzt wurde
		/// </summary>
		public string LastUsedFilepath { get => _lastUsedFilepath; set { _lastUsedFilepath = value; Fileoperations.Backup(Databases.MainDatabase); } }

		/// <summary>
		/// Filepath of loaded Geocache DB
		/// </summary>
		public string GeocacheDB_Filepath { get => _geocacheDB_Filepath; set { _geocacheDB_Filepath = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Filepath of loaded Ratingprofile DB
		/// </summary>
		public string RatingprofileDB_Filepath { get => _ratingprofileDB_Filepath; set { _ratingprofileDB_Filepath = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Filepath of Routingprofile DB
		/// </summary>
		public string RoutingprofileDB_Filepath { get => _routingprofileDB_Filepath; set { _routingprofileDB_Filepath = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Filepath of Route DB
		/// </summary>
		public string RoutesDB_Filepath { get => _routeDB_Filepath; set { _routeDB_Filepath = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Filepath of loaded RouterDB
		/// </summary>
		public string RouterDB_Filepath
		{
			get => _routerDB_Filepath; set { _routerDB_Filepath = value; Fileoperations.Backup(Databases.MainDatabase); }
		}

		//Mapspecific
		/// <summary>
		/// Last used Map resolution
		/// </summary>
		public double LastMapResolution { get => _lastMapResolution; set { _lastMapResolution = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Last map Center
		/// </summary>
		public Coordinate LastMapPosition { get => _lastMapPosition; set { _lastMapPosition = value; Fileoperations.Backup(Databases.MainDatabase); } }

		/// <summary>
		/// Ratingprofile that is selected
		/// </summary>
		public Ratingprofile ActiveRatingprofile { get => GetRatingprofile(_activeRatingprofile); set { if(value!=null) _activeRatingprofile = value.Name; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Routingprofile that is selected
		/// </summary>
		public Routingprofile ActiveRoutingprofile { get => GetRoutingprofile(_activeRoutingprofile); set { if (value != null) _activeRoutingprofile = value.Name; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Routingprofile that is selected
		/// </summary>
		public RoutePlanner ActiveRoute { get => GetRoute(_activeRoute); set { if (value != null) _activeRoute = value.Name; Fileoperations.Backup(Databases.MainDatabase); } }

		/// <summary>
		/// Marker size in pixel
		/// </summary>
		public int MarkerSize { get => _markerSize; set { _markerSize = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Minimal Rating of all geocaches. Used for the creation of the color coding
		/// </summary>
		public float MinimalRating { get => _minimalRating; set { _minimalRating = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Maximal Rating of all geocaches. Used for the creation of the color coding
		/// </summary>
		public float MaximalRating { get => _maximalRating; set { _maximalRating = value; Fileoperations.Backup(Databases.MainDatabase); } }
        /// <summary>
		/// Maximal Rating of all geocaches. Used for the creation of the color coding
		/// </summary>
		public double MinAllowedRating { get => _minAllowedRating; set { _minAllowedRating = value; Fileoperations.Backup(Databases.MainDatabase); } }

        /// <summary>
        /// Wether the routing algorithm should look for geocaches that should be targeted
        /// </summary>
        public bool Autotargetselection { get => _autotargetselection; set { _autotargetselection = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// How much should be the limit to the distance that the route covers after Autotargetselection
		/// </summary>
		public float PercentageOfDistanceInAutoTargetselection_Max { get => _percentageOfDistanceInAutoTargetselection_Max; set { _percentageOfDistanceInAutoTargetselection_Max = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// How much should be the limit to the distance that the route covers after Autotargetselection
		/// </summary>
		public float PercentageOfDistanceInAutoTargetselection_Min { get => _percentageOfDistanceInAutoTargetselection_Min; set { _percentageOfDistanceInAutoTargetselection_Min = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// How many routes to compare when seleccting targets in Autotargetselection
		/// </summary>
		public float OnRouteDistanceLimit { get => _onRouteDistanceLimit; set { _onRouteDistanceLimit = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Wether the caclulation of the routes should be displayed live
		/// </summary>
		public bool DisplayLiveCalculation { get => _displayLiveCalculation; set { _displayLiveCalculation = value; Fileoperations.Backup(Databases.MainDatabase); } }

        /// <summary>
        /// Fires when the Maximal or the minimal rating of the geocaches was changed
        /// </summary>
        public event EventHandler MaximalRatingsChangedEvent;

		#region Methods
		/// <summary>
		/// checks if a DatabaseFilepath and the associated file exist.
		/// </summary>
		/// <param name="DatabaseName"></param>
		public bool IsFilepathSet(Databases DatabaseName)
		{
			string DatabaseFilepath = null;
			switch (DatabaseName)
			{
				case Databases.Geocaches:
					DatabaseFilepath = GeocacheDB_Filepath;
					break;
				case Databases.Ratingprofiles:
					DatabaseFilepath = RatingprofileDB_Filepath;
					break;
				case Databases.Routingprofiles:
					DatabaseFilepath = RoutingprofileDB_Filepath;
					break;
				case Databases.RouterDB:
					DatabaseFilepath = RouterDB_Filepath;
					break;
				case Databases.Routes:
					DatabaseFilepath = RoutesDB_Filepath;
					break;
			}
			if (DatabaseFilepath == null || !File.Exists(DatabaseFilepath))//"||" So it doesn't run into exception if it is null
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Displays a fledialog to set the filepath of the specified database. returns true on success
		/// </summary>
		/// <param name="DatabaseName"></param>
		/// <returns></returns>
		public bool OpenExistingDBFile(Databases DatabaseName)
		{
			OpenFileDialog StandardFileDialog = new OpenFileDialog()
			{
				InitialDirectory = LastUsedFilepath,
				FilterIndex = 0,
			};

			switch (DatabaseName)
			{
				case Databases.Geocaches:
					StandardFileDialog.Filter = Properties.FileDialogFilters.Geocaches;
					break;
				case Databases.Ratingprofiles:
					StandardFileDialog.Filter = Properties.FileDialogFilters.Ratingprofiles;
					break;
				case Databases.Routingprofiles:
					StandardFileDialog.Filter = Properties.FileDialogFilters.Routingprofiles;
					break;
				case Databases.RouterDB:
					StandardFileDialog.Filter = Properties.FileDialogFilters.RouterDB;
					break;
				case Databases.Routes:
					StandardFileDialog.Filter= Properties.FileDialogFilters.Routes;
					break;

			}

			if (StandardFileDialog.ShowDialog() == true)
			{
				LastUsedFilepath = StandardFileDialog.FileName;

				if (DatabaseName == Databases.Geocaches)
				{
					GeocacheDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
				else if (DatabaseName == Databases.Ratingprofiles)
				{
					RatingprofileDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
				else if (DatabaseName == Databases.Routingprofiles)
				{
					RoutingprofileDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
				else if (DatabaseName == Databases.RouterDB)
				{
					RouterDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
				else if (DatabaseName == Databases.Routes)
				{
					RoutesDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
			}

			return false;//Since Databasefilepath hasn't been set
		}

        /// <summary>
        /// Recalculates the Maximal and the minimal ratings and triggers the appropriate event
        /// </summary>
        public void RecalculateRatingLimits()
        {
            if (App.Geocaches.Count > 0)
            {
                App.Geocaches = new SortableBindingList<Geocache>(App.Geocaches.OrderByDescending(x => x.Rating).ToList());
                Startup.BindLists();//Since binding is lost when new list is created
                MaximalRating = App.Geocaches[0].Rating;//Possible since list is sorted
                MinimalRating = App.Geocaches[App.Geocaches.Count - 1].Rating;
                if (MaximalRatingsChangedEvent != null)
                {
                    MaximalRatingsChangedEvent(this, null);
                }
            }
        }

        private Ratingprofile GetRatingprofile(string RP_Name)
        {
            if (App.Ratingprofiles.Count(x => x.Name == RP_Name) > 0)
            {
                return App.Ratingprofiles.First(x => x.Name == RP_Name);
            }
            else
            {
                return null;
            }
        }

        private Routingprofile GetRoutingprofile(string RP_Name)
        {
            if (App.Routingprofiles.Count(x => x.Name == RP_Name) > 0)
            {
                return App.Routingprofiles.First(x => x.Name == RP_Name);
            }
            else
            {
                return null;
            }
        }

        private RoutePlanner GetRoute(string RP_Name)
        {
            if (App.Routes.Count(x => x.Name == RP_Name) > 0)
            {
                return App.Routes.First(x => x.Name == RP_Name);
            }
            else
            {
                return null;
            }
        }
        #endregion


    }

    /// <summary>
    /// List of all Database types. Used as parameter to determine which Database is meant
    /// </summary>
	public enum Databases
	{
        MainDatabase,
		Geocaches,
		Ratingprofiles,
		Routingprofiles,
		RouterDB,
		Routes
	}
	
}
