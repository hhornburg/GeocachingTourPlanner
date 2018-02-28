using Itinero;
using Itinero.LocalGeo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeocachingTourPlanner
{
	class Tourplanning
	{
		/// <summary>
		/// Returns null if calculation fails
		/// </summary>
		/// <param name="router"></param>
		/// <param name="profile"></param>
		/// <param name=""></param>
		/// <returns></returns>
		public static KeyValuePair<Route, List<Geocache>> GetRoute(Routingprofile profile, List<Geocache> AllGeocaches, Coordinate Startpoint, Coordinate Endpoint, List<Geocache> GeocachesToInclude)
		{
			StringBuilder Log = new StringBuilder();
			DateTime StartTime = DateTime.Now;
			Log.AppendLine("\n  ==========New Route Calculation========== \n");
			Log.AppendLine("Current Time:" + StartTime);
			Log.AppendLine("Profile: " + profile.Name);

			#region Create Routers
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
					Application.UseWaitCursor = false;
					return null;
				}
			}
			//One for every thread
			Router Router1 = new Router(Program.RouterDB);
			Router Router2 = new Router(Program.RouterDB);

			#endregion

			List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>> RoutingData = new List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>>();
			List<Geocache> GeocachesOnRoute = new List<Geocache>();

			RouterPoint Startpoint_RP;
			RouterPoint Endpoint_RP;
			Route InitialRoute;
			List<KeyValueTriple<Geocache, float, RouterPoint>> InitialRouteGeocaches = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
			float CurrentRouteDistance;
			float CurrentRouteTime;

			try
			{
				Startpoint_RP = Router1.Resolve(profile.ItineroProfile.profile, Startpoint);
			}
			catch (Itinero.Exceptions.ResolveFailedException)
			{
				MessageBox.Show("Please select a Startingpoint close to a road");
				Application.UseWaitCursor = false;
				return null;
			}
			try
			{
				Endpoint_RP = Router1.Resolve(profile.ItineroProfile.profile, Endpoint);
			}
			catch (Itinero.Exceptions.ResolveFailedException)
			{
				MessageBox.Show("Please select an Endpoint close to a road");
				Application.UseWaitCursor = false;
				return null;
			}

			//Calculate initial Route
			try
			{
				InitialRoute = Router1.Calculate(profile.ItineroProfile.profile, Startpoint_RP, Endpoint_RP);
			}
			catch (Itinero.Exceptions.RouteNotFoundException)
			{
				MessageBox.Show("Can't calculate a route between start and endpoint. Please change your selection");
				Application.UseWaitCursor = false;
				return null;
			}
			CurrentRouteDistance = InitialRoute.TotalDistance;
			CurrentRouteTime = InitialRoute.TotalTime;

			#region resolving
			DateTime ResolvingTime = DateTime.Now;
			List<Geocache> GeocachesNotAlreadyUsed = new List<Geocache>(AllGeocaches);
			//NO Parallelisation of this part, since then synchronisation would be necessary to prevent Geocaches from appearing multiple times in the Range of the Initail route which consists only of one segment
			for (int i = 0; i < InitialRoute.Shape.Length; i += Program.DB.EveryNthShapepoint)
			{
				List<Geocache> GeocachesToRemove = new List<Geocache>();
				Parallel.ForEach(GeocachesNotAlreadyUsed, GC =>
			   {
				   float Distance = -1;
				   try
				   {
					   Distance = Coordinate.DistanceEstimateInMeter(new Coordinate(GC.lat, GC.lon), InitialRoute.Shape[i]);
				   }
				   catch (IndexOutOfRangeException e)
				   {
					   Log.AppendLine("IndexOutofRangeException in Resolving");
				   }

					if (Distance < (profile.MaxDistance * 1000 - CurrentRouteDistance) / 2 && Distance>0)
				   {
					   RouterPoint RouterPointOfGeocache = null;
					   Result<RouterPoint> ResolveResult = Router1.TryResolve(profile.ItineroProfile.profile, GC.lat, GC.lon);
					   if (!ResolveResult.IsError)
					   {
							RouterPointOfGeocache = ResolveResult.Value;
							InitialRouteGeocaches.Add(new KeyValueTriple<Geocache, float, RouterPoint>(GC, Distance, RouterPointOfGeocache));//Push the resoved location on
						}
					   GeocachesToRemove.Add(GC); //As it is either added to this segment or not reachable
					}
			   });

				GeocachesNotAlreadyUsed.RemoveAll(x=>GeocachesToRemove.Contains(x));
			}
			Log.AppendLine("Resolving took " + (DateTime.Now - ResolvingTime).TotalSeconds + " seconds");
			#endregion

			RoutingData.Add(new KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>(InitialRoute, InitialRouteGeocaches));

			#region Add Geocaches User selected to Include
			foreach (Geocache GeocacheToAdd in GeocachesToInclude)
			{
				int IndexOfRouteToInsertIn = -1;
				int IndexOfGeocacheToAdd = -1;
				KeyValueTriple<Geocache, float, RouterPoint> GeocacheToAdd_KVT = null;
				Route RouteToInsertIn = null;

				for (int i = 0; i < RoutingData.Count; i++)
				{
					KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>> route = RoutingData[i];

					for (int k = 0; k < route.Value.Count; k++)
					{
						KeyValueTriple<Geocache, float, RouterPoint> TestedGeocache = route.Value[k];

						if (GeocacheToAdd==TestedGeocache.Key) 
						{
							IndexOfRouteToInsertIn = i;
							IndexOfGeocacheToAdd = k;
							GeocacheToAdd_KVT = TestedGeocache;
							RouteToInsertIn = route.Key;
							goto IteratingDone;
						}
					}
				}

				IteratingDone:
				//TODO Multithread
				if (IndexOfGeocacheToAdd > 0)
				{
					Route NewPart1 = null;
					Result<Route> Result1 = Router1.TryCalculate(profile.ItineroProfile.profile, Router1.Resolve(profile.ItineroProfile.profile, RouteToInsertIn.Shape[0]), GeocacheToAdd_KVT.Value2);
					if (Result1.IsError)
					{
						Log.AppendLine("Route calculation with Geocache to include  failed.");
					}
					else
					{
						NewPart1 = Result1.Value;

						Route NewPart2 = null;
						Result<Route> Result2 = Router2.TryCalculate(profile.ItineroProfile.profile, GeocacheToAdd_KVT.Value2, Router2.Resolve(profile.ItineroProfile.profile, RouteToInsertIn.Shape[RouteToInsertIn.Shape.Length - 1]));
						if (Result2.IsError)
						{
							Log.AppendLine("Route calculation with Geocache to include  failed.");
						}
						else
						{
							NewPart2 = Result2.Value;
							// So one doesn't have to iterate through all routes
							CurrentRouteDistance -= RouteToInsertIn.TotalDistance;
							CurrentRouteDistance += NewPart1.TotalDistance;
							CurrentRouteDistance += NewPart2.TotalDistance;

							CurrentRouteTime -= RouteToInsertIn.TotalTime;
							CurrentRouteTime += NewPart1.TotalTime;
							CurrentRouteTime += NewPart2.TotalTime;

							InsertIntoRoute(profile, RoutingData, NewPart1, NewPart2, IndexOfRouteToInsertIn, CurrentRouteDistance);
							GeocachesOnRoute.Add(GeocacheToAdd);
						}
					}
				}
			}
			#endregion

			File.AppendAllText("Routerlog.txt", Log.ToString());

			//TODO Preselection Algorithm

			KeyValuePair<Route, List<Geocache>> Result = CalculateRouteToEnd(Router1, Router2, profile, RoutingData, GeocachesOnRoute, CurrentRouteDistance, CurrentRouteTime);

			Log = new StringBuilder();
			Log.AppendLine("Time after routing finished:" + DateTime.Now);
			Log.AppendLine("Routing took " + (DateTime.Now - StartTime).TotalSeconds + " seconds");
			File.AppendAllText("Routerlog.txt", Log.ToString());

			return Result;

		}

		#region Subroutines
		private static KeyValuePair<Route, List<Geocache>> CalculateRouteToEnd(Router router1, Router router2, Routingprofile profile, List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>> RoutingData, List<Geocache> GeocachesOnRoute, float CurrentRouteDistance, float CurrentRouteTime)
		{
			//ALWAYS keep RoutingDataList sorted by the way it came. It determines the direction of the route.

			//TODO Make MaxDistance to meters as well
			///////////////////////////////////////////////////////////////////////////////////////
			//TAKE CARE RouteDistance is in m, MaxDistance in km!!!
			////////////////////////////////////////////////////////////////////////////////

			StringBuilder Log = new StringBuilder();
			int iterationcounter = 0;
			TimeSpan RouteCalculationTime = TimeSpan.Zero;
			TimeSpan InsertingTime = TimeSpan.Zero;
			Log.AppendLine("Entered CalculateRouteToEnd");

			//Values
			KeyValueTriple<Geocache, float, RouterPoint> GeocacheToAdd = null;
			Route RouteToInsertIn = null;
			int IndexOfGeocacheToInsert = -1;
			int IndexOfRouteToInsertIn = -1;
			int GeocachesInRange = 0;
			float CurrentRoutePoints = 0;

			//Backup of these values
			KeyValueTriple<Geocache, float, RouterPoint> LastGeocacheToAdd = null;
			Route LastRouteToInsertIn = null;
			int LastIndexOfTouteToInsertIn = -1;
			int LastIndexOfRouteToInsertIn = -1;
			int LastGeocachesInRange = 0;
			float LastCurrentRouteDistance = 0;
			float LastCurrentRouteTime = 0;
			float LastCurrentRoutePoints = 0;

			do
			{
				iterationcounter++;

				//Backup Data in case the route gets worse
				LastGeocacheToAdd = GeocacheToAdd;
				LastRouteToInsertIn = RouteToInsertIn;
				LastIndexOfTouteToInsertIn = IndexOfGeocacheToInsert;
				LastIndexOfRouteToInsertIn = IndexOfRouteToInsertIn;
				LastGeocachesInRange = GeocachesInRange;
				LastCurrentRouteDistance = CurrentRouteDistance;
				LastCurrentRouteTime = CurrentRouteTime;

				//Reset values
				GeocacheToAdd = null;
				RouteToInsertIn = null;
				IndexOfGeocacheToInsert = 0;
				IndexOfRouteToInsertIn = 0;
				GeocachesInRange = 0;

				//Remove all Caches that are not in Range and meanwhile find best rated geocache. Necessary, as only Caches close to the latest added route are checked from before
				for (int i = 0; i < RoutingData.Count; i++)
				{
					KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>> route = RoutingData[i];

					for (int k = 0; k < route.Value.Count; k++)
					{
						KeyValueTriple<Geocache, float, RouterPoint> TestedGeocache = route.Value[k];

						if (TestedGeocache.Value1 > (profile.MaxDistance * 1000 - CurrentRouteDistance) / 2)    //Since you have to get forth and back... 
																												//UNDONE Check if this is really necessary
						{
							route.Value.Remove(TestedGeocache);
						}
						else
						{
							if (LastGeocacheToAdd == null || !GeocachesOnRoute.Contains(TestedGeocache.Key))
							{
								if (GeocacheToAdd == null)
								{
									GeocacheToAdd = TestedGeocache;
									RouteToInsertIn = route.Key;
									IndexOfGeocacheToInsert = k;
									IndexOfRouteToInsertIn = i;
								}
								else if (GeocacheToAdd.Key.Rating < TestedGeocache.Key.Rating)//Since this is the new best geocachce
								{
									GeocacheToAdd = TestedGeocache;
									RouteToInsertIn = route.Key;
									IndexOfGeocacheToInsert = k;
									IndexOfRouteToInsertIn = i;
								} else if (GeocacheToAdd == TestedGeocache && TestedGeocache.Value1 < GeocacheToAdd.Value1)//So the Cache is in reach of another partial route
								{
									GeocacheToAdd = TestedGeocache;
									RouteToInsertIn = route.Key;
									IndexOfGeocacheToInsert = k;
									IndexOfRouteToInsertIn = i;
								}
								GeocachesInRange++;
							}
						}
					}
				}

				if (GeocacheToAdd != null)
				{

					GeocachesOnRoute.Add(GeocacheToAdd.Key);
					RoutingData[IndexOfRouteToInsertIn].Value.RemoveAt(IndexOfGeocacheToInsert);// As it is no longer a new target

					DateTime RoutingStart = DateTime.Now;
					Route NewPart1 = null;
					Task<bool> Calculate1 = Task<bool>.Factory.StartNew(() =>
					{
						Result<Route> Result1 = router1.TryCalculate(profile.ItineroProfile.profile, router1.Resolve(profile.ItineroProfile.profile, RouteToInsertIn.Shape[0]), GeocacheToAdd.Value2);
						if (Result1.IsError)
						{
							return false;
						}
						else
						{
							NewPart1 = Result1.Value;
							return true;
						}
					});

					Route NewPart2 = null;
					Task<bool> Calculate2 = Task<bool>.Factory.StartNew(() =>
					{
						Result<Route> Result2 = router2.TryCalculate(profile.ItineroProfile.profile, GeocacheToAdd.Value2, router2.Resolve(profile.ItineroProfile.profile, RouteToInsertIn.Shape[RouteToInsertIn.Shape.Length - 1]));
						if (Result2.IsError)
						{
							return false;
						}
						else
						{
							NewPart2 = Result2.Value;
							return true;
						}
					});

					Task.WaitAll(Calculate1, Calculate2);
					RouteCalculationTime += DateTime.Now - RoutingStart;

					if (!(Calculate1.Result && Calculate2.Result))
					{
						Log.AppendLine("Route calculation failed.");
						//Reset
						GeocachesOnRoute.Remove(GeocacheToAdd.Key);//Thus the geocache that made the route fail
						GeocacheToAdd = LastGeocacheToAdd;//Unsure wether this is necessary
						RouteToInsertIn = LastRouteToInsertIn;//Same here
						CurrentRoutePoints = LastCurrentRoutePoints;//Reset the Route to the iteration before, but keep the Geocache that made it worse excluded
						CurrentRouteDistance = LastCurrentRouteDistance;
						CurrentRoutePoints = LastCurrentRoutePoints;
					}
					else
					{

						// So one doesn't have to iterate through all routes
						CurrentRouteDistance -= RouteToInsertIn.TotalDistance;
						CurrentRouteDistance += NewPart1.TotalDistance;
						CurrentRouteDistance += NewPart2.TotalDistance;

						CurrentRouteTime -= RouteToInsertIn.TotalTime;
						CurrentRouteTime += NewPart1.TotalTime;
						CurrentRouteTime += NewPart2.TotalTime;

						Log.AppendLine("New Route calculated.\nLength (Max Length) in km:" + CurrentRouteDistance / 1000 + " (" + profile.MaxDistance + ") \nTime (Max Time) in min: " + (CurrentRouteTime / 60 + GeocachesOnRoute.Count * profile.TimePerGeocache) + " (" + profile.MaxTime + ")\nGeocaches:" + GeocachesOnRoute.Count);

						CurrentRoutePoints += GeocacheToAdd.Key.Rating;

						if (CurrentRouteDistance > profile.MaxDistance * 1000)
						{
							CurrentRoutePoints -= (CurrentRouteDistance - profile.MaxDistance * 1000) * profile.PenaltyPerExtraKM / 1000;
						}
						if (CurrentRouteTime / 60 + GeocachesOnRoute.Count * profile.TimePerGeocache > profile.MaxTime)
						{
							CurrentRoutePoints -= (CurrentRouteTime / 60 - profile.MaxTime) * profile.PenaltyPerExtra10min / 10;
						}
						Log.AppendLine("New Points:" + CurrentRoutePoints + " Old Points:" + LastCurrentRoutePoints);

						if (LastCurrentRoutePoints <= CurrentRoutePoints)
						{
							DateTime Startinsertg = DateTime.Now;
							InsertIntoRoute(profile, RoutingData, NewPart1, NewPart2, IndexOfRouteToInsertIn, CurrentRouteDistance);
							InsertingTime += DateTime.Now - Startinsertg;
						}
						else
						{
							//Reset to Iteration before, except that the Geocache isn't added to the List Again

							GeocachesOnRoute.Remove(GeocacheToAdd.Key);//Thus the geocache that made the route worse
							GeocacheToAdd = LastGeocacheToAdd;//Unsure wether this is necessary
							RouteToInsertIn = LastRouteToInsertIn;//Same here
							CurrentRoutePoints = LastCurrentRoutePoints;//Reset the Route to the iteration before, but keep the Geocache that made it worse excluded
							CurrentRouteDistance = LastCurrentRouteDistance;
							CurrentRoutePoints = LastCurrentRoutePoints;

							Log.AppendLine("Had to remove latest added geocache as made the number of points of the route less. GCCode:" + GeocacheToAdd.Key.GCCODE);
						}
					}
				}
				Log.AppendLine("==========New Iteration==========");

			} while (GeocachesInRange - 1 > 0);//-1, as the one has been tried to be added to the Route. If th eif clause jumped in, it makes no differenc, as -1 is still less than 0

		
			Route FinalRoute = RoutingData[0].Key;

			for (int i = 1; i < RoutingData.Count; i++)
			{
				FinalRoute = FinalRoute.Concatenate(RoutingData[i].Key);
			}
			if (FinalRoute.TotalDistance >= CurrentRouteDistance + 5 || FinalRoute.TotalDistance <= CurrentRouteDistance - 5)//Rounding Tolerance
			{
				Log.AppendLine("WARNING: Final total distance doesn't match calculated total distance. Final:" + FinalRoute.TotalDistance + " Calculated: " + CurrentRouteDistance);
			}
			if (FinalRoute.TotalTime >= CurrentRouteTime + 5 || FinalRoute.TotalTime <= CurrentRouteTime - 5)//Rounding Tolerance
			{
				Log.AppendLine("WARNING: Final total Time doesn't match calculated total Time. Final:" + FinalRoute.TotalTime + " Calculated: " + CurrentRouteTime);
			}

			Log.AppendLine("Done after " + iterationcounter + " iterations");
			Log.AppendLine("Routing took " + RouteCalculationTime.TotalSeconds + "seconds");
			Log.AppendLine("Inserting took " + InsertingTime.TotalSeconds + "seconds");

			File.AppendAllText("Routerlog.txt", Log.ToString());

			return new KeyValuePair<Route, List<Geocache>>(FinalRoute, GeocachesOnRoute);
		}

		private static void InsertIntoRoute(Routingprofile profile, List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>> RoutingData, Route NewPart1, Route NewPart2, int IndexOfRouteToReplace, float CurrentRouteDistance)
		{
			List<KeyValueTriple<Geocache, float, RouterPoint>> NewPart1Geocaches = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
			List<KeyValueTriple<Geocache, float, RouterPoint>> GeocachesNotAlreadyUsedThread1 = new List<KeyValueTriple<Geocache, float, RouterPoint>>(RoutingData[IndexOfRouteToReplace].Value);
			List<KeyValueTriple<Geocache, float, RouterPoint>> NewPart2Geocaches = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
			List<KeyValueTriple<Geocache, float, RouterPoint>> GeocachesNotAlreadyUsedThread2 = new List<KeyValueTriple<Geocache, float, RouterPoint>>(RoutingData[IndexOfRouteToReplace].Value);
			Coordinate Startingpoint = RoutingData[IndexOfRouteToReplace].Key.Shape[0];
			Coordinate Endpoint = RoutingData[IndexOfRouteToReplace].Key.Shape[0];

			Task Thread1 = Task.Factory.StartNew(() =>
			{
				for (int i = 0; i < NewPart1.Shape.Length; i += Program.DB.EveryNthShapepoint)
				{

					foreach (KeyValueTriple<Geocache, float, RouterPoint> GC_KVT in new List<KeyValueTriple<Geocache, float, RouterPoint>>(GeocachesNotAlreadyUsedThread1))
					{
						float Distance = Coordinate.DistanceEstimateInMeter(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart1.Shape[i]);
						//float Distance = MyDistance(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart1.Shape[i]);//TODO Check which takes less processor time
						if (Distance < (profile.MaxDistance * 1000 - CurrentRouteDistance) / 2)
						{
							NewPart1Geocaches.Add(new KeyValueTriple<Geocache, float, RouterPoint>(GC_KVT.Key, Distance, GC_KVT.Value2));//Push the resolved location on
							
							GeocachesNotAlreadyUsedThread1.Remove(GC_KVT);//We no longer need to consider this one, as it is already adde
						}
					}
				}
			});

			Task Thread2 = Task.Factory.StartNew(() =>
			{

				for (int i = 0; i < NewPart2.Shape.Length; i += Program.DB.EveryNthShapepoint)
				{
					foreach (KeyValueTriple<Geocache, float, RouterPoint> GC_KVT in new List<KeyValueTriple<Geocache, float, RouterPoint>>(GeocachesNotAlreadyUsedThread2))
					{
						float Distance = Coordinate.DistanceEstimateInMeter(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart2.Shape[i]);
						//float Distance = MyDistance(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart2.Shape[i]);//TODO Check which takes less processor time
						if (Distance < (profile.MaxDistance * 1000 - CurrentRouteDistance) / 2)
						{
							NewPart2Geocaches.Add(new KeyValueTriple<Geocache, float, RouterPoint>(GC_KVT.Key, Distance, GC_KVT.Value2));
							GeocachesNotAlreadyUsedThread2.Remove(GC_KVT);//We no longer need to consider this one, as it is already added
						}
					}
				}
				
			});

			Task.WaitAll(Thread1, Thread2);
			//Put the new parts in place of the old part
			RoutingData.RemoveAt(IndexOfRouteToReplace);
			RoutingData.InsertRange(IndexOfRouteToReplace, new List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>>()
			{
				new KeyValuePair<Route, List<KeyValueTriple<Geocache, float,RouterPoint>>>(NewPart1, NewPart1Geocaches),
				new KeyValuePair<Route, List<KeyValueTriple<Geocache, float,RouterPoint>>>(NewPart2, NewPart2Geocaches)
			});

		}

		private static float MyDistance(Coordinate Coo1, Coordinate Coo2)
		{
			double Distance_lat = 111.3 * (Coo1.Latitude - Coo2.Latitude); //111.3km is the Approximate Distance between two latitudes
			double AverageLon_inRad = (Coo1.Latitude - Coo2.Latitude) / 2 * 0.0175;//0.0175 is pi/180
			double Distance_lon = 111.3 * Math.Cos(AverageLon_inRad) * (Coo1.Longitude - Coo2.Longitude);
			return (float)Math.Sqrt(Distance_lat * Distance_lat + Distance_lon * Distance_lon)*1000;
		}
		#endregion

		}
}
