using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Itinero;
using Itinero.LocalGeo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeocachingTourPlanner
{
	class Tourplanning
	{
		StringBuilder Log = new StringBuilder();

		Router Router1 = null;
		Router Router2 = null;

		List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>> RoutingData = new List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>>();
		List<Geocache> GeocachesOnRoute = new List<Geocache>();

		RouterPoint Startpoint_RP;
		RouterPoint Endpoint_RP;
		Route InitialRoute;
		Routingprofile SelectedProfile = new Routingprofile();
		List<KeyValueTriple<Geocache, float, RouterPoint>> InitialRouteGeocachesInReach = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
		List<Geocache> GeocachesNotAlreadyUsed = new List<Geocache>();
		float CurrentRouteDistance;
		float CurrentRouteTime;
		float CurrentRoutePoints;

		/// <summary>
		/// Returns null if calculation fails
		/// </summary>
		/// <param name="router"></param>
		/// <param name="profile"></param>
		/// <param name=""></param>
		/// <returns></returns>
		public void GetRoute(Routingprofile profile, List<Geocache> AllGeocaches, Coordinate Startpoint, Coordinate Endpoint, List<Geocache> GeocachesToInclude)
		{
			DateTime StartTime = DateTime.Now;
			Log.AppendLine("\n  ==========New Route Calculation========== \n");
			Log.AppendLine("Current Time:" + StartTime);
			Log.AppendLine("Profile: " + profile.Name);

			SelectedProfile = profile;

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
					return;
				}
			}
			//One for every thread
			Router1 = new Router(Program.RouterDB);
			Router2 = new Router(Program.RouterDB);

			#endregion

			try
			{
				Startpoint_RP = Router1.Resolve(SelectedProfile.ItineroProfile.profile, Startpoint);
			}
			catch (Itinero.Exceptions.ResolveFailedException)
			{
				MessageBox.Show("Please select a Startingpoint close to a road");
				Application.UseWaitCursor = false;
				return;
			}
			try
			{
				Endpoint_RP = Router1.Resolve(SelectedProfile.ItineroProfile.profile, Endpoint);
			}
			catch (Itinero.Exceptions.ResolveFailedException)
			{
				MessageBox.Show("Please select an Endpoint close to a road");
				Application.UseWaitCursor = false;
				return;
			}

			//Calculate initial Route
			try
			{
				InitialRoute = Router1.Calculate(SelectedProfile.ItineroProfile.profile, Startpoint_RP, Endpoint_RP);
			}
			catch (Itinero.Exceptions.RouteNotFoundException)
			{
				MessageBox.Show("Can't calculate a route between start and endpoint. Please change your selection");
				Application.UseWaitCursor = false;
				return;
			}
			CurrentRouteDistance = InitialRoute.TotalDistance;
			CurrentRouteTime = InitialRoute.TotalTime;

			RoutingData.Add(new KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>(InitialRoute, InitialRouteGeocachesInReach));


			GeocachesNotAlreadyUsed = new List<Geocache>(AllGeocaches);
			
			//Program.MainWindow.DisplayPreliminaryRoute(RoutingData);

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

				Result<RouterPoint> GeocacheToAddResolveResult = Router1.TryResolve(SelectedProfile.ItineroProfile.profile, GeocacheToAdd.lat, GeocacheToAdd.lon);
				if (!GeocacheToAddResolveResult.IsError)
				{
					GeocacheToAdd_KVT = new KeyValueTriple<Geocache, float, RouterPoint>(GeocacheToAdd, MinDistance, GeocacheToAddResolveResult.Value);

					Route NewPart1 = null;
					Result<Route> Result1 = Router1.TryCalculate(SelectedProfile.ItineroProfile.profile, Router1.Resolve(SelectedProfile.ItineroProfile.profile, RouteToInsertIn.Shape[0]), GeocacheToAdd_KVT.Value2);
					if (Result1.IsError)
					{
						Log.AppendLine("Route calculation with Geocache to include  failed.");
					}
					else
					{
						NewPart1 = Result1.Value;

						Route NewPart2 = null;
						Result<Route> Result2 = Router2.TryCalculate(SelectedProfile.ItineroProfile.profile, GeocacheToAdd_KVT.Value2, Router2.Resolve(SelectedProfile.ItineroProfile.profile, RouteToInsertIn.Shape[RouteToInsertIn.Shape.Length - 1]));
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

							CurrentRoutePoints += GeocacheToAdd.Rating;

							InsertIntoRoute(NewPart1, NewPart2, IndexOfRouteToInsertIn);
							GeocachesOnRoute.Add(GeocacheToAdd);
						}

					}
				}
			}
			#endregion

			if (Program.DB.RouterMode != RouterMode.On_the_go)
			{
				DateTime StartDirectionDecision = DateTime.Now;
				DirectionDecision();
				Log.AppendLine("Directiondecision took " + (DateTime.Now - StartDirectionDecision).TotalSeconds + " seconds");

				//Program.MainWindow.DisplayPreliminaryRoute(RoutingData);
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

					if (minDistance < (SelectedProfile.MaxDistance * 1000 - CurrentRouteDistance) / 2 && minDistance > 0)
					{
						RouterPoint RouterPointOfGeocache = null;
						Result<RouterPoint> ResolveResult = Router1.TryResolve(SelectedProfile.ItineroProfile.profile, GC.lat, GC.lon);
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

			KeyValuePair<Route, List<Geocache>> Result = CalculateRouteToEnd();

			Log = new StringBuilder();
			Log.AppendLine("Time after routing finished:" + DateTime.Now);
			Log.AppendLine("Route calculation took " + (DateTime.Now - StartTime).TotalSeconds + " seconds");
			File.AppendAllText("Routerlog.txt", Log.ToString());
			
			Program.MainWindow.AddFinalRoute(Result, profile);
			Application.UseWaitCursor = false;


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

		private void DirectionDecision()
		{
			GeocachesNotAlreadyUsed.OrderByDescending(x => x.Rating);
			List<Geocache> GeocachesToRemove = new List<Geocache>();

			List<DirectionDesignInternalResult> Results = new List<DirectionDesignInternalResult>();

			for(int GlobalTargetIndex = 0; GlobalTargetIndex<Program.DB.RoutefindingWidth; GlobalTargetIndex++)
			{
				bool CacheInReachFound=false;
				int LocalTargetIndex = GlobalTargetIndex;

				while (!CacheInReachFound)
				{
					#region Find closest Route
					int IndexOfRouteToInsertIn = 0;
					float MinDistance = -1;
					for (int k = 0; k < RoutingData.Count; k++)//Thus each partial route
					{
						for (int m = 0; m < RoutingData[k].Key.Shape.Length; m += Program.DB.EveryNthShapepoint)
						{
							float Distance = Coordinate.DistanceEstimateInMeter(RoutingData[k].Key.Shape[m], new Coordinate(GeocachesNotAlreadyUsed[LocalTargetIndex].lat, GeocachesNotAlreadyUsed[LocalTargetIndex].lon));

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
					if (MinDistance < (SelectedProfile.MaxDistance * 1000 - CurrentRouteDistance) / 2)
					{
						CacheInReachFound = true;

						Result<RouterPoint> GeocacheToAddResolveResult = Router1.TryResolve(SelectedProfile.ItineroProfile.profile, GeocachesNotAlreadyUsed[LocalTargetIndex].lat, GeocachesNotAlreadyUsed[LocalTargetIndex].lon);
						Route RouteToInsertIn = RoutingData[IndexOfRouteToInsertIn].Key;

						KeyValueTriple<Geocache, float, RouterPoint> TargetGeocacheKVT = null;
						if (!GeocacheToAddResolveResult.IsError)
						{
							TargetGeocacheKVT = new KeyValueTriple<Geocache, float, RouterPoint>(GeocachesNotAlreadyUsed[LocalTargetIndex], MinDistance, GeocacheToAddResolveResult.Value);

							Route NewPart1 = null;
							Result<Route> Result1 = Router1.TryCalculate(SelectedProfile.ItineroProfile.profile, Router1.Resolve(SelectedProfile.ItineroProfile.profile, RouteToInsertIn.Shape[0]), TargetGeocacheKVT.Value2);
							if (!Result1.IsError)
							{
								NewPart1 = Result1.Value;

								Route NewPart2 = null;
								Result<Route> Result2 = Router2.TryCalculate(SelectedProfile.ItineroProfile.profile, TargetGeocacheKVT.Value2, Router2.Resolve(SelectedProfile.ItineroProfile.profile, RouteToInsertIn.Shape[RouteToInsertIn.Shape.Length - 1]));
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

									#region Find geocaches in reach
									//Calculate the restrictive way, as only they will be considered anyways
									List<KeyValueTriple<Geocache, float, RouterPoint>> NewPart1Geocaches = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
									List<KeyValueTriple<Geocache, float, RouterPoint>> GeocachesNotAlreadyUsedThread1 = new List<KeyValueTriple<Geocache, float, RouterPoint>>(RoutingData[IndexOfRouteToInsertIn].Value);
									List<KeyValueTriple<Geocache, float, RouterPoint>> NewPart2Geocaches = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
									List<KeyValueTriple<Geocache, float, RouterPoint>> GeocachesNotAlreadyUsedThread2 = new List<KeyValueTriple<Geocache, float, RouterPoint>>(RoutingData[IndexOfRouteToInsertIn].Value);
									Coordinate Startingpoint = RouteToInsertIn.Shape[0];
									Coordinate Endpoint = RouteToInsertIn.Shape[0];

									Thread Thread1 = new Thread(new ThreadStart(() =>
									{
										foreach (KeyValueTriple<Geocache, float, RouterPoint> GC_KVT in new List<KeyValueTriple<Geocache, float, RouterPoint>>(GeocachesNotAlreadyUsedThread1))
										{
											float minDistance = -1;

											for (int k = 0; k < NewPart1.Shape.Length; k += Program.DB.EveryNthShapepoint)
											{

												float Distance = Coordinate.DistanceEstimateInMeter(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart1.Shape[k]);

												if (minDistance < 0 || Distance < minDistance)
												{
													minDistance = Distance;
												}
											}

											if (minDistance < (SelectedProfile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)
											{
												NewPart1Geocaches.Add(new KeyValueTriple<Geocache, float, RouterPoint>(GC_KVT.Key, minDistance, GC_KVT.Value2));//Push the resolved location on

												GeocachesNotAlreadyUsedThread1.Remove(GC_KVT);//We no longer need to consider this one, as it is already adde
											}
										}
									}));

									Thread Thread2 = new Thread(new ThreadStart(() =>
									{

										foreach (KeyValueTriple<Geocache, float, RouterPoint> GC_KVT in new List<KeyValueTriple<Geocache, float, RouterPoint>>(GeocachesNotAlreadyUsedThread2))
										{
											float minDistance = -1;

											for (int k = 0; k < NewPart2.Shape.Length; k += Program.DB.EveryNthShapepoint)
											{

												float Distance = Coordinate.DistanceEstimateInMeter(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart1.Shape[k]);

												if (minDistance < 0 || Distance < minDistance)
												{
													minDistance = Distance;
												}
											}

											if (minDistance < (SelectedProfile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)
											{
												NewPart2Geocaches.Add(new KeyValueTriple<Geocache, float, RouterPoint>(GC_KVT.Key, minDistance, GC_KVT.Value2));//Push the resolved location on

												GeocachesNotAlreadyUsedThread1.Remove(GC_KVT);//We no longer need to consider this one, as it is already adde
											}
										}

									}));

									Thread1.Start();
									Thread2.Start();

									Thread1.Join();
									Thread2.Join();
									#endregion
									//UNDONE optional recursive call if Route stays too short

									float ReachablePoints = 0;
									foreach(KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>> Route in RoutingData)
									{
										if (Route.Key != RouteToInsertIn)
										{
											foreach(KeyValueTriple<Geocache, float, RouterPoint> KVP in Route.Value)
											{
												if(KVP.Value1<(SelectedProfile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)//thus it is still in reach after the calculation
												{
													ReachablePoints += KVP.Key.Rating;
												}
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

									if (ReachablePoints != 0)
									{


										DirectionDesignInternalResult ThisResult = new DirectionDesignInternalResult();
										ThisResult.ReachablePoints = ReachablePoints;
										ThisResult.IndexOfRouteToReplace = IndexOfRouteToInsertIn;
										ThisResult.NewPart1 = NewPart1;
										ThisResult.NewPart1Geocaches = NewPart1Geocaches;
										ThisResult.NewPart2 = NewPart2;
										ThisResult.NewPart2Geocaches = NewPart2Geocaches;
										ThisResult.GeocacheToAdd = GeocachesNotAlreadyUsed[LocalTargetIndex];
										ThisResult.TestRouteDistance = TestRouteDistance;
										ThisResult.TestRouteTime = TestRouteTime;

										Results.Add(ThisResult);
									}
									else//No Targets in reach
									{
										GeocachesToRemove.Add(GeocachesNotAlreadyUsed[LocalTargetIndex]);
										LocalTargetIndex = Program.DB.RoutefindingWidth + GeocachesToRemove.Count;//The next not processed Geocache
									}
								}//Resolving error of Item on Route shouldn't happen
							}
						}
						else //Couldn't resolve
						{
							GeocachesToRemove.Add(GeocachesNotAlreadyUsed[LocalTargetIndex]);
							LocalTargetIndex = Program.DB.RoutefindingWidth + GeocachesToRemove.Count;//The next not processed Geocache
						}
					}
					else //The Cache is not in reach;
					{
						GeocachesToRemove.Add(GeocachesNotAlreadyUsed[LocalTargetIndex]);
						LocalTargetIndex = Program.DB.RoutefindingWidth + GeocachesToRemove.Count;//The next not processed Geocache
					}
				}
			}
			if (Results.Count != 0)//Otherwise just let the final routing algorithm do its job
			{
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
				CurrentRoutePoints += BestResult.GeocacheToAdd.Rating;
				File.AppendAllText("Routerlog.txt", "Chose " + BestResult.GeocacheToAdd + "\n");
				GeocachesNotAlreadyUsed.Remove(BestResult.GeocacheToAdd);
				foreach (Geocache GC in GeocachesToRemove)
				{
					GeocachesNotAlreadyUsed.Remove(GC);
				}
			}
		}

		private KeyValuePair<Route, List<Geocache>> CalculateRouteToEnd()
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
				Log.AppendLine("==========New Iteration==========");
				iterationcounter++;

				//Backup Data in case the route gets worse
				LastGeocacheToAdd = GeocacheToAdd;
				LastRouteToInsertIn = RouteToInsertIn;
				LastIndexOfTouteToInsertIn = IndexOfGeocacheToInsert;
				LastIndexOfRouteToInsertIn = IndexOfRouteToInsertIn;
				LastGeocachesInRange = GeocachesInRange;
				LastCurrentRouteDistance = CurrentRouteDistance;
				LastCurrentRouteTime = CurrentRouteTime;
				LastCurrentRoutePoints = CurrentRoutePoints;

				//Reset values
				GeocacheToAdd = null;
				RouteToInsertIn = null;
				IndexOfGeocacheToInsert = 0;
				IndexOfRouteToInsertIn = 0;
				GeocachesInRange = 0;

				//Remove all Caches that are not in Range (broad criterium) and meanwhile find best rated geocache (narrow distance criterium). 
				//Necessary, as only Caches close to the latest added route are checked from before
				//Differentiation between narrow and wide criterum in order to make sure that a parallel equally long route is more likely to still be
				for (int i = 0; i < RoutingData.Count; i++)
				{
					KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>> route = RoutingData[i];

					for (int k = 0; k < route.Value.Count; k++)
					{
						KeyValueTriple<Geocache, float, RouterPoint> TestedGeocache = route.Value[k];

						if (TestedGeocache.Value1 < (SelectedProfile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)
						{
							if (LastGeocacheToAdd == null || !GeocachesOnRoute.Contains(TestedGeocache.Key))//Just skip them, less complicated than removing
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
						}else if (TestedGeocache.Value1 > (SelectedProfile.MaxDistance * 1000 - CurrentRouteDistance) / 2 + 0.1 * SelectedProfile.MaxDistance * 1000) //Still a bad solution, but better than risking to exclude to many caches completely
						{
							route.Value.Remove(TestedGeocache);
						}
					}
				}

				if (GeocacheToAdd != null)
				{
					Log.AppendLine("Added: " + GeocacheToAdd.Key.GCCODE);

					GeocachesOnRoute.Add(GeocacheToAdd.Key);
					RoutingData[IndexOfRouteToInsertIn].Value.RemoveAt(IndexOfGeocacheToInsert);// As it is no longer a new target

					DateTime RoutingStart = DateTime.Now;
					Route NewPart1 = null;
					Thread Calculate1 = new Thread(new ThreadStart( () =>
					{
						Result<Route> Result1 = Router1.TryCalculate(SelectedProfile.ItineroProfile.profile, Router1.Resolve(SelectedProfile.ItineroProfile.profile, RouteToInsertIn.Shape[0]), GeocacheToAdd.Value2);
						if (!Result1.IsError)
						{
							NewPart1 = Result1.Value;
						}
					}));

					Route NewPart2 = null;
					Thread Calculate2 = new Thread(new ThreadStart(() =>
					{
						Result<Route> Result2 = Router2.TryCalculate(SelectedProfile.ItineroProfile.profile, GeocacheToAdd.Value2, Router2.Resolve(SelectedProfile.ItineroProfile.profile, RouteToInsertIn.Shape[RouteToInsertIn.Shape.Length - 1]));
						if (!Result2.IsError)
						{
							NewPart2 = Result2.Value;
						}
					}));

					Calculate1.Start();
					Calculate2.Start();

					Calculate1.Join();
					Calculate2.Join();

					RouteCalculationTime += DateTime.Now - RoutingStart;
									

					if (NewPart1==null || NewPart2==null)
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

						Log.AppendLine("New Route calculated.\nLength (Max Length) (Old Length) in km:" + CurrentRouteDistance / 1000 + " (" + SelectedProfile.MaxDistance + ") (" + LastCurrentRouteDistance / 1000 + ")");
						Log.AppendLine("Time(Max Time) in min: " + (CurrentRouteTime / 60 + GeocachesOnRoute.Count * SelectedProfile.TimePerGeocache) + "(" + SelectedProfile.MaxTime + ") " + "(" + (LastCurrentRouteTime / 60 + (GeocachesOnRoute.Count - 1) * SelectedProfile.TimePerGeocache) + ")\nGeocaches: " + GeocachesOnRoute.Count);

						CurrentRoutePoints += GeocacheToAdd.Key.Rating;

						if (CurrentRouteDistance > SelectedProfile.MaxDistance * 1000)
						{
							CurrentRoutePoints -= (CurrentRouteDistance - SelectedProfile.MaxDistance * 1000) * SelectedProfile.PenaltyPerExtraKM / 1000;
						}
						if (CurrentRouteTime / 60 + GeocachesOnRoute.Count * SelectedProfile.TimePerGeocache > SelectedProfile.MaxTime)
						{
							CurrentRoutePoints -= (CurrentRouteTime / 60 - SelectedProfile.MaxTime) * SelectedProfile.PenaltyPerExtra10min / 10;
						}
						Log.AppendLine("New Points:" + CurrentRoutePoints + " Old Points:" + LastCurrentRoutePoints);

						if (LastCurrentRoutePoints <= CurrentRoutePoints)
						{
							DateTime Startinsertg = DateTime.Now;
							InsertIntoRoute(NewPart1, NewPart2, IndexOfRouteToInsertIn);
							InsertingTime += DateTime.Now - Startinsertg;
						}
						else
						{
							//Reset to Iteration before, except that the Geocache isn't added to the List Again

							Log.AppendLine("Had to remove latest added geocache as made the number of points of the route less. GCCode:" + GeocacheToAdd.Key.GCCODE);

							GeocachesOnRoute.Remove(GeocacheToAdd.Key);//Thus the geocache that made the route worse
							GeocacheToAdd = LastGeocacheToAdd;//Unsure wether this is necessary
							RouteToInsertIn = LastRouteToInsertIn;//Same here
							CurrentRoutePoints = LastCurrentRoutePoints;//Reset the Route to the iteration before, but keep the Geocache that made it worse excluded
							CurrentRouteDistance = LastCurrentRouteDistance;
							CurrentRouteTime = LastCurrentRouteTime;
						}
					}
				}

				new Thread(new ThreadStart(() =>
				{
					Program.MainWindow.DisplayPreliminaryRoute(RoutingData);
				})).Start();

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

		private void InsertIntoRoute(Route NewPart1, Route NewPart2, int IndexOfRouteToReplace)
		{
			List<KeyValueTriple<Geocache, float, RouterPoint>> NewPart1Geocaches = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
			List<KeyValueTriple<Geocache, float, RouterPoint>> GeocachesNotAlreadyUsedThread1 = new List<KeyValueTriple<Geocache, float, RouterPoint>>(RoutingData[IndexOfRouteToReplace].Value);
			List<KeyValueTriple<Geocache, float, RouterPoint>> NewPart2Geocaches = new List<KeyValueTriple<Geocache, float, RouterPoint>>();
			List<KeyValueTriple<Geocache, float, RouterPoint>> GeocachesNotAlreadyUsedThread2 = new List<KeyValueTriple<Geocache, float, RouterPoint>>(RoutingData[IndexOfRouteToReplace].Value);
			Coordinate Startingpoint = RoutingData[IndexOfRouteToReplace].Key.Shape[0];
			Coordinate Endpoint = RoutingData[IndexOfRouteToReplace].Key.Shape[0];

			Thread Thread1 = new Thread(new ThreadStart(() =>
			{
				foreach (KeyValueTriple<Geocache, float, RouterPoint> GC_KVT in new List<KeyValueTriple<Geocache, float, RouterPoint>>(GeocachesNotAlreadyUsedThread1))
				{
					float minDistance = -1;

					for (int i = 0; i < NewPart1.Shape.Length; i += Program.DB.EveryNthShapepoint)
					{

						float Distance = Coordinate.DistanceEstimateInMeter(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart1.Shape[i]);

						if (minDistance < 0 || Distance < minDistance)
						{
							minDistance = Distance;
						}
					}

					if (minDistance < (SelectedProfile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)
					{
						NewPart1Geocaches.Add(new KeyValueTriple<Geocache, float, RouterPoint>(GC_KVT.Key, minDistance, GC_KVT.Value2));//Push the resolved location on
							
						GeocachesNotAlreadyUsedThread1.Remove(GC_KVT);//We no longer need to consider this one, as it is already adde
					}
				}
			}));

			Thread Thread2 = new Thread(new ThreadStart(() =>
			{
				foreach (KeyValueTriple<Geocache, float, RouterPoint> GC_KVT in new List<KeyValueTriple<Geocache, float, RouterPoint>>(GeocachesNotAlreadyUsedThread1))
				{
					float minDistance = -1;

					for (int i = 0; i < NewPart2.Shape.Length; i += Program.DB.EveryNthShapepoint)
					{

						float Distance = Coordinate.DistanceEstimateInMeter(new Coordinate(GC_KVT.Key.lat, GC_KVT.Key.lon), NewPart2.Shape[i]);

						if (minDistance < 0 || Distance < minDistance)
						{
							minDistance = Distance;
						}
					}

					if (minDistance < (SelectedProfile.MaxDistance * 1000 - CurrentRouteDistance) / Program.DB.Divisor + Program.DB.Tolerance)
					{
						NewPart1Geocaches.Add(new KeyValueTriple<Geocache, float, RouterPoint>(GC_KVT.Key, minDistance, GC_KVT.Value2));//Push the resolved location on

						GeocachesNotAlreadyUsedThread2.Remove(GC_KVT);//We no longer need to consider this one, as it is already adde
					}
				}

			}));

			Thread1.Start();
			Thread2.Start();

			Thread1.Join();
			Thread2.Join();

			//Put the new parts in place of the old part
			RoutingData.RemoveAt(IndexOfRouteToReplace);
			RoutingData.InsertRange(IndexOfRouteToReplace, new List<KeyValuePair<Route, List<KeyValueTriple<Geocache, float, RouterPoint>>>>()
			{
				new KeyValuePair<Route, List<KeyValueTriple<Geocache, float,RouterPoint>>>(NewPart1, NewPart1Geocaches),
				new KeyValuePair<Route, List<KeyValueTriple<Geocache, float,RouterPoint>>>(NewPart2, NewPart2Geocaches)
			});

			//Program.MainWindow.DisplayPreliminaryRoute(RoutingData);

		}
		
		#endregion
		
	}
}
