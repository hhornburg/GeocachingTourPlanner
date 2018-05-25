using Itinero.LocalGeo;
using Microsoft.Win32;
using System.IO;

namespace GeocachingTourPlanner
{
	public class Database
	{
		/// <summary>
		/// Initialisierer für das Serialisieren
		/// </summary>
		public Database()
		{
		}

		/// <summary>
		/// Letzer Speicherrt der in einem Dateimangaer benutzt wurde
		/// </summary>
		public string LastUsedFilepath { get; set; }

		//DB Spezifisch
		public string GeocacheDB_Filepath { get; set; }
		public string RatingprofileDB_Filepath { get; set; }
		public string RoutingprofileDB_Filepath { get; set; }
		public string RouterDB_Filepath { get; set; }

		//Mapspecific
		public double LastMapResolution { get; set;}
		public Coordinate LastMapPosition { get; set; }

		public int MarkerSize { get; set; }
		/// <summary>
		/// Minimale Bewertung die ein Geocache in der Aktuellen Liste erreicht. Wird zum Erstellen der Farbcodierung benutzt
		/// </summary>
		public float MinimalRating { get; set; }
		/// <summary>
		/// Maximale Bewertung die ein Geocache in der Aktuellen Liste erreicht. Wird zum Erstellen der Farbcodierung benutzt
		/// </summary>
		public float MaximalRating { get; set; }

		public bool Autotargetselection { get; set; }
		public float PercentageOfDistanceInAutoTargetselection_Max { get; set; }
		public float PercentageOfDistanceInAutoTargetselection_Min { get; set; }
		public int RoutefindingWidth { get; set; }
		public bool DisplayLiveCalculation { get; set; }

		#region Methods
		/// <summary>
		/// checks if a DatabaseFilepath and the associated file exist.
		/// </summary>
		/// <param name="DatabaseFilepath"></param>
		/// <param name="DatabaseName"></param>
		public bool IsFilepathSet(Databases DatabaseName)
		{
			string DatabaseFilepath=null;
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
			}
			if(DatabaseFilepath == null || !File.Exists(DatabaseFilepath))//"||" So it doesn't run into exception if it is null
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
					StandardFileDialog.Filter = "gcdb files (*.gcdb)|*.gcdb|All files (*.*)|*.*";
					break;
				case Databases.Ratingprofiles:
					StandardFileDialog.Filter = "ratingprofiles files (*.ratingprf)|*.ratingprf|All files (*.*)|*.*";
					break;
				case Databases.Routingprofiles:
					StandardFileDialog.Filter = "routingprofile files (*.routingprf)|*.routingprf|All files (*.*)|*.*";
					break;
				case Databases.RouterDB:
					StandardFileDialog.Filter = "Routerdb files (*.routerdb)|*.routerdb|All files (*.*)|*.*";
					break;
			}

			if (StandardFileDialog.ShowDialog() == true)
			{
				LastUsedFilepath = StandardFileDialog.FileName;

				if (DatabaseName==Databases.Geocaches)
				{
					GeocacheDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
				else if (DatabaseName==Databases.Ratingprofiles)
				{
					RatingprofileDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
				else if (DatabaseName==Databases.Routingprofiles)
				{
					RoutingprofileDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
				else if (DatabaseName==Databases.RouterDB)
				{
					RouterDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
			}

			return false;//Since Databasefilepath hasn't been set
		}
		#endregion

		
	}

	public enum Databases
	{
		Geocaches,
		Ratingprofiles,
		Routingprofiles,
		RouterDB
	}
	
}
