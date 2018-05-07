﻿using Itinero;
using Itinero.IO.Osm;
using Itinero.LocalGeo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GeocachingTourPlanner
{
	class Fileoperations
	{
		static XmlSerializer DBSerializer = new XmlSerializer(typeof(Database));
		static XmlSerializer RoutingprofilesSerializer = new XmlSerializer(typeof(SortableBindingList<Routingprofile>));
		static XmlSerializer RatingprofilesSerializer = new XmlSerializer(typeof(SortableBindingList<Ratingprofile>));
		static XmlSerializer GeocachesSerializer = new XmlSerializer(typeof(SortableBindingList<Geocache>));

		public static void ReadMainDatabase()
		{
			StreamReader DBReader = null;
			try
			{
				DBReader = new StreamReader(Program.Database_Filepath);
				Program.DB = (Database)DBSerializer.Deserialize(DBReader);
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

		/// <summary>
		/// Reads routingprofiles from the filepath specified in the database. Only checks wether it is set, but takes no action if not
		/// </summary>
		public static void ReadRoutingprofiles()
		{
			Program.Routingprofiles.Clear();
			StreamReader RPReader = null;
			if (Program.DB.IsFilepathSet(Databases.Routingprofiles))//returns true if the user has set a valid database
			{
				try
				{
					RPReader = new StreamReader(Program.DB.RoutingprofileDB_Filepath);
					Program.Routingprofiles = (SortableBindingList<Routingprofile>)RoutingprofilesSerializer.Deserialize(RPReader);
					//Deserializing makes the lists lose their bindings
					Startup.BindLists();
					RPReader.Close();

					Program.MainWindow.UpdateStatus("Successfully read routingprofiles");
					Program.Routingprofiles.ResetBindings();
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

		/// <summary>
		/// Reads ratingprofiles from file specified in the database. Only checks wether it is set, but takes no action if not
		/// </summary>
		public static void ReadRatingprofiles()
		{
			Program.Ratingprofiles.Clear();
			StreamReader BPReader = null;
			if (Program.DB.IsFilepathSet(Databases.Ratingprofiles))//returns true if the user has set a valid database
			{
				try
				{
					BPReader = new StreamReader(Program.DB.RatingprofileDB_Filepath);
					Program.Ratingprofiles= (SortableBindingList<Ratingprofile>)RatingprofilesSerializer.Deserialize(BPReader);
					//Deserializing makes the lists lose their bindings
					Startup.BindLists();
					BPReader.Close();

					Program.MainWindow.UpdateStatus("Successfully read ratingprofiles");
					Program.Ratingprofiles.ResetBindings();
				}
				catch (Exception)
				{
					MessageBox.Show("Error in Ratingdatabases!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					if (BPReader != null)
					{
						BPReader.Close();
					}
				}
			}
		}

		/// <summary>
		/// Reads geocaches from database file specified in the main database. Only checks wether it is set, but takes no action if not
		/// </summary>
		public static void ReadGeocaches()
		{
			Program.Geocaches.Clear(); 
			StreamReader GCReader = null;

			if (Program.DB.IsFilepathSet(Databases.Geocaches))//returns true if the user has set a valid database
			{
				try
				{
					GCReader = new StreamReader(Program.DB.GeocacheDB_Filepath);
					Program.Geocaches = (SortableBindingList<Geocache>)GeocachesSerializer.Deserialize(GCReader);

					//So the MinimalRating and MaximalRating property get set and the map displays it correctly (fixes issue #4)
					Program.Geocaches.OrderByDescending(x => x.Rating);
					Program.DB.MaximalRating = Program.Geocaches[0].Rating;//Possible since list is sorted
					Program.DB.MinimalRating = Program.Geocaches[Program.Geocaches.Count - 1].Rating;

					Program.MainWindow.UpdateStatus("Successfully read geocaches");

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
				Program.Geocaches.ResetBindings();
				Program.MainWindow.GeocachesStateLabel.Text = Program.Geocaches.Count + " Geocaches loaded";
			}
		}

		/// <summary>
		/// Reads the set RouterDB in a new thread
		/// </summary>
		public static void ReadRouterDB()
		{
			new Thread(new ThreadStart(() =>
			{
				try
				{
					Program.MainWindow.UpdateStatus("Loading RouterDB in progress", 99);
					using (var stream = new FileInfo(Program.DB.RouterDB_Filepath).OpenRead())
					{
						Program.RouterDB = RouterDb.Deserialize(stream);
					}
					Backup(null);

					Program.MainWindow.SetRouterDBLabel("Successfully loaded RouterDB");
					Program.MainWindow.UpdateStatus("Successfully loaded RouterDB", 100);
				}
				catch (Exception)
				{
					MessageBox.Show("Failed to read RouterDB", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			})).Start();
		}

		/// <summary>
		/// The main Database gets saved anyways specify which otherList should be saved alongside. Returns true on success
		/// </summary>
		/// <param name="ExtraBackup"></param>
		public static bool Backup(object ExtraBackup)
		{
			//Aus Performancegrnden nicht alles
			if (ExtraBackup == Program.Geocaches)
			{

				if (Program.DB.IsFilepathSet(Databases.Geocaches))
				{
					TextWriter GeocachesWriter = null;
					try
					{
						GeocachesWriter = new StreamWriter(Program.DB.GeocacheDB_Filepath);
						GeocachesSerializer.Serialize(GeocachesWriter, Program.Geocaches);
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
			}

			else if (ExtraBackup == Program.Routingprofiles)
			{
				if (Program.DB.IsFilepathSet(Databases.Routingprofiles))
				{
					TextWriter RoutingprofileWriter = null;
					try
					{
						RoutingprofileWriter = new StreamWriter(Program.DB.RoutingprofileDB_Filepath);
						RoutingprofilesSerializer.Serialize(RoutingprofileWriter, Program.Routingprofiles);
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
			}

			else if (ExtraBackup == Program.Ratingprofiles)
			{
				if (Program.DB.IsFilepathSet(Databases.Ratingprofiles))
				{
					TextWriter RatingprofileWriter = null;
					try
					{
						RatingprofileWriter = new StreamWriter(Program.DB.RatingprofileDB_Filepath);
						RatingprofilesSerializer.Serialize(RatingprofileWriter, Program.Ratingprofiles);
					}
					catch (IOException)
					{
						MessageBox.Show("Fileerror. Is the Ratingprofiles Database used by another program?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					finally
					{

						if (RatingprofileWriter != null)
						{
							RatingprofileWriter.Close();
						}
					}
				}
			}

			//Last one, so changes made in the Backup Routine can be saved
			TextWriter DBWriter = new StreamWriter(Program.Database_Filepath);
			try
			{DBSerializer.Serialize(DBWriter, Program.DB);
			}
			catch (IOException)
			{
				MessageBox.Show("Fileerror. Is the Main Database used by another program?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			DBWriter.Close();
			return true;
		}

		public static void ExportGPX(string OverlayTag)
		{
			SaveFileDialog StandardFileDialog = new SaveFileDialog
			{
				InitialDirectory = Program.DB.LastUsedFilepath,
				Filter = "gpx files (*.gpx)|*.gpx|All files (*.*)|*.*",
				FilterIndex = 0,
				RestoreDirectory = true,
				Title = "Export route as track"
			};
			if (StandardFileDialog.ShowDialog() == DialogResult.OK)
			{

				KeyValuePair<string, Tourplanning.RouteData> RouteToSeialize = Program.Routes.First(x => x.Key == OverlayTag);

				XmlDocument GPX = new XmlDocument();

				XmlDeclaration xmldecl = GPX.CreateXmlDeclaration("1.0", "UTF-8", null);

				XmlNode root = GPX.CreateElement("gpx");
				GPX.AppendChild(root);

				XmlAttribute xmlns = GPX.CreateAttribute("xmlns");
				xmlns.Value = "http://www.topografix.com/GPX/1/1";
				root.Attributes.Append(xmlns);

				XmlAttribute version = GPX.CreateAttribute("version");
				version.Value = "1.1";
				root.Attributes.Append(version);

				XmlAttribute creator = GPX.CreateAttribute("creator");
				creator.Value = "GeocachingTourPlanner";
				root.Attributes.Append(creator);

				foreach (Geocache GC in RouteToSeialize.Value.GeocachesOnRoute())
				{
					XmlElement wpt = GPX.CreateElement("wpt");
					//Coordinates
					XmlAttribute latitude = GPX.CreateAttribute("lat");
					latitude.Value = GC.lat.ToString(CultureInfo.InvariantCulture);
					XmlAttribute longitude = GPX.CreateAttribute("lon");
					longitude.Value = GC.lon.ToString(CultureInfo.InvariantCulture);
					wpt.Attributes.Append(latitude);
					wpt.Attributes.Append(longitude);

					//Name
					XmlElement gcname = GPX.CreateElement("name");
					gcname.InnerText = GC.Name;
					wpt.AppendChild(gcname);
					//link
					XmlElement link = GPX.CreateElement("link");
					XmlAttribute linkattribute = GPX.CreateAttribute("href");
					linkattribute.Value = "https://www.coord.info/" + GC.GCCODE;
					link.Attributes.Append(linkattribute);
					wpt.AppendChild(link);

					root.AppendChild(wpt);
				}

				XmlNode track = GPX.CreateElement("trk");
				root.AppendChild(track);

				//Name of track
				XmlNode name = GPX.CreateElement("name");
				name.InnerText = RouteToSeialize.Key;
				track.AppendChild(name);

				XmlNode tracksegment = GPX.CreateElement("trkseg");

				Route FinalRoute = RouteToSeialize.Value.partialRoutes[0].partialRoute;

				for (int i = 1; i < RouteToSeialize.Value.partialRoutes.Count; i++)
				{
					FinalRoute = FinalRoute.Concatenate(RouteToSeialize.Value.partialRoutes[i].partialRoute);
				}

				foreach (Coordinate COO in FinalRoute.Shape)
				{
					XmlNode trackpoint = GPX.CreateElement("trkpt");

					//Coordinates
					XmlAttribute latitude = GPX.CreateAttribute("lat");
					latitude.Value = COO.Latitude.ToString(CultureInfo.InvariantCulture);
					XmlAttribute longitude = GPX.CreateAttribute("lon");
					longitude.Value = COO.Longitude.ToString(CultureInfo.InvariantCulture);

					trackpoint.Attributes.Append(latitude);
					trackpoint.Attributes.Append(longitude);
					tracksegment.AppendChild(trackpoint);
				}
				track.AppendChild(tracksegment);

				GPX.InsertBefore(xmldecl, root);
				GPX.Save(StandardFileDialog.FileName);
			}
		}

		public static void ImportGeocaches()
		{
			string GPXDatei;
			OpenFileDialog SelectGPXFileDialog = new OpenFileDialog
			{
				InitialDirectory = Program.DB.LastUsedFilepath,
				Filter = "gpx files (*.gpx)|*.gpx|All files (*.*)|*.*",
				FilterIndex = 0,
				RestoreDirectory = true,
				Title = "Import geocaches"
			};

			if (SelectGPXFileDialog.ShowDialog() == DialogResult.OK)
			{
				DialogResult Importmodus = MessageBox.Show("Should the Geocaches be loaded into a new Database?", "Import", MessageBoxButtons.YesNoCancel);
				if (Importmodus == DialogResult.Yes)
				{
					//Create a new File
					SaveFileDialog NewFileDialog = new SaveFileDialog
					{
						InitialDirectory = Program.DB.LastUsedFilepath,
						Filter = "gcdb files (*.gcdb)|*.gcdb|All files (*.*)|*.*",
						FilterIndex = 0,
						RestoreDirectory = true,
						Title = "Create new, empty geocachedatabase"
					};

					if (NewFileDialog.ShowDialog() == DialogResult.OK)
					{
						//Save the curent geocaches to the current file, so no data is lost. Nothing happens if there are no geocaches
						Backup(Program.Geocaches);

						//Create new database File
						File.Create(NewFileDialog.FileName).Close();
						Program.Geocaches.Clear();
						Program.DB.GeocacheDB_Filepath = NewFileDialog.FileName;
					}
				}
				else if (Importmodus == DialogResult.No)
				{
					//Do nothing
				}
				else // a.k.a cancel
				{
					return;
				}

				try
				{
					GPXDatei = SelectGPXFileDialog.FileName;
					Program.DB.LastUsedFilepath = SelectGPXFileDialog.FileName;
					XElement RootElement = XElement.Load(GPXDatei);

					XNamespace gpx = "http://www.topografix.com/GPX/1/0"; //default namespace
					XNamespace groundspeak = "http://www.groundspeak.com/cache/1/0/1";

					foreach (XElement elem in RootElement.Elements(gpx + "wpt"))
					{
						XElement CacheDetails = elem.Element(groundspeak + "cache");
						if (CacheDetails != null)//Thus the waipoint is a geocache
						{
							Geocache geocache = new Geocache { };

							geocache.lat = float.Parse(elem.FirstAttribute.ToString().Replace("\"", "").Replace("lat=", ""), CultureInfo.InvariantCulture);
							geocache.lon = float.Parse(elem.LastAttribute.ToString().Replace("\"", "").Replace("lon=", ""), CultureInfo.InvariantCulture);
							geocache.GCCODE = (string)elem.Element(gpx + "name").Value;
							geocache.Name = (string)elem.Element(gpx + "urlname").Value;
							geocache.DRating = float.Parse(CacheDetails.Element(groundspeak + "difficulty").Value.ToString(), CultureInfo.InvariantCulture);
							geocache.TRating = float.Parse(CacheDetails.Element(groundspeak + "terrain").Value.ToString(), CultureInfo.InvariantCulture);
							geocache.NeedsMaintenance = CacheDetails.Element(groundspeak + "attributes").Elements().ToList().Exists(x => x.FirstAttribute.Value == 42.ToString());
							geocache.DateHidden = DateTime.Parse(elem.Element(gpx + "time").Value.ToString());
							switch (elem.Element(gpx + "type").Value)
							{
								case "Geocache|Unknown Cache":
									geocache.Type = GeocacheType.Mystery;
									break;
								case "Geocache|Traditional Cache":
									geocache.Type = GeocacheType.Traditional;
									break;
								case "Geocache|Multi-cache":
									geocache.Type = GeocacheType.Multi;
									break;
								case "Geocache|Virtual Cache":
									geocache.Type = GeocacheType.Virtual;
									break;
								case "Geocache|Wherigo Cache":
									geocache.Type = GeocacheType.Wherigo;
									break;
								case "Geocache|Webcam Cache":
									geocache.Type = GeocacheType.Webcam;
									break;
								case "Geocache|Letterbox Hybrid":
									geocache.Type = GeocacheType.Letterbox;
									break;
								case "Geocache|Earthcache":
									geocache.Type = GeocacheType.EarthCache;
									break;
								case "Geocache|Mega-Event Cache":
									geocache.Type = GeocacheType.MegaEvent;
									break;
								case "Geocache|Event Cache":
									geocache.Type = GeocacheType.Event;
									break;
								case "Geocache|Cache In Trash Out Event":
									geocache.Type = GeocacheType.Cito;
									break;
								default:
									geocache.Type = GeocacheType.Other;
									break;
							}
							switch (CacheDetails.Element(groundspeak + "container").Value)
							{
								case "Large":
									geocache.Size = GeocacheSize.Large;
									break;
								case "Regular":
									geocache.Size = GeocacheSize.Regular;
									break;
								case "Small":
									geocache.Size = GeocacheSize.Small;
									break;
								case "Micro":
									geocache.Size = GeocacheSize.Micro;
									break;
								default:
									geocache.Size = GeocacheSize.Other;
									break;
							}
							if (!Program.Geocaches.Any(x => x.GCCODE == geocache.GCCODE))
							{
								Program.Geocaches.Add(geocache);
							}
							else
							{
								//Nothing in the moment, would be good if it would update the Geocaches
							}


							Program.MainWindow.UpdateStatus("Successfully imported geocaches");
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
					Program.MainWindow.UpdateStatus("Import of geocaches failed");
				}
				Backup(Program.Geocaches);
				Program.Geocaches.ResetBindings();
			}
		}

		public static void ImportOSMData()
		{
			OpenFileDialog StandardFileDialog = new OpenFileDialog
			{
				InitialDirectory = Program.DB.LastUsedFilepath,
				Filter = "pbf files (*.pbf)|*.pbf|All files (*.*)|*.*",
				FilterIndex = 0,
				RestoreDirectory = true,
				Title = "Import OSM Data"
			};

			if (StandardFileDialog.ShowDialog() == DialogResult.OK)
			{
				//Create a new File
				SaveFileDialog NewFileDialog = new SaveFileDialog
				{
					InitialDirectory = Program.DB.LastUsedFilepath,
					Filter = "Routerdb files (*.routerdb)|*.routerdb|All files (*.*)|*.*",
					FilterIndex = 0,
					RestoreDirectory = true,
					Title = "Create new Routerdb file"
				};

				if (NewFileDialog.ShowDialog() == DialogResult.OK)
				{
					File.Create(NewFileDialog.FileName).Close();
					Backup(null);
					Program.RouterDB = new RouterDb();
					Program.DB.RouterDB_Filepath = NewFileDialog.FileName;


					MessageBox.Show("This might take a while, depending on how big your pbf file is.\n How about getting yourself a coffee?");

					new Thread(new ThreadStart(() =>
					{
						Program.ImportOfOSMDataRunning = true;
						try
						{
							using (var stream = new FileInfo(StandardFileDialog.FileName).OpenRead())
							{
								Program.MainWindow.UpdateStatus("Import of OSM Data in progress", 99);
								Program.RouterDB.LoadOsmData(stream, new Itinero.Profiles.Vehicle[] { Itinero.Osm.Vehicles.Vehicle.Bicycle, Itinero.Osm.Vehicles.Vehicle.Car, Itinero.Osm.Vehicles.Vehicle.Pedestrian });
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
							Program.ImportOfOSMDataRunning = false;
							Program.MainWindow.UpdateStatus("Import of OSM Data failed",100);
							return;
						}
						Program.MainWindow.UpdateStatus("Import of OSM Data finished", 100);
						Program.ImportOfOSMDataRunning = false;

						// write the routerdb to disk.
						if (Program.DB.RouterDB_Filepath == null||Program.DB.RouterDB_Filepath=="")
						{
							Program.DB.OpenExistingDBFile(Databases.RouterDB);
						}

						Task Serialize = Task.Factory.StartNew(() =>
						{
						//just let it run in background
						using (var stream = new FileInfo(Program.DB.RouterDB_Filepath).Open(FileMode.Create))
							{
								Program.RouterDB.Serialize(stream);
							}
						});

						Backup(null);

						Program.MainWindow.SetRouterDBLabel("RouterDB set");
						MessageBox.Show("Successfully imported OSM Data");

					})).Start();
				}
			}
		}

		public static void NewRoutingprofileDatabase()
		{
			SaveFileDialog StandardFileDialog = new SaveFileDialog
			{
				InitialDirectory = Program.DB.LastUsedFilepath,
				Filter = "routingprofile files (*.routingprf)|*.routingprf|All files (*.*)|*.*",
				FilterIndex = 0,
				RestoreDirectory = true,
				Title = "Create new, empty routingprofilesdatabase"
			};

			if (StandardFileDialog.ShowDialog() == DialogResult.OK)
			{
				File.Create(StandardFileDialog.FileName);
				Backup(Program.Routingprofiles);
				Program.Routingprofiles.Clear();
				Program.DB.RoutingprofileDB_Filepath = StandardFileDialog.FileName;
			}

			Program.MainWindow.UpdateStatus("Created new Routingprofiledatabase");
			Program.Routingprofiles.ResetBindings();
		}

		public static void NewRatingprofileDatabase()
		{
			SaveFileDialog StandardFileDialog = new SaveFileDialog
			{
				InitialDirectory = Program.DB.LastUsedFilepath,
				Filter = "ratingprofiles files (*.ratingprf)|*.ratingprf|All files (*.*)|*.*",
				FilterIndex = 0,
				RestoreDirectory = true,
				Title = "Create new, empty ratingprofilesdatabase"
			};

			bool retry = false;
			do
			{
				retry = false;
				if (StandardFileDialog.ShowDialog() == DialogResult.OK)
				{
					File.Create(StandardFileDialog.FileName);
					Backup(Program.Ratingprofiles);
					Program.Ratingprofiles.Clear();
					Program.DB.RatingprofileDB_Filepath = StandardFileDialog.FileName;
				}
			} while (retry);

			Program.MainWindow.UpdateStatus("Created new Routingprofledatabase");
			Program.Ratingprofiles.ResetBindings();
		}

		public static class Routerlog
		{
			public static void AddMainInformation(string Message)
			{
				File.AppendAllText("Routerlog.txt", "[" + DateTime.Now + "]:\t" + Message + "\n");
			}

			public static void AddSubInformation(string Message)
			{
				File.AppendAllText("Routerlog.txt", "[" + DateTime.Now + "]:\t\t" + Message + "\n");
			}

			public static void AddMainSection(string Message)
			{
				File.AppendAllText("Routerlog.txt", "[" + DateTime.Now + "]:\t====================" + Message + "====================\n");
			}

			public static void AddSubSection(string Message)
			{
				File.AppendAllText("Routerlog.txt", "[" + DateTime.Now + "]:\t==========" + Message + "==========\n");
			}

			public class LogCollector
			{
				private StringBuilder Logs = new StringBuilder();

				/// <summary>
				/// Used when parallel threads are running to make sure logs are sensible
				/// </summary>
				/// <param name="ThreadName"></param>
				public LogCollector(string ThreadName)
				{
					Logs.Append("[" + DateTime.Now + "]:\t-----Log for Thread " + ThreadName + "---------------");
				}

				public void AddMainInformation(string Message)
				{
					Logs.Append("[" + DateTime.Now + "]:\t|" + Message + "\n");
				}

				public void AddSubInformation(string Message)
				{
					Logs.Append("[" + DateTime.Now + "]:\t|\t" + Message + "\n");
				}

				public void AddMainSection(string Message)
				{
					Logs.Append("[" + DateTime.Now + "]:\t|====================" + Message + "====================\n");
				}

				public void AddSubSection(string Message)
				{
					Logs.Append("[" + DateTime.Now + "]:\t|==========" + Message + "==========\n");
				}
				/// <summary>
				/// Writes Logs to file
				/// </summary>
				public void Write()
				{
					Logs.Append("[" + DateTime.Now + "]:\t-----End of Log for this Thread---------------\n");
					File.AppendAllText("Routerlog.txt", Logs.ToString());
				}
			} 

		}
	}
}
