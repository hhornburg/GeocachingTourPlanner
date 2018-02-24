﻿using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
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
		private double StartLat=0;
		private double StartLon=0;
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
			for (int i = 0; i < 10; i++)
			{
				TargetCombobox.Items.Add(Program.Geocaches[i].Rating + " - " + Program.Geocaches[i].GCCODE);
			}
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
			//They Caches not in range get kicked out at the beginning of every iteration
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
						if (ApproxDistance(GC.lat, GC.lon, StartLat, StartLon) < (SelectedProfile.MaxDistance - CurrentRoute.TotalDistance))
						{
							GeocachesInRange.Add(GC);
							NotAddedGeocaches.Remove(GC);
						}
					}
					else
					{
						if (ApproxDistance(GC.lat, GC.lon, StartLat, StartLon) < (SelectedProfile.MaxDistance))
						{
							GeocachesInRange.Add(GC);
							NotAddedGeocaches.Remove(GC);
						}
					}
				}
				foreach(Geocache RoutePoint in new List<Geocache>(NotAddedGeocaches))
				{
					foreach (Geocache GC in NotAddedGeocaches)
					{
						if (CurrentRoute != null)
						{
							if (ApproxDistance(GC.lat, GC.lon, StartLat, StartLon) < (SelectedProfile.MaxDistance - CurrentRoute.TotalDistance/1000))
							{
								GeocachesInRange.Add(GC);
								NotAddedGeocaches.Remove(GC);
							}
						}
						else
						{
							if (ApproxDistance(GC.lat, GC.lon, RoutePoint.lat, RoutePoint.lon) < (SelectedProfile.MaxDistance))
							{
								GeocachesInRange.Add(GC);
								NotAddedGeocaches.Remove(GC);
							}
						}
					}
				}

				GeocachesInRange.OrderByDescending(x => x.Rating);
				GeocachesOnRoute.Add(GeocachesInRange[0]);

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
				List<RouterPoint> PointsOnRoute = new List<RouterPoint>();
				foreach(Geocache GC in GeocachesOnRoute)
				{
					PointsOnRoute.Add(router.Resolve(SelectedProfile.ItineroProfile.profile, GC.lat, GC.lon));
				}
				CurrentRoute = router.Calculate(SelectedProfile.ItineroProfile.profile, PointsOnRoute.ToArray());
				
				//Calculate Points of Route
				foreach(Geocache GC in GeocachesOnRoute)
				{
					RoutePoints += GC.Rating;
				}
				if (CurrentRoute.TotalDistance > SelectedProfile.MaxDistance)
				{
					RoutePoints -= (CurrentRoute.TotalDistance/1000 - SelectedProfile.MaxDistance) * SelectedProfile.PenaltyPerExtraKM;
				}
				if (CurrentRoute.TotalTime + GeocachesOnRoute.Count*SelectedProfile.TimePerGeocache > SelectedProfile.MaxTime)
				{
					RoutePoints -= (CurrentRoute.TotalTime/60 - SelectedProfile.MaxTime) * SelectedProfile.PenaltyPerExtra10min / 10;
				}
				
			} while (LastRoutePoints < RoutePoints);

			List<PointLatLng> GMAPRoute = new List<PointLatLng>();
			foreach(Coordinate COO in CurrentRoute.Shape)
			{
				GMAPRoute.Add(new PointLatLng(COO.Latitude, COO.Longitude));
			}

			GMapOverlay RouteOverlay = new GMapOverlay("Route");
			RouteOverlay.Routes.Add(new GMapRoute(GMAPRoute,"Route"));
			Program.MainWindow.Map.Overlays.Add(RouteOverlay);
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

		private void Map_OnMapDrag()
		{
			StartLat = Map.Position.Lat;
			StartLon = Map.Position.Lng;
		}

		private void Dropdown_SelectedIndexChanged(object sender, EventArgs e)
		{
			((ComboBox)sender).Text = ((ComboBox)sender).SelectedItem.ToString();//So I can just check the text and it doesn't matter whether the user typed it or selected it
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
			return Math.Sqrt(Math.Abs(lat1 - lat2) * Math.Abs(lat1 - lat2) + Math.Abs(lon1 - lon2) * Math.Abs(lon1 - lon2))* 40030/360;
		}
		#endregion

		
	}
}
