using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using GMap.NET;

namespace GeocachingTourPlanner
{
	public class Database
	{
		/// <summary>
		/// Initialisierer für das Serialisieren
		/// </summary>
		public Database()
		{
			GeocacheDB_Filepath = "Geocaches";
			RatingprofileDB_Filepath = "Ratingprofiles";
			RoutingprofileDB_Filepath = "Routingprofiles";
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
		public double LastMapZoom { get; set;}
		public PointLatLng LastMapPosition { get; set; }
		/// <summary>
		/// Minimale Bewertung die ein Geocache in der Aktuellen Liste erreicht. Wird zum Erstellen der Farbcodierung benutzt
		/// </summary>
		public float MinimalRating { get; set; }
		/// <summary>
		/// Maximale Bewertung die ein Geocache in der Aktuellen Liste erreicht. Wird zum Erstellen der Farbcodierung benutzt
		/// </summary>
		public float MaximalRating { get; set; }

		public RouterMode RouterMode { get; set; }
		public int EveryNthShapepoint { get; set; }
		public int Divisor { get; set; }
		public int Tolerance { get; set; }
		public int RoutefindingWidth { get; set; }

		#region Methods
		/// <summary>
		/// checks if a DatabaseFilepath and the associated file exist. If it is not the case it asks wether the user wants to select a database file. Returns true if a file exists in the end.
		/// </summary>
		/// <param name="DatabaseFilepath"></param>
		/// <param name="DatabaseName"></param>
		public bool CheckDatabaseFilepath(Databases DatabaseName)
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
				if(MessageBox.Show(new Form { TopMost = true }, "No " + DatabaseName + " found. Do you want to select a file?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
				{
					if (SetDatabaseFilepath(DatabaseName))
					{
						return true;
					}
				}
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
		public bool SetDatabaseFilepath(Databases DatabaseName)
		{
			OpenFileDialog StandardFileDialog = new OpenFileDialog()
			{
				InitialDirectory = LastUsedFilepath,
				Filter = "All files (*.*)|*.*"
			};

			if (StandardFileDialog.ShowDialog() == DialogResult.OK)
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

	public enum RouterMode
	{
		On_the_go,
		Some_thought,
		Try_hard
	}
}
