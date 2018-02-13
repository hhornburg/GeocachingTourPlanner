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

			//Load Ratingprofiles from the File specified in the Database
			ReadRatingprofiles();
			//Geocaches
			ReadGeocaches();
			//Routingprofile
			ReadRoutingprofiles();

            //Tabelleneinstellungen
            MainWindow.GeocacheTable.DataSource = Geocaches;
            MainWindow.GeocacheTable.Columns["GCCODE"].DisplayIndex = 0;
            MainWindow.GeocacheTable.Columns["Name"].DisplayIndex = 1;
            MainWindow.GeocacheTable.Columns["lat"].DisplayIndex = 2;
            MainWindow.GeocacheTable.Columns["lon"].DisplayIndex = 3;
            MainWindow.GeocacheTable.Columns["Type"].DisplayIndex = 4;
            MainWindow.GeocacheTable.Columns["Size"].DisplayIndex = 5;
            MainWindow.GeocacheTable.Columns["DRating"].DisplayIndex = 6;
            MainWindow.GeocacheTable.Columns["TRating"].DisplayIndex = 7;
            MainWindow.GeocacheTable.Columns["Rating"].DisplayIndex = MainWindow.GeocacheTable.ColumnCount-1;
			//

			Application.Run(MainWindow);
        }
		

		/// <summary>
		/// Zum Erstellen des Menüs
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void Ratingprofiles_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                ToolStripMenuItem Menuitem = new ToolStripMenuItem();
                Menuitem.Text = Ratingprofiles[e.NewIndex].ToString();
                Menuitem.Click += Menuitem_Click;
                MainWindow.RatingprofilesToolStripMenuItem.DropDownItems.Insert(0,Menuitem);
            }
            else if (e.ListChangedType == ListChangedType.Reset)
            {
                
                foreach(Ratingprofile bp in Ratingprofiles)
                {
                    MainWindow.RatingprofilesToolStripMenuItem.DropDownItems.Clear();
                    ToolStripMenuItem Menuitem = new ToolStripMenuItem();
                    Menuitem.Text = bp.ToString();
                    Menuitem.Click += new EventHandler(Menuitem_Click);
                    MainWindow.RatingprofilesToolStripMenuItem.DropDownItems.Insert(0, Menuitem);
                }
                MainWindow.RatingprofilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                MainWindow.toolStripSeparator2,
                MainWindow.NewRatingprofileToolStripMenuItem});
            }
            
        }

        private static void Menuitem_Click(object sender, EventArgs e)
        {
            new NewRatingProfileWindow(Ratingprofiles.First(x=>x.Name==sender.ToString())).Show();
        }

        /// <summary>
        /// The main Database gets saved anyways specify which otherList should be saved alongside
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
                TextWriter GeocachesWriter;
				try
				{
					GeocachesWriter = new StreamWriter(DB.GeocacheDB_Filepath);
					GeocachesSerializer.Serialize(GeocachesWriter, Geocaches);
				}
				catch
				{
					MessageBox.Show("Fileerror. Is the Geocaches Database used by another program?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return false;
				}
				if (GeocachesWriter != null)
				{
					GeocachesWriter.Close();
				}
				
			}

			else if (ExtraBackup == Routingprofiles)
			{
				if (DB.RoutingDB_Filepath == null)
				{
					DB.RoutingDB_Filepath = "Routingprofile";
				}
				TextWriter RoutingprofileWriter;
				try
				{
					RoutingprofileWriter = new StreamWriter(DB.RoutingDB_Filepath);
					RoutingprofilesSerializer.Serialize(RoutingprofileWriter, Routingprofiles);
				}
				catch (IOException)
				{
					MessageBox.Show("Fileerror. Is the Routingprofiles Database used by another program?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return false;
				}
				if (RoutingprofileWriter != null)
				{
					RoutingprofileWriter.Close();
				}
			}
			else if (ExtraBackup == Ratingprofiles)
			{
				if (DB.RatingDB_Filepath == null)
				{
					DB.RatingDB_Filepath = "Ratingprofiles";
				}

				TextWriter BewertungsprofileWriter;
				try
				{
					BewertungsprofileWriter = new StreamWriter(DB.RatingDB_Filepath);
					RatingprofilesSerializer.Serialize(BewertungsprofileWriter, Ratingprofiles);
				}
				catch (IOException)
				{
					MessageBox.Show("Fileerror. Is the Ratingprofiles Database used by another program?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				if (BewertungsprofileWriter != null)
				{
					BewertungsprofileWriter.Close();
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
        
		public static void ReadRoutingprofiles()
		{
			Routingprofiles.Clear();
			StreamReader RPReader = null;
			if (DB.CheckDatabaseFilepath(DB.RoutingDB_Filepath,"Routingdatabase"))//returns true if the user has set a valid database
			{
				try
				{
					RPReader = new StreamReader(DB.RoutingDB_Filepath);
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
			
		}

		public static void ReadRatingprofiles()
		{
			Ratingprofiles.Clear();
			StreamReader BPReader = null;
			if (DB.CheckDatabaseFilepath(DB.RatingDB_Filepath, "Ratingdatabase"))//returns true if the user has set a valid database
			{
				try
				{
					BPReader = new StreamReader(DB.RatingDB_Filepath);
					Ratingprofiles = (SortableBindingList<Ratingprofile>)RatingprofilesSerializer.Deserialize(BPReader);
					BPReader.Close();

				}
				catch (Exception)
				{
					MessageBox.Show("No valid Ratingdatabases found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Ratingprofiles.ListChanged += new ListChangedEventHandler(Ratingprofiles_ListChanged);
				}
				finally
				{
					if (BPReader != null)
					{
						BPReader.Close();
					}
				}

				//To make them show up in the menu
				Ratingprofiles.ListChanged += new ListChangedEventHandler(Ratingprofiles_ListChanged);
				Ratingprofiles.ResetBindings();
				Backup(Ratingprofiles);
			}
		}

		public static void ReadGeocaches()
		{
			Geocaches.Clear();
			StreamReader GCReader = null;

			if (DB.CheckDatabaseFilepath(DB.GeocacheDB_Filepath, "Geocachedatabase"))//returns true if the user has set a valid database
			{
				try
				{
					GCReader = new StreamReader(DB.GeocacheDB_Filepath);
					Geocaches = (SortableBindingList<Geocache>)GeocachesSerializer.Deserialize(GCReader);
				}

				catch (Exception)
				{
					MessageBox.Show("No valid Geocachedatabase found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				finally
				{
					if (GCReader != null)
					{
						GCReader.Close();
					}
				}
			}
		}
	}
}
