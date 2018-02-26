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
        static string Database_Filepath = "Database";
        static XmlSerializer DBSerializer = new XmlSerializer(typeof(Database));

        /// <summary>
        /// To make backups faster and minimize damage in loss of compatibility, the Profiles and the Geocaches are put in different Databases
        /// </summary>
        public static SortableBindingList<Ratingprofile> Ratingprofiles = new SortableBindingList<Ratingprofile>();
        static XmlSerializer RatingprofilesSerializer = new XmlSerializer(typeof(SortableBindingList<Ratingprofile>));
        public static SortableBindingList<Routingprofile> Routingprofiles = new SortableBindingList<Routingprofile>();
        static XmlSerializer RoutingprofilesSerializer = new XmlSerializer(typeof(SortableBindingList<Routingprofile>));
        public static SortableBindingList<Geocache> Geocaches = new SortableBindingList<Geocache>();
        static XmlSerializer GeocachesSerializer = new XmlSerializer(typeof(SortableBindingList<Geocache>));

		// Itinero
		public static RouterDb RouterDB = new RouterDb();
		public static BindingList<KeyValueTriple<string, Route, List<Geocache>>> Routes = new BindingList<KeyValueTriple<string, Route, List<Geocache>>>();
		
	/// <summary>
	/// Main entrypoint
	/// </summary>
	public static Form1 MainWindow;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow = new Form1();

			//Initialisierungen:
			//DB
			if (File.Exists(Database_Filepath))//Thus it is not the first start of the program
			{
				StreamReader DBReader = null;
				try
				{
					DBReader = new StreamReader(Database_Filepath);
					DB = (Database)DBSerializer.Deserialize(DBReader);
				}
				catch (Exception)
				{
					MessageBox.Show("Couldn't import Database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					if (DBReader != null)
					{
						DBReader.Close();
					}
				}
			}
			else
			{
				AcceptLicenseWindow licenseWindow = new AcceptLicenseWindow();
				licenseWindow.ShowDialog();
				if (!licenseWindow.AcceptedLicense)
				{
					MainWindow.Close();
				}
			}

			if (DB.RouterDB_Filepath != null)
			{
				using (var stream = new FileInfo(DB.RouterDB_Filepath).OpenRead())
				{
				 RouterDB = RouterDb.Deserialize(stream);
				}
			}
			
			//Load Ratingprofiles from the File specified in the Database
			ReadRatingprofiles();
			//Geocaches
			ReadGeocaches();
			//Routingprofile
			ReadRoutingprofiles();
			Backup(null);//so settings get saved in the DB. Nothing else, as it just came from the file


			MainWindow.GeocacheTable.DataSource = Program.Geocaches;

			Application.Run(MainWindow);
        }
		
		/// <summary>
		/// The main Database gets saved anyways specify which otherList should be saved alongside. Returns true on success
		/// </summary>
		/// <param name="ExtraBackup"></param>
		public static bool Backup(object ExtraBackup)
        {
            //Aus Performancegrnden nicht alles
            if (ExtraBackup == Geocaches)
            {
                if (DB.GeocacheDB_Filepath == null)
                {
                    DB.GeocacheDB_Filepath = "Geocaches";
                }
                TextWriter GeocachesWriter = null;
				try
				{
					GeocachesWriter = new StreamWriter(DB.GeocacheDB_Filepath);
					GeocachesSerializer.Serialize(GeocachesWriter, Geocaches);
					return true;
				}
				catch
				{
					MessageBox.Show("Fileerror. Is the Geocaches Database used by another program?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				finally
				{
					if (GeocachesWriter != null)
					{
						GeocachesWriter.Close();
					}
				}
				
			}

			else if (ExtraBackup == Routingprofiles)
			{
				if (DB.RoutingprofileDB_Filepath == null)
				{
					DB.RoutingprofileDB_Filepath = "Routingprofile";
				}
				TextWriter RoutingprofileWriter = null;
				try
				{
					RoutingprofileWriter = new StreamWriter(DB.RoutingprofileDB_Filepath);
					RoutingprofilesSerializer.Serialize(RoutingprofileWriter, Routingprofiles);
				}
				catch (IOException)
				{
					MessageBox.Show("Fileerror. Is the Routingprofiles Database used by another program?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				finally
				{
					if (RoutingprofileWriter != null)
					{
						RoutingprofileWriter.Close();
					}
				}
			}

			else if (ExtraBackup == Ratingprofiles)
			{
				if (DB.RatingprofileDB_Filepath == null)
				{
					DB.RatingprofileDB_Filepath = "Ratingprofiles";
				}

				TextWriter BewertungsprofileWriter=null;
				try
				{
					BewertungsprofileWriter = new StreamWriter(DB.RatingprofileDB_Filepath);
					RatingprofilesSerializer.Serialize(BewertungsprofileWriter, Ratingprofiles);
					return true;
				}
				catch (IOException)
				{
					MessageBox.Show("Fileerror. Is the Ratingprofiles Database used by another program?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				finally
				{

					if (BewertungsprofileWriter != null)
					{
						BewertungsprofileWriter.Close();
					}
				}
			}
            
            //Last one, so changes made in the Backup Routine can be saved
            TextWriter DBWriter = new StreamWriter(Database_Filepath);
			try
			{

				DBSerializer.Serialize(DBWriter, DB);
			}
			catch (IOException)
			{
				MessageBox.Show("Dateifehler. Wird die Datenbankdatei von einem anderen Programm verwendet?", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
            DBWriter.Close();
			return true;
        }

		#region ReadingDatabases
		public static void ReadRoutingprofiles()
		{
			Routingprofiles.Clear();
			StreamReader RPReader = null;
			if (DB.CheckDatabaseFilepath(Database.Databases.Routingprofiles))//returns true if the user has set a valid database
			{
				try
				{
					RPReader = new StreamReader(DB.RoutingprofileDB_Filepath);
					Routingprofiles = (SortableBindingList<Routingprofile>)RoutingprofilesSerializer.Deserialize(RPReader);
					RPReader.Close();
				}
				catch (Exception)
				{
					MessageBox.Show("Error in Routingdatabase!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					if (RPReader != null)
					{
						RPReader.Close();
					}
				}
			}

			//To make them show up in the menu
			Routingprofiles.ListChanged += new ListChangedEventHandler(MainWindow.Routingprofiles_ListChanged);
			Routingprofiles.ResetBindings();
			Backup(Routingprofiles);

		}

		public static void ReadRatingprofiles()
		{
			Ratingprofiles.Clear();
			StreamReader BPReader = null;
			if (DB.CheckDatabaseFilepath(Database.Databases.Ratingprofiles))//returns true if the user has set a valid database
			{
				try
				{
					BPReader = new StreamReader(DB.RatingprofileDB_Filepath);
					Ratingprofiles = (SortableBindingList<Ratingprofile>)RatingprofilesSerializer.Deserialize(BPReader);
					BPReader.Close();

				}
				catch (Exception)
				{
					MessageBox.Show("Error in Ratingdatabases!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Ratingprofiles.ListChanged += new ListChangedEventHandler(MainWindow.Ratingprofiles_ListChanged);
				}
				finally
				{
					if (BPReader != null)
					{
						BPReader.Close();
					}
				}

				//To make them show up in the menu
				Ratingprofiles.ListChanged += new ListChangedEventHandler(MainWindow.Ratingprofiles_ListChanged);
				Ratingprofiles.ResetBindings();
				Backup(Ratingprofiles);
			}
		}

		public static void ReadGeocaches()
		{
			Geocaches.Clear();
			StreamReader GCReader = null;

			if (DB.CheckDatabaseFilepath(Database.Databases.Geocaches))//returns true if the user has set a valid database
			{
				try
				{
					GCReader = new StreamReader(DB.GeocacheDB_Filepath);
					Geocaches = (SortableBindingList<Geocache>)GeocachesSerializer.Deserialize(GCReader);

					//So the MinimalRating and MaximalRating property get set and the map displays it correctly (fixes issue #4)
					Geocaches.OrderByDescending(x => x.Rating);
					DB.MaximalRating = Geocaches[0].Rating;//Possible since list is sorted
					DB.MinimalRating = Geocaches[Geocaches.Count - 1].Rating;
				}

				catch (Exception)
				{
					MessageBox.Show("Error in Geocachedatabase!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					if (GCReader != null)
					{
						GCReader.Close();
					}
				}
				Geocaches.ResetBindings();
			}
		}
		#endregion
	}
}
