using GeocachingTourPlanner.IO;
using GeocachingTourPlanner.Routing;
using Itinero.LocalGeo;
using Microsoft.Win32;
using System.IO;

namespace GeocachingTourPlanner.Types
{
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
		private Ratingprofile _activeRatingprofile;
		private Routingprofile _activeRoutingprofile;
		private RoutePlanner _activeRoute;
		private int _markerSize;
		private float _minimalRating;
		private float _maximalRating;
		private bool _autotargetselection;
		private float _percentageOfDistanceInAutoTargetselection_Max;
		private float _percentageOfDistanceInAutoTargetselection_Min;
		private int _routefindingWidth;
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
		public Ratingprofile ActiveRatingprofile { get => _activeRatingprofile; set { _activeRatingprofile = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Routingprofile that is selected
		/// </summary>
		public Routingprofile ActiveRoutingprofile { get => _activeRoutingprofile; set { _activeRoutingprofile = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Routingprofile that is selected
		/// </summary>
		public RoutePlanner ActiveRoute { get => _activeRoute; set { _activeRoute = value; Fileoperations.Backup(Databases.MainDatabase); } }

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
		public int RoutefindingWidth { get => _routefindingWidth; set { _routefindingWidth = value; Fileoperations.Backup(Databases.MainDatabase); } }
		/// <summary>
		/// Wether the caclulation of the routes should be displayed live
		/// </summary>
		public bool DisplayLiveCalculation { get => _displayLiveCalculation; set { _displayLiveCalculation = value; Fileoperations.Backup(Databases.MainDatabase); } }

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
					StandardFileDialog.Filter = "Geocache Database files (*.gcdb)|*.gcdb|All files (*.*)|*.*";
					break;
				case Databases.Ratingprofiles:
					StandardFileDialog.Filter = "Ratingprofiles files (*.ratingprf)|*.ratingprf|All files (*.*)|*.*";
					break;
				case Databases.Routingprofiles:
					StandardFileDialog.Filter = "Routingprofile files (*.routingprf)|*.routingprf|All files (*.*)|*.*";
					break;
				case Databases.RouterDB:
					StandardFileDialog.Filter = "RouterDB files (*.routerdb)|*.routerdb|All files (*.*)|*.*";
					break;
				case Databases.Routes:
					StandardFileDialog.Filter= "Route files (*.routes)|*.routes|All files (*.*)|*.*";
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
