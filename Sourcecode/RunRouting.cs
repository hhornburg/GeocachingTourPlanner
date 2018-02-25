using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Itinero;
using Itinero.LocalGeo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeocachingTourPlanner
{
	public partial class RunRouting : Form
	{
		private float StartLat=0;
		private float StartLon=0;
		private Routingprofile  SelectedProfile;
		private List<Geocache> GeocachesOnRoute = new List<Geocache>();
		private float RoutePoints = 0;
		private float LastRoutePoints = 0;
		private Route CurrentRoute;

		public RunRouting()
		{
			InitializeComponent();
			foreach(Routingprofile RP in Program.Routingprofiles)
			{
				ProfilesCombobox.Items.Add(RP.Name);
			}
			TargetCombobox.Text = "Select Startingpoint and Profile first";
			TargetCombobox.Enabled = false;
		}

		private void StartRatingButton_Click(object sender, EventArgs e)
		{
			//Get selected values
			try
			{
				if (StartLat == 0 || StartLat == 0)
				{
					MessageBox.Show("Please select a Startingpoint from the map", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				SelectedProfile = Program.Routingprofiles.First(x => x.Name == ProfilesCombobox.Text);
				string SelectedGCCODE = TargetCombobox.Text.Remove(0, TargetCombobox.Text.LastIndexOf(" ")+1);
				GeocachesOnRoute.Add(Program.Geocaches.First(x => x.GCCODE == SelectedGCCODE));
			}
			catch (Exception)
			{
				MessageBox.Show("Please select values from the Dropdownlist only", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//Create Copy of Geocaches
			List<Geocache> NotAddedGeocaches = new List<Geocache>();
			//The Caches not in range get kicked out at the beginning of every iteration
			List<Geocache> GeocachesInRange = new List<Geocache>(Program.Geocaches);
			GeocachesInRange.Remove(GeocachesOnRoute[0]);//The Target Geocache should be removed, as it is no new target

			do
			{
				//Create Copy of Geocaches
				NotAddedGeocaches = new List<Geocache>(GeocachesInRange);
				GeocachesInRange = new List<Geocache>();
				//Remember Points of last route
				LastRoutePoints = RoutePoints;

				//Remove all Geocaches out of reach of StartingPoint
				foreach (Geocache GC in new List<Geocache>(NotAddedGeocaches))
				{
					if (CurrentRoute != null)
					{
						if (ApproxDistance(GC.lat, GC.lon, StartLat, StartLon) <= (SelectedProfile.MaxDistance - CurrentRoute.TotalDistance/1000)/2)//As you have to geet there and back again
						{
							GeocachesInRange.Add(GC);
							NotAddedGeocaches.Remove(GC);
						}
					}
					else
					{
						if (ApproxDistance(GC.lat, GC.lon, StartLat, StartLon) <= (SelectedProfile.MaxDistance)/2)//As you have to geet there and back again
						{
							GeocachesInRange.Add(GC);
							NotAddedGeocaches.Remove(GC);
						}
					}
				}
				foreach(Geocache RoutePoint in GeocachesOnRoute)
				{
					foreach (Geocache GC in new List<Geocache>(NotAddedGeocaches))
					{
						if (CurrentRoute != null)
						{
							if (ApproxDistance(GC.lat, GC.lon, StartLat, StartLon) < (SelectedProfile.MaxDistance - CurrentRoute.TotalDistance/1000)/2)
							{
								GeocachesInRange.Add(GC);
								NotAddedGeocaches.Remove(GC);
							}
						}
						else
						{
							if (ApproxDistance(GC.lat, GC.lon, RoutePoint.lat, RoutePoint.lon) < (SelectedProfile.MaxDistance)/2)
							{
								GeocachesInRange.Add(GC);
								NotAddedGeocaches.Remove(GC);
							}
						}
					}
				}

				if (GeocachesInRange.Count != 0)
				{
					GeocachesInRange.OrderByDescending(x => x.Rating);
					GeocachesOnRoute.Add(GeocachesInRange[0]);
					GeocachesInRange.RemoveAt(0);

					if (Program.RouterDB.IsEmpty)
					{
						if (Program.DB.RouterDB_Filepath != null)
						{
							using (var stream = new FileInfo(Program.DB.RouterDB_Filepath).OpenRead())
							{
								Program.RouterDB = RouterDb.Deserialize(stream);
							}
						}
						else
						{
							MessageBox.Show("Import or set RouterDB before creating route!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}

					Router router = new Router(Program.RouterDB);

					//Make the Points which the route should pass
					List<RouterPoint> PointsOnRoute = new List<RouterPoint>();
					try
					{
						PointsOnRoute.Add(router.Resolve(SelectedProfile.ItineroProfile.profile, StartLat, StartLon));
					}
					catch (Itinero.Exceptions.ResolveFailedException)
					{
						MessageBox.Show("Please select a Startingpoint close to a road");
						return;
					}

					foreach (Geocache GC in new List<Geocache>(GeocachesOnRoute))
					{
						try
						{
							PointsOnRoute.Add(router.Resolve(SelectedProfile.ItineroProfile.profile, GC.lat, GC.lon));
						}
						catch (Itinero.Exceptions.ResolveFailedException)
						{
							GeocachesOnRoute.Remove(GC);//As it is not reachable
						}
					}


					PointsOnRoute.Add(router.Resolve(SelectedProfile.ItineroProfile.profile, StartLat, StartLon));//As start is currently also the End

					//Calculate Route
					try
					{
						CurrentRoute = router.Calculate(SelectedProfile.ItineroProfile.profile, PointsOnRoute.ToArray());
					}
					catch (Itinero.Exceptions.RouteNotFoundException)
					{
						//Route creation error, Itinero intern problem
						GeocachesOnRoute.RemoveAt(GeocachesOnRoute.Count - 1);//As the last geocache hasn't been fitted into the Route. From List of Geocaches in Range should remain, as this one is causing trouble.
																			  //Effectively, this causes it to take the current route. As far as seen until now, not a too big problem.
					}

					//Calculate Points of Route
					RoutePoints = 0;
					foreach (Geocache GC in GeocachesOnRoute)
					{
						RoutePoints += GC.Rating;
					}
					if (CurrentRoute.TotalDistance / 1000 > SelectedProfile.MaxDistance)
					{
						RoutePoints -= (CurrentRoute.TotalDistance / 1000 - SelectedProfile.MaxDistance) * SelectedProfile.PenaltyPerExtraKM;
					}
					if (CurrentRoute.TotalTime / 60 + GeocachesOnRoute.Count * SelectedProfile.TimePerGeocache > SelectedProfile.MaxTime)
					{
						RoutePoints -= (CurrentRoute.TotalTime / 60 - SelectedProfile.MaxTime) * SelectedProfile.PenaltyPerExtra10min / 10;
					}
				}
			} while (GeocachesInRange.Count>0 && LastRoutePoints <= RoutePoints);

			//Name of the route which will be used for all further referencing
			string Routetag = SelectedProfile.Name + " Route " + (SelectedProfile.RoutesOfthisType + 1);

			Program.Routes.Add(new KeyValueTriple<string, Route,List<Geocache>>(Routetag,CurrentRoute,GeocachesOnRoute));
			List<PointLatLng> GMAPRoute = new List<PointLatLng>();
			
			foreach(Coordinate COO in CurrentRoute.Shape)
			{
				GMAPRoute.Add(new PointLatLng(COO.Latitude, COO.Longitude));
			}

			
			SelectedProfile.RoutesOfthisType++;

			GMapOverlay RouteOverlay = new GMapOverlay(Routetag);
			RouteOverlay.Routes.Add(new GMapRoute(GMAPRoute, Routetag));
			foreach (Geocache GC in GeocachesOnRoute)
			{
				GMapMarker GCMarker = null;
				//Three Categories => Thirds of the Point range
				if (GC.Rating > (Program.DB.MinimalRating) + 0.66 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
				{
					GCMarker = new GMarkerGoogle(new PointLatLng(GC.lat, GC.lon), GMarkerGoogleType.green_small);
					RouteOverlay.Markers.Add(GCMarker);
				}
				else if (GC.Rating > (Program.DB.MinimalRating) + 0.33 * (Program.DB.MaximalRating - Program.DB.MinimalRating))
				{
					GCMarker = new GMarkerGoogle(new PointLatLng(GC.lat, GC.lon), GMarkerGoogleType.yellow_small);
					RouteOverlay.Markers.Add(GCMarker);
				}
				else
				{
					GCMarker = new GMarkerGoogle(new PointLatLng(GC.lat, GC.lon), GMarkerGoogleType.red_small);
					RouteOverlay.Markers.Add(GCMarker);
				}

				GCMarker.ToolTipText = GC.GCCODE + "\n" + GC.Name + "\n" + GC.Type + "(" + GC.DateHidden.Date.ToString().Remove(10) + ")\nD-Wertung: " + GC.DRating + "\nT-Wertung: " + GC.TRating + "\nBewertung: " + GC.Rating;
				GCMarker.Tag = GC.GCCODE;
			}
			Program.MainWindow.Map.Overlays.Add(RouteOverlay);
			Program.MainWindow.newRouteControlElement(Routetag);
			Close();
			
			
		}

		private void CancelRatingButton_Click(object sender, EventArgs e)
		{
			Close();
		}
		
		private void Map_Load(object sender, EventArgs e)
		{
			Map.MapProvider = OpenCycleLandscapeMapProvider.Instance;
			GMaps.Instance.Mode = AccessMode.ServerOnly;
			//Set Views
			if (Program.DB.LastMapZoom == 0)
			{
				Program.DB.LastMapZoom = 5;
			}
			Map.Zoom = Program.DB.LastMapZoom;

			if (Program.DB.LastMapPosition.IsEmpty)//Equals that the user hasn't seen the map before (fixes #2)
			{
				Program.DB.LastMapPosition = new PointLatLng(49.0, 8.5);
			}
			Map.Position = Program.DB.LastMapPosition;
		}

		bool Mapdragging = false;
		private void Map_OnMapDrag()
		{
			Mapdragging = true;
		}

		private void Map_OnMapZoomChanged()
		{
			if (StartLat != Map.Position.Lat)
			{
				StartLat = (float)Map.Position.Lat;
				StartLon = (float)Map.Position.Lng;
				SetBestGeocachesInReach();
			}
		}

		private void Map_MouseUp(object sender, MouseEventArgs e)
		{
			if (Mapdragging)//So calculation only kicks in if dragging is over
			{
				StartLat = (float)Map.Position.Lat;
				StartLon = (float)Map.Position.Lng;
				SetBestGeocachesInReach();
				Mapdragging = false;
			}
		}

		private void Dropdown_SelectedIndexChanged(object sender, EventArgs e)
		{
			((ComboBox)sender).Text = ((ComboBox)sender).SelectedItem.ToString();//So I can just check the text and it doesn't matter whether the user typed it or selected it
		}
		
		private void ProfilesCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			((ComboBox)sender).Text = ((ComboBox)sender).SelectedItem.ToString();//So I can just check the text and it doesn't matter whether the user typed it or selected it
			SelectedProfile = Program.Routingprofiles.First(x => x.Name == ProfilesCombobox.Text);
			SetBestGeocachesInReach();
		}
		/// <summary>
		/// Sets the dropdownlist of target geocaches to the best geocaches in reach
		/// </summary>
		private void SetBestGeocachesInReach()
		{
			if (StartLon == 0|| SelectedProfile == null)//Coordinates change together
			{
				return;
			}
			
			List<Geocache> PossibleGeocaches = new List<Geocache>();
			foreach (Geocache GC in Program.Geocaches)
			{
				if (ApproxDistance(GC.lat, GC.lon, StartLat, StartLon) < (SelectedProfile.MaxDistance))
				{
					PossibleGeocaches.Add(GC);
				}
				if (PossibleGeocaches.Count == 10)
				{
					break;
				}
			}

			TargetCombobox.Enabled = true;

			PossibleGeocaches.OrderByDescending(x => x.Rating);
			TargetCombobox.Items.Clear();
			for (int i = 0; i < 10; i++)
			{
				TargetCombobox.Items.Add(PossibleGeocaches[i].Rating + " - " + PossibleGeocaches[i].GCCODE);
			}
		}
		#region Helperfunctions for Routing
		/// <summary>
		/// Returns approximate distance in Km
		/// </summary>
		/// <param name="lat1"></param>
		/// <param name="lon1"></param>
		/// <param name="lat2"></param>
		/// <param name="lon2"></param>
		/// <returns></returns>
		private double ApproxDistance(double lat1, double lon1, double lat2, double lon2)
		{
			//Approximation for short distances
			double distance = Math.Sqrt(Math.Abs(lat1 - lat2) * Math.Abs(lat1 - lat2) + Math.Abs(lon1 - lon2) * Math.Abs(lon1 - lon2)) * 40030 / 360;
			return distance;
		}



		#endregion

		
	}
}
