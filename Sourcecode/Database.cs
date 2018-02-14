using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
			RatingDB_Filepath = "Ratingprofiles";
			RoutingDB_Filepath = "Routingprofiles";
		}

		/// <summary>
		/// Letzer Speicherrt der in einem Dateimangaer benutzt wurde
		/// </summary>
		public string LastUsedFilepath { get; set; }

		//DB Spezifisch
		public string GeocacheDB_Filepath { get; set; }
		public string RatingDB_Filepath { get; set; }
		public string RoutingDB_Filepath { get; set; }

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

		#region Methods
		/// <summary>
		/// checks if a DatabaseFilepath and the associated file exist. If it is not the case it asks wether the user wants to select a database file. Returns true if a file exists in the end.
		/// </summary>
		/// <param name="DatabaseFilepath"></param>
		/// <param name="DatabaseName"></param>
		public bool CheckDatabaseFilepath(string DatabaseFilepath, string DatabaseName)
		{
			if(DatabaseFilepath == null || !File.Exists(DatabaseFilepath))//"||" So it doesn't run into exception if it is null
			{
				if(MessageBox.Show("No " + DatabaseName + " found. Do you want to select a file?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
				{
					if (SetDatabaseFilepath(DatabaseFilepath))
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
		/// <param name="DatabaseFilepath"></param>
		/// <returns></returns>
		public bool SetDatabaseFilepath(string DatabaseFilepath)
		{
			OpenFileDialog StandardFileDialog = new OpenFileDialog()
			{
				InitialDirectory = LastUsedFilepath,
				Filter = "All files (*.*)|*.*"
			};

			if (StandardFileDialog.ShowDialog() == DialogResult.OK)
			{
				LastUsedFilepath = StandardFileDialog.FileName;

				if (DatabaseFilepath.Equals(GeocacheDB_Filepath))
				{
					GeocacheDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
				else if (DatabaseFilepath.Equals(RatingDB_Filepath))
				{
					RatingDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
				else if (DatabaseFilepath.Equals(RoutingDB_Filepath))
				{
					RoutingDB_Filepath = StandardFileDialog.FileName;
					return true; //since databasefilepath has been set;
				}
			}

			return false;//Since Databasefilepath hasn't been set
		}
		#endregion
	}
}
