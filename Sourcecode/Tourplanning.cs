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
			List<KeyValueTriple<Geocache, float, RouterPoint>> InitialRouteGeocachesInReach = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
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

			RoutingData.Add(new KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>(InitialRoute, InitialRouteGeocachesInReach));


			List<Geocache> GeocachesNotAlreadyUsed = new List<Geocache>(AllGeocaches);

			#region Add Geocaches User selected to Include
			foreach (Geocache GeocacheToAdd in GeocachesToInclude)
			{
				int IndexOfRouteToInsertIn = -1;
				KeyValueTriple<Geocache, float, RouterPoint> GeocacheToAdd_KVT = null;
				Route RouteToInsertIn = null;
				GeocachesNotAlreadyUsed.Remove(GeocacheToAdd);//As it is either added or not reachble

				//find which partial route it is closest to. Shouldm't tkae too long as there are only few partial routes at this point
				float MinDistance = -1;
				for (int i = 0; i < RoutingData.Count; i++)//Thus each partial route ?? Ther e is only one partial route ??
				{
					for (int k = 0; k < RoutingData[i].Key.Shape.Length; k += Program.DB.EveryNthShapepoint)
					{
						float Distance = Coordinate.DistanceEstimateInMeter(RoutingData[i].Key.Shape[k], new Coordinate(GeocacheToAdd.lat, GeocacheToAdd.lon));
						if (MinDistance < 0 || Distance < MinDistance)
						{
							IndexOfRouteToInsertIn = i;
							MinDistance = Distance;
						}
					}
				}

				RouteToInsertIn = RoutingData[IndexOfRouteToInsertIn].Key;

				Result<RouterPoint> GeocacheToAddResolveResult = Router1.TryResolve(profile.ItineroProfile.profile, GeocacheToAdd.lat, GeocacheToAdd.lon);
				if (!GeocacheToAddResolveResult.IsError)
				{
					GeocacheToAdd_KVT = new KeyValueTriple<Geocache, float, RouterPoint>(GeocacheToAdd, MinDistance, GeocacheToAddResolveResult.Value);

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

			if (Program.DB.RouterMode != RouterMode.On_the_go)
			{
				DateTime StartDirectionDecision = DateTime.Now;
				DirectionDecision(Router2, profile, RoutingData, GeocachesOnRoute, GeocachesNotAlreadyUsed, CurrentRouteDistance, CurrentRouteTime);
				Log.Append("Directiondecision took " + (DateTime.Now - StartDirectionDecision).TotalSeconds + " seconds");
			}
					
			#region Checking if geocaches are in reach and resolving all
			DateTime ResolvingTime = DateTime.Now;
			//NO Parallelisation of this part, since then synchronisation would be necessary to prevent Geocaches from appearing multiple times in the Range of the Initail route which consists only of one segment
			for (int h = 0; h < RoutingData.Count; h++)
			{
				//This way the Geocache can be added to multiple partial routes, but no multiple times to the same one
				Parallel.ForEach(GeocachesNotAlreadyUsed, GC =>
				{
					float minDistance = -1;

					for (int i = 0; i < RoutingData[h].Key.Shape.Length; i += Program.DB.EveryNthShapepoint)
					{
						
						float Distance = Coordinate.DistanceEstimateInMeter(new Coordinate(GC.lat, GC.lon), RoutingData[h].Key.Shape[i]);

						if (minDistance < 0 || Distance < minDistance)
						{
							minDistance = Distance;
						}
					}

					if (minDistance < (profile.MaxDistance * 1000 - CurrentRouteDistance) / 2 && minDistance > 0)
					{
						RouterPoint RouterPointOfGeocache = null;
						Result<RouterPoint> ResolveResult = Router1.TryResolve(profile.ItineroProfile.profile, GC.lat, GC.lon);
						if (!ResolveResult.IsError)
						{
							RouterPointOfGeocache = ResolveResult.Value;
							RouterPointOfGeocache = ResolveResult.Value;
							RoutingData[h].Value.Add(new KeyValueTriple<Geocache, float, RouterPoint>(GC, minDistance, RouterPointOfGeocache));//Push the resoved location on
						}
					}
				});
			}
			Log.AppendLine("Resolving took " + (DateTime.Now - ResolvingTime).TotalSeconds + " seconds");
			#endregion

			File.AppendAllText("Routerlog.txt", Log.ToString());

			KeyValuePair<Route, List<Geocache>> Result = CalculateRouteToEnd(Router1, Router2, profile, RoutingData, GeocachesOnRoute, CurrentRouteDistance, CurrentRouteTime);

			Log = new StringBuilder();
			Log.AppendLine("Time after routing finished:" + DateTime.Now);
			Log.AppendLine("Routing took " + (DateTime.Now - StartTime).TotalSeconds + " seconds");
			File.AppendAllText("Routerlog.txt", Log.ToString());

			return Result;

		}

		#region Subroutines

		private struct DirectionDesignInternalResult
		{
			public float ReachablePoints { get; set; }
			public int IndexOfRouteToReplace;
			public Geocache GeocacheToAdd { get; set; }
			public Route NewPart1 { get; set; }
			public Route NewPart2 { get; set; }
			public List<KeyValueTriple<Geocache, float, RouterPoint>> NewPart1Geocaches { get; set; }
			public List<KeyValueTriple<Geocache, float, RouterPoint>> NewPart2Geocaches { get; set; }
			public float TestRouteDistance { get; set; }
			public float TestRouteTime { get; set; }
		}

		private static void DirectionDecision(Router router, Routingprofile profile, List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>> RoutingData, List<Geocache> GeocachesOnRoute, List<Geocache> GeocachesNotAlreadyUsed,  float CurrentRouteDistance, float CurrentRouteTime)
		{
			GeocachesNotAlreadyUsed.OrderByDescending(x => x.Rating);
			List<Geocache> GeocachesToRemove = new List<Geocache>();

			List<DirectionDesignInternalResult> Results = new List<DirectionDesignInternalResult>();

			for(int i = 0; i<Program.DB.RoutefindingWidth; i++)
			{
				bool CacheInReachFound=false;

				while (!CacheInReachFound)
				{
					#region Find closest Route
					int IndexOfRouteToInsertIn = 0;
					float MinDistance = -1;
					for (int k = 0; k < RoutingData.Count; k++)//Thus each partial route
					{
						for (int m = 0; m < RoutingData[k].Key.Shape.Length; m += Program.DB.EveryNthShapepoint)
						{
							float Distance = Coordinate.DistanceEstimateInMeter(RoutingData[k].Key.Shape[m], new Coordinate(GeocachesNotAlreadyUsed[i].lat, GeocachesNotAlreadyUsed[i].lon));

							if (MinDistance < 0 || Distance < MinDistance)
							{
								IndexOfRouteToInsertIn = k;
								MinDistance = Distance;
							}
						}
					}
					#endregion

					//Check if the Cache is in Range
					//Divide by two here, as it is not the CalculateRouteToEnd and all should be kept that are possibly in range
					if (MinDistance < (profile.MaxDistance * 1000 - CurrentRouteDistance) / 2)
					{
						CacheInReachFound = true;

						Result<RouterPoint> GeocacheToAddResolveResult = router.TryResolve(profile.ItineroProfile.profile, GeocachesNotAlreadyUsed[i].lat, GeocachesNotAlreadyUsed[i].lon);
						Route RouteToInsertIn = RoutingData[IndexOfRouteToInsertIn].Key;

						KeyValueTriple<Geocache, float, RouterPoint> TargetGeocacheKVT = null;
						if (!GeocacheToAddResolveResult.IsError)
						{
							TargetGeocacheKVT = new KeyValueTriple<Geocache, float, RouterPoint>(GeocachesNotAlreadyUsed[i], MinDistance, GeocacheToAddResolveResult.Value);

							Route NewPart1 = null;
							Result<Route> Result1 = router.TryCalculate(profile.ItineroProfile.profile, router.Resolve(profile.ItineroProfile.profile, RouteToInsertIn.Shape[0]), TargetGeocacheKVT.Value2);
							if (!Result1.IsError)
							{
								NewPart1 = Result1.Value;

								Route NewPart2 = null;
								Result<Route> Result2 = router.TryCalculate(profile.ItineroProfile.profile, TargetGeocacheKVT.Value2, router.Resolve(profile.ItineroProfile.profile, RouteToInsertIn.Shape[RouteToInsertIn.Shape.Length - 1]));
								if (!Result2.IsError)
								{

									NewPart2 = Result2.Value;

									// So one doesn't have to iterate through all routes
									//TestRouteDistance, as one doesn't know wether this on is taken
									float TestRouteDistance = CurrentRouteDistance;
									TestRouteDistance -= RouteToInsertIn.TotalDistance;
									TestRouteDistance += NewPart1.TotalDistance;
									TestRouteDistance += NewPart2.TotalDistance;

									float TestRouteTime = CurrentRouteTime;
									TestRouteTime -= RouteToInsertIn.TotalTime;
									TestRouteTime += NewPart1.TotalTime;
									TestRouteTime += NewPart2.TotalTime;

									List<KeyValueTriple<Geocache, float, RouterPoint>> NewPart1Geocaches = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
									List<KeyValueTriple<Geocache, float, RouterPoint>> GeocachesNotAlreadyUsedThread1 = new List<KeyValueTriple<Geocache, float, RouterPoint>>(RoutingData[IndexOfRouteToInsertIn].Value);
									List<KeyValueTriple<Geocache, float, RouterPoint>> NewPart2Geocaches = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
									List<KeyValueTriple<Geocache, float, RouterPoint>> GeocachesNotAlreadyUsedThread2 = new List<KeyValueTriple<Geocache, float, RouterPoint>>(RoutingData[IndexOfRouteToInsertIn].Value);
									Coordinate Startingpoint = RouteToInsertIn.Shape[0];
									Coordinate Endpoint = RouteToInsertIn.Shape[0];

									Task Thread1 = Task.Factory.StartNew(() =>
									{
										for (int k = 0; k < NewPart1.Shape.Length; k += Program.DB.EveryNthShapepoint)
										{

											foreach (KeyValueTriple<Geocache, float, RouterPoint> GC_KVT in new List<KeyValueTriple<Geocache, float, RouterPoint>>(GeocachesNotAlreadyUsedThread1))
											{
												float Distance = Coordinate.DistanceEstimateInMeter(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart1.Shape[k]);
											//float Distance = MyDistance(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart1.Shape[i]);//TODO Check which takes less processor time
											if (Distance < (profile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)
												{
													NewPart1Geocaches.Add(new KeyValueTriple<Geocache, float, RouterPoint>(GC_KVT.Key, Distance, GC_KVT.Value2));//Push the resolved location on

												GeocachesNotAlreadyUsedThread1.Remove(GC_KVT);//We no longer need to consider this one, as it is already adde
											}
											}
										}
									});

									Task Thread2 = Task.Factory.StartNew(() =>
									{

										for (int k = 0; k < NewPart2.Shape.Length; k += Program.DB.EveryNthShapepoint)
										{
											foreach (KeyValueTriple<Geocache, float, RouterPoint> GC_KVT in new List<KeyValueTriple<Geocache, float, RouterPoint>>(GeocachesNotAlreadyUsedThread2))
											{
												float Distance = Coordinate.DistanceEstimateInMeter(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart2.Shape[k]);
											//float Distance = MyDistance(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart2.Shape[i]);//TODO Check which takes less processor time
											if (Distance < (profile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)
												{
													NewPart2Geocaches.Add(new KeyValueTriple<Geocache, float, RouterPoint>(GC_KVT.Key, Distance, GC_KVT.Value2));
													GeocachesNotAlreadyUsedThread2.Remove(GC_KVT);//We no longer need to consider this one, as it is already added
											}
											}
										}

									});

									Task.WaitAll(Thread1, Thread2);

									//UNDONE optional recursive call if Route stays too short

									float ReachablePoints = 0;
									foreach(KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>> Route in RoutingData)
									{
										if (Route.Key != RouteToInsertIn)
										{
											foreach(KeyValueTriple<Geocache, float, RouterPoint> KVP in Route.Value)
											{
												ReachablePoints += KVP.Key.Rating; //UNDONE find some better way
											}
										}
									}
									foreach(KeyValueTriple<Geocache, float, RouterPoint> KVP in NewPart1Geocaches)
									{
										ReachablePoints += KVP.Key.Rating;
									}
									foreach (KeyValueTriple<Geocache, float, RouterPoint> KVP in NewPart2Geocaches)
									{
										ReachablePoints += KVP.Key.Rating;
									}

									DirectionDesignInternalResult ThisResult = new DirectionDesignInternalResult();
									ThisResult.ReachablePoints = ReachablePoints;
									ThisResult.IndexOfRouteToReplace = IndexOfRouteToInsertIn;
									ThisResult.NewPart1 = NewPart1;
									ThisResult.NewPart1Geocaches = NewPart1Geocaches;
									ThisResult.NewPart2 = NewPart2;
									ThisResult.NewPart2Geocaches = NewPart2Geocaches;
									ThisResult.GeocacheToAdd = GeocachesNotAlreadyUsed[i];
									ThisResult.TestRouteDistance = TestRouteDistance;
									ThisResult.TestRouteTime = TestRouteTime;

									Results.Add(ThisResult);
								}//Resolving error of Item on Route shouldn't happen
							}
						}
						else //Couldn't resolve
						{
							GeocachesToRemove.Add(GeocachesNotAlreadyUsed[i]);
							i+=Program.DB.RoutefindingWidth+GeocachesToRemove.Count;//The next not processed Geocache
						}
					}
					else //The Cache is not in reach;
					{
						GeocachesToRemove.Add(GeocachesNotAlreadyUsed[i]);
						i += Program.DB.RoutefindingWidth + GeocachesToRemove.Count;//The next not processed Geocache
					}
				}
			}

			DirectionDesignInternalResult BestResult = Results.Find(x => x.ReachablePoints == Results.Max(y => y.ReachablePoints));
			RoutingData.RemoveAt(BestResult.IndexOfRouteToReplace);
			RoutingData.InsertRange(BestResult.IndexOfRouteToReplace, new List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>>()
			{
				new KeyValuePair<Route, List<KeyValueTriple<Geocache, float,RouterPoint>>>(BestResult.NewPart1, BestResult.NewPart1Geocaches),
				new KeyValuePair<Route, List<KeyValueTriple<Geocache, float,RouterPoint>>>(BestResult.NewPart2, BestResult.NewPart2Geocaches)
			});
			GeocachesOnRoute.Add(BestResult.GeocacheToAdd);
			CurrentRouteDistance = BestResult.TestRouteDistance;
			CurrentRouteTime = BestResult.TestRouteTime;

			GeocachesNotAlreadyUsed.Remove(BestResult.GeocacheToAdd);
			foreach(Geocache GC in GeocachesToRemove)
			{
				GeocachesNotAlreadyUsed.Remove(GC);
			}

		}

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

						if (TestedGeocache.Value1 > (profile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)   
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

			} while (GeocachesInRange - 1 > 0);//-1, as the one has been tried to be added to the Route. If the if clause jumped in, it makes no differenc, as -1 is still less than 0

		
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
						if (Distance < (profile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)
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
						if (Distance < (profile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)
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
